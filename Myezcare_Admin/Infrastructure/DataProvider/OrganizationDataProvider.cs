using Myezcare_Admin.Helpers;
using Myezcare_Admin.Models;
using Myezcare_Admin.Models.Entity;
using Myezcare_Admin.Models.ViewModel;
using Myezcare_Admin.Resources;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
//using static Myezcare_Admin.Infrastructure.Common;

namespace Myezcare_Admin.Infrastructure.DataProvider
{
    public class OrganizationDataProvider : BaseDataProvider, IOrganizationDataProvider
    {
        public ServiceResponse SetAddOrganizationPage(long organizationId)
        {
            var response = new ServiceResponse();
            try
            {
                SetAddOrganizationModel setAddOrganizationModel = new SetAddOrganizationModel();
                List<SearchValueData> searchList = new List<SearchValueData>
                    {
                        new SearchValueData { Name = "OrganizationID", Value = Convert.ToString(organizationId),IsEqual=true }
                    };
                MyEzcareOrganization organization = GetEntity<MyEzcareOrganization>(searchList);
                if (organization == null)
                    organization = new MyEzcareOrganization();


                setAddOrganizationModel.MyEzcareOrganization = organization;
                setAddOrganizationModel.OrganizationTypeList.Add(new NameValueDataInString { Name = AgencyType.HomeCare.ToString(), Value = AgencyType.HomeCare.ToString() });
                setAddOrganizationModel.OrganizationTypeList.Add(new NameValueDataInString { Name = AgencyType.DayCare.ToString(), Value = AgencyType.DayCare.ToString() });

                response.Data = setAddOrganizationModel;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse AddOrganization(MyEzcareOrganization organization, long loggedInUserId)
        {
            var response = new ServiceResponse();
            bool editMode = organization.OrganizationID > 0;

            List<SearchValueData> searchList = new List<SearchValueData>
                    {
                        new SearchValueData { Name = "OrganizationID", Value = Convert.ToString(organization.OrganizationID)},
                        new SearchValueData { Name = "OrganizationType", Value = organization.OrganizationType},
                        new SearchValueData { Name = "DisplayName", Value = organization.DisplayName},
                        new SearchValueData { Name = "CompanyName", Value = organization.CompanyName},
                        new SearchValueData { Name = "DomainName", Value = organization.DomainName},
                        new SearchValueData { Name = "StartDate", Value = Convert.ToString(organization.StartDate)},
                        new SearchValueData { Name = "DBName", Value = organization.DBName},
                        new SearchValueData { Name = "DBPassword", Value = organization.DBPassword},
                        new SearchValueData { Name = "DBProviderName", Value = organization.DBProviderName},
                        new SearchValueData { Name = "DBServer", Value = organization.DBServer},
                        new SearchValueData { Name = "DBUserName", Value = organization.DBUserName}
                    };

            if (organization.EndDate != null)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToString(organization.EndDate) });

            TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.SaveOrganizationData, searchList);
            if (result.TransactionResultId == -1)
            {
                response.IsSuccess = false;
                response.Message = Resource.OrganizationDuplicateErrorMessage;
                return response;
            }
            response.Message = editMode ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.Organization) : string.Format(Resource.RecordCreatedSuccessfully, Resource.Organization);

            ServiceResponse res = new ServiceResponse();
            res = Common.UpdateCache(siteName: organization.DomainName, catchType: Common.CatchType_Organization);

