using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Zarephath.Core.Models;
using System.Web.Mvc;
using System.Net.Mail;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Infrastructure.DataProvider;
using System.Web;


namespace Zarephath.Core.Infrastructure.Utility.CareGiverApi
{
    public class CareGiverApi : BaseDataProvider
    {
        private static string CareGiverApiUrl = "https://www.caregivertraininguniversity.com/api/ping ";//  "https://www.caregivertraininguniversity.com/api/ping";


        private static string CareGiverApi_BaseUrl = "https://www.caregivertraininguniversity.com/api";
        private string ProductTypeURL = "https://api.fda.gov/other/nsde.json?count=product_type";

        private string LanguageUrl = CareGiverApi_BaseUrl + "/languages/";
        private string CourseUrl = CareGiverApi_BaseUrl + "/courses/";
        private string UserUrl = CareGiverApi_BaseUrl + "/users/";
        //private string DxToken = "&token=3932f3b0-cfab-11dc-95ff-0800200c9a663932f3b0-cfab-11dc-95ff-0800200c9a66";
        private string DxToken = ConfigSettings.DxToken; //"&token=88851CDAAC0F418995D099D08770F2FF81042CC507CE4292B4EE3C46E31BBB3D";
        private string DxCodeUrl = "https://clinicaltables.nlm.nih.gov/api/icd10cm/v3/search?sf=code,name&terms=";
        private string SpecialistUrl = "https://clinicaltables.nlm.nih.gov/api/npi_idv/v3/search?terms=";
        private string NinjaUrl = ConfigSettings.NinjaInvoiceUrl;
        private string XNinjaToken = ConfigSettings.XNinjaToken;
        private string NinjaTokenKey = ConfigSettings.NinjaTokenKey;
        // private string NewUsersUrl = CareGiverApi_BaseUrl + "";






        //  public static string UserName = String.Empty;
        // public static string Password = String.Empty;
        public static string HttpType_Get = String.Empty;
        public static string HttpType_Post = String.Empty;
        public static string ContentType_Json = String.Empty;
        private HttpClient client = new HttpClient();
        private EmployeeDataProvider _employeeDataProvider;

        public DateTime ExpireDate { get; private set; }
        public DateTime NotificationDate { get; private set; }

        static CareGiverApi()
        {
            //  UserName = ConfigSettings.EbriggsUserName;//"default@ebriggspf.com";
            // Password = ConfigSettings.EbriggsPassword;//"eBriggsPilotFish";
            HttpType_Get = "GET";
            HttpType_Post = "POST";
            ContentType_Json = "application/json";
        }



        //Start Get Certificate Care giver function start

        public ServiceResponse GetLanguage()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            HttpClient client = new HttpClient();
            using (HttpResponseMessage response = client.GetAsync(LanguageUrl).Result)
            {
                using (HttpContent content = response.Content)
                {
                    string result = content.ReadAsStringAsync().Result;
                    if (result != null)
                    {
                        List<CareGiverApi_LanguageModel> resModel = JsonConvert.DeserializeObject<List<CareGiverApi_LanguageModel>>(result);
                        serviceResponse.IsSuccess = true;
                        serviceResponse.Data = resModel;
                    }
                }
            }


            return serviceResponse;
        }

