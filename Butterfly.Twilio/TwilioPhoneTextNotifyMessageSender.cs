﻿using Butterfly.Notify;
using NLog;
using System;
using System.Threading.Tasks;

using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Butterfly.Twilio
{
    public class TwilioPhoneTextNotifyMessageSender : BaseNotifyMessageSender {

        protected static readonly Logger logger = LogManager.GetCurrentClassLogger();

        protected readonly string twilioAccountSid;
        protected readonly string twilioAuthToken;

        public TwilioPhoneTextNotifyMessageSender(string twilioAccountSid, string twilioAuthToken) {
            this.twilioAccountSid = twilioAccountSid;
            this.twilioAuthToken = twilioAuthToken;
        }

        protected override async Task DoSendAsync(string from, string to, string subject, string bodyText, string bodyHtml) {
            TwilioClient.Init(this.twilioAccountSid, this.twilioAuthToken);

            try {
                MessageResource messageResource = await MessageResource.CreateAsync(
                    to: new PhoneNumber(to),
                    from: new PhoneNumber(from),
                    body: bodyText);
                logger.Debug($"SendAsync():messageResource.Sid={messageResource.Sid}");
            }
            catch (Exception e) {
                logger.Error(e);
            }
        }

        /*
        public override Dictionary<string, object> Lookup(string phone) {
            var lookupsClient = new LookupsClient(this.twilioAccountSid, this.twilioAuthToken);

            var phoneNumber = lookupsClient.GetPhoneNumber(phone, false);
            if (phoneNumber.RestException != null) throw new Exception(phoneNumber.RestException.Message);

            return new Dictionary<string, object> {
                { COUNTRY_CODE, phoneNumber.CountryCode },
                { FORMATTED, phoneNumber.NationalFormat },
            };
        }
        */
    }
}