            response.IsSuccess = true;
            return response;

        }

        public ServiceResponse SetOrganizationListPage()
        {
            var response = new ServiceResponse();
            SetOrganizationListModel setOrganizationListModel = GetMultipleEntity<SetOrganizationListModel>(StoredProcedure.SetOrganizationListModel);
            //SetOrganizationListModel setOrganizationListModel = new SetOrganizationListModel();
            setOrganizationListModel.SearchOrganizationModel = new SearchOrganizationModel { IsDeleted = 0 };
            setOrganizationListModel.ActiveFilter = Common.SetDeleteFilter();
            response.Data = setOrganizationListModel;
            return response;
        }
        public ServiceResponse GetAllOrganizationList()
        {
            var response = new ServiceResponse();
            DataSet _dt = GetDataSet(StoredProcedure.GetAllOrganizationList);
            var data = (from DataRow dr in _dt.Tables[0].Rows
                        select new OrganizationListModel()
                        {
                            OrganizationID = dr["OrganizationID"] != null ? Convert.ToInt64(dr["OrganizationID"].ToString()) : 0,
                            DisplayName = dr["DisplayName"] != null ? dr["DisplayName"].ToString() : "",
                            CompanyName = dr["CompanyName"] != null ? dr["CompanyName"].ToString() : ""
                        }).ToList();
            response.Data = data;
            return response;
        }

        public ServiceResponse GetOrganizationList(SearchOrganizationModel searchOrganizationModel, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();
            SetSearchFilterForOrganizationListPage(searchOrganizationModel, searchList);
            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<OrganizationListModel> totalData = GetEntityList<OrganizationListModel>(StoredProcedure.GetOrganizationList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<OrganizationListModel> getOrganizationList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.IsSuccess = true;
            response.Data = getOrganizationList;
            return response;
        }

        private static void SetSearchFilterForOrganizationListPage(SearchOrganizationModel searchOrganizationModel, List<SearchValueData> searchList)
        {
            //searchList.Add(new SearchValueData { Name = "OrganizationType", Value = searchOrganizationModel.OrganizationType });
            searchList.Add(new SearchValueData { Name = "OrganizationTypeID", Value = Convert.ToString(searchOrganizationModel.OrganizationTypeID) });
            searchList.Add(new SearchValueData { Name = "OrganizationStatusID", Value = Convert.ToString(searchOrganizationModel.OrganizationStatusID) });
            searchList.Add(new SearchValueData { Name = "DisplayName", Value = searchOrganizationModel.DisplayName });
            searchList.Add(new SearchValueData { Name = "CompanyName", Value = searchOrganizationModel.CompanyName });
            searchList.Add(new SearchValueData { Name = "DomainName", Value = searchOrganizationModel.DomainName });
            if (searchOrganizationModel.StartDate != null)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToString(searchOrganizationModel.StartDate) });
            if (searchOrganizationModel.EndDate != null)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToString(searchOrganizationModel.EndDate) });
            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchOrganizationModel.IsDeleted) });
        }

        public ServiceResponse SaveOrganization(AddOrganizationModel model, long loggedInUserId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>
                    {
                        new SearchValueData { Name = "OrganizationTypeID", Value = Convert.ToString(model.OrganizationTypeID)},
                        new SearchValueData { Name = "OrganizationStatusID", Value = Convert.ToString((int)EnumOrganizationStatus.Not_Started_Yet_New)},
                        new SearchValueData { Name = "DisplayName", Value = model.DisplayName},
                        new SearchValueData { Name = "CompanyName", Value = model.CompanyName},
                        new SearchValueData { Name = "DomainName", Value = model.DomainName},
                        new SearchValueData { Name = "Email", Value = model.Email},
                        new SearchValueData { Name = "Mobile", Value = model.Mobile},
                        new SearchValueData { Name = "WorkPhone", Value = model.WorkPhone},
                        new SearchValueData { Name = "DefaultEsignTerms", Value = model.DefaultEsignTerms},
                        new SearchValueData { Name = "LoggedInID", Value = Convert.ToString(loggedInUserId)},
                        new SearchValueData { Name = "SystemID", Value = Common.GetMAcAddress()}

                    };

            TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.SaveOrganization, searchList);
            if (result.TransactionResultId == -1)
            {
                response.IsSuccess = false;
                response.Message = Resource.OrganizationDuplicateErrorMessage;
                return response;
            }
            response.Message = Resource.OrganizationSavedSuccessfully;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse SaveFile(HttpRequestBase httpRequestBase, long loggedInUserID)
        {
            var response = new ServiceResponse();
            string basePath = ConfigSettings.DataImportUploadPath;

            HttpPostedFileBase file = httpRequestBase.Files[0];
            response = Common.SaveFile(file, basePath);
            response.Message = response.IsSuccess ? Resource.FileUploaded : Resource.FileUploadFailedNoFileSelected;
            return response;
        }

        public ServiceResponse ImportDataInDatabase(ImportDataTypeModel model, long loggedInUserID)
        {
            var response = new ServiceResponse();
            try
            {
                var path = HttpContext.Current.Server.MapPath(model.FilePath);

                long organizationId = !string.IsNullOrEmpty(model.EncryptedOrganizationID) ? Convert.ToInt64(Crypto.Decrypt(model.EncryptedOrganizationID)) : 0;
                if (organizationId != 0)
                {
                    List<SearchValueData> searchParam = new List<SearchValueData>();
                    searchParam.Add(new SearchValueData { Name = "OrganizationID", Value = Convert.ToString(organizationId), IsEqual = true });
                    MyEzcareOrganization org = GetEntity<MyEzcareOrganization>(StoredProcedure.GetOrganizationDetailsById, searchParam);
                    if (org != null)
                    {
                        DataImportDataProvider dataProvider = new DataImportDataProvider(org);

                        List<SearchValueData> schParam = new List<SearchValueData>();
                        schParam.Add(new SearchValueData("UserID", Convert.ToString(loggedInUserID)));
                        schParam.Add(new SearchValueData("SystemID", Common.GetHostAddress()));

                        if (model.ImportDataType == Common.ImportTypeEnum.Patient.ToString())
                        {
                            response = dataProvider.ReadAndValidateMultipleSheets<PatientImportModel>(path,
                            new string[] { Constants.AdminTempPatientTable, Constants.AdminTempPatientContactTable },
                            new string[] { Constants.AdminTempPatientColumns, Constants.AdminTempPatientContactColumns },
                            StoredProcedure.ValidateAndInsertAdminPatient,
                            SessionHelper.LoggedInID, schParam);

                            PatientImportModel importModel = (PatientImportModel)response.Data;
                            if (importModel.TransactionResult.TransactionResultId == 0)
                                response.IsSuccess = false;
                            else
                                response.IsSuccess = true;
                        }
                        else if (model.ImportDataType == Common.ImportTypeEnum.Employee.ToString())
                        {
                            response = dataProvider.ReadAndValidateMultipleSheets<EmployeeImportModel>(path,
                            new string[] { Constants.AdminTempEmployeeTable },
                            new string[] { Constants.AdminTempEmployeeColumns },
                            StoredProcedure.ValidateAndInsertAdminEmployee,
                            SessionHelper.LoggedInID, schParam);

                            EmployeeImportModel importModel = (EmployeeImportModel)response.Data;
                            if (importModel.TransactionResult.TransactionResultId == 0)
                                response.IsSuccess = false;
                            else
                                response.IsSuccess = true;
                        }

                        File.Delete(path);
                        response.Message = response.IsSuccess ? string.Format("{0} {1}", Resource.Patient, Resource.DataImported) : Resource.ErrorOccured;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = Resource.CantConstructConnectionString;
                    }
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = Resource.CantConstructConnectionString;
                }
            }
            catch (Exception ex)
            {
                SetResponse(ex, response);
            }

            return response;
        }

        public ServiceResponse SetOrganizationEsignPage(long organizationId, long organizationEsignId)
        {
            var response = new ServiceResponse();
            try
            {
                SetOrganizationEsignModel setModel = new SetOrganizationEsignModel();
                List<SearchValueData> searchList = new List<SearchValueData>
                    {
                        new SearchValueData { Name = "OrganizationID", Value = Convert.ToString(organizationId), IsEqual = true },
                        new SearchValueData { Name = "OrganizationEsignID", Value = Convert.ToString(organizationEsignId), IsEqual = true }
                    };

                OrganizationEsignModel esignDetails = GetMultipleEntity<OrganizationEsignModel>(StoredProcedure.GetOrganizationEsignDetails, searchList);

                setModel.OrganizationDetails = esignDetails.OrganizationDetails;
                setModel.ServicePlans = esignDetails.ServicePlans;
                setModel.ServicePlanComponents = esignDetails.ServicePlanComponents;
                setModel.OrganizationTypeList = esignDetails.OrganizationTypeList;
                setModel.ServiceTypeList = esignDetails.ServiceTypeList;
                setModel.TransactionResult = esignDetails.TransactionResult;

                response.Data = setModel;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                SetResponse(ex, response);
            }
            return response;
        }

        public ServiceResponse OrganizationEsign(OrganizationEsignModel organizationEsign, long loggedInUserId)
        {
            string systemId = System.Web.HttpContext.Current.Request.UserHostAddress;
            var response = new ServiceResponse();
            try
            {
                string selectedServicePlanIds = string.Join(",", organizationEsign.ServicePlans.Select(m => m.ServicePlanID).ToArray());

                List<SearchValueData> searchList = new List<SearchValueData>
                    {
                        new SearchValueData { Name = "OrganizationEsignID", Value = Convert.ToString(organizationEsign.OrganizationDetails.OrganizationEsignID)},
                        new SearchValueData { Name = "OrganizationID", Value = Convert.ToString(organizationEsign.OrganizationDetails.OrganizationID)},
                        new SearchValueData { Name = "CompanyName", Value = organizationEsign.OrganizationDetails.CompanyName},
                        new SearchValueData { Name = "DisplayName", Value = organizationEsign.OrganizationDetails.DisplayName},
                        new SearchValueData { Name = "Email", Value = organizationEsign.OrganizationDetails.Email },
                        new SearchValueData { Name = "Phone", Value = organizationEsign.OrganizationDetails.Phone },
                        new SearchValueData { Name = "WorkPhone", Value = organizationEsign.OrganizationDetails.WorkPhone },
                        new SearchValueData { Name = "DefaultEsignTerms", Value = organizationEsign.OrganizationDetails.DefaultEsignTerms },
                        new SearchValueData { Name = "OrganizationTypeID", Value = Convert.ToString(organizationEsign.OrganizationDetails.OrganizationTypeID) },
                        new SearchValueData { Name = "IsCompleted", Value = Convert.ToString(organizationEsign.OrganizationDetails.IsCompleted) },
                        new SearchValueData { Name = "IsInProcess", Value = Convert.ToString(organizationEsign.OrganizationDetails.IsInProcess) },
                        new SearchValueData { Name = "SelectedServicePlanIds", Value = selectedServicePlanIds },
                        new SearchValueData { Name = "OrganizationStatusFormCreated", Value = Convert.ToString(EnumOrganizationStatus.In_Process_Esign_Form_Created.GetHashCode()) },
                        new SearchValueData { Name = "LoggedInUserId", Value = Convert.ToString(loggedInUserId) },
                        new SearchValueData { Name = "SystemID", Value = Convert.ToString(systemId) }
                    };

                TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.SaveOrganizationEsign, searchList);
                if (result.TransactionResultId == -1)
                {
                    response.IsSuccess = false;
                    response.Message = Resource.ErrorOccured;
                    return response;
                }
                response.Message = string.Format(Resource.EsignSaved);
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                SetResponse(ex, response);
                return response;
            }
        }

        public ServiceResponse SendEsignEmail(OrganizationEsignDetails organizationEsign, long loggedInUserId)
        {
            string systemId = System.Web.HttpContext.Current.Request.UserHostAddress;
            var response = new ServiceResponse();
            try
            {
                var isSentMail = SendEsignEmail(organizationEsign);

                if (isSentMail)
                {
                    List<SearchValueData> searchList = new List<SearchValueData>
                    {
                        new SearchValueData { Name = "OrganizationEsignID", Value = Convert.ToString(organizationEsign.OrganizationEsignID)},
                        new SearchValueData { Name = "OrganizationID", Value = Convert.ToString(organizationEsign.OrganizationID)},
                        new SearchValueData { Name = "OrganizationStatusEsignEmailSent", Value = Convert.ToString(EnumOrganizationStatus.In_Process_Esign_Email_Sent.GetHashCode()) },
                        new SearchValueData { Name = "LoggedInUserId", Value = Convert.ToString(loggedInUserId) },
                        new SearchValueData { Name = "SystemID", Value = Convert.ToString(systemId) }
                    };

                    TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.UpdateEsignStatus, searchList);
                    if (result.TransactionResultId == -1)
                    {
                        response.IsSuccess = false;
                        response.Message = Resource.ErrorOccured;
                        return response;
                    }
                }

                response.Message = isSentMail ? Resource.EmailSentSuccess : Resource.EmailSentFail;
                response.IsSuccess = isSentMail;
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = string.Format("{0}. Error: {1}", Resource.ErrorOccured, ex.Message);
                return response;
            }
        }

        private bool SendEsignEmail(OrganizationEsignDetails org)
        {
            bool isSentMail = false;
            OrganizationEsignToken token = new OrganizationEsignToken();

            EmailTemplate emailTemplate = GetEntity<EmailTemplate>(new List<SearchValueData>
                                            {
                                                new SearchValueData
                                                {
                                                    Name = "EmailTemplateTypeID",
                                                    Value = Convert.ToInt16(EnumEmailType.EsignEmail).ToString(),
                                                    IsEqual = true
                                                }
                                            });

            token.DisplayName = org.DisplayName;
            token.LogoImage = ConfigSettings.SiteLogo;

            EncryptedMailMessageToken encryptedmailmessagetoken = new EncryptedMailMessageToken();
            encryptedmailmessagetoken.EncryptedValue = Convert.ToInt32(org.OrganizationEsignID);
            encryptedmailmessagetoken.IsUsed = false;
            encryptedmailmessagetoken.ExpireDateTime = DateTime.Now.AddDays(7);
            SaveObject(encryptedmailmessagetoken);

            var id = Crypto.Encrypt(Convert.ToString(encryptedmailmessagetoken.EncryptedMailID));
            token.EsignUrl = Common.GetSiteBaseUrl() + Constants.EsignViewUrl + id;
            emailTemplate.EmailTemplateBody = TokenReplace.ReplaceTokens(emailTemplate.EmailTemplateBody, token);
            isSentMail = Common.SendEmail(emailTemplate.EmailTemplateSubject, null, org.Email, emailTemplate.EmailTemplateBody, EnumEmailType.EsignEmail.ToString());
            return isSentMail;
        }

        public ServiceResponse SetCustomerEsignPage(string id)
        {
            var response = new ServiceResponse();
            try
            {
                SetCustomerEsignModel model = new SetCustomerEsignModel();
                var mailMessageId = Crypto.Decrypt(id);

                EncryptedMailMessageToken encryptedMailMessageToken = new EncryptedMailMessageToken();
                encryptedMailMessageToken = GetEntity<EncryptedMailMessageToken>(Convert.ToInt64(mailMessageId));

                if (encryptedMailMessageToken.EncryptedMailID > 0)
                {
                    if (encryptedMailMessageToken.ExpireDateTime >= DateTime.Now && encryptedMailMessageToken.IsUsed == false)
                    {
                        long organizationEsignId = encryptedMailMessageToken.EncryptedValue;
                        List<SearchValueData> searchList = new List<SearchValueData>
                        {
                            new SearchValueData { Name = "OrganizationEsignID", Value = Convert.ToString(organizationEsignId), IsEqual = true }
                        };
                        CustomerEsignModel esignDetails = GetMultipleEntity<CustomerEsignModel>(StoredProcedure.GetCustomerEsignDetails, searchList);
                        model.CustomerEsignDetails = esignDetails.CustomerEsignDetails;
                        model.OrganizationSettingDetails = esignDetails.OrganizationSettingDetails ?? new OrganizationSettingDetails();
                        model.ServicePlans = esignDetails.ServicePlans;
                        model.ServicePlanComponents = esignDetails.ServicePlanComponents;
                        model.TransactionResult = esignDetails.TransactionResult;
                        model.IsSuccess = true;

                        searchList = new List<SearchValueData>
                        {
                            new SearchValueData { Name = "OrganizationID", Value = Convert.ToString(model.CustomerEsignDetails.OrganizationID), IsEqual = true }
                        };

                        OrganizationFormPageModel data = GetMultipleEntity<OrganizationFormPageModel>(StoredProcedure.SetOrganizationFormPage, searchList);
                        model.OrganizationFormPageModel = data;

                        response.IsSuccess = true;
                    }
                    else
                        response.IsSuccess = false;
                }
                else
                    response.IsSuccess = false;

                response.Data = model;
                return response;
            }
            catch (Exception ex)
            {
                SetResponse(ex, response);
            }
            return response;
        }

        public ServiceResponse CheckDomainNameExists(string domainName)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>
                        {
                            new SearchValueData { Name = "DomainName", Value = domainName, IsEqual = true }
                        };

                response = GetEntity<ServiceResponse>(StoredProcedure.CheckDomainNameExists, searchList);

                return response;
            }
            catch (Exception ex)
            {
                SetResponse(ex, response);
            }
            return response;
        }

        public ServiceResponse CustomerEsign(CustomerEsignModel customerEsign, long loggedInUserId)
        {
            string systemId = System.Web.HttpContext.Current.Request.UserHostAddress;
            var response = new ServiceResponse();
            try
            {
                string selectedServicePlanIds = string.Join(",", customerEsign.ServicePlans.Select(m => m.ServicePlanID).ToArray());

                List<SearchValueData> searchList = new List<SearchValueData>
                    {
                        new SearchValueData { Name = "OrganizationEsignID", Value = Convert.ToString(customerEsign.CustomerEsignDetails.OrganizationEsignID)},
                        new SearchValueData { Name = "OrganizationID", Value = Convert.ToString(customerEsign.CustomerEsignDetails.OrganizationID)},
                        new SearchValueData { Name = "CompanyName", Value = customerEsign.CustomerEsignDetails.CompanyName},
                        new SearchValueData { Name = "DisplayName", Value = customerEsign.CustomerEsignDetails.DisplayName},
                        new SearchValueData { Name = "DomainName", Value = customerEsign.CustomerEsignDetails.DomainName},
                        new SearchValueData { Name = "Email", Value = customerEsign.CustomerEsignDetails.Email },
                        new SearchValueData { Name = "Phone", Value = customerEsign.CustomerEsignDetails.Phone },
                        new SearchValueData { Name = "WorkPhone", Value = customerEsign.CustomerEsignDetails.WorkPhone },
                        new SearchValueData { Name = "EsignName", Value = customerEsign.CustomerEsignDetails.EsignName },
                        new SearchValueData { Name = "IsSMTPSettingsEntered", Value = Convert.ToString(customerEsign.OrganizationSettingDetails.IsSMTPSettingsEntered) },
                        new SearchValueData { Name = "NetworkHost", Value = customerEsign.OrganizationSettingDetails.NetworkHost},
                        new SearchValueData { Name = "NetworkPort", Value = customerEsign.OrganizationSettingDetails.NetworkPort},
                        new SearchValueData { Name = "FromTitle", Value = customerEsign.OrganizationSettingDetails.FromTitle},
                        new SearchValueData { Name = "FromEmail", Value = customerEsign.OrganizationSettingDetails.FromEmail },
                        new SearchValueData { Name = "FromEmailPassword", Value = customerEsign.OrganizationSettingDetails.FromEmailPassword },
                        new SearchValueData { Name = "EnableSSL", Value = Convert.ToString(customerEsign.OrganizationSettingDetails.EnableSSL) },
                        new SearchValueData { Name = "IsTwilioSettingsEntered", Value = Convert.ToString(customerEsign.OrganizationSettingDetails.IsTwilioSettingsEntered) },
                        new SearchValueData { Name = "TwilioCountryCode", Value = customerEsign.OrganizationSettingDetails.TwilioCountryCode },
                        new SearchValueData { Name = "TwilioLocation", Value = customerEsign.OrganizationSettingDetails.TwilioLocation },
                        new SearchValueData { Name = "SelectedServicePlanIds", Value = selectedServicePlanIds },
                        new SearchValueData { Name = "OrganizationStatusEsignCompleted", Value = Convert.ToString(EnumOrganizationStatus.In_Process_Esign_Completed.GetHashCode()) },
                        new SearchValueData { Name = "LoggedInUserId", Value = Convert.ToString(loggedInUserId) },
                        new SearchValueData { Name = "SystemID", Value = Convert.ToString(systemId) }
                    };

                TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.SaveCustomerEsign, searchList);
                if (result.TransactionResultId == -1)
                {
                    response.IsSuccess = false;
                    response.Message = Resource.ErrorOccured;
                    return response;
                }
                response.Message = string.Format(Resource.EsignSaved);
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                SetResponse(ex, response);
                return response;
            }
        }

        public void SetResponse(Exception ex, ServiceResponse response)
        {
            string message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
#if DEBUG
            message += ex.Message;
#endif
            response.IsSuccess = false;
            response.Message = message;
        }
    }
}
