using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;


namespace MasDev.Common.Http
{
	public static class Https
	{

		static byte[] SelfSignedCertificateHash;



		public static void AcceptSelfSignedCertificate (byte[] selfSignedCertificateHash)
		{
			SelfSignedCertificateHash = selfSignedCertificateHash;
			ServicePointManager.ServerCertificateValidationCallback =
				ValidateServerCertficate;
		}



		public static void AcceptAnyCertificates ()
		{
			ServicePointManager.ServerCertificateValidationCallback =
				ValidateAnyServerCertficate;
		}



		static bool ValidateAnyServerCertficate (object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			return true;
		}



		static bool ValidateServerCertficate (object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			if (sslPolicyErrors == SslPolicyErrors.None)
			{
				return true;
			}
				
			bool certMatch = false;
			byte[] certHash = cert.GetCertHash ();

			if (certHash.Length == SelfSignedCertificateHash.Length)
			{
				certMatch = true;
				for (int idx = 0; idx < certHash.Length; idx++)
				{
					if (certHash [idx] != SelfSignedCertificateHash [idx])
					{
						certMatch = false;
						break;
					}
				}
			}
			return certMatch;
		}
	}
}

