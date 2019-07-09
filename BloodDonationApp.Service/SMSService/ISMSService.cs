using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BloodDonationApp.Service
{
    public interface ISMSService
    {
        bool SendSMS(string To, string Message);
    }
}
