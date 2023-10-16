using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Twilio;
using Twilio.Rest.Api.V2010;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Api.V2010.Account.AvailablePhoneNumberCountry;
using Twilio.Rest.Api.V2010.Account.Usage;

namespace Myezcare_Admin.Controllers
{
    public class TwilioAPIController : ApiController
    {
        [HttpGet]
        public List<AccountResource> SearchSubAccount(string subAccountName)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                string accountSid = ConfigurationManager.AppSettings["accountSid"].ToString();
                string authToken = ConfigurationManager.AppSettings["authToken"].ToString();

                TwilioClient.Init(accountSid, authToken);

                var accounts = AccountResource.Read(subAccountName);
                return accounts.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        public AccountResource FetchSubAccount(string sid)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                string accountSid = ConfigurationManager.AppSettings["accountSid"].ToString();
                string authToken = ConfigurationManager.AppSettings["authToken"].ToString();

                TwilioClient.Init(accountSid, authToken);

                var account = AccountResource.Fetch(pathSid: sid);
                return account;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        public List<AccountResource> ListAllAccounts()
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                string accountSid = ConfigurationManager.AppSettings["accountSid"].ToString();
                string authToken = ConfigurationManager.AppSettings["authToken"].ToString();

                TwilioClient.Init(accountSid, authToken);

                var accounts = AccountResource.Read(limit: 20);
                return accounts.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public AccountResource CreateSubAccount(string subAccountName)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                string accountSid = ConfigurationManager.AppSettings["accountSid"].ToString();
                string authToken = ConfigurationManager.AppSettings["authToken"].ToString();

                TwilioClient.Init(accountSid, authToken);

