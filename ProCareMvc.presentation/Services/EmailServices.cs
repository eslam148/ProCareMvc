using System.Net.Mail;
using System.Net;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using MimeKit;
using static Org.BouncyCastle.Math.EC.ECCurve;
using NuGet.Common;
using ProCareMvc.Database.Entity;

namespace ProCareMvc.presentation.Services
{
    public class EmailServices
    {
        private readonly IConfiguration _config;

        public EmailServices(IConfiguration config)
        {
            _config = config;
        }
        public void sendEmail(string To,string Subject,string body)
        {
            var smtpServer = _config["EmailSettings:SmtpServer"];
            var smtpPort = int.Parse(_config["EmailSettings:SmtpPort"]);
            var senderEmail = _config["EmailSettings:SenderEmail"];
            var senderName = _config["EmailSettings:SenderName"];
            var appPassword = _config["EmailSettings:Password"];
            try
            {
                 
                var client = new SmtpClient(smtpServer, smtpPort)
                {
                    Credentials = new NetworkCredential(senderName, appPassword),
                    EnableSsl = true
                };
               
                client.Send(senderEmail, To, Subject, body);

               // client.Send(senderEmail, To, "Hello world", message);
            }
            catch (Exception ex) { 
                Console.WriteLine(ex.Message);

            }
            Console.WriteLine("send Success");

        }
    }
}
