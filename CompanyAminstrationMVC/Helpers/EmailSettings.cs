using CompanyAdminstrationMVC.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace CompanyAdminstrationMVC.PL.Helpers
{
	public class EmailSettings
	{

		public static void SendEmail(Email email) 
		{ 
		
		 var client = new SmtpClient("Smtp.gmail.com" ,587);

			client.EnableSsl = true;
			client.Credentials = new NetworkCredential("ym09602@gmail.com", "myaszphlnfqsurab") ;

			client.Send("ym09602@gmail.com" ,email.To ,email.Subject , email.Body );

		}





	}
}
