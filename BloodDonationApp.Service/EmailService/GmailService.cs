using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BloodDonationApp.Service
{
    public class GmailService: IEmailService
    {
        private readonly string _mailFrom = string.Empty;
        private readonly string _emailSendDefault = string.Empty;
        private readonly string _defaultGateway = string.Empty;
        private readonly string _smtpPort = string.Empty;
        private readonly string _smtpUsername = string.Empty;
        private readonly string _smtpPassword = string.Empty;
        private readonly string _smtpEnableSsl = string.Empty;
        private readonly string _defaultEmail = string.Empty;

        private readonly IConfiguration config;
        public GmailService(IConfiguration config)
        {
            this.config = config;
            _mailFrom = this.config["GmailCredentials:MailFrom"];
            _emailSendDefault = this.config["GmailCredentials:EmailSendDefault"];
            _defaultGateway = this.config["GmailCredentials:DefaultGateway"];
            _smtpPort = this.config["GmailCredentials:SMTPPort"];
            _smtpUsername = this.config["GmailCredentials:SMTPUsername"];
            _smtpPassword = this.config["GmailCredentials:SMTPPassword"];
            _smtpEnableSsl = this.config["GmailCredentials:SMTPEnableSsl"];
            _defaultEmail = this.config["GmailCredentials:DefaultEmail"];

        }

        public bool SendMail(string mailTo, string subject, string body, AttachmentCollection attachments, string mailFrom = "")
        {
            bool isSent = true;

            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                smtp.UseDefaultCredentials = false;
                if (!bool.Parse(_emailSendDefault))
                {
                    smtp.Host = _defaultGateway;
                    smtp.Port = int.Parse(_smtpPort);
                    smtp.Credentials = new System.Net.NetworkCredential(_smtpUsername, _smtpPassword);
                    smtp.EnableSsl = bool.Parse(_smtpEnableSsl); ;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                }
                mail.From = new MailAddress(string.IsNullOrEmpty(mailFrom) ? _defaultEmail : mailFrom, "Blood Donation App");
                mail.To.Add(mailTo);
                mail.IsBodyHtml = true;
                mail.Subject = subject;
                mail.Body = body;
                smtp.Send(mail);
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return isSent;
        }
    }
}
