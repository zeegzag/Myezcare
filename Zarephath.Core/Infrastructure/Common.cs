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
using EDI_837_835_HCCP;
using Newtonsoft.Json;
using Twilio;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Infrastructure.Utility;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;
using iTextSharp.text.pdf;
using Formatting = Newtonsoft.Json.Formatting;
using HttpResponse = System.Web.HttpResponse;
using iTextSharp.text;
using System.Data;
using Microsoft.VisualBasic.FileIO;
using OopFactory.Edi835Parser.Models;
//using Zarephath.Core.Infrastructure.GetLatLongByAddress;
using System.Globalization;
using System.Security.Cryptography;
using Microsoft.SqlServer.Types;
using System.Drawing;
using System.Net.Http;
using System.Web.Http;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
//using Microsoft.SqlServer.Management.Sdk.Sfc;
//using static Zarephath.Core.Models.ViewModel.HC_AddPayorModel;

namespace Zarephath.Core.Infrastructure
{
    public class Common
    {
        private static readonly object _syncFileAppendText = new object();

        public static void SyncAppendAllText(string path, string contents)
        {
            lock (_syncFileAppendText)
            {
                File.AppendAllText(path, contents);
            }
        }

        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public enum UserType
        {
            [Display(ResourceType = typeof(Resource), Name = "Employee")]
            Employee = 1,
            [Display(ResourceType = typeof(Resource), Name = "Referral")]
            Referral = 2
        }
        public enum DocumentationType
        {
            [Display(ResourceType = typeof(Resource), Name = "Internal")]
            Internal = 1,
            [Display(ResourceType = typeof(Resource), Name = "External")]
            External = 2
        }
        public static List<string> CombinationGeneraton<T>(T[] list, int k, int m)
        {
            List<string> strList = new List<string>();
            int i;
            if (k == m)
            {
                string strValue = string.Empty;
                for (i = 0; i <= m; i++)
                {
                    strValue += list[i] + ",";
                }
                if (strValue.Length > 0)
                {
                    strList.Add(strValue.TrimEnd(','));
                }
            }
            else
            {
                for (i = k; i <= m; i++)
                {
                    swapTwoNumber(ref list[k], ref list[i]);
                    strList.AddRange(CombinationGeneraton(list, k + 1, m));
                    swapTwoNumber(ref list[k], ref list[i]);
                }
            }
            return strList;
        }

        public static void swapTwoNumber<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        public static void GetSubDomain()
        {
            string domain = Domain();

            if (domain.Contains("www"))
                domain = domain.Replace("www", "").Trim();

            var subdomain = "";
            if (!string.IsNullOrEmpty(domain))
            {
                var nodes = domain.Split('.');
                subdomain = nodes[0];
            }

            //subdomain = "localhost_01";

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
            string domain = Domain();

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

        public static string Domain()
        {
#if DEBUG
            string domain = string.IsNullOrEmpty(ConfigSettings.Domain) ? HttpContext.Current.Request.Url.DnsSafeHost : ConfigSettings.Domain;
#else
            string domain = HttpContext.Current.Request.Url.DnsSafeHost;
#endif
            return domain;
        }

        public static string GetAccessPath(string path) =>
           string.IsNullOrEmpty(path) ? null : $"/Uploads/AccessFile/{Crypto.Encrypt(path)}{Path.GetExtension(path)}";

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

        public static void MergePDFs(string outPutFilePath, string[] filesPath)
        {
            Document document = new Document();
            //create newFileStream object which will be disposed at the end
            using (FileStream newFileStream = new FileStream(outPutFilePath, FileMode.Create))
            {
                // step 2: we create a writer that listens to the document
                PdfCopy writer = new PdfCopy(document, newFileStream);
                if (writer == null)
                {
                    return;
                }

                // step 3: we open the document
                document.Open();

                foreach (string fileName in filesPath)
                {
                    // we create a reader for a certain document
                    PdfReader reader = new PdfReader(fileName);
                    reader.ConsolidateNamedDestinations();

                    // step 4: we add content
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        PdfImportedPage page = writer.GetImportedPage(reader, i);
                        writer.AddPage(page);
                    }

                    PRAcroForm form = reader.AcroForm;
                    if (form != null)
                    {
                        writer.CopyDocumentFields(reader);
                    }

                    reader.Close();
                }

                // step 5: we close the document and writer
                writer.Close();
                document.Close();
            }//disposes the newFileStream object
        }


        public static bool SendFileBytesToResponse(string filePath, string fileName, bool deleteFile = true)
        {
            WebClient req = new WebClient();
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.ClearContent();
            response.ClearHeaders();
            response.BufferOutput = true;
            response.Buffer = true;

            //fileName = (fileName).Replace(" ", "_") + Path.GetExtension(filePath);
            //response.AddHeader("Content-Type", "application/pdf");
            //response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);

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
            if (deleteFile && filePath.Contains("\\temp\\"))
                File.Delete(filePath);

            response.BinaryWrite(data);
            response.End();
            return true;
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

        public static string CheckAndSetDxCodeType(string dXCodeType, bool isPrimary = false)
        {
            string tempDxCodeType = dXCodeType;
            if (isPrimary)
            {
                switch (dXCodeType)
                {
                    case Constants.DXCodeType_ICD10_Secondary:
                    case Constants.DXCodeType_ICD10_Primary:
                        tempDxCodeType = Constants.DXCodeType_ICD10_Primary;
                        break;
                    case Constants.DXCodeType_ICD09_Secondary:
                    case Constants.DXCodeType_ICD09_Primary:
                        tempDxCodeType = Constants.DXCodeType_ICD09_Primary;
                        break;
                }
            }
            else
            {
                switch (dXCodeType)
                {
                    case Constants.DXCodeType_ICD10_Secondary:
                    case Constants.DXCodeType_ICD10_Primary:
                        tempDxCodeType = Constants.DXCodeType_ICD10_Secondary;
                        break;
                    case Constants.DXCodeType_ICD09_Secondary:
                    case Constants.DXCodeType_ICD09_Primary:
                        tempDxCodeType = Constants.DXCodeType_ICD09_Secondary;
                        break;
                }
            }
            return tempDxCodeType;
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
                {
                    return true;
                }
                else
                {
                    //AceessDeniedErrorHistoryLogs(permission, "Localhost", 1);
                }
            }
            // AceessDeniedErrorHistoryLogs(permission, "Localhost", 1);
            return false;

            //return SessionHelper.Permissions != null ? SessionHelper.Permissions.Any(m => m.PermissionID.ToString() == permission) : false;
        }

        public enum DDType
        {
            [Display(ResourceType = typeof(Resource), Name = "CareType")]
            CareType = 1,
            [Display(ResourceType = typeof(Resource), Name = "PayerGroup")]
            PayerGroup = 2,
            [Display(ResourceType = typeof(Resource), Name = "BussinessLine")]
            BussinessLine = 3,
            [Display(ResourceType = typeof(Resource), Name = "NPIOptions")]
            NPIOptions = 4,
            [Display(ResourceType = typeof(Resource), Name = "RevenueCode")]
            RevenueCode = 5,
            [Display(ResourceType = typeof(Resource), Name = "AdmissionType")]
            AdmissionType = 6,
            [Display(ResourceType = typeof(Resource), Name = "AdmissionSource")]
            AdmissionSource = 7,
            [Display(ResourceType = typeof(Resource), Name = "PatientStatus")]
            PatientStatus = 8,
            [Display(ResourceType = typeof(Resource), Name = "VisitType")]
            VisitType = 9,
            [Display(ResourceType = typeof(Resource), Name = "FacilityCode")]
            FacilityCode = 10,
            [Display(ResourceType = typeof(Resource), Name = "PatientFrequencyCode")]
            PatientFrequencyCode = 11,
            [Display(ResourceType = typeof(Resource), Name = "PatientSystemStatus")]
            PatientSystemStatus = 12,
            [Display(ResourceType = typeof(Resource), Name = "TaskFrequencyCode")]
            TaskFrequencyCode = 13,
            [Display(ResourceType = typeof(Resource), Name = "AssessmentQuestionCategory")]
            AssessmentQuestionCategory = 14,
            [Display(ResourceType = typeof(Resource), Name = "AssessmentQuestionSubCategory")]
            AssessmentQuestionSubCategory = 15,
            [Display(ResourceType = typeof(Resource), Name = "Gender")]
            Gender = 16,
            [Display(ResourceType = typeof(Resource), Name = "LanguagePreference")]
            LanguagePreference = 17,
            [Display(ResourceType = typeof(Resource), Name = "Designation")]
            Designation = 18,
            [Display(ResourceType = typeof(Resource), Name = "DocumentSection")]
            DocumentSection = 19,
            [Display(ResourceType = typeof(Resource), Name = "SubSection")]
            SubSection = 20,
            [Display(ResourceType = typeof(Resource), Name = "HealthPlan")]
            BeneficiaryType = 22,
            [Display(ResourceType = typeof(Resource), Name = "PriorAuthorizationFrequency")]
            PriorAuthorizationFrequency = 24,
            [Display(ResourceType = typeof(Resource), Name = "NPINumber")]
            NPINumber = 30,
            [Display(ResourceType = typeof(Resource), Name = "EmployeeGroup")]
            EmployeeGroup = 32
        }


        public enum SetValue
        {
            CurrentTime = 1, LoggedInUserId
        }
        public enum SearchOperator
        {
            EqualTo = 1, NotEqualTo, BeginsWith, EndsWith, Contains, DoesNotContains, GreaterThan, LessThan
        }

