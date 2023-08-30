using System.Net.Mail;
using System.Text;
using SMSMicroService.Entities.Domains;
using SMSMicroService.Helpers.Interfaces; 

namespace SMSMicroService.Helpers
{
    public class EmailHelper: IEmailHelper
    {
        public async Task<bool> Send(EmailDomain model)
        {
            var mail = new MailMessage();
            mail.BodyEncoding = Encoding.UTF8;
            mail.Subject = model.Subject;
            mail.Body = model.Body;
            model.Email.ForEach(p =>
            {
                if(!string.IsNullOrEmpty(p))
                    mail.To.Add(p);
            });

            var from = AppConfig.Get("EmailSettings:From");

            mail.From = new MailAddress(from);
            
            mail.Priority = MailPriority.High;
            mail.IsBodyHtml = true;

            var host = AppConfig.Get("EmailSettings:Host"); 
            var port = AppConfig.Get("EmailSettings:Port"); 
            var smtp = new SmtpClient(host, int.Parse(port));
            smtp.UseDefaultCredentials = true;
            smtp.EnableSsl = true;

            var username = AppConfig.Get("EmailSettings:Username");
            var password = AppConfig.Get("EmailSettings:Password");

            smtp.Credentials = new System.Net.NetworkCredential(username, password);

            try
            {
                smtp.Send(mail);
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }  
    }
}
