using System;
using System.Collections.Generic;
using System.Web.Mvc;
using HomeCareApi.Infrastructure;
using HomeCareApi.Infrastructure.DataProvider;
using HomeCareApi.Infrastructure.IDataProvider;
using HomeCareApi.Models.ApiModel;
using HomeCareApi.Models.Entity;
using Twilio.AspNet.Mvc;
using Twilio.TwiML;
using Gather = Twilio.TwiML.Voice.Gather;
using Uri = System.Uri;
//using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Notify.V1.Service;
using Twilio.Jwt.Client;
using Twilio.Jwt;
using Twilio;
using Twilio.Http;
using Twilio.Types;
using HomeCareApi.Models.ViewModel;

namespace HomeCareApi.Controllers
{
    public class IvrController : TwilioController
    {
        //public ActionResult BULK()
        //{
        //    // Find your Account SID and Auth Token at twilio.com/console
        //    string accountSid = "AC66ecdae635db80264a7e2db77a4d92c8";
        //    string authToken = "d24e24722dbfb3197e1a51a04459d944";
        //    string serviceSid = "IS5c4c21849d905c9f8b1da6c9c101805c";

        //    TwilioClient.Init(accountSid, authToken);

        //    var notification = NotificationResource.Create(
        //        serviceSid,
        //        toBinding: new List<string> {
        //            "{\"binding_type\":\"sms\",\"address\":\"+918780600912\"}",
        //            "{\"binding_type\":\"sms\",\"address\":\"+918780600912\"}",
        //            "{\"binding_type\":\"sms\",\"address\":\"+919737999861\"}",
        //            "{\"binding_type\":\"sms\",\"address\":\"+918734057239\"}"
        //        },
        //        body: "This is the Bulk SMS Demo Message");


        //    return null;
        //}


        private IIvrDataProvider _ivrDataProvider;
        static string CompanyName = string.Empty;

        static string ClockInText = "ClockIn";
        static string ClockOutText = "ClockOut";

        [HttpGet]
        public string Index()
        {
            return "Hey! It's working. " + Guid.NewGuid().ToString("N");
        }

        [HttpPost]
        public TwiMLResult Welcome()
        {
            //TwilioClient.Init(ConfigSettings.TwilioAccountSID, ConfigSettings.TwilioAuthToken);
            //var number = IncomingPhoneNumberResource.Fetch("PN736cdd2dbb6ba766ea9292434b716729");

            //var callerIds = IncomingPhoneNumberResource.Read(
            //phoneNumber: new PhoneNumber(ConfigSettings.TwilioFromNo));

            //foreach (var callerId in callerIds)
            //{
            //    Console.WriteLine(callerId.PhoneNumber);
            //}
            var response = new VoiceResponse();
            _ivrDataProvider = new IvrDataProvider();
            ApiResponse apiResponse = _ivrDataProvider.ValidateReferralContactNumber(Request["from"]);

            CompanyName = Request["CompanyName"];

            var referralIds = Convert.ToString(apiResponse.Data);
            if (apiResponse.IsSuccess && !string.IsNullOrWhiteSpace(referralIds))
            {
                string url = String.Format(ConfigSettings.APIUrl + "/ivr/ivrcode?referralIds={0}&CompanyName={1}", referralIds, CompanyName);
                var gather = new Gather(action: new Uri(url), numDigits: 10,
                    timeout: 60); //timeout:60
                //gather.Say("Welcome to Myezcare EVV. Please Enter your 10 digit IVR Code.",
                //    voice: ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage);

                gather.Say("Welcome to  my easy care automated clock in and clock, out system. Please Enter your 10 digit Mobile Number.",
                    voice: ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage);

                response.Append(gather);
                return TwiML(response);
            }
            else
            {
                string url = String.Format(ConfigSettings.APIUrl + "/ivr/ivrverification?CompanyName={0}", CompanyName);
                var gather = new Gather(action: new Uri(url), numDigits: 1,
                    timeout: 60); //timeout:60
                //gather.Say("This is not a patient's registered phone number. Press 1 to bypass patient verification or press 2 for exit.",
                //    voice: ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage);

                gather.Say("This is not a registered patient's phone number.To skip verification press 1 , to cancel this call simply hang up",
                    voice: ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage);

                response.Append(gather);
                return TwiML(response);
            }
            //return EndTheCall("Your number is not registered with us. Thank you for calling Myezcare EVV.");
        }


