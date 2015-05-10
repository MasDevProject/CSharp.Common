using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Linq;
using System.Security;


namespace MasDev.Owin.Auth
{
	public interface IAccessTokenProtector
	{
		byte[] Protect (string serializedAccessToken);

		string Unprotect (byte[] protectedAccessToken);
	}

	public class DefaultAccessTokenProtector : IAccessTokenProtector
	{
		readonly byte[] _key;

		public DefaultAccessTokenProtector (string password)
		{
			using (var sha1 = new SHA256Managed ()) {
				_key = sha1.ComputeHash (Encoding.UTF8.GetBytes (password));
			}
		}


		public byte[] Protect (string serializedAccessToken)
		{
			var data = Encoding.UTF8.GetBytes (serializedAccessToken);
			byte[] dataHash;
			using (var sha = new SHA256Managed ()) {
				dataHash = sha.ComputeHash (data);
			}

			using (var aesAlg = new AesManaged ()) {
				aesAlg.Key = _key;
				aesAlg.GenerateIV ();

				using (var encryptor = aesAlg.CreateEncryptor (aesAlg.Key, aesAlg.IV))
				using (var msEncrypt = new MemoryStream ()) {
					msEncrypt.Write (aesAlg.IV, 0, 16);

					using (var csEncrypt = new CryptoStream (msEncrypt, encryptor, CryptoStreamMode.Write))
					using (var bwEncrypt = new BinaryWriter (csEncrypt)) {
						bwEncrypt.Write (dataHash);
						bwEncrypt.Write (data.Length);
						bwEncrypt.Write (data);
					}
					var protectedData = msEncrypt.ToArray ();
					return protectedData;
				}
			}
		}

		public string Unprotect (byte[] protectedAccessToken)
		{
			using (var aesAlg = new AesManaged ()) {
				aesAlg.Key = this._key;

				using (var msDecrypt = new MemoryStream (protectedAccessToken)) {
					byte[] iv = new byte[16];
					msDecrypt.Read (iv, 0, 16);

					aesAlg.IV = iv;

					using (var decryptor = aesAlg.CreateDecryptor (aesAlg.Key, aesAlg.IV))
					using (var csDecrypt = new CryptoStream (msDecrypt, decryptor, CryptoStreamMode.Read))
					using (var brDecrypt = new BinaryReader (csDecrypt)) {
						var signature = brDecrypt.ReadBytes (32);
						var len = brDecrypt.ReadInt32 ();
						var data = brDecrypt.ReadBytes (len);

						byte[] dataHash;
						using (var sha = new SHA256Managed ()) {
							dataHash = sha.ComputeHash (data);
						}

						if (!dataHash.SequenceEqual (signature))
							throw new SecurityException ("Signature does not match the computed hash");

						return Encoding.UTF8.GetString (data);
					}
				}
			}
		}
	}
}

