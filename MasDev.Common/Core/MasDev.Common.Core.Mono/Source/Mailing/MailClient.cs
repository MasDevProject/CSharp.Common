using System.Net.Mail;
using System.Net;

namespace MasDev.Mono
{
	public static class MailClient
	{
		public static void SendViaGmail (string to, string subject, string body, string username, string password, string senderEmail, string senderAlias, bool htmlBody)
		{
			Send ("smtp.gmail.com", 587, subject, body, username, password, senderEmail, senderAlias, htmlBody, to);
		}

		public static void Send (string smtp, int port, string subject, string body, string username, string password, string senderEmail, string senderAlias, bool htmlBody, params string[] to)
		{
			var mail = new MailMessage ();
			mail.From = new MailAddress (senderEmail, senderAlias);
			mail.Subject = subject;
			mail.Body = body;
			mail.IsBodyHtml = htmlBody;

			foreach (var email in to)
				mail.To.Add (email);

			var client = new SmtpClient (smtp);
			client.Port = port;
			client.Credentials = new NetworkCredential (username, password);
			client.EnableSsl = true;

			ServicePointManager.ServerCertificateValidationCallback = delegate {
				return true;
			};
			client.Send (mail);
		}
	}
}