        [HttpPost]
        public ActionResult IvrVerification(string digits)
        {

            var response = new VoiceResponse();
            if (digits == "1")
            {
                string url = String.Format(ConfigSettings.APIUrl + "/ivr/ivrcode?CompanyName={0}", CompanyName);
                var gather = new Gather(action: new Uri(url), numDigits: 10,
                    timeout: 60); //timeout:60
                //gather.Say("Welcome to Myezcare EVV. Please Enter your 10 digit IVR Code.",
                //    voice: ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage);

                gather.Say("Welcome to  my easy care automated clock in and clock, out system. Please Enter your 10 digit Mobile Number.",
                    voice: ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage);
                response.Append(gather);
                return TwiML(response);
            }
            else if (digits == "2")
            {
                return EndTheCall("");
            }
            else
            {
                var gather = new Gather(action: Url.ActionUri("endthecall", "ivr"));
                response.Append(gather);
            }
            return TwiML(response);
        }


        [HttpPost]
        public ActionResult IvrCode(string digits, string referralIds = null)
        {
            var response = new VoiceResponse();
            var gather = new Gather();
            if (string.IsNullOrEmpty(digits) || digits.Length != 10)
            {
                //gather.Say("Please add correct IVR code again", ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage);

                gather.Say("Please add correct Mobile Number again", ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage);
            }
            else
            {
                string url = String.Format(ConfigSettings.APIUrl + "/ivr/ivrpin?mobileNumber={0}&referralIds={1}&CompanyName={2}", digits, referralIds, CompanyName);
                gather = new Gather(action: new Uri(url),
                    numDigits: 4, timeout: 60);
                //gather.Say("Please enter your 4 digit IVR PIN",
                //    voice: ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage);

                gather.Say("Please enter your 4 digit security Pin",
                        voice: ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage);
            }
            response.Append(gather);
            return TwiML(response);
        }

        [HttpPost]
        public ActionResult IvrPin(string digits, string mobileNumber, string referralIds = null)
        {
            var response = new VoiceResponse();
            var gather = new Gather();
            if (string.IsNullOrEmpty(digits) || digits.Length != 4)
            {
                //gather.Say("Please enter correct IVR pin again.",
                //    voice: ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage);

                gather.Say("Please enter correct security pin again.",
                        voice: ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage);
            }
            else
            {
                _ivrDataProvider = new IvrDataProvider();
                ApiResponse apiResponse = _ivrDataProvider.ValidateIvrCode(mobileNumber, digits);
                if (apiResponse.IsSuccess)
                {
                    Employee employee = ((ValidateIVRCodeModel)apiResponse.Data).Employee;//((ApiResponse<Employee>)apiResponse).Data;
                    string url = String.Format(ConfigSettings.APIUrl + "/ivr/clockinput?employeeId={0}&referralIds={1}&CompanyName={2}", employee.EmployeeID, referralIds, CompanyName);
                    gather = new Gather(
                        action: new Uri(url),
                        numDigits: 1, timeout: 60);
                    //gather.Say("Please Press 1 for Clock In, Press 2 for Clock Out.", ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage);

                    gather.Say("Press '1' to clock in , press '2' to clock out.", ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage);
                }
                else
                {
                    if (((ValidateIVRCodeModel)apiResponse.Data).Employee != null && ((ValidateIVRCodeModel)apiResponse.Data).PermissionID == 0 && referralIds == null)
                    {
                        gather = new Gather(action: Url.ActionUri("endthecall", "ivr"));
                        //gather.Say("You have not permission for Bypass clock in clock out. Thank you for calling MyEzcare EVV. Good bye.", ConfigSettings.TwilioVoice,
                        //    language: ConfigSettings.TwilioLanguage);

                        gather.Say("You have not permission for Bypass clock in clock out. Thank you for using My easy care IVR clock in and clock out system. Bye Bye", ConfigSettings.TwilioVoice,
                            language: ConfigSettings.TwilioLanguage);
                    }
                    else
                    {
                        gather = new Gather(action: Url.ActionUri("endthecall", "ivr"));
                        //gather.Say("Entered details are incorrect. Thank you for calling MyEzcare EVV. Good bye.", ConfigSettings.TwilioVoice,
                        //    language: ConfigSettings.TwilioLanguage);

                        gather.Say("Entered details are incorrect. Thank you for using My easy care IVR clock in and clock out system. Bye Bye", ConfigSettings.TwilioVoice,
                                language: ConfigSettings.TwilioLanguage);
                    }
                }
            }
            response.Append(gather);
            return TwiML(response);
        }

