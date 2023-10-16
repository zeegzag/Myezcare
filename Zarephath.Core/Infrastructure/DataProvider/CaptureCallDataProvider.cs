using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using PetaPoco;
using Zarephath.Core.Helpers;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class CaptureCallDataProvider : BaseDataProvider, ICaptureCallDataProvider
    {
        public ServiceResponse AddCaptureCall(long capturecallID)
        {
            var response = new ServiceResponse();
            var isEditMode = capturecallID > 0;

            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData>();
                searchParam.Add(new SearchValueData { Name = "CapturecallID", Value = Convert.ToString(capturecallID) });
                //searchParam.Add(new SearchValueData { Name = "Type", Value = "GetById" });
                var model = GetMultipleEntity<CaptureCallModel>(StoredProcedure.CaptureCallDetails, searchParam);

                if (model.CaptureCall == null)
                    model.CaptureCall = new CaptureCall();

                response.IsSuccess = true;
                response.Data = model;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, e.Message);
            }
            return response;
        }

        public ServiceResponse AddCaptureCall(CaptureCalls capturecall,string EmployeesIDs, string RoleIds,string RelatedWithPatient, long loggedInUserId)
        {
            var response = new ServiceResponse();
            string storeType = "Orbeon";
            List<SearchOrbeonFormSearch> result;
            try
            {
                ICaptureCallDataProvider _captureCallDataProvider = new CaptureCallDataProvider();
                if (capturecall.OrbeonID != null)
                {
                    capturecall.OrbeonID = capturecall.OrbeonID.Trim();
                    string conStr = ConfigurationManager.ConnectionStrings[Constants.OrbeonConnectionString].ConnectionString;
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        con.Open();
                        var dt = new DataTable();

                        string query = $"exec {StoredProcedure.GetEZOrbeonData_ByFormID} '{capturecall.OrbeonID}';";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            using (SqlDataAdapter da = new SqlDataAdapter())
                            {
                                da.SelectCommand = cmd;
                                da.Fill(dt);
                            }
                            result = Common.DataTableToList<SearchOrbeonFormSearch>(dt);
                        }
                    }
                    if (result != null && result.Count() > 0)
                    {
                        response.IsSuccess = true;
                        var orbeonData = result.FirstOrDefault();
                        var actualFilepath = orbeonData.FormName;
                        var dataList = new List<SearchValueData>();
                        dataList.Add(new SearchValueData { Name = "Id", Value = Convert.ToString(capturecall.Id) });
                        dataList.Add(new SearchValueData { Name = "FirstName", Value = capturecall.FirstName });
                        dataList.Add(new SearchValueData { Name = "LastName", Value = capturecall.LastName });
                        dataList.Add(new SearchValueData { Name = "Contact", Value = capturecall.Contact });
                        dataList.Add(new SearchValueData { Name = "Email", Value = capturecall.Email });
                        dataList.Add(new SearchValueData { Name = "Address", Value = capturecall.Address });
                        dataList.Add(new SearchValueData { Name = "City", Value = capturecall.City });
                        dataList.Add(new SearchValueData { Name = "StateCode", Value = capturecall.StateCode });
                        dataList.Add(new SearchValueData { Name = "ZipCode", Value = capturecall.ZipCode });
                        dataList.Add(new SearchValueData { Name = "Notes", Value = capturecall.Notes });
                        dataList.Add(new SearchValueData { Name = "RoleIds", Value = Convert.ToString(RoleIds) });
                        dataList.Add(new SearchValueData { Name = "EmployeesIDs", Value = Convert.ToString(EmployeesIDs) });
                        dataList.Add(new SearchValueData { Name = "CallType", Value = Convert.ToString(capturecall.CallType) });
                        dataList.Add(new SearchValueData { Name = "RelatedWithPatient", Value = Convert.ToString(RelatedWithPatient) });
                        dataList.Add(new SearchValueData { Name = "InquiryDate", Value = Convert.ToDateTime(capturecall.InquiryDate).ToString(Constants.DbDateFormat) });//Convert.ToString(capturecall.InquiryDate) });
                        dataList.Add(new SearchValueData { Name = "Status", Value = Convert.ToString(capturecall.Status) });
                        dataList.Add(new SearchValueData { Name = "loggedInUserId", Value = Convert.ToString(loggedInUserId) });
                        dataList.Add(new SearchValueData { Name = "GroupIDs", Value = capturecall.GroupIDs });
                        if (capturecall.OrbeonID != null)
                        {
                            dataList.Add(new SearchValueData { Name = "FileName", Value = orbeonData.FormName });
                            dataList.Add(new SearchValueData { Name = "FilePath", Value = string.Format("/{0}/{1}", orbeonData.FormApp, orbeonData.FormName) });
                            dataList.Add(new SearchValueData { Name = "GoogleFileId", Value = capturecall.OrbeonID });
                        }
                        int data = (int)GetScalar(StoredProcedure.SaveCaptureCall, dataList);
                        //response.Data = data;
                        //response.Message = "Form is saved.";
                        if (data == -1)
                        {
                            response.Data = data;
                            response.IsSuccess = false;
                            response.Message = "Details already exist";
                        }
                        else
                        {
                            response.Data = data;
                            response.Message = "Form is saved.";
                            response.IsSuccess = true;
                        }

                    }
                    else
                    {
                        //Not updated.
                        response.IsSuccess = false;
                        response.Message = Resource.ErrorOccured;
                    }
                }
                else
                {
                    var dataList = new List<SearchValueData>();
                    dataList.Add(new SearchValueData { Name = "Id", Value = Convert.ToString(capturecall.Id) });
                    dataList.Add(new SearchValueData { Name = "FirstName", Value = capturecall.FirstName });
                    dataList.Add(new SearchValueData { Name = "LastName", Value = capturecall.LastName });
                    dataList.Add(new SearchValueData { Name = "Contact", Value = capturecall.Contact });
                    dataList.Add(new SearchValueData { Name = "Email", Value = capturecall.Email });
                    dataList.Add(new SearchValueData { Name = "Address", Value = capturecall.Address });
                    dataList.Add(new SearchValueData { Name = "City", Value = capturecall.City });
                    dataList.Add(new SearchValueData { Name = "StateCode", Value = capturecall.StateCode });
                    dataList.Add(new SearchValueData { Name = "ZipCode", Value = capturecall.ZipCode });
                    dataList.Add(new SearchValueData { Name = "Notes", Value = capturecall.Notes });
                    dataList.Add(new SearchValueData { Name = "RoleIds", Value = Convert.ToString(RoleIds) });
                    dataList.Add(new SearchValueData { Name = "EmployeesIDs", Value = Convert.ToString(EmployeesIDs) });
                    dataList.Add(new SearchValueData { Name = "CallType", Value = Convert.ToString(capturecall.CallType) });
                    dataList.Add(new SearchValueData { Name = "RelatedWithPatient", Value = Convert.ToString(RelatedWithPatient) });
                    dataList.Add(new SearchValueData { Name = "InquiryDate", Value = Convert.ToDateTime(capturecall.InquiryDate).ToString(Constants.DbDateFormat) });//Convert.ToString(capturecall.InquiryDate) });
                    dataList.Add(new SearchValueData { Name = "Status", Value = Convert.ToString(capturecall.Status) });
                    dataList.Add(new SearchValueData { Name = "loggedInUserId", Value = Convert.ToString(loggedInUserId) });
                    dataList.Add(new SearchValueData { Name = "GroupIDs", Value = capturecall.GroupIDs });

                    int data = (int)GetScalar(StoredProcedure.SaveCaptureCall, dataList);
                    if (data == -1)
                    {
                        response.Data = data;
                        response.IsSuccess = false;
                        response.Message = "Details already exist";
                    }
                    else
                    {
                        response.Data = data;
                        response.Message = "Form is saved.";
                        response.IsSuccess = true;
                    }


                }



            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse SetCaptureCallListPage()
        {
            ServiceResponse response = new ServiceResponse();
            SetCaptureCallListPage model = new SetCaptureCallListPage();

            model.DeleteFilter = Common.SetDeleteFilter();
            model.SearchCaptureCallListPage = new SearchCaptureCallListPage() { IsDeleted = 0 };
            response.Data = model;
            response.IsSuccess = true;
            return response;
            }

        public ServiceResponse GetCaptureCallList(SearchCaptureCallListPage searchCaptureCallListPage, int pageIndex, int pageSize,
            string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchCaptureCallListPage != null)
                SetSearchFilterForCaptureCallList(searchCaptureCallListPage, searchList);

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));
            //searchList.Add(new SearchValueData { Name = "Type", Value = "List" });

            List<ListCaptureCallModel> totalData = GetEntityList<ListCaptureCallModel>(StoredProcedure.GetCaptureCallList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListCaptureCallModel> list = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = list;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse DeleteCapture(SearchCaptureCallListPage searchCaptureCallListPage, int pageIndex, int pageSize,
            string sortIndex, string sortDirection, long loggedInUserId)
        {
            var response = new ServiceResponse();

            List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

            SetSearchFilterForCaptureCallList(searchCaptureCallListPage, searchList);

            if (searchCaptureCallListPage != null)

                searchList.Add(new SearchValueData { Name = "Id", Value = Convert.ToString(searchCaptureCallListPage.ListOfIdsInCsv) });

            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });

            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserId) });

            List<ListCaptureCallModel> totalData = GetEntityList<ListCaptureCallModel>(StoredProcedure.DeleteCaptureCall, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListCaptureCallModel> list = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = list;
            response.IsSuccess = true;
            response.Message = "Record deleted successfully";
            return response;
        }

        private static void SetSearchFilterForCaptureCallList(SearchCaptureCallListPage searchCaptureCallListPage, List<SearchValueData> searchList)
        {
            if (!string.IsNullOrEmpty(searchCaptureCallListPage.Name))
                searchList.Add(new SearchValueData { Name = "Name", Value = Convert.ToString(searchCaptureCallListPage.Name) });
            if (!string.IsNullOrEmpty(searchCaptureCallListPage.Email))
                searchList.Add(new SearchValueData { Name = "Email", Value = Convert.ToString(searchCaptureCallListPage.Email) });
            if (!string.IsNullOrEmpty(searchCaptureCallListPage.Address))
                searchList.Add(new SearchValueData { Name = "Address", Value = Convert.ToString(searchCaptureCallListPage.Address) });
            if (!string.IsNullOrEmpty(searchCaptureCallListPage.Status))
                searchList.Add(new SearchValueData { Name = "Status", Value = Convert.ToString(searchCaptureCallListPage.Status) });
            if (!string.IsNullOrEmpty(searchCaptureCallListPage.Contact))
                searchList.Add(new SearchValueData { Name = "Contact", Value = Convert.ToString(searchCaptureCallListPage.Contact) });
            if (!string.IsNullOrEmpty(searchCaptureCallListPage.Notes))
                searchList.Add(new SearchValueData { Name = "Notes", Value = Convert.ToString(searchCaptureCallListPage.Notes) });
            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchCaptureCallListPage.IsDeleted) });
        }

        public Page<T> GetPageInStoredProcResultSet<T>(int pageIndex, int pageSize, int count, List<T> itemsList) where T : class, new()
        {
            var result = new Page<T>
            {
                CurrentPage = pageIndex,
                ItemsPerPage = pageSize,
                TotalItems = count
            };
            result.TotalPages = result.TotalItems / pageSize;

            if ((result.TotalItems % pageSize) != 0)
                result.TotalPages++;

            result.Items = itemsList;
            return result;
        }

        public ServiceResponse ConvertToReferral(ConvertToReferralModel capturecall, long LoggedInID)
        {
            var response = new ServiceResponse();
            try
            {
                ICaptureCallDataProvider _captureCallDataProvider = new CaptureCallDataProvider();

                var dataList = new List<SearchValueData>();
                dataList.Add(new SearchValueData { Name = "Id", Value = Convert.ToString(capturecall.Id) });
                dataList.Add(new SearchValueData { Name = "FirstName", Value = capturecall.FirstName });
                dataList.Add(new SearchValueData { Name = "LastName", Value = capturecall.LastName });

                dataList.Add(new SearchValueData { Name = "Phone", Value = Convert.ToString(capturecall.Contact) });
                dataList.Add(new SearchValueData { Name = "Email", Value = capturecall.Email });
                dataList.Add(new SearchValueData { Name = "Address", Value = capturecall.Address });
                dataList.Add(new SearchValueData { Name = "City", Value = capturecall.City });
                dataList.Add(new SearchValueData { Name = "StateCode", Value = capturecall.StateCode });
                dataList.Add(new SearchValueData { Name = "ZipCode", Value = capturecall.ZipCode });
                dataList.Add(new SearchValueData { Name = "Status", Value = capturecall.Status });
                dataList.Add(new SearchValueData { Name = "loggedInUserId", Value = Convert.ToString(SessionHelper.LoggedInID) });
                dataList.Add(new SearchValueData { Name = "GroupIDs", Value = capturecall.GroupIDs });

                // long data = (long)GetScalar(StoredProcedure.ConvertToReferral, dataList);
                ConvertToReferralModel data = GetEntity<ConvertToReferralModel>(StoredProcedure.ConvertToReferral, dataList);
                if (data.ReferralID == -1)
                {
                    response.IsSuccess = false;
                    response.Message = "Referral already converted";
                }
                else
                {
                    response.Data = data.EncryptedReferralID;
                    LinkDocModel linkDocModel = new LinkDocModel()
                    {
                        DocumentID = capturecall.OrbeonID,
                        //ReferralDocumentID = data.ReferralDocumentID,
                        ReferralID = data.ReferralID,
                        ComplianceID = -4
                    };
                    _captureCallDataProvider.HC_OrbeonFormMapping(linkDocModel, false);
                    response.IsSuccess = true;
                    response.Message = "Referral converted successfully";
                }



            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }
        public ServiceResponse HC_OrbeonFormMapping(LinkDocModel model, bool isEmployeeDocument)
        {
            ServiceResponse response = new ServiceResponse();
            string storeType = "Orbeon";
            List<SearchOrbeonFormSearch> result;

            try
            {
                model.DocumentID = model.DocumentID.Trim();

                string conStr = ConfigurationManager.ConnectionStrings[Constants.OrbeonConnectionString].ConnectionString;
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    var dt = new DataTable();

                    ///////////////////////////////////////////////
                    string query = $"exec {StoredProcedure.GetEZOrbeonData_ByFormID} '{model.DocumentID}';";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        //cmd.Parameters.AddWithValue("@DocumentID", model.DocumentID.Trim());

                        using (SqlDataAdapter da = new SqlDataAdapter())
                        {
                            da.SelectCommand = cmd;
                            da.Fill(dt);
                        }
                        result = Common.DataTableToList<SearchOrbeonFormSearch>(dt);
                    }
                }
                if (result != null && result.Count() > 0)
                {
                    response.IsSuccess = true;
                    ///////////////////////////////////////////////////////////////

                    var orbeonData = result.FirstOrDefault();
                    var employeeId = orbeonData.EmployeeID > 0 ? orbeonData.EmployeeID : model.EmployeeID;
                    var referralId = orbeonData.ReferralID > 0 ? orbeonData.ReferralID : model.ReferralID;
                    var userId = isEmployeeDocument ? employeeId : referralId;

                    // var ReferralId = isEmployeeDocument ? orbeonData.EmployeeID : orbeonData.ReferralID;
                    var ComplianceId = model.ComplianceID;
                    //   string basePath = String.Format(isEmployeeDocument ? _cacheHelper.EmployeeDocumentFullPath : _cacheHelper.ReferralDocumentFullPath, _cacheHelper.Domain) + userId + "/";


                    int UserType = isEmployeeDocument ? (int)ReferralDocument.UserTypes.Employee : (int)ReferralDocument.UserTypes.Referral;
                    Compliance compliance = GetEntity<Compliance>(Convert.ToInt64(ComplianceId));
                    string KindOfDocument = compliance.DocumentationType == 1 ? DocumentType.DocumentKind.Internal.ToString() : DocumentType.DocumentKind.External.ToString();

                    var actualFilepath = orbeonData.FormName;

                    var searchList = new List<SearchValueData>()
                    {
                        new SearchValueData {Name = "ReferralDocumentID ", Value = Convert.ToString(model.ReferralDocumentID) },
                        new SearchValueData {Name = "FileName", Value = orbeonData.FormName},
                        new SearchValueData {Name = "FilePath", Value =  string.Format("/{0}/{1}", orbeonData.FormApp, orbeonData.FormName)},
                        new SearchValueData {Name = "ReferralID ", Value = Convert.ToString(userId) },
                        new SearchValueData {Name = "ComplianceID ", Value = Convert.ToString(model.ComplianceID) },
                        new SearchValueData {Name = "UserType", Value = UserType.ToString() },
                        new SearchValueData {Name = "KindOfDocument ", Value = KindOfDocument },
                        //new SearchValueData {Name = "DocumentTypeID ", Value = DocumentTypeID.ToString() },
                        new SearchValueData {Name = "LoggedInUserID", Value = SessionHelper.LoggedInID.ToString()},
                        new SearchValueData {Name = "SystemID ", Value = HttpContext.Current.Request.UserHostAddress },
                        new SearchValueData {Name = "StoreType ", Value = storeType },
                        new SearchValueData {Name = "GoogleFileId ", Value = model.DocumentID },
                        new SearchValueData {Name = "GoogleDetails ", Value = "" },
                    };

                    var data = GetEntity<ReferralDocument>(StoredProcedure.HC_SaveReferralDocument, searchList);
                    response.Data = data;
                    response.Message = "Form is saved.";


                    ///////////////////////////////////////////////////////////////
                }
                else
                {
                    //Not updated.
                    response.IsSuccess = false;
                    response.Message = Resource.ErrorOccured;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
            }

            return response;
        }

    }
}