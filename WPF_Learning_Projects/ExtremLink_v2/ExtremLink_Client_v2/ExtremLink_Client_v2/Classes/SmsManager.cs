using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Verify.V2.Service;
using Twilio.Types;

namespace ExtremLink_Client_v2.Classes
{
    public static class SmsManager
    {
        /*
        A class which manage the sms messages.
        */
        
        // Attributes
        private static string accountSid = "";
        private static string authToken = "";

        public static void sendSmsMessage(string receiverPhoneNum, string msg)
        {
            // A function which sends sms messages.
            TwilioClient.Init(accountSid, authToken);
            var verification = VerificationResource.Create(
                to: "+972556838827",
                channel: "sms",
                pathServiceSid: ""
            );
        }
    }
}