        [HttpPost]
        public ActionResult ClockInput(string digits, string employeeId, string referralIds = null)
        {
            var response = new VoiceResponse();
            if (digits == "1")
            {
                string url = (!string.IsNullOrEmpty(referralIds)) ?
                    String.Format(ConfigSettings.APIUrl + "/ivr/clockin?employeeId={0}&referralIds={1}&CompanyName={2}", employeeId, referralIds, CompanyName) :
                    String.Format(ConfigSettings.APIUrl + "/ivr/ivrbypassclockin?employeeId={0}&CompanyName={1}", employeeId, CompanyName);
                response.Redirect(new Uri(url), HttpMethod.Post);
            }
            else if (digits == "2")
            {
                string url = (!string.IsNullOrEmpty(referralIds)) ?
                    String.Format(ConfigSettings.APIUrl + "/ivr/clockout?employeeId={0}&referralIds={1}&CompanyName={2}", employeeId, referralIds, CompanyName) :
                    String.Format(ConfigSettings.APIUrl + "/ivr/ivrbypassclockout?employeeId={0}&CompanyName={1}", employeeId, CompanyName);
                response.Redirect(new Uri(url),
                    HttpMethod.Post);
            }
            else
            {
                var gather = new Gather(action: Url.ActionUri("endthecall", "ivr"));
                response.Append(gather);
            }
            return TwiML(response);
        }

        [HttpPost]
        public ActionResult ClockIn(string employeeId, string referralIds)
        {
            _ivrDataProvider = new IvrDataProvider();
            ApiResponse apiResponse = _ivrDataProvider.ClockIn(employeeId, referralIds);
            //return apiResponse.IsSuccess
            //    ? EndTheCall("Your clock in is apply successfully.")
            //    : EndTheCall("We have not found any schedule for Clock In.");

            //return apiResponse.IsSuccess
            //    ? EndTheCall("You have successfully clocked in. Please make sure to complete the time using the my easy care mobile app.")
            //    : EndTheCall("Huh! No schedule corresponding to the patient found. Please hang up and call office administrator.");

            if (apiResponse.IsSuccess)
            {
                return EndTheCall("You have successfully clocked in. Please make sure to complete the time using the my easy care mobile app.");
            }
            else
            {
                if ((int)apiResponse.Data == 1)
                {
                    var response = new VoiceResponse();
                    var gather = new Gather();

                    string url = String.Format(ConfigSettings.APIUrl + "/ivr/InstantNoScheduleClockIn?employeeId={0}&referralIds={1}&CompanyName={2}", employeeId, referralIds, CompanyName);
                    gather = new Gather(action: new Uri(url), numDigits: 1, timeout: 60);
                    //gather = new Gather(action: Url.ActionUri("InstantNoScheduleClockIn", "ivr"));
                    gather.Say("Huh! No schedule corresponding to the patient found. Do you want to do Instant No Schedule Clock In? Press 1 for YES, Press 2 for NO and end the call.", ConfigSettings.TwilioVoice,
                        language: ConfigSettings.TwilioLanguage);
                    response.Append(gather);
                    return TwiML(response);
                }
                return EndTheCall("Huh! No schedule corresponding to the patient found. Please hang up and call office administrator.");
            }

            //return apiResponse.IsSuccess
            //    ? EndTheCall("Your clock in is apply successfully.")
            //    : EndTheCall("We have not found any schedule for Clock In.");
        }


        [HttpPost]
        public ActionResult IVRBypassClockIn(string employeeId)
        {
            _ivrDataProvider = new IvrDataProvider();
            ApiResponse apiResponse = _ivrDataProvider.ClockOut(employeeId, referralIds);
            //return apiResponse.IsSuccess
            //    ? EndTheCall("Your clock out is apply successfully.")
            //    : EndTheCall("We have not found any schedule for Clock Out.");

            return apiResponse.IsSuccess
                ? EndTheCall("You have successfully clocked out. Please make sure to complete the time using the my easy care mobile app.")
                : EndTheCall("Huh! No schedule corresponding to the patient found. Please hang up and call office administrator.");
            ApiResponse apiResponse = _ivrDataProvider.IVRBypassClockIn(employeeId);
            //return apiResponse.IsSuccess
            //    ? EndTheCall("Your clock in is apply successfully.")
            //    : EndTheCall("We have not found any schedule for Clock In.");
            if (apiResponse.IsSuccess)
            {
                return EndTheCall("Your clock in is apply successfully.");
            }
            else
            {
                if ((int)apiResponse.Data == 1)
                {
                    var response = new VoiceResponse();
                    var gather = new Gather();

                    string url = String.Format(ConfigSettings.APIUrl + "/ivr/GetPatientDetails?type={2}&employeeId={0}&CompanyName={1}", employeeId, CompanyName, ClockInText);
                    gather = new Gather(action: new Uri(url), numDigits: 1, timeout: 60);
                    //gather = new Gather(action: Url.ActionUri("InstantNoScheduleClockIn", "ivr"));
                    gather.Say("We have not found any schedule for Clock In. Do you want to do Instant No Schedule Clock In? Press 1 for YES, Press 2 for NO and end the call.", ConfigSettings.TwilioVoice,
                        language: ConfigSettings.TwilioLanguage);
                    response.Append(gather);
                    return TwiML(response);
                }
                return EndTheCall("We have not found any schedule for Clock In.");
            }

        }



