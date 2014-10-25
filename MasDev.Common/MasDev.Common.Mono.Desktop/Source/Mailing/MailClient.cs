using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Net.Security;


namespace MasDev.Common.Mono
{
	public static class MailClient
	{
	
		public static void Send (string to, string subject, string body, string username, string password, string alias)
		{
			var mail = new MailMessage ();

			mail.From = new MailAddress (username, alias);
			mail.To.Add (to);
			mail.Subject = subject;
			mail.Body = body;

			var smtpServer = new SmtpClient ("smtp.mandrillapp.com");
			smtpServer.Port = 587;
			smtpServer.Credentials = new NetworkCredential (username, password);
			smtpServer.EnableSsl = true;
			ServicePointManager.ServerCertificateValidationCallback = 
				(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;


			smtpServer.Send (mail);
		}
			
	}
}

