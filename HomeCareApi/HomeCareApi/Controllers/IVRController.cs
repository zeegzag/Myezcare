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
    [Route("api/ivr")]
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
        static string logPath = string.Empty;
        static string FromMobile = string.Empty;
        static string referralIdForInstantSchedule = string.Empty;
        static string AudioFileURL = string.Empty;
        static string SelectedLanguage = string.Empty;

        BaseDataProvider baseDataProvider = new BaseDataProvider();

        [HttpPost]
        public TwiMLResult Index()
        {
            var response = new VoiceResponse();
            try
            {
                CompanyName = Request["CompanyName"];

                FromMobile = Request["from"];
                baseDataProvider.InsertIvrLogInDatabse(FromMobile);

                string url = String.Format(ConfigSettings.APIUrl + "/ivr/Welcome?CompanyName={0}", CompanyName);
                var gather = new Gather(action: new Uri(url), numDigits: 1,
                    timeout: 10); //timeout:60

                //Message: Press 1 for English. Presione 2 para español. Pindutin ang 3 para sa Fillipino.
                var uri = new Uri(ConfigSettings.APIUrl + "/assets/IvrAudioFiles/LanguageSelection.mp3");
                gather.Play(url: uri);

                response.Append(gather);
                response.Redirect(new Uri(url), HttpMethod.Post);
            }
            catch (Exception e)
            {
                baseDataProvider.InsertIvrLogInDatabse(FromMobile,errorLog:e.Message);

            }
            return TwiML(response);
        }



        [HttpPost]
        public TwiMLResult Welcome(string digits)
        {
            var response = new VoiceResponse();
            
            try
            {
                baseDataProvider.InsertIvrLogInDatabse(FromMobile, string.Format("Language Select Message : Press 1 for English. Presione 2 para español. Pindutin ang 3 para sa Fillipino. Input={0}", digits));
                if (string.IsNullOrEmpty(digits))
                {
                    string url = String.Format(ConfigSettings.APIUrl + "/ivr/Index?CompanyName={0}", CompanyName);
                    response.Redirect(new Uri(url), HttpMethod.Post);
                    return TwiML(response);
                }
                else
                {
                    if (digits == "1")
                    {
                        SelectedLanguage = digits;
                        AudioFileURL = ConfigSettings.APIUrl + Constants.EnglishAudioFilePath;
                    }
                    else if(digits == "2")
                    {
                        SelectedLanguage = digits;
                        AudioFileURL = ConfigSettings.APIUrl + Constants.SpanishAudioFilePath;
                    }
                    else if (digits == "3")
                    {
                        SelectedLanguage = digits;
                        AudioFileURL = ConfigSettings.APIUrl + Constants.FilipinoAudioFilePath;
                    }
                    else
                    {
                        var uri = new Uri(ConfigSettings.APIUrl + "/assets/IvrAudioFiles/InvalidOption.mp3");
                        response.Play(url: uri);
                        string url = String.Format(ConfigSettings.APIUrl + "ivr/index?CompanyName={0}", CompanyName);
                        response.Redirect(new Uri(url), HttpMethod.Post);
                    }
                }

                _ivrDataProvider = new IvrDataProvider();
                ApiResponse apiResponse = _ivrDataProvider.ValidateReferralContactNumber(Request["from"]);
                FromMobile = Request["from"];

                //CompanyName = Request["CompanyName"];
                logPath = AppDomain.CurrentDomain.BaseDirectory + String.Format(ConfigSettings.IvrLogFullPath, CompanyName);

                var referralIds = Convert.ToString(apiResponse.Data);
                if (apiResponse.IsSuccess && !string.IsNullOrWhiteSpace(referralIds))
                {
                    string url = String.Format(ConfigSettings.APIUrl + "/ivr/ivrcode?referralIds={0}&CompanyName={1}&Redirect=Welcome", referralIds, CompanyName);
                    //var gather = new Gather(action: new Uri(url), numDigits: 10, timeout: 10);
                    var gather = new Gather(action: new Uri(url), finishOnKey:"#", timeout: 10);

                    var uri = new Uri(AudioFileURL + "Welcome_EnterMobileOrIvrID.mp3");
                    gather.Play(url: uri);
                    //gather.Say("Welcome to  my easy care automated clock in and clock out system. Please Enter your 10 digit Mobile Number or IVR ID.",
                    //    voice: ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage);

                    response.Append(gather);
                    response.Redirect(new Uri(url), HttpMethod.Post);

                    //return TwiML(response);
                }
                else
                {
                    string url = String.Format(ConfigSettings.APIUrl + "/ivr/ivrverification?CompanyName={0}", CompanyName);
                    var gather = new Gather(action: new Uri(url), numDigits: 1,
                        timeout: 10); //timeout:60

                    var uri = new Uri(AudioFileURL + "ThisIsNotPatientRegisteredPhone.mp3");
                    gather.Play(url: uri);
                    //gather.Say("This is not a registered patient's phone number. To skip verification press 1 , to cancel this call simply hang up",
                    //    voice: ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage);

                    response.Append(gather);
                    response.Redirect(new Uri(url), HttpMethod.Post);

                    //return TwiML(response);
                }
            }
            catch (Exception e)
            {
                baseDataProvider.InsertIvrLogInDatabse(FromMobile, errorLog: e.Message);
            }
            //return EndTheCall("Your number is not registered with us. Thank you for calling Myezcare EVV.");
            return TwiML(response);
        }


        [HttpPost]
        public ActionResult IvrVerification(string digits,string PreviousData=null)
        {
            var response = new VoiceResponse();
            try
            {
                baseDataProvider.InsertIvrLogInDatabse(FromMobile, string.Format("This is not a registered patient's phone number. To skip verification press 1 , to cancel this call simply hang up. Input={0} PreviousData={1}", digits, PreviousData));
                Common.CreateLogFile(string.Format("IvrVerification digits={0}", digits), FromMobile, logPath, ConfigSettings.IsCaptureCallLog);

                if (digits == "1")
                {
                    string url = String.Format(ConfigSettings.APIUrl + "/ivr/patientdetail?CompanyName={0}", CompanyName);
                    var gather = new Gather(action: new Uri(url), numDigits: 1, timeout: 10);

                    var uri = new Uri(AudioFileURL + "Welcome_EnterPatientAcNoOrMobNo.mp3");
                    gather.Play(url: uri);
                    //gather.Say("Welcome to  my easy care automated clock in and clock out system. Press 1 for enter patient's account number, Press 2 for enter patient's mobile number. Press star to go main menu",
                    //    voice: ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage);

                    response.Append(gather);
                    response.Redirect(new Uri(url), HttpMethod.Post);

                    return TwiML(response);
                }
                else if(string.IsNullOrEmpty(digits))
                {
                    string url = String.Format(ConfigSettings.APIUrl + "ivr/welcome?CompanyName={0}&digits={1}", CompanyName,SelectedLanguage);
                    response.Redirect(new Uri(url), HttpMethod.Post);

                    //var gather = new Gather(action: Url.ActionUri("endthecall", "ivr"));
                    //response.Append(gather);
                }
                else
                {
                    var uri = new Uri(AudioFileURL + "InvalidOption.mp3");
                    response.Play(url: uri);
                    string url = String.Format(ConfigSettings.APIUrl + "ivr/welcome?CompanyName={0}&digits={1}", CompanyName, SelectedLanguage);
                    response.Redirect(new Uri(url), HttpMethod.Post);
                }
            }
            catch (Exception e)
            {
                baseDataProvider.InsertIvrLogInDatabse(FromMobile, errorLog: e.Message);
            }
            return TwiML(response);
        }

        public ActionResult PatientDetail(string digits)
        {
            var response = new VoiceResponse();

            try
            {
                baseDataProvider.InsertIvrLogInDatabse(FromMobile, string.Format("Welcome to  my easy care automated clock in and clock out system. Press 1 for enter patient's account number, Press 2 for enter patient's mobile number. Press star to go main menu. Input={0}", digits));
                Common.CreateLogFile(string.Format("PatientDetail digits={0}", digits), FromMobile, logPath, ConfigSettings.IsCaptureCallLog);

                if (digits == "1")
                {
                    string url = String.Format(ConfigSettings.APIUrl + "/ivr/verifypatient?count={0}&CompanyName={1}", 0, CompanyName);
                    var gather = new Gather(action: new Uri(url), finishOnKey: "#", timeout: 10);

                    var uri = new Uri(AudioFileURL + "EnterPatientAccNo.mp3");
                    gather.Play(url: uri);
                    //gather.Say("Enter Patient's account number and press # to submit.",
                    //    voice: ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage);
                    response.Append(gather);
                    response.Redirect(new Uri(url), HttpMethod.Post);
                }
                else if (digits == "2")
                {
                    string url = String.Format(ConfigSettings.APIUrl + "/ivr/verifypatientbymobile?count={0}&CompanyName={1}", 0, CompanyName);
                    //var gather = new Gather(action: new Uri(url), numDigits: 10, timeout: 5);
                    var gather = new Gather(action: new Uri(url), finishOnKey: "#", timeout: 10);

                    var uri = new Uri(AudioFileURL + "EnterPatientMobNo.mp3");
                    gather.Play(url: uri);
                    //gather.Say("Please enter Patient's mobile number.",
                    //    voice: ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage);
                    response.Append(gather);
                    response.Redirect(new Uri(url), HttpMethod.Post);
                }
                else if (digits == "*")
                {
                    //Go to main menu
                    string url = String.Format(ConfigSettings.APIUrl + "ivr/welcome?CompanyName={0}&digits={1}", CompanyName, SelectedLanguage);
                    response.Redirect(new Uri(url), HttpMethod.Post);
                    return TwiML(response);
                }
                else if(string.IsNullOrEmpty(digits))
                {
                    string url = String.Format(ConfigSettings.APIUrl + "/ivr/ivrverification?CompanyName={0}&digits=1", CompanyName);
                    response.Redirect(new Uri(url), HttpMethod.Post);

                    //var gather = new Gather(action: Url.ActionUri("endthecall", "ivr"));
                    //response.Append(gather);
                }
                else
                {
                    var uri = new Uri(AudioFileURL + "InvalidOption.mp3");
                    response.Play(url: uri);
                    string url = String.Format(ConfigSettings.APIUrl + "/ivr/ivrverification?CompanyName={0}&digits=1", CompanyName);
                    response.Redirect(new Uri(url), HttpMethod.Post);
                }
            }
            catch (Exception e)
            {
                baseDataProvider.InsertIvrLogInDatabse(FromMobile, errorLog: e.Message);
            }
            return TwiML(response);
        }

        [HttpPost]
        public ActionResult VerifyPatient(string digits,int count)
        {
            var response = new VoiceResponse();

            try
            {
                if (string.IsNullOrEmpty(digits))
                {
                    string url = String.Format(ConfigSettings.APIUrl + "/ivr/patientdetail?CompanyName={0}&digits=1", CompanyName);
                    response.Redirect(new Uri(url), HttpMethod.Post);
                    return TwiML(response);
                }

                baseDataProvider.InsertIvrLogInDatabse(FromMobile, string.Format("Enter Patient's account number and press # to submit. Input={0}, count={1}", digits,count));
                Common.CreateLogFile(string.Format("VerifyPatient digits={0}, count={1}", digits, count), FromMobile, logPath, ConfigSettings.IsCaptureCallLog);

                _ivrDataProvider = new IvrDataProvider();
                ApiResponse apiResponse = _ivrDataProvider.VerifyPatient(digits);
                var referralId = Convert.ToString(apiResponse.Data);
                //int count = 0;
                referralId = string.IsNullOrEmpty(referralId) ? "0" : referralId;

                if (Convert.ToInt64(referralId) > 0)
                {
                    string url = String.Format(ConfigSettings.APIUrl + "/ivr/ivrcode?referralIds={0}&CompanyName={1}&PreviousData={2}&Redirect=VerifyPatient", referralId, CompanyName, digits);
                    //var gather = new Gather(action: new Uri(url), numDigits: 10,timeout: 5);
                    var gather = new Gather(action: new Uri(url), finishOnKey: "#", timeout: 10);

                    var uri = new Uri(AudioFileURL + "EnterMobileOrIvrID.mp3");
                    gather.Play(url: uri);
                    //gather.Say("Please Enter your 10 digit Mobile Number or IVR ID.",
                    //    voice: ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage);
                    response.Append(gather);
                    response.Redirect(new Uri(url), HttpMethod.Post);
                }
                else
                {
                    count = count + 1;
                    if (count < 3)
                    {
                        string url = String.Format(ConfigSettings.APIUrl + "/ivr/verifypatient?count={0}&CompanyName={1}", count, CompanyName);
                        var gather = new Gather(action: new Uri(url), finishOnKey: "#", timeout: 10);

                        var uri = new Uri(AudioFileURL + "AccountNumberNotFound.mp3");
                        gather.Play(url: uri);
                        //gather.Say("Account number not found, please try again.",
                        //    voice: ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage);
                        response.Append(gather);

                        string url1 = String.Format(ConfigSettings.APIUrl + "/ivr/VerifyPatient?count={0}&CompanyName={1}&digits={2}", count-1, CompanyName,digits);
                        response.Redirect(new Uri(url1), HttpMethod.Post);
                    }
                    else
                    {
                        return EndTheCall(AudioFileURL + "MaximumAttemptReachedCallAgain.mp3");
                    }
                }
            }
            catch (Exception e)
            {
                baseDataProvider.InsertIvrLogInDatabse(FromMobile, errorLog: e.Message);
            }

            return TwiML(response);
        }
        

        [HttpPost]
        public ActionResult VerifyPatientByMobile(string digits, int count)
        {
            var response = new VoiceResponse();

            try
            {
                if (string.IsNullOrEmpty(digits))
                {
                    string url = String.Format(ConfigSettings.APIUrl + "/ivr/patientdetail?CompanyName={0}&digits=2", CompanyName);
                    response.Redirect(new Uri(url), HttpMethod.Post);
                    return TwiML(response);
                }

                baseDataProvider.InsertIvrLogInDatabse(FromMobile, string.Format("Please enter Patient's mobile number. Input={0}, count={1}", digits, count));
                Common.CreateLogFile(string.Format("VerifyPatientByMobile digits={0}, count={1}", digits, count), FromMobile, logPath, ConfigSettings.IsCaptureCallLog);

                _ivrDataProvider = new IvrDataProvider();
                ApiResponse apiResponse = _ivrDataProvider.VerifyPatientByMobile(digits);

                var referralIds = Convert.ToString(apiResponse.Data);
                if (apiResponse.IsSuccess && !string.IsNullOrWhiteSpace(referralIds))
                {
                    string url = String.Format(ConfigSettings.APIUrl + "/ivr/ivrcode?referralIds={0}&CompanyName={1}&PreviousData={2}&Redirect=VerifyPatientByMobile", referralIds, CompanyName, digits);
                    //var gather = new Gather(action: new Uri(url), numDigits: 10,timeout: 5);
                    var gather = new Gather(action: new Uri(url), finishOnKey: "#", timeout: 10);

                    var uri = new Uri(AudioFileURL + "EnterMobileOrIvrID.mp3");
                    gather.Play(url: uri);
                    //gather.Say("Please Enter your 10 digit Mobile Number or IVR ID.",
                    //    voice: ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage);
                    response.Append(gather);
                    response.Redirect(new Uri(url), HttpMethod.Post);
                }
                else
                {
                    count = count + 1;
                    if (count < 3)
                    {
                        string url = String.Format(ConfigSettings.APIUrl + "/ivr/verifypatientbymobile?count={0}&CompanyName={1}", count, CompanyName);
                        //var gather = new Gather(action: new Uri(url), numDigits: 10, timeout: 10);
                        var gather = new Gather(action: new Uri(url), finishOnKey: "#", timeout: 10);

                        var uri = new Uri(AudioFileURL + "MobileNumberNotFound.mp3");
                        gather.Play(url: uri);
                        //gather.Say("Mobile number not found, please try again.",
                        //    voice: ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage);

                        response.Append(gather);

                        string url1 = String.Format(ConfigSettings.APIUrl + "/ivr/verifypatientbymobile?count={0}&CompanyName={1}&digits={2}", count-1, CompanyName,digits);
                        response.Redirect(new Uri(url1), HttpMethod.Post);
                    }
                    else
                    {
                        return EndTheCall(AudioFileURL + "MaximumAttemptReachedCallAgain.mp3");
                    }
                }
            }
            catch (Exception e)
            {
                baseDataProvider.InsertIvrLogInDatabse(FromMobile, errorLog: e.Message);
            }
            return TwiML(response);
        }

        [HttpPost]
        public ActionResult IvrCode(string digits, string Redirect, string referralIds = null,string PreviousData=null,int count=0)
        {
            var response = new VoiceResponse();

            try
            {
                baseDataProvider.InsertIvrLogInDatabse(FromMobile, string.Format("Please Enter your 10 digit Mobile Number or IVR ID. Input={0}, Redirect={1}, referralIds={2}, PreviousData={3}, count={4}", digits, Redirect, referralIds, PreviousData, count));
                Common.CreateLogFile(string.Format("IvrCode digits={0}, referralIds={1}", digits, referralIds), FromMobile, logPath, ConfigSettings.IsCaptureCallLog);

                var gather = new Gather();
                //if (digits.Length != 10)
                //{
                //    gather.Say("I am sorry no record found, Please enter the correct 10 digit Mobile number or IVR Id", ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage);
                //}
                if (string.IsNullOrEmpty(digits))
                {
                    if (Redirect == "Welcome")
                    {
                        string url = String.Format(ConfigSettings.APIUrl + "ivr/welcome?CompanyName={0}&digits={1}", CompanyName,SelectedLanguage);
                        response.Redirect(new Uri(url), HttpMethod.Post);
                    }
                    else if (Redirect == "VerifyPatient")
                    {
                        string url = String.Format(ConfigSettings.APIUrl + "/ivr/verifypatient?count={0}&CompanyName={1}&digits={2}", 0, CompanyName, PreviousData);
                        response.Redirect(new Uri(url), HttpMethod.Post);
                    }
                    else if (Redirect == "VerifyPatientByMobile")
                    {
                        string url = String.Format(ConfigSettings.APIUrl + "/ivr/verifypatientbymobile?count={0}&CompanyName={1}&digits={2}", 0, CompanyName, PreviousData);
                        response.Redirect(new Uri(url), HttpMethod.Post);
                    }
                }
                else
                {
                    _ivrDataProvider = new IvrDataProvider();
                    ApiResponse apiResponse = _ivrDataProvider.ValidateEmployeeMobile(digits);

                    if (apiResponse.IsSuccess)
                    {
                        string url = String.Format(ConfigSettings.APIUrl + "/ivr/ivrpin?mobileNumber={0}&referralIds={1}&CompanyName={2}", digits, referralIds, CompanyName);
                        //gather = new Gather(action: new Uri(url),numDigits: 4, timeout: 10);
                        gather = new Gather(action: new Uri(url), finishOnKey:"#", timeout: 10);

                        var uri = new Uri(AudioFileURL + "EnterSecurityPin.mp3");
                        gather.Play(url: uri);
                        //gather.Say("Please enter security Pin",
                        //        voice: ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage);
                        response.Append(gather);
                        response.Redirect(new Uri(url), HttpMethod.Post);
                    }
                    else
                    {
                        count = count + 1;
                        if (count < 3)
                        {
                            string url = String.Format(ConfigSettings.APIUrl + "/ivr/IvrCode?count={0}&CompanyName={1}&referralIds={2}&Redirect={3}", count, CompanyName,referralIds,Redirect);
                            //gather = new Gather(action: new Uri(url), numDigits: 10, timeout: 10);
                            gather = new Gather(action: new Uri(url), finishOnKey: "#", timeout: 10);

                            var uri = new Uri(AudioFileURL + "MobileNumberNotFound.mp3");
                            gather.Play(url: uri);
                            //gather.Say("Mobile number not found, please try again.",
                            //    voice: ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage);

                            response.Append(gather);
                            string url1 = String.Format(ConfigSettings.APIUrl + "/ivr/IvrCode?count={0}&CompanyName={1}&referralIds={2}&Redirect={3}&digits={4}", count-1, CompanyName, referralIds, Redirect,digits);
                            response.Redirect(new Uri(url1), HttpMethod.Post);
                        }
                        else
                        {
                            return EndTheCall(AudioFileURL + "MaximumAttemptReachedCallAgain.mp3");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                baseDataProvider.InsertIvrLogInDatabse(FromMobile, errorLog: e.Message);
            }

            return TwiML(response);
        }

        [HttpPost]
        public ActionResult IvrPin(string digits, string mobileNumber, string referralIds = null, int count=0)
        {
            var response = new VoiceResponse();

            try
            {
                if (string.IsNullOrEmpty(digits))
                {
                    string url = String.Format(ConfigSettings.APIUrl + "/ivr/ivrcode?referralIds={0}&CompanyName={1}&digits={2}", referralIds, CompanyName, mobileNumber);
                    response.Redirect(new Uri(url), HttpMethod.Post);
                    return TwiML(response);
                }

                baseDataProvider.InsertIvrLogInDatabse(FromMobile, string.Format("Please enter security Pin. Input={0}, mobilenumber={1}, referralIds={2}, count={3}", digits, mobileNumber, referralIds, count));
                Common.CreateLogFile(string.Format("IvrPin digits={0}, mobilenumber={1}, referralIds={2}", digits, mobileNumber, referralIds), FromMobile, logPath, ConfigSettings.IsCaptureCallLog);

                var gather = new Gather();
                if (string.IsNullOrEmpty(digits) || digits.Length != 4)
                {
                    var uri = new Uri(AudioFileURL + "EnterCorrectSecurityPin.mp3");
                    gather.Play(url: uri);
                    //gather.Say("Please enter correct security pin again.",
                    //        voice: ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage);
                }
                else
                {
                    _ivrDataProvider = new IvrDataProvider();
                    string patientPhoneNo = Request["from"];
                    ApiResponse apiResponse = _ivrDataProvider.ValidateIvrCode(mobileNumber, digits, patientPhoneNo);
                    if (apiResponse.IsSuccess)
                    {
                        if (((ValidateIVRCodeModel)apiResponse.Data).PermissionID == 0)
                        {
                            gather = new Gather(action: Url.ActionUri("endthecall", "ivr"));

                            var uri = new Uri(AudioFileURL + "NoPermissionForBypassClockInClockOut.mp3");
                            gather.Play(url: uri);
                            response.Append(gather);

                            //return EndTheCall(ConfigSettings.APIUrl + "/assets/IvrAudioFiles/NoPermissionForBypassClockInClockOut.mp3");
                            //gather.Say("You do not have permission for Bypass clock in clock out. Thank you for using My easy care IVR clock in and clock out system", ConfigSettings.TwilioVoice,
                            //    language: ConfigSettings.TwilioLanguage);
                        }
                        else
                        {
                            Employee employee = ((ValidateIVRCodeModel)apiResponse.Data).Employee;//((ApiResponse<Employee>)apiResponse).Data;
                            ApiResponse apiRes = _ivrDataProvider.CheckForClockInClockOut(Convert.ToString(employee.EmployeeID), referralIds);

                            string actionTaken = ((int)apiRes.Data == 0) ? Constants.ClockIn : Constants.ClockOut;

                            string url = String.Format(ConfigSettings.APIUrl + "/ivr/clockinput?employeeId={0}&referralIds={1}&CompanyName={2}&MobileNumber={3}&IvrPin={4}&ActionTaken={5}", employee.EmployeeID, referralIds, CompanyName, mobileNumber, digits, actionTaken);
                            gather = new Gather(
                                action: new Uri(url),
                                numDigits: 1, timeout: 10);
                            if ((int)apiRes.Data == 0)
                            {
                                var uri = new Uri(AudioFileURL + "Press1ToClockIn.mp3");
                                gather.Play(url: uri);
                                //gather.Say("Press '1' to clock in. Press star to go main menu.", ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage);
                                response.Append(gather);
                                response.Redirect(new Uri(url), HttpMethod.Post);
                            }
                            else
                            {
                                var uri = new Uri(AudioFileURL + "Press2ToclockOut.mp3");
                                gather.Play(url: uri);
                                //gather.Say("Press '2' to clock out. Press star to go main menu.", ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage);
                                response.Append(gather);
                                response.Redirect(new Uri(url), HttpMethod.Post);
                            }
                        }
                    }
                    else
                    {
                        //if (((ValidateIVRCodeModel)apiResponse.Data).Employee != null && ((ValidateIVRCodeModel)apiResponse.Data).PermissionID == 0 && referralIds == null)
                        //{
                        //    gather = new Gather(action: Url.ActionUri("endthecall", "ivr"));

                        //    var uri = new Uri(ConfigSettings.APIUrl + "/assets/IvrAudioFiles/NoPermissionForBypassClockInClockOut.mp3");
                        //    gather.Play(url: uri);
                        //    response.Append(gather);
                        //    //gather.Say("You do not have permission for Bypass clock in clock out. Thank you for using My easy care IVR clock in and clock out system", ConfigSettings.TwilioVoice,
                        //    //    language: ConfigSettings.TwilioLanguage);
                        //}
                        //else
                        //{
                            count = count + 1;
                            if (count < 3)
                            {
                                string url = String.Format(ConfigSettings.APIUrl + "/ivr/IvrPin?count={0}&CompanyName={1}&mobileNumber={2}&referralIds={3}", count, CompanyName,mobileNumber, referralIds);
                                //gather = new Gather(action: new Uri(url), numDigits: 4, timeout: 10);
                                gather = new Gather(action: new Uri(url), finishOnKey:"#", timeout: 10);

                                var uri = new Uri(AudioFileURL + "SecurityPinNotFound.mp3");
                                gather.Play(url: uri);
                                //gather.Say("Mobile number not found, please try again.",
                                //    voice: ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage);

                                response.Append(gather);
                                string url1 = String.Format(ConfigSettings.APIUrl + "/ivr/IvrPin?count={0}&CompanyName={1}&mobileNumber={2}&referralIds={3}&digits={4}", count-1, CompanyName, mobileNumber, referralIds,digits);
                                response.Redirect(new Uri(url1), HttpMethod.Post);
                            }
                            else
                            {
                                return EndTheCall(AudioFileURL + "MaximumAttemptReachedCallAgain.mp3");
                            }


                            //gather = new Gather(action: Url.ActionUri("endthecall", "ivr"));

                            //var uri = new Uri(ConfigSettings.APIUrl + "/assets/IvrAudioFiles/EnteredDetailsAreIncorrect.mp3");
                            //gather.Play(url: uri);
                            //response.Append(gather);
                            ////gather.Say("Entered details are incorrect. Thank you for using My easy care IVR clock in and clock out system", ConfigSettings.TwilioVoice,
                            ////        language: ConfigSettings.TwilioLanguage);
                        //}
                    }
                }
            }
            catch (Exception e)
            {
                baseDataProvider.InsertIvrLogInDatabse(FromMobile, errorLog: e.Message);
            }
            return TwiML(response);
        }

        [HttpPost]
        public ActionResult ClockInput(string digits, string employeeId,string MobileNumber,string IvrPin,string ActionTaken, string referralIds = null)
        {
            var response = new VoiceResponse();
            try
            {
                Common.CreateLogFile(string.Format("ClockInput digits={0}, employeeId={1}, referralIds={2}", digits, employeeId, referralIds), FromMobile, logPath, ConfigSettings.IsCaptureCallLog);
                if (digits == "1" && ActionTaken == Constants.ClockIn)
                {
                    baseDataProvider.InsertIvrLogInDatabse(FromMobile, string.Format("Press '1' to clock in. Press star to go main menu. Input={0}, employeeId={1}, MobileNumber={2}, IvrPin={3}, ActionTaken={4}, referralIds={5}", digits, employeeId, MobileNumber, IvrPin, ActionTaken, referralIds));
                    string url = (!string.IsNullOrEmpty(referralIds)) ?
                        String.Format(ConfigSettings.APIUrl + "/ivr/clockin?employeeId={0}&referralIds={1}&CompanyName={2}", employeeId, referralIds, CompanyName) :
                        String.Format(ConfigSettings.APIUrl + "/ivr/ivrbypassclockin?employeeId={0}&CompanyName={1}", employeeId, CompanyName);
                    response.Redirect(new Uri(url), HttpMethod.Post);
                }
                else if (digits == "2" && ActionTaken == Constants.ClockOut)
                {
                    baseDataProvider.InsertIvrLogInDatabse(FromMobile, string.Format("Press '2' to clock out. Press star to go main menu. Input={0}, employeeId={1}, MobileNumber={2}, IvrPin={3}, ActionTaken={4}, referralIds={5}", digits, employeeId, MobileNumber, IvrPin, ActionTaken, referralIds));
                    string url = (!string.IsNullOrEmpty(referralIds)) ?
                        String.Format(ConfigSettings.APIUrl + "/ivr/clockout?employeeId={0}&referralIds={1}&CompanyName={2}", employeeId, referralIds, CompanyName) :
                        String.Format(ConfigSettings.APIUrl + "/ivr/ivrbypassclockout?employeeId={0}&CompanyName={1}", employeeId, CompanyName);
                    response.Redirect(new Uri(url),
                        HttpMethod.Post);
                }
                else if (digits == "*")
                {
                    //Go to main menu (not registered patient number)
                    string url = String.Format(ConfigSettings.APIUrl + "ivr/welcome?CompanyName={0}&digits={1}", CompanyName,SelectedLanguage);
                    response.Redirect(new Uri(url), HttpMethod.Post);
                    return TwiML(response);
                }
                else if (string.IsNullOrEmpty(digits))
                {
                    string url = String.Format(ConfigSettings.APIUrl + "/ivr/ivrpin?mobileNumber={0}&referralIds={1}&CompanyName={2}&digits={3}", MobileNumber, referralIds, CompanyName, IvrPin);
                    response.Redirect(new Uri(url), HttpMethod.Post);
                    return TwiML(response);
                }
                else
                {
                    var uri = new Uri(AudioFileURL + "InvalidOption.mp3");
                    response.Play(url: uri);
                    string url = String.Format(ConfigSettings.APIUrl + "/ivr/ivrpin?mobileNumber={0}&referralIds={1}&CompanyName={2}&digits={3}", MobileNumber, referralIds, CompanyName, IvrPin);
                    response.Redirect(new Uri(url), HttpMethod.Post);
                    //var gather = new Gather(action: Url.ActionUri("endthecall", "ivr"));
                    //response.Append(gather);
                }
            }
            catch (Exception e)
            {
                baseDataProvider.InsertIvrLogInDatabse(FromMobile, errorLog: e.Message);
            }
            return TwiML(response);
        }



        [HttpPost]
        public ActionResult ClockIn(string employeeId, string referralIds)
        {
            
            Common.CreateLogFile(string.Format("ClockIn employeeId={0}, referralIds={1}", employeeId, referralIds), FromMobile, logPath, ConfigSettings.IsCaptureCallLog);
            _ivrDataProvider = new IvrDataProvider();
            ApiResponse apiResponse = _ivrDataProvider.ClockIn(employeeId, referralIds);
            var response = new VoiceResponse();

            if (apiResponse.IsSuccess)
            {
                baseDataProvider.InsertIvrLogInDatabse(FromMobile, string.Format("Thank you, clock in completed successfully. employeeId={0}, referralIds={1}", employeeId, referralIds));
                return EndTheCall(AudioFileURL + "ClockInCompleted.mp3");
                //return EndTheCall("Thank you, clock in completed successfully");
            }
            else
            {
                if ((int)apiResponse.Data == 1)
                {
                    var gather = new Gather();

                    string url = String.Format(ConfigSettings.APIUrl + "/ivr/InstantNoScheduleClockIn?employeeId={0}&referralIds={1}&CompanyName={2}&Redirect=ClockIn", employeeId, referralIds, CompanyName);
                    gather = new Gather(action: new Uri(url), numDigits: 1, timeout: 10);

                    var uri = new Uri(AudioFileURL + "NoScheduleFoundPress1ForInstantSchedule.mp3");
                    gather.Play(url: uri);
                    //gather.Say("No schedules found, Press '1' to clock in with IVR instant scheduling. Press star to go main menu.", ConfigSettings.TwilioVoice,
                    //    language: ConfigSettings.TwilioLanguage);
                    response.Append(gather);
                    response.Redirect(new Uri(url), HttpMethod.Post);
                    return TwiML(response);
                }
                else
                {
                    var uri = new Uri(AudioFileURL + "NoPermissionForInstantScheduling.mp3");
                    response.Play(url: uri);
                    baseDataProvider.InsertIvrLogInDatabse(FromMobile, string.Format("You do not have permission to clock in using instant IVR scheduling, please contact office administrator. employeeId={0}, referralIds={1}", employeeId, referralIds),isCompleted:true);

                    //response.Say("You do not have permission to clock in using instant IVR scheduling, please contact office administrator");
                    response.Hangup();
                }
                return EndTheCall(AudioFileURL + "NoScheduleFound.mp3");
                //return EndTheCall("No schedule corresponding to the patient found. Please hang up and call office administrator.");
            }
        }

        [HttpPost]
        public ActionResult IVRBypassClockIn(string employeeId)
        {
            baseDataProvider.InsertIvrLogInDatabse(FromMobile, string.Format("IVRBypassClockIn employeeId={0}", employeeId));
            Common.CreateLogFile(string.Format("IVRBypassClockIn employeeId={0}", employeeId), FromMobile, logPath, ConfigSettings.IsCaptureCallLog);
            _ivrDataProvider = new IvrDataProvider();
            ApiResponse apiResponse = _ivrDataProvider.IVRBypassClockIn(employeeId);
            var response = new VoiceResponse();
            
            if (apiResponse.IsSuccess)
            {
                return EndTheCall(AudioFileURL + "EnteredDetailsAreIncorrect.mp3");
                //return EndTheCall("Thank you, clock in completed successfully");
            }
            else
            {
                if ((int)apiResponse.Data == 1)
                {
                    var gather = new Gather();

                    string url = String.Format(ConfigSettings.APIUrl + "/ivr/GetPatientDetails?type={2}&employeeId={0}&CompanyName={1}", employeeId, CompanyName, ClockInText);
                    gather = new Gather(action: new Uri(url), numDigits: 1, timeout: 60);

                    var uri = new Uri(AudioFileURL + "NoScheduleFoundPress1ForInstantSchedule.mp3");
                    gather.Play(url: uri);
                    //gather.Say("No schedules found, Press '1' to clock in with IVR instant scheduling. Press star to go main menu.", ConfigSettings.TwilioVoice,
                    //    language: ConfigSettings.TwilioLanguage);
                    response.Append(gather);
                    return TwiML(response);
                }
                else
                {
                    var uri = new Uri(AudioFileURL + "NoPermissionForInstantScheduling.mp3");
                    response.Play(url: uri);
                    //response.Say("You do not have permission to clock in using instant IVR scheduling, please contact office administrator");
                    baseDataProvider.InsertIvrLogInDatabse(FromMobile, string.Format("You do not have permission to clock in using instant IVR scheduling, please contact office administrator. employeeId={0}",employeeId), isCompleted: true);
                    response.Hangup();
                }
                return EndTheCall(AudioFileURL + "NoScheduleFound.mp3");
                //return EndTheCall("No schedule corresponding to the patient found. Please hang up and call office administrator.");
            }
        }

        [HttpPost]
        public ActionResult ClockOut(string employeeId, string referralIds)
        {
            Common.CreateLogFile(string.Format("ClockOut employeeId={0}, referralIds={1}", employeeId, referralIds), FromMobile, logPath, ConfigSettings.IsCaptureCallLog);
            _ivrDataProvider = new IvrDataProvider();
            ApiResponse apiResponse = _ivrDataProvider.ClockOut(employeeId, referralIds);
            var response = new VoiceResponse();


            Common.CreateLogFile(string.Format("ClockOut IsSuccess={0}, apiResponse.Data={1}", apiResponse.IsSuccess, apiResponse.Data), FromMobile, logPath, ConfigSettings.IsCaptureCallLog);
            if (apiResponse.IsSuccess)
            {
                if ((int)apiResponse.Data == 1)
                {
                    baseDataProvider.InsertIvrLogInDatabse(FromMobile, string.Format("Thank you, clock out completed successfully. Finish you time sheet using my easy care mobile app at your convenience. employeeId={0}, referralIds={1}", employeeId, referralIds));
                    return EndTheCall(AudioFileURL + "ClockOutCompleted.mp3");
                    //return EndTheCall("Thank you, clock out completed successfully. Finish you time sheet using my easy care mobile app at your convenience");
                }
                else
                {
                    return EndTheCall(AudioFileURL + "NoScheduleFound.mp3");
                }
            }
            else
            {
                if ((int)apiResponse.Data == 1)
                {
                    var gather = new Gather();

                    string url = String.Format(ConfigSettings.APIUrl + "/ivr/InstantNoScheduleClockOut?digits=1&employeeId={0}&referralIds={1}&CompanyName={2}", employeeId,referralIds, CompanyName);
                    response.Redirect(new Uri(url), HttpMethod.Post);

                    return TwiML(response);
                }
                else
                {
                    var uri = new Uri(AudioFileURL + "NoPermissionForInstantScheduling.mp3");
                    response.Play(url: uri);
                    //response.Say("You do not have permission to clock in using instant IVR scheduling, please contact office administrator");
                    baseDataProvider.InsertIvrLogInDatabse(FromMobile, string.Format("You do not have permission to clock in using instant IVR scheduling, please contact office administrator. employeeId={0}, referralIds={1}", employeeId,referralIds), isCompleted: true);
                    response.Hangup();
                }
                return EndTheCall(AudioFileURL + "NoScheduleFound.mp3");
                //return EndTheCall("No schedule corresponding to the patient found. Please hang up and call office administrator.");
            }

        }

        [HttpPost]
        public ActionResult IVRBypassClockOut(string employeeId)
        {
            baseDataProvider.InsertIvrLogInDatabse(FromMobile, string.Format("IVRBypassClockOut employeeId={0}", employeeId));
            Common.CreateLogFile(string.Format("IVRBypassClockOut employeeId={0}", employeeId), FromMobile, logPath, ConfigSettings.IsCaptureCallLog);
            _ivrDataProvider = new IvrDataProvider();
            ApiResponse apiResponse = _ivrDataProvider.IVRBypassClockOut(employeeId);
            var response = new VoiceResponse();

            if (apiResponse.IsSuccess)
            {
                return EndTheCall(AudioFileURL + "ClockOutCompleted.mp3");
                //return EndTheCall("You have successfully clocked out. Please make sure to complete the time using the my easy care mobile app.");
            }
            else
            {
                if ((int)apiResponse.Data == 1) // Has Permissions
                {
                    var gather = new Gather();
                    string url = String.Format(ConfigSettings.APIUrl + "/ivr/GetPatientDetails?type={2}&employeeId={0}&CompanyName={1}", employeeId, CompanyName, ClockOutText);
                    
                    response.Redirect(new Uri(url), HttpMethod.Post);
                    return TwiML(response);
                }
                else
                {
                    var uri = new Uri(AudioFileURL + "NoPermissionForInstantScheduling.mp3");
                    response.Play(url: uri);
                    //response.Say("You do not have permission to clock in using instant IVR scheduling, please contact office administrator");
                    baseDataProvider.InsertIvrLogInDatabse(FromMobile, string.Format("You do not have permission to clock in using instant IVR scheduling, please contact office administrator. employeeId={0}", employeeId), isCompleted: true);
                    response.Hangup();
                }
                return EndTheCall(AudioFileURL + "NoScheduleFound.mp3");
                //return EndTheCall("No schedule corresponding to the patient found. Please hang up and call office administrator.");
            }
        }



        private TwiMLResult EndTheCall(string message)
        {
            var response = new VoiceResponse();

            baseDataProvider.InsertIvrLogInDatabse(FromMobile, string.Format("EndTheCall message={0}", message),isCompleted:true);
            var uri= new Uri(message);
            response.Play(url: uri);

            uri = new Uri(AudioFileURL + "EndCallSpeech.mp3");
            response.Play(url:uri);
            //response.Say(message + " Thank you for using My easy care IVR clock in and clock out system",
            //    voice: ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage);
            response.Hangup();
            return TwiML(response);
        }

        [HttpPost]
        public ActionResult ThankYou(string digits)
        {
            var response = new VoiceResponse();
            var gather = new Gather();

            var uri = new Uri(AudioFileURL + "SuccesfullyClockedIn.mp3");
            response.Play(url: uri);
            //gather.Say("Thanks you are successfully clocked in",
            //    voice: ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage).Play(new System.Uri("http://www.hubharp.com/web_sound/BachGavotteShort.mp3"));
            response.Append(gather);
            return TwiML(response);
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

                gather.Say("Please enter patient account number.", ConfigSettings.TwilioVoice,
                    language: ConfigSettings.TwilioLanguage);
                response.Append(gather);
                return TwiML(response);
            }
            return EndTheCall(AudioFileURL + "ThankYouForCalling.mp3");
            //return EndTheCall("Thank you for the calling.");
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
                return TwiML(response);
            }

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
        public ActionResult InstantNoScheduleClockIn(string digits, string employeeId, string referralIds,string Redirect)
        {
            var response = new VoiceResponse();

            baseDataProvider.InsertIvrLogInDatabse(FromMobile, string.Format("No schedules found, Press '1' to clock in with IVR instant scheduling. Press star to go main menu. Input={0},employeeId={1},referralIds={2},Redirect={3}", digits, employeeId, referralIds, Redirect));

            if (string.IsNullOrEmpty(digits))
            {
                if (Redirect == "ClockIn")
                {
                    string url = String.Format(ConfigSettings.APIUrl + "/ivr/clockin?employeeId={0}&referralIds={1}&CompanyName={2}", employeeId,referralIds,CompanyName);
                    response.Redirect(new Uri(url), HttpMethod.Post);
                    return TwiML(response);
                }
            }

            if (referralIds.Contains(","))
            {

                string url = String.Format(ConfigSettings.APIUrl + "/ivr/verifypatientforinstantschedule?count={0}&employeeId={1}&CompanyName={2}&referralIds={3}", 0,employeeId, CompanyName,referralIds);
                var gather = new Gather(action: new Uri(url), finishOnKey: "#", timeout: 10);

                var uri = new Uri(AudioFileURL + "FoundMultiplePatients.mp3");
                gather.Play(url: uri);
                //gather.Say("We have found multiple patients. Please enter patient's account number and press # to submit.",
                //    voice: ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage);
                response.Append(gather);
                response.Redirect(new Uri(url), HttpMethod.Post);

                return TwiML(response);
            }
            else
            {

                if (digits == "1")
                {
                    _ivrDataProvider = new IvrDataProvider();
                    ApiResponse apiResponse = _ivrDataProvider.CreatePendingScheduleClockInOut(employeeId, referralIds, true);
                    if (apiResponse.IsSuccess)
                        return EndTheCall(AudioFileURL + "ClockInCompleted.mp3");
                        //return EndTheCall("Your clock in is apply successfully.");
                    else
                        return EndTheCall(AudioFileURL + "ThereAreSomeProblem.mp3");
                        //return EndTheCall("There are some problem, please try later. Thank you for the calling.");
                }
                else if (digits == "*")
                {
                    string url = String.Format(ConfigSettings.APIUrl + "ivr/welcome?CompanyName={0}&digits={1}", CompanyName, SelectedLanguage);
                    response.Redirect(new Uri(url), HttpMethod.Post);
                    return TwiML(response);
                }
                return EndTheCall(AudioFileURL + "ThankYouForCalling.mp3");
                //return EndTheCall("Thank you for the calling.");
            }
        }


        [HttpPost]
        public ActionResult VerifyPatientForInstantSchedule(string digits, int count,string employeeId, string referralIds=null)
        {
            var response = new VoiceResponse();

            if (string.IsNullOrEmpty(digits))
            {
                string url = String.Format(ConfigSettings.APIUrl + "/ivr/InstantNoScheduleClockIn?digits=1&employeeId={0}&referralIds={1}&CompanyName={2}",employeeId, referralIds, CompanyName);
                response.Redirect(new Uri(url), HttpMethod.Post);
                return TwiML(response);
            }

            baseDataProvider.InsertIvrLogInDatabse(FromMobile, string.Format("We have found multiple patients. Please enter patient's account number and press # to submit. Input={0},count={1},employeeId={2},referralIds={3}", digits,count, employeeId, referralIds));
            Common.CreateLogFile(string.Format("VerifyPatientForInstantSchedule digits={0}, count={1}, employeeId={2}", digits, count,employeeId), "IvrLog", logPath, ConfigSettings.IsCaptureCallLog);

            _ivrDataProvider = new IvrDataProvider();
            ApiResponse apiResponse = _ivrDataProvider.VerifyPatient(digits);
            var referralId = Convert.ToString(apiResponse.Data);
            //int count = 0;

            referralId = string.IsNullOrEmpty(referralId) ? "0" : referralId;

            if (Convert.ToInt64(referralId) > 0)
            {
                //set referralID for Instant Clockout
                referralIdForInstantSchedule = referralId;
                string url = String.Format(ConfigSettings.APIUrl + "/ivr/InstantNoScheduleClockIn?digits=1&employeeId={0}&referralIds={1}&CompanyName={2}", employeeId, referralId, CompanyName);
                response.Redirect(new Uri(url), HttpMethod.Post);
                return TwiML(response);
            }
            else
            {
                count = count + 1;
                if (count < 3)
                {
                    string url = String.Format(ConfigSettings.APIUrl + "/ivr/verifypatientforinstantschedule?count={0}&employeeId={1}&CompanyName={2}", count, employeeId, CompanyName);
                    var gather = new Gather(action: new Uri(url), finishOnKey: "#", timeout: 60);

                    var uri = new Uri(AudioFileURL + "AccountNumberNotFound.mp3");
                    gather.Play(url: uri);
                    //gather.Say("Account number not found, please try again.",
                    //    voice: ConfigSettings.TwilioVoice, language: ConfigSettings.TwilioLanguage);
                    response.Append(gather);
                    string url1 = String.Format(ConfigSettings.APIUrl + "/ivr/verifypatientforinstantschedule?count={0}&employeeId={1}&CompanyName={2}&digits={3}", count-1, employeeId, CompanyName, digits);
                    response.Redirect(new Uri(url1), HttpMethod.Post);

                    return TwiML(response);
                }
                else
                {
                    return EndTheCall(AudioFileURL + "MaximumAttemptReachedCallAgain.mp3");
                }

            }

            //return TwiML(response);
        }



        [HttpPost]
        public ActionResult InstantNoScheduleClockOut(string digits, string employeeId, string referralIds)
        {
            var response = new VoiceResponse();
            if (referralIds.Contains(","))
            {
                string url = String.Format(ConfigSettings.APIUrl + "/ivr/InstantNoScheduleClockOut?digits=1&employeeId={0}&referralIds={1}&CompanyName={2}", employeeId, referralIdForInstantSchedule, CompanyName);
                response.Redirect(new Uri(url), HttpMethod.Post);
                return TwiML(response);
            }
            else
            {
                if (digits == "1")
                {
                    _ivrDataProvider = new IvrDataProvider();
                    ApiResponse apiResponse = _ivrDataProvider.CreatePendingScheduleClockInOut(employeeId, referralIds, false);
                    if (apiResponse.IsSuccess)
                        return EndTheCall(AudioFileURL + "ClockOutCompleted.mp3");
                        //return EndTheCall("Your clock out is apply successfully.");
                    else
                        return EndTheCall(AudioFileURL + "ThereAreSomeProblem.mp3");
                        //return EndTheCall("There are some problem, please try later. Thank you for the calling.");
                }
                return EndTheCall(AudioFileURL + "ThankYouForCalling.mp3");
                //return EndTheCall("Thank you for the calling.");
            }
        }




        #endregion
    }
}