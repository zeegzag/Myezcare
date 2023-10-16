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
    public class DocumentDataProvider : BaseDataProvider, IDocumentDataProvider
    {
        public DocumentDataProvider()
        {
        }

        public DocumentDataProvider(string conString)
            : base(conString)
        {
        }

        public ApiResponse GetSectionList(ApiRequest<GetSectionModel> request)
        {
            ApiResponse response;
            ConfigEBFormModel model = new ConfigEBFormModel();
            try
            {
                //var UserType = (request.Data == Common.UserType.Referral.ToString()) ? (int)Common.UserType.Referral : (int)Common.UserType.Employee;
                List<SearchValueData> srchParam = new List<SearchValueData>();
                srchParam.Add(new SearchValueData(Properties.UserType, Convert.ToString((int)Common.UserType.Referral)));

                List<SectionSubsection> sectionList = GetEntityList<SectionSubsection>(StoredProcedure.API_GetSectionList, srchParam);
                foreach (SectionSubsection item in sectionList)
                {
                    if (item.IsOrbeonForm)
                    {
                        string mode = "new";
                        string url = string.Format("{0}/fr/{1}&orbeon-embeddable=true&EmployeeID={2}&ReferralID={3}&OrganizationID={4}&DomainName={5}",
                            ConfigSettings.OrbeonBaseUrl,
                            string.Format("{0}/{1}?form-version={2}", item.NameForUrl, mode, item.Version),
                            0,
                            request.Data.ReferralID,
                            OrganizationData.OrganizationID,
                            OrganizationData.DomainName);
                        item.FormURL = string.Format("{0}/resources/forms/ezcare/embed.html?formURL={1}", ConfigSettings.OrbeonBaseUrl, HttpUtility.UrlEncode(url));
                    }
                    else if (item.IsInternalForm)
                    {
                        item.FormURL = string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Constants.LoadHtmlFormURL
                        + "?FormURL=" + HttpUtility.UrlEncode(item.InternalFormPath)
                        + "&OrgPageID=" + "Mobile"
                        + "&IsEditMode=" + "true"
                        + "&EmployeeID=" + 0
                        + "&ReferralID=" + request.Data.ReferralID
                        + "&EBriggsFormID=" + item.EBFormID
                        + "&OriginalEBFormID=" + item.EBFormID
                        + "&FormId=" + item.FormId
                        + "&SubSectionID=" + item.ComplianceID
                        + "&EbriggsFormMppingID=" + "0";
                    }
                    else if(item.NameForUrl != null && item.Version != null)
                    {
                            var newFormURL = model.EBBaseSiteUrl + "/form/" + item.NameForUrl + "?version=" + item.Version + "&PageId=Mobile";
                            item.FormURL = model.MyezcareFormsUrl + "?formURL=" + HttpUtility.UrlEncode(newFormURL);
                    }
                    else
                    {
                        item.FormURL = null;
                    }
                }

                response = Common.ApiCommonResponse(true, "", StatusCode.Ok, sectionList);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<EmpVisitHistory>(e.Message, null);
            }
            return response;
        }

        public ApiResponse SaveSectionSubSection(ApiRequest<AddDirSubDirModal> request)
        {
            ApiResponse response;
            var timeUtc = DateTime.UtcNow;
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(GetTimeZone());
            var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);
            try
            {
                var IsSection = request.Data.ParentID != 0 ? false : true;
                var UserType = request.Data.UserType == Common.UserType.Referral.ToString() ? (int)Common.UserType.Referral : (int)Common.UserType.Employee;

                var searchParameter = new List<SearchValueData>();
                searchParameter.Add(new SearchValueData(Properties.ComplianceID, Convert.ToString(request.Data.ComplianceID)));
                searchParameter.Add(new SearchValueData(Properties.Name, request.Data.Name));
                searchParameter.Add(new SearchValueData(Properties.Value, request.Data.Value));
                searchParameter.Add(new SearchValueData(Properties.Type,IsSection ? Constants.Directory: Constants.SubDirectory));
                searchParameter.Add(new SearchValueData(Properties.DocumentationType, Convert.ToString(request.Data.DocumentationType)));
                searchParameter.Add(new SearchValueData(Properties.IsTimeBased, Convert.ToString(request.Data.IsTimeBased)));
                searchParameter.Add(new SearchValueData(Properties.UserType, Convert.ToString(UserType)));
                searchParameter.Add(new SearchValueData(Properties.EBFormID, request.Data.EBFormID));
                searchParameter.Add(new SearchValueData(Properties.ParentID, Convert.ToString(request.Data.ParentID)));
                searchParameter.Add(new SearchValueData(Properties.ServerCurrentDateTime, today.ToString(Constants.DbDateTimeFormat)));
                searchParameter.Add(new SearchValueData(Properties.LoggedInID, request.Data.EmployeeID.ToString()));
                searchParameter.Add(new SearchValueData(Properties.SystemID, Common.GetHostAddress()));

                SectionModal data = GetMultipleEntity<SectionModal>(StoredProcedure.API_SaveSectionSubSection, searchParameter);

                var Message = string.Empty;
                if (request.Data.ComplianceID==0)
                {
                    Message = IsSection ? Resource.SectionAddedSuccessfully : Resource.SubSectionAddedSuccessfully;
                }
                else
                {
                    Message = IsSection ? Resource.SectionUpdatedSuccessfully : Resource.SubSectionUpdatedSuccessfully;
                }

                if (data.Result == -1)
                {
                    response = Common.ApiCommonResponse(false, Resource.ItemAlreadyExists, StatusCode.BadRequest);
                }
                else
                {
                    response = Common.ApiCommonResponse(true, Message, StatusCode.Ok, (object)data.Result);
                }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<EmpVisitHistory>(e.Message, null);
            }
            return response;
        }

        public ApiResponse DeleteSectionSubSection(ApiRequest<DeleteSecSubSecModel> request)
        {
            ApiResponse response;
            var timeUtc = DateTime.UtcNow;
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(GetTimeZone());
            var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);
            try
            {
                var searchParameter = new List<SearchValueData>();
                searchParameter.Add(new SearchValueData(Properties.ComplianceID, Convert.ToString(request.Data.ComplianceID)));
                searchParameter.Add(new SearchValueData(Properties.ServerCurrentDateTime, today.ToString(Constants.DbDateTimeFormat)));
                searchParameter.Add(new SearchValueData(Properties.LoggedInID, request.Data.EmployeeID.ToString()));
                searchParameter.Add(new SearchValueData(Properties.SystemID, Common.GetHostAddress()));

                int result = (int)GetScalar(StoredProcedure.API_DeleteSectionSubSection, searchParameter);

                
                if (result>0)
                {
                    response = Common.ApiCommonResponse(true, Resource.DeletedSuccessfully, StatusCode.Ok);
                }
                else
                {
                    response = Common.ApiCommonResponse(false, Resource.ItemAlreadyExists, StatusCode.BadRequest);
                }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<EmpVisitHistory>(e.Message, null);
            }
            return response;
        }

        public ApiResponse GetOrganizationFormList(ApiRequest request)
        {
            ApiResponse response;
            try
            {
                CacheHelper_MyezcareOrganization chMyezcareOrg = new CacheHelper_MyezcareOrganization();
                MyEzcareOrganization myOrg = chMyezcareOrg.GetCachedData<MyEzcareOrganization>();
                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "OrganizationID", Value = Convert.ToString(myOrg.OrganizationID) });
                List<FormListModel> data = GetEntityList<FormListModel>(StoredProcedure.API_GetOrganizationFormListForMapping, searchList);

                response = Common.ApiCommonResponse(true, "", StatusCode.Ok, data);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<EmpVisitHistory>(e.Message, null);
            }
            return response;
        }

        public ApiResponse GetSubSectionList(ApiRequest<GetSubSecModel> request)
        {
            ApiResponse response;
            ConfigEBFormModel model = new ConfigEBFormModel();
            try
            {
                //var UserType = (request.Data == Common.UserType.Referral.ToString()) ? (int)Common.UserType.Referral : (int)Common.UserType.Employee;
                List<SearchValueData> srchParam = new List<SearchValueData>();
                srchParam.Add(new SearchValueData(Properties.ComplianceID, Convert.ToString(request.Data.ComplianceID)));
                srchParam.Add(new SearchValueData(Properties.UserType, Convert.ToString((int)Common.UserType.Referral)));
                List<SectionSubsection> sectionList = GetEntityList<SectionSubsection>(StoredProcedure.API_GetSubSectionList, srchParam);
                foreach (SectionSubsection item in sectionList)
                {
                    if (item.IsOrbeonForm)
                    {
                        string mode = "new";
                        string url = string.Format("{0}/fr/{1}&orbeon-embeddable=true&EmployeeID={2}&ReferralID={3}&OrganizationID={4}&DomainName={5}",
                            ConfigSettings.OrbeonBaseUrl,
                            string.Format("{0}/{1}?form-version={2}", item.NameForUrl, mode, item.Version),
                            0,
                            request.Data.ReferralID,
                            OrganizationData.OrganizationID,
                            OrganizationData.DomainName);
                        item.FormURL = string.Format("{0}/resources/forms/ezcare/embed.html?formURL={1}", ConfigSettings.OrbeonBaseUrl, HttpUtility.UrlEncode(url));
                    }
                    else if (item.IsInternalForm)
                    {
                        item.FormURL = string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Constants.LoadHtmlFormURL
                        + "?FormURL=" + HttpUtility.UrlEncode(item.InternalFormPath)
                        + "&OrgPageID=" + "Mobile"
                        + "&IsEditMode=" + "true"
                        + "&EmployeeID=" + 0
                        + "&ReferralID=" + request.Data.ReferralID
                        + "&EBriggsFormID=" + item.EBFormID
                        + "&OriginalEBFormID=" + item.EBFormID
                        + "&FormId=" + item.FormId
                        + "&SubSectionID=" + item.ComplianceID
                        + "&EbriggsFormMppingID=" + "0";
                    }
                    else if (item.NameForUrl != null && item.Version != null)
                    {
                        var newFormURL = model.EBBaseSiteUrl + "/form/" + item.NameForUrl + "?version=" + item.Version + "&PageId=Mobile";
                        item.FormURL = model.MyezcareFormsUrl + "?formURL=" + HttpUtility.UrlEncode(newFormURL);
                    }
                    else
                    {
                        item.FormURL = null;
                    }
                }
                response = Common.ApiCommonResponse(true, "", StatusCode.Ok, sectionList);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<EmpVisitHistory>(e.Message, null);
            }
            return response;
        }

        public ApiResponseList<DocumentList> GetDocumentList(ApiRequest<ListModel<SearchDocumentModel>> request)
        {
            ApiResponseList<DocumentList> response;
            try
            {
                List<SearchValueData> list = new List<SearchValueData>();
                var DocumentationType = request.Data.SearchParams.DocumentationType == 1 ? Constants.Internal : 
                    request.Data.SearchParams.DocumentationType == 2 ? Constants.External : null;
                list.Add(new SearchValueData { Name = Properties.FromIndex, Value = Convert.ToString(((request.Data.PageIndex - 1) * request.Data.PageSize) + 1) });
                list.Add(new SearchValueData { Name = Properties.ToIndex, Value = Convert.ToString(request.Data.PageIndex * request.Data.PageSize) });
                list.Add(new SearchValueData { Name = Properties.SortType, Value = request.Data.SortType });
                list.Add(new SearchValueData { Name = Properties.SortExpression, Value = request.Data.SortExpression });
                list.Add(new SearchValueData { Name = Properties.ComplianceID, Value = Convert.ToString(request.Data.SearchParams.ComplianceID) });
                list.Add(new SearchValueData { Name = Properties.UserType, Value = Convert.ToString((int)UserTypes.Referral) });
                list.Add(new SearchValueData { Name = Properties.ReferralID, Value = Convert.ToString(request.Data.SearchParams.ReferralID) });
                list.Add(new SearchValueData { Name = Properties.DocumentName, Value = Convert.ToString(request.Data.SearchParams.DocumentName) });
                list.Add(new SearchValueData { Name = Properties.DocumentationType, Value = DocumentationType });
                list.Add(new SearchValueData { Name = Properties.SearchInDate, Value = request.Data.SearchParams.SearchInDate });
                if (request.Data.SearchParams.StartDate != null)
                    list.Add(new SearchValueData { Name = Properties.StartDate, Value = request.Data.SearchParams.StartDate.Value.ToString(Constants.DbDateFormat) });
                if (request.Data.SearchParams.EndDate != null)
                    list.Add(new SearchValueData { Name = Properties.EndDate, Value = request.Data.SearchParams.EndDate.Value.ToString(Constants.DbDateFormat) });

                List<DocumentList> listItem = GetEntityList<DocumentList>(StoredProcedure.API_GetDocumentList, list);
                foreach(DocumentList item in listItem)
                {
                    item.FilePath = string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Common.GetAccessPath(item.FilePath);
                }

                if (listItem != null)
                {
                    response = Common.ApiCommonResponseList(true, "", StatusCode.Ok, GetPageList(listItem, request.Data.PageIndex, request.Data.PageSize, null));
                }
                else
                {
                    var getPage = new Page<DocumentList>();
                    response = Common.ApiCommonResponseList(true, "", StatusCode.Ok, getPage);
                }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<DocumentList>(e.Message, null);
            }
            return response;
        }

        public ApiResponse GetDocumentInfo(ApiRequest<DocumentInfoRequestModal> request)
        {
            ApiResponse response;
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "ReferralDocumentID", Value = Convert.ToString(request.Data.ReferralDocumentID) });
                searchList.Add(new SearchValueData { Name = "EbriggsFormMppingID", Value = Convert.ToString(request.Data.EbriggsFormMppingID) });
                searchList.Add(new SearchValueData { Name = "ComplianceID", Value = Convert.ToString(request.Data.ComplianceID) });
                DocumentInfoModal info = GetEntity<DocumentInfoModal>(StoredProcedure.API_GetDocumentInfo, searchList);

                response = Common.ApiCommonResponse(true, "", StatusCode.Ok, info);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<EmpVisitHistory>(e.Message, null);
            }
            return response;
        }

        public ApiResponse UpdateDocument(ApiRequest<DocumentModel> request)
        {
            ApiResponse response;
            var timeUtc = DateTime.UtcNow;
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(GetTimeZone());
            var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);
            try
            {
                var DocumentationType = request.Data.DocumentationType == 1 ? Constants.Internal :
                    request.Data.DocumentationType == 2 ? Constants.External : null;
                List<SearchValueData> srchParam = new List<SearchValueData>();
                srchParam.Add(new SearchValueData(Properties.ReferralDocumentID, Convert.ToString(request.Data.ReferralDocumentID)));
                srchParam.Add(new SearchValueData(Properties.FileName, request.Data.FileName));
                srchParam.Add(new SearchValueData(Properties.DocumentationType, DocumentationType));
                if (request.Data.ExpirationDate.HasValue)
                srchParam.Add(new SearchValueData(Properties.ExpirationDate, request.Data.ExpirationDate.Value.ToString(Constants.DbDateFormat)));
                srchParam.Add(new SearchValueData(Properties.ServerCurrentDateTime, today.ToString(Constants.DbDateTimeFormat)));
                srchParam.Add(new SearchValueData(Properties.LoggedInID, request.Data.EmployeeID.ToString()));
                srchParam.Add(new SearchValueData(Properties.SystemID, Common.GetHostAddress()));


                int data = (int)GetScalar(StoredProcedure.API_UpdateDocument, srchParam);

                response = data==1?Common.ApiCommonResponse(true, Resource.DocumentUpdated, StatusCode.Ok, null) : Common.ApiCommonResponse(false, Resource.DocumentTypeExists, StatusCode.Ok, null);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<EmpVisitHistory>(e.Message, null);
            }
            return response;
        }

        public ApiResponse SaveFormPreference(ApiRequest<FormPreferenceModel> request)
        {
            ApiResponse response;
            ConfigEBFormModel model = new ConfigEBFormModel();
            var timeUtc = DateTime.UtcNow;
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(GetTimeZone());
            var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);
            try
            {
                List<SearchValueData> srchParam = new List<SearchValueData>();
                srchParam.Add(new SearchValueData(Properties.ComplianceID, Convert.ToString(request.Data.ComplianceID)));
                srchParam.Add(new SearchValueData(Properties.EBFormID, request.Data.EBFormID));
                srchParam.Add(new SearchValueData(Properties.SavePreference, Convert.ToString(request.Data.SavePreference)));
                srchParam.Add(new SearchValueData(Properties.ServerCurrentDateTime, today.ToString(Constants.DbDateTimeFormat)));
                srchParam.Add(new SearchValueData(Properties.LoggedInID, request.Data.EmployeeID.ToString()));
                srchParam.Add(new SearchValueData(Properties.SystemID, Common.GetHostAddress()));

                SectionSubsection data = GetEntity<SectionSubsection>(StoredProcedure.API_SaveFormPreference, srchParam);

                var FormURL = string.Empty;
                if (data.IsOrbeonForm)
                {
                    string mode = "new";
                    string url = string.Format("{0}/fr/{1}&orbeon-embeddable=true&EmployeeID={2}&ReferralID={3}&OrganizationID={4}&DomainName={5}",
                        ConfigSettings.OrbeonBaseUrl,
                        string.Format("{0}/{1}?form-version={2}", data.NameForUrl, mode, data.Version),
                        0,
                        request.Data.ReferralID,
                        OrganizationData.OrganizationID,
                        OrganizationData.DomainName);
                    FormURL = string.Format("{0}/resources/forms/ezcare/embed.html?formURL={1}", ConfigSettings.OrbeonBaseUrl, HttpUtility.UrlEncode(url));
                }
                else if (data.IsInternalForm)
                {
                    FormURL = string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Constants.LoadHtmlFormURL
                    + "?FormURL=" + HttpUtility.UrlEncode(data.InternalFormPath)
                    + "&OrgPageID=" + "Mobile"
                    + "&IsEditMode=" + "true"
                    + "&EmployeeID=" + 0
                    + "&ReferralID=" + request.Data.ReferralID
                    + "&EBriggsFormID=" + data.EBFormID
                    + "&OriginalEBFormID=" + data.EBFormID
                    + "&FormId=" + data.FormId
                    + "&SubSectionID=" + request.Data.ComplianceID
                    + "&EbriggsFormMppingID=" + "0";
                }
                else
                {
                    var newFormURL = model.EBBaseSiteUrl + "/form/" + data.NameForUrl + "?version=" + data.Version + "&PageId=Mobile";
                    FormURL = model.MyezcareFormsUrl + "?formURL=" + HttpUtility.UrlEncode(newFormURL);
                }

                response = Common.ApiCommonResponse(true, request.Data.SavePreference? Resource.PreferenceStoredSuccessfully:"", StatusCode.Ok, FormURL);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<EmpVisitHistory>(e.Message, null);
            }
            return response;
        }

        public ApiResponse OpenSavedForm(ApiRequest<OpenFormModel> request)
        {
            ApiResponse response;
            ConfigEBFormModel model = new ConfigEBFormModel();
            var timeUtc = DateTime.UtcNow;
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(GetTimeZone());
            var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);
            try
            {
                List<SearchValueData> srchParam = new List<SearchValueData>();
                srchParam.Add(new SearchValueData(Properties.EbriggsFormMppingID, Convert.ToString(request.Data.EbriggsFormMppingID)));
                srchParam.Add(new SearchValueData(Properties.UpdateForm, Convert.ToString(request.Data.UpdateForm)));

                SavedFormDetails data = GetEntity<SavedFormDetails>(StoredProcedure.API_OpenSavedForm, srchParam);

                var FormURL = string.Empty;
                if (data.IsInternalForm)
                {
                    FormURL = string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Constants.LoadHtmlFormURL
                    + "?FormURL=" + HttpUtility.UrlEncode(data.InternalFormPath)
                    + "&OrgPageID=" + "Mobile"
                    + "&IsEditMode=" + "true"
                    + "&ReferralID=" + data.ReferralID
                    + "&EBriggsFormID=" + data.EBFormID
                    + "&OriginalEBFormID=" + data.EBFormID
                    + "&FormId=" + data.FormId
                    + "&EbriggsFormMppingID=" + request.Data.EbriggsFormMppingID;
                }
                else
                {
                    var newFormURL = string.Empty;
                    if (request.Data.UpdateForm)
                    {
                        newFormURL = model.EBBaseSiteUrl + "/form/" + data.NameForUrl + "?version=" + data.Version + "&id=" + data.EBriggsFormID + "&PageId=Mobile";
                    }
                    else
                    {
                        newFormURL = model.EBBaseSiteUrl + "/pdf/" + data.NameForUrl + "?version=" + data.Version + "&id=" + data.EBriggsFormID + "&PageId=Mobile";
                    }
                    FormURL = model.MyezcareFormsUrl + "?formURL=" + HttpUtility.UrlEncode(newFormURL);
                }

                response = Common.ApiCommonResponse(true,null, StatusCode.Ok, FormURL);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<EmpVisitHistory>(e.Message, null);
            }
            return response;
        }

        public ApiResponse OpenSavedOrbeonForm(ApiRequest<OpenOrbeonFormModel> request)
        {
            ApiResponse response;
            ConfigEBFormModel model = new ConfigEBFormModel();
            var timeUtc = DateTime.UtcNow;
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(GetTimeZone());
            var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);
            try
            {
                List<SearchValueData> srchParam = new List<SearchValueData>();
                srchParam.Add(new SearchValueData(Properties.ReferralDocumentID, Convert.ToString(request.Data.ReferralDocumentID)));
                SavedFormDetails data = GetEntity<SavedFormDetails>(StoredProcedure.API_OpenSavedOrbeonForm, srchParam);
                if (data != null)
                {
                   if(data.StoreType== "Orbeon")
                    {
                        var FormURL = string.Empty;
                        string mode = !string.IsNullOrEmpty(data.OrbeonFormID) ? "edit/" + data.OrbeonFormID : "new";
                        string url = string.Format("{0}/fr/{1}&orbeon-embeddable=true&EmployeeID={2}&ReferralID={3}&OrganizationID={4}&DomainName={5}",
                            ConfigSettings.OrbeonBaseUrl,
                            string.Format("{0}/{1}?form-version={2}", data.NameForUrl, mode, data.Version),
                            0,
                            request.Data.ReferralID,
                            OrganizationData.OrganizationID,
                            OrganizationData.DomainName);
                        FormURL = string.Format("{0}/resources/forms/ezcare/embed.html?formURL={1}", ConfigSettings.OrbeonBaseUrl, HttpUtility.UrlEncode(url));

                        response = Common.ApiCommonResponse(true, null, StatusCode.Ok, FormURL);
                    }
                    else
                    {
                        var FormURL = string.Empty;
                        string mode = !string.IsNullOrEmpty(data.OrbeonFormID) ? "edit/" + data.OrbeonFormID : "new";
                         FormURL = string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Common.GetAccessPath(data.NameForUrl);
                       

                        response = Common.ApiCommonResponse(true, null, StatusCode.Ok, FormURL);
                    }
                }
                else
                {
                    response = Common.ApiCommonResponse(false, Resource.RecordNotFound, StatusCode.NotFound);
                }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<EmpVisitHistory>(e.Message, null);
            }
            return response;
        }

        public ApiResponse UploadDocument(HttpRequest currentHttpRequest, ApiRequest<PostDocumentModel> request)
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
                    //string basePath = ConfigSettings.UploadBasePath;
                    //List<FileModel> filelist = new List<FileModel>();
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
                        client.Headers.Remove(Properties.ReferralID);
                        client.Headers.Remove(Properties.ComplianceID);
                        client.Headers.Remove(Properties.KindOfDocument);
                        client.Headers.Remove(Properties.FileName);
                        client.Headers.Remove(Properties.FileType);
                        client.Headers.Remove(Properties.UserType);

                        var userType = request.Data.ReferralID > 0 ? (int)UserTypes.Referral : (int)UserTypes.Employee;
                        client.Headers.Add(Properties.UserType, Convert.ToString(userType));
                        client.Headers.Add(Properties.EmployeeID, Convert.ToString(request.Data.EmployeeID));
                        client.Headers.Add(Properties.ReferralID, Convert.ToString(request.Data.ReferralID));
                        client.Headers.Add(Properties.ComplianceID, Convert.ToString(request.Data.ComplianceID));
                        client.Headers.Add(Properties.KindOfDocument, Convert.ToString(request.Data.KindOfDocument));
                        client.Headers.Add(Properties.FileName, AllKey[i]);
                        client.Headers.Add(Properties.FileType, Convert.ToString(request.Data.FileType));
                        client.UploadData(string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Constants.UploadDocumentViaAPIURL, Constants.Post, fileData);
                    }
                    response = Common.ApiCommonResponse(true, Resource.DocumentUploadSuccessfully, StatusCode.Ok);

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

        public ApiResponse SaveForm(ApiRequest<SaveFormModel> request)
        {
            ApiResponse response;
            var timeUtc = DateTime.UtcNow;
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(GetTimeZone());
            var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);
            try
            {
                List<SearchValueData> srchParam = new List<SearchValueData>();
                srchParam.Add(new SearchValueData(Properties.ReferralID, Convert.ToString(request.Data.ReferralID)));
                srchParam.Add(new SearchValueData(Properties.ComplianceID, Convert.ToString(request.Data.ComplianceID)));
                srchParam.Add(new SearchValueData(Properties.EBriggsFormID, request.Data.EBriggsFormID));
                srchParam.Add(new SearchValueData(Properties.OriginalEBFormID, request.Data.OriginalEBFormID));
                srchParam.Add(new SearchValueData(Properties.FormId, request.Data.FormId));
                srchParam.Add(new SearchValueData(Properties.ServerCurrentDateTime, today.ToString(Constants.DbDateTimeFormat)));
                srchParam.Add(new SearchValueData(Properties.LoggedInID, request.Data.EmployeeID.ToString()));
                srchParam.Add(new SearchValueData(Properties.SystemID, Common.GetHostAddress()));

                SaveFormResult result = GetEntity<SaveFormResult>(StoredProcedure.API_SaveForm, srchParam);

                response = Common.ApiCommonResponse(true, Resource.FormSavedSuccessfully, StatusCode.Ok, result);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse SaveOrbeonForm(ApiRequest<SaveOrbeonFormModel> request)
        {
            ApiResponse response;
            var timeUtc = DateTime.UtcNow;
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(GetTimeZone());
            var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);
            try
            {
                var searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData(Properties.DocumentID, request.Data.OrbeonFormID));
                List<SearchOrbeonFormSearch> searchResult = Orbeon_GetEntityList<SearchOrbeonFormSearch>(StoredProcedure.GetEZOrbeonData_ByFormID, searchList);
                if (searchResult != null && searchResult.Count() > 0)
                {
                    var orbeonData = searchResult.FirstOrDefault();
                    var employeeId = orbeonData.EmployeeID > 0 ? orbeonData.EmployeeID : request.Data.EmployeeID;
                    var referralId = orbeonData.ReferralID > 0 ? orbeonData.ReferralID : request.Data.ReferralID;
                    var userId = request.Data.IsEmployeeDocument ? employeeId : referralId;
                    var complianceId = request.Data.ComplianceID;
                    int userType = request.Data.IsEmployeeDocument ? (int)UserTypes.Employee : (int)UserTypes.Referral;
                    Compliance compliance = GetEntity<Compliance>(Convert.ToInt64(complianceId));
                    string kindOfDocument = compliance.DocumentationType == 1 ? DocumentKind.Internal.ToString() : DocumentKind.External.ToString();
                    string storeType = "Orbeon";

                    List<SearchValueData> srchParam = new List<SearchValueData>();
                    srchParam.Add(new SearchValueData(Properties.ReferralDocumentID, Convert.ToString(request.Data.ReferralDocumentID)));
                    srchParam.Add(new SearchValueData(Properties.FileName, orbeonData.FormName));
                    srchParam.Add(new SearchValueData(Properties.FilePath, string.Format("/{0}/{1}", orbeonData.FormApp, orbeonData.FormName)));
                    srchParam.Add(new SearchValueData(Properties.ReferralID, Convert.ToString(userId)));
                    srchParam.Add(new SearchValueData(Properties.ComplianceID, Convert.ToString(complianceId)));
                    srchParam.Add(new SearchValueData(Properties.UserType, Convert.ToString(userType)));
                    srchParam.Add(new SearchValueData(Properties.KindOfDocument, kindOfDocument));
                    srchParam.Add(new SearchValueData(Properties.LoggedInID, request.Data.EmployeeID.ToString()));
                    srchParam.Add(new SearchValueData(Properties.SystemID, Common.GetHostAddress()));
                    srchParam.Add(new SearchValueData(Properties.StoreType, storeType));
                    srchParam.Add(new SearchValueData(Properties.GoogleFileId, request.Data.OrbeonFormID));
                    srchParam.Add(new SearchValueData(Properties.GoogleDetails, ""));

                    ReferralDocument result = GetEntity<ReferralDocument>(StoredProcedure.API_SaveOrbeonForm, srchParam);
                    response = Common.ApiCommonResponse(true, Resource.FormSavedSuccessfully, StatusCode.Ok, result);
                }
                else
                {
                    response = Common.ApiCommonResponse(false, Resource.SomethingWentWrong, StatusCode.NotFound);
                }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse SaveFormName(ApiRequest<SaveFormNameRequestModal> request)
        {
            ApiResponse response;
            try
            {
                List<SearchValueData> srchParam = new List<SearchValueData>();
                srchParam.Add(new SearchValueData(Properties.EbriggsFormMppingID, Convert.ToString(request.Data.EbriggsFormMppingID)));
                srchParam.Add(new SearchValueData(Properties.FormName, request.Data.FormName));

                GetScalar(StoredProcedure.API_SaveFormName, srchParam);

                response = Common.ApiCommonResponse(true, null, StatusCode.Ok, null);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError(e.Message);
            }
            return response;
        }

        public ApiResponse DeleteDocument(ApiRequest<DeleteDocumentModel> request)
        {
            ApiResponse response;
            var timeUtc = DateTime.UtcNow;
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(GetTimeZone());
            var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);
            try
            {
                if (request.Data.ReferralDocumentID > 0)
                {
                    HttpClient client = new HttpClient();
                    var values = new NameValueCollection();
                    var myContent = JsonConvert.SerializeObject(request.Data);
                    var stringContent = new StringContent(myContent, UnicodeEncoding.UTF8, "application/json");
                    var response1 = client.PostAsync(string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Constants.DeleteReferralDocumentViaAPIURL, stringContent);
                }

                List<SearchValueData> srchParam = new List<SearchValueData>();
                srchParam.Add(new SearchValueData(Properties.ReferralDocumentID, Convert.ToString(request.Data.ReferralDocumentID)));
                srchParam.Add(new SearchValueData(Properties.EbriggsFormMppingID, Convert.ToString(request.Data.EbriggsFormMppingID)));
                srchParam.Add(new SearchValueData(Properties.ServerCurrentDateTime, today.ToString(Constants.DbDateTimeFormat)));
                srchParam.Add(new SearchValueData(Properties.LoggedInID, request.Data.EmployeeID.ToString()));
                srchParam.Add(new SearchValueData(Properties.SystemID, Common.GetHostAddress()));


                int data = (int)GetScalar(StoredProcedure.API_DeleteDocument, srchParam);

                response = data == 1 ? Common.ApiCommonResponse(true, Resource.DocumentDeleted, StatusCode.Ok, null) : Common.ApiCommonResponse(false, Resource.SomethingWentWrong, StatusCode.Ok, null);
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<EmpVisitHistory>(e.Message, null);
            }
            return response;
        }

        public ApiResponse UploadImage(HttpRequest currentHttpRequest, ApiRequest<PostDocumentModel> request)
        {
            ApiResponse response = new ApiResponse();
            WebClient client = new WebClient();
            int uploadCount = 0;
            //string p = string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Constants.UploadEmpSignatureURL;

            try
            {
                string base64String = string.Empty;
                string fullPath = string.Empty;
                HttpFileCollection Files = HttpContext.Current.Request.Files;
                List<SearchValueData> data = new List<SearchValueData>();
                if (currentHttpRequest.Files.Count > 0)
                {
                    //string basePath = ConfigSettings.UploadBasePath;
                    //List<FileModel> filelist = new List<FileModel>();
                    var AllKey = currentHttpRequest.Files.AllKeys;

                    for (int i = 0; i < currentHttpRequest.Files.Count; i++)
                    {
                        HttpPostedFile file = currentHttpRequest.Files[i];
                        if(file.ContentLength>0)
                        {
                            string modifiedFileName = "";
                            if (!File.Exists(fullPath + Path.GetFileName(modifiedFileName)))
                            {
                                file.SaveAs(fullPath + Path.GetFileName(modifiedFileName));
                                uploadCount++;
                               
                            }
                        }

                        byte[] fileData = null;
                        using (var binaryReader = new BinaryReader(file.InputStream))
                        {
                            fileData = binaryReader.ReadBytes(file.ContentLength);
                        }

                        //Remove Header to avoid duplication
                        client.Headers.Remove(Properties.EmployeeID);
                        client.Headers.Remove(Properties.ReferralID);
                        client.Headers.Remove(Properties.ComplianceID);
                        client.Headers.Remove(Properties.KindOfDocument);
                        client.Headers.Remove(Properties.FileName);
                        client.Headers.Remove(Properties.FileType);


                        client.Headers.Add(Properties.EmployeeID, Convert.ToString(request.Data.EmployeeID));
                        client.Headers.Add(Properties.ReferralID, Convert.ToString(request.Data.ReferralID));
                        client.Headers.Add(Properties.ComplianceID, Convert.ToString(request.Data.ComplianceID));
                        client.Headers.Add(Properties.KindOfDocument, Convert.ToString(request.Data.KindOfDocument));
                        client.Headers.Add(Properties.FileName, AllKey[i]);
                        client.Headers.Add(Properties.FileType, Convert.ToString(request.Data.FileType));
                        client.UploadData(string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + Constants.UploadDocumentViaAPIURL, Constants.Post, fileData);
                    }
                    response = Common.ApiCommonResponse(true, Resource.DocumentUploadSuccessfully, StatusCode.Ok);

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


        #region Organization Settings
        public ApiResponse GetOrganizationSettingDetail(ApiRequest<OrganizationDataRequest> request)
        {
            ApiResponse response;
            OrganizationData formData = new OrganizationData();
            try
            {
                List<SearchValueData> srchParam = new List<SearchValueData>();
                srchParam.Add(new SearchValueData(Properties.OrganizationID, Convert.ToString(OrganizationData.OrganizationID)));

                formData = GetMultipleEntity<OrganizationData>(StoredProcedure.GetOrganizationData, srchParam);
                if (formData?.OrganizationSettings.OrganizationID != null)
                {
                    response = Common.ApiCommonResponse(true, Resource.DataFetchedSuccessfully, StatusCode.Ok, formData);
                }
                else
                { response = Common.ApiCommonResponse(false, Resource.RecordNotFound, StatusCode.BadRequest, formData); }
            }
            catch (Exception e)
            {
                response = Common.InternalServerError<EmpVisitHistory>(e.Message, null);
            }
            return response;
        }
        #endregion
    }
}