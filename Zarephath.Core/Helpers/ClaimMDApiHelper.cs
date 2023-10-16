using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Helpers
{
    public class ClaimMDApiHelper
    {
        private static readonly Encoding encoding = Encoding.UTF8;
        private BatchDataProvider _BatchDataProvider;
        private CacheHelper _cacheHelper = new CacheHelper();

        public string AccountKey()
        {
            string value = _cacheHelper.ClaimMDAccountKey;
            if (string.IsNullOrEmpty(value))
            { value = System.Configuration.ConfigurationManager.AppSettings["AccountKey"].ToString(); }
            return value;
        }

        public string UserID()
        {
            string value = _cacheHelper.ClaimMDUserID;
            if (string.IsNullOrEmpty(value))
            { value = System.Configuration.ConfigurationManager.AppSettings["UserID"].ToString(); }
            return value;
        }

        public ClaimMDResponse UploadClaims(BatchValidationResponseModel claimModel)
        {
            var filepath = HttpContext.Current.Server.MapCustomPath(claimModel.Edi837FilePath);
            string requestURL = System.Configuration.ConfigurationManager.AppSettings["ClaimMD_UploadFiles_URL"].ToString();
            WebClient wc = new WebClient();
            byte[] bytes = wc.DownloadData(filepath);
            Dictionary<string, object> postParameters = new Dictionary<string, object>();
            postParameters.Add("AccountKey", AccountKey());
            postParameters.Add("UserID", UserID());
            postParameters.Add("File", new FileParameter(bytes, Path.GetFileName(filepath), "text/plain"));
            // API Post

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            HttpWebResponse webResponse = MultipartFormDataPost(requestURL, postParameters);
            StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
            string returnResponseText = responseReader.ReadToEnd();
            XDocument doc = XDocument.Parse(returnResponseText);
            string jsonText = JsonConvert.SerializeXNode(doc).Replace("@", "e_");
            ClaimMDResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<ClaimMDResponse>(jsonText);
            if (response != null && response.result != null && response.result.claim != null)
            {
                string jsonconvert = JsonConvert.SerializeObject(response.result.claim);
                var token = JToken.Parse(jsonconvert);
                if (token is JArray)
                {
                    response.result.UploadedClaims = JsonConvert.DeserializeObject<List<UC_Claims>>(jsonconvert).ToList();
                }
                else if (token is JObject)
                {
                    response.result.UploadedClaim = JsonConvert.DeserializeObject<UC_Claims>(jsonconvert);
                }
            }
            webResponse.Close();
            return response;
        }

        public ClaimMD_NotesResponse ClaimNotesRequest(string claim_md_Id)
        {
            string requestURL = System.Configuration.ConfigurationManager.AppSettings["ClaimMD_ClaimNotesReuest_URL"].ToString();
            WebClient wc = new WebClient();
            Dictionary<string, object> postParameters = new Dictionary<string, object>();
            postParameters.Add("AccountKey", AccountKey());
            postParameters.Add("UserID", UserID());
            postParameters.Add("ClaimMD_ID", claim_md_Id);
            // API Post

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            HttpWebResponse webResponse = MultipartFormDataPost(requestURL, postParameters);
            StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
            string returnResponseText = responseReader.ReadToEnd();
            XDocument doc = XDocument.Parse(returnResponseText);
            string jsonText = JsonConvert.SerializeXNode(doc).Replace("@", "e_");
            ClaimMD_NotesResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<ClaimMD_NotesResponse>(jsonText);

            if (response != null && response.result != null && response.result.notes != null)
            {
                string jsonconvert = JsonConvert.SerializeObject(response.result.notes);
                var token = JToken.Parse(jsonconvert);
                if (token is JArray)
                {
                    response.result.UC_NoteList = JsonConvert.DeserializeObject<List<UC_Note>>(jsonconvert).ToList();
                }
                else if (token is JObject)
                {
                    response.result.UC_Note = JsonConvert.DeserializeObject<UC_Note>(jsonconvert);

                    response.result.UC_NoteList = new List<UC_Note>();
                    response.result.UC_NoteList.Add(response.result.UC_Note);

                }
            }
            webResponse.Close();
            return response;
        }



        public ClaimMDResponse ClaimMessagesRequest(long responseId)
        {
            string requestURL = System.Configuration.ConfigurationManager.AppSettings["ClaimMD_ClaimMessagesReuest_URL"].ToString();
            WebClient wc = new WebClient();
            Dictionary<string, object> postParameters = new Dictionary<string, object>();
            postParameters.Add("AccountKey", AccountKey());
            postParameters.Add("UserID", UserID());

            string id = Convert.ToString(responseId);
            postParameters.Add("ResponseID", string.IsNullOrEmpty(id) ? "0" : id);
            // API Post

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            HttpWebResponse webResponse = MultipartFormDataPost(requestURL, postParameters);
            StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
            string returnResponseText = responseReader.ReadToEnd();
            XDocument doc = XDocument.Parse(returnResponseText);
            string jsonText = JsonConvert.SerializeXNode(doc).Replace("@", "e_");
            ClaimMDResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<ClaimMDResponse>(jsonText);
            if (response != null && response.result != null && response.result.claim != null)
            {
                string jsonconvert = JsonConvert.SerializeObject(response.result.claim);
                var token = JToken.Parse(jsonconvert);
                if (token is JArray)
                {
                    response.result.UploadedClaims = JsonConvert.DeserializeObject<List<UC_Claims>>(jsonconvert).ToList();

                    foreach (UC_Claims item in response.result.UploadedClaims)
                    {
                        SetMessage(item);
                    }


                }
                else if (token is JObject)
                {
                    response.result.UploadedClaim = JsonConvert.DeserializeObject<UC_Claims>(jsonconvert);
                    SetMessage(response.result.UploadedClaim);

                    response.result.UploadedClaims = new List<UC_Claims>();
                    response.result.UploadedClaims.Add(response.result.UploadedClaim);

                }
            }
            webResponse.Close();
            return response;
        }



        public UC_Claims SetMessage(UC_Claims UploadedClaim)
        {


            string jsonconvertMsg = JsonConvert.SerializeObject(UploadedClaim.messages);
            var tokenMsg = JToken.Parse(jsonconvertMsg);
            if (tokenMsg is JArray)
            {
                UploadedClaim.E_MessagesList = JsonConvert.DeserializeObject<List<UC_Message>>(jsonconvertMsg).ToList();
            }
            else if (tokenMsg is JObject)
            {
                UploadedClaim.E_Message = JsonConvert.DeserializeObject<UC_Message>(jsonconvertMsg);
                UploadedClaim.E_MessagesList = new List<UC_Message>();
                UploadedClaim.E_MessagesList.Add(UploadedClaim.E_Message);
            }


            return UploadedClaim;
        }

        public HttpWebResponse MultipartFormDataPost(string postUrl, Dictionary<string, object> postParameters)
        {
            string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());
            string contentType = "multipart/form-data; boundary=" + formDataBoundary;
            byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);
            return PostForm(postUrl, contentType, formData);
        }

        public HttpWebResponse PostForm(string postUrl, string contentType, byte[] formData)
        {
            HttpWebRequest request = WebRequest.Create(postUrl) as HttpWebRequest;
            if (request == null)
            {
                throw new NullReferenceException("request is not a http request");
            }

            request.Method = "POST";
            request.ContentType = contentType;
            request.CookieContainer = new CookieContainer();
            request.ContentLength = formData.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(formData, 0, formData.Length);
                requestStream.Close();
            }

            return request.GetResponse() as HttpWebResponse;
        }

        public HttpWebResponse MultipartFormDataGet(string getUrl, Dictionary<string, object> postParameters)
        {
            string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());
            string contentType = "multipart/form-data; boundary=" + formDataBoundary;
            byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);
            return GetForm(getUrl, contentType, formData);
        }

        public HttpWebResponse GetForm(string getUrl, string contentType, byte[] formData)
        {
            HttpWebRequest request = WebRequest.Create(getUrl) as HttpWebRequest;
            if (request == null)
            {
                throw new NullReferenceException("request is not a http request");
            }

            request.Method = "GET";
            request.ContentType = contentType;
            request.CookieContainer = new CookieContainer();
            request.ContentLength = formData.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(formData, 0, formData.Length);
                requestStream.Close();
            }

            return request.GetResponse() as HttpWebResponse;
        }

        public byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
        {
            Stream formDataStream = new System.IO.MemoryStream();
            bool needsCLRF = false;

            foreach (var param in postParameters)
            {
                if (needsCLRF)
                    formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

                needsCLRF = true;

                if (param.Value is FileParameter)
                {
                    FileParameter fileToUpload = (FileParameter)param.Value;
                    string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n",
                        boundary,
                        param.Key,
                        fileToUpload.FileName ?? param.Key,
                        fileToUpload.ContentType ?? "application/octet-stream");

                    formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));
                    formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
                }
                else
                {
                    string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
                        boundary,
                        param.Key,
                        param.Value);
                    formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));
                }
            }

            // Add the end of the request.  Start with a newline  
            string footer = "\r\n--" + boundary + "--\r\n";
            formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));

            // Dump the Stream into a byte[]  
            formDataStream.Position = 0;
            byte[] formData = new byte[formDataStream.Length];
            formDataStream.Read(formData, 0, formData.Length);
            formDataStream.Close();

            return formData;
        }

        public class FileParameter
        {
            public byte[] File { get; set; }
            public string FileName { get; set; }
            public string ContentType { get; set; }
            public FileParameter(byte[] file) : this(file, null) { }
            public FileParameter(byte[] file, string filename) : this(file, filename, null) { }
            public FileParameter(byte[] file, string filename, string contenttype)
            {
                File = file;
                FileName = filename;
                ContentType = contenttype;
            }
        }

        #region Get Latest ERA
        public ServiceResponse GetLatestERA()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            //ERAMDResponse

            _BatchDataProvider = new BatchDataProvider();
            var res = _BatchDataProvider.GetNpiDetails().Data;
            //if(res!=null)
            //{
            GetLatestERAModel NPIModel = (GetLatestERAModel)res;
            string Submitter_NM109_IdCode = NPIModel.NPIDetails.Submitter_NM109_IdCode;

            if (string.IsNullOrEmpty(NPIModel.NPIDetails.Submitter_NM109_IdCode) && string.IsNullOrEmpty(NPIModel.NPIDetails.TaxID))
            {
                serviceResponse.Message = "This service is not enabled. Please set NPI and Tax ID from organization settings page. Please contact admin for more details.";
                return serviceResponse;
            }


            string RecievedTime = "";
            if (NPIModel.RecievedTimeModel == null || NPIModel.RecievedTimeModel.RecievedTime == null)
                RecievedTime = Convert.ToDateTime(DateTime.Now.AddMonths(-2)).ToString("MM-dd-yyyy");
            else
                RecievedTime = Convert.ToDateTime(NPIModel.RecievedTimeModel.RecievedTime).ToString("MM-dd-yyyy");

            //}

            string requestURL = System.Configuration.ConfigurationManager.AppSettings["ClaimMD_GetLatestERA_URL"].ToString();
            WebClient wc = new WebClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;


            ERAMDResponse response = new ERAMDResponse();
            NameValueCollection myParams = new NameValueCollection();
            myParams.Add("AccountKey", AccountKey());
            myParams.Add("UserID", UserID());
            myParams.Add("ReceivedAfterDate", RecievedTime);
            myParams.Add("NPI", Submitter_NM109_IdCode);
            wc.QueryString.Add(myParams);


            string returnResponseText = "";
            string jsonText = "";
            if (!string.IsNullOrEmpty(NPIModel.NPIDetails.Submitter_NM109_IdCode))
            {
                returnResponseText = wc.DownloadString(requestURL);
                XDocument doc = XDocument.Parse(returnResponseText);
                jsonText = JsonConvert.SerializeXNode(doc).Replace("@", "e_");
                //ERAMDResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<ERAMDResponse>(jsonText);


                try
                {
                    response = Newtonsoft.Json.JsonConvert.DeserializeObject<ERAMDResponse>(jsonText);
                }
                catch (Exception ex)
                {
                    ERAMDResponse_Single response_1 = Newtonsoft.Json.JsonConvert.DeserializeObject<ERAMDResponse_Single>(jsonText);
                    response.result.era.Add(response_1.result.era);
                }
            }

            #region ERA Call with Tax ID
            if (!string.IsNullOrEmpty(NPIModel.NPIDetails.TaxID))
            {
                myParams.Remove("NPI");
                myParams.Add("TaxID", NPIModel.NPIDetails.TaxID);
                wc = new WebClient();
                wc.QueryString.Add(myParams);
                returnResponseText = wc.DownloadString(requestURL);
                XDocument docTax = XDocument.Parse(returnResponseText);
                jsonText = JsonConvert.SerializeXNode(docTax).Replace("@", "e_");

                ERAMDResponse response2 = new ERAMDResponse();
                try
                {
                    response2 = Newtonsoft.Json.JsonConvert.DeserializeObject<ERAMDResponse>(jsonText);
                }
                catch (Exception ex)
                {
                    ERAMDResponse_Single response2_1 = Newtonsoft.Json.JsonConvert.DeserializeObject<ERAMDResponse_Single>(jsonText);
                    response2.result.era.Add(response2_1.result.era);
                }

                if (response2 != null && response2.result != null && response2.result.era != null)
                {
                    foreach (var item in response2.result.era)
                    {
                        if (response == null || response.result == null || response.result.era == null)
                            response = new ERAMDResponse();

                        if (!response.result.era.Any(c => c.e_eraid == item.e_eraid))
                        {
                            response.result.era.Add(item);
                        }
                    }
                }

            }

            #endregion

            serviceResponse.IsSuccess = true;
            serviceResponse.Data = response;
            return serviceResponse;
        }

        public ERAPDFResponse GetLatestERAPDF(string eraId)
        {
            string requestURL = System.Configuration.ConfigurationManager.AppSettings["ClaimMD_GetERAPDF_URL"].ToString();
            using (WebClient wc = new WebClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                NameValueCollection myParams = new NameValueCollection();
                myParams.Add("AccountKey", AccountKey());
                myParams.Add("UserID", UserID());
                myParams.Add("eraid", eraId);
                wc.QueryString.Add(myParams);

                string returnResponseText = wc.DownloadString(requestURL);
                XDocument doc = XDocument.Parse(returnResponseText);
                string jsonText = JsonConvert.SerializeXNode(doc).Replace("@", "e_");
                ERAPDFResponse response = JsonConvert.DeserializeObject<ERAPDFResponse>(jsonText);
                return response;
            }
        }

        public ERAResponse GetLatestERA835(string eraId)
        {
            string requestURL = System.Configuration.ConfigurationManager.AppSettings["ClaimMD_GetERA835_URL"].ToString();
            using (WebClient wc = new WebClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                NameValueCollection myParams = new NameValueCollection();
                myParams.Add("AccountKey", AccountKey());
                myParams.Add("UserID", UserID());
                myParams.Add("eraid", eraId);
                wc.QueryString.Add(myParams);

                string returnResponseText = wc.DownloadString(requestURL);
                XDocument doc = XDocument.Parse(returnResponseText);
                string jsonText = JsonConvert.SerializeXNode(doc).Replace("@", "era_");
                ERAResponse response = JsonConvert.DeserializeObject<ERAResponse>(jsonText);
                response.result.data = response.result.data.Replace("era_", "@");
                return response;
            }
        }


        public static string RemoveInvalidXmlChars(string text)
        {
            var validChars = text.Where(ch => System.Xml.XmlConvert.IsXmlChar(ch)).ToArray();
            return new string(validChars);
        }
        public PayerListResponse GetPayerList(string payer_id = "", string payer_name = "")
        {
            string requestURL = System.Configuration.ConfigurationManager.AppSettings["ClaimMD_GetPayerList_URL"].ToString();
            using (WebClient wc = new WebClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                NameValueCollection myParams = new NameValueCollection();
                myParams.Add("AccountKey", AccountKey());

                if (!string.IsNullOrEmpty(payer_id))
                    myParams.Add("payer_id", payer_id);

                if (!string.IsNullOrEmpty(payer_name))
                    myParams.Add("payer_name", payer_name);
                wc.QueryString.Add(myParams);

                string returnResponseText = wc.DownloadString(requestURL);
                returnResponseText = RemoveInvalidXmlChars(returnResponseText);
                returnResponseText = Regex.Replace(returnResponseText, @"<payer (?<number>\d+)", "<payer _${number}");
                XDocument doc = XDocument.Parse(returnResponseText);

                string jsonText = JsonConvert.SerializeXNode(doc).Replace("@", "");//.Replace("@", "era_");
                PayerListResponse response = JsonConvert.DeserializeObject<PayerListResponse>(jsonText);
                return response;
            }
        }



        public PayerEnrollResponse PayerEnroll(string payer_id, string enroll_type, string prov_taxid, string prov_npi)
        {
            string requestURL = System.Configuration.ConfigurationManager.AppSettings["ClaimMD_PayerEnroll_URL"].ToString();
            using (WebClient wc = new WebClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                //NameValueCollection myParams = new NameValueCollection();
                //myParams.Add("AccountKey", AccountKey());
                //myParams.Add("payer_id", payer_id);
                //myParams.Add("enroll_type ", enroll_type);
                //myParams.Add("prov_taxid ", prov_taxid);
                //wc.QueryString.Add(myParams);
                //string returnResponseText = wc.DownloadString(requestURL);

                string myParameters = string.Format("AccountKey={0}&payerid={1}&enroll_type={2}&prov_taxid={3}&prov_npi={4}"
                    , AccountKey(), payer_id, enroll_type, prov_taxid, prov_npi);
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                string returnResponseText = wc.UploadString(requestURL, myParameters);


                returnResponseText = RemoveInvalidXmlChars(returnResponseText);
                XDocument doc = XDocument.Parse(returnResponseText);

                string jsonText = JsonConvert.SerializeXNode(doc).Replace("@", "");//.Replace("@", "era_");
                PayerEnrollResponse response = JsonConvert.DeserializeObject<PayerEnrollResponse>(jsonText);
                response.LogMessage = returnResponseText;
                return response;
            }
        }

        public UC_ArchieveResult ArchieveClaim(string claimId)
        {
            string requestURL = System.Configuration.ConfigurationManager.AppSettings["ClaimMD_ArchieveClaim_URL"].ToString();
            using (WebClient wc = new WebClient())
            {
                NameValueCollection myParams = new NameValueCollection();
                myParams.Add("AccountKey", AccountKey());
                myParams.Add("UserID", UserID());
                myParams.Add("claimid", claimId);
                wc.QueryString.Add(myParams);

                string returnResponseText = wc.DownloadString(requestURL);
                XDocument doc = XDocument.Parse(returnResponseText);
                string jsonText = JsonConvert.SerializeXNode(doc).Replace("@", "e_");
                UC_ArchieveResult response = JsonConvert.DeserializeObject<UC_ArchieveResult>(jsonText);
                return response;
            }
        }
        #endregion
    }
}
