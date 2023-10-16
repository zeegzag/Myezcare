using HomeCareApi.Infrastructure.IDataProvider;
using HomeCareApi.Models.ApiModel;
using HomeCareApi.Models.Entity;
using HomeCareApi.Models.General;
using HomeCareApi.Models.ViewModel;
using HomeCareApi.Resources;
using Newtonsoft.Json;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;

namespace HomeCareApi.Infrastructure.DataProvider
{
    public class EmployeeDataProvider : BaseDataProvider, IEmployeeDataProvider
    {
        public EmployeeDataProvider()
        {
        }

        public EmployeeDataProvider(string conString)
            : base(conString)
        {
        }

        public ApiResponse GetIVRPin(ApiRequest<string> request)
        {
            ApiResponse response = new ApiResponse { IsSuccess = false };


            try
            {
                // Get user from the mobile number
                LoginDetails loginDetails = GetMultipleEntity<LoginDetails>(StoredProcedure.GetEmployeeDetail, new List<SearchValueData>
                {
                    new SearchValueData(Properties.UserName, request.Data),
                });

                Employee employee = loginDetails.Employee;

                if (employee != null)
                {
                    response = Common.ApiCommonResponse<string>(false, Resource.DataFetchedSuccessfully, StatusCode.Ok, employee.IVRPin);
                }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<EmpVisitHistory>(e.Message, null);
            }


            return response;
        }

        public ApiResponse SaveIVRPin(ApiRequest<EmployeeIVRModel> request)
        {
            ApiResponse response;
            //var timeUtc = DateTime.UtcNow;
            //var timeZone = TimeZoneInfo.FindSystemTimeZoneById(GetTimeZone());
            //var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);

            try
            {
                var searchParameter = new List<SearchValueData>();
                searchParameter.Add(new SearchValueData(Properties.UserName, request.Data.Username));
                searchParameter.Add(new SearchValueData(Properties.IVRPin, request.Data.IVRPin));
                searchParameter.Add(new SearchValueData(Properties.LoggedInID, request.Data.EmployeeID.ToString()));
                searchParameter.Add(new SearchValueData(Properties.SystemID, Common.GetHostAddress()));

                var result = GetScalar(StoredProcedure.API_Update_EmployeeIVR, searchParameter);

                var Message = string.Empty;


                if ((int)result != 1)
                {
                    response = Common.ApiCommonResponse(false, Resource.BadRequestMessage, StatusCode.BadRequest);
                }
                else
                {
                    response = Common.ApiCommonResponse(true, Message, StatusCode.Ok, (object)request);
                }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<EmployeeIVRModel>(e.Message, null);
            }
            return response;
        }

        public ApiResponse SaveProfileImage(HttpRequest currentHttpRequest, ApiRequest<EmployeeProfileImageModel> request)
        {
            ApiResponse response = new ApiResponse();
            WebClient client = new WebClient();
            //string p = string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Constants.UploadEmpSignatureURL;


            try
            {
                string base64String = string.Empty;
                string fullPath = string.Empty;
                List<SearchValueData> data = new List<SearchValueData>();
                if (currentHttpRequest.Files.Count > 0)
                {
                    var AllKey = currentHttpRequest.Files.AllKeys;

                    for (int i = 0; i < currentHttpRequest.Files.Count; i++)
                    {
                        HttpPostedFile file = currentHttpRequest.Files[i];

                        byte[] fileData = null;
                        using (var binaryReader = new BinaryReader(file.InputStream))
                        {
                            fileData = binaryReader.ReadBytes(file.ContentLength);
                        }

                        //Remove Header to avoid duplication
                        client.Headers.Remove(Properties.EmployeeID);
                        client.Headers.Remove(Properties.EmployeeSignatureID);
                        client.Headers.Remove(Properties.ImageKey);

                        client.Headers.Add(Properties.EmployeeID, Convert.ToString(request.Data.EmployeeID));
                        client.Headers.Add(Properties.ImageKey, "ProfilePic");

                        string url = string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Constants.UploadEmpSignatureURL;

                        client.UploadData(url, Constants.Post, fileData);
                    }


                    response = Common.ApiCommonResponse(true, Resource.DetailSaveSuccessfully, StatusCode.Ok);
                }
                else
                {
                    response = Common.ApiCommonResponse(false, Resource.BadRequest, StatusCode.NotFound);
                }
            }
            catch (Exception e)
            {
                response = Common.ApiCommonResponse(false, Common.SetExceptionMessage(Resource.ServerError + e.Message),
                    StatusCode.InternalServerError + " //--// " + e.InnerException + " //--// " + e.StackTrace);
            }
            return response;
        }

