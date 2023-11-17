using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Amazon.S3;
using Newtonsoft.Json;
using PetaPoco;
using Zarephath.Core.Helpers;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;
using System.IO;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Zarephath.Core.Infrastructure.Utility.eBriggsForms;
using Zarephath.Core.Infrastructure.Utility.CareGiverApi;
using Twilio;
using Twilio.Rest.Fax.V1;
using System.Text;
using Org.BouncyCastle.Asn1.Ocsp;
using DocumentFormat.OpenXml.Office2010.Excel;
//using DocumentFormat.OpenXml.Wordprocessing;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class ReferralDataProvider : BaseDataProvider, IReferralDataProvider
    {
        CacheHelper _cacheHelper = new CacheHelper();

        #region ZarePath Data Provider Code

        #region Add Referral

        #region Client Tab

        public ServiceResponse SetAddReferralPage(long referralID, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();

            ReferralModel referralModel = new ReferralModel();

            List<SearchValueData> searchList = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralID", Value = referralID.ToString()},
                 };

            referralModel.AddReferralModel = GetMultipleEntity<AddReferralModel>(StoredProcedure.SetAddReferralPage, searchList);

            if (referralModel.AddReferralModel.Referral != null && referralModel.AddReferralModel.Referral.ReferralID > 0)
            {
                if (Common.HasPermission(Constants.Permission_View_All_Referral) == false &&
                    Common.HasPermission(Constants.Permission_View_Assinged_Referral))
                {
                    if (referralModel.AddReferralModel.Referral.Assignee != loggedInUserID)
                    {
                        response.ErrorCode = Constants.ErrorCode_AccessDenied;
                        return response;
                    }
                }
                referralModel.AddReferralModel.Referral.ReferralCaseloadIDs =
                    !string.IsNullOrEmpty(referralModel.AddReferralModel.Referral.SetSelectedReferralCaseloadIDs)
                    ? referralModel.AddReferralModel.Referral.SetSelectedReferralCaseloadIDs.Split(',').ToList() : null;

            }
            if (referralModel.AddReferralModel.ReferralPayorMapping == null)
                referralModel.AddReferralModel.ReferralPayorMapping = new ReferralPayorMapping();
            referralModel.AddReferralModel.GenderList = Common.SetGenderList();
            referralModel.AddReferralModel.PrivateRoomList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.ROITypes = Common.SetROIType();
            referralModel.AddReferralModel.PrimaryContactGuardianList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.DCSGuardianList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.EmergencyContactList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.NoticeProviderOnFileList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.CareConsentList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.SelfAdministrationList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.HealthInformationList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.AdmissionRequirementList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.AdmissionOrientationList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.VoiceMailList = Common.SetAutorizedNotAutorizedForBoolean();
            referralModel.AddReferralModel.PermissionEmailList = Common.SetAutorizedNotAutorizedForBoolean();
            referralModel.AddReferralModel.PermissionSMSList = Common.SetAutorizedNotAutorizedForBoolean();
            referralModel.AddReferralModel.NetworkCrisisPlanList = Common.SetNameValueDataForYesNoNAData();
            referralModel.AddReferralModel.ZarephathCrisisPlanList = Common.SetNameValueDataForYesNoNAData();
            referralModel.AddReferralModel.PHIList = Common.SetYesNoListForBoolean();

            referralModel.AddReferralModel.ROIList = Common.SetNameValueDataForYesNoNAData();

            referralModel.AddReferralModel.ZSPRespiteList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.ZSPLifeSkillsList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.ZSPCounsellingList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.ZSPRespiteGuardianSignatureList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.ZSPRespiteBHPSignedList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.ZSPLifeSkillsGuardianSignatureList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.ZSPLifeSkillsBHPSignedList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.ZSPCounsellingGuardianSignatureList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.ZSPCounsellingBHPSignedList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.NetworkServicePlanList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.NetworkServiceGuardianSignatureList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.NetworkServiceBHPSignedList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.NSPSPidentifyServiceList = Common.SetNameValueDataForYesNoNAData();
            referralModel.AddReferralModel.BXAssessmentList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.BXAssessmentBHPSignedList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.DemographicList = Common.SetNameValueDataForYesNoNAData();
            referralModel.AddReferralModel.SNCDList = Common.SetNameValueDataForYesNoNAData();
            referralModel.AddReferralModel.ACAssessmentList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.DocumentKind = Common.SetDocumentKindOf();
            referralModel.AddReferralModel.PermissionMailList = Common.SetAutorizedNotAutorizedForBoolean();

            referralModel.AddReferralModel.BindMealsandSummaryofFood = Common.SetThroughoutWeekend();
            referralModel.AddReferralModel.EnumCoordinationofCare = Common.SetCoordinationofCare();
            referralModel.AddReferralModel.SummaryofFood = Common.SetSummaryofFood();
            referralModel.AddReferralModel.Coordinationofcare = Common.SetCoordinationofcare();
            referralModel.AddReferralModel.Coordinationofcare = Common.SetCoordinationofcare();
            referralModel.AddReferralModel.DeleteFilter = Common.SetDeleteFilter();

            //referralModel.AddReferralModel.ContactTypeList.RemoveAll(x => referralModel.AddReferralModel.ContactInformationList.Any(y => y.ContactTypeID == x.ContactTypeID));

            if (referralID > 0)
            {
                referralModel.AddReferralModel.AmazonSettingModel =
                    AmazonFileUpload.GetAmazonModelForClientSideUpload(loggedInUserID,
                                                                       ConfigSettings.AmazoneUploadPath +
                                                                       ConfigSettings.ReferralUploadPath +
                                                                       referralModel.AddReferralModel.Referral.AHCCCSID +
                                                                       "/", ConfigSettings.PrivateAcl);
                if (referralModel.AddReferralModel.Referral != null && referralModel.AddReferralModel.Referral.ReferralID > 0)
                {
                    referralModel.AddReferralModel.Referral.IsEditMode = true;
                    referralModel.AddReferralModel.Referral.Gender = referralModel.AddReferralModel.Referral.Gender == "M" ? "1" : "2";
                    referralModel.AddReferralModel.Referral.EncryptedReferralID = Crypto.Encrypt(Convert.ToString(referralID));
                    response.IsSuccess = true;
                }
                else
                {
                    response.ErrorCode = Constants.ErrorCode_NotFound;
                    return response;
                }
            }
            else
            {
                referralModel.AddReferralModel.Referral = new Referral
                {
                    ReferralStatusID = (int)Common.ReferralStatusEnum.ReferralInitialReview,
                    Population = Constants.Child,
                    Title = Constants.RomanNumeral,
                    ZarephathCrisisPlan = Constants.N,
                    Demographic = Constants.N,
                    SNCD = Constants.N,
                    AROI = Constants.N,
                    NetworkCrisisPlan = Constants.N,
                    NSPSPidentifyService = Constants.NA,
                    PermissionForMail = true,
                    Assignee = loggedInUserID//SessionHelper.LoggedInID
                };
                response.IsSuccess = true;
            }


            #region Set Default Tab To Open


            if (Common.HasPermission(Constants.Permission_ReferralDetails_View_AddUpdate))
                referralModel.AddReferralModel.DefaultTab = Constants.HashUrl_ReferralDetails;
            else if (Common.HasPermission(Constants.Permission_ReferralDocuments_View_AddUpdate))
                referralModel.AddReferralModel.DefaultTab = Constants.HashUrl_ReferralDocument;
            else if (Common.HasPermission(Constants.Permission_ReferralChecklist_View_AddUpdate))
                referralModel.AddReferralModel.DefaultTab = Constants.HashUrl_ReferralChecklist;
            else if (Common.HasPermission(Constants.Permission_ReferralSparForm_View_AddUpdate))
                referralModel.AddReferralModel.DefaultTab = Constants.HashUrl_ReferralSparform;
            else if (Common.HasPermission(Constants.Permission_ReviewMeasurement_All_View_AddUpdate))
                referralModel.AddReferralModel.DefaultTab = Constants.HashUrl_ReferralReviewMeasurement;
            else if (Common.HasPermission(Constants.Permission_ReferralInternalMessaging_View_AddUpdate))
                referralModel.AddReferralModel.DefaultTab = Constants.HashUrl_ReferralInternalMessage;
            else if (Common.HasPermission(Constants.Permission_Schedule_Hisotry))
                referralModel.AddReferralModel.DefaultTab = Constants.Permission_Schedule_Hisotry;
            else if (Common.HasPermission(Constants.Permission_NoteList))
                referralModel.AddReferralModel.DefaultTab = Constants.HashUrl_ReferralNote;
            else
                referralModel.AddReferralModel.DefaultTab = Constants.HashUrl_ReferralAccessDenied;


            if (referralID == 0 && !Common.HasPermission(Constants.Permission_ReferralDetails_AddUpdate))
                referralModel.AddReferralModel.DefaultTab = Constants.HashUrl_ReferralAccessDenied;

            //if (SessionHelper.Permissions.Any(permission => Constants.Permission_ReferralDetails_View_AddUpdate.Contains(permission.PermissionID.ToString())))
            //    referralModel.AddReferralModel.DefaultTab = Constants.HashUrl_ReferralDetails;
            //else if (SessionHelper.Permissions.Any(permission => Constants.Permission_ReferralDocuments_View_AddUpdate.Contains(permission.PermissionID.ToString())))
            //    referralModel.AddReferralModel.DefaultTab = Constants.HashUrl_ReferralDocument;
            //else if (SessionHelper.Permissions.Any(permission => Constants.Permission_ReferralChecklist_View_AddUpdate.Contains(permission.PermissionID.ToString())))
            //    referralModel.AddReferralModel.DefaultTab = Constants.HashUrl_ReferralChecklist;
            //else if (SessionHelper.Permissions.Any(permission => Constants.Permission_ReferralSparForm_View_AddUpdate.Contains(permission.PermissionID.ToString())))
            //    referralModel.AddReferralModel.DefaultTab = Constants.HashUrl_ReferralSparform;
            //else if (SessionHelper.Permissions.Any(permission => Constants.Permission_ReviewMeasurement_All_View_AddUpdate.Contains(permission.PermissionID.ToString())))
            //    referralModel.AddReferralModel.DefaultTab = Constants.HashUrl_ReferralReviewMeasurement;
            //else if (SessionHelper.Permissions.Any(permission => Constants.Permission_ReferralInternalMessaging_View_AddUpdate.Contains(permission.PermissionID.ToString())))
            //    referralModel.AddReferralModel.DefaultTab = Constants.HashUrl_ReferralInternalMessage;
            //else if (SessionHelper.Permissions.Any(permission => Constants.Permission_Schedule_Hisotry.Contains(permission.PermissionID.ToString())))
            //    referralModel.AddReferralModel.DefaultTab = Constants.Permission_Schedule_Hisotry;
            //else if (SessionHelper.Permissions.Any(permission => Constants.Permission_Billing_Notes.Contains(permission.PermissionID.ToString())))
            //    referralModel.AddReferralModel.DefaultTab = Constants.HashUrl_ReferralNote;
            //else
            //    referralModel.AddReferralModel.DefaultTab = Constants.HashUrl_ReferralAccessDenied;

            #endregion


            response.Data = referralModel.AddReferralModel;
            response.IsSuccess = true;
            return response;
        }
        public ServiceResponse HC_AddReferralSSNLog(HC_AddReferralModel addReferralModel, long loggedInUserID)
        {
            AuditLogTable audit = new AuditLogTable();
            audit.AuditActionType = "ViewSSSN";
            audit.DataModel = "Referral";
            audit.DateTimeStamp = DateTime.UtcNow; ;
            audit.ParentKeyFieldID = addReferralModel.Referral.ReferralID;
            audit.ChildKeyFieldID = addReferralModel.Referral.ReferralID;
            audit.ValueBefore = JsonConvert.SerializeObject(addReferralModel.Referral);
            audit.ValueAfter = JsonConvert.SerializeObject(addReferralModel.Referral);
            audit.Changes = "Show Referral SSN ";
            audit.SystemID = Common.GetHostAddress();
            IAuditLogDataProvider auditLogDataProvider = new AuditLogDataProvider();
            ServiceResponse response = auditLogDataProvider.AddAuditLog(audit, loggedInUserID);
            return response;
        }


        public static Int32 GetAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;

            var a = (today.Year * 100 + today.Month) * 100 + today.Day;
            var b = (dateOfBirth.Year * 100 + dateOfBirth.Month) * 100 + dateOfBirth.Day;

            return (a - b) / 10000;
        }

        public ServiceResponse AddReferral(AddReferralModel addReferralModel, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();

            if (addReferralModel.Referral != null)
            {
                bool isEditMode = addReferralModel.Referral.ReferralID > 0;

                #region Server Side Validation Check for Referral
                var tempReferral = new Referral();

                if (isEditMode)
                {
                    tempReferral = GetEntity<Referral>(addReferralModel.Referral.ReferralID);
                }

                if (addReferralModel.Referral.ReferralStatusID == null)
                {
                    addReferralModel.Referral.ReferralStatusID = isEditMode
                                                                     ? tempReferral.ReferralStatusID ?? (int)
                                                                       ReferralStatus.ReferralStatuses.New_Referral
                                                                     : (int)
                                                                       ReferralStatus.ReferralStatuses.New_Referral;
                }

                if (addReferralModel.Referral.Assignee == null)
                {
                    addReferralModel.Referral.ReferralStatusID = isEditMode
                                                                     ? tempReferral.Assignee ?? loggedInUserID
                                                                     : loggedInUserID;
                }

                // Check for the Age, if the age is greater than 18 then the referral cannot be created.
                //if (addReferralModel.Referral.Dob != null && addReferralModel.Referral.Dob.Value != null)
                //{
                //    int age = new DateTime(DateTime.Now.Subtract(addReferralModel.Referral.Dob.Value).Ticks).Year - 1;
                //    if (age > 18)
                //    {
                //        response.Message = Resource.ReferralCannotBeCreated;
                //        return response;
                //    }
                //}





                if (addReferralModel.Referral.IsSaveAsDraft.HasValue && !addReferralModel.Referral.IsSaveAsDraft.Value)
                {
                    // Check if there no primary contact and legal contact then show alert to user that referral cannot be created.
                    var result = addReferralModel.ContactInformationList.FirstOrDefault(q => q.ContactTypeID != (int)Common.ContactTypes.PrimaryPlacement);
                    if (result != null)
                        result = addReferralModel.ContactInformationList.FirstOrDefault(q => q.ContactTypeID != (int)Common.ContactTypes.LegalGuardian);

                    if (result == null)
                    {
                        response.Message = Resource.ContactRequired;
                        return response;
                    }

                    //if(!addReferralModel.ContactInformationList.Any(c=>c.IsDCSLegalGuardian || c.IsPrimaryPlacementLegalGuardian))
                    //{
                    //    response.Message = Resource.MustSetLegalGuardian;
                    //    return response;
                    //}


                    // Check for the Zare Phath Service, atleast one service plan should be selected.

                    if (!addReferralModel.Referral.RespiteService && !addReferralModel.Referral.LifeSkillsService &&
                          !addReferralModel.Referral.CounselingService && !addReferralModel.Referral.ConnectingFamiliesService)
                    {
                        response.Message = Resource.ZarePhathServicePlanRequired;
                        return response;
                    }

                }

                // Check for the Add/Edit Contact and accordingly update.                             

                #endregion

                #region Check Client Exist or not for New Referral Only. We are using AHCCCS ID. AHCCCS ID will unique for each client

                addReferralModel.Referral.IsEditMode = addReferralModel.Referral.ReferralID > 0;

                var searchParam = new List<SearchValueData>();
                searchParam.Add(new SearchValueData { Name = "AHCCCSID", Value = addReferralModel.Referral.AHCCCSID, IsEqual = true });
                //searchParam.Add(new SearchValueData { Name = "CISNumber", Value = addReferralModel.Referral.CISNumber, IsEqual = true });

                var clientInfo = isEditMode ? new Client() : GetEntity<Client>(searchParam) ?? new Client();


                #endregion

                #region Check for client with SAME AHCCCS ID and CIS Number does exist

                //searchParam.Clear();
                //searchParam.Add(new SearchValueData { Name = "AHCCCSID", Value = addReferralModel.Referral.AHCCCSID, IsEqual = true });
                //searchParam.Add(new SearchValueData { Name = "CISNumber", Value = addReferralModel.Referral.CISNumber, IsEqual = true });

                if (isEditMode)
                {
                    searchParam.Add(new SearchValueData { Name = "ClientID", Value = addReferralModel.Referral.ClientID.ToString(), IsNotEqual = true });
                }

                var referral = GetEntityList<Referral>(searchParam);
                if (referral.Any())
                {
                    response.Message = Resource.ClientAlreadyExists;
                    return response;
                }

                #endregion

                #region Add Client into Client table if Referral is new and No related client information found into existing Database

                if (!isEditMode && clientInfo.ClientID == 0)
                {
                    clientInfo = new Client
                    {
                        FirstName = addReferralModel.Referral.FirstName,
                        MiddleName = addReferralModel.Referral.MiddleName,
                        LastName = addReferralModel.Referral.LastName,
                        Dob = addReferralModel.Referral.Dob,
                        Gender = addReferralModel.Referral.Gender == "1" ? Constants.Gender_Male : Constants.Gender_FeMale,
                        ClientNumber = addReferralModel.Referral.ClientNumber,
                        AHCCCSID = addReferralModel.Referral.AHCCCSID,
                        CISNumber = addReferralModel.Referral.CISNumber
                    };
                    SaveObject(clientInfo, loggedInUserID);
                }


                #region for Insert/Update CaseLoad Record

                string customWhere = string.Format("(ReferralID={0})", addReferralModel.Referral.ReferralID);

                List<ReferralCaseload> referralCaseload = GetEntityList<ReferralCaseload>(null, customWhere);
                foreach (var item in referralCaseload)
                {
                    DeleteEntity<ReferralCaseload>(item.ReferralCaseloadID);
                }
                foreach (var id in addReferralModel.Referral.ReferralCaseloadIDs)
                {
                    ReferralCaseload aCaseload = new ReferralCaseload();
                    aCaseload.ReferralID = addReferralModel.Referral.ReferralID;
                    aCaseload.EmployeeID = Convert.ToInt64(id);
                    SaveObject(aCaseload, loggedInUserID);
                }

                #endregion

                #endregion

                #region Add/Update Referral Related details

                #region Add/Update Referral
                addReferralModel.Referral.ClientID = clientInfo.ClientID > 0 ? clientInfo.ClientID : addReferralModel.Referral.ClientID;
                addReferralModel.Referral.Gender = addReferralModel.Referral.Gender == "1" ? Constants.Gender_Male : Constants.Gender_FeMale;

                #region Set default value of dropdowns for save as draft
                if (addReferralModel.Referral.CaseManagerID == 0)
                {
                    addReferralModel.Referral.CaseManagerID = null;
                }
                if (addReferralModel.Referral.AgencyLocationID == 0)
                {
                    addReferralModel.Referral.AgencyLocationID = null;
                }
                if (addReferralModel.Referral.AgencyID == 0)
                {
                    addReferralModel.Referral.AgencyID = null;
                }
                if (addReferralModel.Referral.RegionID == null)
                {
                    addReferralModel.Referral.RegionID = null;
                }
                if (addReferralModel.Referral.ReferralStatusID == 0)
                {
                    addReferralModel.Referral.ReferralStatusID = null;
                }
                if (addReferralModel.Referral.DropOffLocation == 0)
                {
                    addReferralModel.Referral.DropOffLocation = null;
                }
                if (addReferralModel.Referral.PickUpLocation == 0)
                {
                    addReferralModel.Referral.PickUpLocation = null;
                }
                if (addReferralModel.Referral.FrequencyCodeID == 0)
                {
                    addReferralModel.Referral.FrequencyCodeID = null;
                }


                #endregion
                // This will save the information in the Referral Table

                if (isEditMode && tempReferral.ReferralStatusID == (int)ReferralStatus.ReferralStatuses.Active &&
                    addReferralModel.Referral.ReferralStatusID == (int)ReferralStatus.ReferralStatuses.Inactive)
                {
                    GetScalar(StoredProcedure.InactiveSchedule,
                              new List<SearchValueData>
                                  {
                                      new SearchValueData
                                          {
                                              Name = "ReferralID",
                                              Value = tempReferral.ReferralID.ToString()
                                          },
                                      new SearchValueData
                                          {
                                              Name = "CancelStatus",
                                              Value = ((int) ScheduleStatus.ScheduleStatuses.Cancelled).ToString()
                                          },
                                      new SearchValueData {Name = "Comment", Value = @Resource.InactiveScheduleComment},
                                      new SearchValueData {Name = "WhoCancel", Value = Constants.Office},
                                      new SearchValueData
                                          {
                                              Name = "ChangeStatus",
                                              Value = string.Join(",", new List<String>
                                                  {
                                                      ((int) ScheduleStatus.ScheduleStatuses.Unconfirmed).ToString(),
                                                      ((int) ScheduleStatus.ScheduleStatuses.Waiting_List).ToString()
                                                  })
                                          }

                                  });
                }
                SaveObject(addReferralModel.Referral, loggedInUserID);
                #endregion

                #region Add Referral Diagnosis Code details

                if (addReferralModel.DxCodeMappingList.Any())
                {
                    var tempList =
                        addReferralModel.DxCodeMappingList.Where(m => m.ReferralDXCodeMappingID == 0).ToList();

                    foreach (var dxCodeMapping in tempList)
                    {
                        ReferralDXCodeMapping referralDxCodeMapping = new ReferralDXCodeMapping
                        {
                            DXCodeID = dxCodeMapping.DXCodeID,
                            EndDate = dxCodeMapping.EndDate,
                            StartDate = dxCodeMapping.StartDate,
                            Precedence = dxCodeMapping.Precedence,
                            ReferralID = addReferralModel.Referral.ReferralID
                        };
                        SaveObject(referralDxCodeMapping, loggedInUserID);

                        Common.CreateAuditTrail(AuditActionType.Create, addReferralModel.Referral.ReferralID, referralDxCodeMapping.ReferralDXCodeMappingID,
                                                 new ReferralDXCodeMapping(), referralDxCodeMapping, loggedInUserID);
                    }

                    var changedDxCode = addReferralModel.DxCodeMappingList.Where(m => m.ReferralDXCodeMappingID > 0).ToList();
                    foreach (var dxcode in changedDxCode)
                    {
                        ReferralDXCodeMapping temp = GetEntity<ReferralDXCodeMapping>(dxcode.ReferralDXCodeMappingID);

                        ReferralDXCodeMapping referralDxCodeMapping = new ReferralDXCodeMapping
                        {
                            ReferralID = addReferralModel.Referral.ReferralID,
                            DXCodeID = dxcode.DXCodeID,
                            EndDate = dxcode.EndDate,
                            StartDate = dxcode.StartDate,
                            Precedence = dxcode.Precedence,
                            ReferralDXCodeMappingID = dxcode.ReferralDXCodeMappingID,
                            CreatedBy = dxcode.CreatedBy,
                            CreatedDate = dxcode.CreatedDate
                        };
                        SaveObject(referralDxCodeMapping, loggedInUserID);

                        Common.CreateAuditTrail(AuditActionType.Update, addReferralModel.Referral.ReferralID, referralDxCodeMapping.ReferralDXCodeMappingID,
                                                 temp, referralDxCodeMapping, loggedInUserID);
                    }
                    //List<string> dxCodelist
                    //    = addReferralModel.DxCodeMappingList.Select(dxcode => dxcode.DXCodeID).ToList();

                    //GetEntity<DXCode>("SaveDxCodeMapping", new List<SearchValueData>
                    //{
                    //    new SearchValueData {Name = "DxCodelist",Value = string.Join(",", dxCodelist)},
                    //    new SearchValueData {Name = "ReferralID",Value = Convert.ToString(addReferralModel.Referral.ReferralID) },
                    //    new SearchValueData {Name = "SystemID",Value = HttpContext.Current.Request.UserHostAddress},
                    //    new SearchValueData {Name = "LoggedInUserId",Value = Convert.ToString(loggedInUserID)},
                    //    new SearchValueData {Name = "ServerDateTime",Value = Convert.ToString(DateTime.Now.ToString(Constants.DbDateTimeFormat))}
                    //});

                }

                #endregion

                #region Add Referral Sibling Add / Update

                if (addReferralModel.ReferralSiblingMappingList.Count > 0)
                {
                    var tempList = addReferralModel.ReferralSiblingMappingList.Where(m => m.ReferralSiblingMappingID == 0).ToList();

                    foreach (var model in tempList)
                    {
                        ReferralSiblingMapping referralSiblingMapping = new ReferralSiblingMapping
                        {
                            ReferralID1 = addReferralModel.Referral.ReferralID,
                            ReferralID2 = model.ReferralID,
                        };
                        SaveObject(referralSiblingMapping, loggedInUserID);
                    }

                    var changedreferralSiblingMapping = addReferralModel.ReferralSiblingMappingList.Where(m => m.ReferralSiblingMappingID > 0).ToList();
                    foreach (var model in changedreferralSiblingMapping)
                    {
                        ReferralSiblingMapping referralSiblingMapping = new ReferralSiblingMapping
                        {
                            ReferralSiblingMappingID = model.ReferralSiblingMappingID,
                            ReferralID1 = model.ReferralID1,
                            ReferralID2 = model.ReferralID2,
                            CreatedBy = model.CreatedBy,
                            CreatedDate = model.CreatedDate
                        };
                        SaveObject(referralSiblingMapping, loggedInUserID);
                    }
                }

                #endregion

                #region Add/Update Referral/Client Contact Information
                // Save Contact Info in the database

                if (addReferralModel.ContactInformationList.Any())
                {
                    foreach (var addAndListContactInformation in addReferralModel.ContactInformationList.OrderBy(c => c.ContactTypeID))
                    {
                        addAndListContactInformation.ReferralID = addReferralModel.Referral.ReferralID;
                        addAndListContactInformation.ClientID = addReferralModel.Referral.ClientID;
                        //addReferralModel.AddAndListContactInformation = addAndListContactInformation;
                        // This method will save the entry in the Contact and Contact Mapping table.
                        SaveContact(addAndListContactInformation, loggedInUserID);
                    }
                }

                // Save the payor info in the table ReferralPayorMapping
                #endregion

                #region Add/Update Referral Payor information. Referral/Client can have a single active payor at a time.




                if (!isEditMode)
                {
                    InsertEntryInReferralPayorMapping(addReferralModel, loggedInUserID);
                }
                else
                {
                    searchParam.Clear();
                    searchParam.Add(new SearchValueData { Name = "ReferralID", Value = addReferralModel.Referral.ReferralID.ToString() });
                    searchParam.Add(new SearchValueData { Name = "IsActive", Value = Convert.ToString("1") });
                    ReferralPayorMapping referralPayorMappings = GetEntity<ReferralPayorMapping>(searchParam);
                    if (referralPayorMappings != null)
                    {

                        if (referralPayorMappings.PayorID == addReferralModel.ReferralPayorMapping.PayorID)
                        {
                            referralPayorMappings.PayorEffectiveDate = addReferralModel.ReferralPayorMapping.PayorEffectiveDate;
                            referralPayorMappings.PayorEffectiveEndDate = addReferralModel.ReferralPayorMapping.PayorEffectiveEndDate;
                        }
                        else
                        {
                            referralPayorMappings.IsActive = false;
                            InsertEntryInReferralPayorMapping(addReferralModel, loggedInUserID);
                        }
                        SaveObject(referralPayorMappings, loggedInUserID);

                    }
                    else
                    {
                        InsertEntryInReferralPayorMapping(addReferralModel, loggedInUserID);
                    }
                }
                #endregion


                #endregion

                response.IsSuccess = true;

                #region SET AUDIT TRAIL

                Common.CreateAuditTrail(isEditMode ? AuditActionType.Update : AuditActionType.Create, addReferralModel.Referral.ReferralID, addReferralModel.Referral.ReferralID,
                                                 tempReferral, addReferralModel.Referral, loggedInUserID);

                #endregion

                response.Message = isEditMode ? string.Format(Resource.ReferralUpdatedSuccessfully, Resource.Referral) : string.Format(Resource.ReferralSavedSuccessfully, Resource.Referral);

                if (!isEditMode)
                {
                    addReferralModel.Referral.EncryptedReferralID = Crypto.Encrypt(Convert.ToString(addReferralModel.Referral.ReferralID));
                }

                #region Insert  Referral Respite Usage Limit

                //List<SearchValueData> searchReferralRespiteUsageLimit = new List<SearchValueData> { new SearchValueData { Name = "ReferralID", Value = addReferralModel.Referral.ReferralID.ToString() } };
                //ReferralRespiteUsageLimit alreadyexistrecord = GetEntity<ReferralRespiteUsageLimit>(StoredProcedure.GetAlreadyExistReferralRespiteUsageLimit, searchReferralRespiteUsageLimit);

                //if (alreadyexistrecord == null)
                //{
                //    DateTime getDate;
                //    if (DateTime.Now.Month > ConfigSettings.ResetRespiteUsageMonth)
                //    {
                //        getDate = new DateTime(DateTime.Now.Year, ConfigSettings.ResetRespiteUsageMonth, ConfigSettings.ResetRespiteUsageDay);
                //    }
                //    else
                //    {
                //        DateTime lastYear = DateTime.Today.AddYears(-(ConfigSettings.ResetRespiteUsageDay));
                //        getDate = new DateTime(lastYear.Year, ConfigSettings.ResetRespiteUsageMonth, ConfigSettings.ResetRespiteUsageDay);
                //    }
                //    ReferralRespiteUsageLimit referralRespiteUsageLimit = new ReferralRespiteUsageLimit();
                //    referralRespiteUsageLimit.ReferralID = addReferralModel.Referral.ReferralID;
                //    referralRespiteUsageLimit.StartDate = getDate;
                //    referralRespiteUsageLimit.EndDate = getDate.AddYears(ConfigSettings.ResetRespiteUsageDay).AddDays(-(ConfigSettings.ResetRespiteUsageDay));
                //    referralRespiteUsageLimit.IsActive = true;
                //    referralRespiteUsageLimit.UsedRespiteHours = 0;
                //    SaveObject(referralRespiteUsageLimit, loggedInUserID);
                //}

                #endregion

                #region  Notify CaseManager When Referral Status Change To Inactive
                if (addReferralModel.NotifyCmForInactiveStatus)
                {
                    if (addReferralModel.Referral.ReferralStatusID == ((int)ReferralStatus.ReferralStatuses.ReferralAccepted))
                    {
                        SendReferralAcceptedStatusEmailToCm(addReferralModel.Referral.ReferralID);
                    }
                    else if (addReferralModel.Referral.ReferralStatusID == ((int)ReferralStatus.ReferralStatuses.Inactive))
                    {
                        SendInactiveStatusEmailToCm(addReferralModel.Referral.ReferralID);
                    }
                }
                #endregion

                response.Data = addReferralModel;
            }
            return response;
        }

        private void InsertEntryInReferralPayorMapping(AddReferralModel addReferralModel, long loggedInUserID)
        {
            if (addReferralModel.ReferralPayorMapping.PayorID > 0)
            {
                ReferralPayorMapping addReferralPayorMapping = new ReferralPayorMapping
                {
                    PayorID = addReferralModel.ReferralPayorMapping.PayorID,
                    PayorEffectiveDate = addReferralModel.ReferralPayorMapping.PayorEffectiveDate,
                    PayorEffectiveEndDate = addReferralModel.ReferralPayorMapping.PayorEffectiveEndDate,
                    ReferralID = addReferralModel.Referral.ReferralID,
                    IsActive = true
                };
                SaveObject(addReferralPayorMapping, loggedInUserID);
            }
        }

        public ServiceResponse AddContact(AddReferralModel addReferralModel, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();

            if (loggedInUserID == 0)
            {
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.UserSessionExpired);
                return response;
            }

            // This method will save the entry in the Contact and Contact Mapping Table
            SaveContact(addReferralModel.AddAndListContactInformation, loggedInUserID);

            List<SearchValueData> searchList = new List<SearchValueData>
            {
                new SearchValueData {Name = "ReferralID", Value = addReferralModel.AddAndListContactInformation.ReferralID.ToString()}
            };

            ContactInformationModal contactInformationModal = new ContactInformationModal
            {
                AddAndListContactInformation =
                        GetEntityList<AddAndListContactInformation>("GetContactInformation", searchList),
                Contact = addReferralModel.Contact
            };
            response.IsSuccess = true;
            response.Data = contactInformationModal;
            return response;
        }

        private void SaveContact(AddAndListContactInformation model, long loggedInUserID)
        {
            if (model != null &&
                model.ReferralID > 0 &&
                model.ClientID > 0)
            {
                switch (model.ContactTypeID)
                {
                    case (int)Common.ContactTypes.PrimaryPlacement:
                        model.IsDCSLegalGuardian = false;
                        model.IsNoticeProviderOnFile = false;
                        break;
                    case (int)Common.ContactTypes.LegalGuardian:
                        model.IsPrimaryPlacementLegalGuardian = false;
                        break;
                    default:
                        model.IsDCSLegalGuardian = false;
                        model.IsNoticeProviderOnFile = false;
                        model.IsPrimaryPlacementLegalGuardian = false;
                        break;
                }





                var searchParam = new List<SearchValueData>();
                Contact contact = new Contact();
                Contact tempContact = new Contact();

                bool isPrimaryPlacementAndLegalSame = false;
                bool isLegalGuardianNeedUpdate = true;

                long oldLegalGuardianContactId = 0;
                if (model.ContactTypeID == (int)Common.ContactTypes.LegalGuardian)
                {
                    oldLegalGuardianContactId = model.ContactID;
                    searchParam.Add(new SearchValueData { Name = "ContactTypeID", Value = ((int)Common.ContactTypes.PrimaryPlacement).ToString() });
                    searchParam.Add(new SearchValueData { Name = "ReferralID", Value = model.ReferralID.ToString() });
                    var contactMapping = GetEntity<ContactMapping>(searchParam) ?? new ContactMapping();
                    if (contactMapping.IsPrimaryPlacementLegalGuardian)
                    {
                        model.ContactID = contactMapping.ContactID;
                        model.IsDCSLegalGuardian = false;
                        model.IsNoticeProviderOnFile = false;
                        isLegalGuardianNeedUpdate = false;
                    }
                    else if (model.ContactID == contactMapping.ContactID)
                    {
                        isPrimaryPlacementAndLegalSame = true;
                        //add new contact id because here  
                        //model.ContactID = 0;
                    }
                }
                //else
                //{

                if (((model.MasterContactUpdated && model.ContactID > 0) || model.ContactID > 0) && isPrimaryPlacementAndLegalSame == false)
                {
                    searchParam.Clear();
                    searchParam.Add(new SearchValueData { Name = "ContactID", Value = model.ContactID.ToString() });
                    contact = GetEntity<Contact>(searchParam) ?? new Contact();
                }
                /* else
                 {
                     searchParam.Clear();
                     searchParam.Add(new SearchValueData { Name = "FirstName", Value = model.FirstName, IsEqual = true });
                     searchParam.Add(new SearchValueData { Name = "LastName", Value = model.LastName, IsEqual = true });
                     if (!string.IsNullOrEmpty(model.Email))
                         searchParam.Add(new SearchValueData { Name = "Email", Value = model.Email, IsEqual = true });
                     searchParam.Add(new SearchValueData { Name = "Address", Value = model.Address, IsEqual = true });
                     searchParam.Add(new SearchValueData { Name = "City", Value = model.City, IsEqual = true });
                     searchParam.Add(new SearchValueData { Name = "State", Value = model.State, IsEqual = true });
                     searchParam.Add(new SearchValueData { Name = "ZipCode", Value = model.ZipCode, IsEqual = true });
                     searchParam.Add(new SearchValueData { Name = "Phone1", Value = model.Phone1, IsEqual = true });
                     searchParam.Add(new SearchValueData { Name = "LanguageID", Value = model.LanguageID.ToString(), IsEqual = true });
                     contact = GetEntity<Contact>(searchParam) ?? new Contact();
                 }*/
                // }
                if (isLegalGuardianNeedUpdate)
                {

                    //if (contact.ContactID == 0
                    //    || (contact.FirstName.ToLower() != model.FirstName.ToLower()
                    //    || contact.LastName.ToLower() != model.LastName.ToLower()
                    //    || ((contact.Email != null && model.Email != null && contact.Email.ToLower() != model.Email.ToLower()) || contact.Email != model.Email)
                    //    || ((contact.Phone2 != null && model.Phone2 != null && contact.Phone2.ToLower() != model.Phone2.ToLower()) || contact.Phone2 != model.Phone2)
                    //    || contact.Address.ToLower() != model.Address.ToLower()
                    //    || contact.Address.ToLower() != model.Address.ToLower()
                    //    || contact.City.ToLower() != model.City.ToLower()
                    //    || contact.State.ToLower() != model.State.ToLower()
                    //    || contact.ZipCode.ToLower() != model.ZipCode.ToLower()
                    //    || contact.Phone1.ToLower() != model.Phone1.ToLower()
                    //    || contact.LanguageID != model.LanguageID)
                    //    )
                    //{



                    //searchParam.Clear();
                    //searchParam.Add(new SearchValueData { Name = "FirstName", Value = model.FirstName, IsEqual = true });
                    //searchParam.Add(new SearchValueData { Name = "LastName", Value = model.LastName, IsEqual = true });
                    //if (!string.IsNullOrEmpty(model.Email))
                    //    searchParam.Add(new SearchValueData { Name = "Email", Value = model.Email, IsEqual = true });
                    //searchParam.Add(new SearchValueData { Name = "Address", Value = model.Address, IsEqual = true });
                    //searchParam.Add(new SearchValueData { Name = "City", Value = model.City, IsEqual = true });
                    //searchParam.Add(new SearchValueData { Name = "State", Value = model.State, IsEqual = true });
                    //searchParam.Add(new SearchValueData { Name = "ZipCode", Value = model.ZipCode, IsEqual = true });
                    //searchParam.Add(new SearchValueData { Name = "Phone1", Value = model.Phone1, IsEqual = true });
                    ////if (!string.IsNullOrEmpty(model.Phone2))
                    ////    searchParam.Add(new SearchValueData { Name = "Phone2", Value = model.Phone2, IsEqual = true });
                    //searchParam.Add(new SearchValueData { Name = "LanguageID", Value = model.LanguageID.ToString(), IsEqual = true });
                    //var tempContact = GetEntity<Contact>(searchParam);// new Contact();


                    //if (tempContact == null && contact.ContactID > 0)
                    //{
                    //    contact.FirstName = model.FirstName;
                    //    contact.LastName = model.LastName;
                    //    contact.Email = model.Email;
                    //    contact.Address = model.Address;
                    //    contact.City = model.City;
                    //    contact.State = model.State;
                    //    contact.ZipCode = model.ZipCode;
                    //    contact.Phone1 = model.Phone1;
                    //    contact.Phone2 = model.Phone2;
                    //    contact.LanguageID = model.LanguageID;
                    //    SaveObject(contact, loggedInUserID);
                    //}
                    //else
                    //{
                    //    contact = tempContact ?? new Contact();
                    //}
                    //}
                    tempContact = JsonConvert.DeserializeObject<Contact>(JsonConvert.SerializeObject(contact));
                    tempContact.ContactType = GetContactTypeName(Convert.ToInt16(model.ContactTypeID));
                    if (contact.ContactID > 0 && !model.AddNewContactDetails)
                    {
                        contact.FirstName = model.FirstName;
                        contact.LastName = model.LastName;
                        contact.Email = model.Email;
                        contact.ApartmentNo = model.ApartmentNo;
                        contact.Address = model.Address;
                        contact.City = model.City;
                        contact.State = model.State;
                        contact.ZipCode = model.ZipCode;
                        contact.Phone1 = model.Phone1;
                        contact.Phone2 = model.Phone2;
                        contact.LanguageID = model.LanguageID;
                        contact.Latitude = model.Latitude;
                        contact.Longitude = model.Longitude;
                        SaveObject(contact, loggedInUserID);
                        //contact = new Contact();
                    }
                    else
                    {
                        contact.ContactID = 0;
                        contact.FirstName = model.FirstName;
                        contact.LastName = model.LastName;
                        contact.Email = model.Email;
                        contact.ApartmentNo = model.ApartmentNo;
                        contact.Address = model.Address;
                        contact.City = model.City;
                        contact.State = model.State;
                        contact.ZipCode = model.ZipCode;
                        contact.Phone1 = model.Phone1;
                        contact.Phone2 = model.Phone2;
                        contact.Latitude = model.Latitude;
                        contact.Longitude = model.Longitude;
                        contact.LanguageID = model.LanguageID;
                        SaveObject(contact, loggedInUserID);
                    }

                    contact.ContactType = GetContactTypeName(Convert.ToInt16(model.ContactTypeID));
                    Common.CreateAuditTrail(contact.ContactID == 0 ? AuditActionType.Create : AuditActionType.Update, model.ReferralID, contact.ContactID,
                                                 tempContact, contact, loggedInUserID);

                }
                else
                {
                    tempContact = JsonConvert.DeserializeObject<Contact>(JsonConvert.SerializeObject(contact));
                    tempContact.ContactType = GetContactTypeName(Convert.ToInt16(model.ContactTypeID));
                    var oldContact = new Contact();
                    oldContact.ContactID = oldLegalGuardianContactId;
                    oldContact.FirstName = model.FirstName;
                    oldContact.LastName = model.LastName;
                    oldContact.Email = model.Email;
                    oldContact.ApartmentNo = model.ApartmentNo;
                    oldContact.Address = model.Address;
                    oldContact.City = model.City;
                    oldContact.State = model.State;
                    oldContact.ZipCode = model.ZipCode;
                    oldContact.Phone1 = model.Phone1;
                    oldContact.Phone2 = model.Phone2;
                    oldContact.LanguageID = model.LanguageID;
                    oldContact.ContactType = GetContactTypeName(Convert.ToInt16(model.ContactTypeID));

                    Common.CreateAuditTrail(contact.ContactID == 0 ? AuditActionType.Create : AuditActionType.Update, model.ReferralID, contact.ContactID,
                                                 oldContact, tempContact, loggedInUserID);
                }

                if (contact.ContactID > 0)
                {
                    searchParam.Clear();
                    ContactMapping contactMapping;
                    if (model.ContactMappingID > 0)
                    {
                        searchParam.Add(new SearchValueData { Name = "ContactMappingID", Value = model.ContactMappingID.ToString() });
                        searchParam.Add(new SearchValueData { Name = "ReferralID", Value = model.ReferralID.ToString() });
                        contactMapping = GetEntity<ContactMapping>(searchParam) ?? new ContactMapping();
                    }
                    else
                    {
                        searchParam.Add(new SearchValueData { Name = "ContactTypeID", Value = model.ContactTypeID.ToString() });
                        searchParam.Add(new SearchValueData { Name = "ReferralID", Value = model.ReferralID.ToString() });
                        contactMapping = GetEntity<ContactMapping>(searchParam) ?? new ContactMapping();
                    }
                    ContactMapping temp = JsonConvert.DeserializeObject<ContactMapping>(JsonConvert.SerializeObject(contactMapping));
                    //ContactMapping contactMapping = GetEntity<ContactMapping>(model.ContactMappingID)??new ContactMapping();
                    contactMapping.ContactID = contact.ContactID;
                    contactMapping.ReferralID = model.ReferralID;
                    contactMapping.ClientID = model.ClientID;
                    contactMapping.IsEmergencyContact = model.IsEmergencyContact;
                    contactMapping.ContactTypeID = model.ContactTypeID;
                    contactMapping.IsPrimaryPlacementLegalGuardian = model.IsPrimaryPlacementLegalGuardian;
                    contactMapping.IsDCSLegalGuardian = model.IsDCSLegalGuardian;
                    contactMapping.ROIExpireDate = model.ROIExpireDate;
                    contactMapping.ROIType = model.ROIType;
                    contactMapping.Relation = model.Relation;
                    contactMapping.IsNoticeProviderOnFile = model.IsNoticeProviderOnFile;
                    SaveObject(contactMapping, loggedInUserID);

                    Common.CreateAuditTrail(temp.ContactMappingID == 0 ? AuditActionType.Create : AuditActionType.Update, model.ReferralID, contactMapping.ContactMappingID,
                                                 temp, contactMapping, loggedInUserID);




                }


            }
        }

        private string GetContactTypeName(int? contactTypeId = null)
        {
            if (contactTypeId == (int)ContactType.ContactTypes.Primary_Placement)
                return ContactType.ContactTypes.Primary_Placement.ToString();
            if (contactTypeId == (int)ContactType.ContactTypes.Legal_Guardian)
                return ContactType.ContactTypes.Legal_Guardian.ToString();
            if (contactTypeId == (int)ContactType.ContactTypes.Secondary_Placement)
                return ContactType.ContactTypes.Secondary_Placement.ToString();
            if (contactTypeId == (int)ContactType.ContactTypes.School_Teacher)
                return ContactType.ContactTypes.School_Teacher.ToString();
            if (contactTypeId == (int)ContactType.ContactTypes.Relative)
                return ContactType.ContactTypes.Relative.ToString();
            if (contactTypeId == (int)ContactType.ContactTypes.Relative2)
                return ContactType.ContactTypes.Relative2.ToString();
            return "Same As After Change";

        }


        public List<Contact> GetContactList(string searchText, int pageSize = 10)
        {
            List<Contact> contactlist = GetEntityList<Contact>("GetContactListForAutoCompleter", new List<SearchValueData>
                {

                new SearchValueData{Name = "SearchText",Value = searchText},
                new SearchValueData{Name = "PazeSize",Value = pageSize.ToString()}
            });
            return contactlist;
        }

        public ServiceResponse DeteteContact(long contactMappingID, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();


            if (loggedInUserID == 0)
            {
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.UserSessionExpired);
                return response;
            }

            if (contactMappingID > 0)
            {
                ContactMapping temp = GetEntity<ContactMapping>(contactMappingID);
                DeleteEntity<ContactMapping>(contactMappingID);
                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordDeletedSuccessfully, Resource.Contact);
                Common.CreateAuditTrail(AuditActionType.Delete, temp.ReferralID, temp.ContactMappingID,
                                                 temp, new ContactMapping(), loggedInUserID);

            }


            // Logic written if the user deletes its primary contact and if we want to delete its legal guardian.

            //if (contactMapping.ContactMappingID > 0 && contactMapping.ContactTypeID == (int)Common.ContactTypes.PrimaryPlacement && contactMapping.IsPrimaryPlacementLegalGuardian)
            //{
            //    List<SearchValueData> searchParam = new List<SearchValueData>
            //        {
            //            new SearchValueData { Name = "ReferralID", Value = contactMapping.ReferralID.ToString()}
            //        };

            //    List<ContactMapping> contactMappingList = GetEntityList<ContactMapping>(searchParam);

            //    if (contactMappingList.Any())
            //    {

            //        ContactMapping deleteContactMapping = contactMappingList.FirstOrDefault(q => q.IsDCSLegalGuardian);
            //        if (deleteContactMapping != null && deleteContactMapping.ContactMappingID > 0)
            //            DeleteEntity<ContactMapping>(deleteContactMapping.ContactMappingID);

            //    }

            //    DeleteEntity<ContactMapping>(contactMapping.ContactMappingID);

            //    response.IsSuccess = true;
            //    response.Message = string.Format(Resource.RecordDeletedSuccessfully, Resource.Contact);
            //}
            return response;
        }

        public ServiceResponse DeleteReferralPayorMapping(long referralPayorMappingID, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();

            if (loggedInUserID == 0)
            {
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.UserSessionExpired);
                return response;
            }

            if (referralPayorMappingID > 0)
            {
                ReferralPayorMapping referralPayorMapping = GetEntity<ReferralPayorMapping>(referralPayorMappingID);
                if (referralPayorMapping != null && referralPayorMapping.ReferralPayorMappingID > 0)
                {
                    referralPayorMapping.IsDeleted = true;
                    SaveObject(referralPayorMapping, loggedInUserID);
                    response.IsSuccess = true;
                    response.Message = string.Format(Resource.RecordDeletedSuccessfully, Resource.Payor);
                }
            }
            return response;
        }

        public ServiceResponse MarkPayorAsActive(long referralID, long referralPayorMappingID, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();
            if (referralPayorMappingID > 0)
            {

                ReferralPayorMapping oldModel = GetEntity<ReferralPayorMapping>(referralPayorMappingID);

                var searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "ReferralID", Value = referralID.ToString() });
                searchList.Add(new SearchValueData { Name = "ReferralPayorMappingID", Value = referralPayorMappingID.ToString() });
                searchList.Add(new SearchValueData { Name = "LoggedIdID", Value = loggedInUserID.ToString() });
                GetScalar(StoredProcedure.MarkPayorAsActive, searchList);
                GetReferralPayorDetails data = GetReferralPayorDetails(referralID);
                response.Data = data;
                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.Record);



                #region Audit Log
                ReferralPayorMapping referralPayorMapping = data.ReferralPayorMapping;

                Common.CreateAuditTrail(oldModel.ReferralPayorMappingID == 0 ? AuditActionType.Create : AuditActionType.Update,
                    oldModel.ReferralID, referralPayorMapping.ReferralPayorMappingID,
                                                 oldModel, referralPayorMapping, loggedInUserID);
                #endregion


            }
            return response;
        }

        private GetReferralPayorDetails GetReferralPayorDetails(long referralID)
        {
            var searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ReferralID", Value = referralID.ToString() });
            GetReferralPayorDetails model = GetMultipleEntity<GetReferralPayorDetails>(StoredProcedure.GetReferralPayorDetails, searchList);
            model.ReferralPayorMapping.TempPayorID = model.ReferralPayorMapping.PayorID;
            return model;

        }

        public ServiceResponse GetPayorDetailsByReferralID(long referralID)
        {
            ServiceResponse response = new ServiceResponse();
            var searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ReferralID", Value = referralID.ToString() });
            List<ListReferralPayorMapping> lstPayors = GetEntityList<ListReferralPayorMapping>(StoredProcedure.GetPayorDetailsByReferralID, searchList);
            response.Data = lstPayors;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse GetReferralPayorDetail(long referralPayorMappingID)
        {
            ServiceResponse response = new ServiceResponse();
            if (referralPayorMappingID > 0)
            {

                var searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "ReferralPayorMappingID", Value = referralPayorMappingID.ToString() });
                ReferralPayorMapping model = GetEntity<ReferralPayorMapping>(StoredProcedure.GetReferralPayorByID, searchList);
                response.Data = model;
                response.IsSuccess = true;
            }
            return response;
        }

        public ServiceResponse UpdateReferralPayorInformation(ReferralPayorMapping model, long loggedInId)
        {
            ServiceResponse response = new ServiceResponse();
            if (model != null && model.ReferralPayorMappingID > 0)
            {

                ReferralPayorMapping referralPayorMapping = GetEntity<ReferralPayorMapping>(model.ReferralPayorMappingID);
                ReferralPayorMapping oldModel = JsonConvert.DeserializeObject<ReferralPayorMapping>(JsonConvert.SerializeObject(referralPayorMapping));
                referralPayorMapping.PayorEffectiveDate = model.PayorEffectiveDate;
                referralPayorMapping.PayorEffectiveEndDate = model.PayorEffectiveEndDate;
                referralPayorMapping.TempPayorID = referralPayorMapping.PayorID;
                SaveObject(referralPayorMapping, loggedInId);
                response.Data = GetReferralPayorDetails(model.ReferralID);
                response.IsSuccess = true;
                response.Message = Resource.ReferralPayorDetailsUpdated;


                Common.CreateAuditTrail(model.ReferralPayorMappingID == 0 ? AuditActionType.Create : AuditActionType.Update,
                    model.ReferralID, referralPayorMapping.ReferralPayorMappingID,
                                                 oldModel, referralPayorMapping, loggedInId);

            }
            return response;
        }


        #region Referral Audit Logs

        public ServiceResponse GetAuditLogList(SearchRefAuditLogListModel searchModel, int pageIndex, int pageSize,
                                                string sortIndex, string sortDirection, long loggedInId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchModel != null)
            {
                searchList.Add(new SearchValueData { Name = "ParentKeyFieldID", Value = Crypto.Decrypt(searchModel.EncryptedReferralID) });
                searchList.Add(new SearchValueData { Name = "UpdatedBy", Value = searchModel.UpdatedBy });
                searchList.Add(new SearchValueData { Name = "Table", Value = searchModel.Table });
                searchList.Add(new SearchValueData { Name = "ActionName", Value = searchModel.ActionName });
                if (searchModel.UpdatedFromDate.HasValue)
                    searchList.Add(new SearchValueData { Name = "FromDate", Value = TimeZoneInfo.ConvertTimeToUtc(searchModel.UpdatedFromDate.Value).ToString(Constants.DbDateTimeFormat) });
                if (searchModel.UpdatedToDate.HasValue)
                    searchList.Add(new SearchValueData { Name = "ToDate", Value = TimeZoneInfo.ConvertTimeToUtc(searchModel.UpdatedToDate.Value.AddDays(1)).ToString(Constants.DbDateTimeFormat) });

            }

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<AuditChangeModel> totalData = GetEntityList<AuditChangeModel>(StoredProcedure.GetAuditLogList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<AuditChangeModel> getAuditChangeList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = getAuditChangeList;
            response.IsSuccess = true;
            return response;
        }

        #endregion

        #region Referral BX Contract

        public ServiceResponse GetBXContractList(SearchRefBXContractListModel searchModel, int pageIndex, int pageSize,
                                                string sortIndex, string sortDirection, long loggedInId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchModel != null)
                searchList.Add(new SearchValueData { Name = "ReferralID", Value = Crypto.Decrypt(searchModel.EncryptedReferralID) });


            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<BXContractModel> totalData = GetEntityList<BXContractModel>(StoredProcedure.GetBXContractList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<BXContractModel> getBXContractModel = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = getBXContractModel;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse SaveBXContract(RefBXContractPageModel model, int pageIndex, int pageSize,
                                        string sortIndex, string sortDirection, long loggedInId)
        {
            var response = new ServiceResponse();

            if (model.ReferralBehaviorContract.ReferralID == 0)
                model.ReferralBehaviorContract.ReferralID =
                    Convert.ToInt64(Crypto.Decrypt(model.SearchRefBXContractListModel.EncryptedReferralID));


            if (model.ReferralBehaviorContract.ReferralBehaviorContractID == 0)
                model.ReferralBehaviorContract.IsActive = true;

            SaveObject(model.ReferralBehaviorContract, loggedInId);
            response = GetBXContractList(model.SearchRefBXContractListModel, pageIndex, pageSize, sortIndex, sortDirection, loggedInId);
            response.IsSuccess = true;
            response.Message = Resource.BXContractAdded;
            return response;
        }

        public ServiceResponse UpdateBXContractStatus(ReferralBehaviorContract model, long loggedInId)
        {

            var response = new ServiceResponse();
            SaveObject(model, loggedInId);
            response.IsSuccess = true;
            response.Message = Resource.BXContractUpdated;
            return response;
        }

        public ServiceResponse SaveSuspensionDetails(ReferralSuspention model, string EncryptedReferralID, long loggedInId, bool ResetSuspension = false, bool ResetBXContract = false)
        {
            var response = new ServiceResponse();
            if (model.ReferralID == 0) model.ReferralID = Convert.ToInt64(Crypto.Decrypt(EncryptedReferralID));

            if (ResetSuspension && model.ReferralID > 0)
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "ReferralID", Value = model.ReferralID.ToString() });
                searchList.Add(new SearchValueData { Name = "ResetBXContract", Value = ResetBXContract.ToString() });
                object isDeleted = GetScalar(StoredProcedure.DeleteReferralSuspension, searchList);
                response.IsSuccess = Convert.ToBoolean(isDeleted);
            }
            else
            {
                Referral referral = GetEntity<Referral>(model.ReferralID);
                Referral tempReferral = JsonConvert.DeserializeObject<Referral>(JsonConvert.SerializeObject(referral));

                #region Update All Future Schedule

                if (referral.ReferralStatusID == (int)ReferralStatus.ReferralStatuses.Active)
                {
                    GetScalar(StoredProcedure.InactiveSchedule,
                              new List<SearchValueData>
                                          {
                                              new SearchValueData{Name = "ReferralID",Value = referral.ReferralID.ToString()},
                                              new SearchValueData{Name = "CancelStatus",Value = ((int) ScheduleStatus.ScheduleStatuses.Cancelled).ToString()},
                                              new SearchValueData {Name = "Comment", Value = @Resource.InactiveScheduleComment},
                                              new SearchValueData {Name = "WhoCancel", Value = Constants.Office},
                                              new SearchValueData{Name = "ChangeStatus",Value = string.Join(",", new List<String>{((int) ScheduleStatus.ScheduleStatuses.Unconfirmed).ToString(),((int) ScheduleStatus.ScheduleStatuses.Waiting_List).ToString()})}
                                          });
                }

                #endregion

                if (model.MakeClientInActive)
                {
                    #region Make CLient Inacative w/ All The Check

                    #region Check all the Data for as per Referral Status


                    //CHECK FOR INACTIVE STATUS
                    if (referral.FirstName == null || referral.LastName == null || referral.Dob == null ||
                           referral.Gender == null || referral.AHCCCSID == null || referral.RegionID == null ||
                           referral.AHCCCSEnrollDate == null || referral.LanguageID == null || referral.DropOffLocation == null ||
                           referral.PickUpLocation == null || referral.FrequencyCodeID == null ||
                           referral.CaseManagerID == null || referral.ReferralSourceID == 0 || referral.ReferralDate == null || referral.Assignee == null)
                    {
                        response.Message = Resource.ReferralStatusChange;
                        return response;
                    }


                    List<ContactMapping> contactlist = GetEntityList<ContactMapping>(new List<SearchValueData>
                            {
                                new SearchValueData{Name = "ReferralID",Value =  Convert.ToString(referral.ReferralID)},
                            });
                    if (contactlist.Count <= 0)
                    {
                        response.Message = Resource.ReferralStatusChange;
                        return response;
                    }
                    List<ReferralPayorMapping> payorlist = GetEntityList<ReferralPayorMapping>(new List<SearchValueData>
                            {
                                new SearchValueData{Name = "ReferralID",Value =  Convert.ToString(referral.ReferralID)},
                                new SearchValueData{Name = "IsActive",Value =  Convert.ToString(Constants.IsActiveStatus)},
                            });
                    if (payorlist.Count <= 0)
                    {
                        response.Message = Resource.ReferralStatusChange;
                        return response;
                    }
                    if (!referral.RespiteService && !referral.LifeSkillsService && !referral.CounselingService && !referral.ConnectingFamiliesService)
                    {
                        response.Message = Resource.ReferralStatusChange;
                        return response;
                    }

                    #endregion

                    #endregion

                    referral.ReferralStatusID = (int)ReferralStatus.ReferralStatuses.Inactive;
                    referral.ClosureDate = DateTime.Now;
                    referral.ClosureReason = Resource.TerminateInactiveReason;
                    SaveObject(referral, loggedInId);

                    #region SET AUDIT TRAIL

                    Common.CreateAuditTrail(AuditActionType.Update, referral.ReferralID, referral.ReferralID, tempReferral, referral, loggedInId);

                    #endregion
                }
                if (model.SuspentionLength == 0)
                {
                    model.ReturnEligibleDate = null;
                }
                else
                {
                    model.ReturnEligibleDate = model.ReferralSuspentionID == 0 ? DateTime.UtcNow.AddDays(model.SuspentionLength) : model.CreatedDate.AddDays(model.SuspentionLength);
                }

                SaveObject(model, loggedInId);
                response.Data = model;
                response.IsSuccess = true;
            }
            if (response.IsSuccess)
                response.Message = Resource.SuspensionSuccessfully;
            else
                response.Message = Resource.ErrorOccured;
            return response;
        }

        public ServiceResponse GetSuspensionDetails(string encryptedReferralID)
        {
            var response = new ServiceResponse();
            SuspensionDetailModel suspensionDetailModel = new SuspensionDetailModel();

            suspensionDetailModel = GetMultipleEntity<SuspensionDetailModel>(StoredProcedure.GetSuspensionDetail,
                                                       new List<SearchValueData>
                                                             {
                                                                 new SearchValueData
                                                                     {
                                                                         Name = "ReferralID",
                                                                         Value = Crypto.Decrypt(encryptedReferralID),
                                                                         IsEqual = true
                                                                     },
                                                                  new SearchValueData
                                                                     {
                                                                         Name = "dayCount",
                                                                         Value = "90",
                                                                         IsEqual = true
                                                                     }
                                                             });
            //ReferralSuspention model = (string.IsNullOrEmpty(EncryptedReferralID) ? new ReferralSuspention() : GetEntity<ReferralSuspention>(new List<SearchValueData>() { new SearchValueData() { Name = "ReferralID", Value = Crypto.Decrypt(EncryptedReferralID), IsEqual = true } })) ??
            //                           new ReferralSuspention();
            if (suspensionDetailModel.ReferralSuspention == null)
                suspensionDetailModel.ReferralSuspention = new ReferralSuspention();

            response.IsSuccess = true;
            response.Data = suspensionDetailModel;
            return response;
        }

        #endregion


        #region Referral Update AHCCCC ID
        public ServiceResponse UpdateAhcccsid(ReferralAhcccsDetails model, Referral referral, long loggedInUserId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            if (model != null)
            {
                searchList.Add(new SearchValueData { Name = "ReferralID", Value = model.ReferralID.ToString() });
                searchList.Add(new SearchValueData { Name = "AHCCCSID", Value = model.AHCCCSID });
                searchList.Add(new SearchValueData { Name = "NewAHCCCSID", Value = model.NewAHCCCSID });

                //int value = (int)GetScalar(StoredProcedure.UpdateAhcccsId, searchList);
                ReferralAhcccsUpdateModel data = GetMultipleEntity<ReferralAhcccsUpdateModel>(StoredProcedure.UpdateAhcccsId, searchList);
                if (data.ReturnValue == 1)
                    response.Message = Resource.AHCCCSIDAlreadyExist;
                if (data.ReturnValue == 2)
                    response.Message = Resource.AHCCCSIDShouldNotMatchWithPrevious;
                if (data.ReturnValue == 3)
                    response.Message = Resource.AHCCCSIDMissingInvalid;
                if (data.ReturnValue == 4)
                {
                    response.IsSuccess = true;
                    response.Message = Resource.AHCCCSIDUpdatedSuccessfully;

                    data.UpdatedReferral = JsonConvert.DeserializeObject<Referral>(JsonConvert.SerializeObject(referral));
                    data.UpdatedReferral.AHCCCSID = model.NewAHCCCSID;
                    Common.CreateAuditTrail(AuditActionType.Update, model.ReferralID, model.ReferralID,
                                                 referral, data.UpdatedReferral, loggedInUserId);
                }


            }
            else
                response.Message = Resource.ErrorOccured;
            return response;
        }
        #endregion

        #endregion

        #region ReferralCheckList Tab

        public ServiceResponse SetReferralCheckList(long referralId)
        {
            ServiceResponse response = new ServiceResponse();
            if (referralId > 0)
            {
                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralID", Value = Convert.ToString(referralId)}
                };

                SetReferralCheckListModel setReferralCheckListModel =
                    GetMultipleEntity<SetReferralCheckListModel>(StoredProcedure.SetReferralCheckList, searchlist);
                if (setReferralCheckListModel.ReferralCheckList == null)
                    setReferralCheckListModel.ReferralCheckList = new ReferralCheckList();

                setReferralCheckListModel = AutoMapReferralChecklistData(setReferralCheckListModel);
                setReferralCheckListModel.YesNoList = Common.SetYesNoListForBoolean();
                response.IsSuccess = true;
                response.Data = setReferralCheckListModel;
            }
            else
                response.Message = Resource.GetReferralCheckListError;

            return response;
        }

        public SetReferralCheckListModel AutoMapReferralChecklistData(SetReferralCheckListModel model)
        {
            if ((model.ReferralDetailModel.RespiteService || model.ReferralDetailModel.LifeSkillsService ||
                 model.ReferralDetailModel.CounselingService) && !model.ReferralCheckList.ServiceRequested)
                model.ReferralCheckList.ServiceRequested = true;

            //if (model.ReferralDetailModel.RespiteService && !model.ReferralCheckList.RespiteService)
            model.ReferralCheckList.RespiteService = model.ReferralDetailModel.RespiteService;

            //if (model.ReferralDetailModel.LifeSkillsService && !model.ReferralCheckList.LifeSkillsService)
            model.ReferralCheckList.LifeSkillsService = model.ReferralDetailModel.LifeSkillsService;

            //if (model.ReferralDetailModel.CounselingService && !model.ReferralCheckList.CounselingService)
            model.ReferralCheckList.CounselingService = model.ReferralDetailModel.CounselingService;

            model.ReferralCheckList.ConnectingFamiliesService = model.ReferralDetailModel.ConnectingFamiliesService;

            model.ReferralCheckList.CASIIScoreText = model.ReferralDetailModel.CASIIScore;
            model.ReferralCheckList.CASIIScore = !string.IsNullOrEmpty(model.ReferralDetailModel.CASIIScore);


            //if (!model.ReferralCheckList.FacilitatorContacted && model.ReferralDetailModel.NotifyCaseManager)
            model.ReferralCheckList.FacilitatorContacted = true;// model.ReferralDetailModel.NotifyCaseManager;

            //if (model.ReferralDetailModel.NotifyCaseManager &&
            //    string.IsNullOrEmpty(model.ReferralCheckList.FacilitatorContactedText))
            //    model.ReferralCheckList.FacilitatorContactedText = Constants.Yes + " (" + Resource.FacilitatorContacted + ")";

            if (model.ReferralDetailModel.AHCCCSEnrollDate.HasValue && !model.ReferralCheckList.AHCCCSVerification)
                model.ReferralCheckList.AHCCCSVerification = true;

            if (model.ReferralDetailModel.AHCCCSEnrollDate.HasValue)
                model.ReferralCheckList.AHCCCSVerificationText = model.ReferralDetailModel.AHCCCSEnrollDate.Value;

            //if (!model.ReferralCheckList.ASPRespiteIntervention && model.ReferralDetailModel.NetworkServicePlan)
            //model.ReferralCheckList.ASPRespiteIntervention = model.ReferralDetailModel.NSPSPIdentifyService;



            //if (string.IsNullOrEmpty(model.ReferralCheckList.ASPRespiteInterventionText))
            //    model.ReferralCheckList.ASPRespiteInterventionText = model.ReferralDetailModel.NetworkServicePlan
            //                                                             ? Constants.Yes
            //                                                             : Constants.No;

            //if ((model.ReferralDetailModel.NetworkServicePlan.HasValue) & !model.ReferralCheckList.SPExpireAndGuardianBHPSigned)// || model.ReferralDetailModel.NSPGuardianSignature || model.ReferralDetailModel.NSPBHPSigned

            model.ReferralCheckList.SPExpireAndGuardianBHPSigned = model.ReferralDetailModel.NetworkServicePlan;
            //if (model.ReferralDetailModel.NSPExpirationDate.HasValue)
            model.ReferralCheckList.SPExpireAndGuardianBHPSignedText = model.ReferralDetailModel.NSPExpirationDate;
            model.ReferralCheckList.SPSignedByGuardian = model.ReferralDetailModel.NSPGuardianSignature;
            model.ReferralCheckList.SPSignedByBHP = model.ReferralDetailModel.NSPBHPSigned;
            model.ReferralCheckList.ASPRespiteIntervention = model.ReferralDetailModel.NSPSPIdentifyService;

            //if (model.ReferralDetailModel.NSPGuardianSignature)


            //if (string.IsNullOrEmpty(model.ReferralCheckList.SPSignedByGuardianText))
            //    model.ReferralCheckList.SPSignedByGuardianText = model.ReferralDetailModel.NSPGuardianSignature
            //                                                         ? Constants.Yes
            //                                                         : Constants.No;

            //if (model.ReferralDetailModel.NSPBHPSigned)


            //if (string.IsNullOrEmpty(model.ReferralCheckList.SPSignedByBHPText))
            //    model.ReferralCheckList.SPSignedByBHPText = model.ReferralDetailModel.NSPBHPSigned
            //                                                         ? Constants.Yes
            //                                                         : Constants.No;

            //if (!model.ReferralCheckList.CoreAssessmentORAnnualUpdateCompleted && model.ReferralDetailModel.BXAssessmentExpirationDate.HasValue)
            model.ReferralCheckList.CoreAssessmentORAnnualUpdateCompleted = model.ReferralDetailModel.BXAssessment;

            if (model.ReferralDetailModel.BXAssessmentExpirationDate.HasValue)
                model.ReferralCheckList.CoreAssessmentORAnnualUpdateCompletedText = model.ReferralDetailModel.BXAssessmentExpirationDate.Value;


            //if (model.ReferralDetailModel.BXAssessmentBHPSigned && !model.ReferralCheckList.AssessmentBHPSigned)
            model.ReferralCheckList.AssessmentBHPSigned = model.ReferralDetailModel.BXAssessmentBHPSigned;

            //if (string.IsNullOrEmpty(model.ReferralCheckList.AssessmentBHPSignedChecked))
            //{
            //    model.ReferralCheckList.AssessmentBHPSignedChecked = model.ReferralDetailModel.BXAssessmentBHPSigned.ToString();
            //}

            //if (!model.ReferralCheckList.SNCD && model.ReferralDetailModel.SNCD == Constants.Y)
            model.ReferralCheckList.SNCD = model.ReferralDetailModel.SNCD;//== Constants.Y;

            if (model.ReferralDetailModel.SNCDExpirationDate.HasValue)
                model.ReferralCheckList.SNCDText = model.ReferralDetailModel.SNCDExpirationDate.Value;


            //if (!model.ReferralCheckList.Demographics && model.ReferralDetailModel.Demographic == Constants.Y)
            model.ReferralCheckList.Demographics = model.ReferralDetailModel.Demographic;// == Constants.Y;

            if (model.ReferralDetailModel.DemographicExpirationDate.HasValue)
                model.ReferralCheckList.DemographicsText = model.ReferralDetailModel.DemographicExpirationDate.Value;

            //if (!model.ReferralCheckList.ROI && !string.IsNullOrEmpty(model.ReferralDetailModel.AROI))
            model.ReferralCheckList.ROI = model.ReferralDetailModel.AROI;// == Constants.Y;
            model.ReferralCheckList.ROIFromAgencyID = model.ReferralDetailModel.AROIAgencyID;
            model.ReferralCheckList.ROIFromAgencyName = model.ReferralDetailModel.ROIFromAgencyName;
            model.ReferralCheckList.ROIFromExpirationDate = model.ReferralDetailModel.AROIExpirationDate;

            //if (string.IsNullOrEmpty(model.ReferralCheckList.ROIText))
            //    model.ReferralCheckList.ROIText = model.ReferralDetailModel.AROI == Constants.Y ? Constants.Yes + " (" + model.ReferralDetailModel.NickName + ")" : Constants.No;

            //if (!model.ReferralCheckList.DiagnosisCodes && !string.IsNullOrEmpty(model.ReferralDetailModel.DXCodes))
            model.ReferralCheckList.DiagnosisCodes = model.ReferralCheckList.DiagnosisCodes;
            model.ReferralCheckList.DiagnosisCodesText = model.ReferralDetailModel.DXCodes;
            model.ReferralCheckList.DiagnosisCodes = !string.IsNullOrEmpty(model.ReferralDetailModel.DXCodes);


            model.ReferralCheckList.ReferralChecklist = model.ReferralDetailModel.IsCazDoc;


            model.ReferralCheckList.AgencyID = model.ReferralDetailModel.AgencyID;
            model.ReferralCheckList.CaseManagerID = model.ReferralDetailModel.CaseManagerID;

            model.ReferralCheckList.ReferringAgency = model.ReferralDetailModel.ReferringAgency;
            model.ReferralCheckList.FacilatorInformation = model.ReferralDetailModel.FacilatorInformation;
            model.ReferralCheckList.Payor = model.ReferralDetailModel.Payor;


            //if (string.IsNullOrEmpty(model.ReferralCheckList.ReferralChecklistText) &&
            //    model.ReferralDetailModel.IsCazDoc)
            //{
            //    model.ReferralCheckList.ReferralChecklistText = Constants.Yes;
            //}
            return model;
        }

        public ServiceResponse SaveReferralCheckList(ReferralCheckList referralCheckListModel, List<DXCodeMappingList> dxCodeMappingList, long loggedInUserId)
        {
            var response = new ServiceResponse();

            if (referralCheckListModel.ReferralID > 0)
            {
                Referral referral = GetEntity<Referral>(referralCheckListModel.ReferralID);
                ReferralCheckList tempReferralCheckList = new ReferralCheckList();
                if (referralCheckListModel.ReferralCheckListID > 0)
                {
                    tempReferralCheckList = GetEntity<ReferralCheckList>(referralCheckListModel.ReferralCheckListID);
                }
                else
                {
                    List<ReferralCheckList> checkLists = GetEntityList<ReferralCheckList>(
                            new List<SearchValueData>
                                {
                                    new SearchValueData
                                        {
                                            Name = "ReferralID",
                                            Value = referralCheckListModel.ReferralID.ToString(),
                                            IsEqual = true
                                        },
                                    new SearchValueData
                                        {
                                            Name = "ReferralCheckListID",
                                            Value = referralCheckListModel.ReferralCheckListID.ToString(),
                                            IsNotEqual = true
                                        }
                                }
                        );

                    if (checkLists != null && checkLists.Any())
                    {
                        response.IsSuccess = false;
                        response.Message = Resource.ReferralCheckListExists;
                        return response;
                    }
                }



                if (referral.ReferralID > 0)
                {
                    if (referralCheckListModel.IsCheckListCompleted && !tempReferralCheckList.IsCheckListCompleted)
                    {
                        referralCheckListModel.ChecklistCompletedBy = SessionHelper.LoggedInID;
                        referralCheckListModel.ChecklistCompletedDate = DateTime.UtcNow;
                    }
                    else if (!referralCheckListModel.IsCheckListCompleted)
                    {
                        referralCheckListModel.ChecklistCompletedBy = null;
                        referralCheckListModel.ChecklistCompletedDate = null;
                    }
                    response = SaveObject(referralCheckListModel, loggedInUserId, Resource.ReferralCheckListSaveSuccess);

                    #region Reverse Mapping code
                    //referral.RespiteService=referralCheckListModel.ASPRespiteIntervention
                    referral.NotifyCaseManager = referralCheckListModel.FacilitatorContacted;
                    referral.RespiteService = referralCheckListModel.RespiteService;
                    referral.LifeSkillsService = referralCheckListModel.LifeSkillsService;
                    referral.CounselingService = referralCheckListModel.CounselingService;
                    referral.ConnectingFamiliesService = referralCheckListModel.ConnectingFamiliesService;
                    referral.AHCCCSEnrollDate = referralCheckListModel.AHCCCSVerificationText;

                    referral.NetworkServicePlan = referralCheckListModel.SPExpireAndGuardianBHPSigned;
                    referral.NSPExpirationDate = referralCheckListModel.SPExpireAndGuardianBHPSignedText;
                    referral.NSPGuardianSignature = referralCheckListModel.SPSignedByGuardian;
                    referral.NSPBHPSigned = referralCheckListModel.SPSignedByBHP;
                    referral.NSPSPidentifyService = referralCheckListModel.ASPRespiteIntervention;

                    referral.BXAssessmentExpirationDate = referralCheckListModel.CoreAssessmentORAnnualUpdateCompletedText;
                    referral.BXAssessment = referralCheckListModel.CoreAssessmentORAnnualUpdateCompleted;
                    referral.BXAssessmentBHPSigned = referralCheckListModel.AssessmentBHPSigned;
                    referral.SNCD = referralCheckListModel.SNCD;//? Constants.Y : Constants.N;
                    referral.SNCDExpirationDate = referralCheckListModel.SNCDText;
                    referral.Demographic = referralCheckListModel.Demographics;// ? Constants.Y : Constants.N;
                    referral.DemographicExpirationDate = referralCheckListModel.DemographicsText;

                    referral.AROI = referralCheckListModel.ROI;//? Constants.Y : Constants.N;
                    referral.AROIAgencyID = referralCheckListModel.ROIFromAgencyID;
                    referral.AROIExpirationDate = referralCheckListModel.ROIFromExpirationDate;

                    referral.CASIIScore = referralCheckListModel.CASIIScoreText;

                    if (referralCheckListModel.AgencyID > 0)
                        referral.AgencyID = referralCheckListModel.AgencyID;
                    if (referralCheckListModel.CaseManagerID > 0)
                        referral.CaseManagerID = referralCheckListModel.CaseManagerID;



                    SaveObject(referral, loggedInUserId);
                    #endregion

                    #region Add Referral Diagnosis Code details

                    if (dxCodeMappingList != null && dxCodeMappingList.Any())
                    {
                        var tempList = dxCodeMappingList.Where(m => m.ReferralDXCodeMappingID == 0).ToList();
                        foreach (var dxCodeMapping in tempList)
                        {
                            ReferralDXCodeMapping referralDxCodeMapping = new ReferralDXCodeMapping
                            {
                                DXCodeID = dxCodeMapping.DXCodeID,
                                EndDate = dxCodeMapping.EndDate,
                                StartDate = dxCodeMapping.StartDate,
                                Precedence = dxCodeMapping.Precedence,
                                ReferralID = referral.ReferralID
                            };
                            SaveObject(referralDxCodeMapping, loggedInUserId);

                            Common.CreateAuditTrail(AuditActionType.Create, referral.ReferralID, referralDxCodeMapping.ReferralDXCodeMappingID,
                                                     new ReferralDXCodeMapping(), referralDxCodeMapping, loggedInUserId);
                        }

                        var changedDxCode = dxCodeMappingList.Where(m => m.ReferralDXCodeMappingID > 0).ToList();
                        foreach (var dxcode in changedDxCode)
                        {
                            ReferralDXCodeMapping temp = GetEntity<ReferralDXCodeMapping>(dxcode.ReferralDXCodeMappingID);

                            ReferralDXCodeMapping referralDxCodeMapping = new ReferralDXCodeMapping
                            {
                                ReferralID = referral.ReferralID,
                                DXCodeID = dxcode.DXCodeID,
                                EndDate = dxcode.EndDate,
                                StartDate = dxcode.StartDate,
                                Precedence = dxcode.Precedence,
                                ReferralDXCodeMappingID = dxcode.ReferralDXCodeMappingID,
                                CreatedBy = dxcode.CreatedBy,
                                CreatedDate = dxcode.CreatedDate
                            };
                            SaveObject(referralDxCodeMapping, loggedInUserId);

                            Common.CreateAuditTrail(AuditActionType.Update, referral.ReferralID, referralDxCodeMapping.ReferralDXCodeMappingID,
                                                     temp, referralDxCodeMapping, loggedInUserId);
                        }


                    }

                    #endregion


                    response.IsSuccess = true;
                    response.Data = SetReferralCheckList(referral.ReferralID).Data;
                }
                else
                    response.Message = Resource.NoReferralFound;
            }
            else
            {
                response.Message = Resource.ReferralCheckListSaveError;
                response.IsSuccess = false;
            }



            return response;
        }

        #endregion ReferralCheckList Tab

        #region ReferralSparForm Tab

        public ServiceResponse SetReferralSparForm(long referralId)
        {
            ServiceResponse response = new ServiceResponse();
            if (referralId > 0)
            {
                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralID", Value = Convert.ToString(referralId)}
                };

                SetReferralSparFormModel setReferralSparFormModel =
                    GetMultipleEntity<SetReferralSparFormModel>(StoredProcedure.SetReferralSparForm, searchlist);
                if (setReferralSparFormModel.ReferralSparForm == null)
                    setReferralSparFormModel.ReferralSparForm = new ReferralSparForm();
                if (!setReferralSparFormModel.ReferralSparForm.ReviewDate.HasValue)
                    setReferralSparFormModel.ReferralSparForm.ReviewDate = DateTime.Now;
                //if (setReferralSparFormModel.ReferralSparForm.AdmissionDate == null)
                setReferralSparFormModel.ReferralSparForm.AdmissionDate = setReferralSparFormModel.ReferralForReferralSparFormModel.ReferralDate.HasValue ? setReferralSparFormModel.ReferralForReferralSparFormModel.ReferralDate : DateTime.Now;
                if (setReferralSparFormModel.ReferralSparForm.Date == DateTime.MinValue)
                    setReferralSparFormModel.ReferralSparForm.Date = DateTime.Now;
                if (setReferralSparFormModel.ReferralSparForm.DemographicDate == DateTime.MinValue)
                    setReferralSparFormModel.ReferralSparForm.DemographicDate = DateTime.Now;

                response.IsSuccess = true;
                response.Data = setReferralSparFormModel;
            }
            else
                response.Message = Resource.GetReferralSparFormError;
            return response;
        }

        public ServiceResponse SaveReferralSparForm(ReferralSparForm referralSparForm, long loggedInUserId)
        {
            var response = new ServiceResponse();
            if (referralSparForm.ReferralID > 0)
            {
                Referral referral = GetEntity<Referral>(referralSparForm.ReferralID);
                ReferralSparForm tempReferralSparForm = new ReferralSparForm();
                if (referralSparForm.ReferralSparFormID > 0)
                {
                    tempReferralSparForm = GetEntity<ReferralSparForm>(referralSparForm.ReferralSparFormID);
                }
                if (referral.ReferralID > 0)
                {
                    if (referralSparForm.IsSparFormCompleted && !tempReferralSparForm.IsSparFormCompleted)
                    {
                        referralSparForm.SparFormCompletedBy = SessionHelper.LoggedInID;
                        referralSparForm.SparFormCompletedDate = DateTime.UtcNow;
                    }
                    else if (!referralSparForm.IsSparFormCompleted)
                    {
                        referralSparForm.SparFormCompletedBy = null;
                        referralSparForm.SparFormCompletedDate = null;
                    }
                    response = SaveObject(referralSparForm, loggedInUserId, Resource.ReferralSparFormSaveSuccess);

                    referral.CASIIScore = referralSparForm.CASIIScore;
                    SaveEntity(referral);
                    response.IsSuccess = true;
                    response.Data = SetReferralSparForm(referral.ReferralID).Data;
                }
                else
                    response.Message = Resource.NoReferralFound;
            }
            else
            {
                response.Message = Resource.ReferralSparFormSaveError;
                response.IsSuccess = false;
            }
            return response;
        }

        #endregion ReferralSparForm Tab

        #region ReferralInternalMessage Tab

        public ServiceResponse SetReferralInternalMessage(long referralInternalMessageId, SearchReferralInternalMessage SearchReferralInternalMessage, long referralId, int pageIndex, int pageSize, string sortIndex, string sortDirection, bool isDelete)
        {
            ServiceResponse response = new ServiceResponse();
            if (referralId > 0)
            {
                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "EmployeeID", Value = Convert.ToString(SessionHelper.LoggedInID)},
                    new SearchValueData {Name = "ReferralID", Value = Convert.ToString(referralId)},
                    new SearchValueData {Name = "ReferralInternalMessageID", Value = Convert.ToString(referralInternalMessageId)},
                    new SearchValueData {Name = "Assignee", Value = Convert.ToString(SearchReferralInternalMessage.Assignee)},
                    new SearchValueData {Name = "IsDelete", Value = Convert.ToString(isDelete)}
                };
                searchlist.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

                List<ReferralInternalMessageModel> totalData = GetEntityList<ReferralInternalMessageModel>(StoredProcedure.SetReferralInternalMessage, searchlist);

                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<ReferralInternalMessageModel> setReferralSparFormModel = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);

                if (isDelete)
                    response.Message = Resource.DeleteNoteSuccess;
                response.IsSuccess = true;
                response.Data = setReferralSparFormModel;
            }
            else
                response.Message = Resource.GetReferralInternalMessageError;
            return response;
        }

        public ServiceResponse SaveReferralInternalMessage(ReferralInternalMessage referralInternalMessage, long loggedInUserId)
        {
            var response = new ServiceResponse();
            string message = referralInternalMessage.ReferralInternalMessageID > 0 ? Resource.UpdateInternalMessage : Resource.CreateInternalMessages;

            if (referralInternalMessage.ReferralID > 0)
            {
                Referral referral = GetEntity<Referral>(referralInternalMessage.ReferralID);
                if (referral.ReferralID > 0)
                {
                    response = SaveObject(referralInternalMessage, loggedInUserId, message);
                    response.IsSuccess = true;
                }
                else
                    response.Message = Resource.NoReferralFound;
            }
            else
                response.Message = Resource.SaveNoteError;
            return response;
        }

        public ServiceResponse ResolveReferralInternalMessage(long referralInternalMessageId, long referralId, long loggedInUserId)
        {
            var response = new ServiceResponse();
            if (referralInternalMessageId > 0 && referralId > 0)
            {
                string customWhere = string.Format("(ReferralInternalMessageID={0} and ReferralID={1})", referralInternalMessageId, referralId);
                ReferralInternalMessage referralInternalMessageDetail = GetEntity<ReferralInternalMessage>(null, customWhere);
                referralInternalMessageDetail.ResolveDate = DateTime.UtcNow;
                referralInternalMessageDetail.IsResolved = true;
                response = SaveObject(referralInternalMessageDetail, loggedInUserId, Resource.InternalMessageResolvedSuccess);

                //#region Create Note

                //INoteDataProvider iNoteDataProvider = new NoteDataProvider();
                //iNoteDataProvider.SaveGeneralNote(referralId, referralInternalMessageDetail.Note, Resource.InternalMessaging, loggedInUserId, null, Constants.Staff, null);

                //#endregion

                response.IsSuccess = true;
            }
            else
            {
                response.Message = Resource.ResolveNoteError;
            }
            return response;
        }

        #endregion ReferralInternalMessage Tab
        #region ReferralNotes
        public ServiceResponse HC_SaveReferralNotes(string RoleID, string EmployeesID, long referralId, long employeeID, string noteDetail, string catId, long loggedInUserID, long CommonNoteID, bool IsEdit = false, bool isPrivate = true)
        {
            ServiceResponse response = new ServiceResponse();
            if (referralId > 0 || employeeID > 0)
            {
                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "EmployeeID", Value = Convert.ToString(employeeID)},
                    new SearchValueData {Name = "ReferralID", Value = Convert.ToString(referralId)},
                    new SearchValueData {Name = "NoteDetail", Value = noteDetail},
                    new SearchValueData {Name = "catId", Value = Convert.ToString(catId)},
                    new SearchValueData {Name = "RoleID", Value = RoleID},
                    new SearchValueData {Name = "EmployeesID", Value = EmployeesID},
                    new SearchValueData {Name = "LoggedInUserID", Value = Convert.ToString(loggedInUserID)},
                    new SearchValueData {Name = "IsEdit", Value = Convert.ToString(IsEdit)},
                    new SearchValueData {Name = "isPrivate", Value = Convert.ToString(isPrivate)},
                };

                if (CommonNoteID > 0)
                {
                    searchlist.Add(new SearchValueData { Name = "CommonNoteID", Value = Convert.ToString(CommonNoteID) });
                }

                GetScalar(StoredProcedure.SaveCommonNote, searchlist);
                response.IsSuccess = true;
                response.Message = Resource.NoteSavedSuccessfully;
            }
            else
                response.Message = Resource.NoteFailed;
            return response;
        }




        public ServiceResponse GetReferralNotes(long referralId, long employeeId, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();
            if (referralId > 0 || employeeId > 0)
            {
                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralID", Value = Convert.ToString(referralId)},
                    new SearchValueData {Name = "EmployeeID", Value = Convert.ToString(employeeId)},
                    new SearchValueData {Name = "LoggedInUser", Value = Convert.ToString(loggedInUserID)},
                };
                List<ReferralNotesModel> totalData = GetEntityList<ReferralNotesModel>(StoredProcedure.GetCommonNotes, searchlist);
                response.IsSuccess = true;
                response.Data = totalData;
            }
            else
                response.Message = Resource.GetReferralInternalMessageError;
            return response;
        }

        public ServiceResponse GetReferralDetails(string ReferralId)
        {
            ServiceResponse response = new ServiceResponse();

            if (ReferralId != null)
            {
                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralId", Value = Convert.ToString(ReferralId)}
                };
                AddRecepient obj = GetMultipleEntity<AddRecepient>(StoredProcedure.GetReferralDocumentDetails, searchlist);
                response.IsSuccess = true;
                response.Data = obj;
            }
            return response;
        }

        public ServiceResponse GetTemplateList()
        {
            ServiceResponse response = new ServiceResponse();
            var searchlist = new List<SearchValueData>();
            List<EmailTemplateList> lst = GetEntityList<EmailTemplateList>(StoredProcedure.GetEmailTemplates, searchlist);
            response.IsSuccess = true;
            response.Data = lst;
            return response;
        }

        public ServiceResponse GetOrganizationSettings()
        {
            ServiceResponse response = new ServiceResponse();
            var searchlist = new List<SearchValueData>();
            List<OrganizationSettings> lst = GetEntityList<OrganizationSettings>(StoredProcedure.GetOrganizationSetting, searchlist);
            response.IsSuccess = true;
            response.Data = lst;
            return response;
        }


        public ServiceResponse GetSignature(long id)
        {
            ServiceResponse response = new ServiceResponse();
            var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "EmployeeID", Value = Convert.ToString(id)}
                };
            List<EmpSignature> lst = GetEntityList<EmpSignature>(StoredProcedure.GetEmployeeSignature, searchlist);
            response.IsSuccess = true;
            response.Message = lst[0].SignaturePath;
            return response;
        }

        public ServiceResponse GetTemplateDetails(string id, long ReferralID)
        {
            ServiceResponse response = new ServiceResponse();
            string StrContent = "";
            string signature = "";
            try
            {

                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "EmailTemplateTypeID", Value = Convert.ToString(id)},
                };
                DisplayEmailTemplate obj = GetEntity<DisplayEmailTemplate>(StoredProcedure.GetEmailTemplateDetails, searchlist);
                if (obj.EmailTemplateBody != null)
                {
                    var searchlist1 = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralID", Value = Convert.ToString(ReferralID)},
                };
                    ReferralDoucmnetAttachmentModel refmodel = GetEntity<ReferralDoucmnetAttachmentModel>(StoredProcedure.GetPatientAttacmentTags, searchlist1);
                    string readFile = obj.EmailTemplateBody;
                    StrContent = string.Empty;
                    StrContent = readFile;
                    if (StrContent.Contains("##PatientName##"))
                    {
                        StrContent = StrContent.Replace("##PatientName##", refmodel.PatientName);
                    }



                    if (StrContent.Contains("##DOB##"))
                    {
                        StrContent = StrContent.Replace("##DOB##", refmodel.DOB);
                    }
                    if (StrContent.Contains("##CaseManager##"))
                    {
                        StrContent = StrContent.Replace("##CaseManager##", refmodel.CaseManager);
                    }



                    if (StrContent.Contains("##Email##"))
                    {
                        StrContent = StrContent.Replace("##Email##", refmodel.Email);
                    }



                    if (StrContent.Contains("##Phone##"))
                    {
                        StrContent = StrContent.Replace("##Phone##", refmodel.Phone);
                    }



                    if (StrContent.Contains("##FullAddress##"))
                    {
                        StrContent = StrContent.Replace("##FullAddress##", refmodel.Address);
                    }
                    if (StrContent.Contains("##Assignee##"))
                    {
                        StrContent = StrContent.Replace("##Assignee##", refmodel.Assignee);
                    }
                    if (StrContent.Contains("##SiteName##"))
                    {
                        StrContent = StrContent.Replace("##SiteName##", _cacheHelper.SiteName);
                    }



                    if (StrContent.Contains("##Date##"))
                    {
                        StrContent = StrContent.Replace("##Date##", DateTime.Today.ToString(Constants.DbDateFormat));
                    }



                    if (!string.IsNullOrEmpty(StrContent))
                    {
                        try
                        {
                            var newsearchlist = new List<SearchValueData>
                            {
                                new SearchValueData {Name = "ReferralId", Value = Convert.ToString(SessionHelper.LoggedInID)},
                            };
                            DisplayEmailSignature totalData = GetEntity<DisplayEmailSignature>(StoredProcedure.GetEmailSignature, newsearchlist);
                            signature += totalData.EmailSignature;
                        }
                        catch
                        {
                            signature = "";
                        }

                    }



                    if (!string.IsNullOrEmpty(signature))
                    {
                        StrContent += signature;
                    }
                    obj.EmailTemplateBody = StrContent.ToString();
                    response.IsSuccess = true;
                    response.Data = obj;
                }
                return response;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return response;
        }



        public ServiceResponse GetReferralEmployee(string RoleId)
        {
            ServiceResponse response = new ServiceResponse();

            var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "RoleId", Value = Convert.ToString(RoleId)},
                };
            List<GetEmpListByRoleId> totalData = GetEntityList<GetEmpListByRoleId>(StoredProcedure.EZC_GetEmpListByRoleId, searchlist);
            response.IsSuccess = true;
            response.Data = totalData;

            response.Message = Resource.GetReferralInternalMessageError;
            return response;
        }

        public ServiceResponse GetEmployeeGroup(string name)
        {
            ServiceResponse response = new ServiceResponse();
            if (name.Length > 0)
            {
                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "Name", Value = Convert.ToString(name)},
                };

                List<CommonList> totalData = GetEntityList<CommonList>(StoredProcedure.GetCommonList, searchlist);


                response.IsSuccess = true;
                response.Data = totalData;
            }
            else
                response.Message = Resource.ExceptionMessage;
            return response;
        }




        public ServiceResponse GetCategory()
        {
            ServiceResponse response = new ServiceResponse();

            var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "id", Value = Convert.ToString("0")},
                };
            List<SelectValueModel> totalData = GetEntityList<SelectValueModel>(StoredProcedure.GetNoteCategory, searchlist);
            response.IsSuccess = true;
            response.Data = totalData;

            response.Message = Resource.GetReferralInternalMessageError;
            return response;
        }

        public string GetReferralDocumentPath(string id)
        {

            var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralDocumentId", Value = Convert.ToString(id)},
                };
            var path = GetEntityList<RefDocument>(StoredProcedure.GetReferralDocument, searchlist);
            string originalPath = path[0].FilePath;
            return originalPath;
        }

        public ServiceResponse GetReferralDocumentDetails(string id)
        {
            ServiceResponse response = new ServiceResponse();
            var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralDocumentId", Value = Convert.ToString(id)},
                };
            List<RefDocument> refDoc = GetEntityList<RefDocument>(StoredProcedure.GetReferralDocument, searchlist);
            response.IsSuccess = true;
            response.Data = refDoc.FirstOrDefault();
            return response;
        }

        public ServiceResponse SendReferralAttachment(MailModel model)
        {
            long v = SessionHelper.ReferraID;
            ServiceResponse response = new ServiceResponse();
            bool isSentMail = false;
            if (model != null)
            {

                isSentMail = Common.SendEmail(model.Subject, model.From, model.To, model.Body, EnumEmailType.HomeCare_Schedule_Registration_Notification.ToString(), model.CC, 1, model.Attachment);

            }
            response.IsSuccess = isSentMail;
            return response;
        }

        public ServiceResponse DeleteReferralNote(long CommonNoteID, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();
            if (CommonNoteID > 0)
            {
                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "CommonNoteID", Value = Convert.ToString(CommonNoteID)},
                    new SearchValueData {Name = "LoggedInUserID", Value = Convert.ToString(loggedInUserID)},
                };


                GetScalar(StoredProcedure.DeleteCommonNote, searchlist);
                response.IsSuccess = true;
            }

            if (!response.IsSuccess)
                response.Message = Resource.ExceptionMessage;

            return response;
        }

        #endregion ReferralNotes
        #region Referral Documents

        #region Documents

        public ServiceResponse SetReferralDocument(long referralId)
        {
            var response = new ServiceResponse();
            if (referralId > 0)
            {
                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralID", Value = Convert.ToString(referralId)}
                };

                response.IsSuccess = true;
            }
            else
                response.Message = Resource.GetReferralDocumentError;
            return response;
        }

        public ServiceResponse SaveFile(string file, string fileName, long referralID, long loggedInID)
        {
            ServiceResponse response = new ServiceResponse();

            if (!string.IsNullOrEmpty(file))
            {

                //ReferralDocument referralDocument = new ReferralDocument
                //{
                //    FileName = fileName,
                //    FilePath = file,
                //    KindOfDocument = Convert.ToString(DocumentType.DocumentKind.Internal),
                //    DocumentTypeID = (int)DocumentType.DocumentTypes.Other,
                //    ReferralID = referralID
                //};
                //SaveObject(referralDocument, loggedInID);

                List<SearchValueData> searchList = new List<SearchValueData>()
                    {
                        new SearchValueData {Name = "FileName", Value = fileName},
                        new SearchValueData {Name = "FilePath", Value = file},
                        new SearchValueData {Name = "ReferralID ", Value = referralID.ToString() },
                        new SearchValueData {Name = "LoggedInUserID", Value = loggedInID.ToString()},
                        new SearchValueData {Name = "SystemID ", Value = HttpContext.Current.Request.UserHostAddress },
                    };

                GetScalar(StoredProcedure.SaveReferralDocument, searchList);

                response.Data = GetReferralDocumentList(referralID, 1, 50, "ReferralDocumentID", "DESC").Data;
                response.IsSuccess = true;
                return response;
            }
            response.Message = Resource.FileUploadFailedNoFileSelected;
            return response;
        }






        public ServiceResponse GetReferralDocumentList(long referralID, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchModel> searchList = new List<SearchModel>();
            searchList.Add(new SearchModel { Name = "ReferralID", Value = referralID.ToString() });
            var customFIlter = "ReferralID =" + referralID;
            response = GetPageRecords<ReferralDocumentList>(pageSize, pageIndex, sortIndex, sortDirection, null, customFIlter);

            response.IsSuccess = true;

            return response;
        }

        public ServiceResponse DeleteDocument(long referralDocumentID, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();
            var UserType = (int)ReferralDocument.UserTypes.Referral;
            if (referralDocumentID > 0)
            {
                ReferralDocument referralDocument = GetEntity<ReferralDocument>(referralDocumentID);
                if (referralDocument != null)
                {
                    response = Common.DeleteFile(referralDocument.FilePath);
                    if (referralDocument.DocumentTypeID == (int)DocumentType.DocumentTypes.Other)
                    {
                        DeleteEntity<ReferralDocument>(referralDocument.ReferralDocumentID);

                    }
                    else
                    {
                        DeleteEntity<ReferralDocument>(referralDocument.ReferralDocumentID);
                        List<SearchValueData> searchParam = new List<SearchValueData>
                            {
                                new SearchValueData {Name = "DocumentTypeID",Value = referralDocument.DocumentTypeID.ToString(),IsEqual = true},
                                new SearchValueData {Name = "UserID",Value = referralDocument.UserID.ToString(),IsEqual = true},
                                new SearchValueData {Name = "UserType",Value = UserType.ToString(),IsEqual = true}
                            };
                        int existingReferralDocumentCount = GetEntityCount<ReferralDocument>(searchParam);
                        if (existingReferralDocumentCount == 0)
                        {
                            Referral referral = GetEntity<Referral>(referralDocument.UserID);
                            List<SearchValueData> referralCheckListSearchParam = new List<SearchValueData>
                                {
                                    new SearchValueData{Name = "ReferralID",Value = referralDocument.UserID.ToString(),IsEqual = true}
                                };
                            ReferralCheckList referralCheckList = GetEntity<ReferralCheckList>(referralCheckListSearchParam);
                            switch (referralDocument.DocumentTypeID)
                            {
                                case (int)DocumentType.DocumentTypes.Admission_Orientation:
                                    referral.AdmissionOrientation = false;
                                    break;
                                case (int)DocumentType.DocumentTypes.Admission_Requirements:
                                    referral.AdmissionRequirements = false;
                                    break; /* optional */
                                case (int)DocumentType.DocumentTypes.Agency_Network_Service_Plan:
                                    referral.NetworkServicePlan = false;
                                    referral.NSPExpirationDate = null;
                                    referral.NSPGuardianSignature = false;
                                    referral.NSPBHPSigned = false;
                                    referral.NSPSPidentifyService = Constants.N;
                                    break; /* optional */
                                case (int)DocumentType.DocumentTypes.Agency_ROI:
                                    referral.AROI = Constants.N;
                                    referral.AROIAgencyID = null;
                                    //referral.AROIName = null;
                                    //referral.AROIRelationship = null;
                                    //referral.AROIPhone = null;
                                    referral.AROIExpirationDate = null;
                                    break; /* optional */
                                case (int)DocumentType.DocumentTypes.Ansell_Casey_Assessment:
                                    referral.ACAssessment = false;
                                    referral.ACAssessmentExpirationDate = null;
                                    break; /* optional */
                                case (int)DocumentType.DocumentTypes.BX_Assessment:
                                    referral.BXAssessment = false;
                                    referral.BXAssessmentExpirationDate = null;
                                    referral.BXAssessmentBHPSigned = false;
                                    break; /* optional */
                                case (int)DocumentType.DocumentTypes.CAZ_Only_Referral_Checklist:
                                    //TODO set value here
                                    if (referralCheckList != null)
                                    {
                                        referralCheckList.ReferralChecklist = false;
                                        referralCheckList.ReferralChecklistText = null;
                                    }
                                    break; /* optional */
                                case (int)DocumentType.DocumentTypes.Care_Consent:
                                    referral.CareConsent = false;
                                    break; /* optional */
                                case (int)DocumentType.DocumentTypes.Demographic:
                                    referral.Demographic = Constants.N;
                                    referral.DemographicExpirationDate = null;
                                    break; /* optional */
                                case (int)DocumentType.DocumentTypes.Health_Information_Disclosure:
                                    referral.HealthInformationDisclosure = false;
                                    break; /* optional */
                                case (int)DocumentType.DocumentTypes.Network_Crisis_Plan:
                                    referral.NetworkCrisisPlan = Constants.N;
                                    break; /* optional */
                                case (int)DocumentType.DocumentTypes.PHI_Referring_Agency:
                                    referral.PHI = false;
                                    referral.PHIExpirationDate = null;
                                    referral.PHIAgencyID = null;
                                    break; /* optional */
                                case (int)DocumentType.DocumentTypes.SNCD:
                                    referral.SNCD = Constants.N;
                                    referral.SNCDExpirationDate = null;
                                    break; /* optional */
                                case (int)DocumentType.DocumentTypes.Self_Administration_of_Medication:
                                    referral.SelfAdministrationofMedication = false;
                                    break; /* optional */
                                case (int)DocumentType.DocumentTypes.Zarephath_Crisis_Plan:
                                    referral.ZarephathCrisisPlan = Constants.N;
                                    break; /* optional */
                                case (int)DocumentType.DocumentTypes.Zarephath_Service_Plan_Counseling:
                                    referral.ZSPCounselling = false;
                                    referral.ZSPCounsellingBHPSigned = false;
                                    referral.ZSPCounsellingExpirationDate = null;
                                    referral.ZSPCounsellingGuardianSignature = false;
                                    break; /* optional */
                                case (int)DocumentType.DocumentTypes.Zarephath_Service_Plan_Life_Skills:
                                    referral.ZSPLifeSkills = false;
                                    referral.ZSPLifeSkillsBHPSigned = false;
                                    referral.ZSPLifeSkillsExpirationDate = null;
                                    referral.ZSPLifeSkillsGuardianSignature = false;
                                    break; /* optional */
                                case (int)DocumentType.DocumentTypes.Zarephath_Service_Plan_Respite:
                                    referral.ZSPRespite = false;
                                    referral.ZSPRespiteExpirationDate = null;
                                    referral.ZSPRespiteBHPSigned = false;
                                    referral.ZSPRespiteGuardianSignature = false;
                                    break; /* optional */
                            }

                            if (referralCheckList != null)
                            {
                                SaveObject(referralCheckList, loggedInUserID);
                            }
                            //SaveObject(referral, loggedInUserID);
                        }
                    }
                    response.IsSuccess = true;
                    response.Data = GetReferralDocumentList(referralDocument.UserID, pageIndex, pageSize, sortIndex,
                                                            sortDirection).Data;
                    response.Message = Resource.DocumentDeleted;
                }
                else
                {
                    response.Message = Resource.ExceptionMessage;
                }
            }
            else
            {
                response.Message = Resource.ExceptionMessage;
            }

            return response;
        }

        public ServiceResponse SaveDocument(ReferralDocument referralDoc, int pageIndex, int pageSize, string sortIndex,
                                            string sortDirection, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();
            var UserType = (int)ReferralDocument.UserTypes.Referral;
            if (referralDoc.ReferralDocumentID > 0)
            {
                //bool IsExists = false;
                ReferralDocument referralDocument = GetEntity<ReferralDocument>(referralDoc.ReferralDocumentID);
                var oldDocumentTypeID = referralDocument.DocumentTypeID;
                Referral referral = GetEntity<Referral>(referralDocument.UserID);
                List<SearchValueData> referralCheckListSearchParam = new List<SearchValueData>
                    {
                        new SearchValueData {Name = "ReferralID",Value = referralDocument.UserID.ToString(),IsEqual = true}
                    };
                ReferralCheckList referralCheckList = GetEntity<ReferralCheckList>(referralCheckListSearchParam);
                //if (referralDoc.DocumentTypeID != (int)DocumentType.DocumentTypes.Other)
                //{
                //    List<SearchValueData> searchParam = new List<SearchValueData>
                //    {
                //        new SearchValueData {Name = "DocumentTypeID",Value = referralDoc.DocumentTypeID.ToString(),IsEqual = true},
                //        new SearchValueData {Name = "ReferralDocumentID",Value = referralDoc.ReferralDocumentID.ToString(),IsNotEqual = true}
                //    };
                //    ReferralDocument existingReferralDocument = GetEntity<ReferralDocument>(searchParam);
                //    if (existingReferralDocument != null)
                //    {
                //        IsExists = true;
                //    }
                //}
                //if (!IsExists)
                //{
                referralDocument.FileName = referralDoc.FileName;
                referralDocument.KindOfDocument = referralDoc.KindOfDocument;
                referralDocument.DocumentTypeID = referralDoc.DocumentTypeID;
                SaveObject(referralDocument, loggedInUserID);

                if (referralDoc.DocumentTypeID != (int)DocumentType.DocumentTypes.Other)

                    if (true)
                    {
                        List<SearchValueData> searchParam = new List<SearchValueData>
                            {
                                new SearchValueData {Name = "DocumentTypeID",Value = oldDocumentTypeID.ToString(),IsEqual = true},
                                new SearchValueData {Name = "ReferralDocumentID",Value = referralDoc.ReferralDocumentID.ToString(),IsNotEqual = true},
                                new SearchValueData {Name = "UserID",Value = referralDoc.UserID.ToString(),IsEqual = true},
                                new SearchValueData {Name = "UserType",Value = UserType.ToString(),IsEqual = true}
                            };
                        int existingReferralDocumentCount = GetEntityCount<ReferralDocument>(searchParam);
                        if (existingReferralDocumentCount == 0)
                        {
                            // To remove old document referral type from referral table
                            #region Switch case To remove old document referral type from referral table
                            switch (oldDocumentTypeID) //TODO: NO LONGER NEEDED SO REMOVING THIS
                            {
                                case (int)DocumentType.DocumentTypes.Admission_Orientation:
                                    referral.AdmissionOrientation = false;
                                    break;
                                case (int)DocumentType.DocumentTypes.Admission_Requirements:
                                    referral.AdmissionRequirements = false;
                                    break; /* optional */
                                case (int)DocumentType.DocumentTypes.Agency_Network_Service_Plan:
                                    referral.NetworkServicePlan = false;
                                    //referral.NSPExpirationDate = null;
                                    //referral.NSPGuardianSignature = false;
                                    //referral.NSPBHPSigned = false;
                                    //referral.NSPSPidentifyService = false;
                                    break; /* optional */
                                case (int)DocumentType.DocumentTypes.Agency_ROI:
                                    referral.AROI = Constants.N;
                                    //referral.AROIAgencyID = null;
                                    //referral.AROIExpirationDate = null;
                                    break; /* optional */
                                case (int)DocumentType.DocumentTypes.Ansell_Casey_Assessment:
                                    referral.ACAssessment = false;
                                    //referral.ACAssessmentExpirationDate = null;
                                    break; /* optional */
                                case (int)DocumentType.DocumentTypes.BX_Assessment:
                                    referral.BXAssessment = false;
                                    //referral.BXAssessmentExpirationDate = null;
                                    //referral.BXAssessmentBHPSigned = false;
                                    break; /* optional */
                                case (int)DocumentType.DocumentTypes.CAZ_Only_Referral_Checklist:
                                    if (referralCheckList != null)
                                    {
                                        referralCheckList.ReferralChecklist = false;
                                        //referralCheckList.ReferralChecklistText = null;
                                    }
                                    break; /* optional */
                                case (int)DocumentType.DocumentTypes.Care_Consent:
                                    referral.CareConsent = false;
                                    break; /* optional */
                                case (int)DocumentType.DocumentTypes.Demographic:
                                    referral.Demographic = Constants.N;
                                    //referral.DemographicExpirationDate = null;
                                    break; /* optional */
                                case (int)DocumentType.DocumentTypes.Health_Information_Disclosure:
                                    referral.HealthInformationDisclosure = false;
                                    break; /* optional */
                                case (int)DocumentType.DocumentTypes.Network_Crisis_Plan:
                                    referral.NetworkCrisisPlan = Constants.N;
                                    break; /* optional */
                                case (int)DocumentType.DocumentTypes.PHI_Referring_Agency:
                                    referral.PHI = false;
                                    //referral.PHIExpirationDate = null;
                                    //referral.PHIAgencyID = null;
                                    break; /* optional */
                                case (int)DocumentType.DocumentTypes.SNCD:
                                    referral.SNCD = Constants.N;
                                    referral.SNCDExpirationDate = null;
                                    break; /* optional */
                                case (int)DocumentType.DocumentTypes.Self_Administration_of_Medication:
                                    referral.SelfAdministrationofMedication = false;
                                    break; /* optional */
                                case (int)DocumentType.DocumentTypes.Zarephath_Crisis_Plan:
                                    referral.ZarephathCrisisPlan = Constants.N;
                                    break; /* optional */
                                case (int)DocumentType.DocumentTypes.Zarephath_Service_Plan_Counseling:
                                    referral.ZSPCounselling = false;
                                    //referral.ZSPCounsellingBHPSigned = false;
                                    //referral.ZSPCounsellingExpirationDate = null;
                                    //referral.ZSPCounsellingGuardianSignature = false;
                                    break; /* optional */
                                case (int)DocumentType.DocumentTypes.Zarephath_Service_Plan_Life_Skills:
                                    referral.ZSPLifeSkills = false;
                                    //referral.ZSPLifeSkillsBHPSigned = false;
                                    //referral.ZSPLifeSkillsExpirationDate = null;
                                    //referral.ZSPLifeSkillsGuardianSignature = false;
                                    break; /* optional */
                                case (int)DocumentType.DocumentTypes.Zarephath_Service_Plan_Respite:
                                    referral.ZSPRespite = false;
                                    //referral.ZSPRespiteExpirationDate = null;
                                    //referral.ZSPRespiteBHPSigned = false;
                                    //referral.ZSPRespiteGuardianSignature = false;
                                    break; /* optional */
                                case (int)DocumentType.DocumentTypes.SZarephath_Service_Plan_Connecting_Families:
                                    referral.ZSPConnectingFamilies = false;
                                    //referral.ZSPConnectingFamiliesBHPSigned = false;
                                    //referral.ZSPConnectingFamiliesExpirationDate = null;
                                    //referral.ZSPConnectingFamiliesGuardianSignature = false;
                                    break; /* optional */
                            }
                            #endregion

                        }
                        // To Update new document referral type from referral table
                        #region Switch Case To Update new document referral type from referral table
                        switch (referralDocument.DocumentTypeID)
                        {
                            case (int)DocumentType.DocumentTypes.Admission_Orientation:
                                referral.AdmissionOrientation = true;
                                break;
                            case (int)DocumentType.DocumentTypes.Admission_Requirements:
                                referral.AdmissionRequirements = true;
                                break; /* optional */
                            case (int)DocumentType.DocumentTypes.Agency_Network_Service_Plan:
                                referral.NetworkServicePlan = true;
                                break; /* optional */
                            case (int)DocumentType.DocumentTypes.Agency_ROI:
                                referral.AROI = Constants.Y;
                                break; /* optional */
                            case (int)DocumentType.DocumentTypes.Ansell_Casey_Assessment:
                                referral.ACAssessment = true;
                                break; /* optional */
                            case (int)DocumentType.DocumentTypes.BX_Assessment:
                                referral.BXAssessment = true;
                                //TODO Update dx codes
                                break; /* optional */
                            case (int)DocumentType.DocumentTypes.CAZ_Only_Referral_Checklist:
                                if (referralCheckList != null)
                                {
                                    referralCheckList.ReferralChecklist = true;
                                }
                                break; /* optional */
                            case (int)DocumentType.DocumentTypes.Care_Consent:
                                referral.CareConsent = true;
                                break; /* optional */
                            case (int)DocumentType.DocumentTypes.Demographic:
                                referral.Demographic = Constants.Y;
                                break; /* optional */
                            case (int)DocumentType.DocumentTypes.Health_Information_Disclosure:
                                referral.HealthInformationDisclosure = true;
                                break; /* optional */
                            case (int)DocumentType.DocumentTypes.Network_Crisis_Plan:
                                referral.NetworkCrisisPlan = Constants.Y;
                                break; /* optional */
                            case (int)DocumentType.DocumentTypes.PHI_Referring_Agency:
                                referral.PHI = true;
                                break; /* optional */
                            case (int)DocumentType.DocumentTypes.SNCD:
                                referral.SNCD = Constants.Y;
                                break; /* optional */
                            case (int)DocumentType.DocumentTypes.Self_Administration_of_Medication:
                                referral.SelfAdministrationofMedication = true;
                                break; /* optional */
                            case (int)DocumentType.DocumentTypes.Zarephath_Crisis_Plan:
                                referral.ZarephathCrisisPlan = Constants.Y;
                                break; /* optional */
                            case (int)DocumentType.DocumentTypes.Zarephath_Service_Plan_Counseling:
                                referral.ZSPCounselling = true;
                                break; /* optional */
                            case (int)DocumentType.DocumentTypes.Zarephath_Service_Plan_Life_Skills:
                                referral.ZSPLifeSkills = true;
                                break; /* optional */
                            case (int)DocumentType.DocumentTypes.Zarephath_Service_Plan_Respite:
                                referral.ZSPRespite = true;
                                break; /* optional */
                            case (int)DocumentType.DocumentTypes.SZarephath_Service_Plan_Connecting_Families:
                                referral.ZSPConnectingFamilies = true;
                                break; /* optional */
                        }
                        #endregion
                    }
                if (referralCheckList != null)
                    SaveObject(referralCheckList, loggedInUserID);
                //SaveObject(referral, loggedInUserID);
                // Commenting This Referral Save, As Client asked to remove Docuemnt + Referral linking 
                response.IsSuccess = true;
                response.Message = Resource.DocumentUpdated;
                response.Data =
                    GetReferralDocumentList(referral.ReferralID, pageIndex, pageSize, sortIndex, sortDirection).Data;
            }
            else
            {
                response.Message = Resource.DocumentTypeExists;
            }
            //}
            //else
            //{
            //    response.Message = Resource.ExceptionMessage;
            //}

            return response;
        }

        #endregion Documents

        #region Missing Documents

        public ServiceResponse SetReferralMissingDocument(long referralId)
        {
            var response = new ServiceResponse();
            if (referralId > 0)
            {
                var searchlist = new List<SearchValueData>
                {

                    new SearchValueData {Name = "ReferralID", Value = Convert.ToString(referralId)},
                    new SearchValueData {Name = "EmailTemplateTypeID", Value = Convert.ToInt16(EnumEmailType.Missing_Expire_Document_Template).ToString()},
                    new SearchValueData {Name = "AgencyROI", Value = Convert.ToString(Constants.AgencyROI)},
                    new SearchValueData {Name = "NetworkServicePlan", Value = Convert.ToString(Constants.NetworkServicePlan)},
                    new SearchValueData {Name = "BXAssessment", Value = Convert.ToString(Constants.BXAssessment)},
                    new SearchValueData {Name = "Demographic", Value = Convert.ToString(Constants.Demographic)},
                    new SearchValueData {Name = "SNCD", Value = Convert.ToString(Constants.SNCD)},
                    new SearchValueData {Name = "NetworkCrisisPlan", Value = Convert.ToString(Constants.NetworkCrisisPlan)},
                    new SearchValueData {Name = "CAZOnly", Value = Convert.ToString(Constants.CAZOnly)},
                    new SearchValueData {Name = "Missing", Value = Convert.ToString(Constants.Missing)},
                    new SearchValueData {Name = "Expired", Value = Convert.ToString(Constants.Expired)}
                 };

                ReferralMissingDocumentModel referralMissingDocumentModel = GetMultipleEntity<ReferralMissingDocumentModel>(StoredProcedure.SetReferralMissingDocument, searchlist);

                #region ScrapCode
                //string htmlString = referralMissingDocumentModel.MissingDocumentList.Count > 0
                //                        ? referralMissingDocumentModel.MissingDocumentList.OrderByDescending(x => x.MissingDocumentType).Aggregate
                //                              ("<b>Client Name:&nbsp;</b>" + referralMissingDocumentModel.SetMissingDocumentTokenModel.ClientName + " " + referralMissingDocumentModel.SetMissingDocumentTokenModel.DateofBirth + referralMissingDocumentModel.SetMissingDocumentTokenModel.AHCCCSID + " " +
                //                               "<br><ul>", (current, missingDocument) => current + string.Format
                //                     ("<li>{0}:&nbsp;{1}</li>", missingDocument.MissingDocumentType, missingDocument.MissingDocumentName))
                //                     : string.Format("<ul><li>{0}</li>", Resource.NoDocumentMissing);
                //htmlString += "</ul>";
                #endregion

                referralMissingDocumentModel.SetMissingDocumentTokenModel.CaseManager =
                    referralMissingDocumentModel.SetMissingDocumentTokenModel.CaseManagerFirstName + " " +
                    referralMissingDocumentModel.SetMissingDocumentTokenModel.CaseManagerLastName;

                string htmlString = "<ul>";

                string clientWithDocDtlli = "<li><b>{0} (#{2}, DOB : {1})</b><br/> {3}</li>";

                string str = "";
                if (referralMissingDocumentModel.MissingDocumentList.Any(m => m.MissingDocumentType == Constants.Missing))
                {
                    str = string.Format("<b>{0}</b> : {1}<br>", Constants.Missing,
                                        string.Join(", ",
                                                    referralMissingDocumentModel.MissingDocumentList.Where(m => m.MissingDocumentType == Constants.Missing)
                                                                                .Select(m => m.MissingDocumentName)));
                }
                if (referralMissingDocumentModel.MissingDocumentList.Any(m => m.MissingDocumentType == Constants.Expired))
                {
                    str += string.Format("<b>{0}</b> : {1}<br>", Constants.Expired,
                                         string.Join(", ", referralMissingDocumentModel.MissingDocumentList.Where(m => m.MissingDocumentType == Constants.Expired).Select(m => m.MissingDocumentName)));
                }
                htmlString += string.Format(clientWithDocDtlli, referralMissingDocumentModel.SetMissingDocumentTokenModel.ClientName,
                                     referralMissingDocumentModel.SetMissingDocumentTokenModel.DateofBirth, referralMissingDocumentModel.SetMissingDocumentTokenModel.AHCCCSID, str);

                htmlString += "</ul>";

                referralMissingDocumentModel.SetMissingDocumentTokenModel.ClientList = htmlString;

                if (string.IsNullOrEmpty(referralMissingDocumentModel.SetMissingDocumentTokenModel.ToEmail))
                {
                    referralMissingDocumentModel.SetMissingDocumentTokenModel.ToEmail = referralMissingDocumentModel.SetMissingDocumentTokenModel.Email;
                    referralMissingDocumentModel.SetMissingDocumentTokenModel.CaseManagerFirstName = Resource.CASEMANGERLabel + referralMissingDocumentModel.SetMissingDocumentTokenModel.CaseManagerFirstName;
                }
                else
                {
                    referralMissingDocumentModel.SetMissingDocumentTokenModel.CaseManagerFirstName = Resource.RecordsDepartment;
                }

                if (!string.IsNullOrEmpty(referralMissingDocumentModel.SetMissingDocumentTokenModel.ClientNickName))
                {
                    referralMissingDocumentModel.SetMissingDocumentTokenModel.ClientName = referralMissingDocumentModel.SetMissingDocumentTokenModel.ClientName +
                        "(" + referralMissingDocumentModel.SetMissingDocumentTokenModel.ClientNickName + ")";
                }

                //referralMissingDocumentModel.SetMissingDocumentTokenModel. = ConfigSettings.SiteBaseURL + Constants.ZerpathLogoImage;
                referralMissingDocumentModel.SetMissingDocumentTokenModel.MissingItems = htmlString;

                var missingDocumentModel = new MissingDocumentModel
                {
                    ToEmail = referralMissingDocumentModel.SetMissingDocumentTokenModel.ToEmail,
                    Subject = referralMissingDocumentModel.EmailTemplate.EmailTemplateSubject,
                    Body = TokenReplace.ReplaceTokens(referralMissingDocumentModel.EmailTemplate.EmailTemplateBody, referralMissingDocumentModel.SetMissingDocumentTokenModel)
                };

                //referralMissingDocumentModel.MissingDocumentModel.Body = TokenReplace.ReplaceTokens(referralMissingDocumentModel.EmailTemplate.EmailTemplateBody, referralMissingDocumentModel.MissingDocumentModel);
                //referralMissingDocumentModel.MissingDocumentModel.Subject = referralMissingDocumentModel.EmailTemplate.EmailTemplateSubject;

                response.Data = missingDocumentModel;
                response.IsSuccess = true;
            }
            else
                response.Message = Resource.GetReferralDocumentError;
            return response;
        }

        public ServiceResponse SendEmailForReferralMissingDocument(MissingDocumentModel missingDocumentModel, long referralId, long loggedInUserID)
        {
            var response = new ServiceResponse();
            if (referralId > 0)
            {
                bool isSentMail = Common.SendEmail(missingDocumentModel.Subject, _cacheHelper.SupportEmail, missingDocumentModel.ToEmail, missingDocumentModel.Body, null, ConfigSettings.CCEmailAddress, (int)EmailHelper.SMTPSetting.EncryptedEmailSetting);
                response.IsSuccess = isSentMail;
                response.Message = isSentMail ? Resource.EmailSentSuccess : Resource.EmailSentFail;
                if (isSentMail)
                {
                    INoteDataProvider iNoteDataProvider = new NoteDataProvider();
                    iNoteDataProvider.SaveGeneralNote(referralId, missingDocumentModel.Body, Resource.WebsiteMissingDocumentEmail,
                                                      loggedInUserID, missingDocumentModel.ToEmail,
                                                      Constants.Case_Manager, Resource.Email);
                }
            }
            else
                response.Message = Resource.NoReferralFound;
            return response;
        }

        #endregion Missing Documents

        #endregion Referral Documents

        #region Delete Referral Sibling

        public ServiceResponse ReferralSiblingMappingDelete(long referralSiblingMappingId)
        {
            ServiceResponse response = new ServiceResponse();

            var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralSiblingMappingID", Value = Convert.ToString(referralSiblingMappingId)}
                };
            List<ReferralSiblingMappingsList> referralSiblingMappingsList = GetEntityList<ReferralSiblingMappingsList>(StoredProcedure.DeleteReferralSibling, searchlist);

            response.Data = referralSiblingMappingsList;
            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordDeletedSuccessfully, Resource.ReferralSiblingMappings);
            return response;
        }

        #endregion

        #endregion Add Referral

        #region Referral List

        public ServiceResponse SendReceiptNotificationEmail(long referralId)
        {
            var response = new ServiceResponse();
            if (referralId > 0)
            {
                var searchList = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralID", Value = Convert.ToString(referralId)},
                    //new SearchValueData {Name = "EmailTemplateType", Value = Convert.ToString(Constants.ReceiptNotificationMail)},
                      new SearchValueData {Name = "EmailTemplateTypeID", Value = Convert.ToInt16(EnumEmailType.ReceiptNotificationMail).ToString()},
                };

                SendReceiptNotificationEmail sendReceiptNotificationEmail = GetMultipleEntity<SendReceiptNotificationEmail>(StoredProcedure.SendReceiptNotificationEmail, searchList);

                sendReceiptNotificationEmail.ReceiptNotificationModel.ZerpathLogoImage = _cacheHelper.SiteBaseURL + Constants.ZerpathLogoImage;

                if (!string.IsNullOrEmpty(sendReceiptNotificationEmail.ReceiptNotificationModel.ClientNickName))
                {
                    sendReceiptNotificationEmail.ReceiptNotificationModel.ClientName =
                        sendReceiptNotificationEmail.ReceiptNotificationModel.ClientName + "(" + sendReceiptNotificationEmail.ReceiptNotificationModel.ClientNickName + ")";
                }

                bool isSentMail = false;
                string body = null;
                if (sendReceiptNotificationEmail.ReceiptNotificationModel.ToEmail != null)
                {
                    body = TokenReplace.ReplaceTokens(sendReceiptNotificationEmail.EmailTemplate.EmailTemplateBody, sendReceiptNotificationEmail.ReceiptNotificationModel);
                    isSentMail = Common.SendEmail(sendReceiptNotificationEmail.EmailTemplate.EmailTemplateSubject, _cacheHelper.SupportEmail, sendReceiptNotificationEmail.ReceiptNotificationModel.ToEmail, body, null, ConfigSettings.CCEmailAddress, (int)EmailHelper.SMTPSetting.EncryptedEmailSetting);
                    response.IsSuccess = isSentMail;
                    response.Message = isSentMail ? Resource.EmailSentSuccess : Resource.EmailSentFail;
                }
                else
                {
                    response.Message = Resource.Casemanageremailismissing;
                }
                if (isSentMail)
                {
                    var referral = GetEntity<Referral>(referralId);
                    referral.NotifyCaseManager = true;
                    if (!referral.NotifyCaseManagerDate.HasValue)
                    {
                        referral.NotifyCaseManagerDate = DateTime.Now.Date;
                    }
                    SaveEntity(referral);
                    response.Data = referral;

                    INoteDataProvider iNoteDataProvider = new NoteDataProvider();
                    iNoteDataProvider.SaveGeneralNote(referralId, body, Resource.EmailReferralNotification, SessionHelper.LoggedInID,
                         sendReceiptNotificationEmail.ReceiptNotificationModel.CaseManager +
                         " (" + sendReceiptNotificationEmail.ReceiptNotificationModel.ToEmail + ")", Resource.CaseManager, Resource.Email);
                }
            }
            else
                response.Message = Resource.NoReferralFound;
            return response;
        }

        public ServiceResponse SetReferralListPage(long loggedInID)
        {
            ServiceResponse response = new ServiceResponse();

            SetReferralListModel setReferralListModel = GetMultipleEntity<SetReferralListModel>(StoredProcedure.SetReferralListPage);
            setReferralListModel.NotifyCaseManager = Common.SetYesNoAllList();
            setReferralListModel.Checklist = Common.SetYesNoAllList();
            setReferralListModel.ClinicalReview = Common.SetYesNoAllList();
            setReferralListModel.Services = Common.SetServicesFilter();
            setReferralListModel.Draft = Common.SetDraftFilter();
            setReferralListModel.DeleteFilter = Common.SetDeleteFilter();
            setReferralListModel.SearchReferralListModel.IsDeleted = 0;
            setReferralListModel.SearchReferralListModel.ServiceID = -1;
            setReferralListModel.SearchReferralListModel.ChecklistID = -1;
            setReferralListModel.SearchReferralListModel.ClinicalReviewID = -1;
            setReferralListModel.SearchReferralListModel.NotifyCaseManagerID = -1;
            setReferralListModel.SearchReferralListModel.IsSaveAsDraft = -1;
            // setReferralListModel.SearchReferralListModel.ReferralStatusID = -1;//(int)ReferralStatus.ReferralStatuses.Active;
            if (Common.HasPermission(Constants.Permission_View_Assinged_Referral) &&
                !Common.HasPermission(Constants.Permission_View_All_Referral))
            {
                setReferralListModel.SearchReferralListModel.AssigneeID = loggedInID;
            }
            response.Data = setReferralListModel;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse GetReferralList(SearchReferralListModel searchReferralModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            List<ReferralList> totalData = new List<ReferralList>();
            if (searchReferralModel != null)
                SetSearchFilterForReferralListPage(searchReferralModel, searchList, loggedInId, true);

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            totalData = GetEntityList<ReferralList>(StoredProcedure.GetReferralList, searchList);


            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ReferralList> getReferralList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);

            response.Data = getReferralList;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse GetReferralsForNurseSchedule(SearchReferralListModel searchReferralModel, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchReferralModel != null)
                SetSearchFilterForReferralListPage(searchReferralModel, searchList, 0, true);

            searchList.Add(new SearchValueData { Name = "SortExpression", Value = Convert.ToString(sortIndex) });
            searchList.Add(new SearchValueData { Name = "SortType", Value = Convert.ToString(sortDirection) });
            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = "0" });

            List<ReferralList> totalData = new List<ReferralList>();
            //searchList.Add(new SearchValueData { Name = "EmployeeId", Value = Convert.ToString(SessionHelper.LoggedInID) });

            totalData = GetEntityList<ReferralList>(StoredProcedure.GetReferralLimitedRecordsForNurseSchedule, searchList);

            response.Data = totalData;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse GetReferralAuthorizationsDetails(string referralIDs)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ReferralIDs", Value = referralIDs });

            List<ReferralBillingAuthorizationList> responseData = GetEntityList<ReferralBillingAuthorizationList>(StoredProcedure.GetReferralAuthorizationsDetails, searchList);

            response.Data = responseData;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse GetReferralDetails(Referral referral)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ReferralID", Value = referral.ReferralID.ToString() });

            ReferralDetailsModel responseData = GetMultipleEntity<ReferralDetailsModel>(StoredProcedure.GetReferralDetails, searchList);

            response.Data = responseData;
            response.IsSuccess = true;
            return response;
        }

        private static void SetSearchFilterForReferralListPage(SearchReferralListModel searchReferralModel, List<SearchValueData> searchList, long loggedInId, bool isList)
        {
            if (!string.IsNullOrEmpty(searchReferralModel.ClientName))
                searchList.Add(new SearchValueData { Name = "ClientName", Value = Convert.ToString(searchReferralModel.ClientName) });

            searchList.Add(new SearchValueData { Name = "NotifyCaseManagerID", Value = Convert.ToString(searchReferralModel.NotifyCaseManagerID) });

            if (searchReferralModel.AgencyID > 0)
                searchList.Add(new SearchValueData { Name = "AgencyID", Value = Convert.ToString(searchReferralModel.AgencyID) });

            if (searchReferralModel.AgencyLocationID > 0)
                searchList.Add(new SearchValueData { Name = "AgencyLocationID", Value = Convert.ToString(searchReferralModel.AgencyLocationID) });

            if (searchReferralModel.AssigneeID > 0)
            {
                searchList.Add(new SearchValueData { Name = "AssigneeID", Value = Convert.ToString(searchReferralModel.AssigneeID) });
            }
            if (searchReferralModel.CaseManagerID > 0)
                searchList.Add(new SearchValueData { Name = "CaseManagerID", Value = Convert.ToString(searchReferralModel.CaseManagerID) });

            searchList.Add(new SearchValueData { Name = "ChecklistID", Value = Convert.ToString(searchReferralModel.ChecklistID) });

            if (!string.IsNullOrEmpty(searchReferralModel.AHCCCSID))
            {
                searchList.Add(new SearchValueData { Name = "AHCCCSID", Value = Convert.ToString(searchReferralModel.AHCCCSID) });
            }

            if (!string.IsNullOrEmpty(searchReferralModel.CISNumber))
            {
                searchList.Add(new SearchValueData { Name = "CISNumber", Value = Convert.ToString(searchReferralModel.CISNumber) });
            }

            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchReferralModel.IsDeleted) });

            searchList.Add(new SearchValueData { Name = "ClinicalReviewID", Value = Convert.ToString(searchReferralModel.ClinicalReviewID) });

            if (searchReferralModel.ReferralStatusID > 0)
                searchList.Add(new SearchValueData { Name = "ReferralStatusID", Value = Convert.ToString(searchReferralModel.ReferralStatusID) });

            if (searchReferralModel.PayorID > 0)
                searchList.Add(new SearchValueData { Name = "PayorID", Value = Convert.ToString(searchReferralModel.PayorID) });

            if (searchReferralModel.ServiceID != -1)
                searchList.Add(new SearchValueData { Name = "ServiceID", Value = Convert.ToString(searchReferralModel.ServiceID) });

            searchList.Add(new SearchValueData { Name = "IsSaveAsDraft", Value = Convert.ToString(searchReferralModel.IsSaveAsDraft) });

            if (searchReferralModel.ParentName != null)
                searchList.Add(new SearchValueData { Name = "ParentName", Value = Convert.ToString(searchReferralModel.ParentName) });

            if (searchReferralModel.ParentPhone != null)
                searchList.Add(new SearchValueData { Name = "ParentPhone", Value = Convert.ToString(searchReferralModel.ParentPhone) });

            if (searchReferralModel.CaseManagerPhone != null)
                searchList.Add(new SearchValueData { Name = "CaseManagerPhone", Value = Convert.ToString(searchReferralModel.CaseManagerPhone) });

            if (searchReferralModel.LanguageID > 0)
                searchList.Add(new SearchValueData { Name = "LanguageID", Value = Convert.ToString(searchReferralModel.LanguageID) });

            if (searchReferralModel.CommaSeparatedIds != null)
                searchList.Add(new SearchValueData { Name = "Groupdids", Value = Convert.ToString(searchReferralModel.CommaSeparatedIds) });

            if (searchReferralModel.CareTypeID != null)
                searchList.Add(new SearchValueData { Name = "CareTypeID", Value = Convert.ToString(searchReferralModel.CareTypeID) });

            if (searchReferralModel.RegionID > 0)
                searchList.Add(new SearchValueData { Name = "RegionID", Value = Convert.ToString(searchReferralModel.RegionID) });

            //searchList.Add(new SearchValueData { Name = "ServicetypeId", Value = Convert.ToString(searchReferralModel.ServiceTypeID) });
            searchList.Add(new SearchValueData { Name = "ServicetypeId", Value = Convert.ToString(searchReferralModel.CommaSeparatedServiceTypeIDs) });
            searchList.Add(new SearchValueData { Name = "DDType_PatientSystemStatus", Value = Convert.ToString((int)Common.DDType.PatientSystemStatus) });

            if (isList)
            {
                searchList.Add(new SearchValueData { Name = "EmployeeId", Value = Convert.ToString(loggedInId) });
                searchList.Add(new SearchValueData { Name = "ServerDateTime", Value = Common.GetOrgCurrentDateTime().ToString(Constants.DbDateTimeFormat) });
                searchList.Add(new SearchValueData { Name = "RecordAccess", Value = Common.HasPermission(Constants.AllRecordAccess) ? "-1" : Common.HasPermission(Constants.SameGroupAndLimitedPatientRecordAccess) ? "1" : "0" });
            }
        }

        public ServiceResponse DeleteReferral(SearchReferralListModel searchReferralListModel, int pageIndex,
                                              int pageSize, string sortIndex, string sortDirection, long loggedInId)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

            SetSearchFilterForReferralListPage(searchReferralListModel, searchList, loggedInId, false);

            if (!string.IsNullOrEmpty(searchReferralListModel.ListOfIdsInCsv))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchReferralListModel.ListOfIdsInCsv });

            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInId) });
            searchList.Add(new SearchValueData { Name = "CurrentDateTime", Value = Common.GetOrgCurrentDateTime().ToString(Constants.DbDateTimeFormat) });

            List<ReferralList> totalData = GetEntityList<ReferralList>(StoredProcedure.DeleteReferral, searchList);
            if (searchReferralListModel.ListOfIdsInCsv != "")
            {
                //searchList.Remove();
                searchList.Clear();
                searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);
                SetSearchFilterForReferralListPage(searchReferralListModel, searchList, loggedInId, true);
                totalData = GetEntityList<ReferralList>(StoredProcedure.GetReferralList, searchList);
            }
            //     totalData = GetEntityList<ReferralList>(StoredProcedure.GetReferralList, searchList);
            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            //if (count == 0)
            //{
            //    response.Message = Common.MessageWithTitle(string.Format(Resource.DeleteFailed, Resource.Referral), Resource.ReferralDependencyExistMessage);
            //    return response;
            //}

            Page<ReferralList> referralList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);

            response.Data = referralList;
            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordDeletedSuccessfully, Resource.Referral);

            return response;
        }

        public ServiceResponse SaveReferralStatus(ReferralStatusModel referralStatusModel, long loggedInId)
        {
            ServiceResponse response = new ServiceResponse();

            Referral referral = GetEntity<Referral>(referralStatusModel.ReferralID);
            if (referral != null)
            {
                #region Check all the Data for as per Referral Status

                if (referralStatusModel.ReferralStatusID == (int)ReferralStatus.ReferralStatuses.Active)
                {
                    if (referral.FirstName == null || referral.LastName == null || referral.Dob == null ||
                        referral.Gender == null || referral.AHCCCSID == null || referral.RegionID == null ||
                        referral.AHCCCSEnrollDate == null || referral.LanguageID == null || referral.DropOffLocation == null ||
                        referral.PickUpLocation == null || referral.FrequencyCodeID == null ||
                        referral.CaseManagerID == null || referral.ReferralSourceID == 0 || referral.ReferralDate == null || referral.Assignee == null)
                    {
                        response.Message = Resource.ReferralStatusChange;
                        return response;
                    }
                }
                else if (referralStatusModel.ReferralStatusID == (int)ReferralStatus.ReferralStatuses.Inactive)
                {
                    if (referral.FirstName == null || referral.LastName == null || referral.Dob == null ||
                           referral.Gender == null || referral.AHCCCSID == null || referral.RegionID == null ||
                           referral.AHCCCSEnrollDate == null || referral.LanguageID == null || referral.DropOffLocation == null ||
                           referral.PickUpLocation == null || referral.FrequencyCodeID == null ||
                           referral.CaseManagerID == null || referral.ReferralSourceID == 0 || referral.ReferralDate == null || referral.Assignee == null
                           || referral.ClosureDate == null || referral.ClosureReason == null)
                    {
                        response.Message = Resource.ReferralStatusChange;
                        return response;
                    }
                }
                else
                {
                    if (referral.FirstName == null || referral.LastName == null || referral.Dob == null ||
                           referral.Gender == null || referral.AHCCCSID == null || referral.RegionID == null ||
                           referral.AHCCCSEnrollDate == null || referral.LanguageID == null || referral.DropOffLocation == null ||
                           referral.PickUpLocation == null || referral.FrequencyCodeID == null ||
                           referral.CaseManagerID == null || referral.ReferralSourceID == 0 || referral.ReferralDate == null || referral.Assignee == null)
                    {
                        response.Message = Resource.ReferralStatusChange;
                        return response;
                    }
                }
                List<ContactMapping> contactlist = GetEntityList<ContactMapping>(new List<SearchValueData>
                            {
                                new SearchValueData{Name = "ReferralID",Value =  Convert.ToString(referral.ReferralID)},
                            });
                if (contactlist.Count <= 0)
                {
                    response.Message = Resource.ReferralStatusChange;
                    return response;
                }
                List<ReferralPayorMapping> payorlist = GetEntityList<ReferralPayorMapping>(new List<SearchValueData>
                            {
                                new SearchValueData{Name = "ReferralID",Value =  Convert.ToString(referral.ReferralID)},
                                new SearchValueData{Name = "IsActive",Value =  Convert.ToString(Constants.IsActiveStatus)},
                            });
                if (payorlist.Count <= 0)
                {
                    response.Message = Resource.ReferralStatusChange;
                    return response;
                }
                if (!referral.RespiteService && !referral.LifeSkillsService && !referral.CounselingService && !referral.ConnectingFamiliesService)
                {
                    response.Message = Resource.ReferralStatusChange;
                    return response;
                }

                #endregion

                if (referral.ReferralStatusID == (int)ReferralStatus.ReferralStatuses.Active &&
                    referralStatusModel.ReferralStatusID == (int)ReferralStatus.ReferralStatuses.Inactive)
                {
                    GetScalar(StoredProcedure.InactiveSchedule,
                              new List<SearchValueData>
                                  {
                                      new SearchValueData
                                          {
                                              Name = "ReferralID",
                                              Value = referral.ReferralID.ToString()
                                          },
                                      new SearchValueData
                                          {
                                              Name = "CancelStatus",
                                              Value = ((int) ScheduleStatus.ScheduleStatuses.Cancelled).ToString()
                                          },
                                      new SearchValueData {Name = "Comment", Value = @Resource.InactiveScheduleComment},
                                      new SearchValueData {Name = "WhoCancel", Value = Constants.Office},
                                      new SearchValueData
                                          {
                                              Name = "ChangeStatus",
                                              Value = string.Join(",", new List<String>
                                                  {
                                                      ((int) ScheduleStatus.ScheduleStatuses.Unconfirmed).ToString(),
                                                      ((int) ScheduleStatus.ScheduleStatuses.Waiting_List).ToString()
                                                  })
                                          }

                                  });
                }

                referral.ReferralStatusID = referralStatusModel.ReferralStatusID;
                SaveObject(referral, loggedInId);
                ReferralStatus referralStatus = GetEntity<ReferralStatus>(referral.ReferralStatusID.Value);
                response.IsSuccess = true;
                response.Data = referralStatus;
                response.Message = Resource.ReferralUpdated;





                #region  Notify CaseManager When Referral Status Change To Inactive OR Referral Accepted
                if (referralStatusModel.NotifyCmForInactiveStatus)
                {
                    if (referralStatusModel.ReferralStatusID == ((int)ReferralStatus.ReferralStatuses.ReferralAccepted))
                    {
                        SendReferralAcceptedStatusEmailToCm(referral.ReferralID);
                    }
                    else if (referralStatusModel.ReferralStatusID == ((int)ReferralStatus.ReferralStatuses.Inactive))
                    {
                        SendInactiveStatusEmailToCm(referral.ReferralID);
                    }

                }
                #endregion








                #region  Scrap Code
                //bool isValid = true;
                //if (referralStatusModel.ReferralStatusID == (int)ReferralStatus.ReferralStatuses.Active)
                //{
                //    if (referral.PickUpLocation == null || referral.DropOffLocation == null ||
                //        referral.FrequencyCodeID == null) //referral.OrientationDate == null || 
                //    {
                //        isValid = false;
                //    }
                //}
                //if (isValid)
                //{
                //}
                //else
                //{
                //    response.IsSuccess = false;
                //    response.Message = Resource.OrientationInfoRequired;
                //} 
                #endregion
            }
            else
            {
                response.Message = Resource.ExceptionMessage;
            }

            return response;
        }


        private void SendInactiveStatusEmailToCm(long referralId)
        {
            var searchList = new List<SearchValueData>
                    {
                        new SearchValueData {Name = "ReferralID", Value = Convert.ToString(referralId)},
                        new SearchValueData {Name = "EmailTemplateTypeID", Value = Convert.ToInt16(EnumEmailType.Notification_Of_Inactivate_Status).ToString()},
                    };
            SendNotificationToCMForInactiveStatus model = GetMultipleEntity<SendNotificationToCMForInactiveStatus>(StoredProcedure.NotificationToCMForInactiveStatus, searchList);
            EmailTemplate emailTemplate = model.EmailTemplate;
            model.NoticeModel.ClosureDate = !string.IsNullOrEmpty(model.NoticeModel.ClosureDate) ? String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(model.NoticeModel.ClosureDate)) : Resource.NA;
            model.NoticeModel.ClosureReason = !string.IsNullOrEmpty(model.NoticeModel.ClosureReason) ? model.NoticeModel.ClosureReason : Resource.NA;

            bool isSentMail = false;
            string body = null;

            string toEmail = model.NoticeModel.RecordRequestEmail;
            toEmail = string.IsNullOrEmpty(toEmail) ? model.NoticeModel.ToEmail : toEmail + ";" + model.NoticeModel.ToEmail;

            if (!string.IsNullOrEmpty(toEmail))
            {
                body = TokenReplace.ReplaceTokens(emailTemplate.EmailTemplateBody, model.NoticeModel);
                isSentMail = Common.SendEmail(emailTemplate.EmailTemplateSubject, _cacheHelper.SupportEmail, toEmail, body);
            }
            if (isSentMail)
            {

                INoteDataProvider iNoteDataProvider = new NoteDataProvider();
                iNoteDataProvider.SaveGeneralNote(referralId, body, Resource.EmailNotificationToCMInactiveStatus, SessionHelper.LoggedInID,
                     model.NoticeModel.CaseManager +
                     " (" + toEmail + ")", Resource.CaseManager, Resource.Email);
            }
        }

        private void SendReferralAcceptedStatusEmailToCm(long referralId)
        {
            var searchList = new List<SearchValueData>
                    {
                        new SearchValueData {Name = "ReferralID", Value = Convert.ToString(referralId)},
                        new SearchValueData {Name = "EmailTemplateTypeID", Value = Convert.ToInt16(EnumEmailType.Notification_Of_ReferralAccepted_Status).ToString()},
                    };
            SendNotificationToCMForReferralAcceptedStatus model = GetMultipleEntity<SendNotificationToCMForReferralAcceptedStatus>(StoredProcedure.NotificationToCMForReferralAcceptedStatus, searchList);
            bool isSentMail = false; string body = null;

            string toEmail = model.NoticeModel.ToEmail;

            if (!string.IsNullOrEmpty(toEmail))
            {
                body = TokenReplace.ReplaceTokens(model.EmailTemplate.EmailTemplateBody, model.NoticeModel);
                isSentMail = Common.SendEmail(model.EmailTemplate.EmailTemplateSubject, _cacheHelper.SupportEmail, toEmail, body);
            }
            if (isSentMail)
            {

                INoteDataProvider iNoteDataProvider = new NoteDataProvider();
                iNoteDataProvider.SaveGeneralNote(referralId, body, Resource.EmailNotificationToCMReferralAcceptedStatus, SessionHelper.LoggedInID,
                     model.NoticeModel.CaseManager +
                     " (" + toEmail + ")", Resource.CaseManager, Resource.Email);
            }
        }


        public ServiceResponse UpdateAssigneeBulk(ReferralBulkUpdateModel referralBulkUpdateModel, long loggedInId)
        {
            ServiceResponse response = new ServiceResponse();
            if (string.IsNullOrEmpty(referralBulkUpdateModel.BulkUpdateType) || string.IsNullOrEmpty(referralBulkUpdateModel.ReferralIDs) || string.IsNullOrEmpty(referralBulkUpdateModel.AssignedValues))
            {
                response.Message = Resource.ExceptionMessage;
                return response;
            }

            GetScalar(StoredProcedure.BulkUpdateReferralList, new List<SearchValueData>
                                          {
                                              new SearchValueData{Name = "BulkUpdateType",Value = referralBulkUpdateModel.BulkUpdateType},
                                              new SearchValueData{Name = "ReferralIDs",Value = referralBulkUpdateModel.ReferralIDs},
                                              new SearchValueData{Name = "AssignedValues",Value =referralBulkUpdateModel.AssignedValues}
                                          });
            response.IsSuccess = true;

            if (response.IsSuccess)
            {
                // response.Data = GetReferralList(searchReferralListModel, pageIndex, pageSize, sortIndex, sortDirection, loggedInId).Data;
                response.Message = Resource.AssigneeUpdated;
            }
            else
            {
                response.Message = Resource.ExceptionMessage;
            }

            return response;
        }

        public ServiceResponse UpdateAssignee(SearchReferralListModel searchReferralListModel, ReferralStatusModel referralStatusModel, int pageIndex, int pageSize,
                                                  string sortIndex, string sortDirection, long loggedInId)
        {
            ServiceResponse response = new ServiceResponse();

            Referral referral = GetEntity<Referral>(referralStatusModel.ReferralID);
            if (referral != null)
            {
                referral.Assignee = referralStatusModel.AssigneeID;
                SaveObject(referral, loggedInId);
                response.IsSuccess = true;
                response.Data =
                    GetReferralList(searchReferralListModel, pageIndex, pageSize, sortIndex, sortDirection, loggedInId)
                        .Data;
                response.Message = Resource.AssigneeUpdated;
            }
            else
            {
                response.Message = Resource.ExceptionMessage;
            }

            return response;
        }

        public ServiceResponse UpdateStatus(SearchReferralListModel searchReferralListModel, ReferralStatusModel referralStatusModel, int pageIndex, int pageSize,
                                                 string sortIndex, string sortDirection, long loggedInId)
        {

            ServiceResponse response = new ServiceResponse();

            Referral referral = GetEntity<Referral>(referralStatusModel.ReferralID);
            if (referral != null)
            {
                if (referral.ReferralStatusID == (int)ReferralStatus.ReferralStatuses.Active &&
                    referralStatusModel.ReferralStatusID != (int)ReferralStatus.ReferralStatuses.Active)
                {
                    GetScalar(StoredProcedure.InactiveSchedule,
                              new List<SearchValueData>
                                  {
                                      new SearchValueData
                                          {
                                              Name = "ReferralID",
                                              Value = referral.ReferralID.ToString()
                                          },
                                      new SearchValueData
                                          {
                                              Name = "CancelStatus",
                                              Value = ((int) ScheduleStatus.ScheduleStatuses.Cancelled).ToString()
                                          },
                                      new SearchValueData {Name = "Comment", Value = @Resource.InactiveScheduleComment},
                                      new SearchValueData {Name = "WhoCancel", Value = Constants.Office},
                                      new SearchValueData
                                          {
                                              Name = "ChangeStatus",
                                              Value = string.Join(",", new List<String>
                                                  {
                                                      ((int) ScheduleStatus.ScheduleStatuses.Unconfirmed).ToString(),
                                                      ((int) ScheduleStatus.ScheduleStatuses.Waiting_List).ToString()
                                                  })
                                          }

                                  });
                }

                referral.ReferralStatusID = referralStatusModel.ReferralStatusID;
                SaveObject(referral, loggedInId);
                response.IsSuccess = true;
                response.Data =
                    GetReferralList(searchReferralListModel, pageIndex, pageSize, sortIndex, sortDirection, loggedInId)
                        .Data;
                response.Message = Resource.StatusUpdated;
            }
            else
            {
                response.Message = Resource.ExceptionMessage;
            }

            return response;
        }

        public ServiceResponse UpdateTimeSlotDetailEmployee(SearchReferralTimeSlotDetail searchReferralTimeSlotDetail, int pageIndex, int pageSize,
                                                 string sortIndex, string sortDirection, long loggedInId)
        {

            ServiceResponse response = new ServiceResponse();

            //ScheduleMaster scheduleMaster = GetEntity<ScheduleMaster>(searchReferralTimeSlotDetail.ScheduleID);
            //if (scheduleMaster != null)
            //{
            //    scheduleMaster.EmployeeID = searchReferralTimeSlotDetail.EmployeeID;
            //    SaveObject(scheduleMaster, loggedInId);
            //    response.IsSuccess = true;

            //    response.Data = GetReferralTimeSlotDetail(searchReferralTimeSlotDetail, pageIndex, pageSize, sortIndex, sortDirection).Data;
            //    response.Message = Resource.AssigneeUpdated;
            //}
            //else
            //{
            //    response.Message = Resource.ExceptionMessage;
            //}
            if (searchReferralTimeSlotDetail != null)
            {
                GetScalar(StoredProcedure.UpdateReferralTimeSlotAssignee, new List<SearchValueData>
                                          {
                                              new SearchValueData{Name = "ScheduleID",Value = searchReferralTimeSlotDetail.ScheduleID.ToString()},
                                              new SearchValueData{Name = "EmployeeID",Value = searchReferralTimeSlotDetail.EmployeeID.ToString()}
                                          });


                response.IsSuccess = true;

                response.Data = GetReferralTimeSlotDetail(searchReferralTimeSlotDetail, pageIndex, pageSize, sortIndex, sortDirection).Data;
                response.Message = Resource.AssigneeUpdated;
            }
            else
            {
                response.Message = Resource.ExceptionMessage;
            }

            return response;
        }



        public ServiceResponse SaveIncidentReportOrbeonForm(EmployeeVisitNoteForm form)
        {
            var response = new ServiceResponse();
            try
            {
                IReferralDataProvider _referralDataProvider = new ReferralDataProvider();

                // Save Referral Document Orbeon
                LinkDocModel linkDocModel = new LinkDocModel()
                {
                    DocumentID = form.OrbeonFormID,
                    ReferralDocumentID = form.ReferralDocumentID,
                    EmployeeID = form.EmployeeID,
                    ReferralID = form.ReferralID,
                    ComplianceID = form.ComplianceID
                };
                var res = _referralDataProvider.HC_GetOrbeonFormDetailsByID(linkDocModel, false);
                if (!res.IsSuccess)
                { return res; }

                // Add Referral Task Form
                var doc = res.Data as ReferralDocument;
                if (form.ReferralTaskFormMappingID == 0)
                {
                    List<SearchValueData> searchList = new List<SearchValueData>();
                    searchList.Add(new SearchValueData { Name = "EmployeeVisitID", Value = Convert.ToString(form.EmployeeVisitID) });
                    searchList.Add(new SearchValueData { Name = "ReferralTaskMappingID", Value = Convert.ToString(form.ReferralTaskMappingID) });
                    searchList.Add(new SearchValueData { Name = "TaskFormMappingID", Value = Convert.ToString(form.TaskFormMappingID) });
                    searchList.Add(new SearchValueData { Name = "ReferralDocumentID", Value = Convert.ToString(doc.ReferralDocumentID) });
                    GetScalar(StoredProcedure.AddReferralTaskForm, searchList);
                    response.Message = string.Format(Resource.RecordCreatedSuccessfully, Resource.IncidentReportForm);
                    response.IsSuccess = true;
                }
                else
                {
                    response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.IncidentReportForm);
                    response.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse SaveReferralFaceSheetForm(EmployeeVisitNoteForm form)
        {
            var response = new ServiceResponse();
            try
            {
                IReferralDataProvider _referralDataProvider = new ReferralDataProvider();

                // Save Referral Document Orbeon
                LinkDocModel linkDocModel = new LinkDocModel()
                {
                    DocumentID = form.OrbeonFormID,
                    ReferralDocumentID = form.ReferralDocumentID,
                    EmployeeID = form.EmployeeID,
                    ReferralID = form.ReferralID,
                    ComplianceID = form.ComplianceID
                };
                var res = _referralDataProvider.HC_GetOrbeonFormDetailsByID(linkDocModel, false);
                if (!res.IsSuccess)
                { return res; }

                // Add Referral Task Form
                var doc = res.Data as ReferralDocument;
                if (form.ReferralDocumentID == 0)
                {
                    List<SearchValueData> searchList = new List<SearchValueData>();
                    searchList.Add(new SearchValueData { Name = "EmployeeVisitID", Value = Convert.ToString(form.EmployeeVisitID) });
                    searchList.Add(new SearchValueData { Name = "ReferralTaskMappingID", Value = Convert.ToString(form.ReferralTaskMappingID) });
                    searchList.Add(new SearchValueData { Name = "TaskFormMappingID", Value = Convert.ToString(form.TaskFormMappingID) });
                    searchList.Add(new SearchValueData { Name = "ReferralDocumentID", Value = Convert.ToString(doc.ReferralDocumentID) });
                    GetScalar(StoredProcedure.AddReferralTaskForm, searchList);
                    response.Message = string.Format(Resource.RecordCreatedSuccessfully, "FaceSheetForm");
                    response.IsSuccess = true;
                }
                else
                {
                    response.Message = string.Format(Resource.RecordUpdatedSuccessfully, "FaceSheetForm");
                    response.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }
        public ServiceResponse SaveVitalForm(EmployeeVisitNoteForm form)
        {
            var response = new ServiceResponse();
            try
            {
                IReferralDataProvider _referralDataProvider = new ReferralDataProvider();

                // Save Referral Document Orbeon
                LinkDocModel linkDocModel = new LinkDocModel()
                {
                    DocumentID = form.OrbeonFormID,
                    ReferralDocumentID = form.ReferralDocumentID,
                    EmployeeID = form.EmployeeID,
                    ReferralID = form.ReferralID,
                    ComplianceID = form.ComplianceID
                };
                var res = _referralDataProvider.HC_GetOrbeonFormDetailsByID(linkDocModel, false);
                if (!res.IsSuccess)
                { return res; }

                // Add Referral Task Form
                var doc = res.Data as ReferralDocument;
                if (form.ReferralDocumentID == 0)
                {
                    List<SearchValueData> searchList = new List<SearchValueData>();
                    searchList.Add(new SearchValueData { Name = "EmployeeVisitID", Value = Convert.ToString(form.EmployeeVisitID) });
                    searchList.Add(new SearchValueData { Name = "ReferralTaskMappingID", Value = Convert.ToString(form.ReferralTaskMappingID) });
                    searchList.Add(new SearchValueData { Name = "TaskFormMappingID", Value = Convert.ToString(form.TaskFormMappingID) });
                    searchList.Add(new SearchValueData { Name = "ReferralDocumentID", Value = Convert.ToString(doc.ReferralDocumentID) });
                    GetScalar(StoredProcedure.AddReferralTaskForm, searchList);
                    response.Message = string.Format(Resource.RecordCreatedSuccessfully, "Vital Form");
                    response.IsSuccess = true;
                }
                else
                {
                    response.Message = string.Format(Resource.RecordUpdatedSuccessfully, "Vital Form");
                    response.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        #endregion Referral List

        #region Referral Tracking List


        public ServiceResponse SetReferralTrackingListPage(long loggedInID)
        {
            ServiceResponse response = new ServiceResponse();

            SetReferralTrackingListModel setReferralTrackingListModel = GetMultipleEntity<SetReferralTrackingListModel>(StoredProcedure.SetReferralTrackingListPage);

            if (setReferralTrackingListModel.SearchReferralTrackingListModel == null)
                setReferralTrackingListModel.SearchReferralTrackingListModel = new SearchReferralTrackingListModel();

            DateTime date = DateTime.Now;
            setReferralTrackingListModel.SearchReferralTrackingListModel.ReferralDate = new DateTime(date.Year, date.Month, 1);
            setReferralTrackingListModel.SearchReferralTrackingListModel.ReferralToDate = setReferralTrackingListModel.SearchReferralTrackingListModel.ReferralDate.Value.AddMonths(1).AddDays(-1);

            setReferralTrackingListModel.DeleteFilter = Common.SetDeleteFilter();
            response.Data = setReferralTrackingListModel;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse GetReferralTrackingList(SearchReferralTrackingListModel searchReferralModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchReferralModel != null)
                SetSearchFilterForReferralTrackingListPage(searchReferralModel, searchList, loggedInId);

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<ReferralTrackingList> totalData = GetEntityList<ReferralTrackingList>(StoredProcedure.GetReferralTrackingList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ReferralTrackingList> getReferralList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = getReferralList;
            response.IsSuccess = true;
            return response;
        }

        private static void SetSearchFilterForReferralTrackingListPage(SearchReferralTrackingListModel searchReferralModel, List<SearchValueData> searchList, long loggedInId)
        {

            if (searchReferralModel.ReferralStatusID > 0)
                searchList.Add(new SearchValueData { Name = "ReferralStatusID", Value = Convert.ToString(searchReferralModel.ReferralStatusID) });
            if (!string.IsNullOrEmpty(searchReferralModel.ClientName))
                searchList.Add(new SearchValueData { Name = "ClientName", Value = Convert.ToString(searchReferralModel.ClientName) });
            if (!string.IsNullOrEmpty(searchReferralModel.AHCCCSID))
                searchList.Add(new SearchValueData { Name = "AHCCCSID", Value = Convert.ToString(searchReferralModel.AHCCCSID) });
            if (searchReferralModel.PayorID > 0)
                searchList.Add(new SearchValueData { Name = "PayorID", Value = Convert.ToString(searchReferralModel.PayorID) });
            if (searchReferralModel.AgencyID > 0)
                searchList.Add(new SearchValueData { Name = "AgencyID", Value = Convert.ToString(searchReferralModel.AgencyID) });
            if (searchReferralModel.CaseManagerID > 0)
                searchList.Add(new SearchValueData { Name = "CaseManagerID", Value = Convert.ToString(searchReferralModel.CaseManagerID) });


            if (searchReferralModel.CMNotifiedDate.HasValue)
                searchList.Add(new SearchValueData { Name = "CMNotifiedDate", Value = searchReferralModel.CMNotifiedDate.Value.ToString(Constants.DbDateFormat) });
            if (searchReferralModel.CMNotifiedToDate.HasValue)
                searchList.Add(new SearchValueData { Name = "CMNotifiedToDate", Value = searchReferralModel.CMNotifiedToDate.Value.ToString(Constants.DbDateFormat) });

            if (searchReferralModel.ReferralDate.HasValue)
                searchList.Add(new SearchValueData { Name = "ReferralDate", Value = searchReferralModel.ReferralDate.Value.ToString(Constants.DbDateFormat) });
            if (searchReferralModel.ReferralToDate.HasValue)
                searchList.Add(new SearchValueData { Name = "ReferralToDate", Value = searchReferralModel.ReferralToDate.Value.ToString(Constants.DbDateFormat) });

            if (searchReferralModel.CreatedDate.HasValue)
                searchList.Add(new SearchValueData { Name = "CreatedDate", Value = TimeZoneInfo.ConvertTimeToUtc(searchReferralModel.CreatedDate.Value).ToString(Constants.DbDateTimeFormat) });
            if (searchReferralModel.CreatedToDate.HasValue)
                searchList.Add(new SearchValueData { Name = "CreatedToDate", Value = TimeZoneInfo.ConvertTimeToUtc(searchReferralModel.CreatedToDate.Value).AddDays(1).ToString(Constants.DbDateTimeFormat) });

            if (searchReferralModel.ChecklistDate.HasValue)
                searchList.Add(new SearchValueData { Name = "ChecklistDate", Value = TimeZoneInfo.ConvertTimeToUtc(searchReferralModel.ChecklistDate.Value).ToString(Constants.DbDateTimeFormat) });
            if (searchReferralModel.ChecklistToDate.HasValue)
                searchList.Add(new SearchValueData { Name = "ChecklistToDate", Value = TimeZoneInfo.ConvertTimeToUtc(searchReferralModel.ChecklistToDate.Value).AddDays(1).ToString(Constants.DbDateTimeFormat) });

            if (searchReferralModel.SparDate.HasValue)
                searchList.Add(new SearchValueData { Name = "SparDate", Value = TimeZoneInfo.ConvertTimeToUtc(searchReferralModel.SparDate.Value).ToString(Constants.DbDateTimeFormat) });
            if (searchReferralModel.SparToDate.HasValue)
                searchList.Add(new SearchValueData { Name = "SparToDate", Value = TimeZoneInfo.ConvertTimeToUtc(searchReferralModel.SparToDate.Value).AddDays(1).ToString(Constants.DbDateTimeFormat) });

            searchList.Add(new SearchValueData { Name = "ReferralTrackingComment", Value = searchReferralModel.ReferralTrackingComment });
            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = "-1" });

            //if (searchModel.UpdatedFromDate.HasValue)
            //    searchList.Add(new SearchValueData { Name = "FromDate", Value = TimeZoneInfo.ConvertTimeToUtc(searchModel.UpdatedFromDate.Value).ToString(Constants.DbDateTimeFormat) });
            //if (searchModel.UpdatedToDate.HasValue)
            //    searchList.Add(new SearchValueData { Name = "ToDate", Value = TimeZoneInfo.ConvertTimeToUtc(searchModel.UpdatedToDate.Value.AddDays(1)).ToString(Constants.DbDateTimeFormat) }); 





        }

        public ServiceResponse DeleteReferralTracking(SearchReferralTrackingListModel searchReferralListModel, int pageIndex,
                                              int pageSize, string sortIndex, string sortDirection, long loggedInId)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

            SetSearchFilterForReferralTrackingListPage(searchReferralListModel, searchList, loggedInId);

            if (!string.IsNullOrEmpty(searchReferralListModel.ListOfIdsInCsv))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchReferralListModel.ListOfIdsInCsv });

            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInId) });

            List<ReferralTrackingList> totalData = GetEntityList<ReferralTrackingList>("DeleteReferral", searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            //if (count == 0)
            //{
            //    response.Message = Common.MessageWithTitle(string.Format(Resource.DeleteFailed, Resource.Referral), Resource.ReferralDependencyExistMessage);
            //    return response;
            //}

            Page<ReferralTrackingList> referralList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);

            response.Data = referralList;
            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.Referral);

            return response;
        }

        public ServiceResponse SaveReferralTrackingStatus(ReferralStatusModel referralStatusModel, long loggedInId)
        {
            ServiceResponse response = new ServiceResponse();

            Referral referral = GetEntity<Referral>(referralStatusModel.ReferralID);
            if (referral != null)
            {
                #region Check all the Data for as per Referral Status

                if (referralStatusModel.ReferralStatusID == (int)ReferralStatus.ReferralStatuses.Active)
                {
                    if (referral.FirstName == null || referral.LastName == null || referral.Dob == null ||
                        referral.Gender == null || referral.AHCCCSID == null || referral.RegionID == null ||
                        referral.AHCCCSEnrollDate == null || referral.LanguageID == null || referral.DropOffLocation == null ||
                        referral.PickUpLocation == null || referral.FrequencyCodeID == null ||
                        referral.CaseManagerID == null || referral.ReferralSourceID == 0 || referral.ReferralDate == null || referral.Assignee == null)
                    {
                        response.Message = Resource.ReferralStatusChange;
                        return response;
                    }
                }
                else if (referralStatusModel.ReferralStatusID == (int)ReferralStatus.ReferralStatuses.Inactive)
                {
                    if (referral.FirstName == null || referral.LastName == null || referral.Dob == null ||
                           referral.Gender == null || referral.AHCCCSID == null || referral.RegionID == null ||
                           referral.AHCCCSEnrollDate == null || referral.LanguageID == null || referral.DropOffLocation == null ||
                           referral.PickUpLocation == null || referral.FrequencyCodeID == null ||
                           referral.CaseManagerID == null || referral.ReferralSourceID == 0 || referral.ReferralDate == null || referral.Assignee == null
                           || referral.ClosureDate == null || referral.ClosureReason == null)
                    {
                        response.Message = Resource.ReferralStatusChange;
                        return response;
                    }
                }
                else
                {
                    if (referral.FirstName == null || referral.LastName == null || referral.Dob == null ||
                           referral.Gender == null || referral.AHCCCSID == null || referral.RegionID == null ||
                           referral.AHCCCSEnrollDate == null || referral.LanguageID == null || referral.DropOffLocation == null ||
                           referral.PickUpLocation == null || referral.FrequencyCodeID == null ||
                           referral.CaseManagerID == null || referral.ReferralSourceID == 0 || referral.ReferralDate == null || referral.Assignee == null)
                    {
                        response.Message = Resource.ReferralStatusChange;
                        return response;
                    }
                }
                List<ContactMapping> contactlist = GetEntityList<ContactMapping>(new List<SearchValueData>
                            {
                                new SearchValueData{Name = "ReferralID",Value =  Convert.ToString(referral.ReferralID)},
                            });
                if (contactlist.Count <= 0)
                {
                    response.Message = Resource.ReferralStatusChange;
                    return response;
                }
                List<ReferralPayorMapping> payorlist = GetEntityList<ReferralPayorMapping>(new List<SearchValueData>
                            {
                                new SearchValueData{Name = "ReferralID",Value =  Convert.ToString(referral.ReferralID)},
                                new SearchValueData{Name = "IsActive",Value =  Convert.ToString(Constants.IsActiveStatus)},
                            });
                if (payorlist.Count <= 0)
                {
                    response.Message = Resource.ReferralStatusChange;
                    return response;
                }
                if (!referral.RespiteService && !referral.LifeSkillsService && !referral.CounselingService && !referral.ConnectingFamiliesService)
                {
                    response.Message = Resource.ReferralStatusChange;
                    return response;
                }

                #endregion

                if (referral.ReferralStatusID == (int)ReferralStatus.ReferralStatuses.Active &&
                    referralStatusModel.ReferralStatusID == (int)ReferralStatus.ReferralStatuses.Inactive)
                {
                    GetScalar(StoredProcedure.InactiveSchedule,
                              new List<SearchValueData>
                                  {
                                      new SearchValueData
                                          {
                                              Name = "ReferralID",
                                              Value = referral.ReferralID.ToString()
                                          },
                                      new SearchValueData
                                          {
                                              Name = "CancelStatus",
                                              Value = ((int) ScheduleStatus.ScheduleStatuses.Cancelled).ToString()
                                          },
                                      new SearchValueData {Name = "Comment", Value = @Resource.InactiveScheduleComment},
                                      new SearchValueData {Name = "WhoCancel", Value = Constants.Office},
                                      new SearchValueData
                                          {
                                              Name = "ChangeStatus",
                                              Value = string.Join(",", new List<String>
                                                  {
                                                      ((int) ScheduleStatus.ScheduleStatuses.Unconfirmed).ToString(),
                                                      ((int) ScheduleStatus.ScheduleStatuses.Waiting_List).ToString()
                                                  })
                                          }

                                  });
                }

                referral.ReferralStatusID = referralStatusModel.ReferralStatusID;
                SaveObject(referral, loggedInId);
                ReferralStatus referralStatus = GetEntity<ReferralStatus>(referral.ReferralStatusID.Value);
                response.IsSuccess = true;
                response.Data = referralStatus;
                response.Message = Resource.ReferralUpdated;



                #region  Notify CaseManager When Referral Status Change To Inactive OR Referral Accepted
                if (referralStatusModel.NotifyCmForInactiveStatus)
                {
                    if (referralStatusModel.ReferralStatusID == ((int)ReferralStatus.ReferralStatuses.ReferralAccepted))
                    {
                        SendReferralAcceptedStatusEmailToCm(referral.ReferralID);
                    }
                    else if (referralStatusModel.ReferralStatusID == ((int)ReferralStatus.ReferralStatuses.Inactive))
                    {
                        SendInactiveStatusEmailToCm(referral.ReferralID);
                    }

                }
                #endregion



                #region  Scrap Code
                //bool isValid = true;
                //if (referralStatusModel.ReferralStatusID == (int)ReferralStatus.ReferralStatuses.Active)
                //{
                //    if (referral.PickUpLocation == null || referral.DropOffLocation == null ||
                //        referral.FrequencyCodeID == null) //referral.OrientationDate == null || 
                //    {
                //        isValid = false;
                //    }
                //}
                //if (isValid)
                //{
                //}
                //else
                //{
                //    response.IsSuccess = false;
                //    response.Message = Resource.OrientationInfoRequired;
                //} 
                #endregion
            }
            else
            {
                response.Message = Resource.ExceptionMessage;
            }

            return response;
        }

        public ServiceResponse SaveReferralTrackingComment(ReferralCommentModel referralCommentModel, long loggedInId)
        {
            ServiceResponse response = new ServiceResponse();

            Referral referral = GetEntity<Referral>(referralCommentModel.ReferralID);
            if (referral != null)
            {
                referral.ReferralTrackingComment = referralCommentModel.Comment;
                SaveObject(referral, loggedInId);

                response.IsSuccess = true;
                response.Message = Resource.ReferralCommentUpdated;
            }
            else
            {
                response.Message = Resource.ExceptionMessage;
            }

            return response;
        }

        #endregion Referral List

        #region Referral Related DX Codes Methods

        /// <summary>
        /// this method will return the list of dxcode 
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="ignoreIds"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<DXCode> GetDxcodeListForAutoComplete(string searchText, string ignoreIds, int pageSize)
        {
            List<DXCode> dxcodelist = new List<DXCode>();
            //   List<DXCode> dxcodelist = GetEntityList<DXCode>(StoredProcedure.GetDxCodeListForAutoCompleter, new List<SearchValueData>
            //    {
            //    new SearchValueData{Name = "SearchText",Value = searchText},
            //    new SearchValueData{Name = "IgnoreIds",Value = ignoreIds},
            //    new SearchValueData{Name = "PazeSize",Value = pageSize.ToString()}

            //});
            if (/*dxcodelist.Count == 0 &&*/ searchText != "")
            {
                ServiceResponse response = new ServiceResponse();
                CareGiverApi careGiverApi = new CareGiverApi();
                var results = careGiverApi.GetDxCode(searchText);
                foreach (var item in results)
                {
                    dxcodelist.Add(new DXCode
                    {
                        DXCodeName = item.DXCodeName,
                        DXCodeWithoutDot = item.DXCodeWithoutDot,
                        DxCodeType = item.DxCodeType,
                        Description = item.Description,
                        DxCodeShortName = item.DxCodeShortName,
                    });
                }
            }
            return dxcodelist;

        }
        public ServiceResponse SaveDxCode(string DXCodeName, string DXCodeWithoutDot, string DxCodeType, string Description, string DxCodeShortName)
        {
            ServiceResponse response = new ServiceResponse();

            var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "DXCodeName", Value = Convert.ToString(DXCodeName)},
                    new SearchValueData {Name = "DXCodeWithoutDot", Value = Convert.ToString(DXCodeWithoutDot)},
                    new SearchValueData {Name = "DxCodeType", Value = Convert.ToString(DxCodeType)},
                    new SearchValueData {Name = "Description", Value = Convert.ToString(Description)},
                    new SearchValueData {Name = "DxCodeShortName", Value = Convert.ToString(DxCodeShortName)},
                };


            GetScalar(StoredProcedure.SaveDxCodeAPI, searchlist);
            response.IsSuccess = true;
            //  response.Message = "DxCodeSavedSuccessfully";
            DXCodeModels dxcodelists = GetEntity<DXCodeModels>(StoredProcedure.GetDxCode, searchlist
            //    new List<SearchValueData>
            //    {
            //  new SearchValueData {Name = "DXCodeName", Value = Convert.ToString(DXCodeName)},
            //        new SearchValueData {Name = "DXCodeWithoutDot", Value = Convert.ToString(DXCodeWithoutDot)},
            //        new SearchValueData {Name = "DxCodeType", Value = Convert.ToString(DxCodeType)},
            //        new SearchValueData {Name = "Description", Value = Convert.ToString(Description)},
            //        new SearchValueData {Name = "DxCodeShortName", Value = Convert.ToString(DxCodeShortName)},

            //}
                );
            if (dxcodelists != null)
            {
                var dxcodelist = dxcodelists;
            }
            else
            {
                var dxcodelist = "";
            }

            response.Data = dxcodelists;
            return response;
        }


        public List<ReferralCaseManagerAutoComplete> GetCaseManagerForAutoComplete(string searchText, int pageSize)
        {
            return GetEntityList<ReferralCaseManagerAutoComplete>(StoredProcedure.HC_GetCaseManagerListForAutoCompleter, new List<SearchValueData>
                {
                new SearchValueData{Name = "SearchText",Value = searchText},
                new SearchValueData{Name = "PazeSize",Value = pageSize.ToString()}

            });
        }

        /// <summary>
        /// This method will delete the mapping of dxcode
        /// </summary>
        /// <param name="referralDxCodeMappingDeleteModel"></param>
        /// <returns></returns>
        public ServiceResponse DeleteReferralDxCodeMapping(ReferralDxCodeMappingDeleteModel referralDxCodeMappingDeleteModel, long referralId, long loggedInId)
        {
            ServiceResponse response = new ServiceResponse();

            ReferralDXCodeMapping tempReferralDxCodeMapping =
                GetEntity<ReferralDXCodeMapping>(referralDxCodeMappingDeleteModel.ReferralDXCodeMappingID);

            List<DXCodeMappingList> dxCodeMappingList = GetEntityList<DXCodeMappingList>(StoredProcedure.DeleteDxCodeMapping,
                new List<SearchValueData>
                {
                    new SearchValueData
                    {
                        Name = "ReferralDXCodeMappingID",
                        Value = Convert.ToString(referralDxCodeMappingDeleteModel.ReferralDXCodeMappingID)
                    },
                    new SearchValueData
                    {
                        Name = "IsSoftDelte",
                        Value = Convert.ToString(referralDxCodeMappingDeleteModel.IsSoftDelte)
                    },
                    new SearchValueData
                    {
                        Name = "ReferralId",
                        Value = Convert.ToString(referralId)
                    }

                });

            response.Data = dxCodeMappingList;
            response.IsSuccess = true;
            response.Message = (referralDxCodeMappingDeleteModel.IsSoftDelte) ? (
                (referralDxCodeMappingDeleteModel.IsEnable) ? Resource.DxCodeEnableSuccessfully : Resource.DxCodeDisabledSuccessfully
                ) : Resource.DxCodeDeleted;

            Common.CreateAuditTrail(AuditActionType.Delete, referralId, referralDxCodeMappingDeleteModel.ReferralDXCodeMappingID,
                                                 tempReferralDxCodeMapping, new ReferralDXCodeMapping(), loggedInId);
            return response;
        }
        public ServiceResponse HC_DeleteReferralDxCodeMapping(ReferralDxCodeMappingDeleteModel referralDxCodeMappingDeleteModel, long referralId, long loggedInId)
        {
            ServiceResponse response = new ServiceResponse();

            //ReferralDXCodeMapping tempReferralDxCodeMapping =
            //    GetEntity<ReferralDXCodeMapping>(referralDxCodeMappingDeleteModel.ReferralDXCodeMappingID);

            List<DXCodeMappingList> dxCodeMappingList = GetEntityList<DXCodeMappingList>(StoredProcedure.HC_DeleteDxCodeMapping,
                new List<SearchValueData>
                {
                    new SearchValueData
                    {
                        Name = "ReferralDXCodeMappingID",
                        Value = Convert.ToString(referralDxCodeMappingDeleteModel.ReferralDXCodeMappingID)
                    },
                    new SearchValueData
                    {
                        Name = "ReferralId",
                        Value = Convert.ToString(referralId)
                    }
                });

            response.Data = dxCodeMappingList;
            response.IsSuccess = true;
            response.Message = Resource.DxCodeDeleted;

            //Common.CreateAuditTrail(AuditActionType.Delete, referralId, referralDxCodeMappingDeleteModel.ReferralDXCodeMappingID,
            //                                     tempReferralDxCodeMapping, new ReferralDXCodeMapping(), loggedInId);
            return response;
        }
        #endregion Sagar

        #region Referral Review Assessment

        public ServiceResponse SetReferralReviewAssessment(long referralID, long referralAssessmentID, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();
            ReferralAssessmentReviewModel referralAssessmentReview = new ReferralAssessmentReviewModel();
            referralAssessmentReview = GetMultipleEntity<ReferralAssessmentReviewModel>(StoredProcedure.ReferralReviewAssessment,
                                                    new List<SearchValueData>
                                                        {
                                                            new SearchValueData{Name = "ReferralID",Value = referralID.ToString()},
                                                            new SearchValueData{Name = "ReferralAssessmentID",Value = referralAssessmentID.ToString()},
                                                            new SearchValueData{Name = "LoginID",Value = loggedInUserID.ToString()},
                                                        });

            if (referralAssessmentReview.ReferralAssessmentReview == null)
            {
                referralAssessmentReview.ReferralAssessmentReview = new ReferralAssessmentReview();
                referralAssessmentReview.ReferralAssessmentReview.Assignee = loggedInUserID;
            }

            if (referralAssessmentReview.PastReferralAssessmentReview == null)
                referralAssessmentReview.PastReferralAssessmentReview = new ReferralAssessmentReview();

            referralAssessmentReview.AmazonSettingModel =
                    AmazonFileUpload.GetAmazonModelForClientSideUpload(loggedInUserID,
                                                                       ConfigSettings.AmazoneUploadPath +
                                                                       ConfigSettings.ReferralAssessmentResultUploadPath + ConfigSettings.TempFiles +
                                                                       referralID +
                                                                       "/", ConfigSettings.PrivateAcl);

            response.IsSuccess = true;
            response.Data = referralAssessmentReview;
            return response;
        }

        public ServiceResponse SaveReferralReviewAssessment(ReferralAssessmentReview model, long loggedInUserID)
        {

            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralID", Value = model.ReferralID.ToString(),IsEqual = true},
                    new SearchValueData{Name = "AssessmentDate",Value = model.AssessmentDate.ToString(Constants.DbDateFormat),IsEqual = true},
                    new SearchValueData{Name = "ReferralAssessmentID",Value = model.ReferralAssessmentID.ToString(),IsNotEqual = true},
                    new SearchValueData{Name = "IsDeleted",Value = "0"}
                };

            ReferralAssessmentReview tempReferralAssessmentReview = GetEntity<ReferralAssessmentReview>(searchParam);
            if (tempReferralAssessmentReview != null)
            {
                response.IsSuccess = false;
                response.Message = Resource.ReferralAssessmentExists;
                return response;
            }





            searchParam = new List<SearchValueData> { new SearchValueData { Name = "ReferralAssessmentID", Value = model.ReferralAssessmentID.ToString(), IsEqual = true } };
            ReferralAssessmentReview tempReview = GetEntity<ReferralAssessmentReview>(searchParam) ?? new ReferralAssessmentReview();



            if (model.MarkAsComplete && model.SignatureBy.HasValue == false)
            {
                model.SignatureBy = loggedInUserID;
                model.SignatureDate = DateTime.UtcNow;
            }
            else if (!model.MarkAsComplete)
            {
                model.SignatureBy = null;
                model.SignatureDate = null;
            }

            if (tempReview.Assignee != model.Assignee)
            {
                model.AssignedDate = DateTime.UtcNow;
                model.AssignedBy = loggedInUserID;
            }

            if (!string.IsNullOrEmpty(model.TempAssessmentResultUrl))
            {
                AmazonFileUpload amazonFileUpload = new AmazonFileUpload();
                string bucket = ConfigSettings.ZarephathBucket;
                string destPath = ConfigSettings.AmazoneUploadPath + ConfigSettings.ReferralAssessmentResultUploadPath + "/" + model.ReferralID +
                    model.TempAssessmentResultUrl.Substring(model.TempAssessmentResultUrl.LastIndexOf('/'));

                amazonFileUpload.MoveFile(bucket, model.TempAssessmentResultUrl, bucket, destPath, S3CannedACL.PublicRead);

                model.FilePath = destPath;
            }
            return SaveObject(model, loggedInUserID, Resource.ReferralReviewAssessmentSavedSuccessfully);
        }

        public ServiceResponse GetReferralReviewAssessmentList(SearchReferralAssessmentReview searchModel)
        {
            ServiceResponse response = new ServiceResponse();

            #region Set SearchField
            List<SearchValueData> search = new List<SearchValueData>
                {
                    new SearchValueData
                        {
                            Name = "ReferralID",
                            Value = searchModel.ReferralID.ToString()
                        }
                };

            if (searchModel.StartDate.HasValue)
            {
                search.Add(new SearchValueData
                {
                    Name = "StartDate",
                    Value = searchModel.StartDate.Value.ToString(Constants.DbDateFormat)
                });
            }


            if (searchModel.EndDate.HasValue)
            {
                search.Add(new SearchValueData
                {
                    Name = "EndDate",
                    Value = searchModel.EndDate.Value.ToString(Constants.DbDateFormat)
                });
            }

            #endregion


            List<ReferralAssessmentReviewListModel> assessmentReviews = GetEntityList<ReferralAssessmentReviewListModel>(StoredProcedure.GetReferralAssessmentReview, search);

            List<ReferralAssessmentReviewGraphModel> graphModel = new List<ReferralAssessmentReviewGraphModel>();

            if (assessmentReviews.Any())
            {
                graphModel = assessmentReviews.Select(m => new ReferralAssessmentReviewGraphModel
                {
                    title = m.AssessmentDate.ToString(ConfigSettings.ClientSideDateFormat),
                    data = m.GetDataPoints()
                }).ToList();
            }



            response.Data = new ReferralAssessmentReviewDetail
            {
                GraphSeriesList = graphModel,
                ReferralAssessmentList = assessmentReviews
            };
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse DeleteReferralReviewAssessment(SearchReferralAssessmentReview searchModel)
        {
            ServiceResponse response = new ServiceResponse();

            #region Set SearchField

            List<SearchValueData> search = new List<SearchValueData>();
            search.Add(new SearchValueData { Name = "ReferralID", Value = searchModel.ReferralID.ToString() });
            search.Add(new SearchValueData { Name = "ReferralAssessmentID", Value = searchModel.ReferralAssessmentID.ToString() });

            if (searchModel.StartDate.HasValue)
                search.Add(new SearchValueData { Name = "StartDate", Value = searchModel.StartDate.Value.ToString(Constants.DbDateFormat) });

            if (searchModel.EndDate.HasValue)
                search.Add(new SearchValueData { Name = "EndDate", Value = searchModel.EndDate.Value.ToString(Constants.DbDateFormat) });


            #endregion


            List<ReferralAssessmentReviewListModel> assessmentReviews = GetEntityList<ReferralAssessmentReviewListModel>(StoredProcedure.DeleteReferralAssessmentReview, search);

            List<ReferralAssessmentReviewGraphModel> graphModel = new List<ReferralAssessmentReviewGraphModel>();

            if (assessmentReviews.Any())
            {
                graphModel = assessmentReviews.Select(m => new ReferralAssessmentReviewGraphModel
                {
                    title = m.AssessmentDate.ToString(ConfigSettings.ClientSideDateFormat),
                    data = m.GetDataPoints()
                }).ToList();
            }

            response.Data = new ReferralAssessmentReviewDetail
            {
                GraphSeriesList = graphModel,
                ReferralAssessmentList = assessmentReviews
            };
            response.IsSuccess = true;
            response.Message = Resource.AnsellCaseyDeletedMessage;
            return response;
        }

        //public ServiceResponse SaveAssessmentResult(string file, string fileName, long referralID, long loggedInID)
        //{
        //    ServiceResponse response = new ServiceResponse();

        //    if (!string.IsNullOrEmpty(file))
        //    {
        //        List<SearchValueData> searchList = new List<SearchValueData>()
        //            {
        //                new SearchValueData {Name = "FileName", Value = fileName},
        //                new SearchValueData {Name = "FilePath", Value = file},
        //                new SearchValueData {Name = "ReferralID ", Value = referralID.ToString() },
        //                new SearchValueData {Name = "LoggedInUserID", Value = loggedInID.ToString()},
        //                new SearchValueData {Name = "SystemID ", Value = HttpContext.Current.Request.UserHostAddress },
        //            };

        //        GetScalar(StoredProcedure.SaveReferralDocument, searchList);


        //        AmazonFileUpload az = new AmazonFileUpload();
        //        response.Data = az.GetPreSignedUrl(ConfigSettings.ZarephathBucket, file);
        //        response.IsSuccess = true;
        //        return response;
        //    }
        //    response.Message = Resource.FileUploadFailedNoFileSelected;
        //    return response;
        //}
        #endregion

        #region Referral Outcome and Measurements

        public ServiceResponse SetReferralOutcomeMeasurement(long referralID, long referralOutcomeMeasurementID, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();


            ReferralOutcomeMeasurement data = GetEntity<ReferralOutcomeMeasurement>(StoredProcedure.ReferralOutcomeMeasurement,
                                                    new List<SearchValueData>
                                                        {
                                                            new SearchValueData{Name = "ReferralID",Value = referralID.ToString()},
                                                            new SearchValueData{Name = "ReferralOutcomeMeasurementID",Value = referralOutcomeMeasurementID.ToString()},
                                                            new SearchValueData{Name = "LoginID",Value = loggedInUserID.ToString()},
                                                        }) ?? new ReferralOutcomeMeasurement();

            response.IsSuccess = true;
            response.Data = data;
            return response;
        }

        public ServiceResponse SaveReferralOutcomeMeasurement(ReferralOutcomeMeasurement model, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();

            ReferralOutcomeMeasurement tempModel = GetEntity<ReferralOutcomeMeasurement>(new List<SearchValueData>
                    {
                        new SearchValueData {Name = "ReferralID", Value = model.ReferralID.ToString(),IsEqual = true},
                        new SearchValueData{Name = "OutcomeMeasurementDate",Value = model.OutcomeMeasurementDate.ToString(Constants.DbDateFormat),IsEqual = true},
                        new SearchValueData{Name = "ReferralOutcomeMeasurementID",Value = model.ReferralOutcomeMeasurementID.ToString(),IsNotEqual = true},
                        new SearchValueData{Name = "IsDeleted",Value = "0"}
                    });
            if (tempModel != null)
            {
                response.IsSuccess = false;
                response.Message = Resource.ReferralOutComeMeasurementExist;
                return response;
            }
            return SaveObject(model, loggedInUserID, Resource.ReferralOutComeMeasurementSavedSuccessfully);
        }

        public ServiceResponse GetReferralOutcomeMeasurementList(SearchReferralOutcomeMeasurement searchModel)
        {
            ServiceResponse response = new ServiceResponse();

            #region Set SearchField

            List<SearchValueData> search = new List<SearchValueData>();
            search.Add(new SearchValueData { Name = "ReferralID", Value = searchModel.ReferralID.ToString() });
            if (searchModel.StartDate.HasValue)
                search.Add(new SearchValueData { Name = "StartDate", Value = searchModel.StartDate.Value.ToString(Constants.DbDateFormat) });
            if (searchModel.EndDate.HasValue)
                search.Add(new SearchValueData { Name = "EndDate", Value = searchModel.EndDate.Value.ToString(Constants.DbDateFormat) });

            #endregion


            List<ReferralOutcomeMeasurement> outcomeMeasurementsReviews = GetEntityList<ReferralOutcomeMeasurement>(StoredProcedure.GetReferralOutcomeMeasurement, search);

            List<ReferralOutcomeMeasurementGraphModel> graphModel = new List<ReferralOutcomeMeasurementGraphModel>();

            if (outcomeMeasurementsReviews.Any())
            {
                graphModel = outcomeMeasurementsReviews.Select(m => new ReferralOutcomeMeasurementGraphModel()
                {
                    title = m.OutcomeMeasurementDate.ToString(ConfigSettings.ClientSideDateFormat),
                    data = m.GetDataPoints()
                }).ToList();
            }



            response.Data = new ReferralOutcomeMeasurementDetail
            {
                GraphSeriesOutcomeMeasurementList = graphModel,
                ReferralOutcomeMeasurementList = outcomeMeasurementsReviews
            };
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse DeleteReferralOutcomeMeasurement(SearchReferralOutcomeMeasurement searchModel)
        {
            ServiceResponse response = new ServiceResponse();

            #region Set SearchField

            List<SearchValueData> search = new List<SearchValueData>();
            search.Add(new SearchValueData { Name = "ReferralID", Value = searchModel.ReferralID.ToString() });
            search.Add(new SearchValueData { Name = "ReferralOutcomeMeasurementID", Value = searchModel.ReferralOutcomeMeasurementID.ToString() });
            if (searchModel.StartDate.HasValue)
                search.Add(new SearchValueData { Name = "StartDate", Value = searchModel.StartDate.Value.ToString(Constants.DbDateFormat) });
            if (searchModel.EndDate.HasValue)
                search.Add(new SearchValueData { Name = "EndDate", Value = searchModel.EndDate.Value.ToString(Constants.DbDateFormat) });

            #endregion


            List<ReferralOutcomeMeasurement> outcomeMeasurementsReviews = GetEntityList<ReferralOutcomeMeasurement>(StoredProcedure.DeleteReferralOutcomeMeasurement, search);

            List<ReferralOutcomeMeasurementGraphModel> graphModel = new List<ReferralOutcomeMeasurementGraphModel>();

            if (outcomeMeasurementsReviews.Any())
            {
                graphModel = outcomeMeasurementsReviews.Select(m => new ReferralOutcomeMeasurementGraphModel()
                {
                    title = m.OutcomeMeasurementDate.ToString(ConfigSettings.ClientSideDateFormat),
                    data = m.GetDataPoints()
                }).ToList();
            }



            response.Data = new ReferralOutcomeMeasurementDetail
            {
                GraphSeriesOutcomeMeasurementList = graphModel,
                ReferralOutcomeMeasurementList = outcomeMeasurementsReviews
            };
            response.IsSuccess = true;
            response.Message = Resource.OutComeDeletedMessage;
            return response;
        }

        #endregion

        #region Referral Monthly Summary

        public ServiceResponse SetReferralMonthlySummary(long referralID, long referralMonthlySummariesID, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();

            ReferralMonthlySummaryModel data = GetMultipleEntity<ReferralMonthlySummaryModel>(StoredProcedure.ReferralMonthlySummary,
                                                    new List<SearchValueData>
                                                        {
                                                            new SearchValueData{Name = "ReferralID",Value = referralID.ToString()},
                                                            new SearchValueData{Name = "ReferralMonthlySummariesID",Value = referralMonthlySummariesID.ToString()},
                                                            new SearchValueData{Name = "LoginID",Value = loggedInUserID.ToString()},
                                                        });


            //ReferralMonthlySummary data = GetEntity<ReferralMonthlySummary>(StoredProcedure.ReferralMonthlySummary,
            //                                        new List<SearchValueData>
            //                                            {
            //                                                new SearchValueData{Name = "ReferralID",Value = referralID.ToString()},
            //                                                new SearchValueData{Name = "ReferralMonthlySummariesID",Value = referralMonthlySummariesID.ToString()},
            //                                                new SearchValueData{Name = "LoginID",Value = loggedInUserID.ToString()},
            //                                            }) ?? new ReferralMonthlySummary();

            data.ReferralMonthlySummary.BreakfastIds = data.ReferralMonthlySummary.Breakfast == null ? null : data.ReferralMonthlySummary.Breakfast.Split(',').ToList();
            data.ReferralMonthlySummary.LunchIds = data.ReferralMonthlySummary.Lunch == null ? null : data.ReferralMonthlySummary.Lunch.Split(',').ToList();
            data.ReferralMonthlySummary.DinnerIds = data.ReferralMonthlySummary.Dinner == null ? null : data.ReferralMonthlySummary.Dinner.Split(',').ToList();
            data.ReferralMonthlySummary.MoodforThroughoutWeekendIds = data.ReferralMonthlySummary.MoodforThroughoutWeekend == null ? null : data.ReferralMonthlySummary.MoodforThroughoutWeekend.Split(',').ToList();
            data.ReferralMonthlySummary.CoordinationofCareatDropOffIds = data.ReferralMonthlySummary.CoordinationofCareatDropOffOption == null ? null : data.ReferralMonthlySummary.CoordinationofCareatDropOffOption.Split(',').ToList();
            data.ReferralMonthlySummary.CoordinationofCareatPickupIds = data.ReferralMonthlySummary.CoordinationofCareatPickupOption == null ? null : data.ReferralMonthlySummary.CoordinationofCareatPickupOption.Split(',').ToList();

            response.IsSuccess = true;
            response.Data = data;
            return response;
        }

        public ServiceResponse SaveReferralMonthlySummary(ReferralMonthlySummary model, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();

            model.Breakfast = model.BreakfastIds == null ? null : String.Join(",", model.BreakfastIds);
            model.Lunch = model.LunchIds == null ? null : String.Join(",", model.LunchIds);
            model.Dinner = model.DinnerIds == null ? null : String.Join(",", model.DinnerIds);
            model.MoodforThroughoutWeekend = model.MoodforThroughoutWeekendIds == null ? null : String.Join(",", model.MoodforThroughoutWeekendIds);
            model.CoordinationofCareatDropOffOption = model.CoordinationofCareatDropOffIds == null ? null : String.Join(",", model.CoordinationofCareatDropOffIds);
            model.CoordinationofCareatPickupOption = model.CoordinationofCareatPickupIds == null ? null : String.Join(",", model.CoordinationofCareatPickupIds);


            ReferralMonthlySummary tempModel = GetEntity<ReferralMonthlySummary>(new List<SearchValueData>
                    {
                        new SearchValueData {Name = "ReferralID", Value = model.ReferralID.ToString(),IsEqual = true},
                        new SearchValueData{Name = "MonthlySummaryStartDate",Value = model.MonthlySummaryStartDate.ToString(Constants.DbDateFormat),IsEqual = true},
                        new SearchValueData{Name = "ReferralMonthlySummariesID",Value = model.ReferralMonthlySummariesID.ToString(),IsNotEqual = true},
                        //new SearchValueData{Name = "IsDeleted",Value = "0"}
                    });

            if (tempModel != null)
            {
                response.IsSuccess = false;
                response.Message = Resource.ReferralMonthlySummaryExist;
                return response;
            }
            return SaveObject(model, loggedInUserID, Resource.ReferralMonthlySummarySavedSuccessfully);
        }

        public ServiceResponse GetReferralMonthlySummaryList(SearchReferralMonthlySummary searchModel, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();

            #region Set SearchField

            List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);
            SetSearchFilterForReferralMonthlySummaryList(searchModel, searchList);

            #region SCRAP
            //searchList.Add(new SearchValueData { Name = "ReferralID", Value = searchModel.ReferralID.ToString() });
            //searchList.Add(new SearchValueData { Name = "FacilityID", Value = searchModel.FacilityID.ToString() });
            //searchList.Add(new SearchValueData { Name = "RegionID", Value = searchModel.RegionID.ToString() });
            //searchList.Add(new SearchValueData { Name = "ClientName", Value = Convert.ToString(searchModel.ClientName) });
            //searchList.Add(new SearchValueData { Name = "CreatedBy", Value = searchModel.CreatedBy.ToString() });
            //if (searchModel.StartDate.HasValue)
            //    searchList.Add(new SearchValueData { Name = "StartDate", Value = searchModel.StartDate.Value.ToString(Constants.DbDateFormat) });
            //if (searchModel.EndDate.HasValue)
            //    searchList.Add(new SearchValueData { Name = "EndDate", Value = searchModel.EndDate.Value.ToString(Constants.DbDateFormat) });

            //searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));
            #endregion

            List<ReferralMonthlySummaryListModel> totalData = GetEntityList<ReferralMonthlySummaryListModel>(StoredProcedure.GetReferralMonthlySummaryList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ReferralMonthlySummaryListModel> getEmployeeList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = getEmployeeList;
            response.IsSuccess = true;
            return response;

            #endregion
        }

        public ServiceResponse DeleteReferralMonthlySummary(SearchReferralMonthlySummary searchReferralMonthlySummary, int pageIndex, int pageSize,
            string sortIndex, string sortDirection, long loggedInUserId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);
            SetSearchFilterForReferralMonthlySummaryList(searchReferralMonthlySummary, searchList);

            if (!string.IsNullOrEmpty(searchReferralMonthlySummary.ListOfIdsInCsv))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = searchReferralMonthlySummary.ListOfIdsInCsv });
            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserId) });

            List<ReferralMonthlySummaryListModel> totalData = GetEntityList<ReferralMonthlySummaryListModel>(StoredProcedure.DeleteReferralMonthlySummary, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ReferralMonthlySummaryListModel> list = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = list;
            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.ReferralMonthlySummary);
            return response;
        }

        private static void SetSearchFilterForReferralMonthlySummaryList(SearchReferralMonthlySummary searchModel, List<SearchValueData> searchList)
        {
            searchList.Add(new SearchValueData { Name = "ReferralID", Value = searchModel.ReferralID.ToString() });
            searchList.Add(new SearchValueData { Name = "FacilityID", Value = searchModel.FacilityID.ToString() });
            searchList.Add(new SearchValueData { Name = "RegionID", Value = searchModel.RegionID.ToString() });
            searchList.Add(new SearchValueData { Name = "ClientName", Value = Convert.ToString(searchModel.ClientName) });
            searchList.Add(new SearchValueData { Name = "CreatedBy", Value = searchModel.CreatedBy.ToString() });
            if (searchModel.StartDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = searchModel.StartDate.Value.ToString(Constants.DbDateFormat) });
            if (searchModel.EndDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = searchModel.EndDate.Value.ToString(Constants.DbDateFormat) });
            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchModel.IsDeleted) });
        }


        public ServiceResponse FindScheduleWithFaciltyAndServiceDate(FindScheduleWithFaciltyAndServiceDateModel model)
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData {Name = "StartDate", Value = model.StartDate},
                    new SearchValueData {Name = "EndDate", Value = model.EndDate},
                    new SearchValueData {Name = "FacilityID", Value = model.FacilityID},
                    new SearchValueData {Name = "ReferralID", Value = model.ReferralID},
                    new SearchValueData {Name = "IsDeleted", Value = "0"}
                };

            //List<ScheduleMaster> scheduleList = GetEntityList<ScheduleMaster>(searchParam);
            response.Data = GetEntityList<ScheduleMaster>(searchParam).Count > 0;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse GetFacilityList()
        {
            ServiceResponse response = new ServiceResponse();
            List<NameValueData> totalData = GetEntityList<NameValueData>(StoredProcedure.GetFailityListForDDL, null);
            response.Data = totalData;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse MonthlySummaryList()
        {
            ServiceResponse response = new ServiceResponse();
            AddReferralModel model = new AddReferralModel();
            model.EmployeeList = GetEntityList<EmployeeDropDownModel>(StoredProcedure.GetEmployeeListForDDL, null);
            model.RegionList = GetEntityList<Region>();
            model.DeleteFilter = Common.SetDeleteFilter();
            model.BindMealsandSummaryofFood = Common.SetThroughoutWeekend();
            model.EnumCoordinationofCare = Common.SetCoordinationofCare();
            model.SummaryofFood = Common.SetSummaryofFood();
            model.Coordinationofcare = Common.SetCoordinationofcare();
            response.Data = model;
            response.IsSuccess = true;
            return response;
        }



        #endregion

        #region Referral Group MonthlySummary

        public ServiceResponse FillGroupMonthlySummaryModel(long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData {Name = "LoggedInUserID", Value = loggedInUserID.ToString()}
                };
            GroupMonthlySummaryModel summaryModel = GetMultipleEntity<GroupMonthlySummaryModel>(StoredProcedure.FillGroupMonthlySummaryModel, searchParam);

            summaryModel.CommonMonthlySummary.Signature = summaryModel.SignatureDetails.SignaturePath;
            summaryModel.CommonMonthlySummary.CompletedBy = summaryModel.SignatureDetails.SignatureBy;

            summaryModel.BindMealsandSummaryofFood = Common.SetThroughoutWeekend();
            summaryModel.EnumCoordinationofCare = Common.SetCoordinationofCare();
            summaryModel.SummaryofFood = Common.SetSummaryofFood();
            summaryModel.Coordinationofcare = Common.SetCoordinationofcare();

            response.Data = summaryModel;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse SearchClientForMonthlySummary(SearchClientForMonthlySummary searchGroupNoteClient, List<long> ignoreClientID)
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ClientName", Value = searchGroupNoteClient.ClientName},
                    new SearchValueData {Name = "PayorID", Value = searchGroupNoteClient.PayorID.ToString()},
                    new SearchValueData {Name = "FacilityID", Value = searchGroupNoteClient.FacilityID.ToString()}
                };

            if (ignoreClientID != null)
                searchParam.Add(new SearchValueData { Name = "IgnoreClientID", Value = string.Join(",", ignoreClientID) });

            if (searchGroupNoteClient.EndDate.HasValue)
                searchParam.Add(new SearchValueData { Name = "EndDate", Value = searchGroupNoteClient.EndDate.Value.ToString(Constants.DbDateFormat) });

            if (searchGroupNoteClient.StartDate.HasValue)
                searchParam.Add(new SearchValueData { Name = "StartDate", Value = searchGroupNoteClient.StartDate.Value.ToString(Constants.DbDateFormat) });

            if (searchGroupNoteClient.PageSize > 0)
                searchParam.Add(new SearchValueData { Name = "PageSize", Value = Convert.ToString(searchGroupNoteClient.PageSize) });

            ClientForGroupNote clientList =
                GetMultipleEntity<ClientForGroupNote>(StoredProcedure.SearchClientForGroupMonthlySummary, searchParam);

            response.IsSuccess = true;
            response.Data = clientList;
            return response;
        }
        public ServiceResponse SaveMultipleMonthlySummary(List<ReferralMonthlySummary> monthlySummaries, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();
            GroupMonthlySummaryStatus noteMsg = new GroupMonthlySummaryStatus();
            noteMsg.ReferralMonthlySummariesIDs = new List<long>();

            var logFileName = "GroupMonthlySummary_" + DateTime.Now.TimeOfDay.Ticks.ToString();
            string logpath = ConfigSettings.LogPath + ConfigSettings.GroupNoteLog;
            string msg = string.Empty;
            foreach (var summary in monthlySummaries)
            {
                ServiceResponse res = SaveReferralMonthlySummary(summary, loggedInUserID);
                if (res.IsSuccess)
                {

                    res.Message = string.IsNullOrEmpty(res.Message) ? Resource.Passed : res.Message;
                    noteMsg.ReferralMonthlySummariesIDs.Add(summary.ReferralMonthlySummariesID);

                    noteMsg.SuccessCount += 1;
                    noteMsg.SuccessMsg += "<li><b>" + summary.Name + "</b> (" + @Resource.AHCCCSID + ": " + summary.AHCCCSID +
                                          ") :- " + res.Message + "</li>";
                }
                else
                {
                    res.Message = string.IsNullOrEmpty(res.Message) ? Resource.NoteFailed : res.Message;
                    noteMsg.ErrorCount += 1;
                    noteMsg.ErrorMsg += "<li><b>" + summary.Name + "</b> (" + @Resource.AHCCCSID + ": " + summary.AHCCCSID +
                                        ") :- " + res.Message + "</li>";
                }
                msg += "<br><b>" + summary.Name + "</b> (" + @Resource.AHCCCSID + ": " + summary.AHCCCSID +
                                          ") :- " + res.Message;
            }
            Common.CreateLogFile(msg, logFileName, logpath);
            noteMsg.SuccessMsg += "</ul>";
            noteMsg.ErrorMsg += "</ul>";
            response.Data = noteMsg;
            response.IsSuccess = true;
            //response.Message = Resource.GroupNoteSavedSuccessfully;
            return response;
        }


        #endregion End Referral Group MonthlySummary

        #region  AutoCompleter for Referral Sibling

        public List<GetReferralInfoList> GetReferralInfo(int pageSize, string ignoreIds, string searchText = null)
        {
            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData {Name = "SearchText", Value = searchText},
                    new SearchValueData{Name = "IgnoreIds",Value = ignoreIds},
                    new SearchValueData {Name = "PageSize", Value =pageSize.ToString()}
                };

            return GetEntityList<GetReferralInfoList>(StoredProcedure.GetSiblingPageReferrals, searchParam);
        }

        public ServiceResponse DeletePreference(ReferralPreferenceModel model)
        {
            ServiceResponse response = new ServiceResponse();
            if (model.ReferralPreferenceID > 0)
            {
                var searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData() { Name = "ReferralPreferenceID", Value = Convert.ToString(model.ReferralPreferenceID), IsEqual = true });
                GetScalar(StoredProcedure.DeleteReferralPreference, searchList);
                response.IsSuccess = true;
            }

            if (!response.IsSuccess)
                response.Message = Resource.ExceptionMessage;

            return response;
        }
        #endregion

        #endregion

        #region In HOME CARE Data Provider Code

        #region ADD Referral
        public ServiceResponse HC_ResolveReferralInternalMessage(long referralInternalMessageId, long referralId, string resolvedComment, long loggedInUserId)
        {
            var response = new ServiceResponse();
            if (referralInternalMessageId > 0 && referralId > 0)
            {
                string customWhere = string.Format("(ReferralInternalMessageID={0} and ReferralID={1})", referralInternalMessageId, referralId);
                ReferralInternalMessage referralInternalMessageDetail = GetEntity<ReferralInternalMessage>(null, customWhere);
                referralInternalMessageDetail.ResolveDate = DateTime.UtcNow;
                referralInternalMessageDetail.ResolvedComment = string.IsNullOrEmpty(resolvedComment) ? null : resolvedComment;
                referralInternalMessageDetail.IsResolved = true;
                response = SaveObject(referralInternalMessageDetail, loggedInUserId, Resource.InternalMessageResolvedSuccess);

                //#region Create Note

                //INoteDataProvider iNoteDataProvider = new NoteDataProvider();
                //iNoteDataProvider.SaveGeneralNote(referralId, referralInternalMessageDetail.Note, Resource.InternalMessaging, loggedInUserId, null, Constants.Staff, null);

                //#endregion

                response.IsSuccess = true;
            }
            else
            {
                response.Message = Resource.ResolveNoteError;
            }
            return response;
        }

        public ServiceResponse HC_SetAddReferralPage(long referralID, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();

            HC_ReferralModel referralModel = new HC_ReferralModel();

            var CareType = Convert.ToUInt16(Common.DDType.CareType);

            List<SearchValueData> searchList = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralID", Value = referralID.ToString()},
                    new SearchValueData { Name = "PreferenceType_Preference", Value = Convert.ToString(Preference.PreferenceKeyType.Preference) },
                    new SearchValueData { Name = "PreferenceType_Skill", Value = Convert.ToString(Preference.PreferenceKeyType.Skill) },
                    new SearchValueData { Name = "DDType_CareType", Value = Convert.ToString((int)Common.DDType.CareType)},
                    new SearchValueData { Name = "DDType_AdmissionType", Value = Convert.ToString((int)Common.DDType.AdmissionType)},
                    new SearchValueData { Name = "DDType_AdmissionSource", Value = Convert.ToString((int)Common.DDType.AdmissionSource)},
                    new SearchValueData { Name = "DDType_PatientStatus", Value = Convert.ToString((int)Common.DDType.PatientStatus)},
                    new SearchValueData { Name = "DDType_FacilityCode", Value = Convert.ToString((int)Common.DDType.FacilityCode)},
                    new SearchValueData { Name = "DDType_VisitType", Value = Convert.ToString((int)Common.DDType.VisitType)},
                    new SearchValueData { Name = "DDType_PatientSystemStatus", Value = Convert.ToString((int)Common.DDType.PatientSystemStatus)},
                    new SearchValueData { Name = "DDType_PatientFrequencyCode", Value = Convert.ToString((int)Common.DDType.PatientFrequencyCode)},
                    new SearchValueData { Name = "DDType_Gender", Value = Convert.ToString((int)Common.DDType.Gender)},
                    new SearchValueData { Name = "DDType_LanguagePreference", Value = Convert.ToString((int)Common.DDType.LanguagePreference)},
                    new SearchValueData { Name = "DDType_BeneficiaryType", Value = Convert.ToString((int)Common.DDType.BeneficiaryType)},
                    new SearchValueData { Name = "DDType_RevenueCode", Value =Convert.ToString((int)Common.DDType.RevenueCode)}
                // new SearchValueData { Name = "DDType_PriorAuthorizationFrequency", Value = Convert.ToString((int)Common.DDType.PriorAuthorizationFrequency)}

                 };
            referralModel.AddReferralModel = GetMultipleEntity<HC_AddReferralModel>(StoredProcedure.HC_SetAddReferralPage, searchList);
            var a = referralModel.AddReferralModel.GenderList.Where(m => m.Name == "M").Select(m => m.Value);
            if (referralID > 0)
            {
                if (referralModel.AddReferralModel.Referral.PhysicianID == null || (Convert.ToString(referralModel.AddReferralModel.Referral.PhysicianID)) == "")
                {
                    referralModel.AddReferralModel.Referral.PhysicianID = 0;
                }
                List<SearchValueData> sList = new List<SearchValueData>
                {
                    new SearchValueData {Name = "PhysicianID", Value = Convert.ToString(referralModel.AddReferralModel.Referral.PhysicianID)},
                };
                referralModel.AddReferralModel.PhysicianModel = GetEntity<Physician>(sList);
            }

            if (referralModel.AddReferralModel.PhysicianModel == null)
            {
                referralModel.AddReferralModel.Referral.PhysicianID = null;
                referralModel.AddReferralModel.PhysicianModel = new Physician();
            }

            if (referralModel.AddReferralModel.SearchReferralPayorMapping == null)
            {
                referralModel.AddReferralModel.SearchReferralPayorMapping = new SearchReferralPayorMapping();
            }

            if (referralModel.AddReferralModel.ReferralBillingSetting == null)
            {
                referralModel.AddReferralModel.ReferralBillingSetting = new ReferralBillingSetting();
            }

            referralModel.AddReferralModel.PrecedenceList = Common.SetPrecedenceList();
            if (referralModel.AddReferralModel.DosageTimes == null)
                referralModel.AddReferralModel.DosageTimes = new List<DosageTimeDetail>();

            //referralModel.AddReferralModel.FacilityCodeList = Common.SetPOSList();
            //referralModel.AddReferralModel.AdmissionTypeList = Common.SetAdmissionTypeList();
            //referralModel.AddReferralModel.AdmissionSourceList = Common.SetAdmissionSourceList();
            //referralModel.AddReferralModel.PatientDischargeStatusList = Common.SetPatientDischargeStatusList();


            #region Compliance Tab GroupBy for Section SubSection
            //----------------------------------------List for Internal Document--------------------------------------------------------
            List<DocumentSection> internalDocSectionList = referralModel.AddReferralModel.InternalDocumentList.ToList().GroupBy(c => new
            {
                c.SectionName
            }).Select(grp => new DocumentSection
            {
                SectionName = grp.Key.SectionName,
                DocumentList = grp.ToList()
            }).ToList();

            //foreach (DocumentSection docSection in internalDocSectionList)
            //{
            //    List<DocumentSubSection> internalDocSubSectionList = docSection.DocumentList.ToList().GroupBy(c => new
            //    {
            //        c.SubSectionName
            //    }).Select(grp => new DocumentSubSection
            //    {
            //        SubSectionName = grp.Key.SubSectionName,
            //        DocumentList = grp.ToList()
            //    }).ToList();

            //    docSection.DocumentSubSectionList.AddRange(internalDocSubSectionList);
            //}

            referralModel.AddReferralModel.InternalDocumentSectionList = internalDocSectionList;

            //-------------------------------------List for External Document-----------------------------------------------------------
            List<DocumentSection> externalDocSectionList = referralModel.AddReferralModel.ExternalDocumentList.ToList().GroupBy(c => new
            {
                c.SectionName
            }).Select(grp => new DocumentSection
            {
                SectionName = grp.Key.SectionName,
                DocumentList = grp.ToList()
            }).ToList();

            //foreach (DocumentSection docSection in externalDocSectionList)
            //{
            //    List<DocumentSubSection> ecternalDocSubSectionList = docSection.DocumentList.ToList().GroupBy(c => new
            //    {
            //        c.SubSectionName
            //    }).Select(grp => new DocumentSubSection
            //    {
            //        SubSectionName = grp.Key.SubSectionName,
            //        DocumentList = grp.ToList()
            //    }).ToList();

            //    docSection.DocumentSubSectionList.AddRange(ecternalDocSubSectionList);
            //}

            referralModel.AddReferralModel.ExternalDocumentSectionList = externalDocSectionList;


            #endregion



            if (referralModel.AddReferralModel.PreferenceList == null || referralModel.AddReferralModel.PreferenceList.Count == 0)
                referralModel.AddReferralModel.PreferenceList = new List<ReferralPreferenceModel>();
            if (referralModel.AddReferralModel.SkillList == null || referralModel.AddReferralModel.SkillList.Count == 0)
                referralModel.AddReferralModel.SkillList = new List<Preference>();
            if (referralModel.AddReferralModel.ReferralSkillList == null || referralModel.AddReferralModel.ReferralSkillList.Count == 0)
                referralModel.AddReferralModel.ReferralSkillList = new List<string>();


            if (referralModel.AddReferralModel.Referral != null && referralModel.AddReferralModel.Referral.ReferralID > 0)
            {
                if (Common.HasPermission(Constants.Permission_View_All_Referral) == false &&
                    Common.HasPermission(Constants.Permission_View_Assinged_Referral))
                {
                    if (referralModel.AddReferralModel.Referral.Assignee != loggedInUserID)
                    {
                        response.ErrorCode = Constants.ErrorCode_AccessDenied;
                        return response;
                    }
                }
                referralModel.AddReferralModel.Referral.ReferralCaseloadIDs =
                    !string.IsNullOrEmpty(referralModel.AddReferralModel.Referral.SetSelectedReferralCaseloadIDs)
                    ? referralModel.AddReferralModel.Referral.SetSelectedReferralCaseloadIDs.Split(',').ToList() : null;

            }
            if (referralModel.AddReferralModel.ReferralPayorMapping == null)
                referralModel.AddReferralModel.ReferralPayorMapping = new ReferralPayorMapping();
            //referralModel.AddReferralModel.GenderList = Common.SetGenderList();
            referralModel.AddReferralModel.PrivateRoomList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.ROITypes = Common.SetROIType();
            referralModel.AddReferralModel.PrimaryContactGuardianList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.DCSGuardianList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.EmergencyContactList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.NoticeProviderOnFileList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.SignatureNeededList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.CareConsentList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.SelfAdministrationList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.HealthInformationList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.AdmissionRequirementList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.AdmissionOrientationList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.VoiceMailList = Common.SetAutorizedNotAutorizedForBoolean();
            referralModel.AddReferralModel.PermissionEmailList = Common.SetAutorizedNotAutorizedForBoolean();
            referralModel.AddReferralModel.PermissionSMSList = Common.SetAutorizedNotAutorizedForBoolean();
            referralModel.AddReferralModel.NetworkCrisisPlanList = Common.SetNameValueDataForYesNoNAData();
            referralModel.AddReferralModel.ZarephathCrisisPlanList = Common.SetNameValueDataForYesNoNAData();
            referralModel.AddReferralModel.PHIList = Common.SetYesNoListForBoolean();

            referralModel.AddReferralModel.ROIList = Common.SetNameValueDataForYesNoNAData();

            referralModel.AddReferralModel.ZSPRespiteList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.ZSPLifeSkillsList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.ZSPCounsellingList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.ZSPRespiteGuardianSignatureList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.ZSPRespiteBHPSignedList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.ZSPLifeSkillsGuardianSignatureList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.ZSPLifeSkillsBHPSignedList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.ZSPCounsellingGuardianSignatureList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.ZSPCounsellingBHPSignedList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.NetworkServicePlanList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.NetworkServiceGuardianSignatureList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.NetworkServiceBHPSignedList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.NSPSPidentifyServiceList = Common.SetNameValueDataForYesNoNAData();
            referralModel.AddReferralModel.BXAssessmentList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.BXAssessmentBHPSignedList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.DemographicList = Common.SetNameValueDataForYesNoNAData();
            referralModel.AddReferralModel.SNCDList = Common.SetNameValueDataForYesNoNAData();
            referralModel.AddReferralModel.ACAssessmentList = Common.SetYesNoListForBoolean();
            referralModel.AddReferralModel.DocumentKind = Common.SetDocumentKindOf();
            referralModel.AddReferralModel.PermissionMailList = Common.SetAutorizedNotAutorizedForBoolean();

            referralModel.AddReferralModel.BindMealsandSummaryofFood = Common.SetThroughoutWeekend();
            referralModel.AddReferralModel.EnumCoordinationofCare = Common.SetCoordinationofCare();
            referralModel.AddReferralModel.SummaryofFood = Common.SetSummaryofFood();
            referralModel.AddReferralModel.Coordinationofcare = Common.SetCoordinationofcare();
            referralModel.AddReferralModel.Coordinationofcare = Common.SetCoordinationofcare();
            referralModel.AddReferralModel.DeleteFilter = Common.SetDeleteFilter();

            //referralModel.AddReferralModel.ContactTypeList.RemoveAll(x => referralModel.AddReferralModel.ContactInformationList.Any(y => y.ContactTypeID == x.ContactTypeID));

            if (referralID > 0)
            {
                referralModel.AddReferralModel.IsEditMode = true;
                referralModel.AddReferralModel.AmazonSettingModel =
                    AmazonFileUpload.GetAmazonModelForClientSideUpload(loggedInUserID,
                                                                       ConfigSettings.AmazoneUploadPath +
                                                                       ConfigSettings.ReferralUploadPath +
                                                                       referralModel.AddReferralModel.Referral.AHCCCSID +
                                                                       "/", ConfigSettings.PrivateAcl);
                if (referralModel.AddReferralModel.Referral != null && referralModel.AddReferralModel.Referral.ReferralID > 0)
                {
                    referralModel.AddReferralModel.Referral.IsEditMode = true;
                    //referralModel.AddReferralModel.Referral.Gender = referralModel.AddReferralModel.Referral.Gender == "M" ? "1" : "2";
                    referralModel.AddReferralModel.Referral.Gender = referralModel.AddReferralModel.Referral.Gender;
                    referralModel.AddReferralModel.Referral.EncryptedReferralID = Crypto.Encrypt(Convert.ToString(referralID));


                    response.IsSuccess = true;
                }
                else
                {
                    response.ErrorCode = Constants.ErrorCode_NotFound;
                    return response;
                }
            }
            else
            {
                referralModel.AddReferralModel.IsEditMode = false;
                referralModel.AddReferralModel.Referral = new Referral
                {
                    Population = Constants.Child,
                    Title = Constants.RomanNumeral,
                    ZarephathCrisisPlan = Constants.N,
                    Demographic = Constants.N,
                    SNCD = Constants.N,
                    AROI = Constants.N,
                    NetworkCrisisPlan = Constants.N,
                    NSPSPidentifyService = Constants.NA,
                    PermissionForMail = true,
                    Assignee = loggedInUserID,
                    ReferralDate = DateTime.Now.Date,
                    ReferralSourceID = (int)EnumReferralSources.Other,
                    MondaySchedule = true,
                    TuesdaySchedule = true,
                    WednesdaySchedule = true,
                    ThursdaySchedule = true,
                    FridaySchedule = true,
                    SaturdaySchedule = true,
                    SundaySchedule = true,
                    BeneficiaryType = "1",
                    ReferralStatusID = (int)Common.ReferralStatusEnum.Inactive,


                    //SessionHelper.LoggedInID
                };

                response.IsSuccess = true;
            }


            #region Set Default Tab To Open


            if (Common.HasPermission(Constants.HC_Permission_Patient_AddUpdate))
                referralModel.AddReferralModel.DefaultTab = Constants.HashUrl_ReferralDetails;
            else if (Common.HasPermission(Constants.HC_Permission_Patient_Schedule))
                referralModel.AddReferralModel.DefaultTab = Constants.HashUrl_ReferralTimeslots;
            else if (Common.HasPermission(Constants.HC_Permission_Patient_Calendar))
                referralModel.AddReferralModel.DefaultTab = Constants.HashUrl_ReferralPatientCalender;

            //else if (Common.HasPermission(Constants.Permission_ReferralSparForm_View_AddUpdate))
            //    referralModel.AddReferralModel.DefaultTab = Constants.HashUrl_ReferralSparform;
            //else if (Common.HasPermission(Constants.Permission_ReviewMeasurement_All_View_AddUpdate))
            //    referralModel.AddReferralModel.DefaultTab = Constants.HashUrl_ReferralReviewMeasurement;
            //else if (Common.HasPermission(Constants.Permission_ReferralInternalMessaging_View_AddUpdate))
            //    referralModel.AddReferralModel.DefaultTab = Constants.HashUrl_ReferralInternalMessage;
            //else if (Common.HasPermission(Constants.Permission_Schedule_Hisotry))
            //    referralModel.AddReferralModel.DefaultTab = Constants.Permission_Schedule_Hisotry;
            //else if (Common.HasPermission(Constants.Permission_NoteList))
            //    referralModel.AddReferralModel.DefaultTab = Constants.HashUrl_ReferralNote;
            else
                referralModel.AddReferralModel.DefaultTab = Constants.HashUrl_ReferralAccessDenied;


            if (referralID == 0 && !Common.HasPermission(Constants.HC_Permission_Patient_AddUpdate))
                referralModel.AddReferralModel.DefaultTab = Constants.HashUrl_ReferralAccessDenied;



            #endregion


            response.Data = referralModel.AddReferralModel;
            response.IsSuccess = true;
            return response;
        }

        public List<Region> GetSearchRegion(int pageSize, string searchText = null)
        {
            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData {Name = "SearchText", Value = searchText},
                    new SearchValueData {Name = "PageSize", Value =pageSize.ToString()}
                };

            List<Region> model = GetEntityList<Region>(StoredProcedure.GetSearchRegion, searchParam) ?? new List<Region>();

            if (model.Count == 0 || model.Count(c => c.RegionName.ToLower() == searchText.ToLower()) == 0)
                model.Insert(0, new Region { RegionName = searchText });

            return model;

        }

        public bool ValidateReferralBillingAuthorization(long referralID, string authType = null)
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralID", Value = referralID.ToString()}
                };
            if (!String.IsNullOrWhiteSpace(authType))
            {
                searchParam.Add(new SearchValueData { Name = "AuthType", Value = authType });
            }

            response = GetEntityList<ServiceResponse>(StoredProcedure.HC_ValidateReferralBillingAuthorization, searchParam).FirstOrDefault();
            return response.IsSuccess;
        }


        public ServiceResponse HC_AddReferral(HC_AddReferralModel addReferralModel, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();
            List<ReferralPreferenceModel> PreferenceList = addReferralModel.PreferenceList;
            string preferenceList = string.Empty;


            if (addReferralModel.Referral != null)
            {
                bool isEditMode = addReferralModel.Referral.ReferralID > 0;

                #region Check UserName Exists
                var searchParameter = new List<SearchValueData>();
                searchParameter.Add(new SearchValueData { Name = "UserName", Value = addReferralModel.Referral.UserName, IsEqual = true });
                searchParameter.Add(new SearchValueData { Name = "ReferralID", Value = addReferralModel.Referral.ReferralID.ToString(), IsNotEqual = true });
                searchParameter.Add(new SearchValueData { Name = "IsDeleted", Value = "0", IsEqual = true });

                var EmployeeUniqueID = GetScalar(StoredProcedure.CheckReferralUserName, searchParameter);
                if (EmployeeUniqueID != null)
                {
                    response.Message = Resource.UserNameAlreadyExists;
                    return response;
                }
                else if (EmployeeUniqueID == null && !string.IsNullOrEmpty(addReferralModel.Referral.UserName))
                {
                    searchParameter.Clear();
                    searchParameter.Add(new SearchValueData { Name = "UserName", Value = addReferralModel.Referral.UserName, IsEqual = true });
                    var employee = GetEntity<Employee>(searchParameter);
                    if (employee != null && employee.EmployeeID > 0)
                    {
                        response.Message = Resource.UserNameAlreadyExists;
                        return response;
                    }
                }
                #endregion


                #region Server Side Validation Check for Referral
                var tempReferral = new Referral();

                if (addReferralModel.Referral.NewPassword != null)
                {
                    PasswordDetail passwordDetail = Common.CreatePassword(addReferralModel.Referral.NewPassword);
                    addReferralModel.Referral.Password = passwordDetail.Password;
                    addReferralModel.Referral.PasswordSalt = passwordDetail.PasswordSalt;
                }




                if (isEditMode)
                {
                    tempReferral = GetEntity<Referral>(addReferralModel.Referral.ReferralID);
                }

                if (addReferralModel.Referral.ReferralStatusID == null)
                {
                    addReferralModel.Referral.ReferralStatusID = isEditMode
                                                                     ? tempReferral.ReferralStatusID ?? (int)
                                                                       ReferralStatus.ReferralStatuses.New_Referral
                                                                     : (int)
                                                                       ReferralStatus.ReferralStatuses.New_Referral;
                }

                if (addReferralModel.Referral.Assignee == null)
                {
                    addReferralModel.Referral.Assignee = isEditMode
                                                                     ? tempReferral.Assignee ?? loggedInUserID
                                                                     : loggedInUserID;
                }

                // Check for the Age, if the age is greater than 18 then the referral cannot be created.
                //if (addReferralModel.Referral.Dob != null && addReferralModel.Referral.Dob.Value != null)
                //{
                //    int age = new DateTime(DateTime.Now.Subtract(addReferralModel.Referral.Dob.Value).Ticks).Year - 1;
                //    if (age > 18)
                //    {
                //        response.Message = Resource.ReferralCannotBeCreated;
                //        return response;
                //    }
                //}





                if (addReferralModel.Referral.IsSaveAsDraft.HasValue && !addReferralModel.Referral.IsSaveAsDraft.Value)
                {
                    // Check if there no primary contact and legal contact then show alert to user that referral cannot be created.
                    var result = addReferralModel.ContactInformationList.FirstOrDefault(q => q.ContactTypeID == (int)Common.ContactTypes.PrimaryPlacement);
                    //if (result != null)
                    //result = addReferralModel.ContactInformationList.FirstOrDefault(q => q.ContactTypeID != (int)Common.ContactTypes.LegalGuardian);

                    if (result == null)
                    {
                        response.Message = Resource.PatientContactRequired;
                        return response;
                    }



                }

                // Check for the Add/Edit Contact and accordingly update.                             

                #endregion

                #region Check Client Exist or not for New Referral Only. We are using AHCCCS ID. AHCCCS ID will unique for each client

                addReferralModel.Referral.IsEditMode = addReferralModel.Referral.ReferralID > 0;

                var searchParam = new List<SearchValueData>();
                searchParam.Add(new SearchValueData { Name = "AHCCCSID", Value = addReferralModel.Referral.AHCCCSID, IsEqual = true });
                //searchParam.Add(new SearchValueData { Name = "CISNumber", Value = addReferralModel.Referral.CISNumber, IsEqual = true });

                var clientInfo = isEditMode ? new Client() : GetEntity<Client>(searchParam) ?? new Client();


                #endregion

                #region Check for client with SAME AHCCCS ID and CIS Number does exist

                //searchParam.Clear();
                //searchParam.Add(new SearchValueData { Name = "AHCCCSID", Value = addReferralModel.Referral.AHCCCSID, IsEqual = true });
                //searchParam.Add(new SearchValueData { Name = "CISNumber", Value = addReferralModel.Referral.CISNumber, IsEqual = true });

                if (isEditMode)
                {
                    searchParam.Add(new SearchValueData { Name = "ClientID", Value = addReferralModel.Referral.ClientID.ToString(), IsNotEqual = true });
                }

                var referral = GetEntityList<Referral>(searchParam);
                if (referral.Any())
                {
                    response.Message = Resource.ClientAccountAlreadyExists;
                    return response;

                }
                //if (addReferralModel.Referral.IsBillable && !this.ValidateReferralBillingAuthorization(addReferralModel.Referral.ReferralID))
                //{
                //    response.Message = Resource.HCBillingAuthorizationValidation;
                //    return response;

                //}
                #endregion

                #region Add Client into Client table if Referral is new and No related client information found into existing Database

                if (!isEditMode && clientInfo.ClientID == 0)
                {
                    clientInfo = new Client
                    {
                        FirstName = addReferralModel.Referral.FirstName,
                        MiddleName = addReferralModel.Referral.MiddleName,
                        LastName = addReferralModel.Referral.LastName,
                        Dob = addReferralModel.Referral.Dob,
                        //Gender = addReferralModel.Referral.Gender == "1" ? Constants.Gender_Male : Constants.Gender_FeMale,
                        Gender = addReferralModel.Referral.Gender,
                        ClientNumber = addReferralModel.Referral.ClientNumber,
                        AHCCCSID = addReferralModel.Referral.AHCCCSID,
                        CISNumber = addReferralModel.Referral.CISNumber
                    };
                    SaveObject(clientInfo, loggedInUserID);
                }


                #region for Insert/Update CaseLoad Record
                // Commented after discussion with Jitendra
                //string customWhere = string.Format("(ReferralID={0})", addReferralModel.Referral.ReferralID);

                //List<ReferralCaseload> referralCaseload = GetEntityList<ReferralCaseload>(null, customWhere);
                //foreach (var item in referralCaseload)
                //{
                //    DeleteEntity<ReferralCaseload>(item.ReferralCaseloadID);
                //}
                //foreach (var id in addReferralModel.Referral.ReferralCaseloadIDs)
                //{
                //    ReferralCaseload aCaseload = new ReferralCaseload();
                //    aCaseload.ReferralID = addReferralModel.Referral.ReferralID;
                //    aCaseload.EmployeeID = Convert.ToInt64(id);
                //    aCaseload.StartDate = DateTime.Now;
                //    SaveObject(aCaseload, loggedInUserID);
                //}

                #endregion

                #endregion

                //#region Add Region(Location)
                //if (addReferralModel.Referral.RegionID == null)
                //{
                //    List<SearchValueData> param = new List<SearchValueData>
                //    {
                //        new SearchValueData {Name = "RegionName", Value = addReferralModel.Referral.RegionName}
                //    };
                //    long RegionID = (long)GetScalar(StoredProcedure.AddRegion, param);
                //    addReferralModel.Referral.RegionID = RegionID;
                //}
                //#endregion

                #region Add/Update Referral Related details

                #region Add/Update Referral
                addReferralModel.Referral.ClientID = clientInfo.ClientID > 0 ? clientInfo.ClientID : addReferralModel.Referral.ClientID;
                //addReferralModel.Referral.Gender = addReferralModel.Referral.Gender == "1" ? Constants.Gender_Male : Constants.Gender_FeMale;
                addReferralModel.Referral.Gender = addReferralModel.Referral.Gender;

                #region Set default value of dropdowns for save as draft
                if (addReferralModel.Referral.CaseManagerID == 0)
                {
                    addReferralModel.Referral.CaseManagerID = null;
                }
                if (addReferralModel.Referral.AgencyLocationID == 0)
                {
                    addReferralModel.Referral.AgencyLocationID = null;
                }
                if (addReferralModel.Referral.AgencyID == 0)
                {
                    addReferralModel.Referral.AgencyID = null;
                }
                if (addReferralModel.Referral.RegionID == null)
                {
                    addReferralModel.Referral.RegionID = null;
                }
                if (addReferralModel.Referral.ReferralStatusID == 0)
                {
                    addReferralModel.Referral.ReferralStatusID = null;
                }
                if (addReferralModel.Referral.DropOffLocation == 0)
                {
                    addReferralModel.Referral.DropOffLocation = null;
                }
                if (addReferralModel.Referral.PickUpLocation == 0)
                {
                    addReferralModel.Referral.PickUpLocation = null;
                }
                if (addReferralModel.Referral.FrequencyCodeID == 0)
                {
                    addReferralModel.Referral.FrequencyCodeID = null;
                }


                #endregion
                // This will save the information in the Referral Table

                if (isEditMode && tempReferral.ReferralStatusID == (int)ReferralStatus.ReferralStatuses.Active &&
                    addReferralModel.Referral.ReferralStatusID != (int)ReferralStatus.ReferralStatuses.Active)
                {
                    GetScalar(StoredProcedure.InactiveSchedule,
                              new List<SearchValueData>
                                  {
                                      new SearchValueData
                                          {
                                              Name = "ReferralID",
                                              Value = tempReferral.ReferralID.ToString()
                                          },
                                      new SearchValueData
                                          {
                                              Name = "CancelStatus",
                                              Value = ((int) ScheduleStatus.ScheduleStatuses.Cancelled).ToString()
                                          },
                                      new SearchValueData {Name = "Comment", Value = @Resource.InactiveScheduleComment},
                                      new SearchValueData {Name = "WhoCancel", Value = Constants.Office},
                                      new SearchValueData
                                          {
                                              Name = "ChangeStatus",
                                              Value = string.Join(",", new List<String>
                                                  {
                                                      ((int) ScheduleStatus.ScheduleStatuses.Unconfirmed).ToString(),
                                                      ((int) ScheduleStatus.ScheduleStatuses.Waiting_List).ToString()
                                                  })
                                          }

                                  });
                }
                SaveObject(addReferralModel.Referral, loggedInUserID);
                #endregion

                #region Add Referral Preference

                foreach (ReferralPreferenceModel item in PreferenceList.Where(c => c.ReferralPreferenceID == 0))
                {
                    preferenceList +=
                        Convert.ToString(item.PreferenceID) + Constants.PipeChar + Convert.ToString(item.PreferenceName) + Constants.Comma;
                }
                if (preferenceList.Length > 0)
                    preferenceList = preferenceList.Remove(preferenceList.LastIndexOf(Constants.CommaChar));

                var searchListValueData = new List<SearchValueData>
                                {
                                    new SearchValueData { Name = "ReferralID", Value = Convert.ToString(addReferralModel.Referral.ReferralID)},
                                    new SearchValueData { Name = "Preferences", Value = preferenceList},
                                    new SearchValueData { Name = "Skills", Value = addReferralModel.StrReferralSkillList},
                                    new SearchValueData { Name = "PreferenceType_Preference", Value = Convert.ToString(Preference.PreferenceKeyType.Preference) },
                                    new SearchValueData { Name = "PreferenceType_Skill", Value = Convert.ToString(Preference.PreferenceKeyType.Skill) },
                                    new SearchValueData { Name = "LoggedInID", Value = Convert.ToString(loggedInUserID) },
                                    new SearchValueData { Name = "SystemID", Value = Common.GetHostAddress() },
                                };
                TransactionResult result1 = GetEntity<TransactionResult>(StoredProcedure.SaveReferralPreferences, searchListValueData);

                #endregion

                #region Add Referral Diagnosis Code details

                if (addReferralModel.DxCodeMappingList.Any())
                {
                    var tempList =
                        addReferralModel.DxCodeMappingList.Where(m => m.ReferralDXCodeMappingID == 0).ToList();

                    foreach (var dxCodeMapping in tempList)
                    {
                        ReferralDXCodeMapping referralDxCodeMapping = new ReferralDXCodeMapping
                        {
                            DXCodeID = dxCodeMapping.DXCodeID,
                            //EndDate = dxCodeMapping.EndDate,
                            //StartDate = dxCodeMapping.StartDate,
                            Precedence = dxCodeMapping.Precedence,
                            ReferralID = addReferralModel.Referral.ReferralID,
                            CreatedBy = loggedInUserID,
                            CreatedDate = DateTime.UtcNow
                        };
                        SaveObject(referralDxCodeMapping, loggedInUserID);
                        Common.CreateAuditTrail(AuditActionType.Create, addReferralModel.Referral.ReferralID, referralDxCodeMapping.ReferralDXCodeMappingID,
                                                 new ReferralDXCodeMapping(), referralDxCodeMapping, loggedInUserID);
                        //Common.CreateAuditTrail(AuditActionType.Create, addReferralModel.Referral.ReferralID, referralDxCodeMapping.ReferralDXCodeMappingID,
                        //                         new ReferralDXCodeMapping(), referralDxCodeMapping, loggedInUserID);
                    }

                    var changedDxCode = addReferralModel.DxCodeMappingList.Where(m => m.ReferralDXCodeMappingID > 0).ToList();
                    foreach (var dxcode in changedDxCode)
                    {
                        ReferralDXCodeMapping temp = GetEntity<ReferralDXCodeMapping>(dxcode.ReferralDXCodeMappingID);

                        ReferralDXCodeMapping referralDxCodeMapping = new ReferralDXCodeMapping
                        {
                            ReferralID = addReferralModel.Referral.ReferralID,
                            DXCodeID = dxcode.DXCodeID,
                            //EndDate = dxcode.EndDate,
                            //StartDate = dxcode.StartDate,
                            Precedence = dxcode.Precedence,
                            ReferralDXCodeMappingID = dxcode.ReferralDXCodeMappingID,
                            CreatedBy = temp.CreatedBy,
                            CreatedDate = temp.CreatedDate,
                            UpdatedBy = loggedInUserID,
                            UpdatedDate = DateTime.UtcNow
                        };
                        SaveObject(referralDxCodeMapping, loggedInUserID);

                        Common.CreateAuditTrail(AuditActionType.Update, addReferralModel.Referral.ReferralID, referralDxCodeMapping.ReferralDXCodeMappingID,
                                                 temp, referralDxCodeMapping, loggedInUserID);
                    }
                }

                #endregion

                #region Add/Update Referral/Client Contact Information
                // Save Contact Info in the database

                if (addReferralModel.ContactInformationList.Any())
                {
                    foreach (var addAndListContactInformation in addReferralModel.ContactInformationList.OrderBy(c => c.ContactTypeID))
                    {
                        addAndListContactInformation.ReferralID = addReferralModel.Referral.ReferralID;
                        addAndListContactInformation.ClientID = addReferralModel.Referral.ClientID;
                        //addReferralModel.AddAndListContactInformation = addAndListContactInformation;
                        // This method will save the entry in the Contact and Contact Mapping table.
                        SaveContact(addAndListContactInformation, loggedInUserID);
                    }
                }

                // Save the payor info in the table ReferralPayorMapping
                #endregion

                #region Add/Update Referral Compliance Details
                if (addReferralModel.DocumentList != null && addReferralModel.DocumentList.Count > 0)
                {
                    foreach (ReferralComplianceDetails item in addReferralModel.DocumentList)
                    {
                        var srchParam = new List<SearchValueData>();
                        srchParam.Add(new SearchValueData("ReferralComplianceID", Convert.ToString(item.ReferralComplianceID)));
                        srchParam.Add(new SearchValueData("ReferralID", Convert.ToString(item.ReferralID == 0 ? addReferralModel.Referral.ReferralID : item.ReferralID)));
                        srchParam.Add(new SearchValueData("ComplianceID", Convert.ToString(item.ComplianceID)));
                        srchParam.Add(new SearchValueData("Value", Convert.ToString(item.Value)));
                        if (item.ExpirationDate.HasValue)
                            srchParam.Add(new SearchValueData { Name = "ExpirationDate", Value = Convert.ToDateTime(item.ExpirationDate).ToString(Constants.DbDateFormat) });
                        srchParam.Add(new SearchValueData { Name = "CreatedDate", Value = Common.GetOrgCurrentDateTime().ToString(Constants.DbDateFormat) });
                        srchParam.Add(new SearchValueData { Name = "CreatedBy", Value = Convert.ToString(loggedInUserID) });
                        srchParam.Add(new SearchValueData { Name = "UpdatedDate", Value = Common.GetOrgCurrentDateTime().ToString(Constants.DbDateFormat) });
                        srchParam.Add(new SearchValueData { Name = "UpdatedBy", Value = Convert.ToString(loggedInUserID) });
                        srchParam.Add(new SearchValueData { Name = "SystemID", Value = Common.GetHostAddress() });

                        GetScalar(StoredProcedure.HC_SaveReferralCompliance, srchParam);
                    }
                }
                #endregion

                #region Set Default Referral Task Mapping

                var searchParam1 = new List<SearchValueData>();
                searchParam1.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(addReferralModel.Referral.ReferralID) });
                searchParam1.Add(new SearchValueData { Name = "CreatedDate", Value = Common.GetOrgCurrentDateTime().ToString(Constants.DbDateFormat) });
                searchParam1.Add(new SearchValueData { Name = "CreatedBy", Value = Convert.ToString(loggedInUserID) });
                searchParam1.Add(new SearchValueData { Name = "SystemID", Value = HttpContext.Current.Request.UserHostAddress });

                GetScalar(StoredProcedure.SetDefaultReferralTaskMapping, searchParam1);

                #endregion


                #region Save Referral Caregiver Details


                var searchParam2 = new List<SearchValueData>();
                searchParam2.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(addReferralModel.Referral.ReferralID) });
                searchParam2.Add(new SearchValueData { Name = "AgencyID", Value = Convert.ToString(addReferralModel.Referral.ReferralCareGiver_AgencyID) });
                searchParam2.Add(new SearchValueData { Name = "LoggedInUserID", Value = Convert.ToString(loggedInUserID) });
                searchParam2.Add(new SearchValueData { Name = "SystemID", Value = HttpContext.Current.Request.UserHostAddress });
                GetScalar(StoredProcedure.HC_SaveReferralCareGiverDetails, searchParam2);



                #endregion

                #region Referral Beneficiary Types

                if (addReferralModel.ReferralBeneficiaryTypes.Any())
                {
                    var tempList = addReferralModel.ReferralBeneficiaryTypes.Where(m => m.ReferralBeneficiaryTypeID == 0).ToList();

                    foreach (var item in tempList)
                    {
                        ReferralBeneficiaryType referralBeneficiaryType = new ReferralBeneficiaryType
                        {
                            BeneficiaryTypeID = item.BeneficiaryTypeID,
                            ReferralID = addReferralModel.Referral.ReferralID,
                            BeneficiaryNumber = item.BeneficiaryNumber,
                            CreatedBy = loggedInUserID,
                            CreatedDate = DateTime.UtcNow
                        };
                        SaveObject(referralBeneficiaryType, loggedInUserID);
                    }

                    var editedBeneficiaryTypes = addReferralModel.ReferralBeneficiaryTypes.Where(m => m.ReferralBeneficiaryTypeID > 0).ToList();
                    foreach (var item in editedBeneficiaryTypes)
                    {
                        ReferralBeneficiaryType temp = GetEntity<ReferralBeneficiaryType>(item.ReferralBeneficiaryTypeID);
                        temp.BeneficiaryTypeID = item.BeneficiaryTypeID;
                        temp.BeneficiaryNumber = item.BeneficiaryNumber;
                        temp.UpdatedBy = loggedInUserID;
                        temp.UpdatedDate = DateTime.UtcNow;
                        SaveObject(temp, loggedInUserID);
                    }
                }

                #endregion Referral Beneficiary Types

                #region Referral Physicians

                if (addReferralModel.ReferralPhysicians.Any())
                {
                    var tempList = addReferralModel.ReferralPhysicians.Where(m => m.ReferralPhysicianID == 0).ToList();

                    foreach (var item in tempList)
                    {
                        ReferralPhysician referralPhysician = new ReferralPhysician
                        {
                            PhysicianID = item.PhysicianID,
                            ReferralID = addReferralModel.Referral.ReferralID,
                            CreatedBy = loggedInUserID,
                            CreatedDate = DateTime.UtcNow
                        };
                        SaveObject(referralPhysician, loggedInUserID);
                    }

                    var editedPhysicians = addReferralModel.ReferralPhysicians.Where(m => m.ReferralPhysicianID > 0).ToList();
                    foreach (var item in editedPhysicians)
                    {
                        ReferralPhysician temp = GetEntity<ReferralPhysician>(item.ReferralPhysicianID);
                        temp.PhysicianID = item.PhysicianID;
                        temp.UpdatedBy = loggedInUserID;
                        temp.UpdatedDate = DateTime.UtcNow;
                        SaveObject(temp, loggedInUserID);
                    }
                }

                #endregion Referral Physicians

                #endregion

                response.IsSuccess = true;


                Common.CreateAuditTrail(isEditMode ? AuditActionType.Update : AuditActionType.Create, addReferralModel.Referral.ReferralID, addReferralModel.Referral.ReferralID,
                                                 tempReferral, addReferralModel.Referral, loggedInUserID);

                response.Message = isEditMode ? string.Format(Resource.ReferralUpdatedSuccessfully, Resource.Referral) : string.Format(Resource.ReferralSavedSuccessfully, Resource.Referral);

                if (!isEditMode)
                {
                    addReferralModel.Referral.EncryptedReferralID = Crypto.Encrypt(Convert.ToString(addReferralModel.Referral.ReferralID));
                }


                response.Data = addReferralModel;
            }
            return response;
        }
        public ServiceResponse ReferralDxCodeMapping(HC_AddReferralModel addReferralModel, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();
            //List<ReferralPreferenceModel> PreferenceList = addReferralModel.PreferenceList;
            //string preferenceList = string.Empty;
            if (addReferralModel.Referral != null)
            {
                bool isEditMode = addReferralModel.Referral.ReferralID > 0;
                #region Server Side Validation Check for Referral
                var tempReferral = new Referral();

                if (isEditMode)
                {
                    tempReferral = GetEntity<Referral>(addReferralModel.Referral.ReferralID);
                }

                // Check for the Add/Edit Contact and accordingly update.                             

                #endregion
                #region Add Referral Diagnosis Code details

                if (addReferralModel.DxCodeMappingList.Any())
                {
                    var tempList =
                        addReferralModel.DxCodeMappingList.Where(m => m.ReferralDXCodeMappingID == 0).ToList();

                    List<DXCodeMappingList> dx = new List<DXCodeMappingList>();

                    foreach (var dxCodeMapping in tempList)
                    {
                        ReferralDXCodeMapping referralDxCodeMapping = new ReferralDXCodeMapping
                        {
                            DXCodeID = dxCodeMapping.DXCodeID,
                            //EndDate = dxCodeMapping.EndDate,
                            //StartDate = dxCodeMapping.StartDate,
                            Precedence = dxCodeMapping.Precedence,
                            ReferralID = addReferralModel.Referral.ReferralID,
                            CreatedBy = loggedInUserID,
                            CreatedDate = DateTime.UtcNow
                        };
                        SaveObject(referralDxCodeMapping, loggedInUserID);
                        dxCodeMapping.ReferralDXCodeMappingID = referralDxCodeMapping.ReferralDXCodeMappingID;
                        dx.Add(dxCodeMapping);

                        Common.CreateAuditTrail(AuditActionType.Create, addReferralModel.Referral.ReferralID, referralDxCodeMapping.ReferralDXCodeMappingID,
                                                 new ReferralDXCodeMapping(), referralDxCodeMapping, loggedInUserID);
                        //Common.CreateAuditTrail(AuditActionType.Create, addReferralModel.Referral.ReferralID, referralDxCodeMapping.ReferralDXCodeMappingID,
                        //                         new ReferralDXCodeMapping(), referralDxCodeMapping, loggedInUserID);
                    }

                    var changedDxCode = addReferralModel.DxCodeMappingList.Where(m => m.ReferralDXCodeMappingID > 0).ToList();
                    foreach (var dxcode in changedDxCode)
                    {
                        ReferralDXCodeMapping temp = GetEntity<ReferralDXCodeMapping>(dxcode.ReferralDXCodeMappingID);

                        ReferralDXCodeMapping referralDxCodeMapping = new ReferralDXCodeMapping
                        {
                            ReferralID = addReferralModel.Referral.ReferralID,
                            DXCodeID = dxcode.DXCodeID,
                            //EndDate = dxcode.EndDate,
                            //StartDate = dxcode.StartDate,
                            Precedence = dxcode.Precedence,
                            ReferralDXCodeMappingID = dxcode.ReferralDXCodeMappingID,
                            CreatedBy = temp.CreatedBy,
                            CreatedDate = temp.CreatedDate,
                            UpdatedBy = loggedInUserID,
                            UpdatedDate = DateTime.UtcNow
                        };
                        SaveObject(referralDxCodeMapping, loggedInUserID);
                        dx.Add(dxcode);
                        Common.CreateAuditTrail(AuditActionType.Update, addReferralModel.Referral.ReferralID, referralDxCodeMapping.ReferralDXCodeMappingID,
                                                 temp, referralDxCodeMapping, loggedInUserID);
                    }
                }

                #endregion

                response.IsSuccess = true;


                Common.CreateAuditTrail(isEditMode ? AuditActionType.Update : AuditActionType.Create, addReferralModel.Referral.ReferralID, addReferralModel.Referral.ReferralID,
                                                 tempReferral, addReferralModel.Referral, loggedInUserID);

                response.Message = isEditMode ? string.Format(Resource.ReferralUpdatedSuccessfully, Resource.Referral) : string.Format(Resource.ReferralSavedSuccessfully, Resource.Referral);

                if (!isEditMode)
                {
                    addReferralModel.Referral.EncryptedReferralID = Crypto.Encrypt(Convert.ToString(addReferralModel.Referral.ReferralID));
                }


                response.Data = addReferralModel;
            }
            return response;
        }

        public ServiceResponse HC_UpdateAccount(ReferralAhcccsDetails model, Referral referral, long loggedInUserId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            if (model != null)
            {
                searchList.Add(new SearchValueData { Name = "ReferralID", Value = model.ReferralID.ToString() });
                searchList.Add(new SearchValueData { Name = "AHCCCSID", Value = model.AHCCCSID });
                searchList.Add(new SearchValueData { Name = "NewAHCCCSID", Value = model.NewAHCCCSID });

                //int value = (int)GetScalar(StoredProcedure.UpdateAhcccsId, searchList);
                ReferralAhcccsUpdateModel data = GetMultipleEntity<ReferralAhcccsUpdateModel>(StoredProcedure.UpdateAhcccsId, searchList);
                if (data.ReturnValue == 1)
                    response.Message = Resource.AcoountAlreadyExists;
                if (data.ReturnValue == 2)
                    response.Message = Resource.AccountShouldNotMatchWithPrevious;
                if (data.ReturnValue == 3)
                    response.Message = Resource.AccountMissingInvalid;
                if (data.ReturnValue == 4)
                {
                    response.IsSuccess = true;
                    response.Message = Resource.AccountUpdatedSuccessfully;

                    data.UpdatedReferral = JsonConvert.DeserializeObject<Referral>(JsonConvert.SerializeObject(referral));
                    data.UpdatedReferral.AHCCCSID = model.NewAHCCCSID;
                    Common.CreateAuditTrail(AuditActionType.Update, model.ReferralID, model.ReferralID,
                                                 referral, data.UpdatedReferral, loggedInUserId);
                }


            }
            else
                response.Message = Resource.ErrorOccured;
            return response;
        }

        public ServiceResponse HC_DeleteReferralBeneficiaryType(ReferralBeneficiaryDetail referralBeneficiaryDetail, long loggedInId)
        {
            ServiceResponse response = new ServiceResponse();

            DeleteEntity<ReferralBeneficiaryType>(referralBeneficiaryDetail.ReferralBeneficiaryTypeID);

            response.IsSuccess = true;
            response.Message = string.Format("{0} {1}", Resource.BeneficiaryType, Resource.Deleted);
            return response;
        }

        public ServiceResponse HC_DeleteReferralPhysician(ReferralPhysicianDetail referralPhysicianDetail, long loggedInId)
        {
            ServiceResponse response = new ServiceResponse();

            DeleteEntity<ReferralPhysician>(referralPhysicianDetail.ReferralPhysicianID);

            response.IsSuccess = true;
            response.Message = string.Format("{0} {1}", Resource.Physician, Resource.Deleted);
            return response;
        }

        public ServiceResponse SaveTaskOrder(List<ReferralDXCodeMapping> model, long RefID)
        {
            ServiceResponse response = new ServiceResponse();
            foreach (ReferralDXCodeMapping item in model)
            {
                var searchParam = new List<SearchValueData>();
                searchParam.Add(new SearchValueData("DXCodeID", Convert.ToString(item.DXCodeID)));
                searchParam.Add(new SearchValueData("ReferralID", Convert.ToString(RefID)));
                searchParam.Add(new SearchValueData("Precedence", Convert.ToString(item.Precedence)));
                searchParam.Add(new SearchValueData("ReferralDXCodeMappingID", Convert.ToString(item.ReferralDXCodeMappingID)));
                GetScalar(StoredProcedure.UpdateDxCode, searchParam);
            }
            response.IsSuccess = true;
            response.Message = "DxCode Updated Successfully";
            return response;

        }
        #endregion

        #region Referral Compliance Details
        public ServiceResponse HC_SaveReferralCompliance(ReferralComplianceModel referralComplianceModel, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                foreach (ReferralComplianceDetails item in referralComplianceModel.DocumentList)
                {
                    var searchParam = new List<SearchValueData>();
                    searchParam.Add(new SearchValueData("ReferralComplianceID", Convert.ToString(item.ReferralComplianceID)));
                    searchParam.Add(new SearchValueData("ReferralID", Convert.ToString(item.ReferralID)));
                    searchParam.Add(new SearchValueData("ComplianceID", Convert.ToString(item.ComplianceID)));
                    searchParam.Add(new SearchValueData("Value", Convert.ToString(item.Value)));
                    if (item.ExpirationDate.HasValue)
                        searchParam.Add(new SearchValueData { Name = "ExpirationDate", Value = Convert.ToDateTime(item.ExpirationDate).ToString(Constants.DbDateFormat) });
                    searchParam.Add(new SearchValueData { Name = "CreatedDate", Value = Convert.ToString(Common.GetOrgCurrentDateTime()) });
                    searchParam.Add(new SearchValueData { Name = "CreatedBy", Value = Convert.ToString(loggedInUserID) });
                    searchParam.Add(new SearchValueData { Name = "UpdatedDate", Value = Convert.ToString(Common.GetOrgCurrentDateTime()) });
                    searchParam.Add(new SearchValueData { Name = "UpdatedBy", Value = Convert.ToString(loggedInUserID) });
                    searchParam.Add(new SearchValueData { Name = "SystemID", Value = Common.GetHostAddress() });

                    GetScalar(StoredProcedure.HC_SaveReferralCompliance, searchParam);
                }



                response.IsSuccess = true;
                response.Message = "Compliance Detail Saved Successfully";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
        #endregion


        #region Referral List

        public ServiceResponse HC_SetReferralListPage(long loggedInID)
        {
            ServiceResponse response = new ServiceResponse();

            SetReferralListModel setReferralListModel = GetMultipleEntity<SetReferralListModel>(StoredProcedure.HC_SetReferralListPage, new List<SearchValueData>
            {
                new SearchValueData { Name = "DDType_PatientSystemStatus", Value = Convert.ToString((int)Common.DDType.PatientSystemStatus)},
                new SearchValueData { Name = "DDType_LanguagePreference", Value = Convert.ToString((int)Common.DDType.LanguagePreference)}
            });
            setReferralListModel.NotifyCaseManager = Common.SetYesNoAllList();
            setReferralListModel.Checklist = Common.SetYesNoAllList();
            setReferralListModel.ClinicalReview = Common.SetYesNoAllList();
            setReferralListModel.Services = Common.SetServicesFilter();
            setReferralListModel.Draft = Common.SetDraftFilter();
            setReferralListModel.DeleteFilter = Common.SetDeleteFilter();
            setReferralListModel.SearchReferralListModel.IsDeleted = 0;
            setReferralListModel.SearchReferralListModel.ServiceID = -1;
            setReferralListModel.SearchReferralListModel.ChecklistID = -1;
            setReferralListModel.SearchReferralListModel.ClinicalReviewID = -1;
            setReferralListModel.SearchReferralListModel.NotifyCaseManagerID = -1;
            setReferralListModel.SearchReferralListModel.IsSaveAsDraft = -1;
            // setReferralListModel.SearchReferralListModel.ReferralStatusID = -1;//(int)ReferralStatus.ReferralStatuses.Active;
            if (Common.HasPermission(Constants.Permission_View_Assinged_Referral) &&
                !Common.HasPermission(Constants.Permission_View_All_Referral))
            {
                setReferralListModel.SearchReferralListModel.AssigneeID = loggedInID;
            }
            response.Data = setReferralListModel;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse HC_SaveReferralStatus(ReferralStatusModel referralStatusModel, long loggedInId)
        {
            ServiceResponse response = new ServiceResponse();

            Referral referral = GetEntity<Referral>(referralStatusModel.ReferralID);
            if (referral != null)
            {
                #region Check all the Data for as per Referral Status

                if (referralStatusModel.ReferralStatusID == (int)ReferralStatus.ReferralStatuses.Active)
                {
                    if (referral.FirstName == null || referral.LastName == null || referral.Dob == null ||
                        referral.Gender == null || referral.AHCCCSID == null || referral.RegionID == null ||
                        referral.LanguageID == null || referral.FrequencyCodeID == null ||
                        referral.ReferralSourceID == 0 || referral.ReferralDate == null || referral.Assignee == null)
                    {
                        response.Message = Resource.ReferralStatusChange;
                        return response;
                    }
                }
                else if (referralStatusModel.ReferralStatusID == (int)ReferralStatus.ReferralStatuses.Inactive)
                {
                    if (referral.FirstName == null || referral.LastName == null || referral.Dob == null ||
                           referral.Gender == null || referral.AHCCCSID == null || referral.RegionID == null ||
                           referral.LanguageID == null || referral.FrequencyCodeID == null ||
                           referral.ReferralSourceID == 0 || referral.ReferralDate == null || referral.Assignee == null
                           || referral.ClosureDate == null || referral.ClosureReason == null)
                    {
                        response.Message = Resource.ReferralStatusChange;
                        return response;
                    }
                }
                else
                {
                    if (referral.FirstName == null || referral.LastName == null || referral.Dob == null ||
                           referral.Gender == null || referral.AHCCCSID == null || referral.RegionID == null ||
                           referral.LanguageID == null || referral.FrequencyCodeID == null ||
                            referral.ReferralSourceID == 0 || referral.ReferralDate == null || referral.Assignee == null)
                    {
                        response.Message = Resource.ReferralStatusChange;
                        return response;
                    }
                }
                List<ContactMapping> contactlist = GetEntityList<ContactMapping>(new List<SearchValueData>
                            {
                                new SearchValueData{Name = "ReferralID",Value =  Convert.ToString(referral.ReferralID)},
                            });
                if (contactlist.Count <= 0)
                {
                    response.Message = Resource.ReferralStatusChange;
                    return response;
                }
                //List<ReferralPayorMapping> payorlist = GetEntityList<ReferralPayorMapping>(new List<SearchValueData>
                //            {
                //                new SearchValueData{Name = "ReferralID",Value =  Convert.ToString(referral.ReferralID)}, 
                //                new SearchValueData{Name = "IsActive",Value =  Convert.ToString(Constants.IsActiveStatus)}, 
                //            });
                //if (payorlist.Count <= 0)
                //{
                //    response.Message = Resource.ReferralStatusChange;
                //    return response;
                //}
                //if (!referral.RespiteService && !referral.LifeSkillsService && !referral.CounselingService && !referral.ConnectingFamiliesService)
                //{
                //    response.Message = Resource.ReferralStatusChange;
                //    return response;
                //}

                #endregion

                if (referral.ReferralStatusID == (int)ReferralStatus.ReferralStatuses.Active &&
                    referralStatusModel.ReferralStatusID == (int)ReferralStatus.ReferralStatuses.Inactive)
                {
                    //GetScalar(StoredProcedure.InactiveSchedule,
                    //          new List<SearchValueData>
                    //              {
                    //                  new SearchValueData
                    //                      {
                    //                          Name = "ReferralID",
                    //                          Value = referral.ReferralID.ToString()
                    //                      },
                    //                  new SearchValueData
                    //                      {
                    //                          Name = "CancelStatus",
                    //                          Value = ((int) ScheduleStatus.ScheduleStatuses.Cancelled).ToString()
                    //                      },
                    //                  new SearchValueData {Name = "Comment", Value = @Resource.InactiveScheduleComment},
                    //                  new SearchValueData {Name = "WhoCancel", Value = Constants.Office},
                    //                  new SearchValueData
                    //                      {
                    //                          Name = "ChangeStatus",
                    //                          Value = string.Join(",", new List<String>
                    //                              {
                    //                                  ((int) ScheduleStatus.ScheduleStatuses.Unconfirmed).ToString(),
                    //                                  ((int) ScheduleStatus.ScheduleStatuses.Waiting_List).ToString()
                    //                              })
                    //                      }

                    //              });
                }

                referral.ReferralStatusID = referralStatusModel.ReferralStatusID;
                SaveObject(referral, loggedInId);
                ReferralStatus referralStatus = GetEntity<ReferralStatus>(referral.ReferralStatusID.Value);
                response.IsSuccess = true;
                response.Data = referralStatus;
                response.Message = Resource.ReferralUpdated;


            }
            else
            {
                response.Message = Resource.ExceptionMessage;
            }

            return response;
        }

        #endregion Referral List

        #region Referral Documents

        #region Documents

        public ServiceResponse HC_SetReferralDocument(long referralId)
        {
            var response = new ServiceResponse();
            if (referralId > 0)
            {
                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralID", Value = Convert.ToString(referralId)}
                };

                response.IsSuccess = true;
            }
            else
                response.Message = Resource.GetReferralDocumentError;
            return response;
        }

        public ServiceResponse HC_SaveFile(HttpRequestBase currentHttpRequest, bool isEmployeeDocument = false, bool isGoogleDriveDocument = false, string googleRefreshToken = "")
        {
            ServiceResponse response = new ServiceResponse();
            var ReferralId = currentHttpRequest.Form["id"];
            var ComplianceId = currentHttpRequest.Form["data"];
            string basePath = String.Format(isEmployeeDocument ? _cacheHelper.EmployeeDocumentFullPath : _cacheHelper.ReferralDocumentFullPath, _cacheHelper.Domain) + ReferralId + "/";
            //string KindOfDocument = Convert.ToString(DocumentType.DocumentKind.Internal);
            //int DocumentTypeID = (int)DocumentType.DocumentTypes.Other;
            int UserType = isEmployeeDocument ? (int)ReferralDocument.UserTypes.Employee : (int)ReferralDocument.UserTypes.Referral;
            Compliance compliance = GetEntity<Compliance>(Convert.ToInt64(ComplianceId));
            string KindOfDocument = compliance.DocumentationType == 1 ? DocumentType.DocumentKind.Internal.ToString() : DocumentType.DocumentKind.External.ToString();

            if (currentHttpRequest.Files.Count != 0)
            {
                bool isSuccess = true;
                List<object> fileDetails = new List<object>();
                for (int i = 0; i < currentHttpRequest.Files.Count; i++)
                {
                    HttpPostedFileBase file = currentHttpRequest.Files[i];
                    ServiceResponse fileResponse;
                    string storeType = "Local";
                    string googleFileId = "";
                    string googleFileJson = "";

                    if (!isGoogleDriveDocument)
                        fileResponse = Common.SaveFile(file, basePath);
                    else
                    {
                        storeType = "Google Drive";
                        var target = new System.IO.MemoryStream();
                        file.InputStream.CopyTo(target);
                        byte[] fileAsBytes = target.ToArray();

                        fileResponse = new GoogleDriveHelper().SaveFile(googleRefreshToken, fileAsBytes, file.ContentType, "", file.FileName);

                        if (fileResponse.IsSuccess)
                        {

                            // save json for reference
                            googleFileJson = ((UploadedFileModel)fileResponse.Data).GoogleFileJson;


                            // save id into a column (for update|delete)
                            dynamic googleResponseObj = JsonConvert.DeserializeObject(googleFileJson);
                            googleFileId = (string)googleResponseObj.id;
                        }
                    }

                    var actualFilepath = string.Empty;
                    if (fileResponse.IsSuccess)
                    {
                        actualFilepath = ((UploadedFileModel)fileResponse.Data).TempFilePath;
                        List<SearchValueData> searchList = new List<SearchValueData>()
                        {
                            new SearchValueData {Name = "FileName", Value = file.FileName},
                            new SearchValueData {Name = "FilePath", Value = actualFilepath},
                            new SearchValueData {Name = "ReferralID ", Value = ReferralId },
                            new SearchValueData {Name = "ComplianceID ", Value = ComplianceId },
                            new SearchValueData {Name = "UserType", Value = UserType.ToString() },
                            new SearchValueData {Name = "KindOfDocument ", Value = KindOfDocument },
                            //new SearchValueData {Name = "DocumentTypeID ", Value = DocumentTypeID.ToString() },
                            new SearchValueData {Name = "LoggedInUserID", Value = SessionHelper.LoggedInID.ToString()},
                            new SearchValueData {Name = "SystemID ", Value = HttpContext.Current.Request.UserHostAddress },
                            new SearchValueData {Name = "StoreType ", Value = storeType },
                            new SearchValueData {Name = "GoogleFileId ", Value = googleFileId },
                            new SearchValueData {Name = "GoogleDetails ", Value = googleFileJson },
                        };

                        GetScalar(StoredProcedure.HC_SaveReferralDocument, searchList);
                    }
                    else
                    { isSuccess = false; }

                    fileDetails.Add(new { FileName = file.FileName, IsSuccess = fileResponse.IsSuccess, Path = actualFilepath });
                }
                response.Data = fileDetails;
                response.IsSuccess = isSuccess;
                response.Message = isSuccess ? string.Format(Resource.RecordProcessedSuccessfully, Resource.Document) : Resource.SomethingWentWrong;
            }
            else
            { response.Message = Resource.FileUploadFailedNoFileSelected; }
            return response;
        }

        public ServiceResponse HC_GetReferralDocumentList(long referralID, long complianceID, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();

            //List<SearchValueData> searchList = new List<SearchValueData>();
            //searchList.Add(new SearchValueData { Name = "ReferralID", Value = referralID.ToString(),IsEqual=true});
            //searchList.Add(new SearchValueData { Name = "UserType", Value = "1" , IsEqual=true});
            var customFIlter = "UserID=" + referralID + " AND UserType=" + (int)ReferralDocument.UserTypes.Referral + " AND ComplianceID=" + complianceID;
            response = GetPageRecords<ReferralDocumentList>(pageSize, pageIndex, sortIndex, sortDirection, null, customFIlter);

            response.IsSuccess = true;

            return response;
        }

        public ServiceResponse HC_GetReferralDocumentListNew(long roleId, SearchReferralDocumentListPage searchReferralDocument, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchReferralDocument != null)
                SetReferralDocumentList(searchReferralDocument, searchList);

            searchList.Add(new SearchValueData { Name = "RoleID", Value = Convert.ToString(roleId) });

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<DocumentList> totalData = GetEntityList<DocumentList>(StoredProcedure.GetReferralDocumentListNew, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<DocumentList> ddMasterList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = ddMasterList;
            response.IsSuccess = true;

            return response;
        }

        private static void SetReferralDocumentList(SearchReferralDocumentListPage searchReferralDocument, List<SearchValueData> searchList)
        {
            var UserType = searchReferralDocument.UserType == ReferralDocument.UserTypes.Referral.ToString() ? (int)ReferralDocument.UserTypes.Referral : (int)ReferralDocument.UserTypes.Employee;
            searchList.Add(new SearchValueData { Name = "UserType", Value = Convert.ToString(UserType) });

            if (UserType == (int)ReferralDocument.UserTypes.Referral)
                searchList.Add(new SearchValueData { Name = "ReferralID", Value = Crypto.Decrypt(searchReferralDocument.EncryptedReferralID) });

            if (UserType == (int)ReferralDocument.UserTypes.Employee)
                searchList.Add(new SearchValueData { Name = "EmployeeID", Value = Crypto.Decrypt(searchReferralDocument.EncryptedEmployeeID) });

            searchList.Add(new SearchValueData { Name = "ComplianceID", Value = Convert.ToString(searchReferralDocument.ComplianceID) });
            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchReferralDocument.IsDeleted) });
            searchList.Add(new SearchValueData { Name = "Name", Value = searchReferralDocument.Name });
            searchList.Add(new SearchValueData { Name = "SearchInDate", Value = searchReferralDocument.SearchInDate });
            if (searchReferralDocument.StartDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(searchReferralDocument.StartDate).ToString(Constants.DbDateFormat) });
            if (searchReferralDocument.EndDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(searchReferralDocument.EndDate).ToString(Constants.DbDateFormat) });

            searchList.Add(new SearchValueData
            {
                Name = "KindOfDocument",
                Value = searchReferralDocument.KindOfDocument == 1 ? Common.DocumentationType.Internal.ToString()
                    : searchReferralDocument.KindOfDocument == 2 ? Common.DocumentationType.External.ToString() : null
            });
            searchList.Add(new SearchValueData { Name = "SearchType", Value = searchReferralDocument.SearchType });

        }

        public ServiceResponse HC_DeleteDocument(long referralDocumentID, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();
            var UserType = (int)ReferralDocument.UserTypes.Referral;
            if (referralDocumentID > 0)
            {
                ReferralDocument referralDocument = GetEntity<ReferralDocument>(referralDocumentID);
                if (referralDocument != null)
                {
                    var fullPath = HttpContext.Current.Server.MapCustomPath(referralDocument.FilePath);
                    response = Common.DeleteFile(fullPath);

                    List<SearchValueData> searchParam = new List<SearchValueData>();
                    searchParam.Add(new SearchValueData("ReferralDocumentID", Convert.ToString(referralDocument.ReferralDocumentID)));
                    searchParam.Add(new SearchValueData("UserID", Convert.ToString(referralDocument.UserID)));
                    searchParam.Add(new SearchValueData("DocumentTypeID", Convert.ToString(referralDocument.DocumentTypeID)));
                    GetScalar(StoredProcedure.HC_DeleteDocument, searchParam);
                    //DeleteEntity<ReferralDocument>(referralDocument.ReferralDocumentID);


                    response.IsSuccess = true;
                    response.Data = HC_GetReferralDocumentList(referralDocument.UserID, referralDocument.ComplianceID, pageIndex, pageSize, sortIndex,
                                                            sortDirection).Data;
                    response.Message = Resource.DocumentDeleted;
                }
                else
                {
                    response.Message = Resource.ExceptionMessage;
                }
            }
            else
            {
                response.Message = Resource.ExceptionMessage;
            }

            return response;
        }

        public ServiceResponse HC_SaveDocument(ReferralDocument referralDoc, int pageIndex, int pageSize, string sortIndex,
                                            string sortDirection, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();
            var UserType = (int)ReferralDocument.UserTypes.Referral;
            if (referralDoc.ReferralDocumentID > 0)
            {
                //bool IsExists = false;
                ReferralDocument referralDocument = GetEntity<ReferralDocument>(referralDoc.ReferralDocumentID);
                Referral referral = GetEntity<Referral>(referralDocument.UserID);
                List<SearchValueData> searchParam = new List<SearchValueData>();

                searchParam.Add(new SearchValueData("ReferralDocumentID", Convert.ToString(referralDoc.ReferralDocumentID)));
                searchParam.Add(new SearchValueData("FileName", referralDoc.FileName));
                searchParam.Add(new SearchValueData("KindOfDocument", referralDoc.KindOfDocument));
                searchParam.Add(new SearchValueData("UserID", Convert.ToString(referralDoc.UserID)));
                searchParam.Add(new SearchValueData("DocumentTypeID", Convert.ToString(referralDoc.DocumentTypeID)));
                searchParam.Add(new SearchValueData("UpdatedDate", Common.GetOrgCurrentDateTime().ToString(Constants.DbDateTimeFormat)));
                searchParam.Add(new SearchValueData("LoggedInUserID", Convert.ToString(loggedInUserID)));
                searchParam.Add(new SearchValueData("SystemID", Common.GetHostAddress()));

                int data = (int)GetScalar(StoredProcedure.HC_SaveDocument, searchParam);

                if (data == 1)
                {
                    response.IsSuccess = true;
                    response.Message = Resource.DocumentUpdated;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = Resource.DocumentTypeExists;
                }
                response.Data =
                    HC_GetReferralDocumentList(referral.ReferralID, referralDocument.ComplianceID, pageIndex, pageSize, sortIndex, sortDirection).Data;
            }
            else
            {
                response.Message = Resource.DocumentTypeExists;
            }
            //}
            //else
            //{
            //    response.Message = Resource.ExceptionMessage;
            //}

            return response;
        }


        public ServiceResponse HC_SaveDocumentNew(ReferralDocument model, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();
            if (model.ReferralDocumentID > 0)
            {
                //bool IsExists = false;
                List<SearchValueData> searchParam = new List<SearchValueData>();

                searchParam.Add(new SearchValueData("ReferralDocumentID", Convert.ToString(model.ReferralDocumentID)));
                searchParam.Add(new SearchValueData("FileName", model.FileName));
                searchParam.Add(new SearchValueData("KindOfDocument", model.KindOfDocument == "1" ? Common.DocumentationType.Internal.ToString()
                : model.KindOfDocument == "2" ? Common.DocumentationType.External.ToString() : null));
                if (model.ExpirationDate.HasValue)
                    searchParam.Add(new SearchValueData("ExpirationDate", model.ExpirationDate.Value.ToString(Constants.DbDateFormat)));
                searchParam.Add(new SearchValueData("UpdatedDate", Common.GetOrgCurrentDateTime().ToString(Constants.DbDateTimeFormat)));
                searchParam.Add(new SearchValueData("LoggedInUserID", Convert.ToString(loggedInUserID)));
                searchParam.Add(new SearchValueData("SystemID", Common.GetHostAddress()));
                searchParam.Add(new SearchValueData("ReferralID", ""));

                int data = (int)GetScalar(StoredProcedure.HC_SaveDocumentNew, searchParam);

                if (data == 1)
                {
                    response.IsSuccess = true;
                    response.Message = Resource.DocumentUpdated;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = Resource.DocumentTypeExists;
                }
            }
            else
            {
                response.Message = Resource.DocumentTypeExists;
            }

            return response;
        }

        #endregion Documents

        #region Missing Documents

        public ServiceResponse HC_SetReferralMissingDocument(long referralId)
        {
            var response = new ServiceResponse();
            if (referralId > 0)
            {
                var searchlist = new List<SearchValueData>
                {

                    new SearchValueData {Name = "ReferralID", Value = Convert.ToString(referralId)},
                    new SearchValueData {Name = "EmailTemplateTypeID", Value = Convert.ToInt16(EnumEmailType.Missing_Expire_Document_Template).ToString()},
                    //new SearchValueData {Name = "AgencyROI", Value = Convert.ToString(Constants.AgencyROI)},
                    //new SearchValueData {Name = "NetworkServicePlan", Value = Convert.ToString(Constants.NetworkServicePlan)},
                    //new SearchValueData {Name = "BXAssessment", Value = Convert.ToString(Constants.BXAssessment)},
                    //new SearchValueData {Name = "Demographic", Value = Convert.ToString(Constants.Demographic)},
                    //new SearchValueData {Name = "SNCD", Value = Convert.ToString(Constants.SNCD)},
                    //new SearchValueData {Name = "NetworkCrisisPlan", Value = Convert.ToString(Constants.NetworkCrisisPlan)},
                    //new SearchValueData {Name = "CAZOnly", Value = Convert.ToString(Constants.CAZOnly)},
                    new SearchValueData {Name = "Missing", Value = Convert.ToString(Constants.Missing)},
                    new SearchValueData {Name = "Expired", Value = Convert.ToString(Constants.Expired)},
                    new SearchValueData {Name = "OrganizationDate", Value = Common.GetOrgCurrentDateTime().ToString(Constants.DbDateFormat)}
                 };

                ReferralMissingDocumentModel referralMissingDocumentModel = GetMultipleEntity<ReferralMissingDocumentModel>(StoredProcedure.HC_SetReferralMissingDocument, searchlist);

                #region ScrapCode
                //string htmlString = referralMissingDocumentModel.MissingDocumentList.Count > 0
                //                        ? referralMissingDocumentModel.MissingDocumentList.OrderByDescending(x => x.MissingDocumentType).Aggregate
                //                              ("<b>Client Name:&nbsp;</b>" + referralMissingDocumentModel.SetMissingDocumentTokenModel.ClientName + " " + referralMissingDocumentModel.SetMissingDocumentTokenModel.DateofBirth + referralMissingDocumentModel.SetMissingDocumentTokenModel.AHCCCSID + " " +
                //                               "<br><ul>", (current, missingDocument) => current + string.Format
                //                     ("<li>{0}:&nbsp;{1}</li>", missingDocument.MissingDocumentType, missingDocument.MissingDocumentName))
                //                     : string.Format("<ul><li>{0}</li>", Resource.NoDocumentMissing);
                //htmlString += "</ul>";
                #endregion

                referralMissingDocumentModel.SetMissingDocumentTokenModel.CaseManager =
                    referralMissingDocumentModel.SetMissingDocumentTokenModel.CaseManagerFirstName + " " +
                    referralMissingDocumentModel.SetMissingDocumentTokenModel.CaseManagerLastName;

                referralMissingDocumentModel.SetMissingDocumentTokenModel.SiteName = _cacheHelper.SiteName;

                string htmlString = "<ul>";

                string clientWithDocDtlli = "<li><b>{0} (#{2}, DOB : {1})</b><br/> {3}</li>";

                string str = "";
                if (referralMissingDocumentModel.MissingDocumentList.Any(m => m.MissingDocumentType == Constants.Missing))
                {
                    str = string.Format("<b>{0}</b> : {1}<br>", Constants.Missing,
                                        string.Join(", ",
                                                    referralMissingDocumentModel.MissingDocumentList.Where(m => m.MissingDocumentType == Constants.Missing)
                                                                                .Select(m => m.MissingDocumentName)));
                }
                if (referralMissingDocumentModel.MissingDocumentList.Any(m => m.MissingDocumentType == Constants.Expired))
                {
                    str += string.Format("<b>{0}</b> : {1}<br>", Constants.Expired,
                                         string.Join(", ", referralMissingDocumentModel.MissingDocumentList.Where(m => m.MissingDocumentType == Constants.Expired).Select(m => m.MissingDocumentName)));
                }
                htmlString += string.Format(clientWithDocDtlli, referralMissingDocumentModel.SetMissingDocumentTokenModel.ClientName,
                                     referralMissingDocumentModel.SetMissingDocumentTokenModel.DateofBirth, referralMissingDocumentModel.SetMissingDocumentTokenModel.AHCCCSID, str);

                htmlString += "</ul>";

                referralMissingDocumentModel.SetMissingDocumentTokenModel.ClientList = htmlString;

                if (string.IsNullOrEmpty(referralMissingDocumentModel.SetMissingDocumentTokenModel.ToEmail))
                {
                    referralMissingDocumentModel.SetMissingDocumentTokenModel.ToEmail = referralMissingDocumentModel.SetMissingDocumentTokenModel.Email;
                    referralMissingDocumentModel.SetMissingDocumentTokenModel.CaseManagerFirstName = Resource.CASEMANGERLabel + referralMissingDocumentModel.SetMissingDocumentTokenModel.CaseManagerFirstName;
                }
                else
                {
                    referralMissingDocumentModel.SetMissingDocumentTokenModel.CaseManagerFirstName = Resource.RecordsDepartment;
                }

                if (!string.IsNullOrEmpty(referralMissingDocumentModel.SetMissingDocumentTokenModel.ClientNickName))
                {
                    referralMissingDocumentModel.SetMissingDocumentTokenModel.ClientName = referralMissingDocumentModel.SetMissingDocumentTokenModel.ClientName +
                        "(" + referralMissingDocumentModel.SetMissingDocumentTokenModel.ClientNickName + ")";
                }

                referralMissingDocumentModel.SetMissingDocumentTokenModel.MissingItems = htmlString;

                var missingDocumentModel = new MissingDocumentModel
                {
                    ToEmail = referralMissingDocumentModel.SetMissingDocumentTokenModel.ToEmail,
                    Subject = referralMissingDocumentModel.EmailTemplate.EmailTemplateSubject,
                    Body = TokenReplace.ReplaceTokens(referralMissingDocumentModel.EmailTemplate.EmailTemplateBody, referralMissingDocumentModel.SetMissingDocumentTokenModel)
                };

                response.Data = missingDocumentModel;
                response.IsSuccess = true;
            }
            else
                response.Message = Resource.GetReferralDocumentError;
            return response;
        }

        public ServiceResponse HC_SendEmailForReferralMissingDocument(MissingDocumentModel missingDocumentModel, long referralId, long loggedInUserID)
        {
            var response = new ServiceResponse();
            if (referralId > 0)
            {
                bool isSentMail = Common.SendEmail(missingDocumentModel.Subject, _cacheHelper.SupportEmail, missingDocumentModel.ToEmail, missingDocumentModel.Body, null, ConfigSettings.CCEmailAddress, (int)EmailHelper.SMTPSetting.EncryptedEmailSetting);
                response.IsSuccess = isSentMail;
                response.Message = isSentMail ? Resource.EmailSentSuccess : Resource.EmailSentFail;
                if (isSentMail)
                {
                    INoteDataProvider iNoteDataProvider = new NoteDataProvider();
                    iNoteDataProvider.SaveGeneralNote(referralId, missingDocumentModel.Body, Resource.WebsiteMissingDocumentEmail,
                                                      loggedInUserID, missingDocumentModel.ToEmail,
                                                      Constants.Case_Manager, Resource.Email);
                }
            }
            else
                response.Message = Resource.NoReferralFound;
            return response;
        }

        #endregion Missing Documents

        #endregion Referral Documents

        #region Referral History

        public ServiceResponse GetReferralHistory(long referralID)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                if (referralID > 0)
                {
                    List<SearchValueData> searchList = new List<SearchValueData>
                    {
                        new SearchValueData { Name = "ReferralID", Value = Convert.ToString(referralID) },
                    };

                    List<ReferralHistory> referralHistoryList = GetEntityList<ReferralHistory>(StoredProcedure.GetReferralHistory, searchList);

                    response.Data = referralHistoryList;
                    response.IsSuccess = true;
                }
                else
                { response.Message = Resource.Invalid; }
            }
            catch (Exception ex)
            { response.Message = Common.MessageWithTitle(Resource.Error, ex.Message); }

            return response;
        }

        public ServiceResponse SaveReferralHistoryItem(ReferralHistory referralHistoryModel, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                bool isEditMode = referralHistoryModel.ReferralHistoryID > 0;
                return SaveObject(referralHistoryModel, loggedInUserID,
                    string.Format((isEditMode ? Resource.RecordUpdatedSuccessfully : Resource.RecordCreatedSuccessfully), Resource.ReferralHistory));
            }
            catch (Exception ex)
            { response.Message = Common.MessageWithTitle(Resource.Error, ex.Message); }

            return response;
        }

        public ServiceResponse DeleteReferralHistoryItem(long referralHistoryID)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                if (referralHistoryID > 0)
                {
                    var record = GetEntity<ReferralHistory>(referralHistoryID);
                    if (record != null)
                    {
                        record.IsDeleted = true;
                        var data = SaveEntity(record);
                        response.Data = data;
                        response.IsSuccess = true;
                        response.Message = string.Format(Resource.RecordDeletedSuccessfully, Resource.ReferralHistory);
                    }
                    else
                    { response.Message = Resource.RecordNotFound; }
                }
                else
                { response.Message = Resource.Invalid; }
            }
            catch (Exception ex)
            { response.Message = Common.MessageWithTitle(Resource.Error, ex.Message); }
            return response;
        }

        #endregion


        #region Referral Caledner

        #region Employee Calender
        public ServiceResponse HC_ReferralCalender()
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData>();
                HC_RefCalenderModel model = GetMultipleEntity<HC_RefCalenderModel>(StoredProcedure.GetReferralCalenderPageModel);
                //model.SearchRefCalender.StartDate = DateTime.Now;
                model.SearchRefCalender.StartDate = Common.GetOrgStartOfWeek();
                response.IsSuccess = true;
                response.Data = model;

            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }


            return response;
        }
        #endregion


        #endregion

        #region Referral Task Mapping

        public ServiceResponse GetVisitTaskList(SearchVisitTaskListPage searchVisitTaskListPage, int pageIndex, int pageSize,
            string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchVisitTaskListPage != null)
                SetSearchFilterForVisitTaskList(searchVisitTaskListPage, searchList);

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<ListVisitTaskModel> totalData = GetEntityList<ListVisitTaskModel>(StoredProcedure.GetVisitTaskListForReferral, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListVisitTaskModel> visitTaskList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = visitTaskList;
            response.IsSuccess = true;
            return response;
        }

        private static void SetSearchFilterForVisitTaskList(SearchVisitTaskListPage searchVisitTaskListPage, List<SearchValueData> searchList)
        {
            if (!string.IsNullOrEmpty(searchVisitTaskListPage.VisitTaskType))
                searchList.Add(new SearchValueData { Name = "VisitTaskType", Value = Convert.ToString(searchVisitTaskListPage.VisitTaskType) });

            if (!string.IsNullOrEmpty(searchVisitTaskListPage.VisitTaskDetail))
                searchList.Add(new SearchValueData { Name = "VisitTaskDetail", Value = Convert.ToString(searchVisitTaskListPage.VisitTaskDetail) });

            searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(searchVisitTaskListPage.ReferralID) });
            searchList.Add(new SearchValueData { Name = "CareTypeID", Value = Convert.ToString(searchVisitTaskListPage.CareTypeID) });
        }

        public ServiceResponse SaveRefVisitTaskList(RefVisitTaskModel model, long loggedInUserId)
        {
            bool toggle = model.toggle;
            ServiceResponse response = new ServiceResponse();
            //AddReferralVisitTask
            var searchList = new List<SearchValueData>();

            if (toggle)//old UI
            {
                searchList.Add(new SearchValueData() { Name = "VisitTaskID", Value = Convert.ToString(model.VisitTaskID) });
                searchList.Add(new SearchValueData() { Name = "IsRequired", Value = Convert.ToString(model.IsRequired) });
                searchList.Add(new SearchValueData() { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
                searchList.Add(new SearchValueData() { Name = "LoggedIn", Value = Convert.ToString(loggedInUserId) });
                searchList.Add(new SearchValueData() { Name = "SystemID", Value = Common.GetMAcAddress() });
                searchList.Add(new SearchValueData() { Name = "ListOfIdsInCsv", Value = Convert.ToString(model.ListOfIdsInCsv) });
                var data1 = (int)GetScalar(StoredProcedure.AddReferralVisitTask, searchList);

                if (data1 == -1)
                {
                    response.Message = Resource.TaskAlreadyExist;

                }
                else
                {
                    response.IsSuccess = true;
                }

            }

            if (!toggle)//New UI
            {
                searchList.Add(new SearchValueData() { Name = "VisitTaskID", Value = Convert.ToString(model.VisitTaskID) });
                searchList.Add(new SearchValueData() { Name = "IsRequired", Value = Convert.ToString(model.IsRequired) });
                searchList.Add(new SearchValueData() { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
                searchList.Add(new SearchValueData() { Name = "LoggedIn", Value = Convert.ToString(loggedInUserId) });
                searchList.Add(new SearchValueData() { Name = "SystemID", Value = Common.GetMAcAddress() });
                //searchList.Add(new SearchValueData() { Name = "ListOfIdsInCsv", Value = Convert.ToString(model.ListOfIdsInCsv) });
                searchList.Add(new SearchValueData() { Name = "Frequency", Value = Convert.ToString(model.Frequency) });
                searchList.Add(new SearchValueData() { Name = "Days", Value = Convert.ToString(model.Days) });
                searchList.Add(new SearchValueData() { Name = "Comment", Value = Convert.ToString(model.Comment) });
                var data = (int)GetScalar(StoredProcedure.MapReferralVisitTask, searchList);
                response.Data = data;

                if (data == 1)
                {
                    response.Message = Resource.UpdateTaskMapDays;
                    response.IsSuccess = true;
                }
                if (data == 2)
                {
                    response.Message = Resource.AddTaskMapDays;
                    response.IsSuccess = true;
                }
            }


            return response;
        }

        public ServiceResponse SaveBulkRefVisitTaskList(List<RefVisitTaskModel> model, long loggedInUserId)
        {
            ServiceResponse response = new ServiceResponse();
            //AddReferralVisitTask

            List<RefVisitTaskModel> List = model;
            foreach (var item in List)
            {
                item.ReferralID = Convert.ToInt64(Crypto.Decrypt(item.EncryptedReferralID));
                var searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData() { Name = "VisitTaskID", Value = Convert.ToString(item.VisitTaskID) });
                searchList.Add(new SearchValueData() { Name = "IsRequired", Value = Convert.ToString(item.IsRequired) });
                searchList.Add(new SearchValueData() { Name = "ReferralID", Value = Convert.ToString(item.ReferralID) });
                searchList.Add(new SearchValueData() { Name = "LoggedIn", Value = Convert.ToString(loggedInUserId) });
                searchList.Add(new SearchValueData() { Name = "SystemID", Value = Common.GetMAcAddress() });
                var data = (int)GetScalar(StoredProcedure.AddReferralVisitTask, searchList);

                if (data == -1)
                {
                    response.Message = Resource.TaskAlreadyExist;

                }
                else
                {
                    response.IsSuccess = true;
                }
            }


            return response;
        }
        public ServiceResponse UpdateGoalIsActiveIsDeletedFlag(SearchVisitTaskListPage model)
        {
            var response = new ServiceResponse();
            var searchParameter = new List<SearchValueData>();
            try
            {
                searchParameter.Add(new SearchValueData("GoalIDs", model.GoalIDs));
                searchParameter.Add(new SearchValueData("IsActive", model.IsActive.ToString()));
                searchParameter.Add(new SearchValueData("IsDeleted", model.IsDeleted.ToString()));
                searchParameter.Add(new SearchValueData("UpdatedBy", Convert.ToString(SessionHelper.LoggedInID)));

                int data = (int)GetScalar(StoredProcedure.UpdateGoalIsActiveIsDeletedFlag, searchParameter);

                if (data == 1)
                {
                    response.IsSuccess = true;
                    response.Message = string.Format(Resource.RecordUpdatedSuccessfully, "Goal ");
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }
        public ServiceResponse SaveTaskDetail(TaskModel model)
        {
            var response = new ServiceResponse();
            var searchParameter = new List<SearchValueData>();
            CacheHelper cache = new CacheHelper();
            try
            {
                searchParameter.Add(new SearchValueData("ReferralTaskMappingID", Convert.ToString(model.ReferralTaskMappingID)));
                searchParameter.Add(new SearchValueData("Comment", model.Comment));
                searchParameter.Add(new SearchValueData("Frequency", model.Frequency));
                searchParameter.Add(new SearchValueData("SystemID", Common.GetHostAddress()));
                searchParameter.Add(new SearchValueData("UpdatedDate", Common.GetOrgCurrentDateTime().ToString(Constants.DbDateTimeFormat)));
                searchParameter.Add(new SearchValueData("LoggedInID", Convert.ToString(SessionHelper.LoggedInID)));

                int data = (int)GetScalar(StoredProcedure.SaveTaskDetail, searchParameter);

                if (data == 1)
                {
                    response.IsSuccess = true;
                    response.Message = Resource.DetaiSavedSuccessfully;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse GetPatientTaskMappings(RefVisitTaskModel model)
        {
            ServiceResponse response = new ServiceResponse();
            var searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData() { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
            searchList.Add(new SearchValueData() { Name = "CareTypeID", Value = Convert.ToString(model.CareTypeID) });
            searchList.Add(new SearchValueData() { Name = "TaskTypeTask", Value = Convert.ToString(VisitTask.TaskType.Task) });
            searchList.Add(new SearchValueData() { Name = "TaskTypeConclusion", Value = Convert.ToString(VisitTask.TaskType.Conclusion) });
            searchList.Add(new SearchValueData() { Name = "DDType_TaskFrequencyCode", Value = Convert.ToString((int)Common.DDType.TaskFrequencyCode) });
            searchList.Add(new SearchValueData() { Name = "VisitTaskDetail", Value = Convert.ToString(model.VisitTaskDetail) });
            var data = GetMultipleEntity<RefVisitTaskListModel>(StoredProcedure.GetPatientTaskMappings, searchList);
            response.Data = data;
            response.IsSuccess = true;
            return response;
        }
        public ServiceResponse GetReferralTaskMappingDetails(SearchVisitTaskListPage model)
        {
            ServiceResponse response = new ServiceResponse();
            var searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData() { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
            searchList.Add(new SearchValueData() { Name = "VisitTaskType", Value = Convert.ToString(model.VisitTaskType) });
            searchList.Add(new SearchValueData() { Name = "CareType", Value = Convert.ToString(model.CareTypeID) });

            var data = GetMultipleEntity<ReferralTaskMappingDetailsModel>(StoredProcedure.GetReferralTaskMapping, searchList);
            response.Data = data;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse GetReferralTaskMappingReports(SearchVisitTaskListPage model)
        {
            ServiceResponse response = new ServiceResponse();
            var searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData() { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });

            var data = GetMultipleEntity<ReferralTaskMappingReportModel>(StoredProcedure.GetReferralTaskMappingReport, searchList);
            response.Data = data;
            response.IsSuccess = true;
            return response;
        }
        public ServiceResponse GetReferralGoal(SearchVisitTaskListPage model)
        {
            ServiceResponse response = new ServiceResponse();
            var searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData() { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });

            var data = GetEntityList<GoalInfoReport>(StoredProcedure.GetReferralGoal, searchList);
            response.Data = data;
            response.IsSuccess = true;
            return response;
        }
        public ServiceResponse SaveReferralGoal(SearchVisitTaskListPage model, long loggedInUserId)
        {
            ServiceResponse response = new ServiceResponse();
            //AddReferralVisitTask
            var searchList = new List<SearchValueData>();

            searchList.Add(new SearchValueData() { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
            searchList.Add(new SearchValueData() { Name = "Goal", Value = Convert.ToString(model.Goal) });
            searchList.Add(new SearchValueData() { Name = "CreatedBy", Value = Convert.ToString(loggedInUserId) });
            searchList.Add(new SearchValueData() { Name = "IsActive", Value = Convert.ToString(model.IsActive) });
            searchList.Add(new SearchValueData() { Name = "IsDeleted", Value = Convert.ToString(model.IsDeleted) });
            searchList.Add(new SearchValueData() { Name = "GoalID", Value = Convert.ToString(model.GoalID) });
            var data = (int)GetScalar(StoredProcedure.AddReferralGoal, searchList);
            response.Data = data;

            if (data == 1)
            {
                response.Message = Resource.UpdateGoal;
                response.IsSuccess = true;

            }
            if (data == 2)
            {
                response.Message = Resource.SaveGoal;
                response.IsSuccess = true;
            }

            return response;
        }

        public ServiceResponse DeleteRefTaskMapping(RefVisitTaskModel model, long loggedInUserId)
        {
            ServiceResponse response = new ServiceResponse();
            var searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData() { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
            searchList.Add(new SearchValueData() { Name = "ReferralTaskMappingID", Value = Convert.ToString(model.ReferralTaskMappingID) });
            searchList.Add(new SearchValueData() { Name = "TaskTypeTask", Value = Convert.ToString(VisitTask.TaskType.Task) });
            searchList.Add(new SearchValueData() { Name = "TaskTypeConclusion", Value = Convert.ToString(VisitTask.TaskType.Conclusion) });
            searchList.Add(new SearchValueData() { Name = "LoggedIn", Value = Convert.ToString(loggedInUserId) });
            searchList.Add(new SearchValueData() { Name = "ListOfIdsInCsv", Value = Convert.ToString(model.ListOfIdsInCsv) });
            var data = GetMultipleEntity<RefVisitTaskListModel>(StoredProcedure.DeleteRefTaskMapping, searchList);
            response.Data = data;
            response.IsSuccess = true;
            return response;
        }


        public ServiceResponse OnTaskChecked(RefVisitTaskModel model, long loggedInUserId)
        {
            ServiceResponse response = new ServiceResponse();
            var searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData() { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
            searchList.Add(new SearchValueData() { Name = "IsRequired", Value = Convert.ToString(model.IsRequired) });
            searchList.Add(new SearchValueData() { Name = "ReferralTaskMappingID", Value = Convert.ToString(model.ReferralTaskMappingID) });
            searchList.Add(new SearchValueData() { Name = "TaskTypeTask", Value = Convert.ToString(VisitTask.TaskType.Task) });
            searchList.Add(new SearchValueData() { Name = "TaskTypeConclusion", Value = Convert.ToString(VisitTask.TaskType.Conclusion) });
            searchList.Add(new SearchValueData() { Name = "LoggedIn", Value = Convert.ToString(loggedInUserId) });
            var data = GetMultipleEntity<RefVisitTaskListModel>(StoredProcedure.OnTaskChecked, searchList);
            response.Data = data;
            response.IsSuccess = true;
            return response;
        }
        public ServiceResponse GetCaretype()
        {
            ServiceResponse response = new ServiceResponse();

            var searchlist = new List<SearchValueData>
            {
            };
            List<CareTypeModel> totalData = GetEntityList<CareTypeModel>(StoredProcedure.GetCareTypes, searchlist);
            response.IsSuccess = true;
            response.Data = totalData;
            return response;
        }
        public ServiceResponse GetCarePlanCaretypes(long ReferralID)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> paramList = new List<SearchValueData>();
            paramList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(ReferralID) });
            List<CareTypeModel> totalData = GetEntityList<CareTypeModel>(StoredProcedure.GetCarePlanCareTypes, paramList);
            response.IsSuccess = true;
            response.Data = totalData;
            return response;
        }

        public ServiceResponse GetVisitTaskCategory(string VisitTaskType, long CareType)
        {
            var response = new ServiceResponse();
            try
            {
                AddVisitTaskModel model = new AddVisitTaskModel();
                List<VisitTaskCategoryModel> categoryList = GetEntityList<VisitTaskCategoryModel>(StoredProcedure.GetVisitTaskByCaretype, new List<SearchValueData>
                {
                    new SearchValueData {Name = "VisitTaskType",Value =VisitTaskType },
                    new SearchValueData{Name = "CareType",Value=Convert.ToString(CareType) }
                });
                response.IsSuccess = true;
                response.Data = categoryList;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse GetTaskByActivity(string VisitTaskType, long CareType, long VisitTaskCategoryId)
        {
            var response = new ServiceResponse();
            try
            {
                List<VisitTask> categoryList = GetEntityList<VisitTask>(StoredProcedure.GetTaskListByActivity, new List<SearchValueData>
                {
                    new SearchValueData {Name = "VisitTaskType",Value=Convert.ToString(VisitTaskType) },
                     new SearchValueData {Name = "CareType",Value=Convert.ToString(CareType) },
                     new SearchValueData {Name = "VisitTaskCategoryId",Value=Convert.ToString(VisitTaskCategoryId) }
                });
                response.IsSuccess = true;
                response.Data = categoryList;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        #endregion

        #region Block Employee Related Stuff

        public ServiceResponse GetBlockEmpList(SearchRefBlockEmpListModel model, int pageIndex, int pageSize,
            string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData() { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));
            List<BlockEmployeeList> totalData = GetEntityList<BlockEmployeeList>(StoredProcedure.GetBlockEmpList, searchList);
            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<BlockEmployeeList> visitTaskList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = visitTaskList;
            response.IsSuccess = true;
            return response;
        }


        public ServiceResponse SaveBlockEmp(ReferralBlockedEmployee model, SearchRefBlockEmpListModel searchModel, long loggedInId, int pageIndex, int pageSize,
            string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData() { Name = "ReferralBlockedEmployeeID", Value = Convert.ToString(model.ReferralBlockedEmployeeID) });
            searchList.Add(new SearchValueData() { Name = "EmployeeID", Value = Convert.ToString(model.EmployeeID) });
            searchList.Add(new SearchValueData() { Name = "BlockingReason", Value = Convert.ToString(model.BlockingReason) });
            searchList.Add(new SearchValueData() { Name = "BlockingRequestedBy", Value = Convert.ToString(model.BlockingRequestedBy) });
            searchList.Add(new SearchValueData() { Name = "ReferralID", Value = Convert.ToString(searchModel.ReferralID) });
            searchList.Add(new SearchValueData() { Name = "LoggedInID", Value = Convert.ToString(loggedInId) });
            searchList.Add(new SearchValueData() { Name = "SystemID", Value = Common.GetHostAddress() });

            var data = (int)GetScalar(StoredProcedure.SaveBlockEmp, searchList);

            ServiceResponse response01 = GetBlockEmpList(searchModel, pageIndex, pageSize, sortIndex, sortDirection);
            response.Data = response01.Data;

            if (data == -1)
                response.Message = Resource.EmployeeAlreadyBlocked;

            if (data == 1)
            {
                response.Message = model.ReferralBlockedEmployeeID > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.BlockEmployees)
                    : string.Format(Resource.RecordCreatedSuccessfully, Resource.BlockEmployees);
                response.IsSuccess = true;
            }
            return response;
        }



        public ServiceResponse DeleteBlockEmp(ReferralBlockedEmployee model, SearchRefBlockEmpListModel searchModel, long loggedInId, int pageIndex, int pageSize,
            string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData() { Name = "ReferralBlockedEmployeeID", Value = Convert.ToString(model.ReferralBlockedEmployeeID) });
            searchList.Add(new SearchValueData() { Name = "LoggedInID", Value = Convert.ToString(loggedInId) });
            GetScalar(StoredProcedure.DeleteBlockEmp, searchList);

            ServiceResponse response01 = GetBlockEmpList(searchModel, pageIndex, pageSize, sortIndex, sortDirection);
            response.Data = response01.Data;

            response.Message = string.Format(Resource.RecordDeletedSuccessfully, Resource.BlockEmployees);
            response.IsSuccess = true;
            return response;
        }






        #endregion

        #region Patient Time Slot
        public ServiceResponse HC_ReferralTimeSlots()
        {
            var response = new ServiceResponse();


            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData>();
                searchParam.Add(new SearchValueData { Name = "DDType_CareType", Value = Convert.ToString((int)Common.DDType.CareType) });
                HC_RTSModel model = GetMultipleEntity<HC_RTSModel>(StoredProcedure.GetReferralTimeSlotsPageModel, searchParam);
                response.IsSuccess = true;
                response.Data = model;

            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }


            return response;
        }

        public ServiceResponse HC_ReferralTimeSlotss(string id)
        {
            var response = new ServiceResponse();


            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData>();
                searchParam.Add(new SearchValueData { Name = "DDType_CareType", Value = Convert.ToString((int)Common.DDType.CareType) });
                searchParam.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(id) });
                HC_RTSModel model = GetMultipleEntity<HC_RTSModel>(StoredProcedure.GetReferralTimeSlotsPageModel, searchParam);
                response.IsSuccess = true;
                response.Data = model;

            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }


            return response;
        }


        #region RTS Msater
        public ServiceResponse GetRtsMasterlist(SearchRTSMaster searchRTSMaster, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchRTSMaster != null)
                SetSearchFilterForEtsMasterListPage(searchRTSMaster, searchList);

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            //List<ListRTSMaster> totalData = GetEntityList<ListRTSMaster>(StoredProcedure.GetRtsMasterList, searchList);
            List<ListRTSMaster> totalData = GetEntityList<ListRTSMaster>(StoredProcedure.GetReferralTimeSlot, searchList);
            //foreach(ListRTSMaster item in totalData)
            //{
            //    item.StartDate = DateTime.Now;
            //}
            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListRTSMaster> getEmployeeList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = getEmployeeList;
            response.IsSuccess = true;
            return response;
        }
        public ServiceResponse GetReferralTimeSlotDetail(SearchReferralTimeSlotDetail searchReferralTimeSlotDetail, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchReferralTimeSlotDetail != null)
                SetSearchFilterForTimeSlotDetail(searchReferralTimeSlotDetail, searchList);

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<ListReferralTimeSlotDetail> totalData = GetEntityList<ListReferralTimeSlotDetail>(StoredProcedure.GetReferralTimeSlotDetail, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListReferralTimeSlotDetail> getreferralDetail = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = getreferralDetail;
            response.IsSuccess = true;
            return response;
        }

        private static void SetSearchFilterForEtsMasterListPage(SearchRTSMaster searchETSMaster, List<SearchValueData> searchList)
        {
            searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(searchETSMaster.ReferralID) });
            if (searchETSMaster.StartDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(searchETSMaster.StartDate).ToString(Constants.DbDateFormat) });
            if (searchETSMaster.EndDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(searchETSMaster.EndDate).ToString(Constants.DbDateFormat) });
            //if (searchETSMaster.Filter.HasValue)
            searchList.Add(new SearchValueData { Name = "Filter", Value = Convert.ToString(searchETSMaster.Filter) });

            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchETSMaster.IsDeleted) });
        }
        private static void SetSearchFilterForTimeSlotDetail(SearchReferralTimeSlotDetail searchReferralTimeSlotDetail, List<SearchValueData> searchList)
        {
            searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(searchReferralTimeSlotDetail.ReferralID) });
            searchList.Add(new SearchValueData { Name = "ReferralTimeSlotMasterID", Value = Convert.ToString(searchReferralTimeSlotDetail.ReferralTimeSlotMasterID) });

            if ((searchReferralTimeSlotDetail.StartDate.HasValue && searchReferralTimeSlotDetail.StartDate.Value.ToString(Constants.DefaultDateFormat) != Constants.DefaultDateValue))
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(searchReferralTimeSlotDetail.StartDate).ToString(Constants.DbDateFormat) });

            if ((searchReferralTimeSlotDetail.EndDate.HasValue && searchReferralTimeSlotDetail.EndDate.Value.ToString(Constants.DefaultDateFormat) != Constants.DefaultDateValue))
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(searchReferralTimeSlotDetail.EndDate).ToString(Constants.DbDateFormat) });


            searchList.Add(new SearchValueData { Name = "ClientName", Value = Convert.ToString(searchReferralTimeSlotDetail.ClientName) });
        }

        public ServiceResponse DeleteRtsMaster(SearchRTSMaster searchRTSMaster, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

            SetSearchFilterForEtsMasterListPage(searchRTSMaster, searchList);

            if (!string.IsNullOrEmpty(searchRTSMaster.ListOfIdsInCsv))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = searchRTSMaster.ListOfIdsInCsv });

            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserID) });


            DeleteRTSDetail result = GetMultipleEntity<DeleteRTSDetail>(StoredProcedure.DeleteRtsMaster, searchList);

            if (result.TransactionResultId == -3)
            {
                response.Message = Resource.VisitExitsDelete;
                return response;
            }

            List<ListRTSMaster> totalData = result.Data;
            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;


            Page<ListRTSMaster> getEmployeeList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = getEmployeeList;
            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.PatientTimeSlot);
            return response;
        }

        public ServiceResponse AddRtsMaster(ReferralTimeSlotMaster model, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> paramList = new List<SearchValueData>();
            paramList.Add(new SearchValueData { Name = "ReferralTimeSlotMasterID", Value = Convert.ToString(model.ReferralTimeSlotMasterID) });
            paramList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
            paramList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(model.StartDate).ToString(Constants.DbDateFormat) });
            paramList.Add(new SearchValueData { Name = "EndDate", Value = model.EndDate != null ? Convert.ToDateTime(model.EndDate).ToString(Constants.DbDateFormat) : null });
            paramList.Add(new SearchValueData { Name = "IsEndDateAvailable", Value = Convert.ToString(model.IsEndDateAvailable) });
            paramList.Add(new SearchValueData { Name = "TodayDate", Value = Common.GetOrgCurrentDateTime().ToString(Constants.DbDateFormat) });
            paramList.Add(new SearchValueData { Name = "SlotEndDate", Value = Common.GetOrgCurrentDateTime().AddDays(ConfigSettings.GenerateEmpRefTimeScheduleDays).ToString(Constants.DbDateFormat) });
            //if (model.IsEndDateAvailable==true)
            //{
            //    paramList.Add(new SearchValueData { Name = "SlotEndDate", Value = model.EndDate.Value.ToString(Constants.DbDateFormat) });
            //}
            paramList.Add(new SearchValueData { Name = "loggedInUserID", Value = Convert.ToString(loggedInUserID) });
            paramList.Add(new SearchValueData { Name = "SystemID", Value = Common.GetHostAddress() });
            paramList.Add(new SearchValueData { Name = "IsWithPriorAuth", Value = Convert.ToString(model.IsWithPriorAuth) });
            paramList.Add(new SearchValueData { Name = "ReferralBillingAuthorizationID", Value = Convert.ToString(model.ReferralBillingAuthorizationID) });
            paramList.Add(new SearchValueData { Name = "CareTypeID", Value = Convert.ToString(model.CareTypeID) });
            TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.AddRtsMaster, paramList);

            if (result.TransactionResultId == -1)
            {
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
                return response;
            }
            if (result.TransactionResultId == -2)
            {
                response.Message = Resource.DateRangeExist;
                return response;
            }
            if (result.TransactionResultId == -3)
            {
                response.Message = Resource.VisitExitsChangeDate;
                return response;
            }
            if (result.TransactionResultId == -5)
            {
                response.IsSuccess = false;
                response.Message = Resource.HCBillingAuthorizationValidation;
                return response;
            }

            response.Message = model.ReferralTimeSlotMasterID > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.DateRange) :
                    string.Format(Resource.RecordCreatedSuccessfully, Resource.DateRange);
            response.IsSuccess = true;
            response.Data = result.TablePrimaryId > 0 ? result.TablePrimaryId : model.ReferralTimeSlotMasterID;
            return response;
        }

        public ServiceResponse AddRtsByPriorAuth(ReferralTimeSlotMaster model, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> paramList = new List<SearchValueData>();
            paramList.Add(new SearchValueData { Name = "ReferralTimeSlotMasterID", Value = Convert.ToString(model.ReferralTimeSlotMasterID) });
            paramList.Add(new SearchValueData { Name = "ReferralBillingAuthorizationID", Value = Convert.ToString(model.ReferralBillingAuthorizationID) });
            paramList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
            paramList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(model.StartDate).ToString(Constants.DbDateFormat) });
            paramList.Add(new SearchValueData { Name = "EndDate", Value = model.EndDate != null ? Convert.ToDateTime(model.EndDate).ToString(Constants.DbDateFormat) : null });
            paramList.Add(new SearchValueData { Name = "IsEndDateAvailable", Value = "1" });
            paramList.Add(new SearchValueData { Name = "TodayDate", Value = Common.GetOrgCurrentDateTime().ToString(Constants.DbDateFormat) });
            paramList.Add(new SearchValueData { Name = "SlotEndDate", Value = Common.GetOrgCurrentDateTime().AddDays(ConfigSettings.GenerateEmpRefTimeScheduleDays).ToString(Constants.DbDateFormat) });
            paramList.Add(new SearchValueData { Name = "loggedInUserID", Value = Convert.ToString(loggedInUserID) });
            paramList.Add(new SearchValueData { Name = "SystemID", Value = Common.GetHostAddress() });
            paramList.Add(new SearchValueData { Name = "IsWithPriorAuth", Value = Convert.ToString(model.IsWithPriorAuth) });
            paramList.Add(new SearchValueData { Name = "CareTypeID", Value = Convert.ToString(model.CareTypeID) });
            TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.AddRtsMaster, paramList);

            if (result.TransactionResultId == -1)
            {
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
                return response;
            }
            if (result.TransactionResultId == -2)
            {
                response.Message = Resource.DateRangeExist;
                return response;
            }

            response.Message = model.ReferralTimeSlotMasterID > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.Schedule) :
                    string.Format(Resource.RecordCreatedSuccessfully, Resource.DateRange);
            response.IsSuccess = true;

            #region Referral Time Slots
            if (SessionHelper.IsCaseManagement)
            {
                AddRTSDetailCaseManagement(model.ReferralTimeSlotMasterID > 0 ? model.ReferralTimeSlotMasterID : result.TablePrimaryId, model, loggedInUserID);
            }
            //switch (data)
            //{
            //    case (int)Common.TimeSlotSaveResult.SQLException:
            //        response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            //        response.IsSuccess = false;
            //        break;
            //    case (int)Common.TimeSlotSaveResult.TimeSlotExistsNoRecordsAdded:
            //        response.Message = Resource.TimeSlotExist;
            //        response.IsSuccess = false;
            //        break;
            //    case (int)Common.TimeSlotSaveResult.TimeSlotExistInAnotherDateRange:
            //        //response.Message = "This time slot is exist in another date range please check";
            //        response.IsSuccess = false;
            //        break;
            //    case (int)Common.TimeSlotSaveResult.TimeSlotsAdded:
            //        response.Message = string.Format(Resource.RecordCreatedSuccessfully, Resource.TimeSlots);
            //        response.IsSuccess = true;
            //        break;
            //    case (int)Common.TimeSlotSaveResult.TimeSlotsPartiallyAdded:
            //        response.Message = Resource.NonConflictingTimeSlotsAdded;
            //        response.IsSuccess = true;
            //        break;
            //}

            #endregion Referral Time Slots

            #region Referral Time Slot Dates

            //paramList = new List<SearchValueData>
            //                {
            //                    new SearchValueData {Name = "StartDate",Value =model.StartDate.ToString(Constants.DbDateFormat)},
            //                    new SearchValueData {Name = "EndDate",Value =model.EndDate.Value.ToString(Constants.DbDateFormat)},
            //                    new SearchValueData {Name = "ReferralID",Value = Convert.ToString(model.ReferralID)},
            //                    new SearchValueData {Name = "ReferralTimeSlotMasterID",Value = Convert.ToString(model.ReferralTimeSlotMasterID)},
            //                    new SearchValueData {Name = "GeneratePatientSchedules",Value = "1"},
            //                    new SearchValueData {Name = "LoggedInID",Value = Convert.ToString(loggedInUserID)}
            //                };
            //GetScalar(StoredProcedure.GenerateReferralTimeSlotDates_ForDayCare, paramList);

            #endregion Referral Time Slot Dates

            response.Data = result.TablePrimaryId > 0 ? result.TablePrimaryId : model.ReferralTimeSlotMasterID;
            return response;
        }

        public void AddRTSDetailCaseManagement(long referralTimeSlotMasterID, ReferralTimeSlotMaster model, long loggedInUserID)
        {
            try
            {
                ReferralTimeSlotDetail detailModel = new ReferralTimeSlotDetail();
                detailModel.ReferralTimeSlotMasterID = referralTimeSlotMasterID;
                detailModel.ReferralID = model.ReferralID;
                detailModel.StartTime = new TimeSpan(0, 1, 0);
                detailModel.EndTime = new TimeSpan(23, 59, 0);
                detailModel.UsedInScheduling = true;

                List<SearchValueData> paramList = new List<SearchValueData>();
                paramList.Add(new SearchValueData { Name = "ReferralTimeSlotDetailID", Value = Convert.ToString(detailModel.ReferralTimeSlotDetailID) });
                paramList.Add(new SearchValueData { Name = "ReferralTimeSlotMasterID", Value = Convert.ToString(detailModel.ReferralTimeSlotMasterID) });
                paramList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(detailModel.ReferralID) });
                //paramList.Add(new SearchValueData { Name = "Day", Value = Convert.ToString(detailModel.Day) });
                paramList.Add(new SearchValueData { Name = "StartTime", Value = Convert.ToString(detailModel.StartTime) });
                paramList.Add(new SearchValueData { Name = "EndTime", Value = Convert.ToString(detailModel.EndTime) });
                paramList.Add(new SearchValueData { Name = "UsedInScheduling", Value = Convert.ToString(detailModel.UsedInScheduling) });
                paramList.Add(new SearchValueData { Name = "Notes", Value = Convert.ToString(detailModel.Notes) });
                paramList.Add(new SearchValueData { Name = "loggedInUserID", Value = Convert.ToString(loggedInUserID) });
                paramList.Add(new SearchValueData { Name = "SystemID", Value = Common.GetHostAddress() });
                paramList.Add(new SearchValueData { Name = "SelectedDays", Value = detailModel.SelectedDays == null ? string.Empty : string.Join(",", detailModel.SelectedDays) });
                paramList.Add(new SearchValueData { Name = "TodayDate", Value = model.StartDate.ToString(Constants.DbDateFormat) });
                paramList.Add(new SearchValueData { Name = "SlotEndDate", Value = model.EndDate.Value.ToString(Constants.DbDateFormat) });
                paramList.Add(new SearchValueData { Name = "CareTypeId", Value = Convert.ToString(detailModel.CareTypeId) });
                paramList.Add(new SearchValueData { Name = "GeneratePatientSchedules", Value = "0" });
                int data = (int)GetScalar(StoredProcedure.AddRtsDetailByPriorAuth, paramList);
            }
            catch (Exception ex)
            {

            }

        }
        public List<ReferralBillingAuthorizations> GetReferralAuthorizationsByReferralId(long referralID, long careTypeId)
        {
            List<ReferralBillingAuthorizations> referralAuthorizations = GetEntityList<ReferralBillingAuthorizations>(StoredProcedure.GetReferralAuthorizations,
                new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralID", Value = Convert.ToString(referralID)},
                    new SearchValueData {Name = "CareTypeID", Value = Convert.ToString(careTypeId)}
                });
            return referralAuthorizations;
        }
        public ServiceResponse GetReferralAuthorizationsByReferralID(long referralID, long careTypeId)
        {
            var serviceResponse = new ServiceResponse();
            //List<ReferralBillingAuthorization> referralAuthorizations = GetEntityList<ReferralBillingAuthorization>(StoredProcedure.GetReferralAuthorizations,
            List<ReferralBillingAuthorizations> referralAuthorizations = GetEntityList<ReferralBillingAuthorizations>(StoredProcedure.GetReferralAuthorizations,
                new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralID", Value = Convert.ToString(referralID)},
                    new SearchValueData {Name = "CareTypeID", Value = Convert.ToString(careTypeId)}
                });
            serviceResponse.Data = referralAuthorizations;
            if (referralAuthorizations.Count == 0)
            {
                serviceResponse.IsSuccess = false;
                serviceResponse.Message = Resource.PleaseAddAuthorization;
                return serviceResponse;
            }

            serviceResponse.IsSuccess = true;
            return serviceResponse;
        }


        #endregion

        #region RTS Detail
        public ServiceResponse GetRtsDetaillist(SearchRTSDetail searchRTSDetail, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchRTSDetail != null)
                SetSearchFilterForEtsDetailListPage(searchRTSDetail, searchList);

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<ListRTSDetails> totalData = GetEntityList<ListRTSDetails>(StoredProcedure.GetRtsDetailList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListRTSDetails> getEmployeeList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = getEmployeeList;
            response.IsSuccess = true;
            return response;
        }

        private static void SetSearchFilterForEtsDetailListPage(SearchRTSDetail searchRTSDetail, List<SearchValueData> searchList)
        {
            searchList.Add(new SearchValueData { Name = "ReferralTimeSlotMasterID", Value = Convert.ToString(searchRTSDetail.ReferralTimeSlotMasterID) });
            searchList.Add(new SearchValueData { Name = "CareTypeID", Value = Convert.ToString(searchRTSDetail.CareTypeId) });
        }

        public ServiceResponse DeleteRtsDetail(SearchRTSDetail searchRTSDetail, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

            SetSearchFilterForEtsDetailListPage(searchRTSDetail, searchList);

            if (!string.IsNullOrEmpty(searchRTSDetail.ListOfIdsInCsv))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = searchRTSDetail.ListOfIdsInCsv });
            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserID) });

            List<ListRTSDetails> totalData = GetEntityList<ListRTSDetails>(StoredProcedure.DeleteRtsDetail, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;


            Page<ListRTSDetails> getEmployeeList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = getEmployeeList;
            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.PatientTimeSlot);
            return response;
        }

        public ServiceResponse AddRtsDetail(ReferralTimeSlotDetail model, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> paramList = new List<SearchValueData>();
            paramList.Add(new SearchValueData { Name = "ReferralTimeSlotDetailID", Value = Convert.ToString(model.ReferralTimeSlotDetailID) });
            paramList.Add(new SearchValueData { Name = "ReferralTimeSlotMasterID", Value = Convert.ToString(model.ReferralTimeSlotMasterID) });
            paramList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
            paramList.Add(new SearchValueData { Name = "Day", Value = Convert.ToString(model.Day) });
            paramList.Add(new SearchValueData { Name = "StartTime", Value = Convert.ToString(model.StartTime) });
            paramList.Add(new SearchValueData { Name = "EndTime", Value = Convert.ToString(model.EndTime) });
            paramList.Add(new SearchValueData { Name = "UsedInScheduling", Value = Convert.ToString(model.UsedInScheduling) });
            paramList.Add(new SearchValueData { Name = "Notes", Value = Convert.ToString(model.Notes) });
            paramList.Add(new SearchValueData { Name = "loggedInUserID", Value = Convert.ToString(loggedInUserID) });
            paramList.Add(new SearchValueData { Name = "SystemID", Value = Common.GetHostAddress() });
            paramList.Add(new SearchValueData { Name = "SelectedDays", Value = model.SelectedDays == null ? string.Empty : string.Join(",", model.SelectedDays) });
            //paramList.Add(new SearchValueData { Name = "TodayDate", Value = Common.GetOrgCurrentDateTime().ToString(Constants.DbDateFormat) });
            //paramList.Add(new SearchValueData { Name = "SlotEndDate", Value = Common.GetOrgCurrentDateTime().AddDays(ConfigSettings.GenerateEmpRefTimeScheduleDays).ToString(Constants.DbDateFormat) });
            paramList.Add(new SearchValueData { Name = "TodayDate", Value = model.StartDate.ToString(Constants.DbDateFormat) });
            //if (!string.IsNullOrEmpty(model.EndDate))
            if (model.EndDate != null)
            {
                // paramList.Add(new SearchValueData { Name = "SlotEndDate", Value = model.EndDate.ToString(Constants.DbDateFormat) });
                paramList.Add(new SearchValueData { Name = "SlotEndDate", Value = Common.GetOrgCurrentDateTime().AddDays(ConfigSettings.GenerateEmpRefTimeScheduleDays).ToString(Constants.DbDateFormat) });
            }
            else
            {
                paramList.Add(new SearchValueData { Name = "SlotEndDate", Value = Common.GetOrgCurrentDateTime().AddDays(ConfigSettings.GenerateEmpRefTimeScheduleDays).ToString(Constants.DbDateFormat) });
            }
            paramList.Add(new SearchValueData { Name = "CareTypeId", Value = Convert.ToString(model.CareTypeId) });
            paramList.Add(new SearchValueData { Name = "GeneratePatientSchedules", Value = SessionHelper.IsDayCare ? "1" : "0" });
            paramList.Add(new SearchValueData { Name = "IsForcePatientSchedules", Value = model.IsForcePatientSchedules ? "1" : "0" });
            paramList.Add(new SearchValueData { Name = "AnyTimeClockIn", Value = model.AnyTimeClockIn ? "1" : "0" });
            paramList.Add(new SearchValueData { Name = "ReferralBillingAuthorizationID", Value = Convert.ToString(model.ReferralBillingAuthorizationID) });
            int data = (int)GetScalar(StoredProcedure.AddRtsDetail, paramList);

            switch (data)
            {
                case (int)Common.TimeSlotSaveResult.SQLException:
                    response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
                    response.IsSuccess = false;
                    break;
                case (int)Common.TimeSlotSaveResult.TimeSlotExistsNoRecordsAdded:
                    response.Message = Resource.TimeSlotExist;
                    response.IsSuccess = false;
                    break;
                case (int)Common.TimeSlotSaveResult.TimeSlotExistInAnotherDateRange:
                    //response.Message = "This time slot is exist in another date range please check";
                    response.IsSuccess = false;
                    break;
                case (int)Common.TimeSlotSaveResult.TimeSlotsAdded:
                    response.Message = string.Format(Resource.RecordCreatedSuccessfully, Resource.TimeSlots);
                    response.IsSuccess = true;
                    break;
                case (int)Common.TimeSlotSaveResult.TimeSlotsPartiallyAdded:
                    response.Message = Resource.NonConflictingTimeSlotsAdded;
                    response.IsSuccess = true;
                    break;
                case (int)Common.TimeSlotSaveResult.TimeSlotIsMoreThanAllowed:
                    //response.Message = "This time slot is exist in another date range please check";
                    response.IsSuccess = false;
                    break;
                case (int)Common.TimeSlotSaveResult.ActiveBillingAuthorizationsIsRequired:
                    response.Message = Resource.HCBillingAuthorizationValidation;
                    response.IsSuccess = false;
                    break;
            }

            response.Data = data;
            return response;
        }

        public ServiceResponse UpdateRtsDetail(ReferralTimeSlotDetail model, SearchRTSDetail searchRTSDetail, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();

            //List<SearchValueData> paramList = new List<SearchValueData>();

            List<SearchValueData> paramList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);


            paramList.Add(new SearchValueData { Name = "ReferralTimeSlotDetailID", Value = Convert.ToString(model.ReferralTimeSlotDetailID) });
            paramList.Add(new SearchValueData { Name = "ReferralTimeSlotMasterID", Value = Convert.ToString(model.ReferralTimeSlotMasterID) });
            paramList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
            //paramList.Add(new SearchValueData { Name = "Day", Value = Convert.ToString(model.Day) });
            paramList.Add(new SearchValueData { Name = "StartTime", Value = Convert.ToString(model.StartTime) });
            paramList.Add(new SearchValueData { Name = "EndTime", Value = Convert.ToString(model.EndTime) });
            paramList.Add(new SearchValueData { Name = "UsedInScheduling", Value = Convert.ToString(model.UsedInScheduling) });
            paramList.Add(new SearchValueData { Name = "Notes", Value = Convert.ToString(model.Notes) });
            paramList.Add(new SearchValueData { Name = "loggedInUserID", Value = Convert.ToString(loggedInUserID) });
            paramList.Add(new SearchValueData { Name = "SystemID", Value = Common.GetHostAddress() });
            paramList.Add(new SearchValueData { Name = "SelectedDays", Value = Convert.ToString(model.Day) });
            paramList.Add(new SearchValueData { Name = "TodayDate", Value = Common.GetOrgCurrentDateTime().ToString(Constants.DbDateFormat) });
            paramList.Add(new SearchValueData { Name = "SlotEndDate", Value = Common.GetOrgCurrentDateTime().AddDays(ConfigSettings.GenerateEmpRefTimeScheduleDays).ToString(Constants.DbDateFormat) });
            paramList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = searchRTSDetail.ListOfIdsInCsv });
            paramList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(false) });
            paramList.Add(new SearchValueData { Name = "CareTypeId", Value = Convert.ToString(model.CareTypeId) });
            paramList.Add(new SearchValueData { Name = "GeneratePatientSchedules", Value = SessionHelper.IsDayCare ? "1" : "0" });
            paramList.Add(new SearchValueData { Name = "AnyTimeClockIn", Value = model.AnyTimeClockIn ? "1" : "0" });
            paramList.Add(new SearchValueData { Name = "ReferralBillingAuthorizationID", Value = Convert.ToString(model.ReferralBillingAuthorizationID) });
            int data = (int)GetScalar(StoredProcedure.UpdateRtsDetail, paramList);

            switch (data)
            {
                case (int)Common.TimeSlotSaveResult.SQLException:
                    response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
                    response.IsSuccess = false;
                    break;
                case (int)Common.TimeSlotSaveResult.TimeSlotExistsNoRecordsAdded:
                    response.Message = Resource.TimeSlotExist;
                    response.IsSuccess = false;
                    break;
                case (int)Common.TimeSlotSaveResult.TimeSlotExistInAnotherDateRange:
                    //response.Message = "This time slot is exist in another date range please check";
                    response.IsSuccess = false;
                    break;
                case (int)Common.TimeSlotSaveResult.TimeSlotsAdded:
                    response.Message = string.Format(Resource.RecordCreatedSuccessfully, Resource.TimeSlots);
                    response.IsSuccess = true;
                    break;
                case (int)Common.TimeSlotSaveResult.TimeSlotsPartiallyAdded:
                    response.Message = Resource.NonConflictingTimeSlotsAdded;
                    response.IsSuccess = true;
                    break;
            }
            response.Data = data;
            return response;
        }


        public ServiceResponse ReferralTimeSlotForceUpdate(ReferralTimeSlotDetail model, SearchRTSDetail searchRTSDetail, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> paramList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

            //List<SearchValueData> paramList = new List<SearchValueData>();
            paramList.Add(new SearchValueData { Name = "ReferralTimeSlotDetailID", Value = Convert.ToString(model.ReferralTimeSlotDetailID) });
            paramList.Add(new SearchValueData { Name = "ReferralTimeSlotMasterID", Value = Convert.ToString(model.ReferralTimeSlotMasterID) });
            paramList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
            //paramList.Add(new SearchValueData { Name = "Day", Value = Convert.ToString(model.Day) });
            paramList.Add(new SearchValueData { Name = "StartTime", Value = Convert.ToString(model.StartTime) });
            paramList.Add(new SearchValueData { Name = "EndTime", Value = Convert.ToString(model.EndTime) });
            paramList.Add(new SearchValueData { Name = "UsedInScheduling", Value = Convert.ToString(model.UsedInScheduling) });
            paramList.Add(new SearchValueData { Name = "Notes", Value = Convert.ToString(model.Notes) });
            paramList.Add(new SearchValueData { Name = "loggedInUserID", Value = Convert.ToString(loggedInUserID) });
            paramList.Add(new SearchValueData { Name = "SystemID", Value = Common.GetHostAddress() });
            paramList.Add(new SearchValueData { Name = "SelectedDays", Value = model.SelectedDays != null ? string.Join(",", model.SelectedDays) : Convert.ToString(model.Day) });
            paramList.Add(new SearchValueData { Name = "TodayDate", Value = Common.GetOrgCurrentDateTime().ToString(Constants.DbDateFormat) });
            paramList.Add(new SearchValueData { Name = "SlotEndDate", Value = Common.GetOrgCurrentDateTime().AddDays(ConfigSettings.GenerateEmpRefTimeScheduleDays).ToString(Constants.DbDateFormat) });
            paramList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = searchRTSDetail.ListOfIdsInCsv });
            paramList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(false) });
            paramList.Add(new SearchValueData { Name = "CareTypeId", Value = Convert.ToString(model.CareTypeId) });
            paramList.Add(new SearchValueData { Name = "IsEdit", Value = (searchRTSDetail.ReferralTimeSlotMasterID == 0) ? Convert.ToString(false) : Convert.ToString(true) });
            paramList.Add(new SearchValueData { Name = "GeneratePatientSchedules", Value = SessionHelper.IsDayCare ? "1" : "0" });
            paramList.Add(new SearchValueData { Name = "AnyTimeClockIn", Value = model.AnyTimeClockIn ? "1" : "0" });
            paramList.Add(new SearchValueData { Name = "ReferralBillingAuthorizationID", Value = Convert.ToString(model.ReferralBillingAuthorizationID) });
            int data = (int)GetScalar(StoredProcedure.ReferralTimeSlotForceUpdate, paramList);

            switch (data)
            {
                case (int)Common.TimeSlotSaveResult.SQLException:
                    response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
                    response.IsSuccess = false;
                    break;
                case (int)Common.TimeSlotSaveResult.TimeSlotExistsNoRecordsAdded:
                    response.Message = Resource.TimeSlotExist;
                    response.IsSuccess = false;
                    break;
                case (int)Common.TimeSlotSaveResult.TimeSlotExistInAnotherDateRange:
                    //response.Message = "This time slot is exist in another date range please check";
                    response.IsSuccess = false;
                    break;
                case (int)Common.TimeSlotSaveResult.TimeSlotsAdded:
                    response.Message = string.Format(Resource.RecordCreatedSuccessfully, Resource.TimeSlots);
                    response.IsSuccess = true;
                    break;
                case (int)Common.TimeSlotSaveResult.TimeSlotsPartiallyAdded:
                    response.Message = Resource.NonConflictingTimeSlotsAdded;
                    response.IsSuccess = true;
                    break;
            }
            response.Data = data;
            return response;
        }


        #endregion



        #endregion

        #region RCL Msater
        public ServiceResponse GetReferralCaseLoadList(SearchRCLMaster searchRclMaster, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchRclMaster != null)
                SetSearchFilterForRclMasterListPage(searchRclMaster, searchList);

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<ListRCLMaster> totalData = GetEntityList<ListRCLMaster>(StoredProcedure.GetReferralCaseLoadList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListRCLMaster> getEmployeeList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = getEmployeeList;
            response.IsSuccess = true;
            return response;
        }

        private static void SetSearchFilterForRclMasterListPage(SearchRCLMaster searchRclMaster, List<SearchValueData> searchList)
        {
            searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(searchRclMaster.ReferralID) });
            if (searchRclMaster.StartDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(searchRclMaster.StartDate).ToString(Constants.DbDateFormat) });
            if (searchRclMaster.EndDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(searchRclMaster.EndDate).ToString(Constants.DbDateFormat) });
        }

        public ServiceResponse RemoveReferralCaseLoad(SearchRCLMaster searchRclMaster, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

            SetSearchFilterForRclMasterListPage(searchRclMaster, searchList);

            if (!string.IsNullOrEmpty(searchRclMaster.ListOfIdsInCsv))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = searchRclMaster.ListOfIdsInCsv });
            if (!string.IsNullOrEmpty(searchRclMaster.CaseLoadType))
                searchList.Add(new SearchValueData { Name = "CaseLoadType", Value = searchRclMaster.CaseLoadType });
            searchList.Add(new SearchValueData { Name = "PermanenetCaseLoadType", Value = Common.CaseLoadTypeEnum.Permanent.ToString() });

            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserID) });

            List<ListRCLMaster> totalData = GetEntityList<ListRCLMaster>(StoredProcedure.RemoveReferralCaseLoad, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListRCLMaster> getEmployeeList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = getEmployeeList;
            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.CaseLoadTitle);
            return response;
        }

        public ServiceResponse AddRclMaster(ReferralCaseload model, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> paramList = new List<SearchValueData>();
            paramList.Add(new SearchValueData { Name = "ReferralCaseloadID", Value = Convert.ToString(model.ReferralCaseloadID) });
            paramList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
            paramList.Add(new SearchValueData { Name = "EmployeeID", Value = Convert.ToString(model.EmployeeID) });
            paramList.Add(new SearchValueData { Name = "CaseLoadType", Value = model.CaseLoadType });
            paramList.Add(new SearchValueData { Name = "PermanentCaseLoadType", Value = Common.CaseLoadTypeEnum.Permanent.ToString() });
            paramList.Add(new SearchValueData { Name = "StartDate", Value = model.StartDate != null ? Convert.ToDateTime(model.StartDate).ToString(Constants.DbDateFormat) : null });
            paramList.Add(new SearchValueData { Name = "EndDate", Value = model.EndDate != null ? Convert.ToDateTime(model.EndDate).ToString(Constants.DbDateFormat) : null });
            paramList.Add(new SearchValueData { Name = "loggedInUserID", Value = Convert.ToString(loggedInUserID) });
            paramList.Add(new SearchValueData { Name = "SystemID", Value = Common.GetHostAddress() });
            TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.AddReferralCaseLoad, paramList);

            if (result.TransactionResultId == -1)
            {
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
                return response;
            }
            if (result.TransactionResultId == -2)
            {
                response.Message = Resource.EmployeeAlreadyAssigned;
                return response;
            }
            if (result.TransactionResultId == -3)
            {
                response.Message = Resource.TemporaryCaseLoadExists;
                return response;
            }

            response.Message = model.ReferralCaseloadID > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.CaseLoadTitle) :
                    string.Format(Resource.RecordCreatedSuccessfully, Resource.Caseload);
            response.IsSuccess = true;
            response.Data = result.TablePrimaryId > 0 ? result.TablePrimaryId : model.ReferralCaseloadID;
            return response;
        }

        public ServiceResponse MarkCaseLoadComplete(ReferralCaseload model, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();

            ReferralCaseload rcl = GetEntity<ReferralCaseload>(model.ReferralCaseloadID);
            if (rcl != null)
            {
                rcl.EndDate = DateTime.Now;
                SaveObject(rcl, loggedInUserID);
                response.Message = model.ReferralCaseloadID > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.Caseload) :
                    string.Format(Resource.RecordCreatedSuccessfully, Resource.Caseload);
                response.IsSuccess = true;
                response.Data = rcl;
            }
            else
            {
                response.Message = Resource.RecordNotFound;
            }

            return response;
        }


        #endregion

        #region  Referral Payor Mapping

        public ServiceResponse HC_AddReferralPayorMapping(ReferralPayorMapping referralPayorMapping, long loggedInUserId)
        {
            var response = new ServiceResponse();
            try
            {
                bool isEditMode = referralPayorMapping.ReferralPayorMappingID > 0;
                referralPayorMapping.ReferralID = Convert.ToInt64(Crypto.Decrypt(referralPayorMapping.EncryptedReferralId));

                List<SearchValueData> searchModel = new List<SearchValueData>
                {
                    new SearchValueData{Name = "ReferralPayorMappingID", Value = referralPayorMapping.ReferralPayorMappingID.ToString()},
                    new SearchValueData{Name = "ReferralID",Value =referralPayorMapping.ReferralID.ToString()},
                    new SearchValueData{Name = "PayorID",Value = referralPayorMapping.PayorID.ToString()},
                    new SearchValueData{Name = "Precedence",Value = referralPayorMapping.Precedence.ToString()},
                    new SearchValueData{Name = "StartDate",Value =Convert.ToDateTime(referralPayorMapping.PayorEffectiveDate).ToString(Constants.DbDateFormat)},
                    new SearchValueData{Name = "EndDate",Value =Convert.ToDateTime(referralPayorMapping.PayorEffectiveEndDate).ToString(Constants.DbDateFormat)},
                    new SearchValueData{Name = "loggedInUserId",Value = Convert.ToString(loggedInUserId)},
                    new SearchValueData{Name = "CurrentDate", Value = Common.GetOrgCurrentDateTime().ToString(Constants.DbDateFormat)},//Convert.ToString(Common.GetOrgCurrentDateTime()) },
                    new SearchValueData{Name = "SystemID", Value = HttpContext.Current.Request.UserHostAddress },
                    new SearchValueData("IsPayorNotPrimaryInsured",Convert.ToString(referralPayorMapping.IsPayorNotPrimaryInsured)),
                    new SearchValueData("InsuredId",Convert.ToString(referralPayorMapping.InsuredId)),
                    new SearchValueData("InsuredFirstName",Convert.ToString(referralPayorMapping.InsuredFirstName)),
                    new SearchValueData("InsuredMiddleName",Convert.ToString(referralPayorMapping.InsuredMiddleName)),
                    new SearchValueData("InsuredLastName",Convert.ToString(referralPayorMapping.InsuredLastName)),
                    new SearchValueData("InsuredAddress",Convert.ToString(referralPayorMapping.InsuredAddress)),
                    new SearchValueData("InsuredCity",Convert.ToString(referralPayorMapping.InsuredCity)),
                    new SearchValueData("InsuredState",Convert.ToString(referralPayorMapping.InsuredState)),
                    new SearchValueData("InsuredZipCode",Convert.ToString(referralPayorMapping.InsuredZipCode)),
                    new SearchValueData("InsuredPhone",Convert.ToString(referralPayorMapping.InsuredPhone)),
                    new SearchValueData("InsuredPolicyGroupOrFecaNumber",Convert.ToString(referralPayorMapping.InsuredPolicyGroupOrFecaNumber)),
                    new SearchValueData("InsuredDateOfBirth",referralPayorMapping.InsuredDateOfBirth.HasValue ? referralPayorMapping.InsuredDateOfBirth.Value.ToString(Constants.DbDateFormat) : string.Empty),
                    new SearchValueData("InsuredGender",Convert.ToString(referralPayorMapping.InsuredGender)),
                    new SearchValueData("InsuredEmployersNameOrSchoolName",Convert.ToString(referralPayorMapping.InsuredEmployersNameOrSchoolName)),
                    new SearchValueData("BeneficiaryTypeID",Convert.ToString(referralPayorMapping.BeneficiaryTypeID)),
                    new SearchValueData("BeneficiaryNumber",Convert.ToString(referralPayorMapping.BeneficiaryNumber)),
                    new SearchValueData("MemberID",Convert.ToString(referralPayorMapping.MemberID)),
                    new SearchValueData("MasterJurisdictionID",Convert.ToString(referralPayorMapping.MasterJurisdictionID)),
                    new SearchValueData("MasterTimezoneID",Convert.ToString(referralPayorMapping.MasterTimezoneID)),
                };
                TransactionResult transactionResult = GetEntity<TransactionResult>(StoredProcedure.HC_SaveReferralPayorMapping, searchModel);

                if (transactionResult.TransactionResultId == -1)
                {
                    response.Message = Resource.RPMRecordAlreadyExists;
                    return response;
                }
                response.IsSuccess = true;
                response.Message = isEditMode ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.ReferralPayorMapping) :
                    string.Format(Resource.RecordCreatedSuccessfully, Resource.ReferralPayorMapping);
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse HC_GetReferralPayorMappingList(string encryptedReferralId, SearchReferralPayorMapping searchReferralPayorMapping, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                searchReferralPayorMapping.ReferralID = Convert.ToInt64(Crypto.Decrypt(encryptedReferralId));

                HC_SetSearchFilterForReferralPayorMappingListPage(searchReferralPayorMapping, searchList);

                Page<ListReferralPayorMapping> listReferralPayorMapping = GetEntityPageList<ListReferralPayorMapping>(StoredProcedure.HC_GetReferralPayorMappingList,
                                                                    searchList, pageSize, pageIndex, sortIndex, sortDirection);
                response.IsSuccess = true;
                response.Data = listReferralPayorMapping;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse HC_DeleteReferralPayorMapping(string encryptedReferralId, SearchReferralPayorMapping searchReferralPayorMapping, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            try
            {
                searchReferralPayorMapping.ReferralID = Convert.ToInt64(Crypto.Decrypt(encryptedReferralId));

                List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);
                HC_SetSearchFilterForReferralPayorMappingListPage(searchReferralPayorMapping, searchList);

                if (!string.IsNullOrEmpty(searchReferralPayorMapping.ListOfIdsInCSV))
                    searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchReferralPayorMapping.ListOfIdsInCSV });

                searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
                DeleteListReferralPayorMapping data = GetMultipleEntity<DeleteListReferralPayorMapping>(StoredProcedure.HC_DeleteReferralPayorMapping, searchList);
                //List<ListReferralPayorMapping> totalData = GetEntityList<ListReferralPayorMapping>(StoredProcedure.HC_DeleteReferralPayorMapping, searchList);
                List<ListReferralPayorMapping> totalData = data.listReferralPayorMappings;
                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<ListReferralPayorMapping> listModel = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
                data.listModel = listModel;
                response.Data = data;
                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.ReferralPayorMapping);
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.DeleteFailed, Resource.ExceptionMessage);
            }
            return response;
        }

        private static void HC_SetSearchFilterForReferralPayorMappingListPage(SearchReferralPayorMapping searchReferralPayorMapping, List<SearchValueData> searchList)
        {
            searchList.Add(new SearchValueData { Name = "PayorName", Value = Convert.ToString(searchReferralPayorMapping.PayorName) });
            searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(searchReferralPayorMapping.ReferralID), IsEqual = true });
            searchList.Add(new SearchValueData { Name = "IsDeleted ", Value = Convert.ToString(searchReferralPayorMapping.IsDeleted) });
            searchList.Add(new SearchValueData { Name = "Precedence", Value = Convert.ToString(searchReferralPayorMapping.Precedence) });
            searchList.Add(new SearchValueData { Name = "BeneficiaryTypeID", Value = Convert.ToString(searchReferralPayorMapping.BeneficiaryTypeID) });

            if (searchReferralPayorMapping.PayorEffectiveDate != null)
                searchList.Add(new SearchValueData { Name = "PayorEffectiveDate", Value = Convert.ToDateTime(searchReferralPayorMapping.PayorEffectiveDate).ToString(Constants.DbDateFormat) });
            if (searchReferralPayorMapping.PayorEffectiveEndDate != null)
                searchList.Add(new SearchValueData { Name = "PayorEffectiveEndDate", Value = Convert.ToDateTime(searchReferralPayorMapping.PayorEffectiveEndDate).ToString(Constants.DbDateFormat) });
        }

        #endregion

        #region Referral Billing Setting
        public ServiceResponse HC_AddReferralBillingSetting(ReferralBillingSetting referralBillingSetting, long loggedInUserId)
        {
            var response = new ServiceResponse();
            try
            {
                referralBillingSetting.ReferralID = Convert.ToInt64(Crypto.Decrypt(referralBillingSetting.EncryptedReferralId));

                List<SearchValueData> searchModel = new List<SearchValueData>();
                searchModel.Add(new SearchValueData { Name = "ReferralBillingSettingID", Value = referralBillingSetting.ReferralBillingSettingID.ToString() });
                searchModel.Add(new SearchValueData { Name = "ReferralID", Value = referralBillingSetting.ReferralID.ToString() });

                if (referralBillingSetting.AuthrizationCode_CMS1500 != null)
                    searchModel.Add(new SearchValueData { Name = "AuthrizationCode_CMS1500", Value = referralBillingSetting.AuthrizationCode_CMS1500.ToString() });

                searchModel.Add(new SearchValueData { Name = "POS_CMS1500", Value = referralBillingSetting.POS_CMS1500.ToString() });

                if (referralBillingSetting.AuthrizationCode_UB04 != null)
                    searchModel.Add(new SearchValueData { Name = "AuthrizationCode_UB04", Value = referralBillingSetting.AuthrizationCode_UB04.ToString() });

                searchModel.Add(new SearchValueData { Name = "POS_UB04", Value = referralBillingSetting.POS_UB04.ToString() });

                searchModel.Add(new SearchValueData { Name = "AdmissionTypeCode_UB04", Value = referralBillingSetting.AdmissionTypeCode_UB04.ToString() });
                searchModel.Add(new SearchValueData { Name = "AdmissionSourceCode_UB04", Value = referralBillingSetting.AdmissionSourceCode_UB04.ToString() });
                searchModel.Add(new SearchValueData { Name = "PatientStatusCode_UB04", Value = referralBillingSetting.PatientStatusCode_UB04.ToString() });

                searchModel.Add(new SearchValueData { Name = "AuthrizationCodeType", Value = referralBillingSetting.AuthrizationCodeType.ToString() });

                searchModel.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserId) });
                searchModel.Add(new SearchValueData { Name = "SystemID", Value = HttpContext.Current.Request.UserHostAddress.ToString() });


                ReferralBillingSetting referralBillingSettingModel = GetEntity<ReferralBillingSetting>(StoredProcedure.HC_AddReferralBillingSetting, searchModel);
                if (referralBillingSettingModel != null)
                {
                    response.IsSuccess = true;
                    response.Message = "Referral setting saved successfully";
                    response.Data = referralBillingSettingModel;
                }
                else
                {
                    response.IsSuccess = false;
                }
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }
        public ServiceResponse HC_GetReferralBillingSetting(ReferralBillingSetting referralBillingSetting)
        {
            var response = new ServiceResponse();
            try
            {
                if (referralBillingSetting.ReferralID == 0)
                    referralBillingSetting.ReferralID = Convert.ToInt64(Crypto.Decrypt(referralBillingSetting.EncryptedReferralId));

                List<SearchValueData> searchModel = new List<SearchValueData>();
                searchModel.Add(new SearchValueData { Name = "ReferralID", Value = referralBillingSetting.ReferralID.ToString() });

                ReferralBillingSetting referralBillingSettingModel = GetEntity<ReferralBillingSetting>(StoredProcedure.HC_GetReferralBillingSetting, searchModel);
                if (referralBillingSettingModel != null)
                {
                    response.IsSuccess = true;
                    response.Data = referralBillingSettingModel;
                }
                else
                {
                    response.IsSuccess = false;
                }
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse HC_AddReferralBillingAuthrization(ReferralBillingAuthorization referralBillingAuthorization, long loggedInUserId)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchModel = new List<SearchValueData>();
                searchModel.Add(new SearchValueData { Name = "ReferralBillingAuthorizationID", Value = Convert.ToString(referralBillingAuthorization.ReferralBillingAuthorizationID) });
                searchModel.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(referralBillingAuthorization.ReferralID) });
                if (referralBillingAuthorization.AuthType == 1)
                {
                    searchModel.Add(new SearchValueData { Name = "Type", Value = AuthType.CMS1500.ToString() });
                }
                else
                {
                    searchModel.Add(new SearchValueData { Name = "Type", Value = AuthType.UB04.ToString() });
                }

                searchModel.Add(new SearchValueData { Name = "PayorID", Value = Convert.ToString(referralBillingAuthorization.PayorID) });
                searchModel.Add(new SearchValueData { Name = "StrServiceCodeIDs", Value = Convert.ToString(referralBillingAuthorization.StrServiceCodeIDs) });

                #region New Change In Billing - Kundan>Kumar>Rai
                searchModel.Add(new SearchValueData { Name = "ServiceCodeID", Value = Convert.ToString(referralBillingAuthorization.ServiceCodeID) });
                searchModel.Add(new SearchValueData { Name = "Rate", Value = Convert.ToString(referralBillingAuthorization.Rate) });
                searchModel.Add(new SearchValueData { Name = "RevenueCode", Value = Convert.ToString(referralBillingAuthorization.RevenueCode) });
                searchModel.Add(new SearchValueData { Name = "UnitType", Value = Convert.ToString(referralBillingAuthorization.UnitType) });

                if (referralBillingAuthorization.UnitType == Convert.ToInt32(EnumUnitType.Time))
                {
                    searchModel.Add(new SearchValueData { Name = "PerUnitQuantity", Value = Convert.ToString(referralBillingAuthorization.PerUnitQuantity) });
                    searchModel.Add(new SearchValueData { Name = "RoundUpUnit", Value = Convert.ToString(referralBillingAuthorization.RoundUpUnit) });
                    searchModel.Add(new SearchValueData { Name = "MaxUnit", Value = Convert.ToString(referralBillingAuthorization.MaxUnit) });
                    searchModel.Add(new SearchValueData { Name = "DailyUnitLimit", Value = Convert.ToString(referralBillingAuthorization.DailyUnitLimit) });
                }
                else
                {
                    searchModel.Add(new SearchValueData { Name = "PerUnitQuantity", Value = "0" });
                    searchModel.Add(new SearchValueData { Name = "RoundUpUnit", Value = "0" });
                    searchModel.Add(new SearchValueData { Name = "MaxUnit", Value = "0" });
                    searchModel.Add(new SearchValueData { Name = "DailyUnitLimit", Value = "0" });
                }
                searchModel.Add(new SearchValueData { Name = "CareType", Value = Convert.ToString(referralBillingAuthorization.CareType) });
                searchModel.Add(new SearchValueData { Name = "ModifierID", Value = Convert.ToString(referralBillingAuthorization.ModifierID) });
                searchModel.Add(new SearchValueData { Name = "TaxonomyID", Value = Convert.ToString(referralBillingAuthorization.TaxonomyID) });
                #endregion

                searchModel.Add(new SearchValueData { Name = "AttachmentFileName", Value = referralBillingAuthorization.AttachmentFileName });
                searchModel.Add(new SearchValueData { Name = "AttachmentFilePath", Value = referralBillingAuthorization.AttachmentFilePath });
                searchModel.Add(new SearchValueData { Name = "UnitLimitFrequency", Value = Convert.ToString(referralBillingAuthorization.UnitLimitFrequency) });

                searchModel.Add(new SearchValueData { Name = "AllowedTime", Value = Convert.ToString(referralBillingAuthorization.AllowedTime) });
                searchModel.Add(new SearchValueData { Name = "AllowedTimeType", Value = Convert.ToString(referralBillingAuthorization.AllowedTimeType) });

                searchModel.Add(new SearchValueData { Name = "AuthorizationCode", Value = referralBillingAuthorization.AuthorizationCode });
                searchModel.Add(new SearchValueData { Name = "StartDate", Value = referralBillingAuthorization.StartDate.Value.ToString(Constants.DbDateFormat) });
                searchModel.Add(new SearchValueData { Name = "EndDate", Value = referralBillingAuthorization.EndDate.Value.ToString(Constants.DbDateFormat) });
                searchModel.Add(new SearchValueData { Name = "PriorAuthorizationFrequencyType", Value = referralBillingAuthorization.PriorAuthorizationFrequencyType.ToString() });

                searchModel.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserId) });
                searchModel.Add(new SearchValueData { Name = "SystemID", Value = HttpContext.Current.Request.UserHostAddress.ToString() });
                searchModel.Add(new SearchValueData { Name = "DxCode", Value = referralBillingAuthorization.DxCode });
                searchModel.Add(new SearchValueData { Name = "DxCodeID", Value = referralBillingAuthorization.DxCodeID });
                searchModel.Add(new SearchValueData { Name = "PayRate", Value = Convert.ToString(referralBillingAuthorization.PayRate) });
                searchModel.Add(new SearchValueData { Name = "FacilityCode", Value = referralBillingAuthorization.FacilityCode });


                TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.HC_AddReferralBillingAuthrization, searchModel);
                if (result.TransactionResultId > 0)
                {
                    response.IsSuccess = true;
                    response.Data = result.TransactionResultId;
                    response.Message = Resource.PriorAuthorizationSaveSuccessfully;
                }
                else if (result.TransactionResultId == -2)
                {
                    response.IsSuccess = false;
                    response.Message = Resource.AuthCodeDateRangeExist;
                }
                else if (result.TransactionResultId == -4)
                {
                    response.IsSuccess = false;
                    response.Message = Resource.MoreTimeThanPossible;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = result.ErrorMessage;
                }

                try
                {
                    if (SessionHelper.IsCaseManagement)
                    {
                        searchModel = new List<SearchValueData>();
                        searchModel.Add(new SearchValueData { Name = "ReferralBillingAuthorizationID", Value = Convert.ToString(referralBillingAuthorization.ReferralBillingAuthorizationID) });
                        searchModel.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(referralBillingAuthorization.ReferralID) });
                        searchModel.Add(new SearchValueData { Name = "PayorID", Value = Convert.ToString(referralBillingAuthorization.PayorID) });
                        searchModel.Add(new SearchValueData { Name = "PriorAuthorizationFrequencyType", Value = referralBillingAuthorization.PriorAuthorizationFrequencyType.ToString() });
                        searchModel.Add(new SearchValueData { Name = "StartDate", Value = referralBillingAuthorization.StartDate.Value.ToString(Constants.DbDateFormat) });
                        searchModel.Add(new SearchValueData { Name = "EndDate", Value = referralBillingAuthorization.EndDate.Value.ToString(Constants.DbDateFormat) });

                        searchModel.Add(new SearchValueData { Name = "IsEndDateAvailable", Value = "1" });
                        searchModel.Add(new SearchValueData { Name = "TodayDate", Value = Common.GetOrgCurrentDateTime().ToString(Constants.DbDateFormat) });
                        searchModel.Add(new SearchValueData { Name = "SlotEndDate", Value = Common.GetOrgCurrentDateTime().AddDays(ConfigSettings.GenerateEmpRefTimeScheduleDays).ToString(Constants.DbDateFormat) });

                        searchModel.Add(new SearchValueData { Name = "loggedInUserId", Value = Convert.ToString(loggedInUserId) });
                        searchModel.Add(new SearchValueData { Name = "SystemID", Value = HttpContext.Current.Request.UserHostAddress.ToString() });

                        TransactionResult timeslotResult = GetEntity<TransactionResult>(StoredProcedure.AddRtsMaterCaseManagement, searchModel);
                        if (timeslotResult.TransactionResultId > 0 && timeslotResult.TablePrimaryId > 0)
                        {
                            ReferralTimeSlotMaster model = new ReferralTimeSlotMaster();
                            model.ReferralID = referralBillingAuthorization.ReferralID;
                            model.StartDate = referralBillingAuthorization.StartDate.Value;
                            model.EndDate = referralBillingAuthorization.EndDate;

                            //AddRTSDetailCaseManagement(timeslotResult.TablePrimaryId, model, loggedInUserId);
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse GetAuthorizationLinkupList(long referralBillingAuthorizationID)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchModel = new List<SearchValueData>();
                searchModel.Add(new SearchValueData { Name = "ReferralBillingAuthorizationID", Value = Convert.ToString(referralBillingAuthorizationID) });

                List<AuthorizationLinkup> data = GetEntityList<AuthorizationLinkup>(StoredProcedure.GetAuthorizationLinkupList, searchModel);

                response.IsSuccess = true;
                response.Data = data;

            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse GetAuthorizationScheduleLinkList(SearchAuthorizationScheduleLinkList searchAuthorizationScheduleLinkList)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchModel = new List<SearchValueData>();
                searchModel.Add(new SearchValueData { Name = "ReferralBillingAuthorizationID", Value = Convert.ToString(searchAuthorizationScheduleLinkList.ReferralBillingAuthorizationID) });
                if (searchAuthorizationScheduleLinkList.StartDate.HasValue)
                { searchModel.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToString(searchAuthorizationScheduleLinkList.StartDate.Value) }); }
                if (searchAuthorizationScheduleLinkList.EndDate.HasValue)
                { searchModel.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToString(searchAuthorizationScheduleLinkList.EndDate.Value) }); }

                List<AuthorizationLinkup> data = GetEntityList<AuthorizationLinkup>(StoredProcedure.GetAuthorizationScheduleLinkList, searchModel);

                response.IsSuccess = true;
                response.Data = data;

            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse UpdateAuthorizationLinkup(UpdateAuthorizationLinkupModel model)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchModel = new List<SearchValueData>();
                searchModel.Add(new SearchValueData { Name = "ReferralBillingAuthorizationID", Value = Convert.ToString(model.ReferralBillingAuthorizationID) });
                searchModel.Add(new SearchValueData { Name = "ScheduleIDs", Value = model.ScheduleIDs });

                object data = GetScalar(StoredProcedure.UpdateAuthorizationLinkup, searchModel);

                response.IsSuccess = true;
                response.Data = data;
                response.Message = Resource.ScheduleDetailUpdated;

            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse HC_SavePriorAuthorization(ReferralBillingAuthorization referralBillingAuthorization, long loggedInUserId)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchModel = new List<SearchValueData>();
                searchModel.Add(new SearchValueData { Name = "ReferralBillingAuthorizationID", Value = Convert.ToString(referralBillingAuthorization.ReferralBillingAuthorizationID) });
                searchModel.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(referralBillingAuthorization.ReferralID) });
                if (referralBillingAuthorization.AuthType == 1)
                {
                    searchModel.Add(new SearchValueData { Name = "Type", Value = AuthType.CMS1500.ToString() });
                }
                else
                {
                    searchModel.Add(new SearchValueData { Name = "Type", Value = AuthType.UB04.ToString() });
                }

                searchModel.Add(new SearchValueData { Name = "PayorID", Value = Convert.ToString(referralBillingAuthorization.PayorID) });
                //searchModel.Add(new SearchValueData { Name = "StrServiceCodeIDs", Value = Convert.ToString(referralBillingAuthorization.StrServiceCodeIDs) });
                //searchModel.Add(new SearchValueData { Name = "AllowedTime", Value = Convert.ToString(referralBillingAuthorization.AllowedTime) });

                searchModel.Add(new SearchValueData { Name = "AuthorizationCode", Value = referralBillingAuthorization.AuthorizationCode });
                searchModel.Add(new SearchValueData { Name = "StartDate", Value = referralBillingAuthorization.StartDate.Value.ToString(Constants.DbDateFormat) });
                searchModel.Add(new SearchValueData { Name = "EndDate", Value = referralBillingAuthorization.EndDate.Value.ToString(Constants.DbDateFormat) });

                searchModel.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserId) });
                searchModel.Add(new SearchValueData { Name = "SystemID", Value = HttpContext.Current.Request.UserHostAddress.ToString() });


                TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.HC_SavePriorAuthorization, searchModel);
                if (result.TransactionResultId > 0)
                {
                    response.IsSuccess = true;
                    response.Message = Resource.PriorAuthorizationSaveSuccessfully;
                }
                else if (result.TransactionResultId == -2)
                {
                    response.IsSuccess = false;
                    response.Message = Resource.AuthCodeDateRangeExist;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = result.ErrorMessage;
                }

                try
                {
                    if (SessionHelper.IsCaseManagement)
                    {
                        searchModel = new List<SearchValueData>();
                        searchModel.Add(new SearchValueData { Name = "ReferralBillingAuthorizationID", Value = Convert.ToString(referralBillingAuthorization.ReferralBillingAuthorizationID) });
                        searchModel.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(referralBillingAuthorization.ReferralID) });
                        searchModel.Add(new SearchValueData { Name = "PayorID", Value = Convert.ToString(referralBillingAuthorization.PayorID) });
                        searchModel.Add(new SearchValueData { Name = "StartDate", Value = referralBillingAuthorization.StartDate.Value.ToString(Constants.DbDateFormat) });
                        searchModel.Add(new SearchValueData { Name = "EndDate", Value = referralBillingAuthorization.EndDate.Value.ToString(Constants.DbDateFormat) });

                        searchModel.Add(new SearchValueData { Name = "IsEndDateAvailable", Value = "1" });
                        searchModel.Add(new SearchValueData { Name = "TodayDate", Value = Common.GetOrgCurrentDateTime().ToString(Constants.DbDateFormat) });
                        searchModel.Add(new SearchValueData { Name = "SlotEndDate", Value = Common.GetOrgCurrentDateTime().AddDays(ConfigSettings.GenerateEmpRefTimeScheduleDays).ToString(Constants.DbDateFormat) });

                        searchModel.Add(new SearchValueData { Name = "loggedInUserId", Value = Convert.ToString(loggedInUserId) });
                        searchModel.Add(new SearchValueData { Name = "SystemID", Value = HttpContext.Current.Request.UserHostAddress.ToString() });

                        TransactionResult timeslotResult = GetEntity<TransactionResult>(StoredProcedure.AddRtsMaterCaseManagement, searchModel);
                        if (timeslotResult.TransactionResultId > 0 && timeslotResult.TablePrimaryId > 0)
                        {
                            ReferralTimeSlotMaster model = new ReferralTimeSlotMaster();
                            model.ReferralID = referralBillingAuthorization.ReferralID;
                            model.StartDate = referralBillingAuthorization.StartDate.Value;
                            model.EndDate = referralBillingAuthorization.EndDate;

                            AddRTSDetailCaseManagement(timeslotResult.TablePrimaryId, model, loggedInUserId);
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }


        public ServiceResponse HC_SavePriorAuthorizationServiceCodeDetails(ReferralBillingAuthorizationServiceCodeModel model, long loggedInUserId)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchModel = new List<SearchValueData>();


                searchModel.Add(new SearchValueData { Name = "ReferralBillingAuthorizationServiceCodeID", Value = Convert.ToString(model.ReferralBillingAuthorizationServiceCodeID) });
                searchModel.Add(new SearchValueData { Name = "ReferralBillingAuthorizationID", Value = Convert.ToString(model.ReferralBillingAuthorizationID) });
                searchModel.Add(new SearchValueData { Name = "ServiceCodeID", Value = Convert.ToString(model.ServiceCodeID) });
                searchModel.Add(new SearchValueData { Name = "DailyUnitLimit", Value = Convert.ToString(model.DailyUnitLimit) });
                searchModel.Add(new SearchValueData { Name = "MaxUnitLimit", Value = Convert.ToString(model.MaxUnitLimit) });
                searchModel.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserId) });
                searchModel.Add(new SearchValueData { Name = "SystemID", Value = HttpContext.Current.Request.UserHostAddress.ToString() });
                TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.HC_SavePriorAuthorizationServiceCodeDetails, searchModel);
                if (result.TransactionResultId > 0)
                {
                    response.IsSuccess = true;
                    response.Message = Resource.PriorAuthorizationServiceCodeSaveSuccessfully;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = result.ErrorMessage;
                }


            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse HC_DeletePAServiceCode(string referralBillingAuthorizationServiceCodeID, long loggedIdID)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "ReferralBillingAuthorizationServiceCodeID", Value = referralBillingAuthorizationServiceCodeID });
                searchList.Add(new SearchValueData { Name = "LoggedIdID", Value = Convert.ToString(loggedIdID) });

                List<ReferralBillingAuthorizationList> totalData = GetEntityList<ReferralBillingAuthorizationList>(StoredProcedure.HC_DeletePAServiceCode, searchList);

                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.Details);
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.DeleteFailed, Resource.ExceptionMessage);
            }
            return response;
        }

        public List<ServiceCodes> HC_GetPayorServicecodeList(string searchText, string ReferralBillingAuthorizationID, int pageSize = 10)
        {
            List<ServiceCodes> contactlist = GetEntityList<ServiceCodes>(StoredProcedure.HC_GetPayorServicecodeList,
                new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralBillingAuthorizationID", Value = ReferralBillingAuthorizationID},
                    new SearchValueData {Name = "SearchText", Value = searchText},
                    new SearchValueData {Name = "PageSize", Value = pageSize.ToString()}
                });
            return contactlist;
        }
        //ServiceResponse 


        public ServiceResponse HC_GetAuthorizationLoadModel(string encryptedReferralId)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "ReferralID", Value = Crypto.Decrypt(encryptedReferralId) });
                GetAuthorizationLoadModel data = GetMultipleEntity<GetAuthorizationLoadModel>(StoredProcedure.HC_GetAuthorizationLoadModel, searchList);
                response.IsSuccess = true;
                response.Data = data;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }


        public List<ServiceCodes> HC_GetPayorMappedServiceCodeList(string searchText, long PayorID, int pageSize = 10)
        {
            List<ServiceCodes> contactlist = GetEntityList<ServiceCodes>(StoredProcedure.HC_GetPayorMappedServiceCode,
                new List<SearchValueData>
                {
                    new SearchValueData {Name = "SearchText", Value = searchText},
                    new SearchValueData {Name = "PageSize", Value = pageSize.ToString()},
                    new SearchValueData {Name = "PayorID", Value = Convert.ToString(PayorID)}
                });
            return contactlist;
        }

        public ServiceResponse HC_GetPayorMappedServiceCodes(long PayorID)
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "PayorID", Value = Convert.ToString(PayorID) });
            searchList.Add(new SearchValueData { Name = "SearchText", Value = "" });
            searchList.Add(new SearchValueData { Name = "PageSize", Value = Convert.ToString(10000) });
            List<ServiceCodes> data = GetEntityList<ServiceCodes>(StoredProcedure.HC_GetPayorMappedServiceCode, searchList);
            response.IsSuccess = true;
            response.Data = data;
            return response;


        }


        public ServiceResponse HC_GetAuthServiceCodes(SearchAuthServiceCodesModel model)
        {

            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ReferralBillingAuthorizationID", Value = Convert.ToString(model.ReferralBillingAuthorizationID) });
            List<AuthServiceCodesModel> data = GetEntityList<AuthServiceCodesModel>(StoredProcedure.HC_GetAuthServiceCodes, searchList);
            response.IsSuccess = true;
            response.Data = data;
            return response;


        }


        public ServiceResponse HC_GetReferralBillingAuthorizationList(string encryptedReferralId, SearchReferralBillingAuthorization searchReferralBillingAuthorization, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                searchReferralBillingAuthorization.ReferralID = Convert.ToInt64(Crypto.Decrypt(encryptedReferralId));

                HC_SetSearchFilterForReferralBillingAuthorizationListPage(searchReferralBillingAuthorization, searchList);

                Page<ReferralBillingAuthorizationList> list = GetEntityPageList<ReferralBillingAuthorizationList>(StoredProcedure.HC_GetReferralBillingAuthorizationList,
                                                                    searchList, pageSize, pageIndex, sortIndex, sortDirection);
                response.IsSuccess = true;
                response.Data = list;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse GetAuthorizationCodesByReferralId(long referralID)
        {
            ServiceResponse response = new ServiceResponse();
            var searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ReferralID", Value = referralID.ToString() });
            List<ReferralBillingAuthorizationServiceCodeModel> list = GetEntityList<ReferralBillingAuthorizationServiceCodeModel>(StoredProcedure.GetAuthorizationDetailsByReferralId, searchList);
            response.Data = list;
            response.IsSuccess = true;
            return response;
        }


        public ServiceResponse HC_GetPriorAuthorizationList(string encryptedReferralId, SearchReferralBillingAuthorization searchReferralBillingAuthorization, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                searchReferralBillingAuthorization.ReferralID = Convert.ToInt64(Crypto.Decrypt(encryptedReferralId));

                HC_SetSearchFilterForPriorAuthorizationListPage(searchReferralBillingAuthorization, searchList);

                Page<ReferralBillingAuthorizationList> list = GetEntityPageList<ReferralBillingAuthorizationList>(StoredProcedure.HC_GetPriorAuthorizationList,
                    searchList, pageSize, pageIndex, sortIndex, sortDirection);
                response.IsSuccess = true;
                response.Data = list;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }
        private static void HC_SetSearchFilterForPriorAuthorizationListPage(SearchReferralBillingAuthorization searchReferralBillingAuthorization, List<SearchValueData> searchList)
        {
            searchList.Add(new SearchValueData { Name = "PayorID", Value = Convert.ToString(searchReferralBillingAuthorization.PayorID), IsEqual = true });
            searchList.Add(new SearchValueData { Name = "AuthorizationCode", Value = Convert.ToString(searchReferralBillingAuthorization.AuthorizationCode) });
            searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(searchReferralBillingAuthorization.ReferralID), IsEqual = true });
            searchList.Add(new SearchValueData { Name = "IsDeleted ", Value = Convert.ToString(searchReferralBillingAuthorization.IsDeleted) });

            if (searchReferralBillingAuthorization.Type == 0)
                searchReferralBillingAuthorization.Type = 1;

            searchList.Add(new SearchValueData { Name = "AuthType ", Value = searchReferralBillingAuthorization.Type == 1 ? AuthType.CMS1500.ToString() : AuthType.UB04.ToString() });

            if (searchReferralBillingAuthorization.StartDate != null)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(searchReferralBillingAuthorization.StartDate).ToString(Constants.DbDateFormat) });
            if (searchReferralBillingAuthorization.EndDate != null)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(searchReferralBillingAuthorization.EndDate).ToString(Constants.DbDateFormat) });
        }

        private static void HC_SetSearchFilterForReferralBillingAuthorizationListPage(SearchReferralBillingAuthorization searchReferralBillingAuthorization, List<SearchValueData> searchList)
        {
            searchList.Add(new SearchValueData { Name = "PayorID", Value = Convert.ToString(searchReferralBillingAuthorization.PayorID), IsEqual = true });
            searchList.Add(new SearchValueData { Name = "AuthorizationCode", Value = Convert.ToString(searchReferralBillingAuthorization.AuthorizationCode) });
            searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(searchReferralBillingAuthorization.ReferralID), IsEqual = true });
            searchList.Add(new SearchValueData { Name = "IsDeleted ", Value = Convert.ToString(searchReferralBillingAuthorization.IsDeleted) });

            searchList.Add(new SearchValueData { Name = "AuthType ", Value = searchReferralBillingAuthorization.Type == 1 ? AuthType.CMS1500.ToString() : AuthType.UB04.ToString() });

            if (searchReferralBillingAuthorization.StartDate != null)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(searchReferralBillingAuthorization.StartDate).ToString(Constants.DbDateFormat) });
            if (searchReferralBillingAuthorization.EndDate != null)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(searchReferralBillingAuthorization.EndDate).ToString(Constants.DbDateFormat) });
        }


        public ServiceResponse HC_GetPAServiceCodeList(SearchReferralBillingAuthorizationCode model)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "ReferralBillingAuthorizationID", Value = Convert.ToString(model.ReferralBillingAuthorizationID), IsEqual = true });
                List<ReferralBillingAuthorizationServiceCodeModel> list = GetEntityList<ReferralBillingAuthorizationServiceCodeModel>(StoredProcedure.HC_GetPAServiceCodeList,
                    searchList);
                response.IsSuccess = true;
                response.Data = list;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }


        public ServiceResponse HC_DeleteReferralBillingAuthorization(string encryptedReferralId, SearchReferralBillingAuthorization searchReferralBillingAuthorization, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            try
            {
                searchReferralBillingAuthorization.ReferralID = Convert.ToInt64(Crypto.Decrypt(encryptedReferralId));

                List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);
                HC_SetSearchFilterForReferralBillingAuthorizationListPage(searchReferralBillingAuthorization, searchList);

                if (!string.IsNullOrEmpty(searchReferralBillingAuthorization.ListOfIdsInCSV))
                    searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchReferralBillingAuthorization.ListOfIdsInCSV });

                searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });

                List<ReferralBillingAuthorizationList> totalData = GetEntityList<ReferralBillingAuthorizationList>(StoredProcedure.HC_DeleteReferralBillingAuthorization, searchList);

                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<ReferralBillingAuthorizationList> listModel = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
                response.Data = listModel;
                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.Details);
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.DeleteFailed, Resource.ExceptionMessage);
            }
            return response;
        }


        public ServiceResponse HC_DeletePriorAuthorization(string encryptedReferralId, SearchReferralBillingAuthorization searchReferralBillingAuthorization, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            try
            {
                searchReferralBillingAuthorization.ReferralID = Convert.ToInt64(Crypto.Decrypt(encryptedReferralId));

                List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);
                HC_SetSearchFilterForPriorAuthorizationListPage(searchReferralBillingAuthorization, searchList);

                if (!string.IsNullOrEmpty(searchReferralBillingAuthorization.ListOfIdsInCSV))
                    searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchReferralBillingAuthorization.ListOfIdsInCSV });

                searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });

                List<ReferralBillingAuthorizationList> totalData = GetEntityList<ReferralBillingAuthorizationList>(StoredProcedure.HC_DeletePriorAuthorization, searchList);

                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<ReferralBillingAuthorizationList> listModel = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
                response.Data = listModel;
                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.Details);
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.DeleteFailed, Resource.ExceptionMessage);
            }
            return response;
        }
        #endregion

        #region Upload Referral Profile Image From API
        public ServiceResponse UploadRefProfileImage(HttpRequestBase currentHttpRequest)
        {
            ServiceResponse response = new ServiceResponse();
            MemoryStream ms = new MemoryStream();

            var ReferralID = string.Empty;

            ReferralID = currentHttpRequest.Headers["ReferralID"];

            currentHttpRequest.InputStream.CopyTo(ms);
            byte[] byteData = ms.ToArray();

            if (byteData.Length > 0)
            {
                string basePath = string.Empty;
                basePath = Common.GetFolderPath((int)Common.FileStorePathType.RefProfileImg);
                string fullPath = basePath + ReferralID + "/";


                DirectoryInfo di = new DirectoryInfo(HttpContext.Current.Server.MapCustomPath(fullPath));

                if (di.Exists)
                {
                    foreach (FileInfo item in di.GetFiles())
                    {
                        item.Delete();
                    }
                }

                response = Common.ByteToImage(byteData, fullPath);

                List<SearchValueData> srchParam = new List<SearchValueData>();

                srchParam.Add(new SearchValueData("ReferralID", Convert.ToString(ReferralID)));
                srchParam.Add(new SearchValueData("ProfileImgPath", ((UploadedFileModel)response.Data).TempFilePath));

                GetScalar(StoredProcedure.UpdateReferralProfileImgPath, srchParam);

                return response;
            }
            response.Message = "File Upload Failed NoFile Selected";
            return response;
        }
        #endregion

        #region CareForm Details
        public ServiceResponse HC_GetCareFormDetails(SearchCareFormDetails searchCareFormDetails)
        {
            var response = new ServiceResponse();
            searchCareFormDetails.ReferralID = Convert.ToInt64(Crypto.Decrypt(searchCareFormDetails.EncryptedReferralID));
            try
            {
                List<SearchValueData> searchModel = new List<SearchValueData>
                {
                    new SearchValueData{Name = "CareFormID",Value = Convert.ToString(searchCareFormDetails.CareFormID)},
                    new SearchValueData{Name = "ReferralID",Value = Convert.ToString(searchCareFormDetails.ReferralID)}
                };
                CareForm careForm = GetEntity<CareForm>(StoredProcedure.HC_GetCareFormDetails, searchModel);

                if (careForm != null)
                {
                    response.IsSuccess = true;
                    response.Data = careForm;
                    return response;
                }
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }
        public ServiceResponse HC_SaveCareFormDetails(CareForm careForm, long loggedInUserId)
        {
            var response = new ServiceResponse();
            bool isEditMode = careForm.CareFormID > 0;
            careForm.ReferralID = Convert.ToInt64(Crypto.Decrypt(careForm.EncryptedReferralID));

            #region Check client signature
            if (string.IsNullOrEmpty(careForm.ClientSignature))
            {
                response.Message = "Client Signature Required";
                return response;
            }
            #endregion

            #region move client signature
            string destPath = String.Format(_cacheHelper.CareFormClientSignaturesFullPath, _cacheHelper.Domain) + careForm.ReferralID + "/";

            ServiceResponse SiteLogofileMoveResponse = new ServiceResponse { IsSuccess = true };
            if (!string.IsNullOrEmpty(careForm.TempClientSignaturePath))
            {
                SiteLogofileMoveResponse = Common.MoveFile(careForm.TempClientSignaturePath, destPath);

                if (SiteLogofileMoveResponse.IsSuccess)
                    careForm.ClientSignature = (string)SiteLogofileMoveResponse.Data;
            }

            #endregion

            try
            {


                List<SearchValueData> searchModel = new List<SearchValueData>
                {
                    new SearchValueData{Name = "CareFormID",Value = Convert.ToString(careForm.CareFormID)},
                    new SearchValueData{Name = "ReferralID",Value = Convert.ToString(careForm.ReferralID)},
                    new SearchValueData{Name = "LocationOfService",Value = careForm.LocationOfService},
                    new SearchValueData{Name = "Phone",Value = careForm.Phone},
                    new SearchValueData{Name = "Cell",Value = careForm.Cell},
                    new SearchValueData{Name = "Email",Value = careForm.Email},
                    new SearchValueData{Name = "IsMedicallyFrail",Value = Convert.ToString(careForm.IsMedicallyFrail)},
                    new SearchValueData{Name = "SpecificFunctionalLimitations",Value = careForm.SpecificFunctionalLimitations},
                    new SearchValueData{Name = "IsChargesForServicesRendered",Value = Convert.ToString(careForm.IsChargesForServicesRendered)},
                    new SearchValueData{Name = "OnRequest",Value = Convert.ToString(careForm.OnRequest)},
                    new SearchValueData{Name = "PlanOfSupervision",Value = careForm.PlanOfSupervision},
                    new SearchValueData{Name = "DurationOfServices",Value = careForm.DurationOfServices},
                    new SearchValueData{Name = "StatementsOfGoals",Value = careForm.StatementsOfGoals},
                    new SearchValueData{Name = "ObjectivesOfServices",Value = careForm.ObjectivesOfServices},
                    new SearchValueData{Name = "DischargePlans",Value = careForm.DischargePlans},
                    new SearchValueData{Name = "DescriptionHowTheTasksArePerformed",Value = careForm.DescriptionHowTheTasksArePerformed},
                    new SearchValueData{Name = "PertinentDiagnosis",Value = careForm.PertinentDiagnosis},
                    new SearchValueData{Name = "IsAttachedMedicationForm",Value = Convert.ToString(careForm.IsAttachedMedicationForm)},
                    new SearchValueData{Name = "Medications",Value = careForm.Medications},
                    new SearchValueData{Name = "Treatments",Value = careForm.Treatments},
                    new SearchValueData{Name = "EquipmentNeeds",Value = careForm.EquipmentNeeds},
                    new SearchValueData{Name = "Diet",Value = careForm.Diet},
                    new SearchValueData{Name = "NutritionalNeeds",Value = careForm.NutritionalNeeds},
                    new SearchValueData{Name = "IsPhysiciansOrdersNeeded",Value = Convert.ToString(careForm.IsPhysiciansOrdersNeeded)},
                    new SearchValueData{Name = "PhysicianOrdersDescription",Value = careForm.PhysicianOrdersDescription},
                    new SearchValueData{Name = "ClientSignature",Value = careForm.ClientSignature},
                    new SearchValueData{Name = "NurseSignature",Value = careForm.NurseSignature},
                    new SearchValueData{Name = "LoggedInUserId", Value = Convert.ToString(loggedInUserId) },
                    new SearchValueData{Name = "CurrentDate", Value = Convert.ToString(Common.GetOrgCurrentDateTime()) },
                    new SearchValueData{Name = "SystemID", Value = HttpContext.Current.Request.UserHostAddress }
                };

                if (!string.IsNullOrEmpty(careForm.ServiceRequested))
                    searchModel.Add(new SearchValueData { Name = "ServiceRequested", Value = Convert.ToString(careForm.ServiceRequested) });

                if (careForm.CareFormDate != null)
                    searchModel.Add(new SearchValueData { Name = "CareFormDate", Value = Convert.ToString(careForm.CareFormDate) });

                if (careForm.PSI_StartDate != null)
                    searchModel.Add(new SearchValueData { Name = "PSI_StartDate", Value = Convert.ToString(careForm.PSI_StartDate) });

                if (careForm.PSI_EndDate != null)
                    searchModel.Add(new SearchValueData { Name = "PSI_EndDate", Value = Convert.ToString(careForm.PSI_EndDate) });

                if (careForm.ClientSignatureDate != null)
                    searchModel.Add(new SearchValueData { Name = "ClientSignatureDate", Value = careForm.ClientSignatureDate.Value.ToString(Constants.DbDateTimeFormat) });

                if (careForm.NurseSignatureDate != null)
                    searchModel.Add(new SearchValueData { Name = "NurseSignatureDate", Value = Convert.ToString(careForm.NurseSignatureDate) });

                CareForm careFormModel = GetEntity<CareForm>(StoredProcedure.HC_SaveCareFormDetails, searchModel);

                if (careFormModel != null)
                {
                    careFormModel.EncryptedCareFormID = Crypto.Encrypt(Convert.ToString(careFormModel.CareFormID));
                    response.IsSuccess = true;
                    response.Data = careFormModel.EncryptedCareFormID;
                    response.Message = isEditMode ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.CareForm) :
                        string.Format(Resource.RecordCreatedSuccessfully, Resource.CareForm);
                    return response;
                }
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }
        public ServiceResponse HC_SaveClientSignature(CareForm careForm, long loggedInUserId)
        {
            ServiceResponse response = new ServiceResponse();
            //var CareFormID = careForm.ReferralID;
            //string path = String.Format(_cacheHelper.CareFormClientSignaturesFullPath, _cacheHelper.Domain) + CareFormID + "/";

            careForm.ClientSignature = careForm.ClientSignature.Remove(0, 22);

            Guid guid = Guid.NewGuid();
            string filename = guid + Constants.ImageFormatPNG;

            string TempBasePath = String.Format(_cacheHelper.UploadPath, _cacheHelper.Domain) + ConfigSettings.TempFiles;
            TempBasePath += SessionHelper.LoggedInID + "/";
            string actualFilePath = HttpContext.Current.Server.MapCustomPath(TempBasePath);

            if (!Directory.Exists(actualFilePath))
            {
                Directory.CreateDirectory(actualFilePath);
            }
            string fullPath = actualFilePath + filename;
            string TempPath = TempBasePath + filename;
            File.WriteAllBytes(fullPath, Convert.FromBase64String(careForm.ClientSignature));

            response.IsSuccess = true;
            response.Message = "Client Signature saved successfully";
            response.Data = TempPath;
            return response;
        }
        public ServiceResponse HC_SaveCareFormPdfFile(byte[] pdf, string filename, long careformID, long loggedInUserId)
        {
            var response = new ServiceResponse();

            string path = String.Format(_cacheHelper.CareFormFullPath, _cacheHelper.Domain) + careformID + "/";

            string actualFilePath = HttpContext.Current.Server.MapCustomPath(path);

            if (!Directory.Exists(actualFilePath))
            {
                Directory.CreateDirectory(actualFilePath);
            }
            string fullPath = actualFilePath + filename;

            File.WriteAllBytes(fullPath, pdf);
            string filePath = path + filename;

            try
            {
                List<SearchValueData> searchModel = new List<SearchValueData>
                {
                    new SearchValueData{Name = "FileName",Value = Convert.ToString(filename)},
                    new SearchValueData{Name = "FilePath",Value = Convert.ToString(filePath)},
                    new SearchValueData{Name = "KindOfDocument",Value = Convert.ToString(DocumentType.DocumentKind.Internal)},
                    new SearchValueData{Name = "UserType",Value = Convert.ToString((int)ReferralDocument.UserTypes.Referral)},
                    new SearchValueData{Name = "CareFormID",Value = Convert.ToString(careformID)},
                    new SearchValueData{Name = "CurrentDate",Value = Convert.ToString(Common.GetOrgCurrentDateTime())},
                    new SearchValueData{Name = "loggedInUserId",Value = Convert.ToString(loggedInUserId)},
                    new SearchValueData{Name = "SystemID ", Value = HttpContext.Current.Request.UserHostAddress}
                };
                TransactionResult transactionResult = GetEntity<TransactionResult>(StoredProcedure.HC_SaveCareFormDocument, searchModel);
                if (transactionResult.TransactionResultId > 0)
                {
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = e.Message;
                return response;
            }
            return response;
        }
        #endregion


        #region Patient Related eBriggs Form

        public ServiceResponse HC_GetFormListPage()
        {
            ServiceResponse respose = new ServiceResponse();

            List<SearchValueData> searchModel = new List<SearchValueData>();
            HC_EBFormModel data = GetMultipleEntity<HC_EBFormModel>(StoredProcedure.HC_GetFormListPage, searchModel);

            FormApi formApi = new FormApi();

            ServiceResponse res = formApi.Authenticate();
            if (!res.IsSuccess) return res;
            FormApi_LoginResponseModel loginResponseModel = (FormApi_LoginResponseModel)res.Data;

            ServiceResponse marResponse = formApi.GetMarkets(loginResponseModel.tenantGuid);
            if (!marResponse.IsSuccess) return marResponse;
            data.MarketList = (List<FormApi_MarketModel>)marResponse.Data;

            ServiceResponse formResponse = formApi.GetFormList(loginResponseModel.tenantGuid);
            if (!formResponse.IsSuccess) return formResponse;
            data.FormList = (List<FormApi_FormModel>)formResponse.Data;

            respose.Data = data;
            respose.IsSuccess = true;
            return respose;
        }


        public ServiceResponse HC_SaveNewEBForm(SaveNewEBFormModel model, long loggedInId)
        {
            ServiceResponse respose = new ServiceResponse();

            try
            {

                List<SearchValueData> searchModel = new List<SearchValueData>();
                searchModel.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
                searchModel.Add(new SearchValueData { Name = "EmployeeID", Value = Convert.ToString(model.EmployeeID) });
                searchModel.Add(new SearchValueData { Name = "EBriggsFormID", Value = Convert.ToString(model.EBriggsFormID) });
                searchModel.Add(new SearchValueData { Name = "OriginalEBFormID", Value = Convert.ToString(model.OriginalEBFormID) });
                searchModel.Add(new SearchValueData { Name = "FormId", Value = Convert.ToString(model.FormId) });
                searchModel.Add(new SearchValueData { Name = "SubSectionID", Value = Convert.ToString(model.SubSectionID) });

                searchModel.Add(new SearchValueData { Name = "EbriggsFormMppingID", Value = Convert.ToString(model.EbriggsFormMppingID) });
                searchModel.Add(new SearchValueData { Name = "HTMLFormContent", Value = Convert.ToString(model.HTMLFormContent) });

                searchModel.Add(new SearchValueData { Name = "LoggedInID", Value = Convert.ToString(loggedInId) });
                searchModel.Add(new SearchValueData { Name = "SystemID", Value = Common.GetMAcAddress() });



                var PrimaryID = GetScalar(StoredProcedure.HC_SaveNewEBForm, searchModel);
                respose.IsSuccess = true;
                respose.Message = Resource.EbriggsFormSaveSucess;
                respose.Data = PrimaryID;

            }
            catch (Exception e)
            {
                respose.Message = Resource.EbriggsFormSaveIssue;
            }
            return respose;
        }

        #endregion

        #region MIF Form
        public ServiceResponse SetMIFForm(long referralID)
        {
            ServiceResponse response = new ServiceResponse();


            MIFFormModel data = GetMultipleEntity<MIFFormModel>(StoredProcedure.HC_SetMIFForm,
                                                    new List<SearchValueData>
                                                        {
                                                            new SearchValueData{Name = "ReferralID",Value = referralID.ToString()},
                                                            new SearchValueData{Name = "TodayDate",Value = DateTime.Now.ToString(Constants.DbDateFormat)}
                                                        });
            data.MIFDetail = new MIFDetail();
            data.PriorAuthorizationDetail = data.PriorAuthorizationDetail ?? new ReferralBillingAuthorization();
            response.IsSuccess = true;
            response.Data = data;
            return response;
        }

        public ServiceResponse SaveMIFDetail(MIFDetail model)
        {
            ServiceResponse response = new ServiceResponse();

            //#region Check client signature
            //if (string.IsNullOrEmpty(model.SignaturePath))
            //{
            //    response.Message = "Client Signature Required";
            //    return response;
            //}
            //#endregion

            #region move signature

            string destPath = String.Format(_cacheHelper.MIFSignatureFullPath, _cacheHelper.Domain);

            ServiceResponse SiteLogofileMoveResponse = new ServiceResponse { IsSuccess = true };
            if (!string.IsNullOrEmpty(model.TempSignaturePath))
            {
                SiteLogofileMoveResponse = Common.MoveFile(model.TempSignaturePath, destPath);

                if (SiteLogofileMoveResponse.IsSuccess)
                    model.SignaturePath = (string)SiteLogofileMoveResponse.Data;
            }

            #endregion

            var searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ReferralID", Value = model.ReferralID.ToString() });
            searchList.Add(new SearchValueData { Name = "FormName", Value = "MIF_" + string.Format("{0}{1}", model.Type, "_") + DateTime.Now.ToString(Constants.ReadableFileNameDateTimeFormat) });
            searchList.Add(new SearchValueData { Name = "FromWhoToWho", Value = model.FromWhoToWho });
            searchList.Add(new SearchValueData { Name = "Type", Value = model.Type });

            if (!string.IsNullOrEmpty(model.IsResponseRequired.ToString()))
                searchList.Add(new SearchValueData { Name = "IsResponseRequired", Value = model.IsResponseRequired.ToString() });

            searchList.Add(new SearchValueData { Name = "ServiceType_AHD", Value = model.ServiceType_AHD.ToString() });
            searchList.Add(new SearchValueData { Name = "ServiceType_ALS", Value = model.ServiceType_ALS.ToString() });
            searchList.Add(new SearchValueData { Name = "ServiceType_ERS", Value = model.ServiceType_ERS.ToString() });
            searchList.Add(new SearchValueData { Name = "ServiceType_HDM", Value = model.ServiceType_HDM.ToString() });
            searchList.Add(new SearchValueData { Name = "ServiceType_HDS", Value = model.ServiceType_HDS.ToString() });
            searchList.Add(new SearchValueData { Name = "ServiceType_PSS", Value = model.ServiceType_PSS.ToString() });
            searchList.Add(new SearchValueData { Name = "ServiceType_EPS", Value = model.ServiceType_EPS.ToString() });

            if (!string.IsNullOrEmpty(model.IsResponseRequired.ToString()))
                searchList.Add(new SearchValueData { Name = "IsInitialServiceOffered", Value = model.IsInitialServiceOffered.ToString() });
            searchList.Add(new SearchValueData { Name = "InitialServiceNoReason", Value = model.InitialServiceNoReason });
            if (model.InitialServiceDate.HasValue)
                searchList.Add(new SearchValueData { Name = "InitialServiceDate", Value = model.InitialServiceDate.Value.ToString(Constants.DbDateFormat) });
            searchList.Add(new SearchValueData { Name = "InitialServiceFrequencyID", Value = Convert.ToString(model.InitialServiceFrequencyID) });

            searchList.Add(new SearchValueData { Name = "ChangeFYI_RecommendationForChange", Value = model.ChangeFYI_RecommendationForChange.ToString() });
            searchList.Add(new SearchValueData { Name = "ChangeFYI_ChangeInHealthFuncStatus", Value = model.ChangeFYI_ChangeInHealthFuncStatus.ToString() });
            searchList.Add(new SearchValueData { Name = "ChangeFYI_Hospitalization", Value = model.ChangeFYI_Hospitalization.ToString() });
            searchList.Add(new SearchValueData { Name = "ChangeFYI_ServiceNotDelivered", Value = model.ChangeFYI_ServiceNotDelivered.ToString() });
            searchList.Add(new SearchValueData { Name = "ChangeFYI_ChangeInFrequencyByCM", Value = model.ChangeFYI_ChangeInFrequencyByCM.ToString() });
            searchList.Add(new SearchValueData { Name = "ChangeFYI_ChangeInPhysician", Value = model.ChangeFYI_ChangeInPhysician.ToString() });
            searchList.Add(new SearchValueData { Name = "ChangeFYI_Other", Value = model.ChangeFYI_Other.ToString() });
            searchList.Add(new SearchValueData { Name = "ChangeFYI_FYI", Value = model.ChangeFYI_FYI.ToString() });

            searchList.Add(new SearchValueData { Name = "Explanation", Value = model.Explanation });
            if (model.EffectiveDateOfChange.HasValue)
                searchList.Add(new SearchValueData { Name = "EffectiveDateOfChange", Value = model.EffectiveDateOfChange.Value.ToString(Constants.DbDateFormat) });
            searchList.Add(new SearchValueData { Name = "DischargeReason", Value = model.DischargeReason });
            if (model.DateOfDischarge.HasValue)
                searchList.Add(new SearchValueData { Name = "DateOfDischarge", Value = model.DateOfDischarge.Value.ToString(Constants.DbDateFormat) });
            searchList.Add(new SearchValueData { Name = "Comments", Value = model.Comments });

            if (model.PriorAuthorizationDateFrom.HasValue)
                searchList.Add(new SearchValueData { Name = "PriorAuthorizationDateFrom", Value = model.PriorAuthorizationDateFrom.Value.ToString(Constants.DbDateFormat) });
            if (model.PriorAuthorizationDateTo.HasValue)
                searchList.Add(new SearchValueData { Name = "PriorAuthorizationDateTo", Value = model.PriorAuthorizationDateTo.Value.ToString(Constants.DbDateFormat) });
            searchList.Add(new SearchValueData { Name = "PriorAuthorizationNo", Value = model.PriorAuthorizationNo });
            searchList.Add(new SearchValueData { Name = "SignaturePath", Value = model.SignaturePath });

            searchList.Add(new SearchValueData { Name = "LoggedIdID", Value = SessionHelper.LoggedInID.ToString() });
            searchList.Add(new SearchValueData { Name = "SystemID", Value = Common.GetHostAddress() });

            MIFDetail detail = GetEntity<MIFDetail>(StoredProcedure.HC_SaveMIFDetail, searchList);

            if (detail != null)
            {
                response.IsSuccess = true;
                //detail.EncryptedMIFFormID = Crypto.Encrypt(detail.MIFFormID.ToString());
                response.Data = detail;
                response.Message = Resource.FormSavedSuccessfully;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = Resource.SomethingWentWrong;
            }
            return response;
        }

        public ServiceResponse HC_SaveMIFSignature(MIFDetail MIFDetail, long loggedInUserId)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                MIFDetail.SignaturePath = MIFDetail.SignaturePath.Remove(0, 22);

                Guid guid = Guid.NewGuid();
                string filename = guid + Constants.ImageFormatPNG;

                string TempBasePath = String.Format(_cacheHelper.UploadPath, _cacheHelper.Domain) + ConfigSettings.TempFiles;
                TempBasePath += SessionHelper.LoggedInID + "/";
                string actualFilePath = HttpContext.Current.Server.MapCustomPath(TempBasePath);

                if (!Directory.Exists(actualFilePath))
                {
                    Directory.CreateDirectory(actualFilePath);
                }
                string fullPath = actualFilePath + filename;
                string TempPath = TempBasePath + filename;
                File.WriteAllBytes(fullPath, Convert.FromBase64String(MIFDetail.SignaturePath));

                response.IsSuccess = true;
                response.Message = "Signature saved successfully";
                response.Data = TempPath;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse GetMIFDetailForPDF(long MIFFormID)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                MIFPrintModel details = GetEntity<MIFPrintModel>(StoredProcedure.HC_GetMIFDetailForPDF, new List<SearchValueData>
                                                        {
                                                            new SearchValueData{Name = "MIFFormID",Value = MIFFormID.ToString()}
                                                        });

                response.Data = details;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse GetReferralMIFForms(long referralId)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                if (referralId > 0)
                {
                    var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralID", Value = Convert.ToString(referralId)}
                };
                    List<MIFDetail> totalData = GetEntityList<MIFDetail>(StoredProcedure.HC_GetReferralMIFForms, searchlist);
                    response.IsSuccess = true;
                    response.Data = totalData;
                }
                else
                    response.Message = Resource.GetReferralInternalMessageError;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }
        #endregion

        #region New Document related changes
        public ServiceResponse HC_GetReferralSectionList(string userType, long roleId, long referralId, string EmployeeID)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                var UserType = (userType == Common.UserType.Referral.ToString()) ? (int)Common.UserType.Referral : (int)Common.UserType.Employee;
                DocumentPageModel modal = new DocumentPageModel();
                List<Section> sectionList = GetEntityList<Section>(StoredProcedure.HC_GetReferralSectionList, new List<SearchValueData>{
                    new SearchValueData{Name = "UserType",Value = Convert.ToString(UserType)},
                    new SearchValueData{Name = "RoleID",Value = Convert.ToString(roleId)},
                    new SearchValueData{Name = "EmployeeID",Value = Convert.ToString(EmployeeID)},
                    new SearchValueData{Name = "ReferralID",Value = Convert.ToString(referralId)}
                });
                modal.SectionList = sectionList;
                modal.DocumentationTypeList = Common.SetDocumentationTypeList();
                modal.SetYesNoList = Common.SetYesNoListForBoolean();
                modal.ConfigEBFormModel = new ConfigEBFormModel();
                modal.UserRoleList = GetEntityList<Role>();
                response.IsSuccess = true;
                response.Data = modal;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }



        public ServiceResponse HC_GetReferralSubSectionList(long SectionID, string userType, long roleId, long referralId, string EmployeeID)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                var UserType = (userType == Common.UserType.Referral.ToString()) ? (int)Common.UserType.Referral : (int)Common.UserType.Employee;
                List<SubSection> subSectionList = GetEntityList<SubSection>(StoredProcedure.HC_GetReferralSubSectionList, new List<SearchValueData>{
                    new SearchValueData{Name = "SectionID",Value = Convert.ToString(SectionID)},
                    new SearchValueData{Name = "UserType",Value = Convert.ToString(UserType)},
                    new SearchValueData{Name = "RoleID",Value = Convert.ToString(roleId)},
                    new SearchValueData{Name = "EmployeeID",Value = Convert.ToString(EmployeeID)},
                    new SearchValueData{Name = "ReferralID",Value = Convert.ToString(referralId)}
                });

                response.IsSuccess = true;
                response.Data = subSectionList;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse HC_SaveSection(SaveSection modal)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                SectionModal data = GetMultipleEntity<SectionModal>(StoredProcedure.HC_SaveSection, new List<SearchValueData>{
                new SearchValueData{Name = "SectionName",Value = modal.SectionName},
                new SearchValueData{Name = "Color",Value = modal.Color},
                new SearchValueData{Name = "DocumentSection",Value = Convert.ToString((int)Common.DDType.DocumentSection)},
                new SearchValueData{Name = "CurrentDate",Value = Convert.ToString(Common.GetOrgCurrentDateTime())},
                new SearchValueData{Name = "LoggedInID",Value = SessionHelper.LoggedInID.ToString()},
                new SearchValueData{Name = "SystemID",Value = Common.GetHostAddress()}
                });

                if (data.Result == -1)
                {
                    response.Message = Resource.ItemAlreadyExists;
                    response.IsSuccess = false;
                }
                else
                {
                    response.Data = data.SectionList;
                    response.IsSuccess = true;
                    response.Message = Resource.SectionAddedSuccessfully;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse HC_SaveSectionNew(AddDirSubDirModal modal, long roleId)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                var UserType = modal.UserType == Common.UserType.Referral.ToString() ? (int)Common.UserType.Referral : (int)Common.UserType.Employee;
                var saveType = modal.UserType == "Employee" ? "EmployeeId" : "ReferralID";
                SectionModal data = GetMultipleEntity<SectionModal>(StoredProcedure.HC_SaveSectionNew, new List<SearchValueData>{
                    new SearchValueData{Name = "Name",Value = modal.Name},
                    new SearchValueData{Name = "Value",Value = modal.Value},
                    new SearchValueData{Name = "Type",Value = Constants.Directory},
                    new SearchValueData{Name = "DocumentationType",Value = Convert.ToString(modal.DocumentationType)},
                    new SearchValueData{Name = "IsTimeBased",Value = Convert.ToString(modal.IsTimeBased)},
                    new SearchValueData{Name = "UserType",Value = Convert.ToString(UserType)},
                    new SearchValueData{Name = "EBFormID",Value = modal.EBFormID},
                    new SearchValueData{Name = "CurrentDate",Value = Convert.ToString(Common.GetOrgCurrentDateTime().ToString(Constants.DbDateTimeFormat))},
                    new SearchValueData{Name = "LoggedInID",Value = SessionHelper.LoggedInID.ToString()},
                    new SearchValueData{Name = "SystemID",Value = Common.GetHostAddress()},
                    new SearchValueData{Name = "SelectedRoles",Value = string.Join(",", modal.SelectedRoles)},
                    new SearchValueData{Name = "RoleID",Value = Convert.ToString(roleId)},
                    new SearchValueData{Name = "HideIfEmpty",Value = Convert.ToString(modal.HideIfEmpty)},
                    new SearchValueData{Name = "ShowToAll",Value = Convert.ToString(modal.ShowToAll)},
                    new SearchValueData{Name = "EmployeeID",Value = Convert.ToString(modal.EmployeeID)},
                    new SearchValueData{Name = "ReferralID",Value = Convert.ToString(modal.ReferralID)},
                });

                if (data.Result == -1)
                {
                    response.Message = Resource.ItemAlreadyExists;
                    response.IsSuccess = false;
                }
                else
                {
                    response.Data = data.Result;
                    response.IsSuccess = true;
                    response.Message = Resource.SectionAddedSuccessfully;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse HC_SaveSubSection(SaveSubSection modal)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                int result = (int)GetScalar(StoredProcedure.HC_SaveSubSection, new List<SearchValueData>{
                new SearchValueData{Name = "SectionID",Value = Convert.ToString(modal.SectionID)},
                new SearchValueData{Name = "SubSectionName",Value = Convert.ToString(modal.SubSectionName)},
                new SearchValueData{Name = "IsTimeBased",Value = Convert.ToString(modal.IsTimeBased)},
                new SearchValueData{Name = "DocumentationType",Value = Convert.ToString(modal.DocumentationType)},
                new SearchValueData{Name = "UserType",Value = Convert.ToString((int)Common.UserType.Referral)},
                new SearchValueData{Name = "CurrentDate",Value = Convert.ToString(Common.GetOrgCurrentDateTime())},
                new SearchValueData{Name = "LoggedInID",Value = SessionHelper.LoggedInID.ToString()},
                new SearchValueData{Name = "SystemID",Value = Common.GetHostAddress()}
                });

                if (result == -1)
                {
                    response.Message = Resource.ItemAlreadyExists;
                    response.IsSuccess = false;
                }
                else
                {
                    response.IsSuccess = true;
                    response.Message = Resource.SubSectionAddedSuccessfully;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }


        public ServiceResponse HC_SaveSubSectionNew(AddDirSubDirModal modal)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                var UserType = modal.UserType == Common.UserType.Referral.ToString() ? (int)Common.UserType.Referral : (int)Common.UserType.Employee;
                var saveType = modal.UserType == "Employee" ? "EmployeeId" : "ReferralID";
                long result = (long)GetScalar(StoredProcedure.HC_SaveSubSectionNew, new List<SearchValueData>{
                    new SearchValueData{Name = "Name",Value = modal.Name},
                    new SearchValueData{Name = "Value",Value = modal.Value},
                    new SearchValueData{Name = "Type",Value = Constants.SubDirectory},
                    new SearchValueData{Name = "DocumentationType",Value = Convert.ToString(modal.DocumentationType)},
                    new SearchValueData{Name = "IsTimeBased",Value = Convert.ToString(modal.IsTimeBased)},
                    new SearchValueData{Name = "UserType",Value = Convert.ToString(UserType)},
                    new SearchValueData{Name = "EBFormID",Value = modal.EBFormID},
                    new SearchValueData{Name = "ParentID",Value = Convert.ToString(modal.ParentID)},
                    new SearchValueData{Name = "CurrentDate",Value = Convert.ToString(Common.GetOrgCurrentDateTime().ToString(Constants.DbDateTimeFormat))},
                    new SearchValueData{Name = "LoggedInID",Value = SessionHelper.LoggedInID.ToString()},
                    new SearchValueData{Name = "SystemID",Value = Common.GetHostAddress()},
                    new SearchValueData{Name = "SelectedRoles",Value = string.Join(",", modal.SelectedRoles)},
                    new SearchValueData{Name = "HideIfEmpty",Value = Convert.ToString(modal.HideIfEmpty)},
                    new SearchValueData{Name = "ShowToAll",Value = Convert.ToString(modal.ShowToAll)},
                     new SearchValueData{Name = "EmployeeID",Value = Convert.ToString(modal.EmployeeID)},
                     new SearchValueData{Name = "ReferralID",Value = Convert.ToString(modal.ReferralID)},
                });

                if (result == -1)
                {
                    response.Message = Resource.ItemAlreadyExists;
                    response.IsSuccess = false;
                }
                else
                {
                    response.Data = result;
                    response.IsSuccess = true;
                    response.Message = Resource.SubSectionAddedSuccessfully;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse HC_GetReferralFormList(FormModal modal)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<EBFormAndDoc> formList = GetEntityList<EBFormAndDoc>(StoredProcedure.HC_GetReferralFormList, new List<SearchValueData>{
                new SearchValueData{Name = "ComplianceID",Value = Convert.ToString(modal.ComplianceID)},
                new SearchValueData{Name = "ReferralID",Value = Convert.ToString(modal.ReferralID)},
                new SearchValueData{Name = "UserType",Value = Convert.ToString((int)ReferralDocument.UserTypes.Referral)}
                });

                response.IsSuccess = true;
                response.Data = formList;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse HC_MapForm(MapFormDocModel modal, long roleID)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                var UserType = (modal.UserType == Common.UserType.Referral.ToString()) ? (int)Common.UserType.Referral : (int)Common.UserType.Employee;
                MapFormDetailModel data = GetMultipleEntity<MapFormDetailModel>(StoredProcedure.HC_MapForm, new List<SearchValueData>{
                new SearchValueData{Name = "ComplianceID",Value = Convert.ToString(modal.ComplianceID)},
                new SearchValueData{Name = "SectionID",Value = Convert.ToString(modal.SectionID)},
                new SearchValueData{Name = "EBFormID",Value = modal.EBFormID},
                new SearchValueData{Name = "MapPermanently",Value = Convert.ToString(modal.MapPermanently)},
                new SearchValueData{Name = "UserType",Value = Convert.ToString(UserType)},
                new SearchValueData{Name = "CurrentDate",Value = Convert.ToString(Common.GetOrgCurrentDateTime())},
                new SearchValueData{Name = "LoggedInID",Value = SessionHelper.LoggedInID.ToString()},
                new SearchValueData{Name = "SystemID",Value = Common.GetHostAddress()},
                new SearchValueData{Name = "RoleID",Value = Convert.ToString(roleID)}
                });

                if (data != null)
                {
                    response.IsSuccess = true;
                    response.Message = Resource.PreferenceStoredSuccessfully;
                    response.Data = data;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = Resource.SomethingWentWrong;
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse HC_SavedNewHtmlFormWithSubsection(SaveNewEBFormModel model, long loggedInId)
        {
            ServiceResponse respose = new ServiceResponse();

            try
            {
                List<SearchValueData> searchModel = new List<SearchValueData>();

                if (model.UserType == ReferralDocument.UserTypes.Referral.ToString())
                    searchModel.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
                if (model.UserType == ReferralDocument.UserTypes.Employee.ToString())
                    searchModel.Add(new SearchValueData { Name = "EmployeeID", Value = Convert.ToString(model.EmployeeID) });
                searchModel.Add(new SearchValueData { Name = "SubSectionID", Value = Convert.ToString(model.SubSectionID) });
                searchModel.Add(new SearchValueData { Name = "EBriggsFormID", Value = Convert.ToString(model.EBriggsFormID) });
                searchModel.Add(new SearchValueData { Name = "OriginalEBFormID", Value = Convert.ToString(model.OriginalEBFormID) });
                searchModel.Add(new SearchValueData { Name = "FormName", Value = model.FormName });
                searchModel.Add(new SearchValueData { Name = "UpdateFormName", Value = Convert.ToString(model.UpdateFormName) });
                searchModel.Add(new SearchValueData { Name = "FormId", Value = Convert.ToString(model.FormId) });
                searchModel.Add(new SearchValueData { Name = "HTMLFormContent", Value = model.HTMLFormContent });
                searchModel.Add(new SearchValueData { Name = "EbriggsFormMppingID", Value = Convert.ToString(model.EbriggsFormMppingID) });
                searchModel.Add(new SearchValueData { Name = "LoggedInID", Value = Convert.ToString(loggedInId) });
                searchModel.Add(new SearchValueData { Name = "SystemID", Value = Common.GetMAcAddress() });


                GetScalar(StoredProcedure.HC_SavedNewHtmlFormWithSubsection, searchModel);
                respose.IsSuccess = true;
                respose.Message = Resource.FormSavedSuccessfully;

            }
            catch (Exception e)
            {
                respose.Message = Resource.EbriggsFormSaveIssue;
            }
            return respose;
        }

        public ServiceResponse HC_DeleteReferralDocument(DeleteDocModel model, long loggedInId)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                if (model.ReferralDocumentID > 0)
                {
                    var fullPath = HttpContext.Current.Server.MapCustomPath(model.FilePath);
                    response = Common.DeleteFile(fullPath);
                }

                List<SearchValueData> searchParam = new List<SearchValueData>();
                searchParam.Add(new SearchValueData("ReferralDocumentID", Convert.ToString(model.ReferralDocumentID)));
                searchParam.Add(new SearchValueData("EbriggsFormMppingID", Convert.ToString(model.EbriggsFormMppingID)));
                searchParam.Add(new SearchValueData("CurrentDateTime", Common.GetOrgCurrentDateTime().ToString(Constants.DbDateTimeFormat)));
                searchParam.Add(new SearchValueData("LoggedInID", Convert.ToString(loggedInId)));
                searchParam.Add(new SearchValueData("IsDeleted", Convert.ToString(model.IsDeleted)));
                GetScalar(StoredProcedure.HC_DeleteReferralDocument, searchParam);

                //FormModal data = new FormModal
                //{
                //    ReferralID = model.ReferralID,
                //    ComplianceID = model.ComplianceID
                //};

                //response.Data = HC_GetReferralFormList(data).Data;
                response.IsSuccess = true;
                response.Message = Resource.DocumentDeleted;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }

            return response;
        }

        public ServiceResponse DeleteReferralDocumentViaAPI(DeleteDocModel model)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                if (Convert.ToInt64(model.ReferralDocumentID) > 0)
                {
                    Uri uri = new Uri(model.FilePath);
                    var fullPath = HttpContext.Current.Server.MapCustomPath(uri.PathAndQuery);
                    response = Common.DeleteFile(fullPath);
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }

            return response;
        }

        public ServiceResponse SaveDocumentFormName(DocFormNameModal model)
        {
            ServiceResponse respose = new ServiceResponse();

            try
            {
                List<SearchValueData> searchModel = new List<SearchValueData>();
                searchModel.Add(new SearchValueData { Name = "EbriggsFormMppingID", Value = Convert.ToString(model.EbriggsFormMppingID) });
                searchModel.Add(new SearchValueData { Name = "FormName", Value = model.FormName });
                searchModel.Add(new SearchValueData { Name = "UpdateFormName", Value = Convert.ToString(model.UpdateFormName) });

                GetScalar(StoredProcedure.SaveDocumentFormName, searchModel);
                respose.IsSuccess = true;

            }
            catch (Exception e)
            {
                respose.Message = e.Message;
            }
            return respose;
        }
        #endregion

        #endregion

        #region Referral Time Slots For Care Type
        public ServiceResponse HC_ReferralCareTypeTimeSlots()
        {
            var response = new ServiceResponse();


            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData>();
                searchParam.Add(new SearchValueData { Name = "DDType_CareType", Value = Convert.ToString((int)Common.DDType.CareType) });
                RefCareTypeSlotsModel model = GetMultipleEntity<RefCareTypeSlotsModel>(StoredProcedure.GetReferralTimeSlotsPageModel, searchParam);
                model.AddTimeSlots = new CareTypeTimeSlot();
                response.IsSuccess = true;
                response.Data = model;

            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }


            return response;
        }

        public ServiceResponse AddCareTypeSlot(CareTypeTimeSlot model, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> paramList = new List<SearchValueData>();
                paramList.Add(new SearchValueData { Name = "CareTypeTimeSlotID", Value = Convert.ToString(model.CareTypeTimeSlotID) });
                paramList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
                paramList.Add(new SearchValueData { Name = "CareTypeID", Value = Convert.ToString(model.CareTypeID) });
                paramList.Add(new SearchValueData { Name = "Count", Value = Convert.ToString(model.Count) });
                paramList.Add(new SearchValueData { Name = "Frequency", Value = Convert.ToString(model.Frequency) });
                paramList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(model.StartDate).ToString(Constants.DbDateFormat) });
                paramList.Add(new SearchValueData { Name = "EndDate", Value = model.EndDate != null ? Convert.ToDateTime(model.EndDate).ToString(Constants.DbDateFormat) : null });
                paramList.Add(new SearchValueData { Name = "TodayDate", Value = Common.GetOrgCurrentDateTime().ToString(Constants.DbDateFormat) });
                paramList.Add(new SearchValueData { Name = "loggedInUserID", Value = Convert.ToString(loggedInUserID) });
                paramList.Add(new SearchValueData { Name = "SystemID", Value = Common.GetHostAddress() });
                TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.AddCareTypeSlot, paramList);

                if (result.TransactionResultId == -1)
                {
                    response.Message = Common.MessageWithTitle(Resource.Error, Resource.CareTypeScheduleDateRangeExist);
                    return response;
                }

                response.Message = model.CareTypeTimeSlotID > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.Schedule) :
                        string.Format(Resource.RecordCreatedSuccessfully, Resource.Schedule);
                response.IsSuccess = true;
                response.Data = result.TablePrimaryId > 0 ? result.TablePrimaryId : model.CareTypeTimeSlotID;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, e.Message);
            }
            return response;
        }

        public ServiceResponse GetCareTypeScheduleList(SearchCTSchedule searchCTSchedule, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchCTSchedule != null)
                SetSearchFilterForCareTypeSchedulePage(searchCTSchedule, searchList);

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<ListCareTypeSchedule> totalData = GetEntityList<ListCareTypeSchedule>(StoredProcedure.GetCareTypeScheduleList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListCareTypeSchedule> getEmployeeList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = getEmployeeList;
            response.IsSuccess = true;
            return response;
        }

        private static void SetSearchFilterForCareTypeSchedulePage(SearchCTSchedule searchCTSchedule, List<SearchValueData> searchList)
        {
            searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(searchCTSchedule.ReferralID) });
            if (searchCTSchedule.StartDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(searchCTSchedule.StartDate).ToString(Constants.DbDateFormat) });
            if (searchCTSchedule.EndDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(searchCTSchedule.EndDate).ToString(Constants.DbDateFormat) });
        }
        #endregion

        #region Upload Referral Document From API
        public ServiceResponse UploadDocumentViaAPI(HttpRequestBase currentHttpRequest)
        {
            ServiceResponse response = new ServiceResponse();
            MemoryStream ms = new MemoryStream();

            var EmployeeID = currentHttpRequest.Headers["EmployeeID"];
            var ReferralID = currentHttpRequest.Headers["ReferralID"];
            var ComplianceID = currentHttpRequest.Headers["ComplianceID"];
            var KindOfDocument = currentHttpRequest.Headers["KindOfDocument"];
            var FileName = currentHttpRequest.Headers["FileName"];
            var FileType = currentHttpRequest.Headers["FileType"];
            var UserType = currentHttpRequest.Headers["UserType"];

            currentHttpRequest.InputStream.CopyTo(ms);
            byte[] byteData = ms.ToArray();

            if (byteData.Length > 0)
            {
                string fullPath = String.Format(_cacheHelper.ReferralDocumentFullPath, _cacheHelper.Domain) + ReferralID + "/";

                DirectoryInfo di = new DirectoryInfo(HttpContext.Current.Server.MapCustomPath(fullPath));
                response = Common.ByteToImage(byteData, fullPath, FileType);

                List<SearchValueData> searchList = new List<SearchValueData>()
                    {
                        new SearchValueData {Name = "FileName", Value = FileName},
                        new SearchValueData {Name = "FilePath", Value = ((UploadedFileModel)response.Data).TempFilePath},
                        new SearchValueData {Name = "ReferralID ", Value = ReferralID },
                        new SearchValueData {Name = "ComplianceID ", Value = ComplianceID },
                        //new SearchValueData {Name = "UserType", Value = Convert.ToString((int)ReferralDocument.UserTypes.Referral) },
                        new SearchValueData {Name = "KindOfDocument ", Value = KindOfDocument },
                        new SearchValueData {Name = "LoggedInUserID", Value = EmployeeID},
                        new SearchValueData {Name = "SystemID ", Value = HttpContext.Current.Request.UserHostAddress },
                    };
                if (UserType != null || UserType != "")
                {
                    searchList.Add(new SearchValueData { Name = "UserType", Value = Convert.ToString(UserType) });
                }
                else
                {
                    searchList.Add(new SearchValueData { Name = "UserType", Value = Convert.ToString((int)ReferralDocument.UserTypes.Referral) });
                }

                GetScalar(StoredProcedure.HC_SaveReferralDocument, searchList);
                return response;
            }
            response.Message = "File Upload Failed NoFile Selected";
            return response;
        }
        #endregion

        public ServiceResponse GetReferralMedications(int referralId, int pageSize, bool isActive)
        {
            ServiceResponse response = new ServiceResponse();
            if (referralId > 0)
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "ReferralId", Value = Convert.ToString(referralId) });
                searchList.Add(new SearchValueData { Name = "IsActive", Value = Convert.ToString(isActive) });

                List<ReferralMedicationDetails> responseData = GetEntityList<ReferralMedicationDetails>(StoredProcedure.GetReferralMedications, searchList);

                response.Data = responseData;
                response.IsSuccess = true;
            }
            else
                response.Message = Resource.Fail;
            return response;
        }

        public ServiceResponse SaveReferralAllergy(ReferralAllergyModel model)
        {
            //int status = 0;
            //if(Convert.ToBoolean(model.Status)==true)
            //{
            //   status = 1;

            //}
            ServiceResponse response = new ServiceResponse();
            if (model.Patient.ToString().Length > 0)
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "Id", Value = Convert.ToString(model.Id) });
                searchList.Add(new SearchValueData { Name = "Allergy", Value = Convert.ToString(model.Allergy) });
                searchList.Add(new SearchValueData { Name = "Status", Value = Convert.ToString(model.Status) });
                searchList.Add(new SearchValueData { Name = "Reaction", Value = Convert.ToString(model.Reaction) });
                searchList.Add(new SearchValueData { Name = "Comment", Value = Convert.ToString(model.Comment) });
                searchList.Add(new SearchValueData { Name = "CreatedBy", Value = Convert.ToString(model.CreatedBy) });
                searchList.Add(new SearchValueData { Name = "Patient", Value = Convert.ToString(model.Patient) });
                searchList.Add(new SearchValueData { Name = "ReportedBy", Value = Convert.ToString(model.ReportedBy) });

                GetScalar(StoredProcedure.SaveReferralallergyDetails, searchList);
                response.IsSuccess = true;
                response.Message = Resource.NoteSavedSuccessfully;
            }
            else
                response.Message = Resource.NoteFailed;
            return response;
        }

        public ServiceResponse GetReferralAllergy(SearchAllergyModel model)
        {
            ServiceResponse response = new ServiceResponse();
            if (model.ReferralId.Length > 0)
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "patient", Value = Convert.ToString(model.ReferralId) });
                searchList.Add(new SearchValueData { Name = "allergy", Value = Convert.ToString(model.Allergy) });
                searchList.Add(new SearchValueData { Name = "status", Value = Convert.ToString(model.Status) });
                List<ReferralAllergyModel> responseData = GetEntityList<ReferralAllergyModel>(StoredProcedure.GetReferralallergyDetails, searchList);

                response.Data = responseData;
                response.IsSuccess = true;
            }
            else
                response.Message = Resource.Fail;
            return response;
        }


        public ServiceResponse GetAllergyDDL()
        {
            try
            {
                ServiceResponse response = new ServiceResponse();
                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "param", Value = Convert.ToString("") });
                List<AllergyDropDown> responseData = GetEntityList<AllergyDropDown>(StoredProcedure.GetAllergyDDL, searchList);
                response.Data = responseData;
                response.IsSuccess = true;

                return response;
            }
            catch
            {
                return null;
            }
        }

        public ServiceResponse SendFax(FaxModel model)
        {
            ServiceResponse response = new ServiceResponse();
            if (model != null)
            {

                //List<SearchValueData> searchList = new List<SearchValueData>();
                //searchList.Add(new SearchValueData { Name = "param", Value = Convert.ToString("") });
                FaxSettings responseData = GetEntity<FaxSettings>(StoredProcedure.GetFaxSetting);
                string accountSid = "AC5d29d9c529ed8fe0ebbb4f31d09432a0";//System.Configuration.ConfigurationManager.AppSettings["FaxAccountSid"];
                string authToken = "ed685a2a94b6bc0bf67eddbafb081c0b"; //System.Configuration.ConfigurationManager.AppSettings["FaxAuthToken"];

                TwilioClient.Init(accountSid, authToken);

                var fax = FaxResource.Create(
                    from: "+12672142273",
                    to: "+18557534312",
                    mediaUrl: new Uri(model.Path)//https://www.twilio.com/docs/documents/25/justthefaxmaam.pd
                );
            }

            return response;
        }


        public ServiceResponse GetAllergyTitle()
        {

            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "id", Value = Convert.ToString("") });
            List<AllergyDropDown> responseData = GetEntityList<AllergyDropDown>(StoredProcedure.GetAllergyTitle, searchList);
            response.Data = responseData;
            response.IsSuccess = true;

            return response;
        }

        //public ServiceResponse GetAllergy()
        //{
        //         ServiceResponse response = new ServiceResponse();

        //        List<AllergyModel> responseData = GetEntityList<AllergyModel>(StoredProcedure.Getallery);
        //        response.Data = responseData;
        //        response.IsSuccess = true;
        //        return response;
        //}



        public ServiceResponse SearchReferralMedications(string search)
        {
            ServiceResponse response = new ServiceResponse();
            if (search != null)
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "MedicationName", Value = Convert.ToString(search) });


                List<SearchMedication> responseData = GetEntityList<SearchMedication>(StoredProcedure.SearchMedication, searchList);

                response.Data = responseData;
                response.IsSuccess = true;
            }
            else
                response.Message = Resource.Fail;
            return response;
        }




        public ServiceResponse Medication(ReferralMedication ReferralMedication)
        {
            ServiceResponse response = new ServiceResponse();
            if (ReferralMedication.ReferralID > 0)
            {
                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralID", Value = Convert.ToString(ReferralMedication.ReferralID)},
                    //new SearchValueData {Name = "ReferralDrugMapping", Value = Convert.ToString(ReferralMedication.ReferralDrugMapping)},
                    //new SearchValueData {Name = "DrugID", Value = Convert.ToString(ReferralMedication.DrugID)},
                    //new SearchValueData {Name = "TypeofMedication", Value = Convert.ToString(ReferralMedication.TypeofMedication)},
                    //new SearchValueData {Name = "PrescribedBy", Value = Convert.ToString(ReferralMedication.PrescribedBy)},
                    //new SearchValueData {Name = "PharmacyName", Value = Convert.ToString(ReferralMedication.PharmacyName)},
                    //new SearchValueData {Name = "Amount", Value = Convert.ToString(ReferralMedication.Amount)},
                    //new SearchValueData {Name = "Frequency", Value = Convert.ToString(ReferralMedication.Frequency)},
                    //new SearchValueData {Name = "GivenFor", Value = Convert.ToString(ReferralMedication.GivenFor)},
                    //new SearchValueData {Name = "Instructions", Value = Convert.ToString(ReferralMedication.Instructions)},
                    //new SearchValueData {Name = "Dosage", Value = Convert.ToString(ReferralMedication.Dosage)},
                    //new SearchValueData {Name = "Route", Value = Convert.ToString(ReferralMedication.Route)},
                    //new SearchValueData {Name = "TimeofDay", Value = Convert.ToString(ReferralMedication.TimeofDay)},
                    //new SearchValueData {Name = "Precautions", Value = Convert.ToString(ReferralMedication.Precautions)},

                };
                GetScalar(StoredProcedure.SaveMedication, searchlist);
                response.IsSuccess = true;
                response.Message = Resource.NoteSavedSuccessfully;
            }
            else
                response.Message = Resource.NoteFailed;
            return response;
        }
        public ServiceResponse SaveReferralMedication(ReferralMedication ReferralMedication)
        {
            ServiceResponse response = new ServiceResponse();
            if (ReferralMedication.ReferralID > 0)
            {
                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralID", Value = Convert.ToString(ReferralMedication.ReferralID)},
                    new SearchValueData {Name = "MedicationId", Value = Convert.ToString(ReferralMedication.MedicationId)},
                    new SearchValueData {Name = "PhysicianID", Value = Convert.ToString(ReferralMedication.PhysicianID)},
                    new SearchValueData {Name = "Dose", Value = Convert.ToString(ReferralMedication.Dose)},
                    new SearchValueData {Name = "Unit", Value = Convert.ToString(ReferralMedication.Unit)},
                    new SearchValueData {Name = "Route", Value = Convert.ToString(ReferralMedication.Route)},
                    new SearchValueData {Name = "Frequency", Value = Convert.ToString(ReferralMedication.Frequency)},
                    new SearchValueData {Name = "Quantity", Value = Convert.ToString(ReferralMedication.Quantity)},
                    new SearchValueData {Name = "StartDate", Value = Convert.ToString(ReferralMedication.StartDate)},
                    new SearchValueData {Name = "EndDate", Value = Convert.ToString(ReferralMedication.EndDate)},
                    new SearchValueData {Name = "IsActive", Value = Convert.ToString(ReferralMedication.IsActive)},
                    new SearchValueData {Name = "SystemID", Value = Convert.ToString(HttpContext.Current.Request.UserHostAddress)},
                    new SearchValueData {Name = "HealthDiagnostics", Value = Convert.ToString(ReferralMedication.HealthDiagnostics)},
                    new SearchValueData {Name = "PatientInstructions", Value = Convert.ToString(ReferralMedication.PatientInstructions)},
                    new SearchValueData {Name = "PharmacistInstructions", Value = Convert.ToString(ReferralMedication.PharmacistInstructions)},
                    new SearchValueData {Name = "ReferralMedicationID", Value = Convert.ToString(ReferralMedication.ReferralMedicationID)},
                    new SearchValueData {Name = "DosageTime", Value = Convert.ToString(ReferralMedication.DosageTime)},

                };
                GetScalar(StoredProcedure.SaveReferralMedication, searchlist);
                response.IsSuccess = true;
                //response.Message = Resource.NoteSavedSuccessfully;
            }
            //else
            //    response.Message = Resource.NoteFailed;
            return response;
        }
        public ServiceResponse DeleteReferralMedication(long ReferralMedicationID, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();
            if (ReferralMedicationID > 0)
            {
                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralMedicationID", Value = Convert.ToString(ReferralMedicationID)},
                    new SearchValueData {Name = "LoggedInUserID", Value = Convert.ToString(loggedInUserID)},
                };


                GetScalar(StoredProcedure.DeleteReferralMedication, searchlist);
                response.IsSuccess = true;
            }

            if (!response.IsSuccess)
                response.Message = Resource.ExceptionMessage;

            return response;
        }
        public ServiceResponse EditReferralMedication(long ReferralMedicationID)
        {
            ServiceResponse response = new ServiceResponse();
            if (ReferralMedicationID > 0)
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "ReferralMedicationID", Value = Convert.ToString(ReferralMedicationID) });
                //searchList.Add(new SearchValueData { Name = "IsActive", Value = Convert.ToString(isActive) });

                ReferralMedicationDetails responseData = GetEntity<ReferralMedicationDetails>(StoredProcedure.GetReferralMedicationsById, searchList);

                response.Data = responseData;
                response.IsSuccess = true;
            }
            else
                response.Message = Resource.Fail;
            return response;
        }

        public ServiceResponse HC_DeleteReferralDocumentGoogle(DeleteDocModel model, long loggedInId, string googleRefeshToken)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                if (model.ReferralDocumentID > 0)
                {
                    response = new GoogleDriveHelper().DeleteFile(googleRefeshToken, model.GoogleFileId);
                }

                List<SearchValueData> searchParam = new List<SearchValueData>();
                searchParam.Add(new SearchValueData("ReferralDocumentID", Convert.ToString(model.ReferralDocumentID)));
                searchParam.Add(new SearchValueData("EbriggsFormMppingID", Convert.ToString(model.EbriggsFormMppingID)));
                searchParam.Add(new SearchValueData("CurrentDateTime", Common.GetOrgCurrentDateTime().ToString(Constants.DbDateTimeFormat)));
                searchParam.Add(new SearchValueData("LoggedInID", Convert.ToString(loggedInId)));
                GetScalar(StoredProcedure.HC_DeleteReferralDocument, searchParam);

                //FormModal data = new FormModal
                //{
                //    ReferralID = model.ReferralID,
                //    ComplianceID = model.ComplianceID
                //};

                //response.Data = HC_GetReferralFormList(data).Data;
                response.IsSuccess = true;
                response.Message = Resource.DocumentDeleted;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }

            return response;
        }

        public ServiceResponse HC_GetDocumentListGoogleDrive(string refreshToken)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                response = new GoogleDriveHelper().GetFiles(refreshToken);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }

            return response;
        }

        public ServiceResponse HC_LinkGoogleDocument(LinkDocModel model, string googleRefeshToken, bool isEmployeeDocument)
        {
            ServiceResponse fileResponse = new ServiceResponse();
            fileResponse.IsSuccess = false;

            try
            {
                if (model.ComplianceID <= 0)
                {
                    fileResponse.Message = "Missing Compliance Information";
                    return fileResponse;
                }
                if (string.IsNullOrEmpty(model.GoogleFileId))
                {
                    fileResponse.Message = "Missing Google File Information";
                    return fileResponse;
                }

                string basePath = String.Format(isEmployeeDocument ? _cacheHelper.EmployeeDocumentFullPath : _cacheHelper.ReferralDocumentFullPath, _cacheHelper.Domain) + model.ReferralID + "/";
                //string KindOfDocument = Convert.ToString(DocumentType.DocumentKind.Internal);
                //int DocumentTypeID = (int)DocumentType.DocumentTypes.Other;
                int UserType = isEmployeeDocument ? (int)ReferralDocument.UserTypes.Employee : (int)ReferralDocument.UserTypes.Referral;
                Compliance compliance = GetEntity<Compliance>(model.ComplianceID);
                string KindOfDocument = compliance.DocumentationType == 1 ? DocumentType.DocumentKind.Internal.ToString() : DocumentType.DocumentKind.External.ToString();



                fileResponse = new GoogleDriveHelper().GetFileInfo(googleRefeshToken, model.GoogleFileId);

                var googleFileModel = ((UploadedFileModel)fileResponse.Data);

                List<SearchValueData> searchList = new List<SearchValueData>()
                    {
                        new SearchValueData {Name = "FileName", Value = googleFileModel.FileOriginalName},
                        new SearchValueData {Name = "FilePath", Value = googleFileModel.TempFilePath},
                        new SearchValueData {Name = "ReferralID ", Value = model.ReferralID.ToString() },
                        new SearchValueData {Name = "ComplianceID ", Value = model.ComplianceID.ToString() },
                        new SearchValueData {Name = "UserType", Value = UserType.ToString() },
                        new SearchValueData {Name = "KindOfDocument ", Value = KindOfDocument },
                        //new SearchValueData {Name = "DocumentTypeID ", Value = DocumentTypeID.ToString() },
                        new SearchValueData {Name = "LoggedInUserID", Value = SessionHelper.LoggedInID.ToString()},
                        new SearchValueData {Name = "SystemID ", Value = HttpContext.Current.Request.UserHostAddress },
                        new SearchValueData {Name = "StoreType ", Value = "Google Drive" },
                        new SearchValueData {Name = "GoogleFileId ", Value = model.GoogleFileId },
                        new SearchValueData {Name = "GoogleDetails ", Value = googleFileModel.GoogleFileJson },
                    };

                GetScalar(StoredProcedure.HC_SaveReferralDocument, searchList);
            }
            catch (Exception ex)
            {
                fileResponse.IsSuccess = false;
                fileResponse.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }

            return fileResponse;
        }

        public ServiceResponse HC_GetReferralPayorsMapping(long referralID, DateTime startDate)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData>();
                searchParam.Add(new SearchValueData("ReferralID", Convert.ToString(referralID)));
                searchParam.Add(new SearchValueData("StartDate", Convert.ToString(startDate)));
                List<PayorList> totalData = GetEntityList<PayorList>(StoredProcedure.HC_GetReferralPayorsMapping, searchParam);
                response.Data = totalData;
                response.IsSuccess = true;
                response.Message = Resource.RecordsFound;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }

            return response;
        }

        public ServiceResponse HC_GetPriorAutherizationCodeByPayorAndRererrals(long payorID, long referralID)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData>();
                searchParam.Add(new SearchValueData("PayorID", Convert.ToString(payorID)));
                searchParam.Add(new SearchValueData("ReferralID", Convert.ToString(referralID)));
                List<GetReferralAuthorizationCodeDetailsModel> totalData = GetEntityList<GetReferralAuthorizationCodeDetailsModel>(StoredProcedure.HC_GetPriorAutherizationCodeByPayorAndRererrals, searchParam);
                response.Data = totalData;
                response.IsSuccess = true;
                response.Message = Resource.RecordsFound;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }

            return response;
        }

        #region ----- Orbeon Forms -----

        public ServiceResponse HC_GetOrbeonFormDetailsByID(LinkDocModel model, bool isEmployeeDocument)
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
                ///////////////////////////////////////////////

                // get date as per orbeon document-id
                //List<SearchValueData> searchList = new List<SearchValueData>();
                //searchList.Add(new SearchValueData { Name = "DocumentID", Value = model.DocumentID });
                //var result = GetEntityList<SearchOrbeonFormSearch>(StoredProcedure.GetEZOrbeonData_ByFormID, searchList);

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


                    /*
                    HttpPostedFileBase file = currentHttpRequest.Files[i];
                    ServiceResponse fileResponse;
                    string storeType = "Local";
                    string googleFileId = "";
                    string googleFileJson = "";

                    if (!isGoogleDriveDocument)
                        fileResponse = Common.SaveFile(file, basePath);
                    else
                    {
                        storeType = "Google Drive";
                        var target = new System.IO.MemoryStream();
                        file.InputStream.CopyTo(target);
                        byte[] fileAsBytes = target.ToArray();

                        fileResponse = new GoogleDriveHelper().SaveFile(googleRefreshToken, fileAsBytes, file.ContentType, "", file.FileName);

                        // save json for reference
                        googleFileJson = ((UploadedFileModel)fileResponse.Data).GoogleFileJson;


                        // save id into a column (for update|delete)
                        dynamic googleResponseObj = JsonConvert.DeserializeObject(googleFileJson);
                        googleFileId = (string)googleResponseObj.id;
                    }
                    */

                    //var actualFilepath = ((UploadedFileModel)fileResponse.Data).TempFilePath;
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

        #endregion


        #region Referral Certificates

        public ServiceResponse GetReferralCertifictaes(long EmployeeId)
        {
            ServiceResponse response = new ServiceResponse();
            if (EmployeeId > 0)
            {
                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "EmployeeID", Value = Convert.ToString(EmployeeId)},
                };

                List<ReferralCertificate> totalData = GetEntityList<ReferralCertificate>(StoredProcedure.GetCertificateList, searchlist);


                response.IsSuccess = true;
                response.Data = totalData;
            }
            else
                response.Message = Resource.ExceptionMessage;
            return response;
        }

        public ServiceResponse SaveCertificates(ReferralCertificate model)
        {
            ServiceResponse response = new ServiceResponse();
            if (model.Name != null)
            {
                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "CertificatePath", Value = Convert.ToString(model.CertificatePath)},
                    new SearchValueData {Name = "Name", Value = Convert.ToString(model.Name)},
                    new SearchValueData {Name = "StartDate", Value = model.StartDate.ToString(Constants.DbDateFormat) },//Convert.ToString(model.StartDate)},
                    new SearchValueData {Name = "EndDate", Value = model.EndDate.ToString(Constants.DbDateFormat)},//Convert.ToString(model.EndDate)},
                    new SearchValueData {Name = "EmployeeID", Value = Convert.ToString(model.EmployeeID)},
                    new SearchValueData {Name = "CreatedBy", Value = Convert.ToString(model.CreatedBy)},
                    new SearchValueData {Name = "CertificateAuthority", Value = Convert.ToString(model.CertificateAuthority)},
                };

                GetScalar(StoredProcedure.SaveCertificates, searchlist);
                response.IsSuccess = true;
                response.Data = "Certificate Uploaded Successfully";
            }
            else
                response.Message = "Certificate Failed";
            return response;
        }


        public ServiceResponse DeleteCertificates(long certificateid)
        {
            ServiceResponse response = new ServiceResponse();
            if (certificateid > 0)
            {
                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "CertificateID", Value = Convert.ToString(certificateid)}


                };

                GetScalar(StoredProcedure.DeleteCertificate, searchlist);
                response.IsSuccess = true;
                response.Data = "Certificate Deleted Successfully";
            }
            else
                response.Message = "Certification Failed";
            return response;
        }


        public ServiceResponse DeleteAllergy(string id)
        {
            ServiceResponse response = new ServiceResponse();
            if (id.Length > 0)
            {
                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "id", Value = Convert.ToString(id)}

                };

                GetScalar(StoredProcedure.DeleteAllergy, searchlist);
                response.IsSuccess = true;
                response.Data = "Allergy Deleted Successfully";
            }
            else
                response.Message = "Allergy Failed";
            return response;
        }




        public ServiceResponse CertificateAuthority()
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData>();
                //searchParam.Add(new SearchValueData { Name = "DDType_CareType", Value = Convert.ToString((int)Common.DDType.CareType) });
                List<CertificateAuthorityModel> model = GetEntityList<CertificateAuthorityModel>(StoredProcedure.GetCertificateAuthority, searchParam);

                response.IsSuccess = true;
                response.Data = model;

            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }


            return response;
        }

        public ServiceResponse UploadCertificate(HttpRequestBase currentHttpRequest, bool isEmployeeDocument = false)
        {
            CacheHelper _cacheHelper = new CacheHelper();
            ServiceResponse response = new ServiceResponse();
            var ReferralId = currentHttpRequest.Form["id"];
            HttpPostedFileBase file = currentHttpRequest.Files[0];

            string basePath = String.Format(_cacheHelper.UploadPath, _cacheHelper.Domain) + ConfigSettings.EmpCertificate;
            basePath += SessionHelper.LoggedInID + "/";
            response = Common.SaveFile(file, basePath);


            var fileResponse = Common.SaveFile(file, basePath);

            var actualFilepath = ((UploadedFileModel)fileResponse.Data).TempFilePath;

            List<SearchValueData> searchList = new List<SearchValueData>()
                    {
                        //new SearchValueData {Name = "FileName", Value = file.FileName},
                        new SearchValueData {Name = "FilePath", Value = actualFilepath},
                      //  new SearchValueData {Name = "EmployeeID ", Value = SessionHelper.LoggedInID.ToString() },
                        new SearchValueData {Name = "LoggedInUserID", Value = SessionHelper.LoggedInID.ToString()},
                       // new SearchValueData {Name = "SystemID ", Value = HttpContext.Current.Request.UserHostAddress },
                    };

            GetScalar(StoredProcedure.UploadCerificate, searchList);



            //response.Message = "Profile Image Save Successfully";
            return response;
        }

        public ServiceResponse GetEmployeeEmail(long id)
        {
            ServiceResponse response = new ServiceResponse();
            if (id > 0)
            {
                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "EmployeeID", Value = Convert.ToString(id)},
                };

                EmployeeEmail totalData = GetMultipleEntity<EmployeeEmail>(StoredProcedure.GetEmployeeEmail, searchlist);


                response.IsSuccess = true;
                response.Message = totalData.Email;
            }
            else
                response.Message = Resource.ExceptionMessage;
            return response;
        }


        public ServiceResponse GetNoteSentenceList(string NoteSentenceTitle, string NoteSentenceDetails, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();

            if (NoteSentenceTitle != null || NoteSentenceDetails != null)
                SearchFilterForNoteSentenceList(NoteSentenceTitle, NoteSentenceDetails, searchList);

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<NoteSentence> totalData = GetEntityList<NoteSentence>(StoredProcedure.GetNoteSentenceList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<NoteSentence> noteSentenceList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = noteSentenceList;
            response.IsSuccess = true;
            return response;
        }

        private static void SearchFilterForNoteSentenceList(string NoteSentenceTitle, string NoteSentenceDetails, List<SearchValueData> searchList)
        {
            if (!string.IsNullOrEmpty(NoteSentenceTitle))
                searchList.Add(new SearchValueData { Name = "NoteSentenceTitle", Value = Convert.ToString(NoteSentenceTitle) });

            if (!string.IsNullOrEmpty(NoteSentenceDetails))
                searchList.Add(new SearchValueData { Name = "NoteSentenceDetails", Value = Convert.ToString(NoteSentenceDetails) });
        }
        #endregion

        public ServiceResponse DxCodeMappingList1(long RefID)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>
                    {
                        new SearchValueData { Name = "ReferralID", Value = Convert.ToString(RefID) },
                    };
            List<DXCodeMappingList1> mappingList = GetEntityList<DXCodeMappingList1>(StoredProcedure.GetReferralDXCodeMappingsList, searchList);
            response.Data = mappingList;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse HC_SaveRefProfileImg(HttpRequestBase currentHttpRequest, bool isEmployeeDocument = false)
        {
            CacheHelper _cacheHelper = new CacheHelper();
            ServiceResponse response = new ServiceResponse();
            var ReferralId = currentHttpRequest.Form["id"];
            HttpPostedFileBase file = currentHttpRequest.Files[0];

            string basePath = String.Format(_cacheHelper.UploadPath, _cacheHelper.Domain) + ConfigSettings.ReferralProfileImg;
            basePath += SessionHelper.LoggedInID + "/";
            response = Common.SaveFile(file, basePath);


            var fileResponse = Common.SaveFile(file, basePath);

            var actualFilepath = ((UploadedFileModel)fileResponse.Data).TempFilePath;

            List<SearchValueData> searchList = new List<SearchValueData>()
                    {
                        new SearchValueData {Name = "FileName", Value = file.FileName},
                        new SearchValueData {Name = "FilePath", Value = actualFilepath},
                        new SearchValueData {Name = "EmployeeID ", Value = SessionHelper.LoggedInID.ToString() },
                        new SearchValueData {Name = "LoggedInUserID", Value = SessionHelper.LoggedInID.ToString()},
                        new SearchValueData {Name = "SystemID ", Value = HttpContext.Current.Request.UserHostAddress },
                    };

            GetScalar(StoredProcedure.SaveProfileImage, searchList);



            response.Message = "Profile Image Save Successfully";
            return response;
        }

        public ServiceResponse SendBulkEmail(MailModel model)
        {
            //long v = SessionHelper.ReferraID;
            ServiceResponse response = new ServiceResponse();
            bool isSentMail = false;
            if (model != null)
            {

                isSentMail = Common.SendEmail(model.Subject, model.From, model.To, model.Body, EnumEmailType.HomeCare_Schedule_Registration_Notification.ToString(), model.CC, 1, model.Attachment);

            }
            response.IsSuccess = isSentMail;
            return response;
        }

        public ServiceResponse GetReferralEmail(string ReferralID)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                if (ReferralID != null)
                {
                    var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralIDs", Value = Convert.ToString(ReferralID)},
                };

                    List<ReferralEmail> totalData = GetEntityList<ReferralEmail>(StoredProcedure.GetReferralEmail, searchlist);
                    response.IsSuccess = true;
                    response.Data = totalData;
                }
                else
                    response.Message = Resource.ExceptionMessage;
                return response;
            }

            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse HC_GetReferralPayor(long loggedInUser)
        {
            ServiceResponse response = new ServiceResponse();

            //Search List
            List<SearchValueData> searchList = new List<SearchValueData>();

            List<GetReferralPayorModel> data = GetEntityList<GetReferralPayorModel>(StoredProcedure.GetReferralPayor, searchList);
            int count = 0;
            if (data != null && data.Count > 0)
                count = data.First().Count;

            Page<GetReferralPayorModel> model = GetPageInStoredProcResultSet(1, 10, count, data);

            response.Data = model;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse HC_GetReferralStatus(long loggedInUser)
        {
            ServiceResponse response = new ServiceResponse();

            //Search List
            List<SearchValueData> searchList = new List<SearchValueData>();

            List<GetReferralStatusModel> data = GetEntityList<GetReferralStatusModel>(StoredProcedure.GetReferralStatus, searchList);
            int count = 0;
            if (data != null && data.Count > 0)
                count = data.First().Count;

            Page<GetReferralStatusModel> model = GetPageInStoredProcResultSet(1, 10, count, data);

            response.Data = model;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse HC_GetReferralCareType(long loggedInUser)
        {
            ServiceResponse response = new ServiceResponse();

            //Search List
            List<SearchValueData> searchList = new List<SearchValueData>();

            List<GetReferralCareTypeModel> data = GetEntityList<GetReferralCareTypeModel>(StoredProcedure.GetReferralCareType, searchList);
            int count = 0;
            if (data != null && data.Count > 0)
                count = data.First().Count;

            Page<GetReferralCareTypeModel> model = GetPageInStoredProcResultSet(1, 10, count, data);

            response.Data = model;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse GetPayorIdentificationNumber(string PayorID)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "PayorID", Value = PayorID });
                GetPayorIdentificationNumberModel data = GetEntity<GetPayorIdentificationNumberModel>(StoredProcedure.GetPayorIdentificationNumber, searchList);
                response.IsSuccess = true;
                response.Data = data;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }
        public ServiceResponse GetDXcodeList(string ReferralID, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();
            if (ReferralID != null)
            {
                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralID", Value = Convert.ToString(ReferralID)},
                    new SearchValueData {Name = "LoggedInUser", Value = Convert.ToString(loggedInUserID)},
                };
                List<DXcodeListModel> totalData = GetEntityList<DXcodeListModel>(StoredProcedure.GetReferralMappedDXcodeList, searchlist);
                response.IsSuccess = true;
                response.Data = totalData;
            }
            else
                response.Message = Resource.GetReferralInternalMessageError;
            return response;
        }
        public ServiceResponse DxChangeSortingOrder(DxChangeSortingOrderModel model)
        {
            ServiceResponse response = new ServiceResponse();
            if (model != null)
            {
                var searchParam = new List<SearchValueData>();
                searchParam.Add(new SearchValueData("ReferralDXCodeMappingID", Convert.ToString(model.ReferralDXCodeMappingID)));
                searchParam.Add(new SearchValueData("originID", Convert.ToString(model.originID)));
                searchParam.Add(new SearchValueData("destinationID", Convert.ToString(model.destinationID)));
                GetScalar(StoredProcedure.ReferralDXCodeMappingChangeSortingOrder, searchParam);
            }

            response.IsSuccess = true;
            response.Message = "Sorting Updated Successfully";
            return response;
        }
        public ServiceResponse GetPayorDetails(string PayorID, string ReferralID)
        {
            ServiceResponse response = new ServiceResponse();
            if (PayorID != null)
            {
                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "PayorID", Value = Convert.ToString(PayorID)},
                    new SearchValueData {Name = "ReferralID", Value = Convert.ToString(ReferralID)},
                };
                Payor totalData = GetEntity<Payor>(StoredProcedure.GetReferralPayorDetail, searchlist);
                response.IsSuccess = true;
                response.Data = totalData;
            }
            else
                response.Message = Resource.GetReferralInternalMessageError;
            return response;
        }
        public ServiceResponse ExistanceOfReferralTimeslot(ReferralTimeSlotMaster model)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> paramList = new List<SearchValueData>();
            paramList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
            paramList.Add(new SearchValueData { Name = "ReferralBillingAuthorizationID", Value = Convert.ToString(model.ReferralBillingAuthorizationID) });

            TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.ExistanceOfRefferralTimeslot, paramList);
            if (result == null)
            {
                response.Data = 0;
            }
            else
            {

                //response.Message = model.ReferralTimeSlotMasterID > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.DateRange) :
                //        string.Format(Resource.RecordCreatedSuccessfully, Resource.DateRange);
                response.IsSuccess = true;
                response.Data = result.TablePrimaryId > 0 ? result.TablePrimaryId : model.ReferralTimeSlotMasterID;

            }
            return response;
        }
        public ServiceResponse PrioAuthorization(long ReferralID, long BillingAuthorizationID)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                //Search List
                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(ReferralID) });
                searchList.Add(new SearchValueData { Name = "BillingAuthorizationID", Value = Convert.ToString(BillingAuthorizationID) });
                PriorAuthorizationModel totalData = GetEntity<PriorAuthorizationModel>(StoredProcedure.API_UniversalPriorAuthorization, searchList);
                response.Data = totalData;
                response.IsSuccess = true;
                //return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "";

            }
            return response;
        }

        public ServiceResponse GetMasterJurisdictionList(string claimProcessor)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                string tableName = GetTableName<MasterJurisdiction>();
                List<NameValueDataInString> data = GetEntityList<NameValueDataInString>($"SELECT [{nameof(MasterJurisdiction.Description)}] [Name], [{nameof(MasterJurisdiction.MasterJurisdictionID)}] [Value] FROM [dbo].[{tableName}] WHERE [{nameof(MasterJurisdiction.CompanyName)}] = '{claimProcessor}'");
                response.Data = data;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                //response.Message = "";

            }
            return response;
        }

        public ServiceResponse GetMasterTimezoneList(string claimProcessor)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                string tableName = GetTableName<MasterTimezone>();
                List<NameValueDataInString> data = GetEntityList<NameValueDataInString>($"SELECT [{nameof(MasterTimezone.Code)}] [Name], [{nameof(MasterTimezone.MasterTimezoneID)}] [Value] FROM [dbo].[{tableName}] WHERE [{nameof(MasterTimezone.CompanyName)}] = '{claimProcessor}'");
                response.Data = data;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                //response.Message = "";

            }
            return response;
        }
        public bool IsDXCodeExist(string referralID, string PayorID)
        {
            List<SearchValueData> searchModel = new List<SearchValueData>
                {
                    new SearchValueData { Name = "referralID", Value =  referralID, DataType="int" },
                    new SearchValueData { Name = "PayorID", Value =  PayorID, DataType="int" },
                   };

            if ((int)GetScalar(StoredProcedure.IsDXCodeExistByReferralID, searchModel) != 0)
            {
                return true;
            }
            return false;
        }
        public ServiceResponse GetReferralSourcesDD(string ItemType, int Isdeleted)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ItemType", Value = Convert.ToString(ItemType) });
            searchList.Add(new SearchValueData { Name = "Isdeleted", Value = Convert.ToString(Isdeleted) });
            List<ReferralSourcesDDModel> totalData = GetEntityList<ReferralSourcesDDModel>(StoredProcedure.GetReferralSourcesDD, searchList);
            response.IsSuccess = true;
            response.Data = totalData;
            return response;
        }
        public ServiceResponse SaveReferralSourcesDD(ReferralSources model, long LoggedInID)
        {
            var response = new ServiceResponse();
            try
            {
                ReferralSourcesDDModel data = GetEntity<ReferralSourcesDDModel>(StoredProcedure.SaveReferralSourcesDD, new List<SearchValueData>
                {
                    new SearchValueData {Name = "Value",Value = Convert.ToString(model.Value)},
                    new SearchValueData {Name = "Name",Value = Convert.ToString(model.Name)},
                    new SearchValueData {Name = "ItemType",Value = Convert.ToString(model.ItemType)},
                     new SearchValueData {Name = "UpdatedBy",Value = Convert.ToString(LoggedInID)}
                   
                });
                if (data.Result == -1)
                {
                    response.IsSuccess = false;
                    response.Message = "Record already Exist";
                }
                else
                {
                    response.IsSuccess = true;
                    response.Data = data;
                    response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.EmployeeVisit);
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }
        public ServiceResponse DeleteReferralSourcesDD(long id,long IsDeleted,string ItemType)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "id", Value = Convert.ToString(id) });
                searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(IsDeleted) });
                searchList.Add(new SearchValueData { Name = "ItemType", Value = Convert.ToString(ItemType) });
                GetScalar(StoredProcedure.DeleteReferralSourcesDD, searchList);
                if (IsDeleted == 0)
                {
                    response.Message = string.Format(Resource.DeletedSuccessfully, Resource.ReferralSource);
                }
                else
                {
                    response.Message = string.Format("Active Successfully", Resource.ReferralSource);
                }
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }
    }
}

