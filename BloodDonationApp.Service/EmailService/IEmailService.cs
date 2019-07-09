using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace BloodDonationApp.Service
{
    public interface IEmailService
    {
        bool SendMail(string mailTo, string subject, string body, AttachmentCollection attachments, string mailFrom = "");
    }
}
