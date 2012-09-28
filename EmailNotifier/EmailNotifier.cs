using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Threading;

namespace TridionCommunity.NotificationFramework
{
    public class EmailNotifier : INotifier
    {
        public void Notify(INotificationMessage message)
        {
            ThreadPool.QueueUserWorkItem(sendMail => SendMail(message));
        }
       
        private void SendMail(INotificationMessage message)
        {

            using (MailMessage mail = new MailMessage("workflowmailer@mycompany.com", "user@address.com"))
            {
                mail.Subject = "Your notifications";
                mail.Body = "Stuff waiting for you";

                using (SmtpClient smtp = new SmtpClient())
                {
                    try
                    {
                        smtp.Send(mail);
                    }
                    catch (ArgumentNullException e)
                    {
                        throw;
                    }
                    catch (InvalidOperationException e)
                    {
                    }
                    catch (SmtpException e)
                    {
                    }                                        
                }
            }
        }
    }
}