        #region Instant No Schedule  Clock In - ClockOut

        [HttpPost]
        public ActionResult GetPatientDetails(string digits, string type, string employeeId)
        {
            if (digits == "1")
            {
                var response = new VoiceResponse();
                var gather = new Gather();

                string url = String.Format(ConfigSettings.APIUrl + "/ivr/GetPatientID?type={2}&employeeId={0}&CompanyName={1}", employeeId, CompanyName, type);
                gather = new Gather(action: new Uri(url), timeout: 60);
                //gather = new Gather(action: Url.ActionUri("InstantNoScheduleClockIn", "ivr"));
                gather.Say("Please enter patient account number.", ConfigSettings.TwilioVoice,
                    language: ConfigSettings.TwilioLanguage);
                response.Append(gather);
                return TwiML(response);
            }
            return EndTheCall("Thank you for the calling.");
        }

        [HttpPost]
        public ActionResult GetPatientID(string digits, string type, string employeeId)
        {

            var response = new VoiceResponse();
            var gather = new Gather();
            if (string.IsNullOrEmpty(digits) || digits.Length > 10)
            {
                gather.Say("Please enter correct Account Number.",
                    voice: ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage);
                response.Append(gather);
            }

            _ivrDataProvider = new IvrDataProvider();
            ApiResponse apiResponse = _ivrDataProvider.IVRBypassClockIn(employeeId);
            //return apiResponse.IsSuccess
            //    ? EndTheCall("Your clock in is apply successfully.")
            //    : EndTheCall("We have not found any schedule for Clock In.");

            return apiResponse.IsSuccess
                ? EndTheCall("You have successfully clocked in. Please make sure to complete the time using the my easy care mobile app.")
                : EndTheCall("Huh! No schedule corresponding to the patient found. Please hang up and call office administrator.");
            ApiResponse apiResponse = _ivrDataProvider.GetPatientID(digits);

            if (apiResponse.IsSuccess && (int)apiResponse.Data > 0)
            {
                int referralIds = (int)apiResponse.Data;
                string url = String.Empty;
                if (type == ClockInText)
                    url = String.Format(ConfigSettings.APIUrl + "/ivr/InstantNoScheduleClockIn?digits=1&employeeId={0}&referralIds={1}&CompanyName={2}", employeeId, referralIds, CompanyName);
                else
                    url = String.Format(ConfigSettings.APIUrl + "/ivr/InstantNoScheduleClockOut?digits=1&employeeId={0}&referralIds={1}&CompanyName={2}", employeeId, referralIds, CompanyName);

                response.Redirect(new Uri(url), HttpMethod.Post);
            }
            else
            {
                gather.Say("Please enter correct Account Number.",
                    voice: ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage);
                response.Append(gather);
            }

            return TwiML(response);
        }

        [HttpPost]
        public ActionResult InstantNoScheduleClockIn(string digits, string employeeId, string referralIds)
        {
            if (digits == "1")
            {
                _ivrDataProvider = new IvrDataProvider();
                ApiResponse apiResponse = _ivrDataProvider.CreatePendingScheduleClockInOut(employeeId, referralIds, true);
                if (apiResponse.IsSuccess)
                    return EndTheCall("Your clock in is apply successfully.");
                else
                    return EndTheCall("There are some problem, please try later. Thank you for the calling.");
            }
            return EndTheCall("Thank you for the calling.");
        }



        [HttpPost]
        public ActionResult InstantNoScheduleClockOut(string digits, string employeeId, string referralIds)
        {
            if (digits == "1")
            {
                _ivrDataProvider = new IvrDataProvider();
                ApiResponse apiResponse = _ivrDataProvider.CreatePendingScheduleClockInOut(employeeId, referralIds, false);
                if (apiResponse.IsSuccess)
                    return EndTheCall("Your clock out is apply successfully.");
                else
                    return EndTheCall("There are some problem, please try later. Thank you for the calling.");
            }
            return EndTheCall("Thank you for the calling.");

        }