        public ServiceResponse GetCourses()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            HttpClient client = new HttpClient();
            using (HttpResponseMessage response = client.GetAsync(CourseUrl).Result)
            {
                using (HttpContent content = response.Content)
                {
                    string result = content.ReadAsStringAsync().Result;
                    if (result != null)
                    {
                        List<CareGiverApi_CourseModel> resModel = JsonConvert.DeserializeObject<List<CareGiverApi_CourseModel>>(result);
                        serviceResponse.IsSuccess = true;
                        serviceResponse.Data = resModel;
                    }
                }
            }
            return serviceResponse;
        }

        public ServiceResponse GetUserCertificates(string email)
        {
            var mylist = new List<CareGiverApi_CourseModel>();
            var list = new List<CareGiverApi_CourseModel>();
            ServiceResponse serviceResponse = new ServiceResponse();
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-ApiKey", "4C36CBBA-0B8C-4BA2-8A4D-B7CD5BD4F056");
            UserUrl = UserUrl + "?emailAddress=" + email;
            using (HttpResponseMessage response = client.GetAsync(UserUrl).Result)
            {
                using (HttpContent content = response.Content)
                {
                    string result = content.ReadAsStringAsync().Result;
                    if (result != null)
                    {
                        List<UserModel> resModel = JsonConvert.DeserializeObject<List<UserModel>>(result);

                        if (resModel.Count > 0)
                        {

                            List<UserCourse> userCourses = resModel[0].Courses;


                            ServiceResponse sResponse = GetCourses();
                            List<CareGiverApi_CourseModel> courselList = (List<CareGiverApi_CourseModel>)sResponse.Data;

                            foreach (var userCourse in userCourses)
                            {
                                CareGiverApi_CourseModel data = courselList.Where(c => c.CourseId == userCourse.CourseId).FirstOrDefault();
                                if (data != null)
                                    userCourse.CourseName = data.Name;
                            }

                            serviceResponse.Data = userCourses;
                        }

                        serviceResponse.IsSuccess = true;

                    }
                }


            }
            return serviceResponse;
        }

        public ServiceResponse GetUser(string email)
        {
            var mylist = new List<CareGiverApi_CourseModel>();
            var list = new List<CareGiverApi_CourseModel>();
            ServiceResponse serviceResponse = new ServiceResponse();
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-ApiKey", "4C36CBBA-0B8C-4BA2-8A4D-B7CD5BD4F056");
            UserUrl = UserUrl + "?emailAddress=" + email;
            using (HttpResponseMessage response = client.GetAsync(UserUrl).Result)
            {
                using (HttpContent content = response.Content)
                {
                    string result = content.ReadAsStringAsync().Result;
                    if (result != null)
                    {
                        List<UserModel> resModel = JsonConvert.DeserializeObject<List<UserModel>>(result);

                        if (resModel.Count > 0)
                        {

                            List<UserCourse> userCourses = resModel[0].Courses;
                            ServiceResponse sResponse = GetCourses();
                            List<CareGiverApi_CourseModel> courselList = (List<CareGiverApi_CourseModel>)sResponse.Data;

                            foreach (var userCourse in userCourses)
                            {
                                CareGiverApi_CourseModel data = courselList.Where(c => c.CourseId == userCourse.CourseId).FirstOrDefault();
                                if (data != null)
                                    userCourse.CourseName = data.Name;


                            }
                            var klist = userCourses.Select(x => Convert.ToDateTime(x.ExamTime)).ToList();
                            foreach (var item in klist)
                            {
                                if (klist.Count < 15)
                                {
                                    mylist.Add(new CareGiverApi_CourseModel
                                    {
                                        ExpireDate = item.AddYears(1),
                                        NotificationDate = item.AddDays(350),

                                    });
                                    foreach (var k in mylist)
                                    {
                                        if (k.NotificationDate == DateTime.Now)
                                        {
                                            var _ExpireDate = k.ExpireDate;
                                            var _NotificationDate = k.NotificationDate;
                                            SmtpClient obj = new SmtpClient();
                                            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                                            var mail = new MailMessage();
                                            mail.From = new MailAddress("akhileshkamal2012@gmail.com");
                                            mail.To.Add("akamal@myezcare.com");
                                            mail.Subject = "Test Mail - 1";
                                            mail.IsBodyHtml = true;
                                            string htmlBody;
                                            htmlBody = " This mail just to inform you that Your CareGiver Certificate validity will be expire on: " + _ExpireDate;
                                            mail.Body = htmlBody;
                                            SmtpServer.Port = 587;
                                            SmtpServer.UseDefaultCredentials = false;
                                            SmtpServer.Credentials = new System.Net.NetworkCredential("akhileshkamal2012@gmail.com", "Akhil@2018");
                                            SmtpServer.EnableSsl = true;
                                            SmtpServer.Send(mail);

                                        }


                                    }

                                }

                            }
                            //   return klist;
                            serviceResponse.Data = mylist;
                        }
                        serviceResponse.IsSuccess = true;

                    }
                }


            }
            return serviceResponse;

        }

        //Start Get Certificate Care giver function End

        // new code for get certificate detail
        public ServiceResponse GetCertificateDetails()
        {
            // var mylist = new List<CareGiverApi_CourseModel>();
            // var list = new List<CareGiverApi_CourseModel>();
            ServiceResponse serviceResponse = new ServiceResponse();
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-ApiKey", "4C36CBBA-0B8C-4BA2-8A4D-B7CD5BD4F056");
            // UserUrl = UserUrl + "?emailAddress=" + email;          
            using (HttpResponseMessage response = client.GetAsync(UserUrl).Result)
            {
                using (HttpContent content = response.Content)
                {
                    string result = content.ReadAsStringAsync().Result;
                    if (result != null)
                    {
                        List<UserModel> resModel = JsonConvert.DeserializeObject<List<UserModel>>(result);

                        if (resModel.Count > 0)
                        {

                            List<UserCourse> userCourses = resModel[0].Courses;


                            ServiceResponse sResponse = GetCourses();
                            List<CareGiverApi_CourseModel> courselList = (List<CareGiverApi_CourseModel>)sResponse.Data;

                            foreach (var userCourse in userCourses)
                            {
                                CareGiverApi_CourseModel data = courselList.Where(c => c.CourseId == userCourse.CourseId).FirstOrDefault();
                                if (data != null)
                                    userCourse.CourseName = data.Name;


                                userCourse.ExpireDate = userCourse.ExamTime;
                                userCourse.NotificationDate = userCourse.ExamTime;



                            }

                            serviceResponse.Data = userCourses;
                        }

                        serviceResponse.IsSuccess = true;

                    }
                }


            }
            return serviceResponse;
        }

        //get Employee details
        public ServiceResponse GetEmployeesList(SearchEmployeeModel searchEmployeeModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            _employeeDataProvider = new EmployeeDataProvider();
            var response = _employeeDataProvider.GetEmployeeList(searchEmployeeModel, pageIndex, pageSize, sortIndex, sortDirection);
            return response;
        }

        /// Medication section 
        public ServiceResponse GetProductType()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("api_key", "e7612d31eef07fe1d8006871ccfe5c57");
            // UserUrl = UserUrl + "?emailAddress=" + email;
            UserUrl = ProductTypeURL;
            using (HttpResponseMessage response = client.GetAsync(UserUrl).Result)
            {
                using (HttpContent content = response.Content)
                {
                    string result = content.ReadAsStringAsync().Result;
                    if (result != null)
                    {
                        resultss resModel = JsonConvert.DeserializeObject<resultss>(result);


                        serviceResponse.IsSuccess = true;

                    }
                }


            }
            return serviceResponse;
        }

        public List<DxCode> GetDxCode(string searchText)
        {
            var DxCodeUrls = DxCodeUrl + searchText;
            var DxCodelist = new List<DxCode>();
            //  ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpClient client = new HttpClient();
                using (HttpResponseMessage response = client.GetAsync(DxCodeUrls).Result)
                {
                    using (HttpContent content = response.Content)
                    {
                        string result = content.ReadAsStringAsync().Result;
                        if (result != null)
                        {

                            List<object> arrResult = JsonConvert.DeserializeObject<List<object>>(result);
                            if (arrResult.Count > 3)
                            {
                                List<object> list = JsonConvert.DeserializeObject<List<object>>(Convert.ToString(arrResult[3]));
                                foreach (var item in list)
                                {
                                    List<string> lstItem = JsonConvert.DeserializeObject<List<string>>(Convert.ToString(item));
                                    if (lstItem.Count > 1)
                                    {
                                        DxCodelist.Add(new DxCode
                                        {
                                            DXCodeName = lstItem[0],
                                            DXCodeWithoutDot = lstItem[0].Replace(".", ""),
                                            Description = lstItem[1],
                                            DxCodeShortName = "ICD10",
                                            DxCodeType = "ABK",

                                        });
                                    }
                                }
                            }
                            //        foreach(var item in DxCodelist)
                            //        {
                            //            var searchlist = new List<SearchValueData>
                            //{
                            //    new SearchValueData {Name = "DXCodeName", Value = Convert.ToString(item.DXCodeName)},
                            //    new SearchValueData {Name = "DXCodeWithoutDot", Value = Convert.ToString(item.DXCodeWithoutDot)},
                            //    new SearchValueData {Name = "DxCodeType", Value = Convert.ToString(item.DxCodeType)},
                            //    new SearchValueData {Name = "Description", Value = Convert.ToString(item.Description)},
                            //    new SearchValueData {Name = "DxCodeShortName", Value = Convert.ToString(item.DxCodeShortName)},
                            //        };
                            //            GetScalar(StoredProcedure.SaveDxCodeAPI, searchlist);

                            //        }
                            //serviceResponse.IsSuccess = true;
                            //serviceResponse.Data = DxCodelist;
                            return DxCodelist;
                        }
                    }
                }
                return DxCodelist;
            }

            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return DxCodelist;
        }
        public ServiceResponse NinjaInvoice(long OrgID)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                var NinjaInvoiceUrl = NinjaUrl + "clients?id_number=" + OrgID;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                client.DefaultRequestHeaders.Add(XNinjaToken, NinjaTokenKey);
                using (HttpResponseMessage response = client.GetAsync(NinjaInvoiceUrl).Result)
                {
                    using (
                        HttpContent content = response.Content)
                    {
                        string result = content.ReadAsStringAsync().Result;
                        if (result != null)
                        {
                            InvoiceModel resModel = JsonConvert.DeserializeObject<InvoiceModel>(result);
                            if (resModel.data.Count > 0 && resModel.data != null)
                            {
                                data client = resModel.data.FirstOrDefault();
                                var client_id = client.id;
                                serviceResponse.IsSuccess = true;
                                serviceResponse.Message = client_id;
                            }

                            serviceResponse.Data = resModel.data;
                        }
                    }
                }
                return serviceResponse;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
        public List<InvoiceMod> NinjaInvoiceList(string client_id)
        {
            var InvoiceModlist = new List<InvoiceMod>();
            try
            {

                var NinjaInvoiceListUrl = NinjaUrl + "invoices?client_id=" + client_id + "&include=invitations&per_page=1000";
                ServiceResponse serviceResponse = new ServiceResponse();
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                client.DefaultRequestHeaders.Add(XNinjaToken, NinjaTokenKey);
                using (HttpResponseMessage response = client.GetAsync(NinjaInvoiceListUrl).Result)
                {
                    using (
                        HttpContent content = response.Content)
                    {
                        string result = content.ReadAsStringAsync().Result;
                        if (result != null)
                        {
                            InvoiceModel resModel = JsonConvert.DeserializeObject<InvoiceModel>(result);

                            if (resModel.data.Count > 0)
                            {
                                foreach (var item in resModel.data)
                                {
                                    if (item.is_deleted == Convert.ToString("false") && (item.archived_at == null || item.archived_at == "0") && (item.status_id!=Convert.ToString("4")))
                                    {
                                        InvoiceModlist.Add(new InvoiceMod
                                        {
                                            id = item.id,
                                            amounts = item.amount,
                                            balance = item.balance,
                                            client_id = item.client_id,
                                            invoice_status_id = item.status_id,
                                            updated_at = item.updated_at,
                                            invoice_number = item.number,
                                            invoice_date = item.date,
                                            DueDate = item.due_date,
                                            is_deleted = item.is_deleted,
                                            invoice_type_id = item.invoice_type_id,
                                            start_date = item.start_date,
                                            invoicepath = item.invitations.OrderByDescending(x => Convert.ToString(x.sent_date)).FirstOrDefault().link
                                        });
                                    }
                                }
                            }

                            return InvoiceModlist;
                        }
                    }
                }
                return InvoiceModlist;
            }
            catch (Exception ex)
            {

                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return InvoiceModlist;
        }
        public List<InvoiceModBilling> NinjaInvoiceListBilling(string client_id)
        {
            var InvoiceModBillinglist = new List<InvoiceModBilling>();
            try
            {

                var NinjaInvoiceListUrl = NinjaUrl + "invoices?client_id=" + client_id + "&include=invitations&per_page=1000";
                ServiceResponse serviceResponse = new ServiceResponse();
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                client.DefaultRequestHeaders.Add(XNinjaToken, NinjaTokenKey);
                using (HttpResponseMessage response = client.GetAsync(NinjaInvoiceListUrl).Result)
                {
                    using (
                        HttpContent content = response.Content)
                    {
                        string result = content.ReadAsStringAsync().Result;
                        if (result != null)
                        {
                            InvoiceModel resModel = JsonConvert.DeserializeObject<InvoiceModel>(result);

                            if (resModel.data.Count > 0)
                            {
                                foreach (var item in resModel.data)
                                {
                                    if (item.is_deleted == Convert.ToString("false") && (item.archived_at == null || item.archived_at == "0"))
                                    {
                                        InvoiceModBillinglist.Add(new InvoiceModBilling
                                        {
                                            id = item.id,
                                            amounts = item.amount,
                                            balance = item.balance,
                                            client_id = item.client_id,
                                            invoice_status_id = item.status_id,
                                            updated_at = item.updated_at,
                                            invoice_number = item.number,
                                            invoice_date = item.date,
                                            DueDate = item.due_date,
                                            is_deleted = item.is_deleted,
                                            invoice_type_id = item.invoice_type_id,
                                            start_date = item.start_date,
                                            invoicepath = item.invitations.OrderByDescending(x => Convert.ToString(x.sent_date)).FirstOrDefault().link
                                        });
                                    }
                                }
                            }

                            return InvoiceModBillinglist;
                        }
                    }
                }
                return InvoiceModBillinglist;
            }
            catch (Exception ex)
            {

                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return InvoiceModBillinglist;
        }
        public List<Specialist> GetSpecialist(string searchText)
        {
            var SpecialistUrls = SpecialistUrl + searchText;
            var Specialist = new List<Specialist>();
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpClient client = new HttpClient();
                using (HttpResponseMessage response = client.GetAsync(SpecialistUrls).Result)
                {
                    using (HttpContent content = response.Content)
                    {
                        string result = content.ReadAsStringAsync().Result;
                        if (result != null)
                        {

                            List<object> arrResult = JsonConvert.DeserializeObject<List<object>>(result);
                            if (arrResult.Count > 3)
                            {
                                List<object> list = JsonConvert.DeserializeObject<List<object>>(Convert.ToString(arrResult[3]));
                                foreach (var item in list)
                                {
                                    List<string> lstItem = JsonConvert.DeserializeObject<List<string>>(Convert.ToString(item));
                                    if (lstItem.Count > 1)
                                    {
                                        Specialist.Add(new Specialist
                                        {
                                            Name = lstItem[0],
                                            NPI = lstItem[1],
                                            Type = lstItem[2],
                                            PracticeAddress = lstItem[3],

                                        });
                                    }
                                }
                            }
                            return Specialist;
                        }
                    }
                }
                return Specialist;
            }

            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return Specialist;
        }

    }

}




