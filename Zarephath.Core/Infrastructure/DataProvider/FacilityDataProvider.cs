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

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class FacilityDataProvider : BaseDataProvider, IFacilityDataProvider
    {
        CacheHelper _cacheHelper = new CacheHelper();

        #region ZarePath Data Provider Code


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

           
            #endregion


            response.Data = referralModel.AddReferralModel;
            response.IsSuccess = true;
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
                if (addReferralModel.Referral.RegionID == 0)
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

            searchList.Add(new SearchValueData { Name = "ServicetypeId", Value = Convert.ToString(searchReferralModel.ServiceTypeID) });
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

                #region Add Region(Location)
                if (addReferralModel.Referral.RegionID == 0)
                {
                    List<SearchValueData> param = new List<SearchValueData>
                    {
                        new SearchValueData {Name = "RegionName", Value = addReferralModel.Referral.RegionName}
                    };
                    long RegionID = (long)GetScalar(StoredProcedure.AddRegion, param);
                    addReferralModel.Referral.RegionID = RegionID;
                }
                #endregion

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
                if (addReferralModel.Referral.RegionID == 0)
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

        private static void SetSearchFilterForEtsMasterListPage(SearchRTSMaster searchETSMaster, List<SearchValueData> searchList)
        {
            searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(searchETSMaster.ReferralID) });
            if (searchETSMaster.StartDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(searchETSMaster.StartDate).ToString(Constants.DbDateFormat) });
            if (searchETSMaster.EndDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(searchETSMaster.EndDate).ToString(Constants.DbDateFormat) });
            //if (searchETSMaster.Filter.HasValue)
            searchList.Add(new SearchValueData { Name = "Filter", Value = Convert.ToString(searchETSMaster.Filter) });
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

            List<ListRTSMaster> totalData = GetEntityList<ListRTSMaster>(StoredProcedure.DeleteRtsMaster, searchList);

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

            AddRTSDetailCaseManagement(model.ReferralTimeSlotMasterID > 0 ? model.ReferralTimeSlotMasterID : result.TablePrimaryId, model, loggedInUserID);

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
            //paramList.Add(new SearchValueData { Name = "Day", Value = Convert.ToString(model.Day) });
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
            if (model.EndDate!=null)
            {
                paramList.Add(new SearchValueData { Name = "SlotEndDate", Value = model.EndDate.ToString(Constants.DbDateFormat) });
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
       


        //public ServiceResponse SaveAndGetFacilityDetails(FacilityDetailModel objFacility)
        //{
        //    ServiceResponse response = new ServiceResponse();
        //    if (objFacility.ReferralId != null)
        //    {
        //        objFacility.ReferralId = Crypto.Decrypt(objFacility.ReferralId);
        //        var searchlist = new List<SearchValueData>
        //        {
        //            new SearchValueData {Name = "ReferralID", Value = Convert.ToString(objFacility.ReferralId)},
        //            new SearchValueData {Name = "TIN", Value = Convert.ToString(objFacility.TIN_Number)},
        //            new SearchValueData {Name = "EIN", Value = Convert.ToString(objFacility.EIN_Number)},
        //            new SearchValueData {Name = "Type", Value = Convert.ToString(objFacility.Type)},
        //        };
        //        FacilityDetailModel totalData = GetEntity<FacilityDetailModel>(StoredProcedure.SaveAndGetFacilityDetails, searchlist);
        //        response.IsSuccess = true;
        //        response.Data = totalData;
        //    }
        //    else
        //        response.Message = Resource.GetReferralInternalMessageError;
        //    return response;
        //}
        public ServiceResponse GetEmployeeGroup(string GroupID)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                var searchlist = new List<SearchValueData>
                {
                     new SearchValueData {Name = "GroupID", Value = Convert.ToString(GroupID)},
                };
                //List<EmployeeGroupList> totalData = GetEntityList<EmployeeGroupList>(StoredProcedure.GetEmployeeGroup, searchlist);
                SetContactListPage totalData = GetMultipleEntity<SetContactListPage>(StoredProcedure.GetEmployeeGroup, searchlist);
                response.IsSuccess = true;
                response.Data = totalData;
            }
            catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Resource.GetReferralInternalMessageError;
            }
            

           // response.Message = Resource.GetReferralInternalMessageError;
            return response;
        }
        public ServiceResponse GetEmployeeByGroupId(string groupIds)
        {
            var response = new ServiceResponse();
            var result = GetEntityList<Employee>(StoredProcedure.GetEmployeeByGroupId, new List<SearchValueData>
            {
                new SearchValueData("GroupId",Convert.ToString(groupIds))
            });

            if (result.Count > 0)
            {
                response.IsSuccess = true;
                response.Data = result;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = Resource.ErrorOccured;
            }
            return response;
        }
        public ServiceResponse UpdateEmployeeGroupId(long GroupID, string EmployeeIDs,bool IsChecked)
        {
            var response = new ServiceResponse();
            try
            {
                //TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.UpdateEmployeeGroupId, new List<SearchValueData>
                List<EmployeeListAssignedGroup> result = GetEntityList<EmployeeListAssignedGroup>(StoredProcedure.UpdateEmployeeGroupId, new List<SearchValueData>
            {
                new SearchValueData("GroupId",Convert.ToString(GroupID)),
                new SearchValueData("EmployeeIds",Convert.ToString(EmployeeIDs)),
                new SearchValueData("IsChecked",Convert.ToString(IsChecked))
            });
                response.Data = result;
                response.IsSuccess = true;
            }
            catch(Exception ex)
            {
                response.IsSuccess = false;
            }
           
            return response;
        }
        public ServiceResponse SaveEmpGroup(string GroupName,long ReferralID,bool IsEditMode)
        {
            ServiceResponse response = new ServiceResponse();
            if (GroupName !=null)
            {
                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralID", Value = Convert.ToString(ReferralID)},
                    new SearchValueData {Name = "GroupName", Value = Convert.ToString(GroupName)},
                    new SearchValueData {Name = "IsEditMode", Value = Convert.ToString(IsEditMode)},
                };
                 EmployeeGroupList totalData = GetEntity<EmployeeGroupList>(StoredProcedure.CreateEmpGroup, searchlist);
                response.IsSuccess = true;
                response.Data = totalData;
            }
            else
                response.Message = Resource.GetReferralInternalMessageError;
            return response;
        }
        public ServiceResponse RemoveAllAssignedGroup(long GroupID, string EmployeeID)
        {
            var response = new ServiceResponse();
            try
            {
                // TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.RemoveAllAssignedGroup, new List<SearchValueData>
                List<EmployeeListAssignedGroup> result = GetEntityList<EmployeeListAssignedGroup>(StoredProcedure.RemoveAllAssignedGroup, new List<SearchValueData>
            {
                new SearchValueData("GroupId",Convert.ToString(GroupID)),
               // new SearchValueData("GroupId",Convert.ToString(GroupID)),
                new SearchValueData("EmployeeIds",Convert.ToString(EmployeeID))
            });
                response.IsSuccess = true;
                response.Data = result;
            }
            catch(Exception ex)
            {
                response.IsSuccess = false;

            }
            return response;
        }

        #endregion
    }
}