        public static T DeserializeXML<T>(string input) where T : class
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(input))
            {
                return (T)ser.Deserialize(sr);
            }
        }

        public static string SerializeXML<T>(T ObjectToSerialize)
        {
            System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(ObjectToSerialize.GetType());
            System.Xml.Serialization.XmlSerializerNamespaces ns = new System.Xml.Serialization.XmlSerializerNamespaces();
            //Add an empty namespace and empty value
            ns.Add("", "");
            MemoryStream ms = new MemoryStream();
            using (XmlTextWriter writer = new XmlTextWriter(ms, Encoding.UTF8))
            {
                xmlSerializer.Serialize(writer, ObjectToSerialize, ns);
                ms = (MemoryStream)writer.BaseStream;
                return Encoding.UTF8.GetString(ms.ToArray());
            }
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
                    logPath = HttpContext.Current.Server.MapCustomPath(ConfigSettings.LogPath);
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
                    logPath = HttpContext.Current.Server.MapCustomPath(logPath);
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

            lock (_syncFileAppendText)
            {
                var sr = new StreamWriter(fileFullPath, true);
                sr.WriteLine("DateTime:" + DateTime.Now);
                sr.WriteLine(message);
                sr.WriteLine("_________________________________________________________________________________________________________________");
                sr.Flush();
                sr.Close();
            }

            return virtualPath;
        }







        public static string CreateLogFile_WithProgress(string message, string logFileName, string logPath, string keyName, double percentComplete)
        {
            string virtualPath = logPath;
            if (String.IsNullOrEmpty(logFileName))
                logFileName = "Exception" + DateTime.Today.ToString("MMddyyyy") + ".txt";

            if (String.IsNullOrEmpty(logPath))
            {
                if (HttpContext.Current != null)
                {
                    logPath = HttpContext.Current.Server.MapCustomPath(ConfigSettings.LogPath);
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
                    logPath = HttpContext.Current.Server.MapCustomPath(logPath);
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

            lock (_syncFileAppendText)
            {
                var sr = new StreamWriter(fileFullPath, true);
                sr.WriteLine("DateTime :-" + DateTime.Now + " :");
                sr.WriteLine(message);
                //sr.WriteLine("_________________________________________________________________________________________________________________");
                sr.WriteLine("\r\n <br/>");
                sr.Flush();
                sr.Close();
            }



            if (!string.IsNullOrEmpty(keyName))
            {

                SessionHelper.SessionObj.Remove(keyName);

                string readContents;
                using (StreamReader streamReader = new StreamReader(fileFullPath, Encoding.UTF8))
                {
                    readContents = streamReader.ReadToEnd();
                    CronJobServiceProgressModel model = new CronJobServiceProgressModel();
                    model.ProgressMessage = readContents;
                    model.PercentComplete = Convert.ToString(percentComplete);
                    SessionHelper.SessionObj[keyName] = model;
                }


            }

            return virtualPath;
        }


















        public static bool IsMatches(string password, string passwordSalt, string oldHashedPassword)
        {
            // Pass a logRounds parameter to GenerateSalt to explicitly specify the
            // amount of resources required to check the password. The work factor
            // increases exponentially, so each increment is twice as much work. If
            // omitted, a default of 10 is used.

            string hashed = "";
            try
            {
                hashed = BCrypt.Net.BCrypt.HashPassword(password, passwordSalt);
            }
            catch (Exception)
            {

            }

            return oldHashedPassword == hashed;
        }

        public static PasswordDetail CreatePassword(string password)
        {
            // Pass a logRounds parameter to GenerateSalt to explicitly specify the
            // amount of resources required to check the password. The work factor
            // increases exponentially, so each increment is twice as much work. If
            // omitted, a default of 10 is used.
            string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
            string hashedPasssword = BCrypt.Net.BCrypt.HashPassword(password, salt);

            PasswordDetail passwordDetail = new PasswordDetail { PasswordSalt = salt, Password = hashedPasssword };

            return passwordDetail;
        }

        public static bool SendEmail(string mailSubject, string fromEmail, string toEmails, string messageBody, string EmailType = "", string ccemailaddress = null, int smtpSettingNumer = (int)EmailHelper.SMTPSetting.GeneralEmailSetting, List<string> lstAttachments = null, bool orgTemplateWrapper = false)
        {
            //toEmails = "asavaliya@kairasoftware.com;radeshara@kairasoftware.com;Brad@zrpath.com;support@kairasoftware.com";
            BaseDataProvider dataProvider = new BaseDataProvider();
            MailMessage contactEmail = new MailMessage();

            if (orgTemplateWrapper)
            {
                string path = HttpContext.Current.Server.MapCustomPath("~/Assets/emailtemplates/main_template.html");
                var builderMainTemplate = new StringBuilder();
                using (StreamReader SourceReader = File.OpenText(path))
                {
                    builderMainTemplate.Append(SourceReader.ReadToEnd());
                }
                CacheHelper _cacheHelper = new CacheHelper();
                string logoURL = string.Format("{0}{1}", _cacheHelper.SiteBaseURL, _cacheHelper.SiteLogo);
                builderMainTemplate.Replace("##OrgLogoURL##", logoURL);
                string address = string.Format("{0}, {1}, {2} {3}", _cacheHelper.OrganizationAddress, _cacheHelper.OrganizationCity, _cacheHelper.OrganizationState, _cacheHelper.OrganizationZipcode);
                builderMainTemplate.Replace("##OrgAddress##", address);
                builderMainTemplate.Replace("##MessageBody##", messageBody);
                messageBody = builderMainTemplate.ToString();
            }

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
            var toEmailArray = new List<string>(toEmails.Split(','));
            foreach (var toEmail in toEmailArray)
                contactEmail.ToList.Add(toEmail.Trim());

            if (String.IsNullOrEmpty(fromEmail))
                contactEmail.From = mailSettings == null ? "" : mailSettings.Smtp.Network.UserName;
            else
                contactEmail.From = fromEmail;

            if (!string.IsNullOrEmpty(ccemailaddress))
            {
                var ccEmailArray = new List<string>(ccemailaddress.Split(','));
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
        public static bool SendEmailWithoutHTMLFormat(string mailSubject, string fromEmail, string toEmails, string messageBody, string EmailType = "", string ccemailaddress = null, int smtpSettingNumer = (int)EmailHelper.SMTPSetting.GeneralEmailSetting, List<string> lstAttachments = null)
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

            contactEmail.IsHtml = false;
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
        public static ServiceResponse SendEmailTest(string mailSubject, string fromEmail, string toEmails, string messageBody, string EmailType = "", string ccemailaddress = null, int smtpSettingNumer = (int)EmailHelper.SMTPSetting.GeneralEmailSetting, List<string> lstAttachments = null)
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

            contactEmail.IsHtml = false;
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
            ServiceResponse response = new ServiceResponse();
            try
            {

                response.Data = EmailHelper.SendEmail(contactEmail, smtpSettingNumer);
                response.IsSuccess = true;
                bool isSent = EmailHelper.SendEmail(contactEmail, smtpSettingNumer);
                InsertEntryInEmailHistoryLogs(mailSubject, toEmails, messageBody, isSent, dataProvider, EmailType);
                // return isSent;
                return response;
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.IsSuccess = false;
                response.Message = ex.Message.ToString();
                InsertEntryInEmailHistoryLogs(mailSubject, toEmails, messageBody, false, dataProvider, EmailType);
                CreateLogFile(ex.Message);
                //return false;
                return response;
            }

        }


        public static string SendTwilioNotification(string message, List<string> binding)
        {
            CacheHelper _cacheHelper = new CacheHelper();
            //string accountSid = ConfigSettings.TwilioAccountSID;
            //string authToken = ConfigSettings.TwilioAuthToken;
            //string serviceSid = ConfigSettings.TwilioServiceSID;

            string accountSid = _cacheHelper.TwilioAccountSID;
            string authToken = _cacheHelper.TwilioAuthToken;
            string serviceSid = _cacheHelper.TwilioServiceSID;



            TwilioClient.Init(accountSid, authToken);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                          | SecurityProtocolType.Tls11
                                                          | SecurityProtocolType.Tls12
                                                          | SecurityProtocolType.Ssl3;
            var notification = Twilio.Rest.Notify.V1.Service.NotificationResource.Create(
                serviceSid,
                toBinding: binding,
                body: message);


            return notification.Sid;
        }


        public static bool SendSms(string toNumber, string body, string emailType)
        {
            CacheHelper _cacheHelper = new CacheHelper();


            BaseDataProvider dataProvider = new BaseDataProvider();
            if (string.IsNullOrEmpty(_cacheHelper.TwilioAccountSID) || string.IsNullOrEmpty(_cacheHelper.TwilioAuthToken)
                || string.IsNullOrEmpty(_cacheHelper.TwilioServiceSID) || string.IsNullOrEmpty(_cacheHelper.TwilioFromNo)
                || string.IsNullOrEmpty(_cacheHelper.TwilioDefaultCountryCode))
            {
                InsertEntryForSmsInEmailHistoryLogs(toNumber, body, false, dataProvider, emailType);
                CreateLogFile("Sms Settings are not found.");
                return false;
            }






            // Find your Account Sid and Auth Token at twilio.com/user/account

            //string AccountSid = ConfigSettings.TwilioAccountSID;
            // string AuthToken = ConfigSettings.TwilioAuthToken;
            //var twilio = new TwilioRestClient(AccountSid, AuthToken);

            // TwilioClient.Init(ConfigSettings.TwilioAccountSID, ConfigSettings.TwilioAuthToken);
            TwilioClient.Init(_cacheHelper.TwilioAccountSID, _cacheHelper.TwilioAuthToken);


            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                          | SecurityProtocolType.Tls11
                                                          | SecurityProtocolType.Tls12
                                                          | SecurityProtocolType.Ssl3;

                // var message = twilio.SendMessage(ConfigSettings.TwilioFromNo, ConfigSettings.DefaultCountryCodeForSms + toNumber, body);
                string countryCode = _cacheHelper.TwilioDefaultCountryCode;//string.IsNullOrEmpty(_cacheHelper.TwilioDefaultCountryCode) ? Constants.DefaultCountryCodeForSms : _cacheHelper.TwilioDefaultCountryCode;
                var message = MessageResource.Create(
                 //ConfigSettings.DefaultCountryCodeForSms + toNumber,
                 countryCode + toNumber,
                from: new PhoneNumber(_cacheHelper.TwilioFromNo),
                body: body);

                if (!string.IsNullOrEmpty(message.Sid))
                {
                    InsertEntryForSmsInEmailHistoryLogs(toNumber, body, true, dataProvider, emailType);
                    return true;
                }
                InsertEntryForSmsInEmailHistoryLogs(toNumber, body, false, dataProvider, emailType);
                return false;
            }
            catch (Exception ex)
            {
                InsertEntryForSmsInEmailHistoryLogs(toNumber, body, false, dataProvider, emailType);
                CreateLogFile(ex.Message);
                return false;
            }
        }



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
        public static void AceessDeniedErrorHistoryLogs(string permission, string Domain, long LoggedInID)
        {
            BaseDataProvider dataProvider = new BaseDataProvider();
            List<SearchValueData> searchParam = new List<SearchValueData>();
            searchParam.Add(new SearchValueData { Name = "permission", Value = permission });
            searchParam.Add(new SearchValueData { Name = "Domain", Value = Domain });
            searchParam.Add(new SearchValueData { Name = "LoggedInID", Value = Convert.ToString(LoggedInID) });
            int data = (int)dataProvider.GetScalar(StoredProcedure.SaveAccessDeniedErrorLogs, searchParam);

        }

        public static void EmployeeLoginLogs(long employeeId, bool actionTypeLogin = true, string actionPlatform = null)
        {
            BaseDataProvider dataProvider = new BaseDataProvider();

            EmployeeLoginDetail employeeLoginDetail = new EmployeeLoginDetail
            {
                EmployeeID = employeeId,
                LoginTime = DateTime.UtcNow,
                ActionType = actionTypeLogin ? EmployeeLoginDetail.LoginActionType.Login.ToString() : EmployeeLoginDetail.LoginActionType.Logout.ToString(),
                ActionPlatform = !string.IsNullOrEmpty(actionPlatform) ? actionPlatform : EmployeeLoginDetail.LoginActionPlatform.Web.ToString(),
            };

            dataProvider.SaveObject(employeeLoginDetail, employeeId);
        }

        private static void InsertEntryForSmsInEmailHistoryLogs(string toPhoneNumber, string messageBody, bool isSent, BaseDataProvider dataProvider, string EmailType)
        {
            var email = new EmailHistoryLog
            {
                ToPhoneNo = toPhoneNumber,
                Body = messageBody,
                IsSent = isSent,
                EmailType = EmailType,
                CreatedDate = DateTime.UtcNow
            };

            dataProvider.SaveEntity(email);
        }

        public static List<SearchValueData> SetPagerValues(int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            List<SearchValueData> searchList = new List<SearchValueData>();

            var searchValueData = new SearchValueData { Name = "SortExpression", Value = Convert.ToString(sortIndex) };
            searchList.Add(searchValueData);

            searchValueData = new SearchValueData { Name = "SortType", Value = Convert.ToString(sortDirection) };
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


        public static List<T> DataTableToList<T>(DataTable dt)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            var columnNames = dt.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .ToList();
            var objectProperties = typeof(T).GetProperties(flags);
            var targetList = dt.AsEnumerable().Select(dataRow =>
            {
                var instanceOfT = Activator.CreateInstance<T>();

                foreach (var properties in objectProperties.Where(properties => columnNames.Contains(properties.Name) && dataRow[properties.Name] != DBNull.Value))
                {
                    properties.SetValue(instanceOfT, dataRow[properties.Name], null);
                }
                return instanceOfT;
            }).ToList();

            return targetList;
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

        public enum AgencyTypeEnum
        {
            ReferralSource = 1,
            CareGiver,
            Self
        }

        public enum ChecklistVisitType
        {
            MonthlyVisit = 1,
            AnnualVisit
        }

        public enum ChecklistType
        {
            PatientIntake = 1,
            MonthlyVisit = 2,
            AnnualVisit = 3
        }

        public enum CaseLoadTypeEnum
        {
            Permanent = 1,
            Temporary
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


        public static List<NameValueData> GetAttendanceStatuses()
        {
            return new List<NameValueData>
                {
                    new NameValueData { Name = Resource.Absent, Value = 0},
                    new NameValueData { Name = Resource.Present, Value = 1 },
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

        public static List<NameValueDataInString> SetDirectoryTypeList()
        {
            return new List<NameValueDataInString>
                {
                    new NameValueDataInString { Name = Constants.Section, Value = Constants.Directory},
                    new NameValueDataInString { Name = Constants.Subsection, Value = Constants.SubDirectory },
                };
        }


        public static List<NameValueData> SetROIType()
        {
            return new List<NameValueData>
                {
                    new NameValueData { Name = Constants.Verbal, Value = (int) ROITypes.Verbal},
                    new NameValueData { Name = Constants.Written, Value = (int)ROITypes.Written },
                };
        }

        public static List<NameValueData> SetGenderList()
        {
            return new List<NameValueData>
                {
                    new NameValueData { Name = Constants.Male, Value = (int) Common.Gender.Male},
                    new NameValueData { Name = Constants.Female, Value = (int)Common.Gender.Female },
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

        public static List<NameValueDataInString> SetCancellationReasons()
        {
            return new List<NameValueDataInString>
                {
                    new NameValueDataInString {Name = Constants.Parent, Value = Constants.Parent},
                    new NameValueDataInString {Name = Constants.Guardian, Value = Constants.Guardian},
                    new NameValueDataInString {Name = Constants.Case_Manager, Value = Constants.Case_Manager},
                    new NameValueDataInString {Name = Constants.Office, Value = Constants.Office},
                };
        }

        public static List<NameValueDataInString> Set835TemplateType()
        {

            return new List<NameValueDataInString>
                {
                    new NameValueDataInString { Name = Resource.EdiFile, Value = Enum835TemplateType.Edi_File.ToString()},
                    new NameValueDataInString { Name = Resource.PaperRemitsEOB, Value = Enum835TemplateType.Paper_Remits_EOB.ToString()}
                };
        }



        public static List<NameValueDataBoolean> SetDocumentKindOf()
        {
            return new List<NameValueDataBoolean>
                {
                    new NameValueDataBoolean { Name = Constants.Internal, Value = "1"},
                    new NameValueDataBoolean { Name = Constants.External, Value = "2" },
                };
        }

        public static List<NameValueDataBoolean> SetAutorizedNotAutorizedForBoolean()
        {
            return new List<NameValueDataBoolean>
                {
                    new NameValueDataBoolean { Name = Constants.Authorized, Value = "true"},
                    new NameValueDataBoolean { Name = Constants.NotAuthorize, Value = "false" },
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
            Relative2,
            EmployeePrimaryPlacement = 7

        }

        public static ServiceResponse SaveFile(HttpPostedFileBase postedFile, string destinationPath, string fileName = "", string fileExtension = "")
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                Guid guid = Guid.NewGuid();
                string fName = guid + "-" + DateTime.Now.ToString(ConfigSettings.DateFormatForSaveFile);
                string actualDestinationPath = HttpContext.Current.Server.MapCustomPath(destinationPath);

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



        public static ServiceResponse SaveFileContent(string fileData, string destinationPath, string fileName, string fileExtension = "")
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                Guid guid = Guid.NewGuid();
                string fName = guid + "-" + DateTime.Now.ToString(ConfigSettings.DateFormatForSaveFile);


                string actualDestinationPath = "";
                if (HttpContext.Current != null)
                {
                    actualDestinationPath = HttpContext.Current.Server.MapCustomPath(destinationPath);
                }
                else
                {
                    actualDestinationPath = AppDomain.CurrentDomain.BaseDirectory + destinationPath;
                }

                if (!Directory.Exists(actualDestinationPath))
                    Directory.CreateDirectory(actualDestinationPath);

                if (string.IsNullOrEmpty(fileExtension))
                    fileExtension = fileName.Split('.').Last();
                string fullFileName = string.Format("{0}{1}.{2}", actualDestinationPath, fName, fileExtension);


                if (File.Exists(fullFileName))
                    File.Delete(fullFileName);

                File.WriteAllText(fullFileName, fileData);

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

        public static ServiceResponse ByteToImage(byte[] fileByte, string destinationPath, string FileExtension = null)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                Guid guid = Guid.NewGuid();
                string fName = guid + "-" + DateTime.Now.ToString(ConfigSettings.DateFormatForSaveFile);
                string actualDestinationPath = HttpContext.Current.Server.MapCustomPath(destinationPath);

                if (!Directory.Exists(actualDestinationPath))
                    Directory.CreateDirectory(actualDestinationPath);

                var fileNameWithExtension = fName + (string.IsNullOrEmpty(FileExtension) ? Constants.ImageJpg : FileExtension);


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
                string actualDestinationPath = HttpContext.Current.Server.MapCustomPath(destinationPath);

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
                fullPath = HttpContext.Current.Server.MapCustomPath(path);
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

                string tempSourcePath = HttpContext.Current.Server.MapCustomPath(sourcePath);
                string tempDestinationPath = HttpContext.Current.Server.MapCustomPath(destinationPath);
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
                string actualDestinationPath = HttpContext.Current.Server.MapCustomPath(path);
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

        public static List<NameValueData> SetBXContractStatusList()
        {
            return new List<NameValueData>
                        {
                            new NameValueData{Name = Constants.All,Value = -1},
                            new NameValueData{Name = Constants.Active,Value = 1},
                            new NameValueData{Name = Constants.InActive,Value = 0}
                        };
        }

        public static List<NameValueData> SetScheduleNotificationAction()
        {
            return new List<NameValueData>
                        {
                            new NameValueData{Name = Constants.All,Value = -1},
                            new NameValueData{Name = Constants.Accepted,Value = 1},
                            new NameValueData{Name = Constants.NotAccepted,Value = 0}
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


        public static List<NameValueData> SetNoteTypes()
        {
            return new List<NameValueData>
                {
                    new NameValueData {Name = Constants.All, Value = -1},
                    new NameValueData {Name = Constants.Billable, Value = 1},
                    new NameValueData {Name = Constants.Non_Billable, Value = 0},
                };
        }

        public static List<NameValueData> SetServicesFilter()
        {
            return new List<NameValueData>
                {
                    new NameValueData {Name = Constants.All, Value = (int) Common.ReferralService.All},
                    new NameValueData {Name = Constants.Respite, Value = (int) Common.ReferralService.Respite},
                    new NameValueData {Name = Constants.Life_Skills, Value = (int) Common.ReferralService.Life_Skills},
                    new NameValueData {Name = Constants.Counselling, Value = (int) Common.ReferralService.Counselling},
                    new NameValueData {Name = Constants.ConnectingFamiliesService, Value = (int) Common.ReferralService.ConnectingFamilies},

                };
        }

        //public static List<NameValueData> SetCareTypeFilter()
        //{
        //    return new List<NameValueData>
        //        {
        //            new NameValueData {Name = Resource.PCA, Value = (int) EnumCareType.PCA},
        //            new NameValueData {Name = Resource.Respite, Value = (int) EnumCareType.Respite}
        //        };
        //}
        public static List<NameValueData> SetUnitTypeFilter()
        {
            return new List<NameValueData>
                {
                    new NameValueData {Name = Resource.UnitTypeTime, Value = (int) EnumUnitType.Time},
                    new NameValueData {Name = Resource.UnitTypeVisit, Value = (int) EnumUnitType.Visit},
                    new NameValueData {Name = Resource.UnitTypePerDay_FlatRate, Value = (int) EnumUnitType.PerDay_FlatRate}
                    
                    //,
                    //new NameValueData {Name = Resource.UnitTypeMile, Value = (int) EnumUnitType.DistanceInMiles},
                    //new NameValueData {Name = Resource.UnitTypeStop, Value = (int) EnumUnitType.Stop}
                };
        }

        public static List<NameValueDataInString> SetServicesFilterFor270Process()
        {
            return new List<NameValueDataInString>
                {
                    new NameValueDataInString {Name = Constants.All, Value = Resource.AllServicesText},
                     new NameValueDataInString {Name = Constants.Respite, Value = Resource.Respite},
                    new NameValueDataInString {Name = Constants.Life_Skills, Value = Resource.LifeSkills},
                    new NameValueDataInString {Name = Constants.Counselling, Value =Resource.Counselling},
                    new NameValueDataInString {Name = Constants.ConnectingFamiliesService, Value = Resource.ConnectingFamilies},

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

        public static List<NameValueData> SetDayOffTypes()
        {
            return new List<NameValueData>
                {
                    new NameValueData {Name = EmployeeDayOff.EmpDayOffType.Other.ToString(), Value = (int) EmployeeDayOff.EmpDayOffType.Other},
                    new NameValueData {Name = EmployeeDayOff.EmpDayOffType.Sick.ToString(), Value = (int) EmployeeDayOff.EmpDayOffType.Sick},
                    new NameValueData {Name = EmployeeDayOff.EmpDayOffType.Vacation.ToString(), Value = (int) EmployeeDayOff.EmpDayOffType.Vacation}
                };
        }

        public static List<NameValueData> SetUserTypeList()
        {
            return new List<NameValueData>
                {
                    new NameValueData {Name = UserType.Employee.ToString(), Value = (int)UserType.Employee},
                    new NameValueData {Name = UserType.Referral.ToString(), Value = (int)UserType.Referral}
                };
        }

        public static List<NameValueData> SetDocumentationTypeList()
        {
            return new List<NameValueData>
                {
                    new NameValueData {Name = DocumentationType.Internal.ToString(), Value = (int)DocumentationType.Internal},
                    new NameValueData {Name = DocumentationType.External.ToString(), Value = (int)DocumentationType.External}
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
        public static List<NameValueData> SetPrecedenceList()
        {
            return new List<NameValueData>
                        {
                            new NameValueData{Name = Constants.Primary,Value = 1},
                            new NameValueData{Name = Constants.Secondary,Value = 2},
                            new NameValueData{Name = Constants.Tertiary,Value = 3}
                        };
        }


        public static List<NameValueData> SetPOSList()
        {
            return new List<NameValueData>
                        {
                            new NameValueData{Name = Constants.Pharmacy,Value = 1},
                            new NameValueData{Name = Constants.Telehealth,Value = 2},
                            new NameValueData{Name = Constants.School,Value = 3},
                            new NameValueData{Name = Constants.HomelessShelter,Value = 4},
                            new NameValueData{Name = Constants.IndianHealthServiceFreeStandingFacility,Value = 5},
                            new NameValueData{Name = Constants.IndianHealthServiceProviderBasedFacility,Value = 6},
                            new NameValueData{Name = Constants.Tribal638FreeStandingFacility,Value = 7},
                            new NameValueData{Name = Constants.Tribal638ProviderBasedFacility,Value = 8},
                            new NameValueData{Name = Constants.PrisonCorrectionalFacility,Value = 9},
                            new NameValueData{Name = Constants.Office,Value = 11},
                            new NameValueData{Name = Constants.Home ,Value = 12},
                            new NameValueData{Name = Constants.AssistedLivingFacility,Value = 13},
                            new NameValueData{Name = Constants.GroupHome,Value = 14},
                            new NameValueData{Name = Constants.MobileUnit,Value = 15},
                            new NameValueData{Name = Constants.TemporaryLodging,Value = 16},
                            new NameValueData{Name = Constants.WalkInRetailHealthClinic,Value = 17},
                            new NameValueData{Name = Constants.PlaceofEmploymentWorksite,Value = 18},
                            new NameValueData{Name = Constants.OffCampusOutpatientHospital,Value = 19},
                            new NameValueData{Name = Constants.UrgentCareFacility,Value = 20},
                            new NameValueData{Name = Constants.InpatientHospital,Value = 21},
                            new NameValueData{Name = Constants.OnCampusOutpatientHospital,Value = 22},
                            new NameValueData{Name = Constants.EmergencyRoomHospital,Value =23},
                            new NameValueData{Name = Constants.AmbulatorySurgicalCenter,Value = 24},
                            new NameValueData{Name = Constants.BirthingCenter,Value = 25},
                            new NameValueData{Name = Constants.MilitaryTreatmentFacility,Value = 26},
                            new NameValueData{Name = Constants.SkilledNursingFacility,Value = 31},
                            new NameValueData{Name = Constants.NursingFacility,Value = 32},
                            new NameValueData{Name = Constants.CustodialCareFacility,Value = 33},
                            new NameValueData{Name = Constants.Hospice,Value = 34},
                            new NameValueData{Name = Constants.AmbulanceLand,Value = 41},
                            new NameValueData{Name = Constants.AmbulanceAirorWater,Value = 42},
                            new NameValueData{Name = Constants.IndependentClinic,Value = 49},
                            new NameValueData{Name = Constants.FederallyQualifiedHealthCenter,Value = 50},
                            new NameValueData{Name = Constants.InpatientPsychiatricFacility ,Value = 51},
                            new NameValueData{Name = Constants.PsychiatricFacilityPartialHospitalization ,Value = 52},
                            new NameValueData{Name = Constants.CommunityMentalHealthCenter ,Value = 53},
                            new NameValueData{Name = Constants.IntermediateCareFacilityMentallyRetarded ,Value = 54},
                            new NameValueData{Name = Constants.ResidentialSubstanceAbuseTreatmentFacility ,Value = 55},
                            new NameValueData{Name = Constants.PsychiatricResidentialTreatmentCenter,Value = 56},
                            new NameValueData{Name = Constants.NonresidentialSubstanceAbuseTreatmentFacility,Value = 57},
                            new NameValueData{Name = Constants.MassImmunizationCenter ,Value = 60},
                            new NameValueData{Name = Constants.ComprehensiveInpatientRehabilitationFacility ,Value = 61},
                            new NameValueData{Name = Constants.ComprehensiveOutpatientRehabilitationFacility ,Value = 62},
                            new NameValueData{Name = Constants.EndStageRenalDiseaseTreatmentFacility ,Value = 65},
                            new NameValueData{Name = Constants.PublicHealthClinic ,Value = 71},
                            new NameValueData{Name = Constants.RuralHealthClinic ,Value = 72},
                            new NameValueData{Name = Constants.IndependentLaboratory ,Value = 81},
                            new NameValueData{Name = Constants.OtherPlaceOfService,Value = 99}
                        };
        }
        public static List<NameValueData> SetAdmissionTypeList()
        {
            return new List<NameValueData>
                        {
                            new NameValueData{Name = Constants.Emergency,Value = 1},
                            new NameValueData{Name = Constants.Urgent,Value = 2},
                            new NameValueData{Name = Constants.Elective,Value = 3},
                            new NameValueData{Name = Constants.Newborn,Value = 4},
                            new NameValueData{Name = Constants.Trauma,Value = 5},
                            new NameValueData{Name = Constants.InformationNotAvailable,Value = 9}
                        };
        }
        public static List<NameValueData> SetAdmissionSourceList()
        {
            return new List<NameValueData>
                        {
                            new NameValueData{Name = Constants.PhysicianReferral,Value = 1},
                            new NameValueData{Name = Constants.ClinicReferral,Value = 2},
                            new NameValueData{Name = Constants.HMOReferral,Value = 3},
                            new NameValueData{Name = Constants.TransferFromHospital,Value = 4},
                            new NameValueData{Name = Constants.TransferFromSNF,Value = 5},
                            new NameValueData{Name = Constants.TransferFromAnotherHealthCareFacility,Value = 6},
                            new NameValueData{Name = Constants.EmergencyRoom,Value = 7},
                            new NameValueData{Name = Constants.CourtLawEnforcement,Value = 8},
                            new NameValueData{Name = Constants.InformationNotAvailable,Value = 9}
                        };
        }
        public static List<NameValueData> SetPatientDischargeStatusList()
        {
            return new List<NameValueData>
                        {
                            new NameValueData{Name = Constants.DischargedToHomeOrSelfCare,Value = 1},
                            new NameValueData{Name = Constants.DischargedTransferredToAnotherShortTermGeneral ,Value =2},
                            new NameValueData{Name = Constants.DischargedTransferredToSkilledNursingFacility ,Value = 3},
                            new NameValueData{Name = Constants.DischargedTransferredToAnIntermediateCareFacility ,Value = 4},
                            new NameValueData{Name = Constants.DischargedTransferredToAnotherTypeOfInstitution ,Value = 5},
                            new NameValueData{Name = Constants.DischargedTransferredToHomeUnderCareOfOrganizedHomeHealthServiceOrganization ,Value = 6},
                            new NameValueData{Name = Constants.LeftAgainstMedicalAdvice ,Value = 7},
                            new NameValueData{Name = Constants.Reserved ,Value = 8},
                            new NameValueData{Name = Constants.AdmittedAsAnInpatientToThisHospital,Value = 9},
                            new NameValueData{Name = Constants.ExpiredOrDidNotRecover ,Value = 20},
                            new NameValueData{Name = Constants.StillaPatient ,Value = 30},
                            new NameValueData{Name = Constants.ExpiredAtHome ,Value = 40},
                            new NameValueData{Name = Constants.ExpiredInaMedicalFacility ,Value = 41},
                            new NameValueData{Name = Constants.ExpiredPlaceUnknown ,Value = 42},
                            new NameValueData{Name = Constants.DischargedToFederalHealthCareFacility ,Value = 43},
                            new NameValueData{Name = Constants.HospiceHome ,Value = 50},
                            new NameValueData{Name = Constants.HospiceMedicalFacility ,Value = 51},
                            new NameValueData{Name = Constants.DischargeToHospitalBasedSwingBed ,Value = 61},
                            new NameValueData{Name = Constants.DischargedToInpatientRehab ,Value = 62},
                            new NameValueData{Name = Constants.DischargedToLongTermCareHospital ,Value = 63},
                            new NameValueData{Name = Constants.DischargedToNursingFacility ,Value = 64},
                            new NameValueData{Name = Constants.DischargedToPsychiatricHospital ,Value = 65},
                            new NameValueData{Name = Constants.DischargedToCriticalAccessHospital ,Value = 66}
                        };
        }

        public static List<NameValueData> SetExpireDateFilter()
        {
            return new List<NameValueData>
                        {
                            new NameValueData{Name = Constants.All,Value = -1},
                            new NameValueData{Name = Constants.Expired,Value = 0},
                            new NameValueData{Name = Constants.Expirein90Days,Value = 1}
                        };
        }

        public static List<NameValueDataInString> SetNoteServices()
        {
            return new List<NameValueDataInString>
                {
                    new NameValueDataInString {Name = Constants.Respite, Value = Constants.Respite},
                    new NameValueDataInString {Name = Constants.Life_Skills, Value = Constants.Life_Skills},
                    new NameValueDataInString {Name = Constants.Counselling, Value = Constants.Counselling},
                    new NameValueDataInString {Name = Constants.ConnectingFamiliesService, Value = Constants.ConnectingFamiliesService},
                };
        }

        public static List<NameValueDataInString> ClaimAdjustmentTypes()
        {
            return new List<NameValueDataInString>
                {
                    new NameValueDataInString {Name = ClaimAdjustmentType.ClaimAdjustmentType_Void, Value = ClaimAdjustmentType.ClaimAdjustmentType_Void},
                    new NameValueDataInString {Name = ClaimAdjustmentType.ClaimAdjustmentType_Replacement, Value = ClaimAdjustmentType.ClaimAdjustmentType_Replacement},
                };
        }

        public static List<NameValueDataInString> SetNoteRelations()
        {
            return new List<NameValueDataInString>
                {
                    new NameValueDataInString {Name = Constants.Parent, Value = Constants.Parent},
                    new NameValueDataInString {Name = Constants.Placement, Value = Constants.Placement},
                    new NameValueDataInString {Name = Constants.Guardian, Value = Constants.Guardian},
                    new NameValueDataInString {Name = Constants.Case_Manager, Value = Constants.Case_Manager},
                    new NameValueDataInString {Name = Constants.Office, Value = Constants.Office},
                    new NameValueDataInString {Name = Constants.Staff, Value = Constants.Staff},
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

        public enum PayorEDIFileType
        {
            Professional = 1,
            Institutional = 2
        }

        public static List<NameValueDataInString> SetEDIFileTypeList()
        {
            return new List<NameValueDataInString>
                {
                    new NameValueDataInString {Name = PayorEDIFileType.Professional.ToString(), Value = PayorEDIFileType.Professional.ToString()},
                    new NameValueDataInString {Name = PayorEDIFileType.Institutional.ToString(), Value = PayorEDIFileType.Institutional.ToString()}
                };
        }

        public class PayorClaimProcessors
        {
            public static readonly NameValueStringData HHAExchange = new NameValueStringData { Name = "HHAeXchange", Value = "HHAExchange" };
            public static readonly NameValueStringData Carebridge = new NameValueStringData { Name = "Carebridge", Value = "Carebridge" };
            public static readonly NameValueStringData Tellus = new NameValueStringData { Name = "Tellus", Value = "Tellus" };
            public static readonly NameValueStringData Sandata = new NameValueStringData { Name = "Sandata", Value = "Sandata" };
            public static readonly NameValueStringData Other = new NameValueStringData { Name = "Other", Value = "Other" };

        }

        public static List<NameValueStringData> SetPayorClaimProcessorList()
        {
            return new List<NameValueStringData>
                {
                    PayorClaimProcessors.HHAExchange,
                    PayorClaimProcessors.Carebridge,
                    PayorClaimProcessors.Tellus,
                    PayorClaimProcessors.Sandata,
                    PayorClaimProcessors.Other,
                };
        }

        public static string GetAggregatorLogPath(string fileName, string aggregator, bool? isSuccess, bool? isWaitingForResponse, long organizationID)
        {
            string path = string.Empty;
            try
            {
                if (aggregator != null && !string.IsNullOrEmpty(fileName) && (isSuccess.HasValue || isWaitingForResponse.HasValue))
                {
                    bool isWaiting = isWaitingForResponse.HasValue && isWaitingForResponse.Value;
                    string statusText = isWaiting ? "Waiting" : (isSuccess.Value ? "Processed" : "Failed");

                    string fileNamePrefix = string.Empty;
                    string logFileNamePrefix = "Logs_";
                    if (aggregator.ToLower() == PayorClaimProcessors.HHAExchange.Value.ToLower())
                    {
                        path = ConfigSettings.HHAXPath;
                        fileNamePrefix = ConfigSettings.HHAXFileNamePrefix;
                    }
                    else if (aggregator.ToLower() == PayorClaimProcessors.Carebridge.Value.ToLower())
                    {
                        path = ConfigSettings.CareBridgePath;
                        fileNamePrefix = ConfigSettings.CareBridgeFileNamePrefix;
                    }
                    else if (aggregator.ToLower() == PayorClaimProcessors.Tellus.Value.ToLower())
                    {
                        path = ConfigSettings.TellusPath;
                        fileNamePrefix = ConfigSettings.TellusFileNamePrefix;
                    }
                    else if (aggregator.ToLower() == PayorClaimProcessors.Sandata.Value.ToLower())
                    {
                        path = ConfigSettings.SandataPath;
                        fileNamePrefix = ConfigSettings.SandataFileNamePrefix;
                    }

                    string filePart = Path.GetFileNameWithoutExtension(fileName).Replace(fileNamePrefix, logFileNamePrefix);
                    path = isWaiting ? $"{path}/{statusText}/{filePart}{Constants.FileExtension_Txt}"
                        : $"{path}/{statusText}/{organizationID}/{filePart}{Constants.FileExtension_Txt}";
                }
            }
            catch { }
            return path;
        }

        public class PayorVisitBilledBy
        {
            public static readonly NameValueStringData HHAExchange = new NameValueStringData { Name = "HHAeXchange", Value = "HHAExchange" };
            public static readonly NameValueStringData Carebridge = new NameValueStringData { Name = "Carebridge", Value = "Carebridge" };
            public static readonly NameValueStringData Sandata = new NameValueStringData { Name = "Sandata", Value = "Sandata" };
            public static readonly NameValueStringData MyEazyCare = new NameValueStringData { Name = "myEZcare", Value = "MyEazyCare" };
        }

        public static List<NameValueStringData> SetPayorVisitBilledByList()
        {
            return new List<NameValueStringData>
                {
                    PayorVisitBilledBy.HHAExchange,
                    PayorVisitBilledBy.Carebridge,
                    PayorVisitBilledBy.Sandata,
                    PayorVisitBilledBy.MyEazyCare,
                };
        }

        public static string GetEnumDisplayValue<T>(T item)
        {
            return EnumHelper<T>.GetDisplayValue(item);

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
                CacheHelper_MyezCare ch_MyezcareOrg = new CacheHelper_MyezCare();
                MyEzcareOrganization myOrg = ch_MyezcareOrg.GetCachedData<MyEzcareOrganization>();
                if (myOrg != null)
                {
                    return myOrg.OrganizationID.ToString();
                }
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

        public static string GetOrgNameDisplayFormat()
        {
            var preference = SessionHelper.OrganizationPreference;
            if (preference != null && !string.IsNullOrEmpty(preference.NameDisplayFormat))
                return preference.NameDisplayFormat;
            else
                return "";
        }
        public static string GetGenericNameFormat(string firstName, string middleName, string lastName)
        {
            var nameFormat = GetOrgNameDisplayFormat();
            if (firstName == null)
                firstName = "";
            if (middleName == null)
                middleName = "";
            if (lastName == null)
                lastName = "";


            if (nameFormat == "First Last")
                return firstName.Trim() + " " + lastName.Trim();
            else if (nameFormat == "Last First")
                return lastName.Trim() + " " + firstName.Trim();
            else if (nameFormat == "First, Last")
                return firstName.Trim() + ", " + lastName.Trim();
            else if (nameFormat == "Last, First Middle")
                return lastName.Trim() + ", " + firstName.Trim() + " " + middleName.Trim();
            else if (nameFormat == "First Middle Last")
                return firstName.Trim() + " " + middleName.Trim() + " " + lastName.Trim();
            else
                return lastName.Trim() + ", " + firstName.Trim();

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
        //public static string GetEmpGeneralNameFormat(string firstName, string lastName)
        //{
        //    if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
        //    {
        //        return String.Format("{0}, {1}", lastName, firstName);
        //    }
        //    if (!string.IsNullOrEmpty(firstName))
        //    {
        //        return firstName;
        //    }
        //    if (!string.IsNullOrEmpty(lastName))
        //    {
        //        return lastName;
        //    }
        //    return null;
        //}


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

        public static string GetFileSize(decimal length, string sizeIn)
        {
            if (length > 0)
            {
                long OneKb = 1024;
                long OneMb = OneKb * 1024;
                long OneGb = OneMb * 1024;
                long OneTb = OneGb * 1024;

                if (Constants.SizeIn_TB == sizeIn)
                    return string.Format("{0} {1}", Convert.ToString(Math.Round((double)length / OneTb, 2)), Constants.SizeIn_TB);
                if (Constants.SizeIn_GB == sizeIn)
                    return string.Format("{0} {1}", Convert.ToString(Math.Round((double)length / OneGb, 2)), Constants.SizeIn_GB);
                if (Constants.SizeIn_MB == sizeIn)
                    return string.Format("{0} {1}", Convert.ToString(Math.Round((double)length / OneMb, 2)), Constants.SizeIn_MB);
                if (Constants.SizeIn_KB == sizeIn)
                    return string.Format("{0} {1}", Convert.ToString(Math.Round((double)length / OneKb, 2)), Constants.SizeIn_KB);
            }
            return "";
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




        public static string HttpContext_Current_Server_MapPath(string relativePath)
        {

            string fullpath = "";
            if (HttpContext.Current != null)
            {
                fullpath = HttpContext.Current.Server.MapCustomPath(relativePath);
            }
            else
            {
                fullpath = AppDomain.CurrentDomain.BaseDirectory + relativePath;
            }

            return fullpath;
        }

        /// <summary>
        /// from html file get its content
        /// </summary>
        /// <param name="htmlFilePath">html file path</param>
        /// <returns>html contents</returns>
        public static string ReadHtmlFile(string htmlFilePath)
        {
            string filePath = HttpContext.Current.Server.MapCustomPath(ConfigSettings.TemplateBasePath + htmlFilePath);
            //string filePath = Path.Combine(baseConfirmationPath, htmlFilePath);
            string htmlString = File.ReadAllText(filePath);
            return htmlString;
        }

        public static List<NameValueData> SetUpload835FileProcessStatusFilter()
        {
            return new List<NameValueData>
                        {
                            new NameValueData{Name =Constants.All,Value = -1},
                            new NameValueData{Name = Convert.ToString(Resource.UnProcessed),Value = (int)EnumUpload835FileProcessStatus.UnProcessed},
                            new NameValueData{Name = Convert.ToString(Resource.InProcess),Value = (int)EnumUpload835FileProcessStatus.InProcess},
                            new NameValueData{Name = Convert.ToString(Resource.Processed),Value = (int)EnumUpload835FileProcessStatus.Processed},
                            new NameValueData{Name = Convert.ToString(Resource.ErrorInProcess),Value = (int)EnumUpload835FileProcessStatus.ErrorInProcess}
                        };
        }

        public static List<NameValueData> SetBatchStatusFilter()
        {
            return new List<NameValueData>
                        {
                            new NameValueData{Name = Constants.All,Value = -1},
                            new NameValueData{Name = Constants.Sent,Value = 1},
                            new NameValueData{Name = Constants.UnSent,Value = 0}
                        };
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

        public static string GenrtaeShortUrl(string URL)
        {
            if (!ConfigSettings.UseBitly)
                return URL;
            string url = null;
            using (WebClient wb = new WebClient())
            {
                string data = string.Format(Constants.BitlyUrl,
                ConfigSettings.BitlyUserName, ConfigSettings.BitlyApiKey, HttpUtility.UrlEncode(URL), "xml");
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(wb.DownloadString(data));
                url = xmlDoc.GetElementsByTagName("url")[0].InnerText;
            }
            return url;
        }

        public static ServiceResponse CreateAuditTrail<T>(AuditActionType action, long parentKeyFieldId, long? childKeyFieldId, T oldObject, T newObject, long loggedInUserID)
        {
            CompareObject compareObject = new CompareObject();
            AuditTrailModel model = compareObject.GetAuditTrailModel(action, parentKeyFieldId, childKeyFieldId, oldObject, newObject);
            if (model == null) return null;

            AuditLogTable audit = new AuditLogTable();
            audit.AuditActionType = model.AuditActionType;
            audit.DataModel = model.DataModel;
            audit.DateTimeStamp = model.DateTimeStamp;
            audit.ParentKeyFieldID = model.ParentKeyFieldID;
            audit.ChildKeyFieldID = model.ChildKeyFieldID;
            audit.ValueBefore = model.ValueBefore;
            audit.ValueAfter = model.ValueAfter;
            audit.Changes = model.Changes;

            IAuditLogDataProvider auditLogDataProvider = new AuditLogDataProvider();
            ServiceResponse response = auditLogDataProvider.AddAuditLog(audit, loggedInUserID);

            return response;
        }

        public static DateTime GetLastDayOfMonth(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
        }

        public static DateTime GetDayOfMonth(DateTime dateTime, int day)
        {
            return new DateTime(dateTime.Year, dateTime.Month, day);
        }


        public static DateTime GetOrgCurrentDateTime()
        {
            CacheHelper cache = new CacheHelper();
            var timeUtc = DateTime.UtcNow;
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(cache.TimeZone);
            var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);
            return today;
        }

        public static DateTime ConvertDateToOrgTimeZone(DateTime date)
        {
            CacheHelper cache = new CacheHelper();
            var timeUtc = date;
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(cache.TimeZone);
            var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);
            return today;
        }

        public static DateTime LocalToOrgDateTime(DateTime date)
        {
            date = date.ToUniversalTime();
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
        }

        public static string GetUnitType(int type)
        {
            if (type == (int)EnumUnitType.Time)
                return Resource.Time;
            if (type == (int)EnumUnitType.Visit)
                return Resource.Visit;
            if (type == (int)EnumUnitType.DistanceInMiles)
                return Resource.Miles;
            if (type == (int)EnumUnitType.Stop)
                return Resource.Stop;
            return "";
        }

        public static string GetUnitLimitFrequency(int? frequency)
        {
            if (frequency == (int)WorkingHourType.Month)
                return Resource.Month;
            if (frequency == (int)EnumUnitType.Visit)
                return Resource.Week;
            return Resource.Day;
        }


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

        public static Byte[] ReturnByteArrayFromStringForitextSharpPDF(string stringContent)
        {
            //Create a byte array that will eventually hold our final PDF
            Byte[] bytes;

            //Boilerplate iTextSharp setup here
            //Create a stream that we can write to, in this case a MemoryStream
            using (var ms = new MemoryStream())
            {
                //Create an iTextSharp Document which is an abstraction of a PDF but **NOT** a PDF
                var doc = new Document();

                //Create a writer that's bound to our PDF abstraction and our stream
                var writer = PdfWriter.GetInstance(doc, ms);
                //Open the document for writing
                doc.Open();

                //Our sample HTML and CSS
                var example_html = "@" + stringContent;

                //XMLWorker also reads from a TextReader and not directly from a string
                using (var srHtml = new StringReader(example_html))
                {
                    //Parse the HTML
                    iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, srHtml);
                }

                doc.Close();
                //After all of the PDF "stuff" above is done and closed but **before** we
                //close the MemoryStream, grab all of the active bytes from the stream
                bytes = ms.ToArray();

                return bytes;
            }
        }

        public static List<NameValueDataInString> SetThroughoutWeekend()
        {
            return new List<NameValueDataInString>
                        {
                            new NameValueDataInString{Name = Resource.Happy,Value = Constants.Happy},
                            new NameValueDataInString{Name = Resource.Sad,Value = Constants.Sad},
                            new NameValueDataInString{Name = Resource.Quiet,Value= Constants.Quiet},
                            new NameValueDataInString{Name = Resource.Talkative,Value= Constants.Talkative},
                            new NameValueDataInString{Name = Resource.Sleepy,Value= Constants.Sleepy},
                            new NameValueDataInString{Name = Resource.FeelingSick,Value= Constants.FeelingSick},
                            new NameValueDataInString{Name = Resource.Helpful,Value= Constants.Helpful},
                            new NameValueDataInString{Name = Resource.Whiny,Value= Constants.Whiny},
                            new NameValueDataInString{Name = Resource.Overactive,Value= Constants.Overactive},
                            new NameValueDataInString{Name = Resource.Bossy,Value= Constants.Bossy},
                            new NameValueDataInString{Name = Resource.Aggressive,Value= Constants.Aggressive},
                            new NameValueDataInString{Name = Resource.onwStaff,Value= Constants.onwStaff},
                            new NameValueDataInString{Name = Resource.Playful,Value= Constants.Playful},
                            new NameValueDataInString{Name = Resource.Demanding,Value= Constants.Demanding},
                            new NameValueDataInString{Name = Resource.Cuddly,Value= Constants.Cuddly},
                            new NameValueDataInString{Name = Resource.Silly,Value= Constants.Silly},
                            new NameValueDataInString{Name = Resource.Angry,Value= Constants.Angry},
                            new NameValueDataInString{Name = Resource.Inquisitive,Value= Constants.Inquisitive},
                            new NameValueDataInString{Name = Resource.Sociable,Value= Constants.Sociable},
                            new NameValueDataInString{Name = Resource.Excitable,Value= Constants.Excitable},
                            new NameValueDataInString{Name = Resource.OtherDetails,Value= Constants.OtherDetails},
                        };
        }

        public static List<NameValueDataInString> SetCoordinationofCare()
        {
            return new List<NameValueDataInString>
                        {
                            new NameValueDataInString{Name = Resource.MedicationLable,Value= Constants.MedicationLable},
                            new NameValueDataInString{Name = Resource.BXReport,Value= Constants.BXReport},
                            new NameValueDataInString{Name = Resource.Activity,Value= Constants.Activity},
                            new NameValueDataInString{Name = Resource.OtherNewLable,Value= Constants.Other},
                        };
        }

        public static List<NameValueDataInString> SetSummaryofFood()
        {
            return new List<NameValueDataInString>
                        {
                            new NameValueDataInString{Name = Resource.All,Value = Constants.All},
                            new NameValueDataInString{Name = Resource.Some,Value = Constants.Some},
                            new NameValueDataInString{Name = Resource.Iwasnthungry,Value= Constants.Hungry},
                        };
        }

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
                            new NameValueData{Name = "5",Value= 5},
                            new NameValueData{Name = "10",Value= 10},
                            new NameValueData{Name = "15",Value= 15},
                            new NameValueData{Name = "30",Value= 30},
                            new NameValueData{Name = "45",Value= 45}
                        };
        }

        public static List<NameValueDataInString> GetAgencyTypes()
        {
            return new List<NameValueDataInString>
                        {
                            new NameValueDataInString{Name = AgencyTypeEnum.ReferralSource.ToString(),Value= AgencyTypeEnum.ReferralSource.ToString() },
                            new NameValueDataInString{Name = AgencyTypeEnum.CareGiver.ToString(),Value= AgencyTypeEnum.CareGiver.ToString() },
                            new NameValueDataInString{Name = AgencyTypeEnum.Self.ToString(),Value= AgencyTypeEnum.Self.ToString() }
            };
        }

        public static List<NameValueDataInString> SetCoordinationofcare()
        {
            return new List<NameValueDataInString>
                        {
                            new NameValueDataInString{Name = Resource.InPerson,Value = Constants.InPerson},
                            new NameValueDataInString{Name = Resource.ViaPhone,Value= Constants.ViaPhone},
                            //new NameValueData{Name = Resource.Unmarked,Value= 5},
                        };
        }



        public static List<NameValueDataInString> SetReconcileStatus()
        {
            return new List<NameValueDataInString>
                        {
                            new NameValueDataInString{Name = Constants.Paid,Value= Constants.Paid},
                            new NameValueDataInString{Name = Constants.Denied,Value= Constants.Denied},
                            new NameValueDataInString{Name = Constants.InProcess,Value= Constants.InProcess},
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

        public static List<NameValueDataInString> GetPreferenceKeyType()
        {
            return new List<NameValueDataInString>
                {
                    new NameValueDataInString {Name = Preference.PreferenceKeyType.Preference.ToString(), Value = Preference.PreferenceKeyType.Preference.ToString()},
                    new NameValueDataInString {Name = Preference.PreferenceKeyType.Skill.ToString(), Value = Preference.PreferenceKeyType.Skill.ToString()}
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

        public static DataTable CSVtoDataTable(string filePath)
        {
            int count = 0;
            char fieldSeparator = ',';
            DataTable csvData = new DataTable();

            using (TextFieldParser csvReader = new TextFieldParser(filePath))
            {
                csvReader.HasFieldsEnclosedInQuotes = true;
                while (!csvReader.EndOfData)
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    string[] fieldData = csvReader.ReadFields();
                    if (count == 0)
                    {
                        foreach (string column in fieldData)
                        {
                            DataColumn datecolumn = new DataColumn(column);
                            datecolumn.AllowDBNull = true;
                            csvData.Columns.Add(datecolumn);
                        }
                        count++;
                    }
                    else
                    {
                        csvData.Rows.Add(fieldData);
                    }

                }
            }

            return csvData;

        }

        public static ClaimIdentifierModel GetBatchNoteIdFromResponse(string claimSubmitterIdentifier)
        {
            ClaimIdentifierModel model = new ClaimIdentifierModel();
            if (claimSubmitterIdentifier.Contains("ZRPB") && claimSubmitterIdentifier.Contains("BN"))
            {
                string[] strTemp = claimSubmitterIdentifier.Split(new string[] { "ZRPB" }, StringSplitOptions.None);
                model.NoteId = Convert.ToInt64(strTemp[0]);
                model.BatchId = Convert.ToInt64(strTemp[1].Split(new string[] { "BN" }, StringSplitOptions.None)[0]);
                model.BatchNoteId = Convert.ToInt64(strTemp[1].Split(new string[] { "BN" }, StringSplitOptions.None)[1]);
            }
            else if (claimSubmitterIdentifier.Contains("N"))
            {

                string[] strTemp = claimSubmitterIdentifier.Trim('N').Split(new string[] { "N" }, StringSplitOptions.None);
                model.BatchNoteId = Convert.ToInt64(strTemp[0]);
                model.BatchNoteId02 = Convert.ToInt64(strTemp[1]);
            }

            return model;
        }


        #region ----- Organization Preference -----

        public static string GetOrgRegion()
        {
            string region = OrgPreferenceRegions.US.Value;

            CacheHelper_MyezCare ch_MyezcareOrg = new CacheHelper_MyezCare();
            MyEzcareOrganization myOrg = ch_MyezcareOrg.GetCachedData<MyEzcareOrganization>();

            IOrgPreferenceDataProvider _dataProvider = new OrgPreferenceDataProvider();
            var preference = _dataProvider.Preferences(myOrg.OrganizationID);

            if (preference != null) { region = preference.Region; }

            return region;
        }

        public static bool IsOrgHasAggregator()
        {
            CacheHelper cacheHelper = new CacheHelper();
            return cacheHelper.HasAggregator;
        }

        public static string GetOrgFormattedCSS()
        {
            string css_Path = "";
            var preference = SessionHelper.OrganizationPreference;
            if (preference != null && !string.IsNullOrEmpty(preference.CssFilePath))
            { css_Path = preference.CssFilePath; }
            return css_Path;
        }

        public static string GetOrgLanguage()
        {
            string language = null;
            var preference = SessionHelper.OrganizationPreference;
            if (preference != null && !string.IsNullOrEmpty(preference.Language))
            { language = preference.Language; }
            return language;
        }

        public static string GetOrgWeekStartDay()
        {
            string weekStartDay = "1";
            var preference = SessionHelper.OrganizationPreference;
            if (preference != null && !string.IsNullOrEmpty(preference.WeekStartDay))
            { weekStartDay = preference.WeekStartDay; }
            return weekStartDay;
        }


        public static string GetOrgDateFormat()
        {
            string format = "DD/MM/YYYY";
            var preference = SessionHelper.OrganizationPreference;
            if (preference != null && !string.IsNullOrEmpty(preference.DateFormat))
            { format = preference.DateFormat; }
            return format.Replace("DD", "dd").Replace("YYYY", "yyyy");
        }
        public static string ConvertToOrgDateFormat(DateTime date)
        {

            return date == null ? "" : date.ToString(GetOrgDateFormat());
        }

        public static string GetOrgCurrencyFormat()
        {
            string symbole = "$";
            var preference = SessionHelper.OrganizationPreference;
            if (preference != null && !string.IsNullOrEmpty(preference.Currency))
            {
                var currency = preference.DateFormat;
                OrgPreferenceDataProvider _dataProvider = new OrgPreferenceDataProvider();
                ServiceResponse response = _dataProvider.GetPreference(preference.OrganizationID);
                if (response.Data != null)
                {
                    var selectedcurrency = (response.Data as OrgPreferenceModel).CurrencyList.Where(a => a.CurrencyID == Convert.ToInt32((response.Data as OrgPreferenceModel).OrganizationPreference.Currency)).FirstOrDefault();
                    if (selectedcurrency != null)
                        symbole = selectedcurrency.Symbol;
                }                
            }
            return symbole;
        }
        public static int GetCalWeekStartDay()
        {
            return Math.Max(0, Convert.ToInt32(Common.GetOrgWeekStartDay()) - 1);
        }

        public static DateTime GetOrgStartOfWeek()
        {
            return GetOrgCurrentDateTime().StartOfWeek((DayOfWeek)Common.GetCalWeekStartDay());
        }

        public static string GetOrgFormattedName(string first, string last, string middle)
        {
            string formattedName = "";
            var preference = SessionHelper.OrganizationPreference;

            if (preference == null) formattedName = string.Format("{0} {1}", first, last);
            else
            {
                switch (preference.NameDisplayFormat)
                {
                    case "First Last":
                        formattedName = string.Format("{0} {1}", first, last);
                        break;
                    case "Last First":
                        formattedName = string.Format("{0} {1}", last, first);
                        break;
                    case "First, Last":
                        formattedName = string.Format("{0}, {1}", first, last);
                        break;
                    case "Last, First":
                        formattedName = string.Format("{0}, {1}", last, first);
                        break;
                    case "Last, First Middle":
                        formattedName = string.Format("{0}, {1} {2}", last, first, middle);
                        break;
                    case "First Middle Last":
                        formattedName = string.Format("{0} {1}", first, middle, last);
                        break;
                }
            }

            return formattedName;
        }

        #endregion

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


        public static LatLong GetLatLongByAddress(string address)
        {
            var root = new GetLatLongByAddress.RootObject();

            var url =
                string.Format(
                    "http://maps.googleapis.com/maps/api/geocode/json?address={0}", address);
            var req = (HttpWebRequest)WebRequest.Create(url);

            var res = (HttpWebResponse)req.GetResponse();

            using (var streamreader = new StreamReader(res.GetResponseStream()))
            {
                var result = streamreader.ReadToEnd();

                if (!string.IsNullOrWhiteSpace(result))
                {
                    root = JsonConvert.DeserializeObject<GetLatLongByAddress.RootObject>(result);
                }
            }
            LatLong latlong = new LatLong();
            if (root.results.Count > 0)
            {
                latlong.Latitude = Convert.ToDouble(root.results[0].geometry.location.lat, CultureInfo.InvariantCulture);
                latlong.Longitude = Convert.ToDouble(root.results[0].geometry.location.lng, CultureInfo.InvariantCulture);
            }
            else
            {
                latlong = null;
            }
            return latlong;
        }

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

        public static string GetFolderPath(int fileType)
        {
            CacheHelper _cacheHelper = new CacheHelper();
            string fileDestinationPath;
            string uploadPath = String.Format(_cacheHelper.UploadPath, _cacheHelper.Domain);
            switch (fileType)
            {
                case (int)FileStorePathType.TempPath:
                    fileDestinationPath = uploadPath + ConfigSettings.TempPath;
                    break;
                case (int)FileStorePathType.EmpSignatures:
                    fileDestinationPath = uploadPath + ConfigSettings.EmpSignatures;
                    break;
                case (int)FileStorePathType.SiteLogoPath:
                    fileDestinationPath = uploadPath + ConfigSettings.SiteLogoPath;
                    break;
                case (int)FileStorePathType.EmpProfileImg:
                    fileDestinationPath = uploadPath + ConfigSettings.EmpProfileImg;
                    break;
                case (int)FileStorePathType.RefProfileImg:
                    fileDestinationPath = uploadPath + ConfigSettings.RefProfileImg;
                    break;
                default:
                    fileDestinationPath = uploadPath;
                    break;
            }

            return fileDestinationPath;
        }

        public enum FileStorePathType
        {
            TempPath = 0, EmpSignatures, SiteLogoPath, EmpProfileImg, RefProfileImg
        }


        public static SqlGeography GetSqlGeoPointFromLatLong(double latitude, double longitude)
        {
            return SqlGeography.Point(latitude, longitude, 4326);
        }


        public enum TimeSlotSaveResult
        {
            SQLException = -1, TimeSlotExistsNoRecordsAdded = -2, TimeSlotsPartiallyAdded = 2, TimeSlotsAdded = 1, TimeSlotExistInAnotherDateRange = -3, TimeSlotIsMoreThanAllowed = -4, ActiveBillingAuthorizationsIsRequired = -5
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

        public enum WorkingHourType
        {
            [Display(ResourceType = typeof(Resource), Name = "Day")]
            Day = 1,
            [Display(ResourceType = typeof(Resource), Name = "Week")]
            Week = 2,
            [Display(ResourceType = typeof(Resource), Name = "Month")]
            Month = 3
        }

        public enum PayorInvoiceType
        {
            [Display(ResourceType = typeof(Resource), Name = "Insurance")]
            Insurance = 1,
            [Display(ResourceType = typeof(Resource), Name = "Cash")]
            Cash = 2
        }

        public static List<NameValueData> SetPayorInvoiceTypeList()
        {
            return new List<NameValueData>
                {
                    new NameValueData {Name = EnumHelper<PayorInvoiceType>.GetDisplayValue(PayorInvoiceType.Insurance), Value = (int)PayorInvoiceType.Insurance},
                    new NameValueData {Name = EnumHelper<PayorInvoiceType>.GetDisplayValue(PayorInvoiceType.Cash), Value = (int)PayorInvoiceType.Cash}
                };
        }

        public enum InvoiceGenerationFrequencies
        {
            [Display(ResourceType = typeof(Resource), Name = "VisitWise")]
            Schedulewise = 1,
            [Display(ResourceType = typeof(Resource), Name = "Weekly")]
            Weekly = 2,
            [Display(ResourceType = typeof(Resource), Name = "Monthly")]
            Monthly = 3
        }

        public static List<NameValueData> SetInvoiceGenerationFrequencyList()
        {
            return new List<NameValueData>
                {
                    new NameValueData {Name = EnumHelper<InvoiceGenerationFrequencies>.GetDisplayValue(InvoiceGenerationFrequencies.Schedulewise), Value = (int)InvoiceGenerationFrequencies.Schedulewise},
                    new NameValueData {Name = EnumHelper<InvoiceGenerationFrequencies>.GetDisplayValue(InvoiceGenerationFrequencies.Weekly), Value = (int)InvoiceGenerationFrequencies.Weekly},
                    new NameValueData {Name = EnumHelper<InvoiceGenerationFrequencies>.GetDisplayValue(InvoiceGenerationFrequencies.Monthly), Value = (int)InvoiceGenerationFrequencies.Monthly}
                };
        }

        public enum BeneficiaryType
        {
            [Display(ResourceType = typeof(Resource), Name = "Medicaid")]
            Medicaid = 1,
            [Display(ResourceType = typeof(Resource), Name = "Medicare")]
            Medicare = 2,
            [Display(ResourceType = typeof(Resource), Name = "TricareChampus")]
            TricareChampus = 3,
            [Display(ResourceType = typeof(Resource), Name = "Champva")]
            Champva = 4,
            [Display(ResourceType = typeof(Resource), Name = "GroupHealthPlan")]
            GroupHealthPlan = 5,
            [Display(ResourceType = typeof(Resource), Name = "FecaBlkLung")]
            FecaBlkLung = 6,
            [Display(ResourceType = typeof(Resource), Name = "Other")]
            Other = 7
        }

        public static List<NameValueData> SetBeneficiaryType()
        {
            return new List<NameValueData>
                {
                    new NameValueData {Name = EnumHelper<BeneficiaryType>.GetDisplayValue(BeneficiaryType.Medicaid), Value = (int)BeneficiaryType.Medicaid},
                    new NameValueData {Name = EnumHelper<BeneficiaryType>.GetDisplayValue(BeneficiaryType.Medicare), Value = (int)BeneficiaryType.Medicare},
                    new NameValueData {Name = EnumHelper<BeneficiaryType>.GetDisplayValue(BeneficiaryType.TricareChampus), Value = (int)BeneficiaryType.TricareChampus},
                    new NameValueData {Name = EnumHelper<BeneficiaryType>.GetDisplayValue(BeneficiaryType.Champva), Value = (int)BeneficiaryType.Champva},
                    new NameValueData {Name = EnumHelper<BeneficiaryType>.GetDisplayValue(BeneficiaryType.GroupHealthPlan), Value = (int)BeneficiaryType.GroupHealthPlan},
                    new NameValueData {Name = EnumHelper<BeneficiaryType>.GetDisplayValue(BeneficiaryType.FecaBlkLung), Value = (int)BeneficiaryType.FecaBlkLung},
                    new NameValueData {Name = EnumHelper<BeneficiaryType>.GetDisplayValue(BeneficiaryType.Other), Value = (int)BeneficiaryType.Other}
                };
        }

        public enum EmployeeDesignationType
        {
            [Display(ResourceType = typeof(Resource), Name = "CaseManager")]
            CaseManager = 1
        }

        public static List<NameValueData> SetEmployeeDesignationTypeList()
        {
            return new List<NameValueData>
                {
                    new NameValueData {Name = EnumHelper<EmployeeDesignationType>.GetDisplayValue(EmployeeDesignationType.CaseManager), Value = (int)EmployeeDesignationType.CaseManager},
                };
        }

        public enum CareTypeFrequency
        {
            [Display(ResourceType = typeof(Resource), Name = "Time")]
            Time = 1,
            [Display(ResourceType = typeof(Resource), Name = "Week")]
            Week = 7,
            [Display(ResourceType = typeof(Resource), Name = "Month")]
            Month = 30,
            [Display(ResourceType = typeof(Resource), Name = "Year")]
            Year = 365
        }

        public static List<NameValueData> SetCareTypeFrequencyList()
        {
            return new List<NameValueData>
                {
                    new NameValueData {Name = EnumHelper<CareTypeFrequency>.GetDisplayValue(CareTypeFrequency.Time), Value = (int)CareTypeFrequency.Time},
                    new NameValueData {Name = EnumHelper<CareTypeFrequency>.GetDisplayValue(CareTypeFrequency.Week), Value = (int)CareTypeFrequency.Week},
                    new NameValueData {Name = EnumHelper<CareTypeFrequency>.GetDisplayValue(CareTypeFrequency.Month), Value = (int)CareTypeFrequency.Month},
                    new NameValueData {Name = EnumHelper<CareTypeFrequency>.GetDisplayValue(CareTypeFrequency.Year), Value = (int)CareTypeFrequency.Year}
                };
        }

        public enum InvoiceStatus
        {
            [Display(ResourceType = typeof(Resource), Name = "Unpaid")]
            Unpaid = 1,
            [Display(ResourceType = typeof(Resource), Name = "Paid")]
            Paid,
            [Display(ResourceType = typeof(Resource), Name = "PartialPaid")]
            PartialPaid,
            [Display(ResourceType = typeof(Resource), Name = "Void")]
            Void
        }

        public static List<NameValueData> SetInvoiceStatusList()
        {
            return new List<NameValueData>
                {
                    new NameValueData {Name = EnumHelper<InvoiceStatus>.GetDisplayValue(InvoiceStatus.Unpaid), Value = (int)InvoiceStatus.Unpaid},
                    new NameValueData {Name = EnumHelper<InvoiceStatus>.GetDisplayValue(InvoiceStatus.Paid), Value = (int)InvoiceStatus.Paid},
                    new NameValueData {Name = EnumHelper<InvoiceStatus>.GetDisplayValue(InvoiceStatus.PartialPaid), Value = (int)InvoiceStatus.PartialPaid},
                    new NameValueData {Name = EnumHelper<InvoiceStatus>.GetDisplayValue(InvoiceStatus.Void), Value = (int)InvoiceStatus.Void},
                };
        }

        public static List<NameValueDataInString> SetOrgTypeList(string[] list)
        {
            List<NameValueDataInString> nameValueList = new List<NameValueDataInString>();
            foreach (string item in list)
            {
                nameValueList.Add(new NameValueDataInString { Name = string.Concat(item.Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' '), Value = item });
            }
            return nameValueList;
        }

        public static Color HexToColor(string hexColor)
        {
            //Remove # if present
            if (hexColor.IndexOf('#') != -1)
                hexColor = hexColor.Replace("#", "");

            int red = 0;
            int green = 0;
            int blue = 0;

            if (hexColor.Length == 6)
            {
                //#RRGGBB
                red = int.Parse(hexColor.Substring(0, 2), NumberStyles.AllowHexSpecifier);
                green = int.Parse(hexColor.Substring(2, 2), NumberStyles.AllowHexSpecifier);
                blue = int.Parse(hexColor.Substring(4, 2), NumberStyles.AllowHexSpecifier);
            }
            else if (hexColor.Length == 3)
            {
                //#RGB
                red = int.Parse(hexColor[0].ToString() + hexColor[0].ToString(), NumberStyles.AllowHexSpecifier);
                green = int.Parse(hexColor[1].ToString() + hexColor[1].ToString(), NumberStyles.AllowHexSpecifier);
                blue = int.Parse(hexColor[2].ToString() + hexColor[2].ToString(), NumberStyles.AllowHexSpecifier);
            }

            return Color.FromArgb(red, green, blue);
        }

        public static List<string> AddSpaceBeforeCapLetters(string[] list)
        {
            var str = new List<string>();
            foreach (string item in list)
            {
                str.Add(string.Concat(item.Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' '));
            }

            return str;
        }
        public enum AttendanceDetailType
        {
            ClockIn = 1,
            ClockOut = 2,
            Break = 3,
            Resume = 4
        }
    }

    public static class CombinationGenerator
    {
        public static IEnumerable<IEnumerable<T>> DifferentCombinations<T>(this IList<T> elements, int k)
        {
            return k == 0 ? new[] { new T[0] } :
              elements.SelectMany((e, i) =>
                elements.Skip(i + 1).ToList().DifferentCombinations(k - 1).Select(c => (new[] { e }).Concat(c)));
        }
    }


    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        private static readonly long DatetimeMinTimeTicks = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks;
        public static long ToJavaScriptMS(this DateTime dt)
        {
            return (long)((dt.ToUniversalTime().Ticks - DatetimeMinTimeTicks) / 10000);
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