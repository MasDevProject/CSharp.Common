using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Net.Security;


namespace MasDev.Common.Mono
{
	public static class MailClient
	{
	
		public static void SendViaGmail (string to, string subject, string body, string username, string password, string senderEmail, string senderAlias)
		{
			var mail = new MailMessage ();
			mail.From = new MailAddress (senderEmail, senderAlias);
			mail.To.Add (to);
			mail.Subject = subject;
			mail.Body = body;

			var client = new SmtpClient ("smtp.gmail.com");
			client.Port = 587;
			client.Credentials = new NetworkCredential (username, password);
			client.EnableSsl = true;

			ServicePointManager.ServerCertificateValidationCallback = delegate {
				return true;
			};
			client.Send (mail);
		}
	}
}

