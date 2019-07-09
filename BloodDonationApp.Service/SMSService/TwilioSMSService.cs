using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using static Twilio.Rest.Api.V2010.Account.Call.FeedbackSummaryResource;

namespace BloodDonationApp.Service
{
    public class TwilioSMSService : ISMSService
    {
        private readonly string _accountSid = string.Empty;
        private readonly string _authToken = string.Empty;
        private readonly string _from = string.Empty;
        private readonly string _nepalISDCode = string.Empty;
        private readonly IConfiguration config;
        public TwilioSMSService(IConfiguration config)
        {
            this.config = config;
            _accountSid = this.config["TwilioCredentials:accountSid"];
            _authToken = this.config["TwilioCredentials:authToken"];
            _from = this.config["TwilioCredentials:from"];
            _nepalISDCode = this.config["TwilioCredentials:NepalISDCode"];
        }

        public bool SendSMS(string To, string Message)
        {
            try
            {
                TwilioClient.Init(_accountSid, _authToken);

                var message = MessageResource.Create(
                    body: Message,
                    from: new Twilio.Types.PhoneNumber(_from),
                    to: new Twilio.Types.PhoneNumber(_nepalISDCode+To)
                );
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