#region CareGiverApiModel

public class CareGiverApi_LanguageModel
{
    public string LanguageId { get; set; }
    public string Description { get; set; }
    public string Culture { get; set; }

}


public class CareGiverApi_CourseModel
{
    public string CourseId { get; set; }
    public string Name { get; set; }
    public string CreditHours { get; set; }
    public string LanguageId { get; set; }
    public string RenewalCourseId { get; set; }
    public DateTime ExpireDate { get; internal set; }
    public DateTime NotificationDate { get; internal set; }
}

//public class CareGiverApi_UsersModel
//{

//  public List<UserModel> data { get; set; }

//}

public class UserModel
{
    public string UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string LastLogin { get; set; }
    public string CreateDate { get; set; }
    public List<UserCourse> Courses { get; set; }
    public DateTime ExpireDate { get; internal set; }
    public DateTime NotificationDate { get; internal set; }

}


public class UserCourse
{

    public string CourseName { get; set; }
    public string CourseId { get; set; }
    public string Score { get; set; }
    public string Passed { get; set; }
    public string ExamTime { get; set; }
    public string ExpiredDateTime
    {
        get
        {
            if (!string.IsNullOrEmpty(ExamTime))
            {
                return Convert.ToDateTime(ExamTime).AddYears(1).ToString("MM/dd/yyyy");

            }
            return string.Empty;
        }
    }
    public string EnrollDate { get; set; }
    public string CertificateUrl { get; set; }
    public string ExpireDate { get; set; }
    public string NotificationDate { get; set; }
}

