using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Configuration;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Xml;
using Newtonsoft.Json;
using Myezcare_Admin.Helpers;
using Myezcare_Admin.Infrastructure.DataProvider;
using Myezcare_Admin.Models;
using Formatting = Newtonsoft.Json.Formatting;
using HttpResponse = System.Web.HttpResponse;
using System.Data;
//using Zarephath.Core.Infrastructure.GetLatLongByAddress;
using System.Globalization;
using System.Security.Cryptography;
using System.Drawing;
using System.Net.Http;
using System.Web.Http;
using System.ComponentModel.DataAnnotations;
using Myezcare_Admin.Resources;
using Myezcare_Admin.Models.ViewModel;
using Myezcare_Admin.Helpers;

namespace Myezcare_Admin.Infrastructure
{
    public class Common
    {


        public static string CatchType_Organization = "1";
        public static string CatchType_ReleaseNote = "2";

        public static ServiceResponse UpdateCache(string siteName = "", string catchType = "")
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                BaseDataProvider dataProvider = new BaseDataProvider();
                string url = string.Empty;

                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "OrganizationName", Value = siteName });
                MyEzcareOrganizationModel myEzcareOrganizationModel = dataProvider.GetMultipleEntity<MyEzcareOrganizationModel>(StoredProcedure.GetOrganizationData, searchList);
                if (myEzcareOrganizationModel.MyEzcareOrganization != null)
                {
                    url = string.Format(ConfigSettings.PublicSiteUrl + ConfigSettings.UpdateSiteCacheUrl + catchType, myEzcareOrganizationModel.MyEzcareOrganization.DomainName);
                }
                using (WebClient client = new WebClient())
                {
                    var reqparm = new System.Collections.Specialized.NameValueCollection();
                    byte[] responsebytes = client.UploadValues(url, "POST", reqparm);
                    string responsebody = Encoding.UTF8.GetString(responsebytes);
                    response = DeserializeObject<ServiceResponse>(responsebody);
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }


        public static void GetSubDomain()
        {
            string domain = HttpContext.Current.Request.Url.DnsSafeHost;

            if (domain.Contains("www"))
                domain = domain.Replace("www", "").Trim();

            var subdomain = "";
            if (!string.IsNullOrEmpty(domain))
            {
                var nodes = domain.Split('.');
                subdomain = nodes[0];
            }

            SessionHelper.DomainName = subdomain;

            //var data = ConfigurationManager.ConnectionStrings[subdomain];
            //if (data != null)
            //{
            //    //if (subdomain == "localhost")
            //    //    subdomain = "localhost:5555";
            //    SessionHelper.DomainName = subdomain;
            //}
            //else
            //{
            //    SessionHelper.DomainName = Resource.NotFound;
            //}
        }

        public static string GetSubDomainName()
        {

            string domain = HttpContext.Current.Request.Url.DnsSafeHost;

            if (domain.Contains("www"))
                domain = domain.Replace("www", "").Trim();

            var subdomain = "";
            if (!string.IsNullOrEmpty(domain))
            {
                var nodes = domain.Split('.');
                subdomain = nodes[0];
            }

            return subdomain;
        }

        public static void ThrowErrorMessage(string Message, HttpStatusCode code = HttpStatusCode.NotAcceptable)
        {
            var resp = new HttpResponseMessage(code)
            {
                ReasonPhrase = Message
            };

            resp.Content = new StringContent(SerializeObject(new ApiServiceResponse
            {
                Code = Constants.StatusCode401,
                Message = Message
            }));


            throw new HttpResponseException(resp);
        }

        public static DateTime GetUtcTimeFromDate(DateTime date)
        {
            //TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time").

            DateTime.SpecifyKind(date, DateTimeKind.Unspecified);

            var UtcDateTime = TimeZoneInfo.ConvertTimeToUtc(date, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")); // 1st param is the date to convert and the second is its TimeZone
            return UtcDateTime;
        }

        public static string GetRandomColor(string value)
        {
            //SystemExceptionm.Dra Color.FromArgb(guid.GetHashCode());


            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(value));
            var c = Color.FromArgb(hash[0], hash[1], hash[2]);
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        public static DateTime GetLocalFromUtc(DateTime value)
        {
            value = DateTime.SpecifyKind((DateTime)value, DateTimeKind.Utc).ToLocalTime();
            return value;
        }

        public static string MessageWithTitle(string title, string messaage)
        {
            return String.Format("<div><h4>{0}</h4><p>{1}</p></div>", title, messaage);
        }

        public static string ReturnCsvString(List<string> strings)
        {
            return "1";// String.Join(",", strings);
        }

        public static bool HasPermission(string permission)
        {
            var strPermissions = string.IsNullOrEmpty(permission) ? new string[] { } : permission.Split(',');
            foreach (var tempPermission in strPermissions)
            {
                var flag = SessionHelper.Permissions != null && SessionHelper.Permissions.Any(m => m.PermissionID.ToString() == tempPermission);
                if (flag)
                    return true;
            }
            return false;
            //return SessionHelper.Permissions != null ? SessionHelper.Permissions.Any(m => m.PermissionID.ToString() == permission) : false;
        }

        public enum SetValue
        {
            CurrentTime = 1, LoggedInUserId
        }

        public enum GeneralMasterEnum
        {
            ServicePlanComponents = 1
        }

        public enum ImportTypeEnum
        {
            Patient = 1,
            Employee
        }

        public enum SearchOperator
        {
            EqualTo = 1, NotEqualTo, BeginsWith, EndsWith, Contains, DoesNotContains, GreaterThan, LessThan
        }

        public enum ServicePlanModuleEnum
        {
            [RequiredText(Name = "HashPatientsRequired", ResourceType = typeof(Resource))]
            [HelpText(Name = "SPPatientHelpText", ResourceType = typeof(Resource))]
            [Display(Name = "HashPatients", ResourceType = typeof(Resource))]
            Patient = 1,
            [RequiredText(Name = "HashFacilitiesRequired", ResourceType = typeof(Resource))]
            [HelpText(Name = "SPFacilityHelpText", ResourceType = typeof(Resource))]
            [Display(Name = "HashFacilities", ResourceType = typeof(Resource))]
            Facility = 2,
            [RequiredText(Name = "HashTasksRequired", ResourceType = typeof(Resource))]
            [HelpText(Name = "SPTasksHelpText", ResourceType = typeof(Resource))]
            [Display(Name = "HashTasks", ResourceType = typeof(Resource))]
            Task = 3,
            [RequiredText(Name = "HashEmployeesRequired", ResourceType = typeof(Resource))]
            [HelpText(Name = "SPEmployeeHelpText", ResourceType = typeof(Resource))]
            [Display(Name = "HashEmployees", ResourceType = typeof(Resource))]
            Employee = 4,
            [RequiredText(Name = "PercentBillingRequired", ResourceType = typeof(Resource))]
            [HelpText(Name = "SPBillingHelpText", ResourceType = typeof(Resource))]
            [Display(Name = "PercentBilling", ResourceType = typeof(Resource))]
            Billing = 5
        }

        public static string SerializeObject<T>(T objectData)
        {
            string defaultJson = JsonConvert.SerializeObject(objectData);
            return defaultJson;
        }

        public string SerializePartialObject<T>(T objectData)
        {
            string defaultJson = JsonConvert.SerializeObject(objectData, Formatting.Indented,
                                  new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return defaultJson;
        }

        public static T DeserializeObject<T>(string json)
        {
            T obj = default(T);
            obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        public static string CreateLogFile(string message, string logFileName = "", string logPath = "")
        {
            string virtualPath = logPath;
            if (String.IsNullOrEmpty(logFileName))
                logFileName = "Exception" + DateTime.Today.ToString("MMddyyyy") + ".txt";

            if (String.IsNullOrEmpty(logPath))
            {
                if (HttpContext.Current != null)
                {
                    logPath = HttpContext.Current.Server.MapPath(ConfigSettings.LogPath);
                    virtualPath = ConfigSettings.LogPath;
                }
                else
                {
                    logPath = AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["Logpath"];
                    virtualPath = ConfigurationManager.AppSettings["Logpath"];
                }
            }
            else
            {
                if (HttpContext.Current != null)
                    logPath = HttpContext.Current.Server.MapPath(logPath);
                else
                    logPath = AppDomain.CurrentDomain.BaseDirectory + logPath;
            }

            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }

            string fileName = logFileName;
            string fileFullPath = logPath + fileName;
            virtualPath = virtualPath + fileName;
            var sr = new StreamWriter(fileFullPath, true);
            sr.WriteLine("DateTime:" + DateTime.Now);
            sr.WriteLine(message);
            sr.WriteLine("===============================================================================================================");
            sr.Flush();
            sr.Close();

            return virtualPath;
        }

        //public static bool IsMatches(string password, string passwordSalt, string oldHashedPassword)
        //{
        //    // Pass a logRounds parameter to GenerateSalt to explicitly specify the
        //    // amount of resources required to check the password. The work factor
        //    // increases exponentially, so each increment is twice as much work. If
        //    // omitted, a default of 10 is used.

        //    string hashed = "";
        //    try
        //    {
        //        hashed = BCrypt.Net.BCrypt.HashPassword(password, passwordSalt);
        //    }
        //    catch (Exception)
        //    {

        //    }

        //    return oldHashedPassword == hashed;
        //}

        //public static PasswordDetail CreatePassword(string password)
        //{
        //    // Pass a logRounds parameter to GenerateSalt to explicitly specify the
        //    // amount of resources required to check the password. The work factor
        //    // increases exponentially, so each increment is twice as much work. If
        //    // omitted, a default of 10 is used.
        //    string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
        //    string hashedPasssword = BCrypt.Net.BCrypt.HashPassword(password, salt);

        //    PasswordDetail passwordDetail = new PasswordDetail { PasswordSalt = salt, Password = hashedPasssword };

        //    return passwordDetail;
        //}

        #region SEND EMAIL / SMS

        public static bool SendEmail(string mailSubject, string fromEmail, string toEmails, string messageBody, string EmailType = "", string ccemailaddress = null, int smtpSettingNumer = (int)EmailHelper.SMTPSetting.GeneralEmailSetting, List<string> lstAttachments = null)
        {
            //toEmails = "asavaliya@kairasoftware.com;radeshara@kairasoftware.com;Brad@zrpath.com;support@kairasoftware.com";
            BaseDataProvider dataProvider = new BaseDataProvider();
            MailMessage contactEmail = new MailMessage();

            Configuration configurationFile;
            if (HttpContext.Current != null)
            {
                configurationFile =
                WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            }
            else
            {
                configurationFile =
                ConfigurationManager.OpenExeConfiguration(Assembly.GetEntryAssembly().Location);
            }

            MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;

            if (mailSettings != null)
                contactEmail.SmtpHost = mailSettings.Smtp.Network.Host;

            contactEmail.IsHtml = true;
            var toEmailArray = new List<string>(toEmails.Split(';'));
            foreach (var toEmail in toEmailArray)
                contactEmail.ToList.Add(toEmail.Trim());

            if (String.IsNullOrEmpty(fromEmail))
                contactEmail.From = mailSettings == null ? "" : mailSettings.Smtp.Network.UserName;
            else
                contactEmail.From = fromEmail;

            if (!string.IsNullOrEmpty(ccemailaddress))
            {
                var ccEmailArray = new List<string>(ccemailaddress.Split(';'));
                foreach (var ccEmail in ccEmailArray)
                    contactEmail.CcList.Add(ccEmail.Trim());
            }

            //contactEmail.CcList = ;
            contactEmail.Subject = mailSubject;
            contactEmail.Body = messageBody;

            contactEmail.AttachmentList = lstAttachments;

            try
            {
                bool isSent = EmailHelper.SendEmail(contactEmail, smtpSettingNumer);
                InsertEntryInEmailHistoryLogs(mailSubject, toEmails, messageBody, isSent, dataProvider, EmailType);
                return isSent;
            }
            catch (Exception ex)
            {
                InsertEntryInEmailHistoryLogs(mailSubject, toEmails, messageBody, false, dataProvider, EmailType);
                CreateLogFile(ex.Message);
                return false;
            }

        }

        //public static string SendTwilioNotification(string message, List<string> binding)
        //{
        //    CacheHelper _cacheHelper = new CacheHelper();
        //    //string accountSid = ConfigSettings.TwilioAccountSID;
        //    //string authToken = ConfigSettings.TwilioAuthToken;
        //    //string serviceSid = ConfigSettings.TwilioServiceSID;

        //    string accountSid = _cacheHelper.TwilioAccountSID;
        //    string authToken = _cacheHelper.TwilioAuthToken;
        //    string serviceSid = _cacheHelper.TwilioServiceSID;



        //    TwilioClient.Init(accountSid, authToken);

        //    var notification = Twilio.Rest.Notify.V1.Service.NotificationResource.Create(
        //        serviceSid,
        //        toBinding: binding,
        //        body: message);

        //    return notification.Sid;
        //}


        //public static bool SendSms(string toNumber, string body, string emailType)
        //{
        //    CacheHelper _cacheHelper = new CacheHelper();


        //    BaseDataProvider dataProvider = new BaseDataProvider();
        //    if (string.IsNullOrEmpty(_cacheHelper.TwilioAccountSID) || string.IsNullOrEmpty(_cacheHelper.TwilioAuthToken)
        //        || string.IsNullOrEmpty(_cacheHelper.TwilioServiceSID) || string.IsNullOrEmpty(_cacheHelper.TwilioFromNo)
        //        || string.IsNullOrEmpty(_cacheHelper.TwilioDefaultCountryCode) )
        //    {
        //        InsertEntryForSmsInEmailHistoryLogs(toNumber, body, false, dataProvider, emailType);
        //        CreateLogFile("Sms Settings are not found.");
        //        return false;
        //    }






        //    // Find your Account Sid and Auth Token at twilio.com/user/account

        //    //string AccountSid = ConfigSettings.TwilioAccountSID;
        //    // string AuthToken = ConfigSettings.TwilioAuthToken;
        //    //var twilio = new TwilioRestClient(AccountSid, AuthToken);

        //   // TwilioClient.Init(ConfigSettings.TwilioAccountSID, ConfigSettings.TwilioAuthToken);
        //    TwilioClient.Init(_cacheHelper.TwilioAccountSID, _cacheHelper.TwilioAuthToken);


        //    try
        //    {
        //        // var message = twilio.SendMessage(ConfigSettings.TwilioFromNo, ConfigSettings.DefaultCountryCodeForSms + toNumber, body);
        //        string countryCode = _cacheHelper.TwilioDefaultCountryCode;//string.IsNullOrEmpty(_cacheHelper.TwilioDefaultCountryCode) ? Constants.DefaultCountryCodeForSms : _cacheHelper.TwilioDefaultCountryCode;
        //       var message = MessageResource.Create(
        //        //ConfigSettings.DefaultCountryCodeForSms + toNumber,
        //        countryCode + toNumber,
        //       from: new PhoneNumber(_cacheHelper.TwilioFromNo),
        //       body: body);

        //        if (!string.IsNullOrEmpty(message.Sid))
        //        {
        //            InsertEntryForSmsInEmailHistoryLogs(toNumber, body, true, dataProvider, emailType);
        //            return true;
        //        }
        //        InsertEntryForSmsInEmailHistoryLogs(toNumber, body, false, dataProvider, emailType);
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        InsertEntryForSmsInEmailHistoryLogs(toNumber, body, false, dataProvider, emailType);
        //        CreateLogFile(ex.Message);
        //        return false;
        //    }
        //}



        private static void InsertEntryInEmailHistoryLogs(string mailSubject, string toEmails, string messageBody, bool isSent, BaseDataProvider dataProvider, string EmailType)
        {
            var email = new EmailHistoryLog
            {
                ToEmail = toEmails,
                Body = messageBody,
                Subject = mailSubject,
                IsSent = isSent,
                EmailType = EmailType,
                CreatedDate = DateTime.UtcNow
            };

            dataProvider.SaveEntity(email);

        }

        //private static void InsertEntryForSmsInEmailHistoryLogs(string toPhoneNumber, string messageBody, bool isSent, BaseDataProvider dataProvider, string EmailType)
        //{
        //    var email = new EmailHistoryLog
        //    {
        //        ToPhoneNo = toPhoneNumber,
        //        Body = messageBody,
        //        IsSent = isSent,
        //        EmailType = EmailType,
        //        CreatedDate = DateTime.UtcNow
        //    };

        //    dataProvider.SaveEntity(email);
        //}

        #endregion

        //public static void EmployeeLoginLogs(long employeeId, bool actionTypeLogin = true, string actionPlatform = null)
        //{
        //    BaseDataProvider dataProvider = new BaseDataProvider();

        //    EmployeeLoginDetail employeeLoginDetail = new EmployeeLoginDetail
        //    {
        //        EmployeeID = employeeId,
        //        LoginTime = DateTime.UtcNow,
        //        ActionType = actionTypeLogin ? EmployeeLoginDetail.LoginActionType.Login.ToString() : EmployeeLoginDetail.LoginActionType.Logout.ToString(),
        //        ActionPlatform = !string.IsNullOrEmpty(actionPlatform) ? actionPlatform : EmployeeLoginDetail.LoginActionPlatform.Web.ToString(),
        //    };

        //    dataProvider.SaveObject(employeeLoginDetail, employeeId);
        //}

        public static List<SearchValueData> SetPagerValues(int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            List<SearchValueData> searchList = new List<SearchValueData>();

            var searchValueData = new SearchValueData { Name = "SortExpression", Value = !string.IsNullOrEmpty(sortIndex) ? Convert.ToString(sortIndex) : null };
            searchList.Add(searchValueData);

            searchValueData = new SearchValueData { Name = "SortType", Value = !string.IsNullOrEmpty(sortDirection) ? Convert.ToString(sortDirection) : null };
            searchList.Add(searchValueData);

            searchValueData = new SearchValueData { Name = "FromIndex", Value = Convert.ToString(pageIndex) };
            searchList.Add(searchValueData);

            searchValueData = new SearchValueData { Name = "PageSize", Value = Convert.ToString(pageSize) };
            searchList.Add(searchValueData);

            return searchList;
        }

        public static DataTable ListToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public enum DepartmentSupervisorStatusEnum
        {
            All = -1,
            Yes = 1,
            No = 0
        }

        public enum YesNoAllEnum
        {
            All = -1,
            Yes = 1,
            No = 0
        }

        public enum ReferralService
        {
            All = -1,
            Respite,
            Life_Skills,
            Counselling,
            ConnectingFamilies
        }

        public enum Gender
        {
            Male = 1,
            Female
        }

        public enum ROITypes
        {
            Verbal = 1,
            Written
        }

        public enum ReferralStatusEnum
        {
            Active = 1,
            ProBono,
            Inactive,
            Discharged,
            NewReferral,
            IncompleteReferral,
            InactiveReferral,
            DormantReferral,
            LifeSkillsOnly,
            ReferralAccepted,
            ReferralInitialReview,
            ReferralOnHold,
            ReferralDenied,
            ConnectingFamilies
        }

        public enum YesNoEnum
        {
            Yes = 1,
            No = 0
        }

        public enum AssignedMeEnum
        {
            All = -1,
            AssignedMe = 1
        }

        public static List<NameValueData> SetYesNoList()
        {
            return new List<NameValueData>
                {
                    new NameValueData { Name = Constants.Yes, Value = (long) YesNoEnum.Yes},
                    new NameValueData { Name = Constants.No, Value = (long)YesNoEnum.No },
                };
        }

        public static List<NameValueData> SetYesNoForOMList()
        {
            return new List<NameValueData>
                {
                    new NameValueData { Name = Constants.Yes, Value = 5},
                    new NameValueData { Name = Constants.No, Value = 1 },
                };
        }

        public static List<NameValueDataInString> GetBlockingRequestedBy()
        {
            return new List<NameValueDataInString>
                {
                    new NameValueDataInString { Name = Resource.Employee, Value = Resource.Employee},
                    new NameValueDataInString { Name = Resource.Patient, Value = Resource.Patient },
                };
        }

        public static List<NameValueDataInString> SetYesNoStringList()
        {
            return new List<NameValueDataInString>
                {
                    new NameValueDataInString { Name = Resource.Yes, Value = Constants.CapsYes},
                    new NameValueDataInString { Name = Resource.No, Value = Constants.CapsNo },
                };
        }

        public static List<NameValueDataBoolean> SetYesNoListForBoolean()
        {
            return new List<NameValueDataBoolean>
                {
                    new NameValueDataBoolean { Name = Constants.Yes, Value = "true" },
                    new NameValueDataBoolean { Name = Constants.No, Value = "false" },
                };
        }

        public static List<NameValueDataBoolean> SetTrueFalseListForBoolean()
        {
            return new List<NameValueDataBoolean>
                {
                    new NameValueDataBoolean { Name = Resource.True, Value = "true" },
                    new NameValueDataBoolean { Name = Resource.False, Value = "false" },
                };
        }

        public static List<NameValueDataBoolean> GetTwilioCountryCodes()
        {
            return new List<NameValueDataBoolean>
                {
                    new NameValueDataBoolean { Name = "US", Value = "US" },
                    new NameValueDataBoolean { Name = "AL", Value = "AL" },
                };
        }

        public static List<NameValueDataInString> SetNameValueDataForYesNoNAData()
        {
            return new List<NameValueDataInString>
                {
                    new NameValueDataInString { Name = Constants.Yes, Value = "Y"},
                    new NameValueDataInString { Name = Constants.No, Value = "N" },
                    new NameValueDataInString { Name = Constants.NotApplicable, Value = "NA" },
                };
        }

        public static List<NameValueDataInString> SetTimeZoneList()
        {
            return new List<NameValueDataInString>
                {
                    new NameValueDataInString {Name="(UTC) Casablanca", Value="Morocco Standard Time" },
                    new NameValueDataInString {Name="(UTC) Greenwich Mean Time : Dublin, Edinburgh, Lisbon, London", Value="GMT Standard Time" },
                    new NameValueDataInString {Name="(UTC) Monrovia, Reykjavik", Value="Greenwich Standard Time"},
                    new NameValueDataInString {Name="(UTC+01:00) Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna", Value="W. Europe Standard Time"},
                    new NameValueDataInString {Name="(UTC+01:00) Belgrade, Bratislava, Budapest, Ljubljana, Prague", Value="Central Europe Standard Time"},
                    new NameValueDataInString {Name="(UTC+01:00) Brussels, Copenhagen, Madrid, Paris", Value="Romance Standard Time"},
                    new NameValueDataInString {Name="(UTC+01:00) Sarajevo, Skopje, Warsaw, Zagreb", Value="Central European Standard Time"},
                    new NameValueDataInString {Name="(UTC+01:00) West Central Africa", Value="W. Central Africa Standard Time"},
                    new NameValueDataInString {Name="(UTC+02:00) Amman", Value="Jordan Standard Time"},
                    new NameValueDataInString {Name="(UTC+02:00) Athens, Bucharest, Istanbul", Value="GTB Standard Time"},
                    new NameValueDataInString {Name="(UTC+02:00) Beirut", Value="Middle East Standard Time"},
                    new NameValueDataInString {Name="(UTC+02:00) Cairo", Value="Egypt Standard Time"},
                    new NameValueDataInString {Name="(UTC+02:00) Harare, Pretoria", Value="South Africa Standard Time"},
                    new NameValueDataInString {Name="(UTC+02:00) Helsinki, Kyiv, Riga, Sofia, Tallinn, Vilnius", Value="FLE Standard Time"},
                    new NameValueDataInString {Name="(UTC+02:00) Jerusalem", Value="Israel Standard Time"},
                    new NameValueDataInString {Name="(UTC+02:00) Minsk", Value="E. Europe Standard Time"},
                    new NameValueDataInString {Name="(UTC+02:00) Windhoek", Value="Namibia Standard Time"},
                    new NameValueDataInString {Name="(UTC+03:00) Baghdad", Value="Arabic Standard Time"},
                    new NameValueDataInString {Name="(UTC+03:00) Kuwait, Riyadh", Value="Arab Standard Time"},
                    new NameValueDataInString {Name="(UTC+03:00) Moscow, St. Petersburg, Volgograd", Value="Russian Standard Time"},
                    new NameValueDataInString {Name="(UTC+03:00) Nairobi", Value="E. Africa Standard Time"},
                    new NameValueDataInString {Name="(UTC+03:00) Tbilisi", Value="Georgian Standard Time"},
                    new NameValueDataInString {Name="(UTC+03:30) Tehran", Value="Iran Standard Time"},
                    new NameValueDataInString {Name="(UTC+04:00) Abu Dhabi, Muscat", Value="Arabian Standard Time"},
                    new NameValueDataInString {Name="(UTC+04:00) Baku", Value="Azerbaijan Standard Time"},
                    new NameValueDataInString {Name="(UTC+04:00) Port Louis", Value="Mauritius Standard Time"},
                    new NameValueDataInString {Name="(UTC+04:00) Yerevan", Value="Caucasus Standard Time"},
                    new NameValueDataInString {Name="(UTC+04:30) Kabul", Value="Afghanistan Standard Time"},
                    new NameValueDataInString {Name="(UTC+05:00) Ekaterinburg", Value="Ekaterinburg Standard Time"},
                    new NameValueDataInString {Name="(UTC+05:00) Islamabad, Karachi", Value="Pakistan Standard Time"},
                    new NameValueDataInString {Name="(UTC+05:00) Tashkent", Value="West Asia Standard Time"},
                    new NameValueDataInString {Name="(UTC+05:30) Chennai, Kolkata, Mumbai, New Delhi", Value="India Standard Time"},
                    new NameValueDataInString {Name="(UTC+05:30) Sri Jayawardenepura", Value="Sri Lanka Standard Time"},
                    new NameValueDataInString {Name="(UTC+05:45) Kathmandu", Value="Nepal Standard Time"},
                    new NameValueDataInString {Name="(UTC+06:00) Almaty, Novosibirsk", Value="N. Central Asia Standard Time"},
                    new NameValueDataInString {Name="(UTC+06:00) Astana, Dhaka", Value="Central Asia Standard Time"},
                    new NameValueDataInString {Name="(UTC+06:30) Yangon (Rangoon)", Value="Myanmar Standard Time"},
                    new NameValueDataInString {Name="(UTC+07:00) Bangkok, Hanoi, Jakarta", Value="SE Asia Standard Time"},
                    new NameValueDataInString {Name="(UTC+07:00) Krasnoyarsk", Value="North Asia Standard Time"},
                    new NameValueDataInString {Name="(UTC+08:00) Beijing, Chongqing, Hong Kong, Urumqi", Value="China Standard Time"},
                    new NameValueDataInString {Name="(UTC+08:00) Irkutsk, Ulaan Bataar", Value="North Asia East Standard Time"},
                    new NameValueDataInString {Name="(UTC+08:00) Kuala Lumpur, Singapore", Value="Singapore Standard Time"},
                    new NameValueDataInString {Name="(UTC+08:00) Perth", Value="W. Australia Standard Time"},
                    new NameValueDataInString {Name="(UTC+08:00) Taipei", Value="Taipei Standard Time"},
                    new NameValueDataInString {Name="(UTC+09:00) Osaka, Sapporo, Tokyo", Value="Tokyo Standard Time"},
                    new NameValueDataInString {Name="(UTC+09:00) Seoul", Value="Korea Standard Time"},
                    new NameValueDataInString {Name="(UTC+09:00) Yakutsk", Value="Yakutsk Standard Time"},
                    new NameValueDataInString {Name="(UTC+09:30) Adelaide", Value="Cen. Australia Standard Time"},
                    new NameValueDataInString {Name="(UTC+09:30) Darwin", Value="AUS Central Standard Time"},
                    new NameValueDataInString {Name="(UTC+10:00) Brisbane", Value="E. Australia Standard Time"},
                    new NameValueDataInString {Name="(UTC+10:00) Canberra, Melbourne, Sydney", Value="AUS Eastern Standard Time"},
                    new NameValueDataInString {Name="(UTC+10:00) Guam, Port Moresby", Value="West Pacific Standard Time"},
                    new NameValueDataInString {Name="(UTC+10:00) Hobart", Value="Tasmania Standard Time"},
                    new NameValueDataInString {Name="(UTC+10:00) Vladivostok", Value="Vladivostok Standard Time"},
                    new NameValueDataInString {Name="(UTC+11:00) Magadan, Solomon Is., New Caledonia", Value="Central Pacific Standard Time"},
                    new NameValueDataInString {Name="(UTC+12:00) Auckland, Wellington", Value="New Zealand Standard Time"},
                    new NameValueDataInString {Name="(UTC+12:00) Fiji, Kamchatka, Marshall Is.", Value="Fiji Standard Time"},
                    new NameValueDataInString {Name="(UTC+13:00) Nuku'alofa", Value="Tonga Standard Time"},
                    new NameValueDataInString {Name="(UTC-01:00) Azores", Value="Azores Standard Time"},
                    new NameValueDataInString {Name="(UTC-01:00) Cape Verde Is.", Value="Cape Verde Standard Time"},
                    new NameValueDataInString {Name="(UTC-02:00) Mid-Atlantic", Value="Mid-Atlantic Standard Time"},
                    new NameValueDataInString {Name="(UTC-03:00) Brasilia", Value="E. South America Standard Time"},
                    new NameValueDataInString {Name="(UTC-03:00) Buenos Aires", Value="Argentina Standard Time"},
                    new NameValueDataInString {Name="(UTC-03:00) Georgetown", Value="SA Eastern Standard Time"},
                    new NameValueDataInString {Name="(UTC-03:00) Greenland", Value="Greenland Standard Time"},
                    new NameValueDataInString {Name="(UTC-03:00) Montevideo", Value="Montevideo Standard Time"},
                    new NameValueDataInString {Name="(UTC-03:30) Newfoundland", Value="Newfoundland Standard Time"},
                    new NameValueDataInString {Name="(UTC-04:00) Atlantic Time (Canada)", Value="Atlantic Standard Time"},
                    new NameValueDataInString {Name="(UTC-04:00) La Paz", Value="SA Western Standard Time"},
                    new NameValueDataInString {Name="(UTC-04:00) Manaus", Value="Central Brazilian Standard Time"},
                    new NameValueDataInString {Name="(UTC-04:00) Santiago", Value="Pacific SA Standard Time"},
                    new NameValueDataInString {Name="(UTC-04:30) Caracas", Value="Venezuela Standard Time"},
                    new NameValueDataInString {Name="(UTC-05:00) Bogota, Lima, Quito, Rio Branco", Value="SA Pacific Standard Time"},
                    new NameValueDataInString {Name="(UTC-05:00) Eastern Time (US &amp; Canada)", Value="Eastern Standard Time"},
                    new NameValueDataInString {Name="(UTC-05:00) Indiana (East)", Value="US Eastern Standard Time"},
                    new NameValueDataInString {Name="(UTC-06:00) Central America", Value="Central America Standard Time"},
                    new NameValueDataInString {Name="(UTC-06:00) Central Time (US &amp; Canada)", Value="Central Standard Time"},
                    new NameValueDataInString {Name="(UTC-06:00) Guadalajara, Mexico City, Monterrey", Value="Central Standard Time (Mexico)"},
                    new NameValueDataInString {Name="(UTC-06:00) Saskatchewan", Value="Canada Central Standard Time"},
                    new NameValueDataInString {Name="(UTC-07:00) Arizona", Value="US Mountain Standard Time"},
                    new NameValueDataInString {Name="(UTC-07:00) Chihuahua, La Paz, Mazatlan", Value="Mountain Standard Time (Mexico)"},
                    new NameValueDataInString {Name="(UTC-07:00) Mountain Time (US &amp; Canada)", Value="Mountain Standard Time"},
                    new NameValueDataInString {Name="(UTC-08:00) Pacific Time (US &amp; Canada)", Value="Pacific Standard Time"},
                    new NameValueDataInString {Name="(UTC-08:00) Tijuana, Baja California", Value="Pacific Standard Time (Mexico)"},
                    new NameValueDataInString {Name="(UTC-09:00) Alaska", Value="Alaskan Standard Time"},
                    new NameValueDataInString {Name="(UTC-10:00) Hawaii", Value="Hawaiian Standard Time"},
                    new NameValueDataInString {Name="(UTC-11:00) Midway Island, Samoa", Value="Samoa Standard Time"},
                    new NameValueDataInString {Name="(UTC-12:00) International Date Line West", Value="Dateline Standard Time"}
                };
        }

        public enum ContactTypes
        {
            PrimaryPlacement = 1,
            LegalGuardian,
            SecondaryPlacement,
            Relative,
            SchoolTeacher,
            Relative2

        }

        public static ServiceResponse SaveFile(HttpPostedFileBase postedFile, string destinationPath, string fileName = "", string fileExtension = "")
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                Guid guid = Guid.NewGuid();
                string fName = guid + "-" + DateTime.Now.ToString(ConfigSettings.DateFormatForSaveFile);
                string actualDestinationPath = HttpContext.Current.Server.MapPath(destinationPath);

                if (!Directory.Exists(actualDestinationPath))
                    Directory.CreateDirectory(actualDestinationPath);

                if (string.IsNullOrEmpty(fileName))
                    fileName = Path.GetFileName(postedFile.FileName);

                if (string.IsNullOrEmpty(fileExtension))
                    fileExtension = fileName.Split('.').Last();
                string fullFileName = string.Format("{0}{1}.{2}", actualDestinationPath, fName, fileExtension);

                postedFile.SaveAs(fullFileName);

                UploadedFileModel fileModel = new UploadedFileModel
                {
                    FileOriginalName = fileName,
                    TempFileName = string.Format("{0}.{1}", fName, fileExtension)
                };
                fileModel.TempFilePath = destinationPath + fileModel.TempFileName;
                response.IsSuccess = true;
                response.Data = fileModel;
                return response;
            }
            catch (Exception exception)
            {
                response.Message = exception.Message;// Resource.ExceptionMessage;
                response.IsSuccess = false;
            }
            return response;
        }

        public static UploadedFileModel SaveFileRetrunFilePath(HttpPostedFileBase postedFile, string destinationPath, string fileName = "", string fileExtension = "")
        {
            string filePath = "";
            UploadedFileModel fileModel = null;
            try
            {
                Guid guid = Guid.NewGuid();
                string fName = guid + "-" + DateTime.Now.ToString(ConfigSettings.DateFormatForSaveFile);
                string actualDestinationPath = HttpContext.Current.Server.MapPath(destinationPath);

                if (!Directory.Exists(actualDestinationPath))
                    Directory.CreateDirectory(actualDestinationPath);

                if (string.IsNullOrEmpty(fileName))
                    fileName = Path.GetFileName(postedFile.FileName);

                if (string.IsNullOrEmpty(fileExtension))
                    fileExtension = fileName.Split('.').Last();
                string fullFileName = string.Format("{0}{1}.{2}", actualDestinationPath, fName, fileExtension);

                postedFile.SaveAs(fullFileName);

                fileModel = new UploadedFileModel
                {
                    FileOriginalName = fileName,
                    TempFileName = string.Format("{0}.{1}", fName, fileExtension)
                };
                fileModel.TempFilePath = destinationPath + fileModel.TempFileName;
                return fileModel;
            }
            catch (Exception exception)
            {
                filePath = exception.Message;
            }
            return fileModel;
        }
        public static ServiceResponse ByteToImage(byte[] fileByte, string destinationPath)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                Guid guid = Guid.NewGuid();
                string fName = guid + "-" + DateTime.Now.ToString(ConfigSettings.DateFormatForSaveFile);
                string actualDestinationPath = HttpContext.Current.Server.MapPath(destinationPath);

                if (!Directory.Exists(actualDestinationPath))
                    Directory.CreateDirectory(actualDestinationPath);

                var fileNameWithExtension = fName + Constants.ImageJpg;

                File.WriteAllBytes(actualDestinationPath + fileNameWithExtension, fileByte);

                UploadedFileModel fileModel = new UploadedFileModel();
                fileModel.TempFilePath = destinationPath + fileNameWithExtension;
                response.IsSuccess = true;
                response.Data = fileModel;
                return response;
            }
            catch (Exception exception)
            {
                response.Message = exception.Message;// Resource.ExceptionMessage;
                response.IsSuccess = false;
            }
            return response;
        }

        public static ServiceResponse SaveFileWithOriginalName(HttpPostedFileBase postedFile, string destinationPath, string fileName = "", string fileExtension = "")
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                Guid guid = Guid.NewGuid();
                string fName = guid + "-" + DateTime.Now.ToString(ConfigSettings.DateFormatForSaveFile);
                string actualDestinationPath = HttpContext.Current.Server.MapPath(destinationPath);

                if (!Directory.Exists(actualDestinationPath))
                    Directory.CreateDirectory(actualDestinationPath);

                if (string.IsNullOrEmpty(fileName))
                    fileName = Path.GetFileName(postedFile.FileName);

                if (string.IsNullOrEmpty(fileExtension))
                    fileExtension = fileName.Split('.').Last();
                string fullFileName = string.Format("{0}{1}.{2}", actualDestinationPath, fileName);

                postedFile.SaveAs(fullFileName);
                UploadedFileModel fileModel = new UploadedFileModel
                {
                    FileOriginalName = fileName,
                    TempFileName = string.Format("{0}.{1}", fileName)
                };
                fileModel.TempFilePath = destinationPath + fileModel.TempFileName;
                response.IsSuccess = true;
                response.Data = fileModel;
                return response;
            }
            catch (Exception)
            {
                response.Message = Resource.ExceptionMessage;
                response.IsSuccess = false;
            }
            return response;
        }

        /// <summary>
        /// Delete file as passed path (file location)
        /// </summary>
        /// <param name="path">Local path of file</param>
        /// <returns></returns>
        public static ServiceResponse DeleteFile(string path)
        {
            string fullPath = path;
            if (!Path.IsPathRooted(path))
            {
                fullPath = HttpContext.Current.Server.MapPath(path);
            }


            ServiceResponse response = new ServiceResponse();
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                response.Data = Resource.FileDeleteSuccess;
                response.IsSuccess = true;
            }
            else
            {
                response.Data = Resource.FileNotExist;
                response.IsSuccess = false;
            }
            return response;
        }

        public static List<NameValueData> SetYesNoAllList()
        {
            return new List<NameValueData>
                {
                    new NameValueData { Name = Constants.All, Value = (int)Common.YesNoAllEnum.All},
                    new NameValueData { Name = Constants.Yes, Value = (int)Common.YesNoAllEnum.Yes},
                    new NameValueData { Name = Constants.No, Value = (int)Common.YesNoAllEnum.No },
                };
        }

        public static object SaveFile(HttpPostedFile postedFile, string destinationPath)
        {
            throw new NotImplementedException();
        }

        public static ServiceResponse MoveFile(string filePath, string destinationPath)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                string sourcePath = Path.GetDirectoryName(filePath);
                string fileNameWithExtension = Path.GetFileName(filePath);

                string tempSourcePath = HttpContext.Current.Server.MapPath(sourcePath);
                string tempDestinationPath = HttpContext.Current.Server.MapPath(destinationPath);
                if (!(Directory.Exists(tempSourcePath) && File.Exists(string.Format("{0}\\{1}", tempSourcePath, fileNameWithExtension))))
                {
                    serviceResponse.IsSuccess = false;
                    return serviceResponse;
                }

                if (!Directory.Exists(tempDestinationPath))
                    Directory.CreateDirectory(tempDestinationPath);
                else
                {
                    //Delete File from destination
                    string filneNameOnly = fileNameWithExtension.Split('.')[0];
                    string[] fileDest = Directory.GetFiles(tempDestinationPath);
                    foreach (string filedest in fileDest.Where(filedest => filedest.Contains(filneNameOnly)))
                        File.Delete(filedest);
                }
                File.Move(string.Format("{0}\\{1}", tempSourcePath, fileNameWithExtension), string.Format("{0}{1}", tempDestinationPath, fileNameWithExtension));
                serviceResponse.IsSuccess = true;
                serviceResponse.Data = string.Format("{0}{1}", destinationPath, fileNameWithExtension);
                return serviceResponse;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.IsSuccess = false;
            }
            return serviceResponse;
        }

        public static ServiceResponse DeleteFromTempFolder(string path)
        {
            ServiceResponse response = new ServiceResponse { IsSuccess = false };
            try
            {
                string actualDestinationPath = HttpContext.Current.Server.MapPath(path);
                if (Directory.Exists(actualDestinationPath))
                {
                    string[] fileDest = Directory.GetFiles(actualDestinationPath);
                    foreach (string file in fileDest)
                        File.Delete(file);
                    Directory.Delete(actualDestinationPath);
                }
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.IsSuccess = false;
            }
            return response;
        }

        public static List<NameValueData> SetActiveFilter()
        {
            return new List<NameValueData>
                        {
                            new NameValueData{Name = Constants.All,Value = -1},
                            new NameValueData{Name = Constants.Active,Value = 1},
                            new NameValueData{Name = Constants.InActive,Value = 0}
                        };
        }

        public static List<NameValueData> SetNoteCompletedFilter()
        {
            return new List<NameValueData>
                {
                    new NameValueData {Name = Constants.All, Value = -1},
                    new NameValueData {Name = Constants.Yes, Value = 1},
                    new NameValueData {Name = Constants.No, Value = 0},
                };
        }

        public static List<NameValueData> SetDraftFilter()
        {
            return new List<NameValueData>
                {
                    new NameValueData {Name = Constants.All, Value = (int) Common.YesNoAllEnum.All},
                    new NameValueData {Name = Constants.Yes, Value = (int) Common.YesNoAllEnum.Yes},
                    new NameValueData {Name = Constants.No, Value = (int) Common.YesNoAllEnum.No},
                };
        }

        public static List<NameValueData> SetDeleteFilter()
        {
            return new List<NameValueData>
                        {
                            new NameValueData{Name = Constants.AllRecords,Value = -1},
                            new NameValueData{Name = Constants.Active,Value = 0},
                            new NameValueData{Name = Constants.Deleted,Value = 1}
                        };
        }

        public static List<NameValueDataInString> SetNoteKind()
        {
            return new List<NameValueDataInString>
                {
                    new NameValueDataInString {Name = Resource.Email, Value = Resource.Email},
                    new NameValueDataInString {Name = Resource.Phone, Value = Resource.Phone},
                    new NameValueDataInString {Name = Resource.SMS, Value = Resource.SMS},
                    //new NameValueDataInString {Name = Resource.IM, Value = Resource.IM},
                    new NameValueDataInString {Name = Resource.Other, Value = Resource.Other}
                };
        }

        public static List<NameValueDataInString> SetCRUDActions()
        {
            return new List<NameValueDataInString>
                {
                    new NameValueDataInString {Name = Resource.Create, Value = Resource.Create},
                    new NameValueDataInString {Name = Resource.Update, Value = Resource.Update},
                    new NameValueDataInString {Name = Resource.Delete, Value = Resource.Delete}
                };
        }

        public static List<NameValueDataInString> SetSuspentionTypes()
        {
            return new List<NameValueDataInString>
                {
                    new NameValueDataInString {Name = Resource.Suspension, Value = Resource.Suspension},
                    new NameValueDataInString {Name = Resource.Termination, Value = Resource.Termination}
                };
        }

        public static List<NameValueDataInString> SetSuspentionLength()
        {
            return new List<NameValueDataInString>
                {
                    new NameValueDataInString {Value = "365", Name = Resource.SL_OneYear},
                    new NameValueDataInString {Value = "30", Name = Resource.SL_30Days},
                    new NameValueDataInString {Value = "60", Name = Resource.SL_60Days},
                    new NameValueDataInString {Value = "90", Name = Resource.SL_90Days},
                    new NameValueDataInString {Value = "0", Name = Resource.SL_Indefinitely},

                };
        }

        public static List<NameValueData> Set835ProcessedOnlyList()
        {
            return new List<NameValueData>
                {
                    new NameValueData {Name = Resource.AllRecords, Value = 0},
                    new NameValueData {Name = Resource.SentOnly, Value = 1},
                    new NameValueData {Name = Resource.SentAnd835Only, Value = 2}
                };

        }

        public static List<NameValueData> GetListFromEnum<T>()
        {
            var list = new List<NameValueData>();
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                var description = EnumHelper<T>.GetDisplayValue((T)item);
                list.Add(new NameValueData() { Name = description, Value = (int)Enum.Parse(typeof(T), item.ToString()) });
            }
            return list;
        }

        public static string GetEnumDisplayValue<T>(T item)
        {
            return EnumHelper<T>.GetDisplayValue(item);

        }

        public static string GetEnumHelpTextValue<T>(T item)
        {
            return EnumHelper<T>.GetHelpTextValue(item);

        }

        public static string GetEnumRequiredTextValue<T>(T item)
        {
            return EnumHelper<T>.GetRequiredTextValue(item);

        }

        public static string GetMAcAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            return sMacAddress;
        }

        public static string GetHostAddress()
        {
            try
            {
                return HttpContext.Current.Request.UserHostAddress;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string GetGeneralNameFormat(string firstName, string lastName)
        {
            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            {
                return String.Format("{0}, {1}", lastName, firstName);
            }
            if (!string.IsNullOrEmpty(firstName))
            {
                return firstName;
            }
            if (!string.IsNullOrEmpty(lastName))
            {
                return lastName;
            }
            return null;
        }

        public static string GetReferralNameFormat(string firstName, string lastName)
        {
            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            {
                return String.Format("{0}, {1}", lastName, firstName);
            }
            if (!string.IsNullOrEmpty(firstName))
            {
                return firstName;
            }
            if (!string.IsNullOrEmpty(lastName))
            {
                return lastName;
            }
            return null;
        }

        public static string GetEmpGeneralNameFormat(string firstName, string lastName)
        {
            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            {
                return String.Format("{0}, {1}", lastName, firstName);
            }
            if (!string.IsNullOrEmpty(firstName))
            {
                return firstName;
            }
            if (!string.IsNullOrEmpty(lastName))
            {
                return lastName;
            }
            return null;
        }

        public static void TransferObject(object source, object target)
        {
            var sourceType = source.GetType();
            var targetType = target.GetType();

            var sourceParameter = Expression.Parameter(typeof(object), "source");
            var targetParameter = Expression.Parameter(typeof(object), "target");

            var sourceVariable = Expression.Variable(sourceType, "castedSource");
            var targetVariable = Expression.Variable(targetType, "castedTarget");

            var expressions = new List<Expression>();

            expressions.Add(Expression.Assign(sourceVariable, Expression.Convert(sourceParameter, sourceType)));
            expressions.Add(Expression.Assign(targetVariable, Expression.Convert(targetParameter, targetType)));

            foreach (var property in sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!property.CanRead)
                    continue;

                var targetProperty = targetType.GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance);
                if (targetProperty != null
                        && targetProperty.CanWrite
                        && targetProperty.PropertyType.IsAssignableFrom(property.PropertyType))
                {
                    expressions.Add(
                        Expression.Assign(
                            Expression.Property(targetVariable, targetProperty),
                            Expression.Convert(
                                Expression.Property(sourceVariable, property), targetProperty.PropertyType)));
                }
            }

            var lambda =
                Expression.Lambda<Action<object, object>>(
                    Expression.Block(new[] { sourceVariable, targetVariable }, expressions),
                    new[] { sourceParameter, targetParameter });

            var del = lambda.Compile();

            del(source, target);
        }

        public static string CsvQuote(string text)
        {
            if (text == null)
            {
                return string.Empty;
            }

            bool containsQuote = false;
            bool containsComma = false;
            int len = text.Length;
            for (int i = 0; i < len && (containsComma == false || containsQuote == false); i++)
            {
                char ch = text[i];
                if (ch == '"')
                {
                    containsQuote = true;
                }
                else if (ch == ',' || char.IsControl(ch))
                {
                    containsComma = true;
                }
            }

            bool mustQuote = containsComma || containsQuote;

            if (containsQuote)
            {
                text = text.Replace("\"", "\"\"");
            }

            if (mustQuote)
            {
                return "\"" + text + "\""; // Quote the cell and replace embedded quotes with double-quote
            }
            else
            {
                return text;
            }
        }

        public static long GetFileSizeInBytes(string fullpath)
        {
            //var webRequest = HttpWebRequest.Create(url);
            //webRequest.Method = "HEAD";
            //WebResponse resp = webRequest.GetResponse();
            //long contentLength;
            //if (long.TryParse(resp.Headers.Get("Content-Length"), out contentLength))
            //{
            //    return Convert.ToString(Math.Round((double)contentLength, 2));
            //}


            FileInfo f = new FileInfo(fullpath);
            return f.Length;
        }

        public static bool SendFileBytesToResponse(string filePath, string fileName)
        {
            WebClient req = new WebClient();
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.ClearContent();
            response.ClearHeaders();
            response.BufferOutput = true;
            response.Buffer = true;

            if (Path.GetExtension(filePath) == ".pdf")
            {
                response.AddHeader("Content-Type", "application/pdf");
                response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            }
            else
            {
                response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
                response.ContentType = "application/vnd.ope ent.spreadsheetml.sheet";// "application/vnd.ms-excel";
            }
            byte[] data = req.DownloadData(filePath);
            //response.TransmitFile(filePath);
            File.Delete(filePath);
            response.BinaryWrite(data);
            response.End();
            return true;
        }

        /// <summary>
        /// from html file get its content
        /// </summary>
        /// <param name="htmlFilePath">html file path</param>
        /// <returns>html contents</returns>
        public static string ReadHtmlFile(string htmlFilePath)
        {
            string filePath = HttpContext.Current.Server.MapPath(ConfigSettings.TemplateBasePath + htmlFilePath);
            //string filePath = Path.Combine(baseConfirmationPath, htmlFilePath);
            string htmlString = File.ReadAllText(filePath);
            return htmlString;
        }






        public static List<NameValueData> SetWeekDays()
        {
            return new List<NameValueData>
                        {
                            new NameValueData{Name = Resource.Sunday,Value = 1},
                            new NameValueData{Name = Resource.Monday,Value = 2},
                            new NameValueData{Name = Resource.Tuesday,Value = 3},
                            new NameValueData{Name = Resource.Wednesday,Value = 4},
                            new NameValueData{Name = Resource.Thursday,Value = 5},
                            new NameValueData{Name = Resource.Friday,Value = 6},
                            new NameValueData{Name = Resource.Saturday,Value = 7},

                        };
        }

        public static bool SendZipFileBytesToResponse(string filePath, string fileName)
        {
            WebClient req = new WebClient();
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.ClearContent();
            response.ClearHeaders();
            response.BufferOutput = true;
            response.Buffer = true;
            response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
            response.ContentType = "application/zip";
            byte[] data = req.DownloadData(filePath);

            File.Delete(filePath);
            response.BinaryWrite(data);
            response.End();
            return true;
        }

        //public static string GenrtaeShortUrl(string URL)
        //{
        //    if (!ConfigSettings.UseBitly)
        //        return URL;
        //    string url = null;
        //    using (WebClient wb = new WebClient())
        //    {
        //        string data = string.Format(Constants.BitlyUrl,
        //        ConfigSettings.BitlyUserName, ConfigSettings.BitlyApiKey, HttpUtility.UrlEncode(URL), "xml");
        //        XmlDocument xmlDoc = new XmlDocument();
        //        xmlDoc.LoadXml(wb.DownloadString(data));
        //        url = xmlDoc.GetElementsByTagName("url")[0].InnerText;
        //    }
        //    return url;
        //}

        //public static ServiceResponse CreateAuditTrail<T>(AuditActionType action, long parentKeyFieldId, long? childKeyFieldId, T oldObject, T newObject, long loggedInUserID)
        //{
        //    CompareObject compareObject = new CompareObject();
        //    AuditTrailModel model = compareObject.GetAuditTrailModel(action, parentKeyFieldId, childKeyFieldId, oldObject, newObject);
        //    if (model == null) return null;

        //    AuditLogTable audit = new AuditLogTable();
        //    audit.AuditActionType = model.AuditActionType;
        //    audit.DataModel = model.DataModel;
        //    audit.DateTimeStamp = model.DateTimeStamp;
        //    audit.ParentKeyFieldID = model.ParentKeyFieldID;
        //    audit.ChildKeyFieldID = model.ChildKeyFieldID;
        //    audit.ValueBefore = model.ValueBefore;
        //    audit.ValueAfter = model.ValueAfter;
        //    audit.Changes = model.Changes;

        //    IAuditLogDataProvider auditLogDataProvider = new AuditLogDataProvider();
        //    ServiceResponse response = auditLogDataProvider.AddAuditLog(audit, loggedInUserID);

        //    return response;
        //}

        public static DateTime GetLastDayOfMonth(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
        }

        public static DateTime GetDayOfMonth(DateTime dateTime, int day)
        {
            return new DateTime(dateTime.Year, dateTime.Month, day);
        }


        //public static DateTime GetOrgCurrentDateTime()
        //{
        //    CacheHelper cache = new CacheHelper();
        //    var timeUtc = DateTime.UtcNow;
        //    var timeZone = TimeZoneInfo.FindSystemTimeZoneById(cache.TimeZone);
        //    var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);
        //    return today;
        //}


        public static DateTime GetLastBusinessDayOfMonth(DateTime dateTime, List<DateTime> holidays = null)
        {
            //var holidays = new List<DateTime> {/* list of observed holidays */};
            if (holidays == null) holidays = new List<DateTime>();
            DateTime lastBusinessDay = new DateTime();
            var i = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
            while (i > 0)
            {
                var dtCurrent = new DateTime(dateTime.Year, dateTime.Month, i);
                if (dtCurrent.DayOfWeek < DayOfWeek.Saturday && dtCurrent.DayOfWeek > DayOfWeek.Sunday &&
                 !holidays.Contains(dtCurrent))
                {
                    lastBusinessDay = dtCurrent;
                    i = 0;
                }
                else
                {
                    i = i - 1;
                }
            }
            return lastBusinessDay;
        }

        public static string ToAgeString(DateTime dob)
        {
            DateTime today = DateTime.Today;

            int months = today.Month - dob.Month;
            int years = today.Year - dob.Year;

            if (today.Day < dob.Day)
            {
                months--;
            }
            if (months < 0)
            {
                years--;
                months += 12;
            }
            //int days = (today - dob.AddMonths((years * 12) + months)).Days;
            return string.Format("{0}Y {1}M", years, months); //days, (days == 1) ? "" : "s");

        }

        //public static Byte[] ReturnByteArrayFromStringForitextSharpPDF(string stringContent)
        //{
        //    //Create a byte array that will eventually hold our final PDF
        //    Byte[] bytes;

        //    //Boilerplate iTextSharp setup here
        //    //Create a stream that we can write to, in this case a MemoryStream
        //    using (var ms = new MemoryStream())
        //    {
        //        //Create an iTextSharp Document which is an abstraction of a PDF but **NOT** a PDF
        //        var doc = new Document();

        //        //Create a writer that's bound to our PDF abstraction and our stream
        //        var writer = PdfWriter.GetInstance(doc, ms);
        //        //Open the document for writing
        //        doc.Open();

        //        //Our sample HTML and CSS
        //        var example_html = "@" + stringContent;

        //        //XMLWorker also reads from a TextReader and not directly from a string
        //        using (var srHtml = new StringReader(example_html))
        //        {
        //            //Parse the HTML
        //            iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, srHtml);
        //        }

        //        doc.Close();
        //        //After all of the PDF "stuff" above is done and closed but **before** we
        //        //close the MemoryStream, grab all of the active bytes from the stream
        //        bytes = ms.ToArray();

        //        return bytes;
        //    }
        //}


        public static List<NameValueData> SetPreferredCommunicationMethod()
        {
            return new List<NameValueData>
                        {
                            new NameValueData{Name = Resource.VoiceMail,Value = 1},
                            new NameValueData{Name = Resource.Email,Value= 2},
                            new NameValueData{Name = Resource.SMS,Value= 3},
                            new NameValueData{Name = Resource.Mail,Value= 4},
                            new NameValueData{Name = Resource.Unmarked,Value= 5},
                        };
        }

        public static List<NameValueData> GetHourList()
        {
            List<NameValueData> list = new List<NameValueData>();
            for (var i = 0; i <= 12; i++)
            {
                list.Add(new NameValueData { Name = i.ToString(), Value = i });
            }
            return list;
        }

        public static List<NameValueData> GetMinuteList()
        {
            return new List<NameValueData>
                        {
                            new NameValueData{Name = "0",Value= 0},
                            new NameValueData{Name = "15",Value= 15},
                            new NameValueData{Name = "30",Value= 30},
                            new NameValueData{Name = "45",Value= 45}
                        };
        }


        public static List<NameValueDataInString> SetReconcileStatus()
        {
            return new List<NameValueDataInString>
                        {
                            new NameValueDataInString{Name = Resource.RSPaid,Value= Resource.RSPaid},
                            new NameValueDataInString{Name = Resource.RSDenied,Value= Resource.RSDenied},
                            new NameValueDataInString{Name = Resource.NA,Value= "-2"},
                        };
        }

        public static List<NameValueDataInString> VisitTaskType()
        {
            return new List<NameValueDataInString>
                {
                    new NameValueDataInString {Name = Resource.Task, Value = Resource.Task},
                    new NameValueDataInString {Name = Resource.Conclusion, Value = Resource.Conclusion}
                };
        }

        public static List<NameValueDataInString> GetImportTypes()
        {
            return new List<NameValueDataInString>
                {
                    new NameValueDataInString {Name = Resource.Patient, Value = ImportTypeEnum.Patient.ToString() },
                    new NameValueDataInString {Name = Resource.Employee, Value = ImportTypeEnum.Employee.ToString() }
                };
        }


        public static string GenerateRandomNumber()
        {
            Guid id = Guid.NewGuid();
            return id.ToString();
        }

        public static T Getarrayvalue<T>(int index, Dictionary<Type, Array> fields)
        {
            Type type = typeof(T);
            Array array;
            if (fields.TryGetValue(type, out array))
            {
                if (index >= 0 && index < array.Length)
                {
                    return (T)array.GetValue(index);
                }
            }
            return default(T);
        }

        //public static DataTable CSVtoDataTable(string filePath)
        //{
        //    int count = 0;
        //    char fieldSeparator = ',';
        //    DataTable csvData = new DataTable();

        //    using (TextFieldParser csvReader = new TextFieldParser(filePath))
        //    {
        //        csvReader.HasFieldsEnclosedInQuotes = true;
        //        while (!csvReader.EndOfData)
        //        {
        //            csvReader.SetDelimiters(new string[] { "," });
        //            string[] fieldData = csvReader.ReadFields();
        //            if (count == 0)
        //            {
        //                foreach (string column in fieldData)
        //                {
        //                    DataColumn datecolumn = new DataColumn(column);
        //                    datecolumn.AllowDBNull = true;
        //                    csvData.Columns.Add(datecolumn);
        //                }
        //                count++;
        //            }
        //            else
        //            {
        //                csvData.Rows.Add(fieldData);
        //            }

        //        }
        //    }

        //    return csvData;

        //}

        //public static ClaimIdentifierModel GetBatchNoteIdFromResponse(string claimSubmitterIdentifier)
        //{
        //    ClaimIdentifierModel model = new ClaimIdentifierModel();
        //    if (claimSubmitterIdentifier.Contains("ZRPB") && claimSubmitterIdentifier.Contains("BN"))
        //    {
        //        string[] strTemp = claimSubmitterIdentifier.Split(new string[] { "ZRPB" }, StringSplitOptions.None);
        //        model.NoteId = Convert.ToInt64(strTemp[0]);
        //        model.BatchId = Convert.ToInt64(strTemp[1].Split(new string[] { "BN" }, StringSplitOptions.None)[0]);
        //        model.BatchNoteId = Convert.ToInt64(strTemp[1].Split(new string[] { "BN" }, StringSplitOptions.None)[1]);
        //    }
        //    else if (claimSubmitterIdentifier.Contains("N"))
        //    {

        //        string[] strTemp = claimSubmitterIdentifier.Trim('N').Split(new string[] { "N" }, StringSplitOptions.None);
        //        model.BatchNoteId = Convert.ToInt64(strTemp[0]);
        //        model.BatchNoteId02 = Convert.ToInt64(strTemp[1]);
        //    }

        //    return model;
        //}




        public static long GetClaimDetails(string value, string type, int order = 1)
        {
            value = value.Trim();

            if (value.Contains("ZRPB") && value.Contains("BN"))
            {
                string[] strTemp = value.Split(new string[] { "ZRPB" },
                    StringSplitOptions.None);
                long noteId = Convert.ToInt64(strTemp[0]);
                long batchId = Convert.ToInt64(strTemp[1].Split(new string[] { "BN" }, StringSplitOptions.None)[0]);
                long batchNoteId = Convert.ToInt64(strTemp[1].Split(new string[] { "BN" }, StringSplitOptions.None)[1]);

                if (type == "Note")
                    return noteId;
                if (type == "Batch")
                    return batchId;
                if (type == "BatchNote")
                    return batchNoteId;

            }
            else if (value.Contains("N"))
            {

                string[] strTemp = value.Trim('N').Split(new string[] { "N" }, StringSplitOptions.None);
                if (order == 1)
                    return Convert.ToInt64(strTemp[0]);
                if (order == 2)
                    return Convert.ToInt64(strTemp[1]);
            }
            return 0;

        }


        //public static LatLong GetLatLongByAddress(string address)
        //{
        //    var root = new GetLatLongByAddress.RootObject();

        //    var url =
        //        string.Format(
        //            "http://maps.googleapis.com/maps/api/geocode/json?address={0}", address);
        //    var req = (HttpWebRequest)WebRequest.Create(url);

        //    var res = (HttpWebResponse)req.GetResponse();

        //    using (var streamreader = new StreamReader(res.GetResponseStream()))
        //    {
        //        var result = streamreader.ReadToEnd();

        //        if (!string.IsNullOrWhiteSpace(result))
        //        {
        //            root = JsonConvert.DeserializeObject<GetLatLongByAddress.RootObject>(result);
        //        }
        //    }
        //    LatLong latlong = new LatLong();
        //    if (root.results.Count > 0)
        //    {
        //        latlong.Latitude = Convert.ToDouble(root.results[0].geometry.location.lat, CultureInfo.InvariantCulture);
        //        latlong.Longitude = Convert.ToDouble(root.results[0].geometry.location.lng, CultureInfo.InvariantCulture);
        //    }
        //    else
        //    {
        //        latlong = null;
        //    }
        //    return latlong;
        //}

        //public static SqlGeography GetLatLongByAddress(string address)
        //{
        //    var root = new GetLatLongByAddress.RootObject();

        //    var url =
        //        string.Format(
        //            "http://maps.googleapis.com/maps/api/geocode/json?address={0}", address);
        //    var req = (HttpWebRequest)WebRequest.Create(url);

        //    var res = (HttpWebResponse)req.GetResponse();

        //    using (var streamreader = new StreamReader(res.GetResponseStream()))
        //    {
        //        var result = streamreader.ReadToEnd();

        //        if (!string.IsNullOrWhiteSpace(result))
        //        {
        //            root = JsonConvert.DeserializeObject<GetLatLongByAddress.RootObject>(result);
        //        }
        //    }
        //    SqlGeography latlong;
        //    if (root.results.Count > 0)
        //    {
        //        var latitude = Convert.ToDouble(root.results[0].geometry.location.lat, CultureInfo.InvariantCulture);
        //        var longitude = Convert.ToDouble(root.results[0].geometry.location.lng, CultureInfo.InvariantCulture);
        //        latlong = SqlGeography.Point(latitude, longitude, 4326);

        //        return latlong;
        //    }
        //    else
        //    {
        //        latlong = null;
        //    }
        //    return latlong;
        //}

        //public static string GetFolderPath(int fileType)
        //{
        //    CacheHelper _cacheHelper = new CacheHelper();
        //    string fileDestinationPath;
        //    string uploadPath = String.Format(ConfigSettings.UploadPath, _cacheHelper.Domain);
        //    switch (fileType)
        //    {
        //        case (int)FileStorePathType.TempPath:
        //            fileDestinationPath = uploadPath + ConfigSettings.TempPath;
        //            break;
        //        case (int)FileStorePathType.EmpSignatures:
        //            fileDestinationPath = uploadPath + ConfigSettings.EmpSignatures;
        //            break;
        //        case (int)FileStorePathType.SiteLogoPath:
        //            fileDestinationPath = uploadPath + ConfigSettings.SiteLogoPath;
        //            break;
        //        case (int)FileStorePathType.EmpProfileImg:
        //            fileDestinationPath = uploadPath + ConfigSettings.EmpProfileImg;
        //            break;
        //        case (int)FileStorePathType.RefProfileImg:
        //            fileDestinationPath = uploadPath + ConfigSettings.RefProfileImg;
        //            break;
        //        default:
        //            fileDestinationPath = uploadPath;
        //            break;
        //    }

        //    return fileDestinationPath;
        //}

        public enum FileStorePathType
        {
            TempPath = 0, EmpSignatures, SiteLogoPath, EmpProfileImg, RefProfileImg
        }


        //public static SqlGeography GetSqlGeoPointFromLatLong(double latitude, double longitude)
        //{
        //    return SqlGeography.Point(latitude, longitude, 4326);
        //}


        public enum TimeSlotSaveResult
        {
            SQLException = -1, TimeSlotExistsNoRecordsAdded = -2, TimeSlotsPartiallyAdded = 2, TimeSlotsAdded = 1
        }


        public static string GetSiteBaseUrl()
        {
            try
            {
                return String.Format(ConfigurationManager.AppSettings["SiteBaseURL"], SessionHelper.DomainName);
            }
            catch (Exception)
            {

                return ConfigurationManager.AppSettings["SiteBaseURL"];
            }
        }
    }

    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }

    public static class ListOfExtension
    {
        public static List<T>[] Partition<T>(this List<T> list, int size)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            if (size < 1)
                throw new ArgumentOutOfRangeException("size");

            int count = (int)Math.Ceiling(list.Count / (double)size);
            List<T>[] partitions = new List<T>[count];

            int k = 0;
            for (int i = 0; i < partitions.Length; i++)
            {
                partitions[i] = new List<T>(size);
                for (int j = k; j < k + size; j++)
                {
                    if (j >= list.Count)
                    {
                        #region Code for empty add object

                        int remaining = list.Count % size;
                        int haveToAdd = size - remaining;

                        for (int t = 0; t < haveToAdd; t++)
                        {
                            var obj = (T)Activator.CreateInstance(typeof(T));
                            partitions[i].Add(obj);
                        }

                        #endregion

                        break;
                    }
                    partitions[i].Add(list[j]);
                }
                k += size;
            }

            return partitions;
        }
    }
}