                return AccountResource.Create(subAccountName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public AccountResource UpdateSubAccount(string status, string name, string subAccountId)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

                AccountResource.StatusEnum status1;
                switch (status)
                {
                    case "active":
                        status1 = AccountResource.StatusEnum.Active;
                        break;
                    case "suspended":
                        status1 = AccountResource.StatusEnum.Suspended;
                        break;
                    case "closed":
                        status1 = AccountResource.StatusEnum.Closed;
                        break;
                    default:
                        throw new Exception("Invalid Status");
                }

                string accountSid = ConfigurationManager.AppSettings["accountSid"].ToString();
                string authToken = ConfigurationManager.AppSettings["authToken"].ToString();

                TwilioClient.Init(accountSid, authToken);

                return AccountResource.Update(
                status: status,
                pathSid: subAccountId,
                friendlyName: name
                );
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        public List<LocalResource> SearchAvailablePhoneNumbers(int areaCode, string pathCountryCode, string accountSid, string authToken)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                TwilioClient.Init(accountSid, authToken);

                var locals = LocalResource.Read(areaCode: areaCode, pathCountryCode: pathCountryCode, limit: 20);

                return locals.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public IncomingPhoneNumberResource RegisterPhoneNumber(string accountSid, string authToken, string phoneNumber)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                TwilioClient.Init(accountSid, authToken);

                var incomingPhoneNumber = IncomingPhoneNumberResource.Create(phoneNumber: new Twilio.Types.PhoneNumber(phoneNumber));
                UpdateWebHook(accountSid, authToken, incomingPhoneNumber.Sid);

                return incomingPhoneNumber;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public MessageResource SendMessageViaService(string accountSid, string authToken, string msid, string message, string toPhoneNumber)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                TwilioClient.Init(accountSid, authToken);

                return MessageResource.Create(
                    body: message,
                    messagingServiceSid: msid,
                    to: new Twilio.Types.PhoneNumber(toPhoneNumber)
                );
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        public List<IncomingPhoneNumberResource> GetPhoneNumbers(string accountSid, string authToken)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                TwilioClient.Init(accountSid, authToken);

                var incomingPhoneNumbers = IncomingPhoneNumberResource.Read(limit: 20);
                return incomingPhoneNumbers.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void UpdateWebHook(string accountSid, string authToken, string phoneSid)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                TwilioClient.Init(accountSid, authToken);

                var incomingPhoneNumbers = IncomingPhoneNumberResource.Update(
                    pathSid: phoneSid,
                    smsMethod: "POST",
                    smsUrl: new Uri("https://mobile.myezcare.com/ivr/index?CompanyName=icarens")
                );

                //voiceUrl: 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public Twilio.Rest.Messaging.V1.ServiceResource CreateMessagingService(string accountSid, string authToken, string messagingServiceName)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                TwilioClient.Init(accountSid, authToken);

                return Twilio.Rest.Messaging.V1.ServiceResource.Create(friendlyName: messagingServiceName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        public List<Twilio.Rest.Messaging.V1.ServiceResource> GetMessagingService(string accountSid, string authToken)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                TwilioClient.Init(accountSid, authToken);

                var services = Twilio.Rest.Messaging.V1.ServiceResource.Read(limit: 20);
                return services.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public Twilio.Rest.Messaging.V1.Service.PhoneNumberResource MapNumberToMessagingService(string accountSid, string authToken, string messagingServiceSid, string phoneNumberSid)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                TwilioClient.Init(accountSid, authToken);

                return Twilio.Rest.Messaging.V1.Service.PhoneNumberResource.Create(
                    phoneNumberSid: phoneNumberSid,
                    pathServiceSid: messagingServiceSid
                );
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        public List<Twilio.Rest.Messaging.V1.Service.PhoneNumberResource> GetNumberFromMessagingService(string accountSid, string authToken, string messagingServiceSid)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                TwilioClient.Init(accountSid, authToken);

                var phoneNumbers = Twilio.Rest.Messaging.V1.Service.PhoneNumberResource.Read(
                    pathServiceSid: messagingServiceSid,
                    limit: 20
                );

                return phoneNumbers.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public Twilio.Rest.Notify.V1.ServiceResource CreateNotifyService(string accountSid, string authToken, string notifyServiceName, string messagingServiceSid)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                TwilioClient.Init(accountSid, authToken);

                return Twilio.Rest.Notify.V1.ServiceResource.Create(friendlyName: notifyServiceName, messagingServiceSid: messagingServiceSid);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        public List<Twilio.Rest.Notify.V1.ServiceResource> GetNotifyService(string accountSid, string authToken)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                TwilioClient.Init(accountSid, authToken);

                var services = Twilio.Rest.Notify.V1.ServiceResource.Read(limit: 20);

                return services.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        public List<Twilio.Base.Resource> GetUsage(string accountSid, string authToken, int test)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                TwilioClient.Init(accountSid, authToken);

                var records = RecordResource.Read();
                var resultRecords = new List<Twilio.Base.Resource>();
                foreach (var record in records)
                {
                    if (Convert.ToInt32(record.Count) > 0 || record.Category == RecordResource.CategoryEnum.Totalprice)
                    {
                        resultRecords.Add(record);
                    }
                }

                return resultRecords;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public TwilioModel ConfigureSubAccount(string subAccountName, int areaCode, string pathCountryCode)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                string accountSid = ConfigurationManager.AppSettings["accountSid"].ToString();
                string authToken = ConfigurationManager.AppSettings["authToken"].ToString();

                TwilioClient.Init(accountSid, authToken);

                var subAccount = AccountResource.Create(subAccountName);
                var availablePhoneNumbers = SearchAvailablePhoneNumbers(areaCode, pathCountryCode, subAccount.Sid, subAccount.AuthToken);
                var registeredPhoneNumber = RegisterPhoneNumber(subAccount.Sid, subAccount.AuthToken, availablePhoneNumbers[0].PhoneNumber.ToString());

                var messagingService = CreateMessagingService(subAccount.Sid, subAccount.AuthToken, "MyEzCare_MessagingService_" + Guid.NewGuid().ToString());
                MapNumberToMessagingService(subAccount.Sid, subAccount.AuthToken, messagingService.Sid, registeredPhoneNumber.Sid);

                var notifyService = CreateNotifyService(subAccount.Sid, subAccount.AuthToken, "MyEzCare_NotifyService_" + Guid.NewGuid().ToString(), messagingService.Sid);

                return new TwilioModel()
                {
                    SubaccountSid = subAccount.Sid,
                    SubaccountToken = subAccount.AuthToken,
                    PhoneNumber = registeredPhoneNumber.PhoneNumber.ToString(),
                    MessagingServiceSid = messagingService.Sid,
                    NotifyServiceSid = notifyService.Sid
                };

                //return new TwilioModel()
                //{
                //    SubaccountSid = "ACdcb64707cf830686323efbcf493c58ab",
                //    SubaccountToken = "c727b624250a6607563e3e71f55bff77",
                //    PhoneNumber = "+1 779 213 0497",
                //    MessagingServiceSid = "MGb8654e1be7108ef92edd8d7da2733000",
                //    NotifyServiceSid = "NMGb8654e1be7108ef92edd8d7da2733000"
                //};
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating New Subaccount(s) - " + ex.Message);
            }
        }
    }

    public class TwilioModel
    {
        public string SubaccountSid { get; set; }
        public string SubaccountToken { get; set; }
        public string PhoneNumber { get; set; }
        public string MessagingServiceSid { get; set; }
        public string NotifyServiceSid { get; set; }
    }
}
