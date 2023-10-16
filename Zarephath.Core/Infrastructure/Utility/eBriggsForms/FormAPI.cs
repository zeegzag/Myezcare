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

namespace Zarephath.Core.Infrastructure.Utility.eBriggsForms
{
    public class FormApi
    {
        private static string FormApiUrl = ConfigSettings.EbriggsUrl;//  "https://209.151.166.188:8443/rest";
        private string LoginUrl = FormApiUrl + "/auth/login/";
        private string LogoutUrl = FormApiUrl + "/auth/logout/";
        private string GetMarketListUrl = FormApiUrl + "/formmarket/markets";
        private string GetFormListUrl = FormApiUrl + "/formlist/data";
        private string GetHtmlFormUrl = FormApiUrl + "/form";
        


        public static string UserName = String.Empty;
        public static string Password = String.Empty;
        public static string HttpType_Get = String.Empty;
        public static string HttpType_Post = String.Empty;
        public static string ContentType_Json = String.Empty;
        private HttpClient client = new HttpClient();

        static FormApi()
        {
            UserName = ConfigSettings.EbriggsUserName;//"default@ebriggspf.com";
            Password = ConfigSettings.EbriggsPassword;//"eBriggsPilotFish";
            HttpType_Get = "GET";
            HttpType_Post = "POST";
            ContentType_Json = "application/json";
        }




        public ServiceResponse Authenticate(string userName = "", string password = "")
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            userName = !String.IsNullOrEmpty(userName) ? userName : UserName;
            password = !String.IsNullOrEmpty(password) ? password : Password;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            string parameters = "{\"username\":\"" + userName + "\",\"password\":\"" + password + "\"}";

            using (HttpResponseMessage response = client.PostAsync(LoginUrl, new StringContent(parameters)).Result)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    serviceResponse.ErrorCode =Constants.ErrorCode_AccessDenied;
                    return serviceResponse;
                }

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    serviceResponse.ErrorCode = Constants.ErrorCode_InternalError;
                    return serviceResponse;
                }

                using (HttpContent content = response.Content)
                {
                    string result = content.ReadAsStringAsync().Result;
                    if (result != null)
                    {
                        FormApi_LoginResponseModel resModel = JsonConvert.DeserializeObject<FormApi_LoginResponseModel>(result);
                        serviceResponse.IsSuccess = true;
                        serviceResponse.Data = resModel;
                    }
                }

            }

            return serviceResponse;
        }



        public ServiceResponse GetMarkets(string tenantGuid)
        {
            ServiceResponse serviceResponse = new ServiceResponse();


            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            GetMarketListUrl = String.Format("{0}?tenantGuid={1}", GetMarketListUrl, tenantGuid);

            using (HttpResponseMessage response = client.GetAsync(GetMarketListUrl).Result)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    serviceResponse.ErrorCode = Constants.ErrorCode_AccessDenied;
                    return serviceResponse;
                }

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    serviceResponse.ErrorCode = Constants.ErrorCode_InternalError;
                    return serviceResponse;
                }



                using (HttpContent content = response.Content)
                {
                    string result = content.ReadAsStringAsync().Result;
                    if (result != null)
                    {
                        List<FormApi_MarketModel> resModel =
                            JsonConvert.DeserializeObject<List<FormApi_MarketModel>>(result);
                        serviceResponse.IsSuccess = true;
                        serviceResponse.Data = resModel;
                    }
                }
            }
            return serviceResponse;
        }


        public ServiceResponse GetFormList(string tenantGuid, string market = "", bool latest = true)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            if (string.IsNullOrEmpty(market))
            GetFormListUrl = String.Format("{0}?tenantGuid={1}&latest={2}", GetFormListUrl, tenantGuid, latest);
            else
                GetFormListUrl = String.Format("{0}?tenantGuid={1}&market{2}&latest={3}", GetFormListUrl, tenantGuid, market, latest);


            using (HttpResponseMessage response = client.GetAsync(GetFormListUrl).Result)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    serviceResponse.ErrorCode = Constants.ErrorCode_AccessDenied;
                    return serviceResponse;
                }

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    serviceResponse.ErrorCode = Constants.ErrorCode_InternalError;
                    return serviceResponse;
                }


                using (HttpContent content = response.Content)
                {
                    string result = content.ReadAsStringAsync().Result;
                    if (result != null)
                    {
                        List<FormApi_FormModel> resModel = JsonConvert.DeserializeObject<List<FormApi_FormModel>>(result);
                        serviceResponse.IsSuccess = true;
                        serviceResponse.Data = resModel;
                    }
                }
            }

            return serviceResponse;
        }



        public ServiceResponse GetHtmlForm(string tenantGuid, FormApi_HtmlFormModel model)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            if (string.IsNullOrEmpty(model.Id))
                GetHtmlFormUrl = String.Format("{0}/{1}?version={2}&tenantGuid={3}", GetHtmlFormUrl, model.FormShortName,model.Version,tenantGuid);
            else
                GetHtmlFormUrl = String.Format("{0}/{1}?version={2}&id{3}&tenantGuid={4}", GetHtmlFormUrl, model.FormShortName, model.Version,model.Id, tenantGuid);


            using (HttpResponseMessage response = client.GetAsync(GetFormListUrl).Result)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    serviceResponse.ErrorCode = Constants.ErrorCode_AccessDenied;
                    return serviceResponse;
                }

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    serviceResponse.ErrorCode = Constants.ErrorCode_InternalError;
                    return serviceResponse;
                }


                using (HttpContent content = response.Content)
                {
                    string result = content.ReadAsStringAsync().Result;
                    if (result != null)
                    {
                        serviceResponse.IsSuccess = true;
                        serviceResponse.Data = result;
                    }
                }
            }

            return serviceResponse;
        }



    }
    
    #region FormApiModel
    public class FormApi_HtmlFormModel
    {
        public string FormShortName { get; set; }
        public string Version { get; set; }
        public string Id { get; set; }
    }

    public class FormApi_LoginResponseModel
    {
        public string tenantGuid { get; set; }
    }


    public class FormApi_MarketModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }


    public class FormApi_FormModel
    {
        public FormApi_FormModel()
        {
            FormPackages = new List<FormApi_FormPackages>();
            FormCategory = new FormApi_FormCategory();
            UniqueFormID = new FormApi_UniqueID();
        }

        [JsonProperty("_id")]
        public FormApi_UniqueID UniqueFormID { get; set; }
        public List<FormApi_FormPackages> FormPackages { get; set; }
        public FormApi_FormCategory FormCategory { get; set; }


        public string FromUniqueID { get; set; }
        public string FormId { get; set; }
        public string Id { get; set; }

        public string Name { get; set; }
        public string NameForUrl { get { return Name.Replace('/', '_'); } }
        public string Version { get; set; }
        public string FormLongName { get; set; }
        public bool IsActive { get; set; }
        public bool HasHtml { get; set; }
        public string NewHtmlURI { get; set; }
        public bool HasPDF { get; set; }
        public string NewPdfURI { get; set; }
        public string EbCategoryID { get; set; }
        public string EbMarketIDs { get; set; }
    }

    public class FormApi_FormPackages
    {
        public string PackageId { get; set; }
        public string PackageName { get; set; }

    }

    public class FormApi_FormCategory
    {
        public string Id { get; set; }
        public string Name { get; set; }

    }


    public class FormApi_UniqueID
    {
        [JsonProperty("$oid")]
        public string Id { get; set; }

    }


    #endregion



 
}