public class ExamCourse
{

    public string ExpireDate { get; set; }
    public string NotificationDate { get; set; }
}
#endregion

#region Medication

public class MedicationModel
{
    public List<metaa> meta;
    public List<resultss> results;

}
public class metaa
{
    public string disclaimer { get; set; }
    public string terms { get; set; }
    public string license { get; set; }
    public string last_updated { get; set; }
}
public class resultss
{
    public string term { get; set; }
    public string count { get; set; }
    // public string Culture { get; set; }

}


#endregion

public class DXCodeModel
{
    //   public string ICD10 { get; set; }
    public List<ICD10> ICD10 { get; set; }

}
public class ICD10
{
    //public long DXCodeID { get; set; }
    //public string DXCodeName { get; set; }
    //public string DXCodeWithoutDot { get; set; }
    //public string DxCodeType { get; set; }
    //public string Description { get; set; }
    //public DateTime EffectiveFrom { get; set; }
    //public DateTime EffectiveTo { get; set; }
    //public bool IsDeleted { get; set; }
    //public string DxCodeShortName { get; set; }
    public string Revision { get; set; }
    public string CodeType { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public string ValidForSubmissionOnUB04 { get; set; }
    public string Note { get; set; }
    public string DottedCode { get; set; }
    public string ChapterSectionCode { get; set; }
    public string ChapterSectionName { get; set; }
    public string BlockBodySystemCode { get; set; }
    public string BlockBodySystemName { get; set; }


}

public class DxCode
{
    public string DXCodeName { get; set; }
    public string DXCodeWithoutDot { get; set; }
    public string DxCodeType { get; set; }
    public string Description { get; set; }
    public string DxCodeShortName { get; set; }
}
public class InvoiceModel
{
    public List<data> data { get; set; }
    //public List<data1> data1 { get; set; }
    public meta meta { get; set; }

}

public class data
{
    public string id { get; set; }
    public string user_id { get; set; }
    public string project_id { get; set; }
    public string assigned_user_id { get; set; }
    public string amount { get; set; }
    public string balance { get; set; }
    public string client_id { get; set; }
    public string vendor_id { get; set; }
    public string status_id { get; set; }
    public string design_id { get; set; }
    public string recurring_id { get; set; }
    public string created_at { get; set; }
    public string updated_at { get; set; }
    public string archived_at { get; set; }
    public string is_deleted { get; set; }
    public string number { get; set; }
    public string discount { get; set; }
    public string po_number { get; set; }
    public string date { get; set; }
    public string last_sent_date { get; set; }
    public string next_send_date { get; set; }
    public string due_date { get; set; }
    public string terms { get; set; }
    public string public_notes { get; set; }
    public string private_notes { get; set; }
    public string uses_inclusive_taxes { get; set; }
    public string tax_name1 { get; set; }
    public string tax_rate1 { get; set; }
    public string tax_name2 { get; set; }
    public string tax_rate2 { get; set; }
    public string tax_name3 { get; set; }
    public string tax_rate3 { get; set; }
    public string total_taxes { get; set; }
    public string is_amount_discount { get; set; }
    public string footer { get; set; }
    public string partial { get; set; }
    public string partial_due_date { get; set; }
    public string custom_value1 { get; set; }
    public string custom_value2 { get; set; }
    public string custom_value3 { get; set; }
    public string custom_value4 { get; set; }
    public string has_tasks { get; set; }
    public string has_expenses { get; set; }
    public string custom_surcharge1 { get; set; }
    public string custom_surcharge2 { get; set; }
    public string custom_surcharge3 { get; set; }
    public string custom_surcharge4 { get; set; }
    public string exchange_rate { get; set; }
    public string custom_surcharge_tax1 { get; set; }
    public string custom_surcharge_tax2 { get; set; }
    public string custom_surcharge_tax3 { get; set; }
    public string custom_surcharge_tax4 { get; set; }
    //"line_items": [],
    public string entity_type { get; set; }
    public string reminder1_sent { get; set; }
    public string reminder2_sent { get; set; }
    public string reminder3_sent { get; set; }
    public string reminder_last_sent { get; set; }
    public string paid_to_date { get; set; }
    public string subscription_id { get; set; }
    public string auto_bill_enabled { get; set; }
    public string invoice_type_id { get; set; }
    public string start_date { get; set; }
    public settings settings { get; set; }
    public tax_info tax_info { get; set; }
    public List<invoice_items> invoice_items { get; set; }
    public List<invitations> invitations { get; set; }
    public List<contacts> contacts { get; set; }
    public List<documents> documents { get; set; }
    public List<gateway_tokens> gateway_tokens { get; set; }




}


public class tax_info
{
    //public string id { get; set; }
}
public class settings
{
    public string currency_id { get; set; }
}
public class documents
{
}
public class gateway_tokens
{
}
public class line_items
{
    public string quantity { get; set; }
    public string cost { get; set; }
    public string product_key { get; set; }
    public string product_cost { get; set; }
    public string notes { get; set; }
    public string discount { get; set; }
    public string is_amount_discount { get; set; }
    public string tax_name1 { get; set; }
    public string tax_rate1 { get; set; }
    public string tax_name2 { get; set; }
    public string tax_rate2 { get; set; }
    public string tax_name3 { get; set; }
    public string tax_rate3 { get; set; }
    public string line_total { get; set; }
    public string gross_line_total { get; set; }
    public string tax_amount { get; set; }
    public string custom_value1 { get; set; }
    public string custom_value2 { get; set; }
    public string custom_value3 { get; set; }
    public string custom_value4 { get; set; }
    public string type_id { get; set; }
    public string tax_id { get; set; }

}



public class invoice_items
{
    public string account_key { get; set; }
    public string is_owner { get; set; }
    public string id { get; set; }
    public string product_key { get; set; }
    public string updated_at { get; set; }
    public string archived_at { get; set; }
    public string notes { get; set; }
    public string cost { get; set; }
    public string qty { get; set; }
    public string tax_name1 { get; set; }
    public string tax_rate1 { get; set; }
    public string tax_name2 { get; set; }
    public string tax_rate2 { get; set; }
    public string invoice_item_type_id { get; set; }
    public string custom_value1 { get; set; }
    public string custom_value2 { get; set; }
    public string discount { get; set; }

}

public class invitations
{
    public string id { get; set; }
    public string client_contact_id { get; set; }
    public string key { get; set; }
    public string link { get; set; }
    public string sent_date { get; set; }
    public string viewed_date { get; set; }
    public string opened_date { get; set; }
    public string updated_at { get; set; }
    public string archived_at { get; set; }
    public string email_error { get; set; }
    public string email_status { get; set; }
}
public class contacts
{
    public string id { get; set; }
    public string first_name { get; set; }
    public string last_name { get; set; }
    public string email { get; set; }
    public string created_at { get; set; }
    public string updated_at { get; set; }
    public string archived_at { get; set; }
    public string is_primary { get; set; }
    public string is_locked { get; set; }
    public string phone { get; set; }
    public string custom_value1 { get; set; }
    public string custom_value2 { get; set; }
    public string custom_value3 { get; set; }
    public string custom_value4 { get; set; }
    public string contact_key { get; set; }
    public string send_email { get; set; }
    public string last_login { get; set; }
    public string password { get; set; }
    public string link { get; set; }


}
public class contacts1
{
    public string account_key { get; set; }
    public string is_owner { get; set; }
    public string id { get; set; }
    public string first_name { get; set; }
    public string last_name { get; set; }
    public string email { get; set; }
    public string contact_key { get; set; }
    public string updated_at { get; set; }
    public string archived_at { get; set; }
    public string is_primary { get; set; }
    public string phone { get; set; }
    public string last_login { get; set; }
    public string send_invoice { get; set; }
    public string custom_value1 { get; set; }
    public string custom_value2 { get; set; }


}
public class meta
{
    public pagination pagination { get; set; }
}
public class pagination
{
    public string total { get; set; }
    public string count { get; set; }
    public string per_page { get; set; }
    public string current_page { get; set; }
    public string total_pages { get; set; }
    //public links links { get; set; }

}
public class links
{
    public string link { get; set; }
    public string next { get; set; }
}
public class Specialist
{
    public string Name { get; set; }
    public string FirstName
    {
        get
        {
            char[] splitchar = { ',' };
            string[] strArr = null;
            strArr = Name.Split(splitchar);
            string FirstName = strArr[0];
            return FirstName;
        }
    }
    public string LastName
    {
        get
        {
            char[] splitchar = { ',' };
            string[] strArr = null;
            strArr = Name.Split(splitchar);
            string LastName = strArr[1];
            return LastName;
        }
    }
    public string NPI { get; set; }
    public string Type { get; set; }
    public string PracticeAddress { get; set; }
    // public string FirstName { get; set; }
    // public string LastName { get; set; }

}