        #endregion


        [HttpPost]
        public ActionResult ClockOut(string employeeId, string referralIds)
        {
            _ivrDataProvider = new IvrDataProvider();
            ApiResponse apiResponse = _ivrDataProvider.ClockOut(employeeId, referralIds);
            //return apiResponse.IsSuccess
            //    ? EndTheCall("Your clock out is apply successfully.")
            //    : EndTheCall("We have not found any schedule for Clock Out.");

            if (apiResponse.IsSuccess)
            {
                return EndTheCall("Your clock out is apply successfully.");
            }
            else
            {
                if ((int)apiResponse.Data == 1)
                {
                    var response = new VoiceResponse();
                    var gather = new Gather();

                    string url =
                        String.Format(
                            ConfigSettings.APIUrl +
                            "/ivr/InstantNoScheduleClockOut?employeeId={0}&referralIds={1}&CompanyName={2}", employeeId,
                            referralIds, CompanyName);
                    gather = new Gather(action: new Uri(url), numDigits: 1, timeout: 60);
                    //gather = new Gather(action: Url.ActionUri("InstantNoScheduleClockIn", "ivr"));
                    gather.Say(
                        "We have not found any schedule for Clock Out. Do you want to do Instant No Schedule Clock Out? Press 1 for YES, Press 2 for NO and end the call.",
                        ConfigSettings.TwilioVoice,
                        language: ConfigSettings.TwilioLanguage);
                    response.Append(gather);
                    return TwiML(response);
                }
                return EndTheCall("We have not found any schedule for Clock In.");
            }
        }



        [HttpPost]
        public ActionResult IVRBypassClockOut(string employeeId)
        {
            _ivrDataProvider = new IvrDataProvider();
            ApiResponse apiResponse = _ivrDataProvider.IVRBypassClockOut(employeeId);
            //return apiResponse.IsSuccess
            //    ? EndTheCall("Your clock out is apply successfully.")
            //    : EndTheCall("We have not found any schedule for Clock In.");

            return apiResponse.IsSuccess
                ? EndTheCall("You have successfully clocked out. Please make sure to complete the time using the my easy care mobile app.")
                : EndTheCall("Huh! No schedule corresponding to the patient found. Please hang up and call office administrator.");
            //return apiResponse.IsSuccess
            //    ? EndTheCall("Your clock out is apply successfully.")
            //    : EndTheCall("We have not found any schedule for Clock In.");

            if (apiResponse.IsSuccess)
            {
                return EndTheCall("Your clock out is apply successfully.");
            }
            else
            {
                if ((int)apiResponse.Data == 1) // Has Permissions
                {
                    var response = new VoiceResponse();
                    var gather = new Gather();

                    string url = String.Format(ConfigSettings.APIUrl + "/ivr/GetPatientDetails?type={2}&employeeId={0}&CompanyName={1}", employeeId, CompanyName, ClockOutText);
                    gather = new Gather(action: new Uri(url), numDigits: 1, timeout: 60);
                    //gather = new Gather(action: Url.ActionUri("InstantNoScheduleClockIn", "ivr"));
                    gather.Say("We have not found any schedule for Clock Out. Do you want to do Instant No Schedule Clock In? Press 1 for YES, Press 2 for NO and end the call.", ConfigSettings.TwilioVoice,
                        language: ConfigSettings.TwilioLanguage);
                    response.Append(gather);
                    return TwiML(response);
                }
                return EndTheCall("We have not found any schedule for Clock Out.");
            }
        }



        private TwiMLResult EndTheCall(string message)
        {
            var response = new VoiceResponse();
            //response.Say(message + " Thank you for calling MyEzcare EVV. Good bye.",
            //    voice: ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage);

            response.Say(message + " Thank you for using My easy care IVR clock in and clock out system. Bye Bye",
                voice: ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage);
            response.Hangup();
            return TwiML(response);
        }

        [HttpPost]
        public ActionResult ThankYou(string digits)
        {
            var response = new VoiceResponse();
            var gather = new Gather();
            gather.Say("Thanks you are successfully clocked in",
                voice: ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage).Play(new System.Uri("http://www.hubharp.com/web_sound/BachGavotteShort.mp3"));
            response.Append(gather);
            return TwiML(response);
        }
    }
}