        public ApiResponse SaveSignature(HttpRequest currentHttpRequest, ApiRequest<EmployeeSignatureModel> request)
        {
            ApiResponse response = new ApiResponse();
            WebClient client = new WebClient();

            try
            {
                string base64String = string.Empty;
                string fullPath = string.Empty;
                List<SearchValueData> data = new List<SearchValueData>();
                if (currentHttpRequest.Files.Count > 0)
                {
                    var AllKey = currentHttpRequest.Files.AllKeys;

                    for (int i = 0; i < currentHttpRequest.Files.Count; i++)
                    {
                        HttpPostedFile file = currentHttpRequest.Files[i];

                        byte[] fileData = null;
                        using (var binaryReader = new BinaryReader(file.InputStream))
                        {
                            fileData = binaryReader.ReadBytes(file.ContentLength);
                        }

                        //Remove Header to avoid duplication
                        client.Headers.Remove(Properties.EmployeeID);
                        client.Headers.Remove(Properties.EmployeeSignatureID);
                        client.Headers.Remove(Properties.ImageKey);

                        client.Headers.Add(Properties.EmployeeID, Convert.ToString(request.Data.EmployeeID));
                        client.Headers.Add(Properties.EmployeeSignatureID, Convert.ToString(request.Data.EmployeeSignatureID));
                        client.Headers.Add(Properties.ImageKey, "Siganture");

                        client.UploadData(string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Constants.UploadEmpSignatureURL, Constants.Post, fileData);
                    }


                    response = Common.ApiCommonResponse(true, Resource.DetailSaveSuccessfully, StatusCode.Ok);
                }
                else
                {
                    response = Common.ApiCommonResponse(false, Resource.BadRequest, StatusCode.NotFound);
                }
            }
            catch (Exception e)
            {
                response = Common.ApiCommonResponse(false, Common.SetExceptionMessage(Resource.ServerError + e.Message),
                    StatusCode.InternalServerError + " //--// " + e.InnerException + " //--// " + e.StackTrace);
            }
            return response;
        }

        public ApiResponse AcceptAgreement(HttpRequest currentHttpRequest, ApiRequest<EmployeeAgreementModel> request)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                // set agreement accept flag
                var searchParameter = new List<SearchValueData>();
                searchParameter.Add(new SearchValueData(Properties.EmployeeID, Convert.ToString(request.Data.EmployeeID)));
                searchParameter.Add(new SearchValueData(Properties.Latitude, Convert.ToString(request.Data.Latitude)));
                searchParameter.Add(new SearchValueData(Properties.Longitude, Convert.ToString(request.Data.Longitude)));
                searchParameter.Add(new SearchValueData(Properties.SystemID, Common.GetHostAddress()));

                var result = GetScalar(StoredProcedure.API_AcceptAgreement, searchParameter);

                var agreementText = (string)result;


                if (string.IsNullOrWhiteSpace(agreementText)) //TermsConditionMobile
                {
                    response = Common.ApiCommonResponse(false, Resource.BadRequestMessage, StatusCode.BadRequest);
                }
                //else
                //{
                //    response = Common.ApiCommonResponse(true, Message, StatusCode.Ok, (object)request);
                //}


                /// TODO: Create pdf with agreement + signature and upload it in employee documents

                WebClient client = new WebClient();

                List<SearchValueData> data = new List<SearchValueData>();
                if (currentHttpRequest.Files.Count > 0)
                {
                    var AllKey = currentHttpRequest.Files.AllKeys;

                    for (int i = 0; i < currentHttpRequest.Files.Count; i++)
                    {
                        HttpPostedFile file = currentHttpRequest.Files[i];

                        //agreementText = "<div>Terms and This is some long agreement for mobile and after that signature should add up. <p>some other stuff</p></div>" + Environment.NewLine + Environment.NewLine + Environment.NewLine;
                        //>> var agreementImage = Helpers.ImageHelper.CreateBitmapImage(agreementText);


                        byte[] fileData = new byte[file.ContentLength];
                        file.InputStream.Read(fileData, 0, file.ContentLength);

                        // append signature age with agreement
                        string pdfPath = new Helpers.PdfHelper().GereratePdf(new List<string> { agreementText }, fileData);

                        //Remove Header to avoid duplication
                        client.Headers.Remove(Properties.EmployeeID);
                        client.Headers.Remove(Properties.EmployeeSignatureID);
                        client.Headers.Remove(Properties.ImageKey);
                        client.Headers.Remove(Properties.KindOfDocument);
                        client.Headers.Remove(Properties.FileName);
                        client.Headers.Remove(Properties.ComplianceID);
                        client.Headers.Remove(Properties.FileType);

                        client.Headers.Add(Properties.EmployeeID, Convert.ToString(request.Data.EmployeeID));
                        client.Headers.Add(Properties.ReferralID, Convert.ToString(request.Data.EmployeeID));
                        client.Headers.Add(Properties.EmployeeSignatureID, Convert.ToString(request.Data.EmployeeSignatureID));
                        client.Headers.Add(Properties.KindOfDocument, "Internal");
                        client.Headers.Add(Properties.FileName, "Mobile Agreement.pdf");
                        client.Headers.Add(Properties.ComplianceID, "-6");
                        client.Headers.Add(Properties.UserType, "2");
                        client.Headers.Add(Properties.FileType, ".pdf");

                        //client.Headers.Add(Properties.ImageKey, "Siganture");


                        //>>var allImages = new System.Drawing.Bitmap[] { agreementImage, Helpers.ImageHelper.ConvertToBitmap(fileData) };
                        //>>var allData = Helpers.ImageHelper.ConvertToByte(Helpers.ImageHelper.CombineBitmap(allImages), System.Drawing.Imaging.ImageFormat.Bmp);

                        // read pdf as bytes
                        var allData = File.ReadAllBytes(pdfPath);

                        // delete pdf file
                        if (File.Exists(pdfPath)) File.Delete(pdfPath);

                        // send pdf bytes to main app
                        client.UploadData(string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Constants.UploadDocumentViaAPIURL, Constants.Post, allData);
                    }


                    response = Common.ApiCommonResponse(true, Resource.DetailSaveSuccessfully, StatusCode.Ok);
                }
                else
                {
                    response = Common.ApiCommonResponse(false, Resource.BadRequest, StatusCode.NotFound);
                }
            }
            catch (Exception e)
            {
                response = Common.ApiCommonResponse(false, Common.SetExceptionMessage(Resource.ServerError + e.Message),
                    StatusCode.InternalServerError + " //--// " + e.InnerException + " //--// " + e.StackTrace);
            }
            return response;
        }
    }
}