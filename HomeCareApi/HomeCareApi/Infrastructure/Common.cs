using HomeCareApi.Models.ApiModel;
using HomeCareApi.Models.ViewModel;
using HomeCareApi.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Elmah;
using HomeCareApi.Infrastructure.DataProvider;
using HomeCareApi.Models.Entity;
using PetaPoco;
using HomeCareApi.Models.General;
using Twilio;
using System.Net.NetworkInformation;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using System.Web.Script.Serialization;
using Zarephath.Core.Infrastructure.Utility.Fcm;
using System.Data.SqlClient;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HomeCareApi.Infrastructure
{
    public class Common
    {
        #region Enum

        /// <summary>
        /// enum for the user role
        /// </summary>
        public enum Role
        {
            SuperAdmin = 1,
            EndUser
        }

        public enum SpTransactionResult
        {
            TransactionFailed = -1,
            RecordNotFoundOrAffected = 0,
            RecordSaved = 1,
            RecordUpdated = 2,
            RecordDeleted = 3,
            IsDuplicateFound = 4
        }

        public enum SetValue
        {
            CurrentTime = 1,
            LoggedInUserId
        }

        public enum SearchOperator
        {
            EqualTo = 1,
            NotEqualTo,
            BeginsWith,
            EndsWith,
            Contains,
            DoesNotContains,
            GreaterThan,
            LessThan
        }

        /// <summary>
        /// enum for the firm priority
        /// </summary>
        public enum FirmPriority
        {
            One = 1,
            Two = 2,
            Default = 3
        }

        /// <summary>
        /// enum for the post priority
        /// </summary>
        public enum PostPriority
        {
            Default = 3
        }

        public enum NotificationType
        {
            UserSignUp = 1,
            AskShowSample = 2,
            ViewFirmCount = 3,
            DailyPost = 4,
            PostNotCreated = 5,
            FirmNotCreated = 6,
            AdminApprovePostReport = 7,
            AdminDenyPostReport = 8,
            AdminApproveFirmReport = 9,
            AdminDenyFirmReport = 10,
            ChatNotification = 11,
            OTPNotification = 12,
            UserNotification = 13,
            LinkNotification = 14,
            ArticleNotification = 15,
            PostRedirectNotification = 16,
            FirmRedirectNotification = 17,
            UserNotLoggedNotification = 18,
            SendConnectionRequest = 19

        }

        public enum DDType
        {
            CareType = 1,
            PayerGroup = 2,
            BussinessLine = 3,
            NPIOptions = 4,
            RevenueCode = 5,
            AdmissionType = 6,
            AdmissionSource = 7,
            PatientStatus = 8,
            VisitType = 9,
            FacilityCode = 10,
            PatientFrequencyCode = 11,
            PatientSystemStatus = 12,
            TaskFrequencyCode = 13,
            AssessmentQuestionCategory = 14,
            AssessmentQuestionSubCategory = 15,
            Gender = 16,
            LanguagePreference = 17,
            Designation = 18
        }

        public enum ScheduleStatuses
        {
            Unconfirmed = 1, Confirmed = 2, LeftMessage = 3, NoAnswer = 4, NoPhone = 5,
            Cancelled = 6, NoConfirmation = 7, WaitingList = 8, NoShow = 9
        }

        public enum ReferralStatuses
        {
            Active = 1,
            ProBono = 2,
            Inactive = 3,
            Discharged = 4,
            NewReferral = 5,
            IncompleteReferral = 6,
            InactiveReferral = 7,
            DormantReferral = 8,
            LifeSkillsOnly = 9,
            ReferralAccepted = 10,
            ReferralInitialReview = 11,
            ReferralOnHold = 12,
            ReferralDenied = 13,
            ConnectingFamilies = 14
        }
        public enum NotificationProcess
        {
            NotProcess = 0,
            InProcess = 2,
            Complete = 1,
            Error = -1
        }

        /// <summary>
        /// Enum for list of email templates in the system
        /// </summary>
        public enum EmailTemplateList
        {
            ContactUs = 1,
            Feedback
        }

        /// <summary>
        /// Enum for email service states
        /// </summary>
        public enum EmailLogStatus
        {
            EmailSendingFailed = -1, EmailSaved = 0, EmailSentSuccessfully = 1, EmailInProgress = 2
        }

        public enum PropertyType
        {
            Sell = 1,
            Rent,
            Both
        }

        public enum SmsLogStatus
        {
            Failed = -1, Sent = 1, InProgress = 0
        }

        public enum SmsTypeEnum
        {
            ClockIn = 1, ClockOut = 2
        }

        public enum UserType
        {
            [Display(ResourceType = typeof(Resource), Name = "Employee")]
            Employee = 1,
            [Display(ResourceType = typeof(Resource), Name = "Referral")]
            Referral = 2
        }
        #endregion Enum

        #region File Operation

        /// <summary>
        /// This metod will move file list and return the service response containing a data of converted file list in comma seperated value which 
        /// will be used in stored procedure.
        /// </summary>
        /// <param name="fileList">File list</param>
        /// <param name="sourcePath">source folder path</param>
        /// <param name="destinationPath">destination folder path where you want to move files</param>
        /// <param name="primaryId">Primary id to save into db for file identification</param>
        /// <param name="loggedInUserId">Loggedin userid</param>
        /// <returns>Service response contaning success and error</returns>
        public static ServiceResponse MoveFileList(List<FileModel> fileList, string sourcePath, string destinationPath, long primaryId, long loggedInUserId = 0)
        {
            ServiceResponse response = new ServiceResponse();
            List<FileModel> list = new List<FileModel>();
            try
            {
                foreach (var file in fileList)
                {
                    if (!string.IsNullOrEmpty(file.TempFileName))
                    {
                        var res = MoveFile(sourcePath, destinationPath, file.TempFileName);

                        if (res.IsSuccess)
                        {
                            FileModel model = new FileModel
                            {
                                IsCoverImage = file.IsCoverImage,
                                FileName = file.FileName,
                                TempFileName = res.Data.ToString(),
                                PrimaryId = primaryId,
                                IsFirmCoverImage = file.IsFirmCoverImage
                            };
                            list.Add(model);
                        }
                    }
                }
                List<string> dataList = list.Select(file => file.PrimaryId + Constants.PipeChar + file.FileName + Constants.PipeChar +
                                                            file.TempFileName + Constants.PipeChar + file.IsCoverImage + Constants.PipeChar + file.IsFirmCoverImage)
                    .ToList();
                response.IsSuccess = true;
                response.Data = string.Join(Constants.Comma, dataList);
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = SetExceptionMessage(Resource.ServerError + e.Message);
            }
            return response;
        }

        /// <summary>
        /// Move files from source to destinatation
        /// </summary>
        /// <param name="sourcePath">source path of file</param>
        /// <param name="destinationPath">destination path of file</param>
        /// <param name="fileNameWithExtension">file name with extention</param>
        /// <returns></returns>
        public static ApiResponse MoveFile(string sourcePath, string destinationPath, string fileNameWithExtension)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                string tempSourcePath = ConfigSettings.FolderBasePath + sourcePath;
                string tempDestinationPath = ConfigSettings.FolderBasePath + destinationPath;
                if (!(Directory.Exists(tempSourcePath) && File.Exists(string.Format("{0}{1}", tempSourcePath, fileNameWithExtension))))
                {
                    response.IsSuccess = false;
                    return response;
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
                //File.Copy(string.Format("{0}{1}", sourcePath, fileNameWithExtension), string.Format("{0}{1}", destinationPath, fileNameWithExtension));
                //File.Delete(string.Format("{0}{1}", sourcePath, fileNameWithExtension));

                File.Move(string.Format("{0}{1}", tempSourcePath, fileNameWithExtension), string.Format("{0}{1}", tempDestinationPath, fileNameWithExtension));

                response.IsSuccess = true;
                response.Message = string.Format("{0}{1}", destinationPath, fileNameWithExtension);
                response.Data = string.Format("{0}", fileNameWithExtension);
                return response;
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.IsSuccess = false;
            }
            return response;
        }

        public static ServiceResponse SaveFile(HttpPostedFileBase postedFile, string destinationPath, string fileName = "", string fileExtension = "")
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                Guid guid = Guid.NewGuid();
                string fName = guid + "-" + DateTime.Now.ToString(Constants.DateFormatForSaveFile);
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

        #endregion

        #region Email

        /// <summary>
        /// Method for send mail as per smtp section.
        /// </summary>
        /// <param name="mailSubject">subject of mail</param>
        /// <param name="fromEmail">from addess of mail </param>
        /// <param name="toEmails">to address of mail</param>
        /// <param name="messageBody">message</param>
        /// <param name="lstAttachments">list of attachments</param>
        /// <param name="sendMailSmtp">smtp section to send mail</param>
        /// <param name="loginUserID">logged in user id</param>
        /// <param name="replyMessageID"></param>
        /// <returns>it will return boolean value of send email status</returns>
        public static bool SendEmail(string mailSubject, string fromEmail, string toEmails, string messageBody,
                                    List<string> lstAttachments = null, string sendMailSmtp = "default", long? loginUserID = 0, string replyMessageID = "", bool orgTemplateWrapper = true)
        {

            MailMessage contactEmail = new MailMessage();
            
            if (orgTemplateWrapper)
            {
                string path = HttpContext.Current.Server.MapPath("~/Assets/emailtemplates/main_template.html");
                var builderMainTemplate = new StringBuilder();
                using (StreamReader SourceReader = File.OpenText(path))
                {
                    builderMainTemplate.Append(SourceReader.ReadToEnd());
                }
                //CacheHelper _cacheHelper = new CacheHelper();
                //CacheHelper_MyezcareOrganization ch_MyezcareOrg = new CacheHelper_MyezcareOrganization();
                SecurityDataProvider sc = new SecurityDataProvider();
                OrganizationSetting orgSettings = sc.GetOrganizationDetail();
                var DomainName = sc.OrganizationData.DomainName;
                string logoURL = string.Format("{0}{1}", "https://"+ DomainName + ".com",orgSettings.SiteLogo);// _cacheHelper.SiteBaseURL, _cacheHelper.SiteLogo);
                builderMainTemplate.Replace("##OrgLogoURL##", logoURL);
                string address = string.Format("{0}, {1}, {2} {3}", orgSettings.OrganizationAddress, orgSettings.OrganizationCity, orgSettings.OrganizationState, orgSettings.OrganizationZipcode);
                builderMainTemplate.Replace("##OrgAddress##", address);
                builderMainTemplate.Replace("##MessageBody##", messageBody);
                messageBody = builderMainTemplate.ToString();
            }

            SmtpSection section =  (SmtpSection)ConfigurationManager.GetSection("mailSettings/" + sendMailSmtp.ToLower()) ??
                                  (SmtpSection)ConfigurationManager.GetSection("mailSettings/default");

            contactEmail.SmtpSection = section;
            contactEmail.IsHtml = true;

            contactEmail.ToList.Add(toEmails);

            //if (!toEmails.Contains(Constants.Comma))
            //{
            //    contactEmail.ToList.Add(toEmails);
            //}
            //else
            //{
            //    foreach (string emailId in toEmails.Split(Constants.CommaChar))
            //    {
            //        contactEmail.ToList.Add(emailId);
            //    }
            //}

            if (string.IsNullOrEmpty(fromEmail))
                contactEmail.From = section == null ? "" : section.Network.UserName;
            else
                contactEmail.From = fromEmail;

            contactEmail.ReplyMessageID = replyMessageID;
            contactEmail.Subject = mailSubject;
            contactEmail.Body = messageBody;
            contactEmail.AttachmentList = lstAttachments;

            try
            {
                Common.CreateLogFile("mailSent sending");
                bool mailSent = EmailHelper.SendEmail(contactEmail);
                Common.CreateLogFile("mailSent sent");
                //IEmailUtilityDataProvider emailUtilityDataProvider = new EmailUtilityDataProvider();
                //EmailLog emailLog = new EmailLog
                //{
                //    ToEmail = toEmails,
                //    Subject = mailSubject,
                //    Body = messageBody,
                //    SentBy = loginUserID,
                //    IsSuccess = mailSent
                //};
                //emailUtilityDataProvider.SaveEmailLog(emailLog);
                //return true;
                return mailSent;
            }
            catch (Exception ex)
            {
                Common.CreateLogFile("error : " + ex.Message + " Stack Trace" + ex.StackTrace);
                ErrorLog.GetDefault(HttpContext.Current).Log(new Error(ex));
                //var signal = new ErrorSignal();
                //signal.Raise(ex);

                //if (HttpContext.Current == null)
                //{
                //    throw ex;
                //}
                //string error = string.Format("Log Created Date: {0}" + Environment.NewLine + "Error:{1}", DateTime.Now,
                //                             ex.Message);
                //error += Environment.NewLine + "---------------------------------------------------------------" +
                //         Environment.NewLine;

                //string exceptionFilePath =
                //    HttpContext.Current.Server.MapPath(ConfigSettings.EmailErrorLogPath);
                //if (!Directory.Exists(exceptionFilePath))
                //{
                //    Directory.CreateDirectory(exceptionFilePath);
                //}
                //string fileName = exceptionFilePath + Constants.EmailExceptionLog + DateTime.Today.ToString(Constants.DateFormatForLog) + Constants.TxtFile;
                //StreamWriter sr = new StreamWriter(fileName, true);
                //sr.WriteLine(error);
                //sr.Flush();
                //sr.Close();
                return false;
            }

        }

        #endregion

        #region Common Methods

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

        public static void CreateLogFile(string message, string logFileName = "", string logPath = "", bool logEnable = true, bool IsDatabaseEntry=false)
        {
            if (logEnable)
            {
                //var timeUtc = DateTime.UtcNow;
                //var timeZone = TimeZoneInfo.FindSystemTimeZoneById(GetTimeZone());
                //var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);

                //#region Log Entry in Database
                //using (SqlConnection con = new SqlConnection(ConfigSettings.ConnectionString))
                //{
                //    using (SqlCommand cmd = new SqlCommand("INSERT INTO Mem_Basic(Mem_Na,Mem_Occ) output INSERTED.ID VALUES(@na,@occ)", con))
                //    {
                //        cmd.Parameters.AddWithValue("@na", Mem_NA);
                //        cmd.Parameters.AddWithValue("@occ", Mem_Occ);
                //        con.Open();

                //        int modified = (int)cmd.ExecuteScalar();

                //        if (con.State == System.Data.ConnectionState.Open)
                //            con.Close();

                //        return modified;
                //    }
                //}

                //#endregion




                if (string.IsNullOrEmpty(logFileName))
                    logFileName = "Exception";

                if (string.IsNullOrEmpty(logPath))
                {
                    logPath = HttpContext.Current != null
                        ? HttpContext.Current.Server.MapPath(ConfigSettings.LogPath)
                        : Path.Combine(ConfigSettings.FolderBasePath + ConfigSettings.LogPath);
                }

                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }

                //Delete log files which are 15 days old
                if (logPath.ToLower().Contains("ivrlog"))
                {
                    string[] files = Directory.GetFiles(logPath);
                    foreach (string file in files)
                    {
                        FileInfo fi = new FileInfo(file);
                        if (fi.CreationTime < DateTime.Now.AddDays(-15))
                            fi.Delete();
                    }
                }

                string fileName = logPath + logFileName+"_" + DateTime.Today.ToString("MMddyyyy") + ".txt";
                var sr = new StreamWriter(fileName, true);
                sr.WriteLine("DateTime:" + DateTime.Now);
                sr.WriteLine(message);
                sr.WriteLine("===============================================================================================================");
                sr.Flush();
                sr.Close();
            }
        }

        /// <summary>
        /// This method will return list of page size.
        /// </summary>
        /// <returns></returns>
        public static string GetPageSizeList()
        {
            List<PageSizeList> list = new List<PageSizeList>();
            List<int> pageSizeList = string.IsNullOrEmpty(ConfigSettings.PageSizeList)
                ? null
                : ConfigSettings.PageSizeList.Split(',')
                    .Select(s => Convert.ToInt32(s))
                    .ToList();

            if (pageSizeList != null)
                list.AddRange(pageSizeList.Select(pagesize => new PageSizeList
                {
                    PageSize = pagesize
                }));
            return SerializeObject(list);
        }

        public static string ConstructQueryString(NameValueCollection parameters)
        {
            return string.Join("&", (from string name in parameters select String.Concat(name, "=", HttpUtility.UrlEncode(parameters[name]))).ToArray());
        }

        public static List<string> RelatedColors(string color)
        {
            List<string> list = new List<string>();
            Int32 iColorInt = Convert.ToInt32(color.Substring(1), 16);
            Color curveColor = System.Drawing.Color.FromArgb(iColorInt);

            for (byte i = 1; i <= 5; i++)
            {
                byte r = Convert.ToByte(curveColor.R + i);
                byte g = Convert.ToByte(curveColor.G + i);
                byte b = Convert.ToByte(curveColor.B + i);

                list.Add(Color.FromArgb(r, g, b).Name);
            }
            for (byte i = 1; i <= 5; i++)
            {
                byte r = Convert.ToByte(curveColor.R - i);
                byte g = Convert.ToByte(curveColor.G - i);
                byte b = Convert.ToByte(curveColor.B - i);

                list.Add(Color.FromArgb(r, g, b).Name);
            }
            return list;
        }

        #endregion Common Methods

        #region Api Response Handler Block

        /// <summary>
        /// Common method to show actual error or not
        /// </summary>
        /// <param name="message">actual error message</param>
        /// <param name="isDuplicateRecord">set true if want to show duplicate record save error</param>
        /// <param name="duplicateRecordField">field which is duplicate</param>
        /// <returns>return message as per config value</returns>
        public static string SetExceptionMessage(string message, bool isDuplicateRecord = false, string duplicateRecordField = "")
        {
            if (isDuplicateRecord)
            {
                return (ConfigSettings.IsShowActualError) ? message : string.Format(Resource.DuplicateRecord, duplicateRecordField);
            }
            return (ConfigSettings.IsShowActualError) ? message : Resource.ExceptionMessage;
        }

        /// <summary>
        /// This method will return server error response
        /// </summary>
        /// <param name="exceptionMessage">exception message passed</param>
        /// <returns></returns>
        public static ApiResponse InternalServerError(string exceptionMessage)
        {
            ApiResponse response = new ApiResponse
            {
                Code = StatusCode.InternalServerError,
                IsSuccess = false,
                Message = SetExceptionMessage(exceptionMessage)
            };
            return response;
        }

        /// <summary>
        /// This method will return server error response
        /// </summary>
        /// <param name="exceptionMessage">exception message passed</param>
        /// <returns></returns>
        public static ApiResponse<T> InternalServerError<T>(string exceptionMessage) where T : class
        {
            ApiResponse<T> response = new ApiResponse<T>
            {
                Code = StatusCode.InternalServerError,
                IsSuccess = false,
                Message = SetExceptionMessage(exceptionMessage)
            };
            return response;
        }

        /// <summary>
        /// This method will return server error response
        /// </summary>
        /// <param name="exceptionMessage">exception message passed</param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResponseList<T> InternalServerError<T>(string exceptionMessage, Page<T> data = null) where T : class
        {
            ApiResponseList<T> response = new ApiResponseList<T>
            {
                Code = StatusCode.InternalServerError,
                IsSuccess = false,
                Message = SetExceptionMessage(exceptionMessage),
                Data = data
            };
            return response;
        }

        /// <summary>
        /// this method will return the respone containing data 
        /// OBJECT Data
        /// </summary>
        /// <param name="isSuccess">response is success or not</param>
        /// <param name="message">response message</param>
        /// <param name="code">response http status code</param>
        /// <param name="data">if response has data</param>
        /// <returns>api response</returns>
        public static ApiResponse ApiCommonResponse(bool isSuccess, string message, string code, object data = null)
        {
            ApiResponse response = new ApiResponse
            {
                Data = data,
                Code = code,
                Message = message,
                IsSuccess = isSuccess
            };
            return response;
        }

        /// <summary>
        /// this method will return the respone containing data 
        /// Data having response as T
        /// </summary>
        /// <param name="isSuccess">response is success or not</param>
        /// <param name="message">response message</param>
        /// <param name="code">response http status code</param>
        /// <param name="data">if response has data</param>
        /// <returns>api response</returns>
        public static ApiResponse<T> ApiCommonResponse<T>(bool isSuccess, string message, string code, T data = null) where T : class
        {
            ApiResponse<T> response = new ApiResponse<T>
            {
                Data = data,
                Code = code,
                Message = message,
                IsSuccess = isSuccess
            };
            return response;
        }

        /// <summary>
        /// this method will return the respone containing data 
        /// Data having response list
        /// </summary>
        /// <param name="isSuccess">response is success or not</param>
        /// <param name="message">response message</param>
        /// <param name="code">response http status code</param>
        /// <param name="data">if response has data</param>
        /// <returns>api response</returns>
        public static ApiResponseList<T> ApiCommonResponseList<T>(bool isSuccess, string message, string code, Page<T> data = null) where T : class
        {
            ApiResponseList<T> response = new ApiResponseList<T>
            {
                Data = data,
                Code = code,
                Message = message,
                IsSuccess = isSuccess
            };
            return response;
        }

        #region Call For Direct Thrown error and return response

        public static void ThrowErrorMessage(string message, HttpStatusCode code = HttpStatusCode.NotAcceptable)
        {
            //string errorMessage = (ConfigSettings.IsShowActualError) ? message : Resource.ExceptionMessage;

            if (string.IsNullOrEmpty(message))
                message = Resource.TokenNotFound;

            var resp = new HttpResponseMessage(code)
            {
                Content = new StringContent("{\"Code\": \"410\",\"IsSuccess\": false,\"Message\": \"" + message + "\"}"),
                ReasonPhrase = message
            };
            throw new HttpResponseException(resp);
        }

        public static string GetDatabaseNameFromApi()
        {
            string databaseName = string.Empty;
            string companyName = string.Empty;
            try
            {
                var jsonSerializer = new JavaScriptSerializer();
                HttpContext.Current.Request.InputStream.Position = 0;

                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    companyName = HttpContext.Current.Request.Form["CompanyName"];
                }
                else
                {
                    var inputStream = new StreamReader(HttpContext.Current.Request.InputStream);

                    if (HttpContext.Current.Request.Url.ToString().Contains("/ivr/")) {

                        companyName = HttpContext.Current.Request.QueryString["CompanyName"];
                    }
                    else {

                        var str = inputStream.ReadToEnd();
                        var data = jsonSerializer.Deserialize<ApiRequest>(str);
                        companyName = data == null ? null : data.CompanyName;
                    }
                }

                //if (!string.IsNullOrEmpty(companyName))
                //{
                //    //string compName = string.Format("Application Name={0}", companyName.Trim());
                //    foreach (ConnectionStringSettings constr in ConfigurationManager.ConnectionStrings)
                //    {
                //        var strArray = constr.ConnectionString.Split(';');

                //        var applicationName = strArray.Where(c => c.Contains("Application Name")).FirstOrDefault();

                //        if (!string.IsNullOrEmpty(applicationName) && applicationName.Contains("=") && applicationName.Split('=')[1].Trim().ToLower() == companyName.Trim().ToLower())
                //        {
                //            databaseName = constr.Name;
                //            break;
                //        }
                //    }
                //}

                //if (string.IsNullOrEmpty(databaseName))
                //{
                //    ThrowErrorMessage(Resource.CompanyNotFound);
                //}

                if (string.IsNullOrEmpty(companyName))
                {
                    ThrowErrorMessage(Resource.CompanyNotFound);
                }

            }
            catch (Exception e)
            {
                ThrowErrorMessage(Resource.CompanyNotFound);
            }
            return companyName;
        }

        #endregion

        #region BadRequest and UnauthorizedUser and return direct response

        public static void BadRequest(ApiResponse response, HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(
                                                                          HttpStatusCode.NotAcceptable,
                response,
                actionContext.ControllerContext.Configuration.Formatters.JsonFormatter
                );
            actionContext.Response.Headers.CacheControl = new CacheControlHeaderValue
            {
                MaxAge = TimeSpan.FromSeconds(86400),
                MustRevalidate = true,
                Public = true

            };

            //actionContext.Response.TrySkipIisCustomErrors = true;

        }

        public static void UnauthorizedUser(ApiResponse response, HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(
                                                                          HttpStatusCode.NotAcceptable,
                response,
                actionContext.ControllerContext.Configuration.Formatters.JsonFormatter
                );
        }

        #endregion

        #endregion Api Response Handler Block

        #region Password

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

        #endregion

        #region Sms Code

        public static bool SendSms(string toNumber, string body, string emailType)
        {
            // Find your Account Sid and Auth Token at twilio.com/user/account
            BaseDataProvider dataProvider = new BaseDataProvider();

            SecurityDataProvider sc = new SecurityDataProvider();
            OrganizationSetting orgSettings= sc.GetOrganizationDetail();

            if (string.IsNullOrEmpty(orgSettings.TwilioAccountSID)
                || string.IsNullOrEmpty(orgSettings.TwilioAuthToken)
                || string.IsNullOrEmpty(orgSettings.TwilioFromNo)
                || string.IsNullOrEmpty(orgSettings.TwilioServiceSID)
                || string.IsNullOrEmpty(orgSettings.TwilioDefaultCountryCode)) {


                InsertEntryForSmsInEmailHistoryLogs(toNumber, body, false, dataProvider, emailType);
                return false;
            }


            


            //dataProvider.GetTwilioDetails();

            string AccountSid = orgSettings.TwilioAccountSID;//  ConfigSettings.TwilioAccountSID;
            string AuthToken = orgSettings.TwilioAuthToken; //ConfigSettings.TwilioAuthToken;
            //var twilio = new TwilioRestClient(AccountSid, AuthToken);
            TwilioClient.Init(orgSettings.TwilioAccountSID, orgSettings.TwilioAuthToken);
            try
            {
                // var message = twilio.SendMessage(ConfigSettings.TwilioFromNo, ConfigSettings.DefaultCountryCodeForSms + toNumber, body);
                var message = MessageResource.Create(
               orgSettings.TwilioDefaultCountryCode + toNumber,
                from: new PhoneNumber(orgSettings.TwilioFromNo),
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

        /// <summary>
        /// this method will return the otp code
        /// </summary>
        /// <returns></returns>
        public static string GetOtp()
        {
            OtpCodeGenerator generateOtp = new OtpCodeGenerator();
            var otpCode = generateOtp.GenerateRandomCode("", 6, 6, false, true, false, false);
            return otpCode;
        }

        #endregion

        public static string GetAccessPath(string path) =>
           string.IsNullOrEmpty(path) ? null : $"Uploads/AccessFile/{Crypto.Encrypt(path)}{Path.GetExtension(path)}";

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
            return HttpContext.Current.Request.UserHostAddress;
        }

        public static DateTime ConvertDateToOrgTimeZone(DateTime date)
        {
            SecurityDataProvider sc = new SecurityDataProvider();
            OrganizationSetting orgSettings = sc.GetOrganizationDetail();
            var timeUtc = date;
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(orgSettings.TimeZone);
            var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);
            return today;
        }

        public static string UploadPath()
        {
            SecurityDataProvider sc = new SecurityDataProvider();
            OrganizationSetting orgSettings = sc.GetOrganizationDetail();
            string path = null;
            if (orgSettings != null) { path = orgSettings.UploadPath; }
            if (string.IsNullOrEmpty(path)) { path = ConfigurationManager.AppSettings["UploadPath"]; }
            return path;
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
    }
}