using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Messaging;
using System.Net;
using System.IO;

namespace ControleApp.Util
{
    public class clsEnviarEmail
    {
        public string EnviarEmail(string EmailTo, string EmailAssunto, string EmailBody, string ComCopia)
        {
            try
            {
                var emailMessenger = CrossMessaging.Current.EmailMessenger;

                // Send a more complex email with the EmailMessageBuilder fluent interface.
                var email = new EmailMessageBuilder()
                  .To(EmailTo)
                  .Subject(EmailAssunto)
                  .Body(EmailBody)
                  .Build();

                emailMessenger.SendEmail(email);

                return "Sucesso !";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
    }
}
