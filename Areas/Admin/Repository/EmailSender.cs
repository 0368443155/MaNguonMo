using System.Net;
using System.Net.Mail;

namespace WebApp.Areas.Admin.Repository
{
	public class EmailSender : IEmailSender
	{
		public Task SendEmailAsync(string email, string subject, string message)
		{
			var client = new SmtpClient("smtp.gmail.com", 587)
			{
				EnableSsl = true, //bật bảo mật
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential("boluclac1301@gmail.com", "kupttxzgfxihqedn")
			};

			return client.SendMailAsync(
				new MailMessage(from: "boluclac1301@gmail.com",
								to: email,
								subject,
								message
								));
		}
	}
}