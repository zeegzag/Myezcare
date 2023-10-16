using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Web;
using System.Web.Configuration;
using PetaPoco;
using Zarephath.Core.Helpers;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text.RegularExpressions;
using Zarephath.Core.Infrastructure.Utility.Fcm;
using System.Text;
using System.Web.Hosting;
using Zarephath.Core.Models.Scheduler;
using ScheduleWidget.ScheduledEvents;
using System.Data;
using static Zarephath.Core.Models.Scheduler.ScheduleDTO;
using ExportToExcel;
using System.Data.SqlClient;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class ScheduleDataProvider : BaseDataProvider, IScheduleDataProvider
    {

        #region Schedule Assignment

        public ServiceResponse SetScheduleAssignmentModel()
        {
            var response = new ServiceResponse();

            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData
                        {
                            Name = "IgnoreFrequency",
                            Value = Convert.ToString((int) FrequencyCode.FrequencyCodes.DNS)
                        }
                };
            ScheduleAssignmentModel scheduleAssignmentModel = GetMultipleEntity<ScheduleAssignmentModel>(StoredProcedure.SetScheduleAssignmentModel, searchParam);
            scheduleAssignmentModel.CancellationReasons = Common.SetCancellationReasons();

            if (scheduleAssignmentModel.RegionList.Any())
                scheduleAssignmentModel.ScheduleSearchModel.RegionID = scheduleAssignmentModel.RegionList[0].RegionID;


            searchParam = new List<SearchValueData>();
            searchParam.Add(new SearchValueData() { Name = "IsDeleted", Value = "0" });
            scheduleAssignmentModel.EmployeeList = GetEntityList<Employee>(searchParam);
            scheduleAssignmentModel.ScheduleSearchModel.StartDate = DateTime.Today;
            scheduleAssignmentModel.ScheduleSearchModel.EndDate = DateTime.Today.AddDays(3);

            response.IsSuccess = true;
            response.Data = scheduleAssignmentModel;
            return response;
        }

        public ServiceResponse GetReferralListForSchedule(SearchReferralListForSchedule searchReferralModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchReferralModel != null)
                SetSearchFilterForReferralList(searchReferralModel, searchList, loggedInId);

            Page<ReferralListForSchedule> page = GetEntityPageList<ReferralListForSchedule>(StoredProcedure.GetReferralListForScheduling, searchList, pageSize, pageIndex, sortIndex, sortDirection);
            response.Data = page;
            response.IsSuccess = true;
            return response;
        }

        private static void SetSearchFilterForReferralList(SearchReferralListForSchedule searchReferralModel, List<SearchValueData> searchList, long loggedInId)
        {
            if (!string.IsNullOrEmpty(searchReferralModel.ContactName))
                searchList.Add(new SearchValueData { Name = "ContactName", Value = Convert.ToString(searchReferralModel.ContactName) });

            if (!string.IsNullOrEmpty(searchReferralModel.Name))
                searchList.Add(new SearchValueData { Name = "Name", Value = Convert.ToString(searchReferralModel.Name) });

            searchList.Add(new SearchValueData { Name = "MaxAge", Value = Convert.ToString(searchReferralModel.MaxAge) });
            searchList.Add(new SearchValueData { Name = "LastAttFromDate", Value = Convert.ToString(searchReferralModel.LastAttFromDate) });
            searchList.Add(new SearchValueData { Name = "LastAttToDate", Value = Convert.ToString(searchReferralModel.LastAttToDate) });
            searchList.Add(new SearchValueData { Name = "MinAge", Value = Convert.ToString(searchReferralModel.MinAge) });
            searchList.Add(new SearchValueData { Name = "ContactTypeID", Value = ((int)Common.ContactTypes.PrimaryPlacement).ToString() });

            if (searchReferralModel.RegioinID > 0)
                searchList.Add(new SearchValueData
                {
                    Name = "RegioinID",
                    Value = Convert.ToString(searchReferralModel.RegioinID)
                });

            if (searchReferralModel.FrequencyCodeID > 0)
                searchList.Add(new SearchValueData
                {
                    Name = "FrequencyCodeID",
                    Value = Convert.ToString(searchReferralModel.FrequencyCodeID)
                });

            if (searchReferralModel.Gender > 0)
            {
                searchList.Add(new SearchValueData
                {
                    Name = "Gender",
                    Value = Convert.ToString(searchReferralModel.Gender)
                });
            }

            if (searchReferralModel.ServiceID >= 0)
                searchList.Add(new SearchValueData
                {
                    Name = "ServiceID",
                    Value = Convert.ToString(searchReferralModel.ServiceID)
                });

            if (searchReferralModel.PayorID > 0)
            {
                searchList.Add(new SearchValueData
                {
                    Name = "PayorID",
                    Value = Convert.ToString(searchReferralModel.PayorID)
                });
            }


            searchList.Add(new SearchValueData
            {
                Name = "IgnoreFrequency",
                Value = Convert.ToString((int)FrequencyCode.FrequencyCodes.DNS)
            });

        }

        public ServiceResponse GetReferralDetailForPopup(long referralID)
        {
            var response = new ServiceResponse();
            ReferralDetailForPopup detailModel = GetMultipleEntity<ReferralDetailForPopup>(StoredProcedure.GetReferralDetailForPopup,
                new List<SearchValueData>{
                        new SearchValueData{Name = "ReferralID",Value = referralID.ToString()},
                        new SearchValueData { Name = "ContactTypeID", Value = ((int)Common.ContactTypes.PrimaryPlacement).ToString() }
                    });

            response.Data = detailModel;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse GetScheduleListByFacility(SearchScheduleListByFacility searchPara)
        {
            var response = new ServiceResponse();
            //Code For Getting Schedule List..
            FacilityScheduleDetails detailModel =
                GetMultipleEntity<FacilityScheduleDetails>(StoredProcedure.GetFacilityScheduleDetails, new List<SearchValueData>
                    {
                        new SearchValueData
                            {
                                Name = "FacilityID",
                                Value = searchPara.FacilityID.ToString()
                            },
                        new SearchValueData
                            {
                                Name = "WeekMasterID",
                                Value = searchPara.WeekMasterID.ToString()
                            }
                        //new SearchValueData
                        //    {
                        //        Name = "StartDate",
                        //        Value = searchPara.StartDate.ToString(Constants.DbDateFormat)
                        //    },
                        //new SearchValueData
                        //    {
                        //        Name = "EndDate",
                        //        Value = searchPara.EndDate.ToString(Constants.DbDateFormat)
                        //    }
                    });

            response.Data = detailModel;
            response.IsSuccess = true;
            return response;
        }

        public List<Facility> GetFacilutyListForAutoComplete(string searchText, int pageSize, long regionID)
        {
            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData{Name = "FacilityName",Value = searchText},
                    new SearchValueData{Name = "IsDeleted",Value = "0",IsEqual = true}
                };
            if (regionID > 0)
            {
                searchParam.Add(new SearchValueData { Name = "RegionID", Value = regionID.ToString(), IsEqual = true });
            }
            Page<Facility> page = GetEntityPageList<Facility>(searchParam, pageSize, 1, "FacilityName", "ASC");

            return page.Items;
        }

        public ServiceResponse LoadAllFacilityByRegion(long? regionID)
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData{Name = "IsDeleted",Value = "0",IsEqual = true}
                };
            if (regionID != null && regionID > 0)
            {
                searchParam.Add(new SearchValueData { Name = "RegionID", Value = regionID.ToString(), IsEqual = true });
            }
            List<Facility> page = GetEntityList<Facility>(searchParam);


            response.IsSuccess = true;
            response.Data = page;
            return response;
        }

        public ServiceResponse SaveScheduleMasterFromCalender(ScheduleAssignmentModel scheduleAssignment, long loggedInUserID)
        {
            ScheduleMaster scheduleMaster;
            if (scheduleAssignment.ScheduleMaster.ScheduleID == 0)
            {

                Referral referral = GetEntity<Referral>(scheduleAssignment.ScheduleMaster.ReferralID);

                if (referral == null || referral.ReferralID == 0)
                {
                    return new ServiceResponse
                    {
                        IsSuccess = false,
                        Message = Resource.ErrorOccured
                    };
                }
                scheduleAssignment.ScheduleMaster.DropOffLocation = referral.DropOffLocation.Value;
                scheduleAssignment.ScheduleMaster.PickUpLocation = referral.PickUpLocation.Value;
                scheduleAssignment.ScheduleMaster.ScheduleStatusID = (int)ScheduleStatus.ScheduleStatuses.Unconfirmed;

            }


            if (scheduleAssignment.ScheduleMaster.FacilityID.HasValue)
            {
                Facility facility = GetEntity<Facility>(scheduleAssignment.ScheduleMaster.FacilityID.Value);
                if (facility != null && facility.FacilityID != 0 && facility.DefaultScheduleStatusID.HasValue)
                {
                    scheduleAssignment.ScheduleMaster.ScheduleStatusID = facility.DefaultScheduleStatusID.Value;
                }
            }



            ServiceResponse response = SaveScheduleMaster(scheduleAssignment.ScheduleMaster, loggedInUserID);


            return response;
        }

        public ServiceResponse SaveScheduleMaster(ScheduleMaster scheduleMaster, long loggedInUserID)
        {
            var response = new ServiceResponse();

            bool isEditing = scheduleMaster.ScheduleID > 0;

            // Code Validate Schedule Conflict And Other Valication
            ServiceResponse validateResponse = ValidateSchedule(scheduleMaster, loggedInUserID);
            if (!validateResponse.IsSuccess)
            {
                return validateResponse;
            }
            ValidateScheduleMasterModel validateScheduleModel = (ValidateScheduleMasterModel)validateResponse.Data;
            // Code Validate Schedule Conflict And Other Validation


            SaveObject(scheduleMaster, loggedInUserID);

            //SET LAST ATTENDANCE AS LAST CONFIRMED STATUS DATE
            GetScalar(StoredProcedure.UpdateReferralLastAttDate,
            new List<SearchValueData> { new SearchValueData { Name = "ReferralID", Value = scheduleMaster.ReferralID.ToString() },
                        new SearchValueData { Name = "ScheduleStatusID", Value = ((int)ScheduleStatus.ScheduleStatuses.Confirmed).ToString() } });


            response.Data = scheduleMaster;

            if (scheduleMaster.ScheduleStatusID == ((int)ScheduleStatus.ScheduleStatuses.Confirmed)
                      && validateScheduleModel.OutOfBadCapacityCount > 0)
            {
                //If Current Schedule is Confirmaed And Facility Capacity is Full Show Warning Messages.
                response.Message = Resource.FacilityOutOfCapacity;
                response.ErrorCode = Constants.ErrorCode_Warning;
                response.IsSuccess = true;
                return response;
            }
            if (validateScheduleModel.OutOfRoomCapacityCount > 0 && scheduleMaster.ScheduleStatusID == ((int)ScheduleStatus.ScheduleStatuses.Confirmed))
            {
                //In Case Of Room capacity is full Show Warning Message.
                response.ErrorCode = Constants.ErrorCode_Warning;
                response.Message = Resource.ScheduledSavedSuccessfullyWithWarning;
            }
            else
            {
                response.Message = isEditing ? Resource.ScheduledUpdatedSuccessfully : Resource.ScheduledAddedSuccessfully;
            }

            response.IsSuccess = true;

            return response;
        }

        private ServiceResponse ValidateSchedule(ScheduleMaster scheduleMaster, long loggedInUserID)
        {
            var response = new ServiceResponse();
            if (scheduleMaster.ScheduleID > 0)
            {
                ScheduleMaster oldschedule = GetEntity<ScheduleMaster>(scheduleMaster.ScheduleID);

                if ((oldschedule.StartDate != scheduleMaster.StartDate || oldschedule.EndDate != scheduleMaster.EndDate) &&
                    (oldschedule.IsAssignedToTransportationGroupUp || oldschedule.IsAssignedToTransportationGroupDown))
                {

                    //In Case of Edit Schedule If Client is already assigned for transportation for that schedule then.
                    // If Client Select to Remove transportation assignment then ststem will remove transportation assignment.
                    #region RemoveTransportation Assignment
                    if (scheduleMaster.TransportationAssignmentAction == Constants.TransportationAssignmentRemoveAction)
                    {
                        ITransportationGroupDataProvider transportationGroupData = new TransportationGroupDataProvider();
                        TransportationGroupClient transportationGroupClient = null;

                        List<SearchValueData> searchValue = new List<SearchValueData>
                                                        {
                                                            new SearchValueData
                                                                {
                                                                    Name = "ScheduleID",
                                                                    Value = scheduleMaster.ScheduleID.ToString()
                                                                }
                                                        };

                        if (oldschedule.StartDate != scheduleMaster.StartDate)
                        {
                            searchValue.Add(new SearchValueData
                            {
                                Name = "TripDirection",
                                Value = TransportationGroup.TripDirectionUp
                            });
                            scheduleMaster.IsAssignedToTransportationGroupUp = false;
                            scheduleMaster.IsAssignedToTransportationGroupDown = false;

                        }
                        else if (oldschedule.EndDate != scheduleMaster.EndDate)
                        {
                            searchValue.Add(new SearchValueData
                            {
                                Name = "TripDirection",
                                Value = TransportationGroup.TripDirectionDown
                            });
                            scheduleMaster.IsAssignedToTransportationGroupDown = false;
                        }

                        transportationGroupClient = GetEntity<TransportationGroupClient>(StoredProcedure.GetTransportationGroupClientByScheduleID, searchValue);
                        if (transportationGroupClient != null)
                        {
                            transportationGroupData.RemoveTransportationGroupClient(
                                transportationGroupClient.TransportationGroupClientID, loggedInUserID);


                        }

                    }
                    else if (scheduleMaster.TransportationAssignmentAction == Constants.TransportationAssignmentKeepAction)
                    {


                    }
                    else
                    {
                        //As Per Client Request System will not restrict for trmoving transportation group on schedule date change 
                        response.Message = Resource.Youcannotchangescheduledate;
                        response.ErrorCode = Constants.ErrorCode_Warning;
                        response.Data = oldschedule;
                        response.IsSuccess = false;
                        return response;
                    }
                    #endregion RemoveTransportation Assignment


                    //As Per Client Request System will not restrict for trmoving transportation group on schedule date change 
                    //response.Message = Resource.Youcannotchangescheduledate;
                    //response.IsSuccess = false;
                    //return response;
                }
            }
            ValidateScheduleMasterModel scheduleCount = GetEntity<ValidateScheduleMasterModel>(StoredProcedure.CheckScheduleConflict, new List<SearchValueData>
                {
                    new SearchValueData
                        {
                            Name = "ScheduleID",
                            Value = scheduleMaster.ScheduleID.ToString()
                        },
                    new SearchValueData
                        {
                            Name = "ReferralID",
                            Value = scheduleMaster.ReferralID.ToString()
                        },
                    new SearchValueData
                        {
                            Name    = "FacilityID",
                            Value = scheduleMaster.FacilityID.ToString()
                        },
                     new SearchValueData
                        {
                            Name = "StartDate",
                            Value = scheduleMaster.StartDate.ToString(Constants.DbDateFormat)
                        },
                        new SearchValueData
                        {
                            Name = "EndDate",
                            Value = scheduleMaster.EndDate.ToString(Constants.DbDateFormat)
                        },

                });
            if (scheduleCount.ScheduleConflictCount > 0)
            {
                response.IsSuccess = false;
                response.Message = Resource.ClientAlreadyScheduled;
                return response;
            }

            //if (scheduleMaster.ScheduleStatusID == ((int)ScheduleStatus.ScheduleStatuses.Confirmed)
            //           && scheduleCount.OutOfBadCapacityCount > 0)
            //{
            //    //If Current Schedule is Confirmaed And Facility Capacity is Full Show Error Messages.
            //    response.IsSuccess = false;
            //    response.Message = Resource.FacilityOutOfCapacity;
            //    return response;
            //}

            //if (scheduleCount.OutOfRoomCapacityCount>0)
            //{
            //    response.IsSuccess = true;
            //    response.Message = Resource.FacilityOutOfCapacity;
            //    return response;
            //}

            response.IsSuccess = true;
            response.Data = scheduleCount;
            return response;
        }

        public ServiceResponse CreateWeek(WeekMaster model, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();
            SaveObject(model, loggedInUserID);
            response.IsSuccess = true;
            response.Data = GetEntityList<WeekMaster>(null, "", "StartDate", "Desc");
            response.Message = Resource.NewWeekAdded;
            return response;
        }

        public ServiceResponse RemoveSchedulesFromWeekFacility(long weekMasterID, long? facilityID, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();

            var search = new List<SearchValueData>
                {
                    new SearchValueData
                        {
                            Name = "WeekMasterID",
                            Value = weekMasterID.ToString()
                        }
                };
            if (facilityID != null)
            {
                search.Add(new SearchValueData
                {
                    Name = "FacilityID",
                    Value = facilityID.Value.ToString()
                });
            }
            List<ScheduleMaster> schedules = GetEntityList<ScheduleMaster>(search);
            if (schedules.Any())
            {
                response = DeleteScheduleMaster(new SearchScheduleMasterModel
                {
                    ListOfIdsInCsv = string.Join(",", schedules.Select(m => m.ScheduleID).ToList()),
                    IsShowListing = false

                }, 0, 1, "", "", SessionHelper.LoggedInID, "");
            }
            else
            {
                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordDeletedSuccessfully, Resource.ScheduleBatchService);
            }

            return response;
        }

        #endregion

        #region Schedule Master

        public ServiceResponse SetScheduleMasterPage()
        {
            var response = new ServiceResponse();

            ScheduleMasterModel setScheduleMasterModel = GetMultipleEntity<ScheduleMasterModel>(StoredProcedure.SetScheduleMasterPage, new List<SearchValueData>());
            setScheduleMasterModel.CancellationReasons = Common.SetCancellationReasons();

            setScheduleMasterModel.AddScheduleBatchService = new ScheduleBatchService
            {
                ScheduleBatchServiceStatus = ScheduleBatchService.ScheduleBatchServiceStatuses.Initiated.ToString()
            };
            setScheduleMasterModel.ScheduleBatchServiceTypeList = Enum.GetNames(typeof(ScheduleBatchService.ScheduleBatchServiceTypes)).Select(m => new NameValueDataInString()
            {
                Name = m,
                Value = m
            }).ToList();

            setScheduleMasterModel.PreferredCommunicationMethod = Common.SetPreferredCommunicationMethod();
            response.IsSuccess = true;
            response.Data = setScheduleMasterModel;
            return response;
        }

        public ServiceResponse GetScheduleMasterList(SearchScheduleMasterModel searchScheduleMasterModel, int pageIndex, int pageSize, string sortIndex, string sortDirection,
                                                     long loggedInId, string sortIndexArray = "")
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchScheduleMasterModel != null)
                SetSearchFilterForScheduleMasterList(searchScheduleMasterModel, searchList, loggedInId);

            Page<ScheduleMasterList> page = GetEntityPageList<ScheduleMasterList>(StoredProcedure.GetScheduleMaster, searchList, pageSize, pageIndex, sortIndex, sortDirection, sortIndexArray);
            response.Data = page;
            response.IsSuccess = true;
            return response;
        }

        private static void SetSearchFilterForScheduleMasterList(SearchScheduleMasterModel searchScheduleMasterModel, List<SearchValueData> searchList, long loggedInId)
        {
            if (searchScheduleMasterModel.StartDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = searchScheduleMasterModel.StartDate.Value.ToString(Constants.DbDateFormat) });

            if (searchScheduleMasterModel.EndDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = searchScheduleMasterModel.EndDate.Value.ToString(Constants.DbDateFormat) });


            if (searchScheduleMasterModel.EndDate == null && searchScheduleMasterModel.StartDate == null && !searchScheduleMasterModel.IsPartial)
            {
                searchList.Add(new SearchValueData { Name = "StartDate", Value = DateTime.UtcNow.Date.ToString(Constants.DbDateFormat) });
            }

            if (searchScheduleMasterModel.ScheduleStatusID > 0)
                searchList.Add(new SearchValueData
                {
                    Name = "ScheduleStatusID",
                    Value = Convert.ToString(searchScheduleMasterModel.ScheduleStatusID)
                });

            if (searchScheduleMasterModel.ScheduleID > 0)
                searchList.Add(new SearchValueData
                {
                    Name = "ScheduleID",
                    Value = Convert.ToString(searchScheduleMasterModel.ScheduleID)
                });

            if (!string.IsNullOrEmpty(searchScheduleMasterModel.Name))
                searchList.Add(new SearchValueData
                {
                    Name = "Name",
                    Value = searchScheduleMasterModel.Name
                });

            if (!string.IsNullOrEmpty(searchScheduleMasterModel.ParentName))
                searchList.Add(new SearchValueData
                {
                    Name = "ParentName",
                    Value = searchScheduleMasterModel.ParentName
                });

            if (searchScheduleMasterModel.DropOffLocation > 0)
                searchList.Add(new SearchValueData
                {
                    Name = "DropOffLocation",
                    Value = Convert.ToString(searchScheduleMasterModel.DropOffLocation)
                });

            if (searchScheduleMasterModel.FacilityID > 0)
                searchList.Add(new SearchValueData
                {
                    Name = "FacilityID",
                    Value = Convert.ToString(searchScheduleMasterModel.FacilityID)
                });

            if (searchScheduleMasterModel.RegionID > 0)
                searchList.Add(new SearchValueData
                {
                    Name = "RegionID",
                    Value = Convert.ToString(searchScheduleMasterModel.RegionID)
                });

            if (searchScheduleMasterModel.LanguageID > 0)
                searchList.Add(new SearchValueData
                {
                    Name = "LanguageID",
                    Value = Convert.ToString(searchScheduleMasterModel.LanguageID)
                });

            if (!string.IsNullOrEmpty(searchScheduleMasterModel.ReferralID))
            {
                searchList.Add(new SearchValueData
                {
                    Name = "ReferralID",
                    Value = Convert.ToString(Crypto.Decrypt(searchScheduleMasterModel.ReferralID))
                });
            }

            if (searchScheduleMasterModel.PreferredCommunicationMethodID > 0)
                searchList.Add(new SearchValueData
                {
                    Name = "PreferredCommunicationMethodID",
                    Value = Convert.ToString(searchScheduleMasterModel.PreferredCommunicationMethodID)
                });

        }

        public ServiceResponse DeleteScheduleMaster(SearchScheduleMasterModel searchScheduleMasterModel, int pageIndex, int pageSize, string sortIndex, string sortDirection,
                                                    long loggedInId, string sortIndexArray = "")
        {
            ServiceResponse response = new ServiceResponse();

            //Check for When IsAssignTransportationGroupDown and UP is true that time cannot Delete record

            string[] idlist = searchScheduleMasterModel.ListOfIdsInCsv.Split(',');

            foreach (var schdeuleid in idlist)
            {
                ScheduleMaster getConfirmList = GetEntity<ScheduleMaster>(Convert.ToInt64(schdeuleid));
                if (getConfirmList.IsAssignedToTransportationGroupDown || getConfirmList.IsAssignedToTransportationGroupUp)
                {
                    response.Message = Resource.Youcannotdeletethisasclient;
                    return response;
                }
            }

            List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);
            SetSearchFilterForScheduleMasterList(searchScheduleMasterModel, searchList, loggedInId);

            if (!string.IsNullOrEmpty(searchScheduleMasterModel.ListOfIdsInCsv))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchScheduleMasterModel.ListOfIdsInCsv });

            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(searchScheduleMasterModel.IsShowListing) });
            searchList.Add(new SearchValueData { Name = "ConfirmationScheduleStatusID", Value = Convert.ToString((int)ScheduleStatus.ScheduleStatuses.Confirmed) });

            List<ScheduleMasterList> totalData = GetEntityList<ScheduleMasterList>(StoredProcedure.DeleteSchedule, searchList);



            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ScheduleMasterList> list = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = list;
            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordDeletedSuccessfully, Resource.Schedule);
            return response;
        }

        public ServiceResponse UpdateScheduleFromScheduleList(SearchScheduleMasterModel searchScheduleMasterModel, ScheduleMaster scheduleMaster, int pageIndex, int pageSize, string sortIndex,
                                            string sortDirection, long loggedInUserID, string sortIndexArray)
        {
            ServiceResponse response = new ServiceResponse();
            if (scheduleMaster.ScheduleID > 0)
            {
                ScheduleMaster tempScheduleMaster = GetEntity<ScheduleMaster>(scheduleMaster.ScheduleID);
                if (tempScheduleMaster != null)
                {
                    //As per Client Request removed this check.
                    //if (scheduleMaster.ScheduleStatusID != tempScheduleMaster.ScheduleStatusID && tempScheduleMaster.ScheduleStatusID == (int)ScheduleStatus.ScheduleStatuses.Confirmed && (tempScheduleMaster.IsAssignedToTransportationGroupDown || tempScheduleMaster.IsAssignedToTransportationGroupUp))
                    //{
                    //    response.Message = Resource.Youcannotchangethestatusasclient;
                    //    return response;
                    //}


                    //If Status Is No Show,No Confirmation and Cancel and it is assigned to transportaion group then remove from assignment.
                    //And Also Make FacilityID to Null
                    if (scheduleMaster.ScheduleStatusID != tempScheduleMaster.ScheduleStatusID && (scheduleMaster.ScheduleStatusID == (int)ScheduleStatus.ScheduleStatuses.No_Show || scheduleMaster.ScheduleStatusID == (int)ScheduleStatus.ScheduleStatuses.No_Confirmation || scheduleMaster.ScheduleStatusID == (int)ScheduleStatus.ScheduleStatuses.Cancelled))
                    {

                        //Get Transportationgroup List for this schedule

                        List<TransportationGroupClient> assignedGroup =
                            GetEntityList<TransportationGroupClient>(new List<SearchValueData>
                                {
                                    new SearchValueData
                                        {
                                            Name = "ScheduleID",
                                            Value = scheduleMaster.ScheduleID.ToString()
                                        }
                                });

                        ITransportationGroupDataProvider transportationGroupDataProvider =
                            new TransportationGroupDataProvider();

                        foreach (TransportationGroupClient transportationGroupClient in assignedGroup)
                        {
                            transportationGroupDataProvider.RemoveTransportationGroupClient(
                                transportationGroupClient.TransportationGroupClientID, loggedInUserID);
                        }


                        //Setting schedulemaster to null.
                        scheduleMaster.FacilityID = null;
                        scheduleMaster.EmployeeID = null;

                        //scheduleMaster.EmployeeID = null;
                        //response.Message = Resource.Youcannotchangethestatusasclient;
                        //return response;

                        tempScheduleMaster.IsAssignedToTransportationGroupDown = false;
                        tempScheduleMaster.IsAssignedToTransportationGroupUp = false;
                    }



                    tempScheduleMaster.ScheduleStatusID = scheduleMaster.ScheduleStatusID;
                    tempScheduleMaster.PickUpLocation = scheduleMaster.PickUpLocation;
                    tempScheduleMaster.DropOffLocation = scheduleMaster.DropOffLocation;
                    tempScheduleMaster.Comments = scheduleMaster.Comments;
                    tempScheduleMaster.FacilityID = scheduleMaster.FacilityID;
                    tempScheduleMaster.EmployeeID = scheduleMaster.EmployeeID;

                    //If Status is not Cancelled then reset value.
                    if (scheduleMaster.ScheduleStatusID != (int)ScheduleStatus.ScheduleStatuses.Cancelled)
                    {
                        tempScheduleMaster.WhoCancelled = null;
                        tempScheduleMaster.WhenCancelled = null;
                        tempScheduleMaster.CancelReason = null;
                    }
                    else //If Status is Cancelled then Set values.
                    {
                        tempScheduleMaster.Comments = scheduleMaster.CancelReason;
                        tempScheduleMaster.WhoCancelled = scheduleMaster.WhoCancelled;
                        tempScheduleMaster.WhenCancelled = scheduleMaster.WhenCancelled;
                        tempScheduleMaster.CancelReason = scheduleMaster.CancelReason;
                    }



                    ServiceResponse res = SaveScheduleMaster(tempScheduleMaster, loggedInUserID);
                    if (res.IsSuccess)
                    {
                        response.IsSuccess = true;
                        searchScheduleMasterModel.ScheduleID = scheduleMaster.ScheduleID;
                        response.Data = GetScheduleMasterList(searchScheduleMasterModel, 1, pageSize, sortIndex, sortDirection, loggedInUserID, sortIndexArray).Data;
                        if (res.ErrorCode == Constants.ErrorCode_Warning)
                        {
                            response.ErrorCode = res.ErrorCode;
                            response.Message = res.Message;
                        }
                        else
                        {
                            response.Message = Resource.ScheduleDetailUpdated;
                        }
                    }
                    else
                    {
                        return res;
                    }
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

        public ServiceResponse ReScheduleFromScheduleList(SearchScheduleMasterModel searchScheduleMasterModel, ScheduleMaster scheduleMaster, int pageIndex, int pageSize, string sortIndex,
                                          string sortDirection, long loggedInUserID, string sortIndexArray)
        {
            ServiceResponse response = new ServiceResponse();


            WeekMaster week = GetEntity<WeekMaster>(scheduleMaster.WeekMasterID.Value);


            ScheduleMaster tempScheduleMaster = new ScheduleMaster
            {
                Comments = scheduleMaster.Comments,
                DropOffLocation = scheduleMaster.DropOffLocation,
                PickUpLocation = scheduleMaster.PickUpLocation,
                IsAssignedToTransportationGroupUp = false,
                IsAssignedToTransportationGroupDown = false,
                IsDeleted = false,
                ReferralID = scheduleMaster.ReferralID,
                StartDate = week.StartDate.Value,
                EndDate = week.EndDate.Value,
                WeekMasterID = scheduleMaster.WeekMasterID,
                ScheduleStatusID = scheduleMaster.ScheduleStatusID,
                FacilityID = scheduleMaster.FacilityID,
                EmployeeID = scheduleMaster.EmployeeID
            };

            var scheduleMasterRescheduleUpdate = GetEntity<ScheduleMaster>(scheduleMaster.ScheduleID);
            if (scheduleMasterRescheduleUpdate != null)
            {
                scheduleMasterRescheduleUpdate.IsReschedule = true;
                SaveObject(scheduleMasterRescheduleUpdate, loggedInUserID);
            }
            ServiceResponse res = SaveScheduleMaster(tempScheduleMaster, loggedInUserID);
            if (res.IsSuccess)
            {
                response.IsSuccess = true;
                response.Message = Resource.ClientRescheduled;
                response.Data = res.Data;
                //GetScheduleMasterList(searchScheduleMasterModel, pageIndex, pageSize, sortIndex, sortDirection,loggedInUserID, sortIndexArray).Data;
            }
            else
            {
                return res;
            }

            return response;
        }

        public ServiceResponse GetScheduleNotificationLogs(long scheduleId)
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchParam = new List<SearchValueData>();
            searchParam.Add(new SearchValueData { Name = "ScheduleID", Value = scheduleId.ToString() });
            List<ScheduleNotificationLogModel> dataModel = GetEntityList<ScheduleNotificationLogModel>(StoredProcedure.GetScheduleNotificationLogs, searchParam);
            response.Data = dataModel;
            response.IsSuccess = true;
            return response;
        }

        #endregion

        #region Schedule Aggregator Logs

        public ServiceResponse SetScheduleAggregatorLogsPage()
        {
            var response = new ServiceResponse();

            SetScheduleAggregatorLogsListPage setScheduleAggregatorLogsModel = GetMultipleEntity<SetScheduleAggregatorLogsListPage>(StoredProcedure.SetScheduleAggregatorLogsListPage, new List<SearchValueData>());
            setScheduleAggregatorLogsModel.ClaimProcessorList = Common.SetPayorClaimProcessorList();
            response.IsSuccess = true;
            response.Data = setScheduleAggregatorLogsModel;
            return response;
        }

        public ServiceResponse GetScheduleAggregatorLogsList(SearchScheduleAggregatorLogsModel searchScheduleAggregatorLogsModel, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();

                if (searchScheduleAggregatorLogsModel != null)
                    SetSearchFilterForScheduleAggregatorLogsList(searchScheduleAggregatorLogsModel, searchList);

                Page<ScheduleAggregatorLogsList> page = GetEntityPageList<ScheduleAggregatorLogsList>(StoredProcedure.GetScheduleAggregatorLogs, searchList, pageSize, pageIndex, sortIndex, sortDirection);
                response.Data = page;
                response.IsSuccess = true;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse ResendAggregatorData(SearchScheduleAggregatorLogsModel searchScheduleAggregatorLogsModel)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();

                searchList.Add(new SearchValueData
                {
                    Name = "ListOfIdsInCsv",
                    Value = Convert.ToString(searchScheduleAggregatorLogsModel.ListOfIdsInCsv)
                });

                object data = GetScalar(StoredProcedure.ResendAggregatorData, searchList);
                response.Data = data;
                response.IsSuccess = true;
                response.Message = Resource.SchedulesAddedToProcessingQueue;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse GetScheduleAggregatorLogsDetails(SearchScheduleAggregatorLogsModel searchScheduleAggregatorLogsModel)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();

                searchList.Add(new SearchValueData
                {
                    Name = "ScheduleID",
                    Value = Convert.ToString(searchScheduleAggregatorLogsModel.ScheduleID)
                });

                List<ScheduleAggregatorLogsDetails> data = GetEntityList<ScheduleAggregatorLogsDetails>(StoredProcedure.GetScheduleAggregatorLogsDetails, searchList);
                response.Data = data;
                response.IsSuccess = true;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        private static void SetSearchFilterForScheduleAggregatorLogsList(SearchScheduleAggregatorLogsModel searchScheduleAggregatorLogsModel, List<SearchValueData> searchList)
        {
            if (searchScheduleAggregatorLogsModel.StartDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = searchScheduleAggregatorLogsModel.StartDate.Value.ToString(Constants.DbDateFormat) });

            if (searchScheduleAggregatorLogsModel.EndDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = searchScheduleAggregatorLogsModel.EndDate.Value.ToString(Constants.DbDateFormat) });


            if (searchScheduleAggregatorLogsModel.EndDate == null && searchScheduleAggregatorLogsModel.StartDate == null)
            {
                searchList.Add(new SearchValueData { Name = "StartDate", Value = DateTime.UtcNow.Date.AddMonths(-1).ToString(Constants.DbDateFormat) });
                searchList.Add(new SearchValueData { Name = "EndDate", Value = DateTime.UtcNow.Date.AddDays(1).ToString(Constants.DbDateFormat) });
                searchList.Add(new SearchValueData { Name = "LastSent", Value = DateTime.UtcNow.Date.ToString(Constants.DbDateFormat) });
            }

            if (!string.IsNullOrEmpty(searchScheduleAggregatorLogsModel.Name))
                searchList.Add(new SearchValueData
                {
                    Name = "Name",
                    Value = searchScheduleAggregatorLogsModel.Name
                });

            if (!string.IsNullOrEmpty(searchScheduleAggregatorLogsModel.Address))
                searchList.Add(new SearchValueData
                {
                    Name = "Address",
                    Value = searchScheduleAggregatorLogsModel.Address
                });

            if (searchScheduleAggregatorLogsModel.EmployeeID > 0)
                searchList.Add(new SearchValueData
                {
                    Name = "EmployeeID",
                    Value = Convert.ToString(searchScheduleAggregatorLogsModel.EmployeeID)
                });

            if (searchScheduleAggregatorLogsModel.LastSent.HasValue)
                searchList.Add(new SearchValueData { Name = "LastSent", Value = searchScheduleAggregatorLogsModel.LastSent.Value.ToString(Constants.DbDateFormat) });

            if (!string.IsNullOrEmpty(searchScheduleAggregatorLogsModel.ClaimProcessor))
                searchList.Add(new SearchValueData
                {
                    Name = "ClaimProcessor",
                    Value = searchScheduleAggregatorLogsModel.ClaimProcessor
                });

            if (!string.IsNullOrEmpty(searchScheduleAggregatorLogsModel.Status))
                searchList.Add(new SearchValueData
                {
                    Name = "Status",
                    Value = Convert.ToString(searchScheduleAggregatorLogsModel.Status)
                });
        }


        #endregion

        #region ScheduleBatchService

        public ServiceResponse SaveScheduleBatchService(ScheduleBatchService batchService, long loggedInUserID)
        {
            batchService.ScheduleBatchServiceName = String.Format("{0}_BatchProcess_{1}", Common.GetEnumDisplayValue((ScheduleBatchService.ScheduleBatchServiceTypes)batchService.ScheduleBatchServiceType), DateTime.UtcNow.ToString(Constants.ReadableFileNameDateTimeFormat));
            return SaveObject(batchService, loggedInUserID, Resource.BatchProcessStarted);
        }

        public ServiceResponse SetScheduleBatchServiceListPage()
        {
            var response = new ServiceResponse();

            ScheduleBatchServiceModel scheduleBatchServiceModel = new ScheduleBatchServiceModel();

            scheduleBatchServiceModel.ScheduleBatchServiceTypeList =
                Common.GetListFromEnum<ScheduleBatchService.ScheduleBatchServiceTypes>();
            scheduleBatchServiceModel.ScheduleBatchServiceStatusList = Enum.GetNames(typeof(ScheduleBatchService.ScheduleBatchServiceStatuses)).Select(m => new NameValueDataInString()
            {
                Name = m,
                Value = m
            }).ToList();

            response.Data = scheduleBatchServiceModel;
            return response;
        }

        public ServiceResponse GetScheduleBatchServiceList(SearchScheduleBatchServiceModel searchScheduleBatchServiceModel, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                SetSearchFilterforScheduleBatchServiceList(searchScheduleBatchServiceModel, searchList);

                Page<ScheduleBatchServiceList> scheduleBatchServiceList = GetEntityPageList<ScheduleBatchServiceList>(StoredProcedure.GetScheduleBatchServiceList,
                                                                    searchList, pageSize, pageIndex, sortIndex, sortDirection);
                response.IsSuccess = true;
                response.Data = scheduleBatchServiceList;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse DeleteScheduleBatchService(SearchScheduleBatchServiceModel searchScheduleBatchServiceModel, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            try
            {
                List<ScheduleBatchService> scheduleBatchService = GetEntityList<ScheduleBatchService>(StoredProcedure.GetScheduleBatchServices, new List<SearchValueData>
                {
                    new SearchValueData{Name = "ListOfIdsInCSV",Value = searchScheduleBatchServiceModel.ListOfIdsInCSV}
                });

                foreach (var model in scheduleBatchService)
                {
                    if (!string.IsNullOrEmpty(model.FilePath))
                    {
                        AmazonFileUpload amazonFileUpload = new AmazonFileUpload();
                        amazonFileUpload.DeleteFile(ConfigSettings.ZarephathBucket, model.FilePath);
                    }
                }

                List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);
                SetSearchFilterforScheduleBatchServiceList(searchScheduleBatchServiceModel, searchList);

                if (!string.IsNullOrEmpty(searchScheduleBatchServiceModel.ListOfIdsInCSV))
                    searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchScheduleBatchServiceModel.ListOfIdsInCSV });

                searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });

                List<ScheduleBatchServiceList> totalData = GetEntityList<ScheduleBatchServiceList>(StoredProcedure.DeleteScheduleBatchService, searchList);

                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<ScheduleBatchServiceList> scheduleBatchServiceList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
                response.Data = scheduleBatchServiceList;
                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordDeletedSuccessfully, Resource.ScheduleBatchService);
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.DeleteFailed, Resource.ExceptionMessage);
            }
            return response;
        }

        private static void SetSearchFilterforScheduleBatchServiceList(SearchScheduleBatchServiceModel searchScheduleBatchServiceModel, List<SearchValueData> searchList)
        {
            if (searchScheduleBatchServiceModel.ScheduleBatchServiceName != null)
                searchList.Add(new SearchValueData
                {
                    Name = "ScheduleBatchServiceName",
                    Value = Convert.ToString(searchScheduleBatchServiceModel.ScheduleBatchServiceName)
                });

            if (!string.IsNullOrEmpty(searchScheduleBatchServiceModel.ScheduleBatchServiceType))
                searchList.Add(new SearchValueData
                {
                    Name = "ScheduleBatchServiceType",
                    Value = searchScheduleBatchServiceModel.ScheduleBatchServiceType
                });
            if (!string.IsNullOrEmpty(searchScheduleBatchServiceModel.ScheduleBatchServiceStatus))
                searchList.Add(new SearchValueData
                {
                    Name = "ScheduleBatchServiceStatus",
                    Value = searchScheduleBatchServiceModel.ScheduleBatchServiceStatus
                });

            if (!string.IsNullOrEmpty(searchScheduleBatchServiceModel.AddedBy))
                searchList.Add(new SearchValueData
                {
                    Name = "AddedBy",
                    Value = searchScheduleBatchServiceModel.AddedBy
                });

            if (!string.IsNullOrEmpty(searchScheduleBatchServiceModel.FilePath))
                searchList.Add(new SearchValueData
                {
                    Name = "FilePath",
                    Value = searchScheduleBatchServiceModel.FilePath
                });
        }

        #endregion

        #region Email Service Confirmation and Cancellation

        public ServiceResponse ConfirmationStatus(string id, long loggedinid)
        {
            var response = new ServiceResponse();
            var idnew = Crypto.Decrypt(id);

            EncryptedMailMessageToken searchMessageToken = GetEntity<EncryptedMailMessageToken>(Convert.ToInt64(idnew));

            NotificationModel notificationModel = new NotificationModel();
            notificationModel.Scheduling = true;

            if (searchMessageToken.EncryptedMailID > 0)
            {
                if (searchMessageToken.ExpireDateTime >= DateTime.Now)
                {
                    //Update MessageToken
                    searchMessageToken.IsUsed = true;
                    SaveEntity(searchMessageToken);
                    //End

                    ScheduleMaster scheduleMaster = GetEntity<ScheduleMaster>(searchMessageToken.EncryptedValue);

                    if (scheduleMaster.IsDeleted == false)
                    {
                        List<SearchValueData> searchModel = new List<SearchValueData>
                            {
                                //new SearchValueData {Name = "ScheduleID", Value = scheduleMaster.ScheduleID.ToString()},
                                new SearchValueData
                                    {
                                        Name = "StartDate",
                                        Value = Convert.ToDateTime(scheduleMaster.StartDate).ToString("MM-dd-yyy")
                                    },
                            };
                        //Check Bad Capacity with Confirmation

                        if (scheduleMaster.ScheduleStatusID == (long)ScheduleStatus.ScheduleStatuses.Cancelled
                            || scheduleMaster.ScheduleStatusID == (long)ScheduleStatus.ScheduleStatuses.No_Show
                            || scheduleMaster.ScheduleStatusID == (long)ScheduleStatus.ScheduleStatuses.No_Confirmation)
                        {
                            notificationModel.Title = Resource.SchedulealreadyCancelled;
                            notificationModel.Message = string.Format(Resource.YourscheduleisalreadyCancelled, Convert.ToDateTime(scheduleMaster.StartDate).ToString(ConfigSettings.ClientSideDateFormat));
                            notificationModel.Email = _cacheHelper.SupportEmail;
                            response.Data = notificationModel;
                            return response;
                        }


                        Count counts = GetEntity<Count>(StoredProcedure.GetScheduleCount, searchModel);

                        Facility facility = GetEntity<Facility>(scheduleMaster.FacilityID.Value);

                        //if (facility.BadCapacity > counts.Counts)
                        if (true) // Remove this Validation As Client Asked to do this.
                        {
                            if (scheduleMaster.ScheduleStatusID != (long)ScheduleStatus.ScheduleStatuses.Confirmed)
                            {
                                //Update Schedule
                                scheduleMaster.ScheduleStatusID = (long)ScheduleStatus.ScheduleStatuses.Confirmed;
                                scheduleMaster.Comments = Resource.ScheduleChangedViaLink;
                                SaveScheduleMaster(scheduleMaster, loggedinid);

                                //End

                                notificationModel.Title = Resource.ScheduleConfirmed;
                                notificationModel.Message = string.Format(Resource.SuccessfullyScheduleConfirmed, Convert.ToDateTime(scheduleMaster.StartDate).ToString(ConfigSettings.ClientSideDateFormat));
                                notificationModel.Email = _cacheHelper.SupportEmail;
                                response.Data = notificationModel;
                            }
                            else
                            {
                                notificationModel.Title = Resource.SchedulealreadyConfirmed;
                                notificationModel.Message = string.Format(Resource.Yourscheduleisalreadyconfirmed, Convert.ToDateTime(scheduleMaster.StartDate).ToString(ConfigSettings.ClientSideDateFormat));
                                notificationModel.Email = _cacheHelper.SupportEmail;
                                response.Data = notificationModel;
                            }
                        }
                        else
                        {
                            notificationModel.Title = Resource.OverBooked;
                            notificationModel.Message = string.Format(Resource.wecannotconfirmed, facility.FacilityName);
                            notificationModel.Email = _cacheHelper.SupportEmail;
                            response.Data = notificationModel;
                        }
                    }
                    else
                    {
                        notificationModel.Title = Resource.Schedulenofound;
                        notificationModel.Message = Resource.Thereisnoschedulefound;
                        notificationModel.Email = _cacheHelper.SupportEmail;
                        response.Data = notificationModel;
                    }
                }
                else
                {
                    notificationModel.Title = Resource.OppsLinkExpired;
                    notificationModel.Message = Resource.TryPastAllowedTime;
                    notificationModel.Email = _cacheHelper.SupportEmail;
                    response.Data = notificationModel;
                }
            }
            else
            {
                notificationModel.Title = Resource.Thislinkisinvalid;
                notificationModel.Message = Resource.OppsInvalidlinkContactOffice;
                notificationModel.Email = _cacheHelper.SupportEmail;
                response.Data = notificationModel;
            }
            return response;
        }

        public ServiceResponse GetCancelEmailDetail(string id)
        {
            var response = new ServiceResponse();
            var idnew = Crypto.Decrypt(id);

            CancelEmailDetailModel cancelEmailDetailmodel = new CancelEmailDetailModel
            {
                EncryptedMailMessageToken = GetEntity<EncryptedMailMessageToken>(Convert.ToInt64(idnew))
            };
            NotificationModel notificationModel = new NotificationModel();
            notificationModel.Scheduling = true;

            if (cancelEmailDetailmodel.EncryptedMailMessageToken.EncryptedMailID > 0)
            {
                if (cancelEmailDetailmodel.EncryptedMailMessageToken.ExpireDateTime >= DateTime.Now)
                {
                    cancelEmailDetailmodel.ScheduleMaster = GetEntity<ScheduleMaster>(cancelEmailDetailmodel.EncryptedMailMessageToken.EncryptedValue);

                    if (cancelEmailDetailmodel.ScheduleMaster.IsDeleted == false)
                    {
                        if (cancelEmailDetailmodel.ScheduleMaster.ScheduleStatusID != (long)ScheduleStatus.ScheduleStatuses.Cancelled)
                        {
                            response.Data = cancelEmailDetailmodel;
                            response.IsSuccess = true;
                        }
                        else
                        {
                            notificationModel.Title = Resource.SchedulealreadyCancelled;
                            notificationModel.Message = string.Format(Resource.YourscheduleisalreadyCancelled, Convert.ToDateTime(cancelEmailDetailmodel.ScheduleMaster.StartDate).ToString(ConfigSettings.ClientSideDateFormat));
                            notificationModel.Email = _cacheHelper.SupportEmail;
                            response.Data = notificationModel;
                            response.IsSuccess = false;
                        }
                    }
                    else
                    {
                        notificationModel.Title = Resource.Schedulenofound;
                        notificationModel.Message = Resource.Thereisnoschedulefound;
                        notificationModel.Email = _cacheHelper.SupportEmail;
                        response.Data = notificationModel;
                    }
                }
                else
                {
                    notificationModel.Title = Resource.OppsLinkExpired;
                    notificationModel.Message = Resource.OppsLinkExpiredContactOffice;
                    notificationModel.Email = _cacheHelper.SupportEmail;
                    response.Data = notificationModel;
                    response.IsSuccess = false;
                }
            }
            else
            {
                notificationModel.Title = Resource.OppsInvalidlink;
                notificationModel.Message = Resource.OppsInvalidlinkContactOffice;
                notificationModel.Email = _cacheHelper.SupportEmail;
                response.Data = notificationModel;
                response.IsSuccess = false;
            }
            return response;
        }

        public ServiceResponse UpdateScheduleCancelstatus(CancelEmailDetailModel cancelEmailDetailModel, long loggeindid)
        {
            var response = new ServiceResponse();
            NotificationModel notificationModel = new NotificationModel();
            notificationModel.Scheduling = true;

            if (cancelEmailDetailModel.EncryptedMailMessageToken.EncryptedMailID > 0)
            {
                if (cancelEmailDetailModel.EncryptedMailMessageToken.ExpireDateTime >= DateTime.Now)
                {
                    //Update MessageToken
                    cancelEmailDetailModel.EncryptedMailMessageToken.IsUsed = true;
                    SaveEntity(cancelEmailDetailModel.EncryptedMailMessageToken);
                    //End
                    ScheduleMaster scheduleMaster = GetEntity<ScheduleMaster>(cancelEmailDetailModel.ScheduleMaster.ScheduleID);
                    if (cancelEmailDetailModel.ScheduleMaster.ScheduleStatusID != (long)ScheduleStatus.ScheduleStatuses.Cancelled)
                    {
                        //Update Schedule

                        List<TransportationGroupClient> assignedGroup = GetEntityList<TransportationGroupClient>(new List<SearchValueData> { new SearchValueData { Name = "ScheduleID", Value = scheduleMaster.ScheduleID.ToString() } });
                        ITransportationGroupDataProvider transportationGroupDataProvider = new TransportationGroupDataProvider();
                        foreach (TransportationGroupClient transportationGroupClient in assignedGroup)
                        {
                            transportationGroupDataProvider.RemoveTransportationGroupClient(
                                transportationGroupClient.TransportationGroupClientID, loggeindid);
                        }


                        //Setting schedulemaster to null.
                        scheduleMaster.FacilityID = null;
                        scheduleMaster.IsAssignedToTransportationGroupDown = false;
                        scheduleMaster.IsAssignedToTransportationGroupUp = false;

                        scheduleMaster.ScheduleStatusID = (long)ScheduleStatus.ScheduleStatuses.Cancelled;
                        scheduleMaster.WhoCancelled = Constants.Parent;
                        scheduleMaster.WhenCancelled = DateTime.Now;
                        string cancelReason = string.IsNullOrEmpty(cancelEmailDetailModel.ScheduleMaster.CancelReason) ? "" : "\nParent's Comment: " + cancelEmailDetailModel.ScheduleMaster.CancelReason;
                        scheduleMaster.CancelReason = String.Format("{0}. {1}", Resource.ScheduleChangedViaLink, cancelReason);
                        scheduleMaster.Comments = String.Format("{0}. {1}", Resource.ScheduleChangedViaLink, cancelReason);
                        SaveScheduleMaster(scheduleMaster, loggeindid);

                        //End
                        notificationModel.Title = Resource.ScheduleCancelled;
                        notificationModel.Message = string.Format(Resource.YourscheduleisCancelled, Convert.ToDateTime(scheduleMaster.StartDate).ToString(ConfigSettings.ClientSideDateFormat));
                        notificationModel.Email = _cacheHelper.SupportEmail;
                        response.Data = notificationModel;
                        response.IsSuccess = true;
                    }
                    else
                    {
                        notificationModel.Title = Resource.SchedulealreadyCancelled;
                        notificationModel.Message = string.Format(Resource.YourscheduleisalreadyCancelled, Convert.ToDateTime(scheduleMaster.StartDate).ToString(ConfigSettings.ClientSideDateFormat));
                        notificationModel.Email = _cacheHelper.SupportEmail;
                        response.Data = notificationModel;
                        response.IsSuccess = false;
                    }
                }
                else
                {
                    notificationModel.Title = Resource.OppsLinkExpired;
                    notificationModel.Message = Resource.OppsLinkExpiredContactOffice;
                    notificationModel.Email = _cacheHelper.SupportEmail;
                    response.Data = notificationModel;
                    response.IsSuccess = false;
                }
            }
            else
            {
                notificationModel.Title = Resource.OppsInvalidlink;
                notificationModel.Message = Resource.OppsInvalidlinkContactOffice;
                notificationModel.Email = _cacheHelper.SupportEmail;
                response.Data = notificationModel;
                response.IsSuccess = false;
            }
            return response;
        }

        public ServiceResponse GetEmailDetail(long scheduleId)
        {
            var response = new ServiceResponse();

            #region Getting List And Email Template Model

            var searchList = new List<SearchValueData> { new SearchValueData { Name = "ScheduleID", Value = Convert.ToString(scheduleId) }, };

            ListScheduleEmail listScheduleEmail = GetEntity<ListScheduleEmail>(StoredProcedure.GetScheduleEmailDetail, searchList);

            EmailTemplate emailTemplate = GetEntity<EmailTemplate>(new List<SearchValueData> { new SearchValueData { Name = "EmailTemplateTypeID", Value = ((int)EnumEmailType.Schedule_Notification).ToString() } });

            #endregion

            if (emailTemplate != null && listScheduleEmail != null)
            {
                #region Get Previous Wednesday

                DateTime lastWednesday = Convert.ToDateTime(listScheduleEmail.StartDate).AddDays(-1);
                while (lastWednesday.DayOfWeek != DayOfWeek.Wednesday)
                {
                    lastWednesday = lastWednesday.AddDays(-1);
                }
                listScheduleEmail.LastWeekGenrateDate = Constants.Wednesday + " " +
                                                        Convert.ToDateTime(lastWednesday)
                                                               .ToString(ConfigSettings.ClientSideDateFormat)
                                                               .Replace("-", "/") +
                                                        " at " + ConfigSettings.TransPortationConfirmCancelTime;

                if (listScheduleEmail.StartDate == listScheduleEmail.EndDate)
                {
                    listScheduleEmail.ScheduleDateString =
                        Convert.ToDateTime(listScheduleEmail.StartDate)
                               .ToString(ConfigSettings.ClientSideDateFormat)
                               .Replace("-", "/");
                }
                else
                {
                    listScheduleEmail.ScheduleDateString = Convert.ToDateTime(listScheduleEmail.StartDate)
                                                                  .ToString(ConfigSettings.ClientSideDateFormat)
                                                                  .Replace("-", "/")
                                                           + " to " +
                                                           Convert.ToDateTime(listScheduleEmail.EndDate)
                                                                  .ToString(ConfigSettings.ClientSideDateFormat)
                                                                  .Replace("-", "/");
                }

                #endregion

                if (!string.IsNullOrEmpty(listScheduleEmail.ClientNickName))
                {
                    listScheduleEmail.FirstName = listScheduleEmail.ClientNickName;
                }
                else
                {
                    listScheduleEmail.FirstName = listScheduleEmail.FirstName;
                }

                #region Get Day of Pickup for base on Start Date

                string pickUpDay =
                    Convert.ToString(Convert.ToDateTime(listScheduleEmail.EndDate).DayOfWeek);
                string pickUpTime = null;

                switch (pickUpDay)
                {
                    case Constants.Monday:
                        pickUpTime = listScheduleEmail.MondayPickUp;
                        break;
                    case Constants.Tuesday:
                        pickUpTime = listScheduleEmail.TuesdayPickUp;
                        break;
                    case Constants.Wednesday:
                        pickUpTime = listScheduleEmail.WednesdayPickUp;
                        break;
                    case Constants.Thursday:
                        pickUpTime = listScheduleEmail.ThursdayPickUp;
                        break;
                    case Constants.Friday:
                        pickUpTime = listScheduleEmail.FridayPickUp;
                        break;
                    case Constants.Saturday:
                        pickUpTime = listScheduleEmail.SaturdayPickUp;
                        break;
                    case Constants.Sunday:
                        pickUpTime = listScheduleEmail.SundayPickUp;
                        break;
                }

                if (!string.IsNullOrEmpty(pickUpTime))
                {
                    listScheduleEmail.AtPickUp = "at";
                }

                #endregion

                #region Get Day of DropOff for base on end date

                string dropOffDay =
                    Convert.ToString(Convert.ToDateTime(listScheduleEmail.StartDate).DayOfWeek);
                string dropOfftime = null;
                switch (dropOffDay)
                {
                    case Constants.Monday:
                        dropOfftime = listScheduleEmail.MondayDropOff;
                        break;
                    case Constants.Tuesday:
                        dropOfftime = listScheduleEmail.TuesdayDropOff;
                        break;
                    case Constants.Wednesday:
                        dropOfftime = listScheduleEmail.WednesdayDropOff;
                        break;
                    case Constants.Thursday:
                        dropOfftime = listScheduleEmail.ThursdayDropOff;
                        break;
                    case Constants.Friday:
                        dropOfftime = listScheduleEmail.FridayDropOff;
                        break;
                    case Constants.Saturday:
                        dropOfftime = listScheduleEmail.SaturdayDropOff;
                        break;
                    case Constants.Sunday:
                        dropOfftime = listScheduleEmail.SundayDropOff;
                        break;
                }

                if (!string.IsNullOrEmpty(dropOfftime))
                {
                    listScheduleEmail.AtDropOff = "at";
                }

                #endregion

                #region Set Value

                listScheduleEmail.PickUpTime = pickUpTime;
                listScheduleEmail.PickUpDay = Convert.ToString(Convert.ToDateTime(listScheduleEmail.EndDate).DayOfWeek) + " " +
                    Convert.ToDateTime(listScheduleEmail.EndDate).ToString(ConfigSettings.ClientSideDateFormat).Replace("-", "/");

                listScheduleEmail.DropOffTime = dropOfftime;
                listScheduleEmail.DropOffDay = Convert.ToString(Convert.ToDateTime(listScheduleEmail.StartDate).DayOfWeek) + " " + Convert.ToDateTime(listScheduleEmail.StartDate)
                           .ToString(ConfigSettings.ClientSideDateFormat).Replace("-", "/");

                listScheduleEmail.ConfirmationUrl = "<a style=\"color: #fff; text-decoration: none; display: block;\" href=\"##ConfirmationURL##\">Confirm Schedule</a>";
                listScheduleEmail.CancellatioUrl = "<a style=\"color: #fff; text-decoration: none; display: block;\"  href=\"##CancelURL##\">Cancel Schedule</a>";

                if (!string.IsNullOrEmpty(listScheduleEmail.DropOffImage))
                {
                    listScheduleEmail.DropOffImage = ConfigSettings.AmazonS3Url + ConfigSettings.ZarephathBucket + "/" + listScheduleEmail.DropOffImage;
                }
                else
                {
                    listScheduleEmail.DropOffImage = _cacheHelper.SiteBaseURL + Constants.NoMapImageUrl;
                }
                if (!string.IsNullOrEmpty(listScheduleEmail.PickUpImage))
                {
                    listScheduleEmail.PickUpImage = ConfigSettings.AmazonS3Url + ConfigSettings.ZarephathBucket + "/" + listScheduleEmail.PickUpImage;
                }
                else
                {
                    listScheduleEmail.PickUpImage = _cacheHelper.SiteBaseURL + Constants.NoMapImageUrl;
                }
                listScheduleEmail.ZerpathLogoImage = _cacheHelper.SiteBaseURL + Constants.ZerpathLogoImage;

                #endregion

                #region Replace the The Content

                ScheduleMasterModel scheduleMasterModel = new ScheduleMasterModel();

                scheduleMasterModel.ScheduleEmailModel.ScheduleID = listScheduleEmail.ScheduleID;
                scheduleMasterModel.ScheduleEmailModel.Email = listScheduleEmail.Email;
                scheduleMasterModel.ScheduleEmailModel.StartDate = listScheduleEmail.StartDate;

                scheduleMasterModel.ScheduleEmailModel.Subject = emailTemplate.EmailTemplateSubject;
                scheduleMasterModel.ScheduleEmailModel.Body = TokenReplace.ReplaceTokens(emailTemplate.EmailTemplateBody, listScheduleEmail);

                #endregion

                response.IsSuccess = true;
                response.Data = scheduleMasterModel.ScheduleEmailModel;
            }

            return response;
        }

        public ServiceResponse GetSMSDetail(long scheduleId)
        {
            var response = new ServiceResponse();

            #region Getting List And SMS Template Model

            var searchList = new List<SearchValueData> { new SearchValueData { Name = "ScheduleID", Value = Convert.ToString(scheduleId) }, };

            ListScheduleEmail listScheduleEmail = GetEntity<ListScheduleEmail>(StoredProcedure.GetScheduleEmailDetail, searchList);

            EmailTemplate emailTemplate = GetEntity<EmailTemplate>(new List<SearchValueData> { new SearchValueData
            { Name = "EmailTemplateTypeID", Value = ((int)EnumEmailType.Schedule_Notification_SMS).ToString() } });

            #endregion

            if (emailTemplate != null && listScheduleEmail != null)
            {
                #region Get Previous Wednesday

                DateTime lastWednesday = Convert.ToDateTime(listScheduleEmail.StartDate).AddDays(-1);
                while (lastWednesday.DayOfWeek != DayOfWeek.Wednesday)
                {
                    lastWednesday = lastWednesday.AddDays(-1);
                }
                listScheduleEmail.LastWeekGenrateDate = Constants.Wed + " " + Convert.ToDateTime(lastWednesday).ToString(ConfigSettings.ScheduleDateFormat) +
                                                        " @ " + ConfigSettings.TransPortationConfirmCancelTime;

                if (listScheduleEmail.StartDate == listScheduleEmail.EndDate)
                {
                    listScheduleEmail.ScheduleDateString = Convert.ToDateTime(listScheduleEmail.StartDate).ToString(ConfigSettings.ScheduleDateFormat);
                }
                else
                {
                    listScheduleEmail.ScheduleDateString = Convert.ToDateTime(listScheduleEmail.StartDate).ToString(ConfigSettings.ScheduleDateFormat)
                                                           + "-" + Convert.ToDateTime(listScheduleEmail.EndDate).ToString(ConfigSettings.ScheduleDateFormat);
                }
                if (!string.IsNullOrEmpty(listScheduleEmail.ClientNickName))
                {
                    listScheduleEmail.FirstName = listScheduleEmail.ClientNickName;
                }
                else
                {
                    listScheduleEmail.FirstName = listScheduleEmail.FirstName;
                }

                #endregion

                #region Set Value

                listScheduleEmail.ConfirmationUrl = "##ConfirmationURL##";

                #endregion

                #region Replace the The Content for SMS Model

                ScheduleMasterModel scheduleMasterModel = new ScheduleMasterModel();
                scheduleMasterModel.ScheduleSmsModel.ToNumber = listScheduleEmail.Phone1;
                scheduleMasterModel.ScheduleSmsModel.ScheduleID = listScheduleEmail.ScheduleID;
                scheduleMasterModel.ScheduleSmsModel.StartDate = listScheduleEmail.StartDate;
                scheduleMasterModel.ScheduleSmsModel.Body = TokenReplace.ReplaceTokens(emailTemplate.EmailTemplateBody, listScheduleEmail);

                #endregion

                response.IsSuccess = true;
                response.Data = scheduleMasterModel.ScheduleSmsModel;

            }
            return response;
        }

        public ServiceResponse SendParentEmail(ScheduleEmailModel scheduleEmailModel, long loggedInUserID)
        {
            var response = new ServiceResponse();

            #region Get Previous Wednesday ScarpCode
            DateTime lastWednesday = Convert.ToDateTime(scheduleEmailModel.StartDate).AddDays(-1);
            while (lastWednesday.DayOfWeek != DayOfWeek.Wednesday)
            {
                lastWednesday = lastWednesday.AddDays(-1);
            }
            #endregion

            DateTime expierDate = DateTime.Now.AddDays(Constants.MonthValue - 1);

            EncryptedMailMessageToken encryptedmailmessagetoken = new EncryptedMailMessageToken();
            encryptedmailmessagetoken.EncryptedValue = Convert.ToInt32(scheduleEmailModel.ScheduleID);
            encryptedmailmessagetoken.IsUsed = false;
            encryptedmailmessagetoken.ExpireDateTime = lastWednesday.AddDays(1);
            SaveObject(encryptedmailmessagetoken);

            var id = Crypto.Encrypt(Convert.ToString(encryptedmailmessagetoken.EncryptedMailID));

            scheduleEmailModel.Body = scheduleEmailModel.Body.Replace("##ConfirmationURL##", _cacheHelper.SiteBaseURL + Constants.ConfirmationUrl + id);
            scheduleEmailModel.Body = scheduleEmailModel.Body.Replace("##CancelURL##", _cacheHelper.SiteBaseURL + Constants.CancellationUrl + id);
            bool isSentMail = false;
            if (scheduleEmailModel.Email != null)
            {
                //isSentMail = Common.SendEmail(scheduleEmailModel.Subject, ConfigSettings.SupportEmail, scheduleEmailModel.Email, scheduleEmailModel.Body, null, ConfigSettings.CCEmailAddress);
                isSentMail = Common.SendEmail(scheduleEmailModel.Subject, _cacheHelper.SupportEmail, scheduleEmailModel.Email, scheduleEmailModel.Body, null);
                response.IsSuccess = isSentMail;
            }

            if (isSentMail)
            {
                INoteDataProvider iNoteDataProvider = new NoteDataProvider();
                iNoteDataProvider.SaveGeneralNote(scheduleEmailModel.ReferralID, scheduleEmailModel.Body, Resource.WebsiteScheduleNotificationEmail, loggedInUserID, scheduleEmailModel.ParentName + " (" +
                                                  scheduleEmailModel.Phone + ")", Resource.Parent, Resource.Email);

                //When Send Mail that time Update Status for Send Mail for this Schedule
                UpdateScheduleMasterNotificaitonDetails(scheduleEmailModel.ScheduleID, Constants.Email);

            }


            #region SAVE Schedle Notification Log
            SaveScheduleNotificationLogDetails(scheduleEmailModel.ReferralID, scheduleEmailModel.ScheduleID, Constants.Email,
                Resource.WebsiteScheduleNotificationEmail, scheduleEmailModel.Body, isSentMail,
                toEmail: scheduleEmailModel.Email, subject: scheduleEmailModel.Subject, createdBy: loggedInUserID);
            #endregion


            response.Message = isSentMail ? Resource.EmailSentSuccess : Resource.EmailSentFailed;

            return response;
        }

        public ServiceResponse SendParentSMS(ScheduleSmsModel scheduleSmsModel, long loggedInUserID)
        {
            string emailtype = Convert.ToString(EnumEmailType.Schedule_Notification_SMS);
            var response = new ServiceResponse();

            //DateTime expierDate = DateTime.Now.AddDays(Constants.MonthValue - 1);

            //EncryptedMailMessageToken encryptedmailmessagetoken = new EncryptedMailMessageToken();
            //encryptedmailmessagetoken.EncryptedValue = Convert.ToInt32(scheduleSmsModel.ScheduleID);
            //encryptedmailmessagetoken.IsUsed = false;
            //encryptedmailmessagetoken.ExpireDateTime = expierDate;
            //SaveObject(encryptedmailmessagetoken);

            var id = Crypto.Encrypt(Convert.ToString(scheduleSmsModel.ScheduleID));

            var url = Common.GenrtaeShortUrl(_cacheHelper.SiteBaseURL + Constants.ConfirmSMSurl + id);

            scheduleSmsModel.Body = scheduleSmsModel.Body.Replace("##ConfirmationURL##", url);
            //scheduleSmsModel.Body = scheduleSmsModel.Body.Replace("##CancelURL##", ConfigSettings.SiteBaseURL + Constants.CancellationUrl + id);

            bool isSentSMS = Common.SendSms(scheduleSmsModel.ToNumber, scheduleSmsModel.Body, emailtype);

            if (isSentSMS)
            {
                INoteDataProvider iNoteDataProvider = new NoteDataProvider();
                iNoteDataProvider.SaveGeneralNote(scheduleSmsModel.ReferralID, scheduleSmsModel.Body, Resource.WebsiteScheduleNotificationSMS, loggedInUserID, scheduleSmsModel.ParentName + " (" +
                                                  scheduleSmsModel.Phone + ")", Resource.Parent, Resource.SMS);

                //When Send Mail that time Update Status for Send Mail for this Schedule
                UpdateScheduleMasterNotificaitonDetails(scheduleSmsModel.ScheduleID, Constants.SMS);
            }

            #region SAVE Schedle Notification Log
            SaveScheduleNotificationLogDetails(scheduleSmsModel.ReferralID, scheduleSmsModel.ScheduleID, Constants.SMS,
                Resource.WebsiteScheduleNotificationSMS, scheduleSmsModel.Body, isSentSMS,
                toPhone: scheduleSmsModel.ToNumber, createdBy: loggedInUserID);
            #endregion

            response.IsSuccess = isSentSMS;
            response.Message = isSentSMS ? Resource.SMSSentSuccess : Resource.SMSSentFail;
            return response;
        }

        public void SaveScheduleNotificationLogDetails(long referralId, long scheduleId, string notificationType, string notificationSubType,
          string body, bool isSent, string toEmail = null, string subject = null, string toPhone = null, long? createdBy = null, string source = Constants.ViaSite)
        {

            ScheduleNotificationLog scheduleNotificationLog = new ScheduleNotificationLog();
            scheduleNotificationLog.ReferralID = referralId;
            scheduleNotificationLog.ScheduleID = scheduleId;
            scheduleNotificationLog.NotificationType = notificationType;
            scheduleNotificationLog.NotificationSubType = notificationSubType;

            scheduleNotificationLog.Source = source;
            scheduleNotificationLog.NotificationType = notificationType;


            if (notificationType == Constants.Email)
            {
                Configuration configurationFile = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
                MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
                scheduleNotificationLog.FromEmailAddress = mailSettings == null ? "" : mailSettings.Smtp.From;
                scheduleNotificationLog.ToEmailAddress = toEmail;
                scheduleNotificationLog.Subject = subject;
            }
            else if (notificationType == Constants.SMS)
            {
                string CountryCode = string.IsNullOrEmpty(_cacheHelper.TwilioDefaultCountryCode) ? Constants.DefaultCountryCodeForSms : _cacheHelper.TwilioDefaultCountryCode;
                scheduleNotificationLog.FromPhone = ConfigSettings.TwilioFromNo;
                scheduleNotificationLog.ToPhone = CountryCode + toPhone;
                //scheduleNotificationLog.ToPhone = ConfigSettings.DefaultCountryCodeForSms + toPhone;
            }


            scheduleNotificationLog.Body = body;
            scheduleNotificationLog.IsSent = isSent;
            scheduleNotificationLog.CreatedDate = DateTime.UtcNow;
            scheduleNotificationLog.CreatedBy = createdBy;

            SaveEntity(scheduleNotificationLog);
        }

        public ServiceResponse GetRegionWiseWeekFacility(long regionID, long weekMasterID)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData {Name = "WeekMasterID", Value = weekMasterID.ToString()},
                    new SearchValueData {Name = "RegionID", Value = regionID.ToString()}
                };

            RegionWiseWeekFacility regionWiseWeekFacility = GetEntity<RegionWiseWeekFacility>(searchParam);

            if (regionWiseWeekFacility != null)
            {
                response.IsSuccess = true;
                response.Data = regionWiseWeekFacility.Facilities.Split(',');
            }
            else
            {
                response.IsSuccess = false;
            }
            return response;
        }

        public ServiceResponse SaveRegionWiseWeekFacility(long regionID, long weekMasterID, string facilites)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData {Name = "WeekMasterID", Value = weekMasterID.ToString()},
                    new SearchValueData {Name = "RegionID", Value = regionID.ToString()}
                };

            RegionWiseWeekFacility regionWiseWeekFacility = GetEntity<RegionWiseWeekFacility>(searchParam);

            if (regionWiseWeekFacility != null)
            {
                regionWiseWeekFacility.Facilities = facilites;
                SaveEntity(regionWiseWeekFacility);
            }
            else
            {
                regionWiseWeekFacility = new RegionWiseWeekFacility
                {
                    Facilities = facilites,
                    RegionID = regionID,
                    WeekMasterID = weekMasterID
                };
                SaveEntity(regionWiseWeekFacility);
            }


            #region Get Transportation Locations Related Client Count For Up/Down Scheduling

            searchParam = new List<SearchValueData> { new SearchValueData { Name = "WeekMasterID", Value = weekMasterID.ToString() }
                , new SearchValueData {Name = "RegionID", Value = regionID.ToString()} };
            ClientCountAtLocationsListModel dataModel = GetMultipleEntity<ClientCountAtLocationsListModel>(StoredProcedure.GetClientCountAtTransportationLocations, searchParam);
            response.Data = dataModel;

            #endregion



            response.IsSuccess = true;
            return response;
        }

        #endregion

        #region General Function For Email,SMS, Respite Notices

        public ServiceResponse SendScheduleDetailEmailSMS(ScheduleDetailEmailSMSParam scheduleDetailEmailSMSParam, bool batchService = false)
        {
            ServiceResponse response = new ServiceResponse();

            #region Set Serach Parameter and List

            var searchList = new List<SearchValueData>
                {
                    new SearchValueData {Name = "Monthday", Value = Convert.ToString(Constants.MonthValue)},
                    new SearchValueData {Name = "WeekDay", Value = Convert.ToString(Constants.WeekValue)},
                    new SearchValueData {Name = "IsWeekMonthFromService", Value = Convert.ToString(scheduleDetailEmailSMSParam.IsWeekMonthFromService)},
                    new SearchValueData {Name = "ScheduleIds", Value = Convert.ToString(scheduleDetailEmailSMSParam.ScheduleIds)}
                };

            List<ListEmailService> listEmailService = GetEntityList<ListEmailService>(StoredProcedure.GetScheduleEmail, searchList);
            #endregion

            if (listEmailService.Count > 0)
            {
                EmailTemplate emailTemplate = GetEntity<EmailTemplate>(new List<SearchValueData>
                                            {
                                                new SearchValueData{Name = "EmailTemplateTypeID",Value =Convert.ToInt16(EnumEmailType.Schedule_Notification).ToString(),IsEqual = true}
                                            });

                EmailTemplate smsTemplate = GetEntity<EmailTemplate>(new List<SearchValueData>
                                            {
                                                new SearchValueData{Name = "EmailTemplateTypeID",Value =Convert.ToInt16(EnumEmailType.Schedule_Notification_SMS).ToString(),IsEqual = true}
                                            });



                foreach (ListEmailService emailServiceModel in listEmailService)
                {
                    bool isSentMail = false;
                    string emailbody = "";
                    bool isSentSms = false;
                    string smSbody = "";

                    //Set Model Values To Send Mail.
                    SetEmailDetailTemplateModel(emailServiceModel, scheduleDetailEmailSMSParam.IsSendSMS);

                    #region SendEmail Or/And SMS

                    //If Referral have set Email Permission and Function passed To Send Mail then it Will send Mail.
                    //if (scheduleDetailEmailSMSParam.IsSendEmail && emailServiceModel.PermissionForEmail && emailServiceModel.PCMEmail && !emailServiceModel.EmailSent)
                    if (scheduleDetailEmailSMSParam.IsSendEmail && emailServiceModel.PermissionForEmail && emailServiceModel.PCMEmail)
                    {
                        if (scheduleDetailEmailSMSParam.IsWeekMonthFromService == false ||
                            (scheduleDetailEmailSMSParam.IsWeekMonthFromService && emailServiceModel.WeekEmailDate == null))
                        {
                            if (emailServiceModel.Email != null)
                            {

                                emailbody = TokenReplace.ReplaceTokens(emailTemplate.EmailTemplateBody, emailServiceModel);
                                //isSentMail = Common.SendEmail(emailTemplate.EmailTemplateSubject, ConfigSettings.SupportEmail, emailServiceModel.Email, emailbody, EnumEmailType.Schedule_Notification.ToString(), ConfigSettings.CCEmailAddress);
                                isSentMail = Common.SendEmail(emailTemplate.EmailTemplateSubject, _cacheHelper.SupportEmail, emailServiceModel.Email, emailbody, EnumEmailType.Schedule_Notification.ToString());

                                #region SAVE Schedle Notification Log
                                SaveScheduleNotificationLogDetails(emailServiceModel.ReferralID, emailServiceModel.ScheduleID, Constants.Email,
                                    Resource.BatchServiceScheduleNotificationEmail, emailbody, isSentMail,
                                    toEmail: emailServiceModel.Email, subject: emailTemplate.EmailTemplateSubject, createdBy: scheduleDetailEmailSMSParam.CreatedBy);
                                #endregion

                                #region Update Schedule Master
                                if (isSentMail)
                                    UpdateScheduleMasterNotificaitonDetails(emailServiceModel.ScheduleID, Constants.Email, scheduleDetailEmailSMSParam.IsWeekMonthFromService);
                                #endregion
                            }
                        }
                    }

                    //If Referral have set SMS Permission and Function passed To Send SMS then it Will send Mail.
                    //if (scheduleDetailEmailSMSParam.IsSendSMS && emailServiceModel.PermissionForSMS && emailServiceModel.PCMSMS && !emailServiceModel.SMSSent)
                    if (scheduleDetailEmailSMSParam.IsSendSMS && emailServiceModel.PermissionForSMS && emailServiceModel.PCMSMS)
                    {
                        if (scheduleDetailEmailSMSParam.IsWeekMonthFromService == false ||
                            (scheduleDetailEmailSMSParam.IsWeekMonthFromService && emailServiceModel.WeekSMSDate == null))
                        {
                            var id = Crypto.Encrypt(Convert.ToString(emailServiceModel.ScheduleID));
                            var url = Common.GenrtaeShortUrl(_cacheHelper.SiteBaseURL + Constants.ConfirmSMSurl + id);
                            emailServiceModel.ConfirmationUrl = url;
                            if (emailServiceModel.Phone1 != null)
                            {
                                smSbody = TokenReplace.ReplaceTokens(smsTemplate.EmailTemplateBody, emailServiceModel);
                                isSentSms = Common.SendSms(emailServiceModel.Phone1, smSbody, Resource.ScheduleNotificationSMS);
                            }

                            #region SAVE Schedle Notification Log

                            SaveScheduleNotificationLogDetails(emailServiceModel.ReferralID, emailServiceModel.ScheduleID, Constants.SMS,
                                Resource.BatchServiceScheduleNotificationSMS, smSbody, isSentSms,
                                toPhone: emailServiceModel.Phone1, createdBy: scheduleDetailEmailSMSParam.CreatedBy);

                            #endregion

                            #region Update Schedule Master
                            if (isSentSms)
                                UpdateScheduleMasterNotificaitonDetails(emailServiceModel.ScheduleID, Constants.SMS, scheduleDetailEmailSMSParam.IsWeekMonthFromService);
                            #endregion
                        }
                    }

                    #endregion

                    //Make Entry in GeneralNote For Sending mail and Sms.

                    #region AddGeneralNote

                    if (isSentMail)
                    {
                        INoteDataProvider iNoteDataProvider = new NoteDataProvider();
                        iNoteDataProvider.SaveGeneralNote(emailServiceModel.ReferralID, emailbody, batchService ? Resource.BatchServiceScheduleNotificationEmail : Resource.ScheduleServiceScheduleNotificationEmail, 0, emailServiceModel.ParentFirstName + " (" +
                                                          emailServiceModel.Phone1 + ")", Resource.Parent, Resource.Email);
                    }

                    if (isSentSms)
                    {
                        INoteDataProvider iNoteDataProvider = new NoteDataProvider();
                        iNoteDataProvider.SaveGeneralNote(emailServiceModel.ReferralID, smSbody, batchService ? Resource.BatchServiceScheduleNotificationSMS : Resource.ScheduleServiceScheduleNotificationSMS, 0, emailServiceModel.ParentFirstName + " (" +
                                                          emailServiceModel.Phone1 + ")", Resource.Parent, Resource.SMS);
                    }

                    #endregion
                }
            }
            response.IsSuccess = true;
            return response;
        }

        private void SetEmailDetailTemplateModel(ListEmailService listEmailService, bool doScheduleShortText = false)
        {

            string dateformat = doScheduleShortText ? ConfigSettings.ScheduleDateFormat : ConfigSettings.ClientSideDateFormat;
            string appendTextAt = doScheduleShortText ? " @ " : " at ";
            string appendText = doScheduleShortText ? "-" : " to ";
            string appendTextWed = doScheduleShortText ? Constants.Wed : Constants.Wednesday;


            listEmailService.ZerpathLogoImage = _cacheHelper.SiteBaseURL + Constants.ZerpathLogoImage;

            /* Last Expiration Date will be last wednesday before schedule startdate*/
            #region Get Previous Wednesday

            if (listEmailService.IsProcessed)
            {
                listEmailService.child = Constants.Childrens;
            }
            else
            {
                listEmailService.child = Constants.Childs;
            }

            DateTime lastWednesday = listEmailService.StartDate;
            while (lastWednesday.DayOfWeek != DayOfWeek.Wednesday)
            {
                lastWednesday = lastWednesday.AddDays(-1);
            }

            listEmailService.LastWeekGenrateDate = appendTextWed + " " + Convert.ToDateTime(lastWednesday).ToString(dateformat).Replace("-", "/") +
                                                             appendTextAt + ConfigSettings.TransPortationConfirmCancelTime;
            #endregion

            #region GenerateEncrypted ID

            EncryptedMailMessageToken encryptedmailmessagetoken = new EncryptedMailMessageToken();
            encryptedmailmessagetoken.EncryptedValue = Convert.ToInt32(listEmailService.ScheduleID);
            encryptedmailmessagetoken.IsUsed = false;
            encryptedmailmessagetoken.ExpireDateTime = lastWednesday.AddDays(1);
            SaveObject(encryptedmailmessagetoken);
            string encryptedID = Crypto.Encrypt(Convert.ToString(encryptedmailmessagetoken.EncryptedMailID));

            listEmailService.ConfirmationUrl = "<a style=\"color: #fff; text-decoration: none; display: block;\" href=\"" + _cacheHelper.SiteBaseURL + Constants.ConfirmationUrl + encryptedID +
                                "\">Confirm Schedule</a>";
            listEmailService.CancellatioUrl = "<a style=\"color: #fff; text-decoration: none; display: block;\"  href=\"" + _cacheHelper.SiteBaseURL + Constants.CancellationUrl + encryptedID +
                "\">Cancel Schedule</a>";
            #endregion

            #region Schedule Date String


            if (listEmailService.StartDate == listEmailService.EndDate)
            {
                listEmailService.ScheduleDateString = Convert.ToDateTime(listEmailService.StartDate).ToString(dateformat).Replace("-", "/");
            }
            else
            {
                listEmailService.ScheduleDateString = Convert.ToDateTime(listEmailService.StartDate).ToString(dateformat).Replace("-", "/")
                + appendText + Convert.ToDateTime(listEmailService.EndDate).ToString(dateformat).Replace("-", "/");
            }

            if (!string.IsNullOrEmpty(listEmailService.ClientNickName))
            {
                listEmailService.FirstName = listEmailService.ClientNickName;
            }
            else
            {
                listEmailService.FirstName = listEmailService.FirstName;
            }

            #endregion

            #region Get Day of Pickup for base on Start Date

            string pickUpDay = Convert.ToString(Convert.ToDateTime(listEmailService.EndDate).DayOfWeek);
            string pickUpTime = null;

            switch (pickUpDay)
            {
                case Constants.Monday:
                    pickUpTime = listEmailService.MondayPickUp;
                    break;
                case Constants.Tuesday:
                    pickUpTime = listEmailService.TuesdayPickUp;
                    break;
                case Constants.Wednesday:
                    pickUpTime = listEmailService.WednesdayPickUp;
                    break;
                case Constants.Thursday:
                    pickUpTime = listEmailService.ThursdayPickUp;
                    break;
                case Constants.Friday:
                    pickUpTime = listEmailService.FridayPickUp;
                    break;
                case Constants.Saturday:
                    pickUpTime = listEmailService.SaturdayPickUp;
                    break;
                case Constants.Sunday:
                    pickUpTime = listEmailService.SundayPickUp;
                    break;
            }

            if (!string.IsNullOrEmpty(pickUpTime))
            {
                listEmailService.AtPickUp = "at";
            }

            #endregion

            #region Get Day of DropOff for base on end date

            string dropOffDay = Convert.ToString(Convert.ToDateTime(listEmailService.StartDate).DayOfWeek);
            string dropOfftime = null;

            switch (dropOffDay)
            {
                case Constants.Monday:
                    dropOfftime = listEmailService.MondayDropOff;
                    break;
                case Constants.Tuesday:
                    dropOfftime = listEmailService.TuesdayDropOff;
                    break;
                case Constants.Wednesday:
                    dropOfftime = listEmailService.WednesdayDropOff;
                    break;
                case Constants.Thursday:
                    dropOfftime = listEmailService.ThursdayDropOff;
                    break;
                case Constants.Friday:
                    dropOfftime = listEmailService.FridayDropOff;
                    break;
                case Constants.Saturday:
                    dropOfftime = listEmailService.SaturdayDropOff;
                    break;
                case Constants.Sunday:
                    dropOfftime = listEmailService.SundayDropOff;
                    break;
            }
            if (!string.IsNullOrEmpty(dropOfftime))
            {
                listEmailService.AtDropOff = "at";
            }

            #endregion

            #region Set PickUP and Dropoff

            listEmailService.PickUpTime = pickUpTime;
            listEmailService.PickUpDay = Convert.ToString(Convert.ToDateTime(listEmailService.EndDate).DayOfWeek) + " " +
                Convert.ToDateTime(listEmailService.EndDate).ToString(ConfigSettings.ClientSideDateFormat).Replace("-", "/");

            listEmailService.DropOffTime = dropOfftime;
            listEmailService.DropOffDay = Convert.ToString(Convert.ToDateTime(listEmailService.StartDate).DayOfWeek) + " " +
                Convert.ToDateTime(listEmailService.StartDate).ToString(ConfigSettings.ClientSideDateFormat).Replace("-", "/");

            if (!string.IsNullOrEmpty(listEmailService.DropOffImage))
            {
                listEmailService.DropOffImage = ConfigSettings.AmazonS3Url + ConfigSettings.ZarephathBucket + "/" + listEmailService.DropOffImage;
            }
            else
            {
                listEmailService.DropOffImage = _cacheHelper.SiteBaseURL + Constants.NoMapImageUrl;
            }
            if (!string.IsNullOrEmpty(listEmailService.PickUpImage))
            {
                listEmailService.PickUpImage = ConfigSettings.AmazonS3Url + ConfigSettings.ZarephathBucket + "/" + listEmailService.PickUpImage;
            }
            else
            {
                listEmailService.PickUpImage = _cacheHelper.SiteBaseURL + Constants.NoMapImageUrl;
            }

            #endregion
        }

        public ServiceResponse GetScheduleDetailEmailHtml(long scheduleid)
        {
            ServiceResponse response = new ServiceResponse();

            var searchList = new List<SearchValueData>
                {
                    new SearchValueData {Name = "Monthday", Value = Convert.ToString(Constants.MonthValue)},
                    new SearchValueData {Name = "WeekDay", Value = Convert.ToString(Constants.WeekValue)},
                    new SearchValueData {Name = "IsWeekMonthFromService", Value = "0"},

                    new SearchValueData {Name = "ScheduleIds", Value = Convert.ToString(scheduleid)}
                };

            List<ListEmailService> listEmailService = GetEntityList<ListEmailService>(StoredProcedure.GetScheduleEmail, searchList);
            EmailTemplate emailTemplate = GetEntity<EmailTemplate>(new List<SearchValueData>
                                    {
                                        new SearchValueData{Name = "EmailTemplateTypeID",Value =Convert.ToInt16(EnumEmailType.Schedule_Notification).ToString(),IsEqual = true}
                                    });
            string emailbody = "";
            foreach (ListEmailService emailServiceModel in listEmailService)
            {
                //Set Model Values To Send Mail.
                SetEmailDetailTemplateModel(emailServiceModel);
                emailbody = TokenReplace.ReplaceTokens(emailTemplate.EmailTemplateBody, emailServiceModel);
            }
            response.IsSuccess = true;
            response.Data = new ScheduleDetailEmailTemplate
            {
                BodyText = emailbody
            };
            return response;
        }

        public ServiceResponse PrintScheduleNotice(string scheduleIds, bool printCompalsary = false, long? loggedInId = null)
        {
            CacheHelper _cacheHelper = new CacheHelper();
            ServiceResponse response = new ServiceResponse();

            var searchList = new List<SearchValueData>
                {
                    new SearchValueData {Name = "Monthday", Value = Convert.ToString(Constants.MonthValue)},
                    new SearchValueData {Name = "WeekDay", Value = Convert.ToString(Constants.WeekValue)},
                    new SearchValueData {Name = "IsWeekMonthFromService", Value = Convert.ToString(false)},
                    new SearchValueData {Name = "ScheduleIds", Value = Convert.ToString(scheduleIds)}
                };

            List<ListEmailService> listEmailService = GetEntityList<ListEmailService>(StoredProcedure.GetScheduleEmail, searchList);

            if (listEmailService == null)
            {
                listEmailService = new List<ListEmailService>();
            }

            #region Set Email Template

            EmailTemplate emailTemplate = GetEntity<EmailTemplate>(new List<SearchValueData>
                {
                    new SearchValueData
                        {
                            Name = "EmailTemplateTypeID",
                            Value = Convert.ToInt16(EnumEmailType.Schedule_Notification_Notice).ToString(),
                            IsEqual = true
                        }
                });

            #endregion

            #region Set Path
            string respiteNoticePrintFileUploadPath = String.Format(_cacheHelper.RespiteNoticePrintFileUploadPath, _cacheHelper.Domain);

            string path = HttpContext.Current.Server.MapCustomPath(respiteNoticePrintFileUploadPath);
            string fileName = string.Format("{0}_{1}", Constants.ScheduleNotificationNotice, DateTime.Now.ToString(Constants.FileNameDateTimeFormat));
            var downloadFileModel = new DownloadFileModel();

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            downloadFileModel.AbsolutePath = string.Format("{0}{1}{2}", path, fileName, Constants.Extention_pdf);

            downloadFileModel.VirtualPath = string.Format("{0}{1}{2}", respiteNoticePrintFileUploadPath, fileName, Constants.Extention_pdf);
            downloadFileModel.FileName = fileName + Constants.Extention_pdf;

            #endregion

            string pdfHtmlString = "";

            #region GENERATE MASTER LIST FOR NOTICE

            List<ListEmailService> masterList = new List<ListEmailService>();
            List<ListEmailService> masterListForNoSiblings;
            List<ListEmailService> masterListForSiblingsOnly;

            #region  Not Sibling id available this data

            masterListForNoSiblings = listEmailService.Where(x => x.SiblingIDs == null && x.PermissionForMail).ToList();
            masterList.AddRange(masterListForNoSiblings);

            #endregion

            masterListForSiblingsOnly = listEmailService.Where(x => x.SiblingIDs != null && x.PermissionForMail).ToList();

            if (masterListForSiblingsOnly.Count > 0)
            {
                foreach (var model in masterListForSiblingsOnly)
                {
                    if (!model.RemoveINNotice)
                    {
                        List<long> ids = model.SiblingIDs.Split(',').Select(Int64.Parse).ToList();
                        var tempModelList = masterListForSiblingsOnly.Where(x => ids.Contains(x.ReferralID) && x.IsProcessed == false).ToList();

                        foreach (var tempModel in tempModelList)
                        {
                            //&& tempModel.PCMMail && !tempModel.NoticeSent
                            if (tempModel.DropTransportLocationID == model.DropTransportLocationID
                                && tempModel.PickupTransportLocationID == model.PickupTransportLocationID
                                && tempModel.StartDate == model.StartDate && tempModel.EndDate == model.EndDate
                                && tempModel.ParentLastName == model.ParentLastName && tempModel.ParentFirstName == model.ParentFirstName
                                && tempModel.ParentEmail == model.ParentEmail && tempModel.ParentAddress == model.ParentAddress
                                && tempModel.ParentPhone == model.ParentPhone)
                            {
                                //if (printCompalsary || (tempModel.PermissionForMail && tempModel.PCMMail && !tempModel.NoticeSent))
                                if (printCompalsary || (tempModel.PermissionForMail && tempModel.PCMMail))
                                {
                                    tempModel.RemoveINNotice = true;
                                    model.IsProcessed = true;
                                    model.ClientName = model.ClientName + ", " + tempModel.ClientName;
                                }
                            }
                        }
                    }
                }
                masterList.AddRange(masterListForSiblingsOnly);
            }
            var newList = masterList.OrderBy(x => x.ParentName).ToList();

            foreach (var item in newList)
            {
                if (!item.RemoveINNotice)
                {
                    //if (printCompalsary || (item.PermissionForMail && item.PCMMail && !item.NoticeSent))
                    if (printCompalsary || (item.PermissionForMail && item.PCMMail))
                    {
                        SetEmailDetailTemplateModel(item);
                        emailTemplate.EmailTemplateBody = Regex.Replace(emailTemplate.EmailTemplateBody, "<hr(.*?)>", "<hr $1 />");
                        emailTemplate.EmailTemplateBody = Regex.Replace(emailTemplate.EmailTemplateBody, "<br(.*?)>", "<br $1 />");

                        string pdfData = TokenReplace.ReplaceTokens(emailTemplate.EmailTemplateBody, item);
                        pdfHtmlString = pdfHtmlString + pdfData;

                        //When Send SMS that time Update Status for Send SMS for this Schedule
                        #region SAVE Schedle Notification Log
                        SaveScheduleNotificationLogDetails(item.ReferralID, item.ScheduleID, Constants.Notice,
                            Resource.BatchServiceScheduleNotificationNotice, pdfData, true,
                            createdBy: loggedInId);
                        #endregion
                    }
                }
            }

            #endregion

            if (string.IsNullOrEmpty(pdfHtmlString))
            {
                pdfHtmlString = Resource.NoValidNoticeFound;
            }
            Byte[] bytes = Common.ReturnByteArrayFromStringForitextSharpPDF(pdfHtmlString);
            File.WriteAllBytes(downloadFileModel.AbsolutePath, bytes);

            #region Scrap Code

            //using (var ms = new MemoryStream())
            //{
            //    var doc = new Document();

            //    var writer = PdfWriter.GetInstance(doc, ms);
            //    doc.Open();

            //    var example_html = "@" + pdfHtmlString;

            //    using (var srHtml = new StringReader(example_html))
            //    {
            //        iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, srHtml);
            //    }

            //    doc.Close();
            //    bytes = ms.ToArray();
            //    File.WriteAllBytes(downloadFileModel.AbsolutePath, bytes);
            //}

            #endregion

            if (listEmailService.Count > 0)
            {
                foreach (var item in listEmailService)
                {

                    UpdateScheduleMasterNotificaitonDetails(item.ScheduleID, Constants.Notice);
                }
            }

            response.Data = downloadFileModel;
            response.IsSuccess = true;

            return response;
        }

        #endregion

        #region Update Email,SMS,Genrate Notice Status

        public void UpdateScheduleMasterNotificaitonDetails(long scheduleId, string type, bool windowService = false)
        {
            ScheduleMaster scheduleMaster = GetEntity<ScheduleMaster>(scheduleId);

            if (type == Constants.Email)
            {
                scheduleMaster.EmailSent = true;
                if (windowService) scheduleMaster.WeekEmailDate = DateTime.UtcNow;
            }
            else if (type == Constants.SMS)
            {
                scheduleMaster.SMSSent = true;
                if (windowService) scheduleMaster.WeekSMSDate = DateTime.UtcNow;
            }
            else if (type == Constants.Notice)
            {
                scheduleMaster.NoticeSent = true;
            }
            SaveObject(scheduleMaster);
        }


        #endregion


        #region Home Care Data Provider

        public ServiceResponse HC_SetScheduleAssignmentModel()
        {
            var response = new ServiceResponse();

            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData
                        {
                            Name = "IgnoreFrequency",
                            Value = Convert.ToString((int) FrequencyCode.FrequencyCodes.DNS)
                        }
                };
            HC_ScheduleAssignmentModel scheduleAssignmentModel = GetMultipleEntity<HC_ScheduleAssignmentModel>(StoredProcedure.HC_SetScheduleAssignmentModel, searchParam);
            scheduleAssignmentModel.CancellationReasons = Common.SetCancellationReasons();

            if (scheduleAssignmentModel.RegionList.Any())
                scheduleAssignmentModel.ScheduleSearchModel.RegionID = scheduleAssignmentModel.RegionList[0].RegionID;
            scheduleAssignmentModel.ScheduleSearchModel.StartDate = Common.GetOrgStartOfWeek();
            scheduleAssignmentModel.ScheduleSearchModel.EndDate = Common.GetOrgStartOfWeek().AddDays(6);



            response.IsSuccess = true;
            response.Data = scheduleAssignmentModel;
            return response;
        }

        public ServiceResponse HC_GetReferralListForSchedule(SearchReferralListForSchedule searchReferralModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchReferralModel != null)
                SetSearchFilterForReferralList(searchReferralModel, searchList, loggedInId);

            Page<ReferralListForSchedule> page = GetEntityPageList<ReferralListForSchedule>(StoredProcedure.HC_GetReferralListForScheduling, searchList, pageSize, pageIndex, sortIndex, sortDirection);
            response.Data = page;
            response.IsSuccess = true;
            return response;
        }


        public ServiceResponse HC_GetReferralDetailForPopup(long referralID)
        {
            var response = new ServiceResponse();
            ReferralDetailForPopup detailModel = GetMultipleEntity<ReferralDetailForPopup>(StoredProcedure.HC_GetReferralDetailForPopup,
                new List<SearchValueData>{
                        new SearchValueData{Name = "ReferralID",Value = referralID.ToString()},
                        new SearchValueData { Name = "ContactTypeID", Value = ((int)Common.ContactTypes.PrimaryPlacement).ToString() }
                    });

            response.Data = detailModel;
            response.IsSuccess = true;
            return response;
        }


        public ServiceResponse GetEmployeesForSchedulingURL(long referralID, string employeeName)
        {
            var response = new ServiceResponse();
            List<EmployeeSchModel> detailModel = GetEntityList<EmployeeSchModel>(StoredProcedure.HC_GetEmployeesForScheduling,
                new List<SearchValueData>{
                        new SearchValueData{Name = "ReferralID",Value = referralID.ToString()},
                        new SearchValueData{Name = "EmployeeName",Value = employeeName},
                        new SearchValueData{Name = "PreferenceType_Prefernce",Value = Convert.ToString(Preference.PreferenceKeyType.Preference)},
                        new SearchValueData{Name = "PreferenceType_Skill",Value = Convert.ToString(Preference.PreferenceKeyType.Skill)}
                    });

            response.Data = detailModel;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse GetEmployeesForEmpCalender(string employeeIDs)
        {
            var response = new ServiceResponse();
            List<EmployeeSchModel> detailModel = GetEntityList<EmployeeSchModel>(StoredProcedure.HC_GetEmployeesForEmpCalender,
                new List<SearchValueData>{
                        new SearchValueData{Name = "EmployeeIDs",Value = employeeIDs}
                    });

            response.Data = detailModel;
            response.IsSuccess = true;
            return response;
        }


        public ServiceResponse GetEmployeeMatchingPreferences(long employeeID, long referralID)
        {
            var response = new ServiceResponse();
            EmployeeDetailSchModel detailModel = GetMultipleEntity<EmployeeDetailSchModel>(StoredProcedure.HC_GetEmployeeMatchingPreferences,
                new List<SearchValueData>{
                        new SearchValueData{Name = "ReferralID",Value = referralID.ToString()},
                        new SearchValueData{Name = "EmployeeID",Value = employeeID.ToString()}
                    });

            response.Data = detailModel;
            response.IsSuccess = true;
            return response;
        }


        public ServiceResponse HC_PrivateDuty_GetEmployeeMatchingPreferences(long employeeID, long referralID)
        {
            var response = new ServiceResponse();
            EmployeeDetailSchModel detailModel = GetMultipleEntity<EmployeeDetailSchModel>(StoredProcedure.HC_PrivateDuty_GetEmployeeMatchingPreferences,
                new List<SearchValueData>{
                        new SearchValueData{Name = "ReferralID",Value = referralID.ToString()},
                        new SearchValueData{Name = "EmployeeID",Value = employeeID.ToString()}
                    });

            response.Data = detailModel;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse GetScheduleListByEmployees(SearchScheduleListByFacility searchPara)
        {
            var response = new ServiceResponse();
            //Code For Getting Schedule List..

            string emps = "";
            if (searchPara.EmployeeIDs != null && searchPara.EmployeeIDs.Count > 0)
                emps = String.Join(",", searchPara.EmployeeIDs);

            string refs = "";
            if (searchPara.ReferralIDs != null && searchPara.ReferralIDs.Count > 0)
                refs = String.Join(",", searchPara.ReferralIDs);

            var detailModel = GetMultipleEntity<EmployeeScheduleDetails>(StoredProcedure.HC_GetEmployeeScheduleDetails, new List<SearchValueData>
            {
                new SearchValueData{Name = "EmployeeIDs",Value = emps},
                new SearchValueData{Name = "ReferralIDs",Value = refs},
                new SearchValueData{Name = "StartDate",Value = searchPara.StartDate.ToString(Constants.DbDateFormat)},
                new SearchValueData{Name = "EndDate",Value = searchPara.EndDate.ToString(Constants.DbDateFormat)}
            });


            //detailModel.ScheduleSearchModel.StartDate = searchPara.StartDate;
            //detailModel.ScheduleSearchModel.EndDate = searchPara.EndDate;

            response.Data = detailModel;
            response.IsSuccess = true;
            return response;
        }


        public ServiceResponse HC_SaveScheduleMasterFromCalender(ScheduleAssignmentModel scheduleAssignment, long loggedInUserID)
        {
            ScheduleMaster scheduleMaster;
            if (scheduleAssignment.ScheduleMaster.ScheduleID == 0)
            {

                Referral referral = GetEntity<Referral>(scheduleAssignment.ScheduleMaster.ReferralID);

                if (referral == null || referral.ReferralID == 0)
                {
                    return new ServiceResponse
                    {
                        IsSuccess = false,
                        Message = Resource.ErrorOccured
                    };
                }
                scheduleAssignment.ScheduleMaster.DropOffLocation = referral.DropOffLocation.Value;
                scheduleAssignment.ScheduleMaster.PickUpLocation = referral.PickUpLocation.Value;
                scheduleAssignment.ScheduleMaster.ScheduleStatusID = (int)ScheduleStatus.ScheduleStatuses.Unconfirmed;

            }


            //if (scheduleAssignment.ScheduleMaster.FacilityID.HasValue)
            //{
            //    Facility facility = GetEntity<Facility>(scheduleAssignment.ScheduleMaster.FacilityID.Value);
            //    if (facility != null && facility.FacilityID != 0 && facility.DefaultScheduleStatusID.HasValue)
            //    {
            //        scheduleAssignment.ScheduleMaster.ScheduleStatusID = facility.DefaultScheduleStatusID.Value;
            //    }
            //}



            ServiceResponse response = HC_SaveScheduleMaster(scheduleAssignment.ScheduleMaster, loggedInUserID);


            return response;
        }


        public ServiceResponse HC_SaveScheduleMaster(ScheduleMaster scheduleMaster, long loggedInUserID)
        {
            var response = new ServiceResponse();

            bool isEditing = scheduleMaster.ScheduleID > 0;

            // Code Validate Schedule Conflict And Other Valication
            ServiceResponse validateResponse = HC_ValidateSchedule(scheduleMaster, loggedInUserID);
            if (!validateResponse.IsSuccess)
            {
                return validateResponse;
            }
            //ValidateScheduleMasterModel validateScheduleModel = (ValidateScheduleMasterModel)validateResponse.Data;
            // Code Validate Schedule Conflict And Other Validation


            SaveObject(scheduleMaster, loggedInUserID);

            //SET LAST ATTENDANCE AS LAST CONFIRMED STATUS DATE
            GetScalar(StoredProcedure.UpdateReferralLastAttDate,
            new List<SearchValueData> { new SearchValueData { Name = "ReferralID", Value = scheduleMaster.ReferralID.ToString() },
                        new SearchValueData { Name = "ScheduleStatusID", Value = ((int)ScheduleStatus.ScheduleStatuses.Confirmed).ToString() } });


            response.Data = scheduleMaster;

            //if (scheduleMaster.ScheduleStatusID == ((int)ScheduleStatus.ScheduleStatuses.Confirmed)
            //          && validateScheduleModel.OutOfBadCapacityCount > 0)
            //{
            //    //If Current Schedule is Confirmaed And Facility Capacity is Full Show Warning Messages.
            //    response.Message = Resource.FacilityOutOfCapacity;
            //    response.ErrorCode = Constants.ErrorCode_Warning;
            //    response.IsSuccess = true;
            //    return response;
            //}
            //if (validateScheduleModel.OutOfRoomCapacityCount > 0 && scheduleMaster.ScheduleStatusID == ((int)ScheduleStatus.ScheduleStatuses.Confirmed))
            //{
            //    //In Case Of Room capacity is full Show Warning Message.
            //    response.ErrorCode = Constants.ErrorCode_Warning;
            //    response.Message = Resource.ScheduledSavedSuccessfullyWithWarning;
            //}
            //else
            //{
            //    response.Message = isEditing ? Resource.ScheduledUpdatedSuccessfully : Resource.ScheduledAddedSuccessfully;
            //}

            response.Message = isEditing ? Resource.ScheduledUpdatedSuccessfully : Resource.ScheduledAddedSuccessfully;

            response.IsSuccess = true;

            return response;
        }

        private ServiceResponse HC_ValidateSchedule(ScheduleMaster scheduleMaster, long loggedInUserID)
        {
            var response = new ServiceResponse();
            if (scheduleMaster.ScheduleID > 0)
            {
                ScheduleMaster oldschedule = GetEntity<ScheduleMaster>(scheduleMaster.ScheduleID);

                if ((oldschedule.StartDate != scheduleMaster.StartDate || oldschedule.EndDate != scheduleMaster.EndDate) &&
                    (oldschedule.IsAssignedToTransportationGroupUp || oldschedule.IsAssignedToTransportationGroupDown))
                {

                    //In Case of Edit Schedule If Client is already assigned for transportation for that schedule then.
                    // If Client Select to Remove transportation assignment then ststem will remove transportation assignment.
                    #region RemoveTransportation Assignment
                    if (scheduleMaster.TransportationAssignmentAction == Constants.TransportationAssignmentRemoveAction)
                    {
                        ITransportationGroupDataProvider transportationGroupData = new TransportationGroupDataProvider();
                        TransportationGroupClient transportationGroupClient = null;

                        List<SearchValueData> searchValue = new List<SearchValueData>
                                                        {
                                                            new SearchValueData
                                                                {
                                                                    Name = "ScheduleID",
                                                                    Value = scheduleMaster.ScheduleID.ToString()
                                                                }
                                                        };

                        if (oldschedule.StartDate != scheduleMaster.StartDate)
                        {
                            searchValue.Add(new SearchValueData
                            {
                                Name = "TripDirection",
                                Value = TransportationGroup.TripDirectionUp
                            });
                            scheduleMaster.IsAssignedToTransportationGroupUp = false;
                            scheduleMaster.IsAssignedToTransportationGroupDown = false;

                        }
                        else if (oldschedule.EndDate != scheduleMaster.EndDate)
                        {
                            searchValue.Add(new SearchValueData
                            {
                                Name = "TripDirection",
                                Value = TransportationGroup.TripDirectionDown
                            });
                            scheduleMaster.IsAssignedToTransportationGroupDown = false;
                        }

                        transportationGroupClient = GetEntity<TransportationGroupClient>(StoredProcedure.GetTransportationGroupClientByScheduleID, searchValue);
                        if (transportationGroupClient != null)
                        {
                            transportationGroupData.RemoveTransportationGroupClient(
                                transportationGroupClient.TransportationGroupClientID, loggedInUserID);


                        }

                    }
                    else if (scheduleMaster.TransportationAssignmentAction == Constants.TransportationAssignmentKeepAction)
                    {


                    }
                    else
                    {
                        //As Per Client Request System will not restrict for trmoving transportation group on schedule date change 
                        response.Message = Resource.Youcannotchangescheduledate;
                        response.ErrorCode = Constants.ErrorCode_Warning;
                        response.Data = oldschedule;
                        response.IsSuccess = false;
                        return response;
                    }
                    #endregion RemoveTransportation Assignment


                    //As Per Client Request System will not restrict for trmoving transportation group on schedule date change 
                    //response.Message = Resource.Youcannotchangescheduledate;
                    //response.IsSuccess = false;
                    //return response;
                }
            }

            HC_ValidateScheduleMasterModel scheduleCount = GetEntity<HC_ValidateScheduleMasterModel>(StoredProcedure.HC_CheckScheduleConflict, new List<SearchValueData>
                {
                    new SearchValueData
                        {
                            Name = "ScheduleID",
                            Value = scheduleMaster.ScheduleID.ToString()
                        },
                    new SearchValueData
                        {
                            Name = "ReferralID",
                            Value = scheduleMaster.ReferralID.ToString()
                        },
                    new SearchValueData
                        {
                            Name    = "EmployeeID",
                            Value = scheduleMaster.EmployeeID.ToString()
                        },
                     new SearchValueData
                        {
                            Name = "StartDate",
                            Value = scheduleMaster.StartDate.ToString(Constants.DbDateTimeFormat)
                        },
                        new SearchValueData
                        {
                            Name = "EndDate",
                            Value = scheduleMaster.EndDate.ToString(Constants.DbDateTimeFormat)
                        },

                });
            if (scheduleCount.ScheduleConflictCount > 0)
            {
                response.IsSuccess = false;
                response.Message = Resource.ClientAlreadyScheduledForThisTime;
                return response;
            }

            if (scheduleCount.PatientPreferenceCount > 0)
            {
                response.IsSuccess = false;
                response.Message = Resource.ClientIsNotAllowedOnThisDay;
                return response;
            }

            //if (scheduleMaster.ScheduleStatusID == ((int)ScheduleStatus.ScheduleStatuses.Confirmed)
            //           && scheduleCount.OutOfBadCapacityCount > 0)
            //{
            //    //If Current Schedule is Confirmaed And Facility Capacity is Full Show Error Messages.
            //    response.IsSuccess = false;
            //    response.Message = Resource.FacilityOutOfCapacity;
            //    return response;
            //}

            //if (scheduleCount.OutOfRoomCapacityCount>0)
            //{
            //    response.IsSuccess = true;
            //    response.Message = Resource.FacilityOutOfCapacity;
            //    return response;
            //}

            response.IsSuccess = true;
            response.Data = scheduleCount;
            return response;
        }


        public ServiceResponse HC_UpdateScheduleFromScheduleList(SearchScheduleMasterModel searchScheduleMasterModel, ScheduleMaster scheduleMaster, int pageIndex, int pageSize, string sortIndex,
                                            string sortDirection, long loggedInUserID, string sortIndexArray)
        {
            ServiceResponse response = new ServiceResponse();
            if (scheduleMaster.ScheduleID > 0)
            {
                ScheduleMaster tempScheduleMaster = GetEntity<ScheduleMaster>(scheduleMaster.ScheduleID);
                if (tempScheduleMaster != null)
                {
                    //As per Client Request removed this check.
                    //if (scheduleMaster.ScheduleStatusID != tempScheduleMaster.ScheduleStatusID && tempScheduleMaster.ScheduleStatusID == (int)ScheduleStatus.ScheduleStatuses.Confirmed && (tempScheduleMaster.IsAssignedToTransportationGroupDown || tempScheduleMaster.IsAssignedToTransportationGroupUp))
                    //{
                    //    response.Message = Resource.Youcannotchangethestatusasclient;
                    //    return response;
                    //}


                    //If Status Is No Show,No Confirmation and Cancel and it is assigned to transportaion group then remove from assignment.
                    //And Also Make FacilityID to Null
                    if (scheduleMaster.ScheduleStatusID != tempScheduleMaster.ScheduleStatusID && (scheduleMaster.ScheduleStatusID == (int)ScheduleStatus.ScheduleStatuses.No_Show || scheduleMaster.ScheduleStatusID == (int)ScheduleStatus.ScheduleStatuses.No_Confirmation || scheduleMaster.ScheduleStatusID == (int)ScheduleStatus.ScheduleStatuses.Cancelled))
                    {

                        //Get Transportationgroup List for this schedule

                        List<TransportationGroupClient> assignedGroup =
                            GetEntityList<TransportationGroupClient>(new List<SearchValueData>
                                {
                                    new SearchValueData
                                        {
                                            Name = "ScheduleID",
                                            Value = scheduleMaster.ScheduleID.ToString()
                                        }
                                });

                        ITransportationGroupDataProvider transportationGroupDataProvider =
                            new TransportationGroupDataProvider();

                        foreach (TransportationGroupClient transportationGroupClient in assignedGroup)
                        {
                            transportationGroupDataProvider.RemoveTransportationGroupClient(
                                transportationGroupClient.TransportationGroupClientID, loggedInUserID);
                        }


                        //Setting schedulemaster to null.
                        scheduleMaster.FacilityID = null;
                        scheduleMaster.EmployeeID = null;

                        //scheduleMaster.EmployeeID = null;
                        //response.Message = Resource.Youcannotchangethestatusasclient;
                        //return response;

                        tempScheduleMaster.IsAssignedToTransportationGroupDown = false;
                        tempScheduleMaster.IsAssignedToTransportationGroupUp = false;
                    }



                    tempScheduleMaster.ScheduleStatusID = scheduleMaster.ScheduleStatusID;
                    tempScheduleMaster.PickUpLocation = scheduleMaster.PickUpLocation;
                    tempScheduleMaster.DropOffLocation = scheduleMaster.DropOffLocation;
                    tempScheduleMaster.Comments = scheduleMaster.Comments;
                    tempScheduleMaster.FacilityID = scheduleMaster.FacilityID;
                    tempScheduleMaster.EmployeeID = scheduleMaster.EmployeeID;

                    //If Status is not Cancelled then reset value.
                    if (scheduleMaster.ScheduleStatusID != (int)ScheduleStatus.ScheduleStatuses.Cancelled)
                    {
                        tempScheduleMaster.WhoCancelled = null;
                        tempScheduleMaster.WhenCancelled = null;
                        tempScheduleMaster.CancelReason = null;
                    }
                    else //If Status is Cancelled then Set values.
                    {
                        tempScheduleMaster.Comments = scheduleMaster.CancelReason;
                        tempScheduleMaster.WhoCancelled = scheduleMaster.WhoCancelled;
                        tempScheduleMaster.WhenCancelled = scheduleMaster.WhenCancelled;
                        tempScheduleMaster.CancelReason = scheduleMaster.CancelReason;
                    }



                    ServiceResponse res = HC_SaveScheduleMaster(tempScheduleMaster, loggedInUserID);
                    if (res.IsSuccess)
                    {
                        response.IsSuccess = true;
                        searchScheduleMasterModel.ScheduleID = scheduleMaster.ScheduleID;
                        response.Data = HC_GetScheduleMasterList(searchScheduleMasterModel, 1, pageSize, sortIndex, sortDirection, loggedInUserID, sortIndexArray).Data;
                        if (res.ErrorCode == Constants.ErrorCode_Warning)
                        {
                            response.ErrorCode = res.ErrorCode;
                            response.Message = res.Message;
                        }
                        else
                        {
                            response.Message = Resource.ScheduleDetailUpdated;
                        }
                    }
                    else
                    {
                        return res;
                    }
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






        public ServiceResponse HC_SetScheduleMasterPage()
        {
            var response = new ServiceResponse();

            HC_ScheduleMasterModel setScheduleMasterModel = GetMultipleEntity<HC_ScheduleMasterModel>(StoredProcedure.HC_SetScheduleMasterPage, new List<SearchValueData>());
            setScheduleMasterModel.CancellationReasons = Common.SetCancellationReasons();
            setScheduleMasterModel.AddScheduleBatchService = new ScheduleBatchService
            {
                ScheduleBatchServiceStatus = ScheduleBatchService.ScheduleBatchServiceStatuses.Initiated.ToString()
            };
            setScheduleMasterModel.ScheduleBatchServiceTypeList = Enum.GetNames(typeof(ScheduleBatchService.ScheduleBatchServiceTypes)).Select(m => new NameValueDataInString()
            {
                Name = m,
                Value = m
            }).ToList();

            setScheduleMasterModel.PreferredCommunicationMethod = Common.SetPreferredCommunicationMethod();
            response.IsSuccess = true;
            response.Data = setScheduleMasterModel;
            return response;
        }



        public ServiceResponse HC_DayCare_SetScheduleMasterPage()
        {
            var response = new ServiceResponse();

            HC_ScheduleMasterModel setScheduleMasterModel = GetMultipleEntity<HC_ScheduleMasterModel>(StoredProcedure.HC_DayCare_SetScheduleMasterPage, new List<SearchValueData>());
            setScheduleMasterModel.CancellationReasons = Common.SetCancellationReasons();
            setScheduleMasterModel.AddScheduleBatchService = new ScheduleBatchService
            {
                ScheduleBatchServiceStatus = ScheduleBatchService.ScheduleBatchServiceStatuses.Initiated.ToString()
            };
            setScheduleMasterModel.ScheduleBatchServiceTypeList = Enum.GetNames(typeof(ScheduleBatchService.ScheduleBatchServiceTypes)).Select(m => new NameValueDataInString()
            {
                Name = m,
                Value = m
            }).ToList();

            setScheduleMasterModel.PreferredCommunicationMethod = Common.SetPreferredCommunicationMethod();
            response.IsSuccess = true;
            response.Data = setScheduleMasterModel;
            return response;
        }


        public ServiceResponse HC_GetScheduleMasterList(SearchScheduleMasterModel searchScheduleMasterModel, int pageIndex, int pageSize, string sortIndex, string sortDirection,
                                                     long loggedInId, string sortIndexArray = "")
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchScheduleMasterModel != null)
                HC_SetSearchFilterForScheduleMasterList(searchScheduleMasterModel, searchList, loggedInId);

            Page<ScheduleMasterList> page = GetEntityPageList<ScheduleMasterList>(StoredProcedure.HC_GetScheduleMaster, searchList, pageSize, pageIndex, sortIndex, sortDirection, sortIndexArray);
            response.Data = page;
            response.IsSuccess = true;
            return response;
        }

        private static void HC_SetSearchFilterForScheduleMasterList(SearchScheduleMasterModel searchScheduleMasterModel, List<SearchValueData> searchList, long loggedInId)
        {
            if (searchScheduleMasterModel.StartDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = searchScheduleMasterModel.StartDate.Value.ToString(Constants.DbDateFormat) });

            if (searchScheduleMasterModel.EndDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = searchScheduleMasterModel.EndDate.Value.ToString(Constants.DbDateFormat) });


            if (searchScheduleMasterModel.EndDate == null && searchScheduleMasterModel.StartDate == null && !searchScheduleMasterModel.IsPartial)
            {
                searchList.Add(new SearchValueData { Name = "StartDate", Value = DateTime.UtcNow.Date.ToString(Constants.DbDateFormat) });
            }

            if (searchScheduleMasterModel.ScheduleStatusID > 0)
                searchList.Add(new SearchValueData
                {
                    Name = "ScheduleStatusID",
                    Value = Convert.ToString(searchScheduleMasterModel.ScheduleStatusID)
                });

            if (searchScheduleMasterModel.ScheduleID > 0)
                searchList.Add(new SearchValueData
                {
                    Name = "ScheduleID",
                    Value = Convert.ToString(searchScheduleMasterModel.ScheduleID)
                });

            if (!string.IsNullOrEmpty(searchScheduleMasterModel.Name))
                searchList.Add(new SearchValueData
                {
                    Name = "Name",
                    Value = searchScheduleMasterModel.Name
                });

            if (!string.IsNullOrEmpty(searchScheduleMasterModel.ParentName))
                searchList.Add(new SearchValueData
                {
                    Name = "ParentName",
                    Value = searchScheduleMasterModel.ParentName
                });

            if (searchScheduleMasterModel.DropOffLocation > 0)
                searchList.Add(new SearchValueData
                {
                    Name = "DropOffLocation",
                    Value = Convert.ToString(searchScheduleMasterModel.DropOffLocation)
                });

            if (searchScheduleMasterModel.FacilityID > 0)
                searchList.Add(new SearchValueData
                {
                    Name = "FacilityID",
                    Value = Convert.ToString(searchScheduleMasterModel.FacilityID)
                });

            if (searchScheduleMasterModel.EmployeeID > 0)
                searchList.Add(new SearchValueData
                {
                    Name = "EmployeeID",
                    Value = Convert.ToString(searchScheduleMasterModel.EmployeeID)
                });

            if (searchScheduleMasterModel.RegionID > 0)
                searchList.Add(new SearchValueData
                {
                    Name = "RegionID",
                    Value = Convert.ToString(searchScheduleMasterModel.RegionID)
                });

            if (searchScheduleMasterModel.LanguageID > 0)
                searchList.Add(new SearchValueData
                {
                    Name = "LanguageID",
                    Value = Convert.ToString(searchScheduleMasterModel.LanguageID)
                });

            if (!string.IsNullOrEmpty(searchScheduleMasterModel.ReferralID))
            {
                searchList.Add(new SearchValueData
                {
                    Name = "ReferralID",
                    Value = Convert.ToString(Crypto.Decrypt(searchScheduleMasterModel.ReferralID))
                });
            }

            if (searchScheduleMasterModel.PreferredCommunicationMethodID > 0)
                searchList.Add(new SearchValueData
                {
                    Name = "PreferredCommunicationMethodID",
                    Value = Convert.ToString(searchScheduleMasterModel.PreferredCommunicationMethodID)
                });

        }




        public ServiceResponse HC_DayCare_GetScheduleMasterList(SearchScheduleMasterModel searchScheduleMasterModel, int pageIndex, int pageSize, string sortIndex, string sortDirection,
                                                     long loggedInId, string sortIndexArray = "")
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchScheduleMasterModel != null)
                HC_DayCare_SetSearchFilterForScheduleMasterList(searchScheduleMasterModel, searchList, loggedInId);

            Page<ScheduleMasterList> page = GetEntityPageList<ScheduleMasterList>(StoredProcedure.HC_DayCare_GetScheduleMaster, searchList, pageSize, pageIndex, sortIndex, sortDirection, sortIndexArray);
            response.Data = page;
            response.IsSuccess = true;
            return response;
        }

        private static void HC_DayCare_SetSearchFilterForScheduleMasterList(SearchScheduleMasterModel searchScheduleMasterModel, List<SearchValueData> searchList, long loggedInId)
        {
            if (searchScheduleMasterModel.StartDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = searchScheduleMasterModel.StartDate.Value.ToString(Constants.DbDateFormat) });

            if (searchScheduleMasterModel.EndDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = searchScheduleMasterModel.EndDate.Value.ToString(Constants.DbDateFormat) });


            if (searchScheduleMasterModel.EndDate == null && searchScheduleMasterModel.StartDate == null && !searchScheduleMasterModel.IsPartial)
            {
                searchList.Add(new SearchValueData { Name = "StartDate", Value = DateTime.UtcNow.Date.ToString(Constants.DbDateFormat) });
            }

            if (searchScheduleMasterModel.ScheduleStatusID > 0)
                searchList.Add(new SearchValueData
                {
                    Name = "ScheduleStatusID",
                    Value = Convert.ToString(searchScheduleMasterModel.ScheduleStatusID)
                });

            if (searchScheduleMasterModel.ScheduleID > 0)
                searchList.Add(new SearchValueData
                {
                    Name = "ScheduleID",
                    Value = Convert.ToString(searchScheduleMasterModel.ScheduleID)
                });

            if (!string.IsNullOrEmpty(searchScheduleMasterModel.Name))
                searchList.Add(new SearchValueData
                {
                    Name = "Name",
                    Value = searchScheduleMasterModel.Name
                });

            if (!string.IsNullOrEmpty(searchScheduleMasterModel.ParentName))
                searchList.Add(new SearchValueData
                {
                    Name = "ParentName",
                    Value = searchScheduleMasterModel.ParentName
                });

            if (searchScheduleMasterModel.DropOffLocation > 0)
                searchList.Add(new SearchValueData
                {
                    Name = "DropOffLocation",
                    Value = Convert.ToString(searchScheduleMasterModel.DropOffLocation)
                });

            if (searchScheduleMasterModel.FacilityID > 0)
                searchList.Add(new SearchValueData
                {
                    Name = "FacilityID",
                    Value = Convert.ToString(searchScheduleMasterModel.FacilityID)
                });

            if (searchScheduleMasterModel.EmployeeID > 0)
                searchList.Add(new SearchValueData
                {
                    Name = "EmployeeID",
                    Value = Convert.ToString(searchScheduleMasterModel.EmployeeID)
                });

            if (searchScheduleMasterModel.RegionID > 0)
                searchList.Add(new SearchValueData
                {
                    Name = "RegionID",
                    Value = Convert.ToString(searchScheduleMasterModel.RegionID)
                });

            if (searchScheduleMasterModel.LanguageID > 0)
                searchList.Add(new SearchValueData
                {
                    Name = "LanguageID",
                    Value = Convert.ToString(searchScheduleMasterModel.LanguageID)
                });

            if (!string.IsNullOrEmpty(searchScheduleMasterModel.ReferralID))
            {
                searchList.Add(new SearchValueData
                {
                    Name = "ReferralID",
                    Value = Convert.ToString(Crypto.Decrypt(searchScheduleMasterModel.ReferralID))
                });
            }

            if (searchScheduleMasterModel.PreferredCommunicationMethodID > 0)
                searchList.Add(new SearchValueData
                {
                    Name = "PreferredCommunicationMethodID",
                    Value = Convert.ToString(searchScheduleMasterModel.PreferredCommunicationMethodID)
                });


            if (!string.IsNullOrEmpty(searchScheduleMasterModel.AttendanceStatus))
            {
                searchList.Add(new SearchValueData
                {
                    Name = "AttendanceStatus",
                    Value = Convert.ToString(searchScheduleMasterModel.AttendanceStatus)
                });
            }

        }


        #region Checklist SMS/Notification

        public ServiceResponse ChecklistGetEmpSMSDetail(long scheduleId, int templateId)
        {
            var response = new ServiceResponse();

            #region Getting List And SMS Template Model

            var searchList = new List<SearchValueData> { new SearchValueData { Name = "ScheduleID", Value = Convert.ToString(scheduleId) }, };

            HC_ListScheduleEmail listScheduleEmail = GetEntity<HC_ListScheduleEmail>(StoredProcedure.HC_ChecklistGetScheduleEmailDetail, searchList);

            EmailTemplate emailTemplate;

            if (templateId != 0)
            {
                emailTemplate = GetEntity<EmailTemplate>(new List<SearchValueData> { new SearchValueData { Name = "EmailTemplateTypeID", Value = templateId.ToString() } });
            }
            else
            {
                emailTemplate = GetEntity<EmailTemplate>(new List<SearchValueData> {
                    new SearchValueData { Name = "EmailTemplateTypeID", Value = ((int)EnumEmailType.HomeCare_Schedule_Notification_SMS).ToString() } });
            }

            #endregion

            if (emailTemplate != null && listScheduleEmail != null)
            {

                #region Replace the The Content for SMS Model

                ScheduleMasterModel scheduleMasterModel = new ScheduleMasterModel();
                scheduleMasterModel.ScheduleSmsModel.ToNumber = listScheduleEmail.EmployeePhone;
                scheduleMasterModel.ScheduleSmsModel.ScheduleID = listScheduleEmail.ScheduleID;
                scheduleMasterModel.ScheduleSmsModel.StartDate = listScheduleEmail.StartDate;
                scheduleMasterModel.ScheduleSmsModel.Body = TokenReplace.ReplaceTokens(emailTemplate.EmailTemplateBody, listScheduleEmail); //"This is a Dummy Schedule Text. Lorem Ipsum is simply dummy text of the printing and typesetting industry."; 

                #endregion

                response.IsSuccess = true;
                response.Data = scheduleMasterModel.ScheduleSmsModel;

            }
            return response;
        }

        public ServiceResponse ChecklistSendEmpSMS(ScheduleSmsModel scheduleSmsModel, long loggedInUserID)
        {
            string emailtype = Convert.ToString(EnumEmailType.HomeCare_MonthlyChecklist_Notification_SMS);
            var response = new ServiceResponse();

            bool isSentSms = Common.SendSms(scheduleSmsModel.ToNumber, scheduleSmsModel.Body, emailtype);

            response.IsSuccess = isSentSms;
            response.Message = isSentSms ? Resource.SMSSentSuccess : Resource.SMSSentFail;
            return response;
        }

        #endregion


        public ServiceResponse GetEmpSMSDetail(long scheduleId, int templateId)
        {
            var response = new ServiceResponse();

            #region Getting List And SMS Template Model

            var searchList = new List<SearchValueData> { new SearchValueData { Name = "ScheduleID", Value = Convert.ToString(scheduleId) }, };

            HC_ListScheduleEmail listScheduleEmail = GetEntity<HC_ListScheduleEmail>(StoredProcedure.HC_GetScheduleEmailDetail, searchList);

            EmailTemplate emailTemplate = GetEntity<EmailTemplate>(new List<SearchValueData> { new SearchValueData
            { Name = "EmailTemplateTypeID", Value = ((int)EnumEmailType.HomeCare_Schedule_Notification_SMS).ToString() } });

            if (templateId != 0)
            {
                emailTemplate = GetEntity<EmailTemplate>(new List<SearchValueData> { new SearchValueData { Name = "EmailTemplateTypeID", Value = templateId.ToString() } });
            }

            #endregion

            if (emailTemplate != null && listScheduleEmail != null)
            {

                #region Replace the The Content for SMS Model

                ScheduleMasterModel scheduleMasterModel = new ScheduleMasterModel();
                scheduleMasterModel.ScheduleSmsModel.ToNumber = listScheduleEmail.EmployeePhone;
                scheduleMasterModel.ScheduleSmsModel.ScheduleID = listScheduleEmail.ScheduleID;
                scheduleMasterModel.ScheduleSmsModel.StartDate = listScheduleEmail.StartDate;
                scheduleMasterModel.ScheduleSmsModel.Body = TokenReplace.ReplaceTokens(emailTemplate.EmailTemplateBody, listScheduleEmail); //"This is a Dummy Schedule Text. Lorem Ipsum is simply dummy text of the printing and typesetting industry."; 

                #endregion

                response.IsSuccess = true;
                response.Data = scheduleMasterModel.ScheduleSmsModel;

            }
            return response;
        }

        public ServiceResponse SendEmpSMS(ScheduleSmsModel scheduleSmsModel, long loggedInUserID)
        {
            string emailtype = Convert.ToString(EnumEmailType.Schedule_Notification_SMS);
            var response = new ServiceResponse();

            bool isSentSms = Common.SendSms(scheduleSmsModel.ToNumber, scheduleSmsModel.Body, emailtype);

            response.IsSuccess = isSentSms;
            response.Message = isSentSms ? Resource.SMSSentSuccess : Resource.SMSSentFail;
            return response;
        }


        public ServiceResponse HC_GetEmpEmailDetail(long scheduleId)
        {
            var response = new ServiceResponse();

            #region Getting List And Email Template Model

            var searchList = new List<SearchValueData> { new SearchValueData { Name = "ScheduleID", Value = Convert.ToString(scheduleId) }, };

            HC_ListScheduleEmail listScheduleEmail = GetEntity<HC_ListScheduleEmail>(StoredProcedure.HC_GetScheduleEmailDetail, searchList);

            EmailTemplate emailTemplate = GetEntity<EmailTemplate>(new List<SearchValueData> { new SearchValueData { Name = "EmailTemplateTypeID", Value = ((int)EnumEmailType.HomeCare_Schedule_Notification).ToString() } });
            listScheduleEmail.HomeCareLogoImage = _cacheHelper.SiteBaseURL + _cacheHelper.TemplateLogo;
            listScheduleEmail.SiteName = _cacheHelper.SiteName;
            #endregion

            if (emailTemplate != null && listScheduleEmail != null)
            {
                ScheduleMasterModel scheduleMasterModel = new ScheduleMasterModel();

                scheduleMasterModel.ScheduleEmailModel.ScheduleID = listScheduleEmail.ScheduleID;
                scheduleMasterModel.ScheduleEmailModel.Email = listScheduleEmail.EmployeeEmail;
                scheduleMasterModel.ScheduleEmailModel.StartDate = listScheduleEmail.StartDate;

                scheduleMasterModel.ScheduleEmailModel.Subject = emailTemplate.EmailTemplateSubject;
                scheduleMasterModel.ScheduleEmailModel.Body = TokenReplace.ReplaceTokens(emailTemplate.EmailTemplateBody, listScheduleEmail);//"This is a Dummy Schedule Email. Lorem Ipsum is simply dummy text of the printing and typesetting industry.";



                response.IsSuccess = true;
                response.Data = scheduleMasterModel.ScheduleEmailModel;
            }

            return response;
        }

        public ServiceResponse HC_SendEmpEmail(ScheduleEmailModel scheduleEmailModel, long loggedInUserID)
        {
            var response = new ServiceResponse();

            bool isSentMail = false;
            if (scheduleEmailModel.Email != null)
            {
                //isSentMail = Common.SendEmail(scheduleEmailModel.Subject, ConfigSettings.SupportEmail, scheduleEmailModel.Email, scheduleEmailModel.Body, null, ConfigSettings.CCEmailAddress);
                isSentMail = Common.SendEmail(scheduleEmailModel.Subject, _cacheHelper.SupportEmail, scheduleEmailModel.Email, scheduleEmailModel.Body, null);
                response.IsSuccess = isSentMail;
            }

            //if (isSentMail)
            //{
            //    //INoteDataProvider iNoteDataProvider = new NoteDataProvider();
            //    //iNoteDataProvider.SaveGeneralNote(scheduleEmailModel.ReferralID, scheduleEmailModel.Body, Resource.WebsiteScheduleNotificationEmail, loggedInUserID, scheduleEmailModel.ParentName + " (" +
            //    //                                  scheduleEmailModel.Phone + ")", Resource.Parent, Resource.Email);

            //    //When Send Mail that time Update Status for Send Mail for this Schedule
            //    UpdateScheduleMasterNotificaitonDetails(scheduleEmailModel.ScheduleID, Constants.Email);

            //}


            #region SAVE Schedle Notification Log
            SaveScheduleNotificationLogDetails(scheduleEmailModel.ReferralID, scheduleEmailModel.ScheduleID, Constants.Email,
                Resource.WebsiteScheduleNotificationEmail, scheduleEmailModel.Body, isSentMail,
                toEmail: scheduleEmailModel.Email, subject: scheduleEmailModel.Subject, createdBy: loggedInUserID);
            #endregion


            response.Message = isSentMail ? Resource.EmailSentSuccess : Resource.EmailSentFailed;

            return response;
        }

        public ServiceResponse HC_GetEmailDetail(long scheduleId)
        {
            var response = new ServiceResponse();

            #region Getting List And Email Template Model

            var searchList = new List<SearchValueData> { new SearchValueData { Name = "ScheduleID", Value = Convert.ToString(scheduleId) }, };

            HC_ListScheduleEmail listScheduleEmail = GetEntity<HC_ListScheduleEmail>(StoredProcedure.HC_GetScheduleEmailDetail, searchList);

            EmailTemplate emailTemplate = GetEntity<EmailTemplate>(new List<SearchValueData> { new SearchValueData { Name = "EmailTemplateTypeID", Value = ((int)EnumEmailType.HomeCare_Schedule_Notification).ToString() } });
            listScheduleEmail.HomeCareLogoImage = _cacheHelper.SiteBaseURL + Constants.AsapCareLogoImage;
            #endregion

            if (emailTemplate != null && listScheduleEmail != null)
            {
                ScheduleMasterModel scheduleMasterModel = new ScheduleMasterModel();

                scheduleMasterModel.ScheduleEmailModel.ScheduleID = listScheduleEmail.ScheduleID;
                scheduleMasterModel.ScheduleEmailModel.Email = listScheduleEmail.PatientEmail;
                scheduleMasterModel.ScheduleEmailModel.StartDate = listScheduleEmail.StartDate;

                scheduleMasterModel.ScheduleEmailModel.Subject = emailTemplate.EmailTemplateSubject;
                scheduleMasterModel.ScheduleEmailModel.Body = TokenReplace.ReplaceTokens(emailTemplate.EmailTemplateBody, listScheduleEmail);//"This is a Dummy Schedule Email. Lorem Ipsum is simply dummy text of the printing and typesetting industry.";

                response.IsSuccess = true;
                response.Data = scheduleMasterModel.ScheduleEmailModel;
            }

            return response;
        }

        public ServiceResponse HC_SendParentEmail(ScheduleEmailModel scheduleEmailModel, long loggedInUserID)
        {
            var response = new ServiceResponse();

            bool isSentMail = false;
            if (scheduleEmailModel.Email != null)
            {
                //isSentMail = Common.SendEmail(scheduleEmailModel.Subject, ConfigSettings.SupportEmail, scheduleEmailModel.Email, scheduleEmailModel.Body, null, ConfigSettings.CCEmailAddress);
                isSentMail = Common.SendEmail(scheduleEmailModel.Subject, _cacheHelper.SupportEmail, scheduleEmailModel.Email, scheduleEmailModel.Body, null);
                response.IsSuccess = isSentMail;
            }

            //if (isSentMail)
            //{
            //    INoteDataProvider iNoteDataProvider = new NoteDataProvider();
            //    iNoteDataProvider.SaveGeneralNote(scheduleEmailModel.ReferralID, scheduleEmailModel.Body, Resource.WebsiteScheduleNotificationEmail, loggedInUserID, scheduleEmailModel.ParentName + " (" +
            //                                      scheduleEmailModel.Phone + ")", Resource.Parent, Resource.Email);

            //    //When Send Mail that time Update Status for Send Mail for this Schedule
            //    UpdateScheduleMasterNotificaitonDetails(scheduleEmailModel.ScheduleID, Constants.Email);

            //}


            #region SAVE Schedle Notification Log
            SaveScheduleNotificationLogDetails(scheduleEmailModel.ReferralID, scheduleEmailModel.ScheduleID, Constants.Email,
                Resource.WebsiteScheduleNotificationEmail, scheduleEmailModel.Body, isSentMail,
                toEmail: scheduleEmailModel.Email, subject: scheduleEmailModel.Subject, createdBy: loggedInUserID);
            #endregion


            response.Message = isSentMail ? Resource.EmailSentSuccess : Resource.EmailSentFailed;

            return response;
        }

        public ServiceResponse HC_GetSMSDetail(long scheduleId)
        {
            var response = new ServiceResponse();

            #region Getting List And SMS Template Model

            var searchList = new List<SearchValueData> { new SearchValueData { Name = "ScheduleID", Value = Convert.ToString(scheduleId) }, };

            HC_ListScheduleEmail listScheduleEmail = GetEntity<HC_ListScheduleEmail>(StoredProcedure.HC_GetScheduleEmailDetail, searchList);

            EmailTemplate emailTemplate = GetEntity<EmailTemplate>(new List<SearchValueData> { new SearchValueData
            { Name = "EmailTemplateTypeID", Value = ((int)EnumEmailType.HomeCare_Schedule_Notification_SMS).ToString() } });

            #endregion

            if (emailTemplate != null && listScheduleEmail != null)
            {

                #region Replace the The Content for SMS Model

                ScheduleMasterModel scheduleMasterModel = new ScheduleMasterModel();
                scheduleMasterModel.ScheduleSmsModel.ToNumber = listScheduleEmail.PatientPhone;
                scheduleMasterModel.ScheduleSmsModel.ScheduleID = listScheduleEmail.ScheduleID;
                scheduleMasterModel.ScheduleSmsModel.StartDate = listScheduleEmail.StartDate;
                scheduleMasterModel.ScheduleSmsModel.Body = TokenReplace.ReplaceTokens(emailTemplate.EmailTemplateBody, listScheduleEmail);//"This is a Dummy Schedule Text. Lorem Ipsum is simply dummy text of the printing and typesetting industry.";

                #endregion

                response.IsSuccess = true;
                response.Data = scheduleMasterModel.ScheduleSmsModel;

            }
            return response;
        }

        public ServiceResponse HC_SendParentSMS(ScheduleSmsModel scheduleSmsModel, long loggedInUserID)
        {
            string emailtype = Convert.ToString(EnumEmailType.Schedule_Notification_SMS);
            var response = new ServiceResponse();

            bool isSentSms = Common.SendSms(scheduleSmsModel.ToNumber, scheduleSmsModel.Body, emailtype);

            response.IsSuccess = isSentSms;
            response.Message = isSentSms ? Resource.SMSSentSuccess : Resource.SMSSentFail;
            return response;
        }



        public ServiceResponse HC_DayCare_DeleteScheduleMaster(SearchScheduleMasterModel searchScheduleMasterModel, int pageIndex, int pageSize, string sortIndex, string sortDirection,
                                                    long loggedInId, string sortIndexArray = "")
        {
            ServiceResponse response = new ServiceResponse();

            //Check for When IsAssignTransportationGroupDown and UP is true that time cannot Delete record

            string[] idlist = searchScheduleMasterModel.ListOfIdsInCsv.Split(',');

            foreach (var schdeuleid in idlist)
            {
                ScheduleMaster getConfirmList = GetEntity<ScheduleMaster>(Convert.ToInt64(schdeuleid));
                if (getConfirmList.IsAssignedToTransportationGroupDown || getConfirmList.IsAssignedToTransportationGroupUp)
                {
                    response.Message = Resource.Youcannotdeletethisasclient;
                    return response;
                }
            }

            List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);
            HC_DayCare_SetSearchFilterForScheduleMasterList(searchScheduleMasterModel, searchList, loggedInId);

            if (!string.IsNullOrEmpty(searchScheduleMasterModel.ListOfIdsInCsv))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchScheduleMasterModel.ListOfIdsInCsv });

            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(searchScheduleMasterModel.IsShowListing) });
            searchList.Add(new SearchValueData { Name = "ConfirmationScheduleStatusID", Value = Convert.ToString((int)ScheduleStatus.ScheduleStatuses.Confirmed) });

            List<ScheduleMasterList> totalData = GetEntityList<ScheduleMasterList>(StoredProcedure.HC_DayCare_DeleteSchedule, searchList);



            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ScheduleMasterList> list = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = list;
            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordDeletedSuccessfully, Resource.Schedule);
            return response;
        }












        #region New Assignment


        public ServiceResponse HC_SetScheduleAssignmentModel01()
        {
            var response = new ServiceResponse();

            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData{Name = "Preference_Skill",Value = Convert.ToString(Preference.PreferenceKeyType.Skill)},
                    new SearchValueData{Name = "Preference_Preference",Value = Convert.ToString(Preference.PreferenceKeyType.Preference)}
                };
            HC_ScheduleAssignmentModel01 scheduleAssignmentModel = GetMultipleEntity<HC_ScheduleAssignmentModel01>(StoredProcedure.HC_SetScheduleAssignmentModel01, searchParam);
            scheduleAssignmentModel.CancellationReasons = Common.SetCancellationReasons();

            scheduleAssignmentModel.ScheduleSearchModel.StartDate = Common.GetOrgStartOfWeek();
            scheduleAssignmentModel.ScheduleSearchModel.EndDate = Common.GetOrgStartOfWeek().AddDays(6);



            response.IsSuccess = true;
            response.Data = scheduleAssignmentModel;
            return response;
        }





        public ServiceResponse HC_DayCare_SetScheduleAssignmentModel()
        {
            var response = new ServiceResponse();

            List<SearchValueData> searchParam = new List<SearchValueData>();
            HC_DayCare_ScheduleAssignmentModel scheduleAssignmentModel = GetMultipleEntity<HC_DayCare_ScheduleAssignmentModel>(StoredProcedure.HC_DayCare_SetScheduleAssignmentModel, searchParam);
            scheduleAssignmentModel.ScheduleSearchModel.StartDate = Common.GetOrgStartOfWeek();
            scheduleAssignmentModel.ScheduleSearchModel.EndDate = Common.GetOrgStartOfWeek().AddDays(6);



            response.IsSuccess = true;
            response.Data = scheduleAssignmentModel;
            return response;
        }

        public ServiceResponse HC_SetVirtualVisitsListPage()
        {
            var response = new ServiceResponse();
            SetVirtualVisitsListModel model = new SetVirtualVisitsListModel();
            List<SearchValueData> searchParam = new List<SearchValueData>();
            searchParam.Add(new SearchValueData() { Name = "IsDeleted", Value = "0" });
            model.EmployeeList = GetEntityList<Employee>(searchParam);
            // generate unique token if not exist else just get from table
            string token = Guid.NewGuid().ToString();
            var tokenData = new EmployeeDataProvider().GetToken(SessionHelper.LoggedInID, 525600, token, true); //(525600 minutes = 1 year)
            model.Token = tokenData.Token;
            response.IsSuccess = true;
            response.Data = model;
            response.Data = model;
            return response;
        }
        public ServiceResponse HC_SetEmployeeVisitsPage()
        {
            var response = new ServiceResponse();

            List<SearchValueData> searchParam = new List<SearchValueData>();
            HC_SetEmployeeVisitsPage scheduleAssignmentModel = GetMultipleEntity<HC_SetEmployeeVisitsPage>(StoredProcedure.HC_SetEmployeeVisitsPage, searchParam);
            scheduleAssignmentModel.SearchReferralEmployeeModel.SlotDate = Common.GetOrgCurrentDateTime().Date;
            //
            response.IsSuccess = true;
            response.Data = scheduleAssignmentModel;
            return response;
        }

        public ServiceResponse HC_CaseManagement_SetScheduleAssignmentModel()
        {
            var response = new ServiceResponse();

            List<SearchValueData> searchParam = new List<SearchValueData>();
            HC_DayCare_ScheduleAssignmentModel scheduleAssignmentModel = GetMultipleEntity<HC_DayCare_ScheduleAssignmentModel>(StoredProcedure.HC_CaseManagement_SetScheduleAssignmentModel, searchParam);
            scheduleAssignmentModel.ScheduleSearchModel.StartDate = Common.GetOrgStartOfWeek();
            scheduleAssignmentModel.ScheduleSearchModel.EndDate = Common.GetOrgStartOfWeek().AddDays(6);



            response.IsSuccess = true;
            response.Data = scheduleAssignmentModel;
            return response;
        }

        public ServiceResponse HC_PrivateDuty_SetScheduleAssignmentModel()
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData{Name = "Preference_Skill",Value = Convert.ToString(Preference.PreferenceKeyType.Skill)},
                    new SearchValueData{Name = "Preference_Preference",Value = Convert.ToString(Preference.PreferenceKeyType.Preference)}
                };
            HC_PrivateDuty_ScheduleAssignmentModel scheduleAssignmentModel = GetMultipleEntity<HC_PrivateDuty_ScheduleAssignmentModel>(StoredProcedure.HC_PrivateDuty_SetScheduleAssignmentModel, searchParam);
            scheduleAssignmentModel.CancellationReasons = Common.SetCancellationReasons();

            scheduleAssignmentModel.ScheduleSearchModel.StartDate = Common.GetOrgStartOfWeek();
            scheduleAssignmentModel.ScheduleSearchModel.EndDate = Common.GetOrgStartOfWeek().AddDays(6);

            response.IsSuccess = true;
            response.Data = scheduleAssignmentModel;
            return response;


        }


        public ServiceResponse HC_GetSchEmployeeListForSchedule(SearchEmployeeListForSchedule searchEmployeeModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchEmployeeModel != null)
                SetSearchFilterForSchEmployeeList(searchEmployeeModel, searchList);

            Page<EmployeeListForSchedule> page = GetEntityPageList<EmployeeListForSchedule>(StoredProcedure.HC_GetEmployeeListForScheduling, searchList, pageSize, pageIndex, sortIndex, sortDirection);
            response.Data = page;
            response.IsSuccess = true;
            return response;
        }
        private static void SetSearchFilterForSchEmployeeList(SearchEmployeeListForSchedule model, List<SearchValueData> searchList)
        {

            searchList.Add(new SearchValueData { Name = "Name", Value = Convert.ToString(model.Name) });
            searchList.Add(new SearchValueData { Name = "StrSkillList", Value = Convert.ToString(model.StrSkillList) });
            searchList.Add(new SearchValueData { Name = "StrPreferenceList", Value = Convert.ToString(model.StrPreferenceList) });
            searchList.Add(new SearchValueData { Name = "FrequencyCodeID", Value = Convert.ToString(model.FrequencyCodeID) });
            searchList.Add(new SearchValueData { Name = "WeekStartDay", Value = Convert.ToString(Common.GetCalWeekStartDay()) });

            if (model.StartDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = model.StartDate.Value.ToString(Constants.DbDateFormat) });
            if (model.EndDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = model.EndDate.Value.ToString(Constants.DbDateFormat) });

        }

        public ServiceResponse HC_PrivateDuty_GetSchEmployeeListForSchedule(SearchEmployeeListForSchedule searchEmployeeModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchEmployeeModel != null)
                PrivateDuty_SetSearchFilterForSchEmployeeList(searchEmployeeModel, searchList);

            Page<EmployeeListForSchedule> page = GetEntityPageList<EmployeeListForSchedule>(StoredProcedure.HC_PrivateDuty_GetEmployeeListForScheduling, searchList, pageSize, pageIndex, sortIndex, sortDirection);
            response.Data = page;
            response.IsSuccess = true;
            return response;
        }
        private static void PrivateDuty_SetSearchFilterForSchEmployeeList(SearchEmployeeListForSchedule model, List<SearchValueData> searchList)
        {

            searchList.Add(new SearchValueData { Name = "Name", Value = Convert.ToString(model.Name) });
            searchList.Add(new SearchValueData { Name = "StrSkillList", Value = Convert.ToString(model.StrSkillList) });
            searchList.Add(new SearchValueData { Name = "StrPreferenceList", Value = Convert.ToString(model.StrPreferenceList) });
            searchList.Add(new SearchValueData { Name = "FrequencyCodeID", Value = Convert.ToString(model.FrequencyCodeID) });
            if (model.StartDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = model.StartDate.Value.ToString(Constants.DbDateFormat) });
            if (model.EndDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = model.EndDate.Value.ToString(Constants.DbDateFormat) });

        }



        public ServiceResponse HC_GetSchEmployeeDetailForPopup(SearchEmployeeListForSchedule model)
        {
            var response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "EmployeeID", Value = model.EmployeeID.ToString() });
            if (model.StartDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = model.StartDate.Value.ToString(Constants.DbDateFormat) });
            if (model.EndDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = model.EndDate.Value.ToString(Constants.DbDateFormat) });

            searchList.Add(new SearchValueData { Name = "Preference_Skill", Value = Convert.ToString(Preference.PreferenceKeyType.Skill) });
            searchList.Add(new SearchValueData { Name = "Preference_Preference", Value = Convert.ToString(Preference.PreferenceKeyType.Preference) });

            EmployeeDetailForPopup detailModel = GetMultipleEntity<EmployeeDetailForPopup>(StoredProcedure.GetSchEmployeeDetailForPopup, searchList);

            response.Data = detailModel;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse HC_PrivateDuty_GetSchEmployeeDetailForPopup(SearchEmployeeListForSchedule model)
        {
            var response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "EmployeeID", Value = model.EmployeeID.ToString() });
            if (model.StartDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = model.StartDate.Value.ToString(Constants.DbDateFormat) });
            if (model.EndDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = model.EndDate.Value.ToString(Constants.DbDateFormat) });

            searchList.Add(new SearchValueData { Name = "Preference_Skill", Value = Convert.ToString(Preference.PreferenceKeyType.Skill) });
            searchList.Add(new SearchValueData { Name = "Preference_Preference", Value = Convert.ToString(Preference.PreferenceKeyType.Preference) });

            EmployeeDetailForPopup detailModel = GetMultipleEntity<EmployeeDetailForPopup>(StoredProcedure.HC_PrivateDuty_GetSchEmployeeDetailForPopup, searchList);

            response.Data = detailModel;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse HC_GetReferralForScheduling(SearchScheduleListByFacility model)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ReferralName", Value = model.ReferralName });
            searchList.Add(new SearchValueData { Name = "StartDate", Value = model.StartDate.ToString(Constants.DbDateFormat) });
            searchList.Add(new SearchValueData { Name = "EndDate", Value = model.EndDate.ToString(Constants.DbDateFormat) });
            searchList.Add(new SearchValueData { Name = "SchStatus", Value = model.SchStatus });
            searchList.Add(new SearchValueData { Name = "ServicetypeId", Value = model.ServiceTypeID });

            List<ReferralScheduleModel> detailModel = GetEntityList<ReferralScheduleModel>(StoredProcedure.HC_GetReferralForScheduling, searchList);

            response.Data = detailModel;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse HC_PrivateDuty_GetReferralForScheduling(SearchScheduleListByFacility model)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ReferralName", Value = model.ReferralName });
            searchList.Add(new SearchValueData { Name = "StartDate", Value = model.StartDate.ToString(Constants.DbDateFormat) });
            searchList.Add(new SearchValueData { Name = "EndDate", Value = model.EndDate.ToString(Constants.DbDateFormat) });
            searchList.Add(new SearchValueData { Name = "SchStatus", Value = model.SchStatus });

            List<ReferralScheduleModel> detailModel = GetEntityList<ReferralScheduleModel>(StoredProcedure.HC_PrivateDuty_GetReferralForScheduling, searchList);

            response.Data = detailModel;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse HC_DayCare_GetReferralForScheduling(SearchScheduleListByFacility model)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ReferralName", Value = model.ReferralName });
            searchList.Add(new SearchValueData { Name = "StartDate", Value = model.StartDate.ToString(Constants.DbDateFormat) });
            searchList.Add(new SearchValueData { Name = "EndDate", Value = model.EndDate.ToString(Constants.DbDateFormat) });
            searchList.Add(new SearchValueData { Name = "SchStatus", Value = model.SchStatus });

            List<ReferralScheduleModel> detailModel = GetEntityList<ReferralScheduleModel>(StoredProcedure.HC_DayCare_GetReferralForScheduling, searchList);

            response.Data = detailModel;
            response.IsSuccess = true;
            return response;
        }


        public ServiceResponse HC_CaseManagement_GetReferralForScheduling(SearchScheduleListByFacility model)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ReferralName", Value = model.ReferralName });
            searchList.Add(new SearchValueData { Name = "StartDate", Value = model.StartDate.ToString(Constants.DbDateFormat) });
            searchList.Add(new SearchValueData { Name = "EndDate", Value = model.EndDate.ToString(Constants.DbDateFormat) });
            searchList.Add(new SearchValueData { Name = "SchStatus", Value = model.SchStatus });

            List<ReferralScheduleModel> detailModel = GetEntityList<ReferralScheduleModel>(StoredProcedure.HC_CaseManagement_GetReferralForScheduling, searchList);

            response.Data = detailModel;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse HC_GetScheduleListByReferrals(SearchScheduleListByFacility searchPara)
        {
            var response = new ServiceResponse();
            //Code For Getting Schedule List..

            string emps = "";
            if (searchPara.EmployeeIDs != null && searchPara.EmployeeIDs.Count > 0)
                emps = String.Join(",", searchPara.EmployeeIDs);

            string refs = "";
            if (searchPara.ReferralIDs != null && searchPara.ReferralIDs.Count > 0)
                refs = String.Join(",", searchPara.ReferralIDs);

            var detailModel = GetMultipleEntity<ReferralScheduleDetails>(StoredProcedure.HC_GetScheduleListByReferrals, new List<SearchValueData>
            {
                new SearchValueData{Name = "EmployeeIDs",Value = emps},
                new SearchValueData{Name = "ReferralIDs",Value = refs},
                new SearchValueData{Name = "StartDate",Value = searchPara.StartDate.ToString(Constants.DbDateFormat)},
                new SearchValueData{Name = "EndDate",Value = searchPara.EndDate.ToString(Constants.DbDateFormat)},
                new SearchValueData{Name = "SchStatus",Value = searchPara.SchStatus},
                new SearchValueData{Name = "ServicetypeId",Value = searchPara.ServiceTypeID}

            });


            //detailModel.ScheduleSearchModel.StartDate = searchPara.StartDate;
            //detailModel.ScheduleSearchModel.EndDate = searchPara.EndDate;
            if (detailModel != null && detailModel.ScheduleList.Count > 0)
            {
                foreach (var item in detailModel.ScheduleList)
                {

                    item.StartTime = item.StartDate != null ? item.StartDate.ToShortTimeString() : "";
                    item.EndTime = item.EndDate != null ? item.EndDate.ToShortTimeString() : "";
                    if (item.strClockInTime != null)
                    {
                        item.strClockInTime = (Convert.ToDateTime(item.strClockInTime)).ToShortTimeString();
                        item.EVVClockIn = item.IVRClockIn ? "I" : "M";
                    }

                    else
                    {
                        item.strClockInTime = string.Empty;
                        item.EVVClockIn = string.Empty;
                    }

                    if (item.strClockOutTime != null)
                    {
                        item.strClockOutTime = (Convert.ToDateTime(item.strClockOutTime)).ToShortTimeString();
                        item.EVVClockOut = item.IVRClockOut ? "I" : "M";
                    }

                    else
                    {
                        item.strClockOutTime = string.Empty;
                        item.EVVClockOut = string.Empty;
                    }
                    //item.strClockInTime = item.strClockInTime != null && item.IVRClockIn ?  : "";
                    //item.strClockOutTime = item.ClockOutTime != null ? item.ClockOutTime.ToShortTimeString() : "";

                }
            }
            response.Data = detailModel;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse HC_GetVirtualVisitsList(SearchVirtualVisitsListModel searchParam, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchParam != null)
                SetSearchFilterForVirtualVisitList(searchParam, searchList, loggedInId);

            Page<VirtualVisitsList> page = GetEntityPageList<VirtualVisitsList>(StoredProcedure.HC_GetVirtualVisitsList, searchList, pageSize, pageIndex, sortIndex, sortDirection);

            if (page != null && page.Items.Count > 0)
            {
                foreach (var item in page.Items)
                {
                    item.StartTime = item.StartDate != null ? item.StartDate.ToShortTimeString() : "";
                    item.EndTime = item.EndDate != null ? item.EndDate.ToShortTimeString() : "";
                }
            }

            response.Data = page;
            response.IsSuccess = true;
            return response;
        }

        private static void SetSearchFilterForVirtualVisitList(SearchVirtualVisitsListModel model, List<SearchValueData> searchList, long loggedInId)
        {
            searchList.Add(new SearchValueData { Name = "ServerDateTime", Value = DateTime.Now.ToString(Constants.DbDateTimeFormat) });
            searchList.Add(new SearchValueData { Name = "VisitType", Value = Convert.ToString(model.VisitType) });
            searchList.Add(new SearchValueData { Name = "EmployeeID", Value = Convert.ToString(model.EmployeeID) });
            searchList.Add(new SearchValueData { Name = "AllRecordAccess", Value = Common.HasPermission(Constants.AllRecordAccess) ? "1" : "0" });
            searchList.Add(new SearchValueData { Name = "LoggedInID", Value = Convert.ToString(loggedInId) });
            searchList.Add(new SearchValueData { Name = "ReferralName", Value = Convert.ToString(model.ReferralName) });
            if (model.StartDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = model.StartDate.Value.ToString(Constants.DbDateFormat) });
            if (model.EndDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = model.EndDate.Value.ToString(Constants.DbDateFormat) });
        }
        public ServiceResponse HC_GetReferralEmployeeVisits(SearchReferralEmployeeModel searchParam, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchParam != null)
            {
                SetSearchFilterForGetReferralEmployeeVisits(searchParam, searchList, loggedInId);
            }
            List<GetReferralEmployeeVisitsModel> page =
                GetEntityList<GetReferralEmployeeVisitsModel>(StoredProcedure.HC_GetReferralEmployeeVisits, searchList
               );



            response.Data = page;
            response.IsSuccess = true;
            return response;
        }
        private static void SetSearchFilterForGetReferralEmployeeVisits(SearchReferralEmployeeModel model, List<SearchValueData> searchList, long loggedInId)
        {
            searchList.Add(new SearchValueData { Name = "SlotDate", Value = model.SlotDate.ToString(Constants.DbDateFormat) });
            searchList.Add(new SearchValueData { Name = "EmployeeID", Value = Convert.ToString(model.EmployeeID) });
            searchList.Add(new SearchValueData { Name = "TransportationType", Value = Convert.ToString(model.TransportationType) });
        }
        public ServiceResponse SavePickUpDropCall(SaveEmployeeVisitsTransportLog model, long loggedInId)
        {
            var response = new ServiceResponse();

            //SaveEmployeeVisitsTransportLog
            List<SearchValueData> searchList = new List<SearchValueData>();
            if (model.EmployeeVisitsTransportLogId != null)
            {
                searchList.Add(new SearchValueData { Name = "Id", Value = Convert.ToString(model.EmployeeVisitsTransportLogId) });
            }
            else
            {
                /*
                searchList.Add(new SearchValueData { Name = "VisitDate", Value = Convert.ToString(((DateTime)Common.GetOrgCurrentDateTime()).Date) });
                searchList.Add(new SearchValueData { Name = "Starttime", Value = Convert.ToString(Common.GetOrgCurrentDateTime()) });
            */
                searchList.Add(new SearchValueData { Name = "VisitDate", Value = DateTime.Now.Date.ToString(Constants.DbDateFormat) });
                searchList.Add(new SearchValueData { Name = "Starttime", Value = DateTime.Now.ToString(Constants.DbDateFormat) });
            }
            searchList.Add(new SearchValueData { Name = "EmployeeID", Value = Convert.ToString(model.EmployeeID) });
            if (model.TransportGroupID != null)
            {
                searchList.Add(new SearchValueData { Name = "TransportGroupID", Value = Convert.ToString(model.TransportGroupID) });
            }
            if (model.TransportAssignPatientID != null)
            {
                searchList.Add(new SearchValueData { Name = "TransportAssignPatientID", Value = Convert.ToString(model.TransportAssignPatientID) });
            }
            searchList.Add(new SearchValueData { Name = "Endtime", Value = DateTime.Now.ToString(Constants.DbDateFormat) });
            searchList.Add(new SearchValueData { Name = "loggedInUserID", Value = Convert.ToString(loggedInId) });
            GetScalar(StoredProcedure.SaveEmployeeVisitsTransportLog, searchList);
            //
            searchList = new List<SearchValueData>();
            if (model.EmployeeVisitsTransportLogId == null)
            {
                var Data = HC_GetReferralEmployeeVisits(new SearchReferralEmployeeModel
                {
                    EmployeeID = model.EmployeeID,
                    SlotDate = ConvertToLocal(model.ClockInTime).Date,
                    TransportationType = model.TransportationType
                }, 0, 0, "", "", loggedInId).Data;
                var Data2 = ((List<GetReferralEmployeeVisitsModel>)Data);
                var item = Data2.Where(e => e.ReferralID == model.ReferralID).FirstOrDefault();
                if (item != null)
                {
                    searchList.Add(new SearchValueData { Name = "EmployeeVisitsTransportLogId", Value = Convert.ToString(item.EmployeeVisitsTransportLogId) });
                }
            }
            else
            {
                searchList.Add(new SearchValueData { Name = "EmployeeVisitsTransportLogId", Value = Convert.ToString(model.EmployeeVisitsTransportLogId) });
            }
            if (model.EmployeeVisitsTransportLogDetailId != null)
            {
                searchList.Add(new SearchValueData { Name = "Id", Value = Convert.ToString(model.EmployeeVisitsTransportLogDetailId) });
            }
            searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
            if (model.ClockInTime != null)
            {
                searchList.Add(new SearchValueData { Name = "ClockInTime", Value = ConvertToLocal(model.ClockInTime).ToString(Constants.DbDateFormat) });
            }
            if (model.ClockOutTime != null)
            {
                searchList.Add(new SearchValueData { Name = "ClockOutTime", Value = ConvertToLocal(model.ClockOutTime).ToString(Constants.DbDateFormat) });
            }
            searchList.Add(new SearchValueData { Name = "Latitude", Value = Convert.ToString(model.Latitude) });
            searchList.Add(new SearchValueData { Name = "Longitude", Value = Convert.ToString(model.Longitude) });


            GetScalar(StoredProcedure.SaveEmployeeVisitsTransportLogDetail, searchList);




            response.IsSuccess = true;
            response.Message = "Success";
            return response;

            DateTime ConvertToLocal(DateTime? d)
            {
                return TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(((DateTime)d), DateTimeKind.Utc), TimeZoneInfo.Local);
            }
        }



        public ServiceResponse HC_PrivateDuty_GetScheduleListByReferrals(SearchScheduleListByFacility searchPara)
        {
            var response = new ServiceResponse();
            //Code For Getting Schedule List..

            string emps = "";
            if (searchPara.EmployeeIDs != null && searchPara.EmployeeIDs.Count > 0)
                emps = String.Join(",", searchPara.EmployeeIDs);

            string refs = "";
            if (searchPara.ReferralIDs != null && searchPara.ReferralIDs.Count > 0)
                refs = String.Join(",", searchPara.ReferralIDs);

            var detailModel = GetMultipleEntity<ReferralScheduleDetails>(StoredProcedure.HC_PrivateDuty_GetScheduleListByReferrals, new List<SearchValueData>
            {
                new SearchValueData{Name = "EmployeeIDs",Value = emps},
                new SearchValueData{Name = "ReferralIDs",Value = refs},
                new SearchValueData{Name = "StartDate",Value = searchPara.StartDate.ToString(Constants.DbDateFormat)},
                new SearchValueData{Name = "EndDate",Value = searchPara.EndDate.ToString(Constants.DbDateFormat)},
                new SearchValueData{Name = "SchStatus",Value = searchPara.SchStatus}
            });


            //detailModel.ScheduleSearchModel.StartDate = searchPara.StartDate;
            //detailModel.ScheduleSearchModel.EndDate = searchPara.EndDate;

            response.Data = detailModel;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse HC_DayCare_GetScheduleListByReferrals(SearchScheduleListByFacility searchPara)
        {
            var response = new ServiceResponse();
            //Code For Getting Schedule List..

            string facilities = "";
            if (searchPara.FacilityIDs != null && searchPara.FacilityIDs.Count > 0)
                facilities = String.Join(",", searchPara.FacilityIDs);

            string refs = "";
            if (searchPara.ReferralIDs != null && searchPara.ReferralIDs.Count > 0)
                refs = String.Join(",", searchPara.ReferralIDs);

            var detailModel = GetMultipleEntity<ADC_ReferralScheduleDetails>(StoredProcedure.HC_DayCare_GetScheduleListByReferrals, new List<SearchValueData>
            {
                new SearchValueData{Name = "FacilityIDs",Value = facilities},
                new SearchValueData{Name = "ReferralIDs",Value = refs},
                new SearchValueData{Name = "StartDate",Value = searchPara.StartDate.ToString(Constants.DbDateFormat)},
                new SearchValueData{Name = "EndDate",Value = searchPara.EndDate.ToString(Constants.DbDateFormat)},
                new SearchValueData{Name = "IsScheduled",Value = searchPara.SchStatus}
            });


            //detailModel.ScheduleSearchModel.StartDate = searchPara.StartDate;
            //detailModel.ScheduleSearchModel.EndDate = searchPara.EndDate;

            response.Data = detailModel;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse HC_SaveReferralProfileImg(HttpRequestBase currentHttpRequest, bool isEmployeeDocument = false, long referralID = 0)
        {
            CacheHelper _cacheHelper = new CacheHelper();
            ServiceResponse response = new ServiceResponse();
            //var ReferralId = currentHttpRequest.Form["id"];
            HttpPostedFileBase file = currentHttpRequest.Files[0];

            string basePath = String.Format(_cacheHelper.UploadPath, _cacheHelper.Domain) + ConfigSettings.EmpProfileImg;
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
                        new SearchValueData {Name = "ReferralID ", Value = Convert.ToString(referralID) },
                    };

            GetScalar(StoredProcedure.SaveProfileImage, searchList);



            response.Message = "Profile Image Save Successfully";
            return response;
        }

        public ServiceResponse HC_DayCare_SetScheduleAttendenceModel()
        {
            var response = new ServiceResponse();

            List<SearchValueData> searchParam = new List<SearchValueData>();
            HC_DayCare_SetScheduleAttendenceModel model = GetMultipleEntity<HC_DayCare_SetScheduleAttendenceModel>(StoredProcedure.HC_DayCare_SetScheduleAttendenceModel, searchParam);
            response.IsSuccess = true;
            response.Data = model;
            return response;
        }

        public ServiceResponse HC_Daycare_GetScheduledPatientList(SearchScheduledPatientModel model)
        {
            var response = new ServiceResponse();


            var detailModel = GetEntityList<Daycare_GetScheduledPatientList>(StoredProcedure.HC_Daycare_GetScheduledPatientList, new List<SearchValueData>
            {
                new SearchValueData{Name = "PatientName",Value = model.PatientName},
                new SearchValueData{Name = "FacilityID",Value = Convert.ToString(model.FacilityID)},
                new SearchValueData{Name = "StartDate",Value = DateTime.Now.ToString(Constants.DbDateFormat)}
            });


            //detailModel.ScheduleSearchModel.StartDate = searchPara.StartDate;
            //detailModel.ScheduleSearchModel.EndDate = searchPara.EndDate;

            response.Data = detailModel;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse Daycare_GetRelationTypeList(int type)
        {
            var response = new ServiceResponse();


            var detailModel = GetEntityList<NameValueData>(StoredProcedure.Daycare_GetRelationTypeList, new List<SearchValueData>
            {
                new SearchValueData{Name = "DDType",Value = type.ToString()}
            });
            response.Data = detailModel;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse HC_Daycare_PatientClockInClockOut(Daycare_SavePatient_AttendecenModel saveModel, long loggedinID)
        {
            var response = new ServiceResponse();

            Daycare_GetScheduledPatientList model = saveModel.Daycare_GetScheduledPatientList;

            string clockInTime = model.ClockInTime.HasValue ? model.ClockInTime.Value.ToString(Constants.DbDateTimeFormat) : Common.GetOrgCurrentDateTime().ToString(Constants.DbDateTimeFormat);
            string clockOutTime = model.IsClockInCompleted ? (model.ClockOutTime.HasValue ? model.ClockOutTime.Value.ToString(Constants.DbDateTimeFormat) : Common.GetOrgCurrentDateTime().ToString(Constants.DbDateTimeFormat)) : string.Empty; ;

            var detailModel = GetEntityList<Daycare_GetScheduledPatientList>(StoredProcedure.HC_Daycare_PatientClockInClockOut, new List<SearchValueData>
            {
                new SearchValueData{Name = "ScheduleID",Value =  Convert.ToString(model.ScheduleID)},
                new SearchValueData{Name = "EmployeeVisitID",Value = Convert.ToString(model.EmployeeVisitID)},
                new SearchValueData{Name = "ClockInTime",Value = clockInTime},
                new SearchValueData{Name = "ClockOutTime",Value = clockOutTime},
                new SearchValueData{Name = "PatientSignature_ClockIN",Value = model.PatientSignature_ClockIN},
                new SearchValueData{Name = "PatientSignature_ClockOut",Value = model.PatientSignature_ClockOut},
                new SearchValueData{Name = "LoggedInID",Value = Convert.ToString(loggedinID)},
                new SearchValueData{Name = "ReferralTaskMappingIDs",Value = Convert.ToString(model.ReferralTaskMappingIDs)},
                new SearchValueData{Name = "FacilityID",Value = Convert.ToString(saveModel.SearchScheduledPatientModel.FacilityID)},
                new SearchValueData{Name = "PatientName",Value = saveModel.SearchScheduledPatientModel.PatientName},
                new SearchValueData{Name = "IsSelf",Value = Convert.ToString(saveModel.Daycare_GetScheduledPatientList.IsSelf)},
                new SearchValueData{Name = "Name",Value = saveModel.Daycare_GetScheduledPatientList.Name},
                new SearchValueData{Name = "Relation",Value = saveModel.Daycare_GetScheduledPatientList.Relation},
                new SearchValueData{Name = "AttendanceType",Value = model.Attendence}
            });


            //detailModel.ScheduleSearchModel.StartDate = searchPara.StartDate;
            //detailModel.ScheduleSearchModel.EndDate = searchPara.EndDate;
            response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.Details);
            response.Data = detailModel;
            response.IsSuccess = true;
            return response;
        }




        public ServiceResponse HC_DayCare_GetSchedulePatientTasks(Daycare_SavePatient_AttendecenModel saveModel)
        {
            var response = new ServiceResponse();

            Daycare_GetScheduledPatientList model = saveModel.Daycare_GetScheduledPatientList;

            //var detailModel = GetEntityList<DayCare_GetSchedulePatientTask>(StoredProcedure.HC_DayCare_GetSchedulePatientTasks, new List<SearchValueData>
            var detailModel = GetMultipleEntity<DayCare_GetSchedulePatientTaskList>(StoredProcedure.HC_DayCare_GetSchedulePatientTasks, new List<SearchValueData>
            {
                new SearchValueData{Name = "ScheduleID",Value =  Convert.ToString(model.ScheduleID)},
                new SearchValueData{Name = "ReferralID",Value =  Convert.ToString(model.ReferralID)}
            });
            response.Data = detailModel;
            response.IsSuccess = true;
            return response;
        }



        public ServiceResponse HC_CaseManagement_GetScheduleListByReferrals(SearchScheduleListByFacility searchPara)
        {
            var response = new ServiceResponse();
            //Code For Getting Schedule List..

            string facilities = "";
            if (searchPara.FacilityIDs != null && searchPara.FacilityIDs.Count > 0)
                facilities = String.Join(",", searchPara.FacilityIDs);

            string refs = "";
            if (searchPara.ReferralIDs != null && searchPara.ReferralIDs.Count > 0)
                refs = String.Join(",", searchPara.ReferralIDs);

            var detailModel = GetMultipleEntity<ReferralScheduleDetails>(StoredProcedure.HC_CaseManagement_GetScheduleListByReferrals, new List<SearchValueData>
            {
                new SearchValueData{Name = "FacilityIDs",Value = facilities},
                new SearchValueData{Name = "ReferralIDs",Value = refs},
                new SearchValueData{Name = "StartDate",Value = searchPara.StartDate.ToString(Constants.DbDateFormat)},
                new SearchValueData{Name = "EndDate",Value = searchPara.EndDate.ToString(Constants.DbDateFormat)},
                new SearchValueData{Name = "IsScheduled",Value = searchPara.SchStatus}
            });


            //detailModel.ScheduleSearchModel.StartDate = searchPara.StartDate;
            //detailModel.ScheduleSearchModel.EndDate = searchPara.EndDate;

            response.Data = detailModel;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse HC_SaveScheduleFromCalender(ScheduleAssignmentModel model, long loggedInUserID)
        {
            if (model.ScheduleMaster.ScheduleID == 0)
            {
                if (model.ScheduleMaster.ReferralID == 0 || model.ScheduleMaster.EmployeeID == 0)
                {
                    return new ServiceResponse
                    {
                        IsSuccess = false,
                        Message = Resource.ErrorOccured
                    };
                }
                model.ScheduleMaster.ScheduleStatusID = (int)ScheduleStatus.ScheduleStatuses.Confirmed;
            }


            ServiceResponse response = HC_SaveSchedule(model.ScheduleMaster, loggedInUserID);


            return response;
        }
        public ServiceResponse HC_SaveSchedule(ScheduleMaster model, long loggedInUserID)
        {
            var response = new ServiceResponse();

            bool isEditing = model.ScheduleID > 0;

            // Code Validate Schedule Conflict And Other Valication
            //ServiceResponse validateResponse = HC_ValidateSchedule(scheduleMaster, loggedInUserID);
            //if (!validateResponse.IsSuccess)
            //{
            //    return validateResponse;
            //}
            //ValidateScheduleMasterModel validateScheduleModel = (ValidateScheduleMasterModel)validateResponse.Data;
            // Code Validate Schedule Conflict And Other Validation


            //SaveObject(scheduleMaster, loggedInUserID);
            //response.Data = scheduleMaster;


            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ScheduleID", Value = Convert.ToString(model.ScheduleID) });
            searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
            searchList.Add(new SearchValueData { Name = "EmployeeID", Value = Convert.ToString(model.EmployeeID) });
            searchList.Add(new SearchValueData { Name = "StartDateTime", Value = model.StartDate.ToString(Constants.DbDateTimeFormat) });
            searchList.Add(new SearchValueData { Name = "EndDateTime", Value = model.EndDate.ToString(Constants.DbDateTimeFormat) });
            searchList.Add(new SearchValueData { Name = "ScheduleStatusID", Value = Convert.ToString(model.ScheduleStatusID) });

            searchList.Add(new SearchValueData { Name = "DayView", Value = Convert.ToString(model.DayView) });
            searchList.Add(new SearchValueData { Name = "loggedInId", Value = Convert.ToString(loggedInUserID) });
            searchList.Add(new SearchValueData { Name = "SystemID", Value = Common.GetHostAddress() });

            var data = (int)GetScalar(StoredProcedure.SaveSchedule, searchList);


            if (data == -1)
                response.Message = Resource.EmployeeIsBlocked;

            if (data == -2)
                response.Message = Resource.EmployeeOnPTO;

            if (data == -3)
                response.Message = Resource.PatientTimeSlotNotAvailableOrScheduled;

            if (data == -4)
                response.Message = Resource.EmployeePatientTimeSlotNotFound;

            if (data == -5)
                response.Message = Resource.EmployeeTimeSlotNotAvailableOrScheduled;



            if (data < 0)
                return response;

            if (data == 0)
            {
                response.Message = model.ScheduleID > 0
                    ? Resource.SchdueleNotUpdated
                    : Resource.SchdueleNotCreated;
                response.ErrorCode = Constants.ErrorCode_Warning;
            }

            if (data > 0)
                response.Message = isEditing ? Resource.ScheduledUpdatedSuccessfully : Resource.ScheduledAddedSuccessfully;
            response.Data = model;
            response.IsSuccess = true;

            return response;
        }


        public ServiceResponse HC_PrivateDuty_SaveScheduleFromCalender(ScheduleAssignmentModel model, long loggedInUserID)
        {
            if (model.ScheduleMaster.ScheduleID == 0)
            {
                if (model.ScheduleMaster.ReferralID == 0 || model.ScheduleMaster.EmployeeID == 0)
                {
                    return new ServiceResponse
                    {
                        IsSuccess = false,
                        Message = Resource.ErrorOccured
                    };
                }
                model.ScheduleMaster.ScheduleStatusID = (int)ScheduleStatus.ScheduleStatuses.Confirmed;
            }


            ServiceResponse response = HC_PrivateDuty_SaveSchedule(model.ScheduleMaster, loggedInUserID);


            return response;
        }
        public ServiceResponse HC_PrivateDuty_SaveSchedule(ScheduleMaster model, long loggedInUserID)
        {
            var response = new ServiceResponse();

            bool isEditing = model.ScheduleID > 0;



            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ScheduleID", Value = Convert.ToString(model.ScheduleID) });
            searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
            searchList.Add(new SearchValueData { Name = "EmployeeID", Value = Convert.ToString(model.EmployeeID) });
            searchList.Add(new SearchValueData { Name = "StartDateTime", Value = model.StartDate.ToString(Constants.DbDateTimeFormat) });
            searchList.Add(new SearchValueData { Name = "EndDateTime", Value = model.EndDate.ToString(Constants.DbDateTimeFormat) });
            searchList.Add(new SearchValueData { Name = "ScheduleStatusID", Value = Convert.ToString(model.ScheduleStatusID) });

            searchList.Add(new SearchValueData { Name = "DayView", Value = Convert.ToString(model.DayView) });
            searchList.Add(new SearchValueData { Name = "loggedInId", Value = Convert.ToString(loggedInUserID) });
            searchList.Add(new SearchValueData { Name = "SystemID", Value = Common.GetHostAddress() });

            var data = (int)GetScalar(StoredProcedure.HC_PrivateDuty_SaveSchedule, searchList);


            if (data == -1)
                response.Message = Resource.EmployeeIsBlocked;

            if (data == -2)
                response.Message = Resource.EmployeeOnPTO;

            if (data == -3)
                response.Message = Resource.PatientTimeSlotNotAvailableOrScheduled;

            if (data == -4)
                response.Message = Resource.EmployeePatientTimeSlotNotFound;

            if (data == -5)
                response.Message = Resource.EmployeeTimeSlotNotAvailableOrScheduled;



            if (data < 0)
                return response;

            if (data == 0)
            {
                response.Message = model.ScheduleID > 0
                    ? Resource.SchdueleNotUpdated
                    : Resource.SchdueleNotCreated;
                response.ErrorCode = Constants.ErrorCode_Warning;
            }

            if (data > 0)
                response.Message = isEditing ? Resource.ScheduledUpdatedSuccessfully : Resource.ScheduledAddedSuccessfully;
            response.Data = model;
            response.IsSuccess = true;

            return response;
        }



        public ServiceResponse HC_DayCare_SaveScheduleFromCalender(ScheduleAssignmentModel model, long loggedInUserID)
        {
            if (model.ScheduleMaster.ScheduleID == 0)
            {
                if (model.ScheduleMaster.ReferralID == 0 || model.ScheduleMaster.EmployeeID == 0)
                {
                    return new ServiceResponse
                    {
                        IsSuccess = false,
                        Message = Resource.ErrorOccured
                    };
                }
                model.ScheduleMaster.ScheduleStatusID = (int)ScheduleStatus.ScheduleStatuses.Confirmed;
            }


            ServiceResponse response = HC_DayCare_SaveSchedule(model.ScheduleMaster, loggedInUserID);


            return response;
        }

        public ServiceResponse HC_DayCare_SaveSchedule(ScheduleMaster model, long loggedInUserID)
        {
            var response = new ServiceResponse();

            bool isEditing = model.ScheduleID > 0;


            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ScheduleID", Value = Convert.ToString(model.ScheduleID) });
            searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
            searchList.Add(new SearchValueData { Name = "EmployeeID", Value = Convert.ToString(model.EmployeeID) });
            searchList.Add(new SearchValueData { Name = "StartDateTime", Value = model.StartDate.ToString(Constants.DbDateTimeFormat) });
            searchList.Add(new SearchValueData { Name = "EndDateTime", Value = model.EndDate.ToString(Constants.DbDateTimeFormat) });
            searchList.Add(new SearchValueData { Name = "ScheduleStatusID", Value = Convert.ToString(model.ScheduleStatusID) });

            searchList.Add(new SearchValueData { Name = "DayView", Value = Convert.ToString(model.DayView) });
            searchList.Add(new SearchValueData { Name = "loggedInId", Value = Convert.ToString(loggedInUserID) });
            searchList.Add(new SearchValueData { Name = "SystemID", Value = Common.GetHostAddress() });

            var data = (int)GetScalar(StoredProcedure.HC_DayCare_SaveSchedule, searchList);


            if (data == -1)
                response.Message = Resource.EmployeeIsBlocked;

            if (data == -2)
                response.Message = Resource.EmployeeOnPTO;

            if (data == -3)
                response.Message = Resource.PatientTimeSlotNotAvailableOrScheduled;

            if (data == -4)
                response.Message = Resource.EmployeePatientTimeSlotNotFound;

            if (data == -5)
                response.Message = Resource.EmployeeTimeSlotNotAvailableOrScheduled;



            if (data < 0)
                return response;

            if (data == 0)
            {
                response.Message = model.ScheduleID > 0
                    ? Resource.SchdueleNotUpdated
                    : Resource.SchdueleNotCreated;
                response.ErrorCode = Constants.ErrorCode_Warning;
            }

            if (data > 0)
                response.Message = isEditing ? Resource.ScheduledUpdatedSuccessfully : Resource.ScheduledAddedSuccessfully;
            response.Data = model;
            response.IsSuccess = true;

            return response;
        }


        public ServiceResponse DeleteScheduleFromCalender(SearchScheduleMasterModel searchScheduleMasterModel, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchScheduleMasterModel.ListOfIdsInCsv });
            searchList.Add(new SearchValueData { Name = "loggedInId", Value = Convert.ToString(loggedInUserID) });
            if (!string.IsNullOrEmpty(searchScheduleMasterModel.Reason))
                searchList.Add(new SearchValueData { Name = "Reason", Value = searchScheduleMasterModel.Reason });

            GetScalar(StoredProcedure.RemoveSchedule, searchList);

            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordDeletedSuccessfully, Resource.Schedule);
            return response;
        }

        public ServiceResponse HC_PrivateDuty_DeleteScheduleFromCalender(SearchScheduleMasterModel searchScheduleMasterModel, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchScheduleMasterModel.ListOfIdsInCsv });
            searchList.Add(new SearchValueData { Name = "loggedInId", Value = Convert.ToString(loggedInUserID) });

            GetScalar(StoredProcedure.HC_PrivateDuty_RemoveSchedule, searchList);

            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordDeletedSuccessfully, Resource.Schedule);
            return response;
        }



        public ServiceResponse HC_DayCare_DeleteScheduleFromCalender(SearchScheduleMasterModel searchScheduleMasterModel, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchScheduleMasterModel.ListOfIdsInCsv });
            searchList.Add(new SearchValueData { Name = "loggedInId", Value = Convert.ToString(loggedInUserID) });

            GetScalar(StoredProcedure.HC_DayCare_DeleteScheduleFromCalender, searchList);

            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordDeletedSuccessfully, Resource.Schedule);
            return response;
        }


        public ServiceResponse HC_GetEmpRefSchPageModel()
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>
                {
                    new SearchValueData{Name = "Preference_Skill",Value = Convert.ToString(Preference.PreferenceKeyType.Skill)},
                    new SearchValueData{Name = "Preference_Preference",Value = Convert.ToString(Preference.PreferenceKeyType.Preference)}
                };
            GetEmpRefSchPageModel data =
                GetMultipleEntity<GetEmpRefSchPageModel>(StoredProcedure.GetEmpRefSchPageModel, searchList);

            response.Data = data;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse HC_PrivateDuty_GetEmpRefSchPageModel()
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>
                {
                    new SearchValueData{Name = "Preference_Skill",Value = Convert.ToString(Preference.PreferenceKeyType.Skill)},
                    new SearchValueData{Name = "Preference_Preference",Value = Convert.ToString(Preference.PreferenceKeyType.Preference)}
                };
            GetEmpRefSchPageModel data =
                GetMultipleEntity<GetEmpRefSchPageModel>(StoredProcedure.HC_PrivateDuty_GetEmpRefSchPageModel, searchList);

            response.Data = data;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse GetCareType(long payorID)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "PayorId", Value = Convert.ToString(payorID) });
            var data =
                    GetEntityList<DDMaster>(StoredProcedure.GetCareTypeDropDownByPayorId, searchList);
            response.Data = data;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse HC_GetEmpRefSchOptions(SearchEmpRefSchOption model, int pageIndex, int pageSize, string sortIndex, string sortDirection, string sortIndexArray, long loggedInId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            // EXEC HELP NEEDED
            if (model != null)
            {
                SetSearchFilterForEmpRefSchList(model, searchList, pageIndex, pageSize, sortIndex, sortDirection, sortIndexArray);

                GetEmpRefSchOptionsModel data =
                    GetMultipleEntity<GetEmpRefSchOptionsModel>(StoredProcedure.GetEmpRefSchOptions, searchList);

                data.PatientDetail.ScheduleID = model.ScheduleID;
                data.PatientDetail.ReferralTSDateID = model.ReferralTSDateID;

                data.Page = GetPageInStoredProcResultSet(pageIndex, pageSize, data.EmployeeTSList.Count > 0 ? data.EmployeeTSList[0].Count : 0, data.EmployeeTSList);
                response.Data = data;
                response.IsSuccess = true;
            }

            return response;
        }

        /*Schedule master OPT*/
        public ServiceResponse GetEmpRefSchOptions_PatientVisitFrequency_HC(SearchEmpRefSchOption model, int pageIndex, int pageSize, string sortIndex, string sortDirection, string sortIndexArray, long loggedInId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            if (model != null)
            {
                SetSearchFilterForEmpRefSchList(model, searchList, pageIndex, pageSize, sortIndex, sortDirection, sortIndexArray);

                GetEmpRefSchOptions_PatientVisitFrequencyModel data =
                    GetMultipleEntity<GetEmpRefSchOptions_PatientVisitFrequencyModel>(StoredProcedure.GetEmpRefSchOptions_PatientVisitFrequency_HC, searchList);

                response.Data = data;
                response.IsSuccess = true;
            }
            return response;
        }

        public ServiceResponse GetEmpRefSchOptions_ClientOnHoldData_HC(SearchEmpRefSchOption model, int pageIndex, int pageSize, string sortIndex, string sortDirection, string sortIndexArray, long loggedInId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            if (model != null)
            {
                SetSearchFilterForEmpRefSchList(model, searchList, pageIndex, pageSize, sortIndex, sortDirection, sortIndexArray);

                GetEmpRefSchOptions_ClientOnHoldDataModel data =
                    GetMultipleEntity<GetEmpRefSchOptions_ClientOnHoldDataModel>(StoredProcedure.GetEmpRefSchOptions_ClientOnHoldData_HC, searchList);

                response.Data = data;
                response.IsSuccess = true;
            }

            return response;
        }
        public ServiceResponse GetEmpRefSchOptions_ReferralInfo_HC(SearchEmpRefSchOption model, int pageIndex, int pageSize, string sortIndex, string sortDirection, string sortIndexArray, long loggedInId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            if (model != null)
            {
                SetSearchFilterForEmpRefSchList(model, searchList, pageIndex, pageSize, sortIndex, sortDirection, sortIndexArray);

                GetEmpRefSchOptions_ReferralInfoModel data =
                    GetMultipleEntity<GetEmpRefSchOptions_ReferralInfoModel>(StoredProcedure.GetEmpRefSchOptions_ReferralInfo_HC, searchList);

                data.PatientDetail.ScheduleID = model.ScheduleID;
                data.PatientDetail.ReferralTSDateID = model.ReferralTSDateID;
                response.Data = data;
                response.IsSuccess = true;
            }
            return response;
        }
        public ServiceResponse GetEmpRefSchOptions_ScheduleInfo_HC(SearchEmpRefSchOption model, int pageIndex, int pageSize, string sortIndex, string sortDirection, string sortIndexArray, long loggedInId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            if (model != null)
            {
                SetSearchFilterForEmpRefSchList(model, searchList, pageIndex, pageSize, sortIndex, sortDirection, sortIndexArray);

                GetEmpRefSchOptions_ScheduleInfoModel data =
                    GetMultipleEntity<GetEmpRefSchOptions_ScheduleInfoModel>(StoredProcedure.GetEmpRefSchOptions_ScheduleInfo_HC, searchList);
            
                data.Page = GetPageInStoredProcResultSet(pageIndex, pageSize, data.EmployeeTSList.Count > 0 ? data.EmployeeTSList[0].Count : 0, data.EmployeeTSList);

                response.Data = data;
                response.IsSuccess = true;
            }
            return response;
        }

        /*Schedule master OPT*/
        public ServiceResponse HC_GetEmpCareTypeIds(SearchEmpRefSchOption model, long loggedInId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            // EXEC HELP NEEDED
            if (model != null)
            {
                searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
                if (model.StartDate.HasValue)
                    searchList.Add(new SearchValueData { Name = "StartDate", Value = model.StartDate.Value.ToString(Constants.DbDateTimeFormat) });
                if (model.EndDate.HasValue)
                    searchList.Add(new SearchValueData { Name = "EndDate", Value = model.EndDate.Value.ToString(Constants.DbDateTimeFormat) });

                searchList.Add(new SearchValueData { Name = "DDType_CareType", Value = Convert.ToString((int)Common.DDType.CareType) });
                GetEmpRefSchOptionsModel responseModel = new GetEmpRefSchOptionsModel()
                {
                    CareTypeList = GetEntityList<DDMaster>(StoredProcedure.GetEmpRefSchOptions_GetCareTypeIds, searchList)
                };

                response.Data = responseModel;
                response.IsSuccess = true;
            }

            return response;
        }

        public ServiceResponse HC_GetRCLEmpRefSchOptions(SearchRCLEmpRefSchOption model, int pageIndex, int pageSize, string sortIndex, string sortDirection, string sortIndexArray, long loggedInId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            // EXEC HELP NEEDED
            if (model != null)
            {
                SetSearchFilterForRCLEmpRefSchList(model, searchList, pageIndex, pageSize, sortIndex, sortDirection, sortIndexArray);

                GetRCLEmpRefSchOptionsModel data =
                    GetMultipleEntity<GetRCLEmpRefSchOptionsModel>(StoredProcedure.GetRCLEmpRefSchOptions, searchList);

                data.Page = GetPageInStoredProcResultSet(pageIndex, pageSize, data.EmployeeTSList.Count > 0 ? data.EmployeeTSList[0].Count : 0, data.EmployeeTSList);
                response.Data = data;
                response.IsSuccess = true;
            }

            return response;
        }

        public ServiceResponse HC_PrivateDuty_GetEmpRefSchOptions(SearchEmpRefSchOption model, int pageIndex, int pageSize, string sortIndex, string sortDirection, string sortIndexArray, long loggedInId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            // EXEC HELP NEEDED
            if (model != null)
            {
                PrivateDuty_SetSearchFilterForEmpRefSchList(model, searchList, pageIndex, pageSize, sortIndex, sortDirection, sortIndexArray);

                GetEmpRefSchOptionsModel data =
                    GetMultipleEntity<GetEmpRefSchOptionsModel>(StoredProcedure.HC_PrivateDuty_GetEmpRefSchOptions, searchList);

                data.PatientDetail.ScheduleID = model.ScheduleID;
                data.PatientDetail.ReferralTSDateID = model.ReferralTSDateID;

                data.Page = GetPageInStoredProcResultSet(pageIndex, pageSize, data.EmployeeTSList.Count > 0 ? data.EmployeeTSList[0].Count : 0, data.EmployeeTSList);
                response.Data = data;
                response.IsSuccess = true;
            }

            return response;
        }

        public ServiceResponse HC_DayCare_GetReferralBillingAuthorizationList(ReferralBillingAuthorizatioSearchModel model)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            // EXEC HELP NEEDED
            if (model != null)
            {
                searchList.Add(new SearchValueData { Name = "PayorID", Value = Convert.ToString(model.PayorID) });
                searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });

                List<ReferralBillingAuthorizatioModel> data = GetEntityList<ReferralBillingAuthorizatioModel>(StoredProcedure.HC_DayCare_GetReferralBillingAuthorizationList, searchList);
                response.Data = data;
                response.IsSuccess = true;
            }

            return response;
        }


        public ServiceResponse HC_DayCare_GetEmpRefSchOptions(SearchEmpRefSchOption model, long loggedInId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            // EXEC HELP NEEDED
            if (model != null)
            {
                searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
                searchList.Add(new SearchValueData { Name = "ScheduleID", Value = Convert.ToString(model.ScheduleID) });
                searchList.Add(new SearchValueData { Name = "SameDateWithTimeSlot", Value = Convert.ToString(model.SameDateWithTimeSlot) });
                if (model.StartDate.HasValue)
                    searchList.Add(new SearchValueData { Name = "StartDate", Value = model.StartDate.Value.ToString(Constants.DbDateTimeFormat) });
                if (model.EndDate.HasValue)
                    searchList.Add(new SearchValueData { Name = "EndDate", Value = model.EndDate.Value.ToString(Constants.DbDateTimeFormat) });
                DayCare_GetEmpRefSchOptionsModel data = GetMultipleEntity<DayCare_GetEmpRefSchOptionsModel>(StoredProcedure.HC_DayCare_GetEmpRefSchOptions, searchList);
                if (data.PatientDetail != null)
                {
                    data.PatientDetail.ScheduleID = model.ScheduleID;
                    data.PatientDetail.ReferralTSDateID = model.ReferralTSDateID;
                }
                response.Data = data;
                response.IsSuccess = true;
            }

            return response;
        }

        public ServiceResponse HC_CaseManagement_GetEmpRefSchOptions(SearchEmpRefSchOption model, long loggedInId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            // EXEC HELP NEEDED
            if (model != null)
            {
                searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
                searchList.Add(new SearchValueData { Name = "ScheduleID", Value = Convert.ToString(model.ScheduleID) });
                searchList.Add(new SearchValueData { Name = "SameDateWithTimeSlot", Value = Convert.ToString(model.SameDateWithTimeSlot) });
                if (model.StartDate.HasValue)
                    searchList.Add(new SearchValueData { Name = "StartDate", Value = model.StartDate.Value.ToString(Constants.DbDateTimeFormat) });
                if (model.EndDate.HasValue)
                    searchList.Add(new SearchValueData { Name = "EndDate", Value = model.EndDate.Value.ToString(Constants.DbDateTimeFormat) });
                CaseManagement_GetEmpRefSchOptionsModel data = GetMultipleEntity<CaseManagement_GetEmpRefSchOptionsModel>(StoredProcedure.HC_CaseManagement_GetEmpRefSchOptions, searchList);
                data.PatientDetail.ScheduleID = model.ScheduleID;
                data.PatientDetail.ReferralTSDateID = model.ReferralTSDateID;
                response.Data = data;
                response.IsSuccess = true;
            }

            return response;
        }

        private static void PrivateDuty_SetSearchFilterForEmpRefSchList(SearchEmpRefSchOption model, List<SearchValueData> searchList, int pageIndex, int pageSize, string sortIndex, string sortDirection, string sortIndexArray)
        {

            searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
            //searchList.Add(new SearchValueData { Name = "EmployeeID", Value = Convert.ToString(model.EmployeeID) });
            searchList.Add(new SearchValueData { Name = "ScheduleID", Value = Convert.ToString(model.ScheduleID) });

            searchList.Add(new SearchValueData { Name = "EmployeeName", Value = model.EmployeeName });


            if (model.StartDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = model.StartDate.Value.ToString(Constants.DbDateTimeFormat) });
            if (model.EndDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = model.EndDate.Value.ToString(Constants.DbDateTimeFormat) });


            if (model.MileRadius.HasValue)
                searchList.Add(new SearchValueData { Name = "MileRadius", Value = Convert.ToString(model.MileRadius) });


            //if (model.SkillId.HasValue)
            //    searchList.Add(new SearchValueData { Name = "SkillId", Value = Convert.ToString(model.SkillId) });
            //if (model.PreferenceId.HasValue)
            //    searchList.Add(new SearchValueData { Name = "PreferenceId", Value = Convert.ToString(model.PreferenceId) });

            searchList.Add(new SearchValueData { Name = "StrSkillList", Value = Convert.ToString(model.StrSkillList) });
            searchList.Add(new SearchValueData { Name = "StrPreferenceList", Value = Convert.ToString(model.StrPreferenceList) });


            searchList.Add(new SearchValueData { Name = "SameDateWithTimeSlot", Value = Convert.ToString(model.SameDateWithTimeSlot) });

            searchList.Add(new SearchValueData { Name = "SortExpression", Value = Convert.ToString(sortIndex) });
            searchList.Add(new SearchValueData { Name = "SortType", Value = Convert.ToString(sortDirection) });
            searchList.Add(new SearchValueData { Name = "FromIndex", Value = Convert.ToString(pageIndex) });
            searchList.Add(new SearchValueData { Name = "PageSize", Value = Convert.ToString(pageSize) });
            searchList.Add(new SearchValueData { Name = "SortIndexArray", Value = Convert.ToString(sortIndexArray) });



        }
        private static void SetSearchFilterForEmpRefSchList(SearchEmpRefSchOption model, List<SearchValueData> searchList, int pageIndex, int pageSize, string sortIndex, string sortDirection, string sortIndexArray)
        {

            searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
            //searchList.Add(new SearchValueData { Name = "EmployeeID", Value = Convert.ToString(model.EmployeeID) });
            searchList.Add(new SearchValueData { Name = "ScheduleID", Value = Convert.ToString(model.ScheduleID) });

            searchList.Add(new SearchValueData { Name = "EmployeeName", Value = model.EmployeeName });


            if (model.StartDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = model.StartDate.Value.ToString(Constants.DbDateTimeFormat) });
            if (model.EndDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = model.EndDate.Value.ToString(Constants.DbDateTimeFormat) });


            if (model.MileRadius.HasValue)
                searchList.Add(new SearchValueData { Name = "MileRadius", Value = Convert.ToString(model.MileRadius) });


            //if (model.SkillId.HasValue)
            //    searchList.Add(new SearchValueData { Name = "SkillId", Value = Convert.ToString(model.SkillId) });
            //if (model.PreferenceId.HasValue)
            //    searchList.Add(new SearchValueData { Name = "PreferenceId", Value = Convert.ToString(model.PreferenceId) });

            searchList.Add(new SearchValueData { Name = "StrSkillList", Value = Convert.ToString(model.StrSkillList) });
            searchList.Add(new SearchValueData { Name = "StrPreferenceList", Value = Convert.ToString(model.StrPreferenceList) });


            searchList.Add(new SearchValueData { Name = "SameDateWithTimeSlot", Value = Convert.ToString(model.SameDateWithTimeSlot) });

            searchList.Add(new SearchValueData { Name = "SortExpression", Value = Convert.ToString(sortIndex) });
            searchList.Add(new SearchValueData { Name = "SortType", Value = Convert.ToString(sortDirection) });
            searchList.Add(new SearchValueData { Name = "FromIndex", Value = Convert.ToString(pageIndex) });
            searchList.Add(new SearchValueData { Name = "PageSize", Value = Convert.ToString(pageSize) });
            searchList.Add(new SearchValueData { Name = "SortIndexArray", Value = Convert.ToString(sortIndexArray) });
            searchList.Add(new SearchValueData { Name = "DDType_CareType", Value = Convert.ToString((int)Common.DDType.CareType) });
            searchList.Add(new SearchValueData { Name = "CareTypeID", Value = Convert.ToString(model.CareTypeID) });
        }


        private static void SetSearchFilterForRCLEmpRefSchList(SearchRCLEmpRefSchOption model, List<SearchValueData> searchList, int pageIndex, int pageSize, string sortIndex, string sortDirection, string sortIndexArray)
        {

            searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });

            searchList.Add(new SearchValueData { Name = "EmployeeName", Value = model.EmployeeName });

            if (model.MileRadius.HasValue)
                searchList.Add(new SearchValueData { Name = "MileRadius", Value = Convert.ToString(model.MileRadius) });

            searchList.Add(new SearchValueData { Name = "StrSkillList", Value = Convert.ToString(model.StrSkillList) });
            searchList.Add(new SearchValueData { Name = "StrPreferenceList", Value = Convert.ToString(model.StrPreferenceList) });

            searchList.Add(new SearchValueData { Name = "SortExpression", Value = Convert.ToString(sortIndex) });
            searchList.Add(new SearchValueData { Name = "SortType", Value = Convert.ToString(sortDirection) });
            searchList.Add(new SearchValueData { Name = "FromIndex", Value = Convert.ToString(pageIndex) });
            searchList.Add(new SearchValueData { Name = "PageSize", Value = Convert.ToString(pageSize) });
            searchList.Add(new SearchValueData { Name = "SortIndexArray", Value = Convert.ToString(sortIndexArray) });
        }

        public ServiceResponse CreateBulkSchedule(SearchEmpRefSchOption model, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            // EXEC HELP NEEDED
            if (model != null)
            {
                searchList.Add(new SearchValueData { Name = "PayorID", Value = Convert.ToString(model.PayorID) });
                searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
                searchList.Add(new SearchValueData { Name = "NewEmployeeID", Value = Convert.ToString(model.EmployeeID) });
                searchList.Add(new SearchValueData { Name = "ScheduleID", Value = Convert.ToString(model.ScheduleID) });
                searchList.Add(new SearchValueData { Name = "EmployeeTimeSlotDetailIDs", Value = model.EmployeeTimeSlotDetailIDs });
                if (model.StartDate.HasValue)
                    searchList.Add(new SearchValueData { Name = "StartDate", Value = model.StartDate.Value.ToString(Constants.DbDateTimeFormat) });
                if (model.EndDate.HasValue)
                    searchList.Add(new SearchValueData { Name = "EndDate", Value = model.EndDate.Value.ToString(Constants.DbDateTimeFormat) });
                searchList.Add(new SearchValueData { Name = "ScheduleStatusID", Value = Convert.ToString((int)ScheduleStatus.ScheduleStatuses.Confirmed) });
                searchList.Add(new SearchValueData { Name = "loggedInId", Value = Convert.ToString(loggedInId) });
                searchList.Add(new SearchValueData { Name = "SystemID", Value = Common.GetHostAddress() });
                searchList.Add(new SearchValueData { Name = "SameDateWithTimeSlot", Value = Convert.ToString(model.SameDateWithTimeSlot) });
                searchList.Add(new SearchValueData { Name = "ReferralTimeSlotDetailIDs", Value = Convert.ToString(model.ReferralTimeSlotDetailIDs) });
                searchList.Add(new SearchValueData { Name = "IsRescheduleAction", Value = Convert.ToString(model.IsRescheduleAction) });
                searchList.Add(new SearchValueData { Name = "IsForceUpdate", Value = Convert.ToString(model.IsForcePatientSchedules) });
                searchList.Add(new SearchValueData { Name = "CareTypeId", Value = Convert.ToString(model.CareTypeID) });
                searchList.Add(new SearchValueData { Name = "ReferralBillingAuthorizationID", Value = Convert.ToString(model.ReferralBillingAuthorizationID) });
                searchList.Add(new SearchValueData { Name = "IsVirtualVisit", Value = Convert.ToString(model.IsVirtualVisit) });
            }

            var result = GetScalar(StoredProcedure.CreateBulkSchedules, searchList);
            long data = Convert.ToInt64(result);
            if (data == -1)
                response.Message = Resource.EmployeeIsBlocked;
            if (data == -2)
                response.Message = Resource.EmployeeOnPTO;
            if (data == -5)
                response.Message = Resource.HCBillingAuthorizationValidation;
            if (data == -4)
                response.Data = data;
            if (data < 0)
                return response;

            if (data == 0)
            {
                if (model.SameDateWithTimeSlot)
                {
                    response.Message = Resource.SchdueleNotUpdated;
                }
                else
                {
                    response.Message = model.ScheduleID > 0
                        ? Resource.SchdueleNotUpdated
                        : Resource.SchdueleNotCreated;
                }
                response.ErrorCode = Constants.ErrorCode_Warning;
            }

            if (data > 0)
            {
                response.Message = String.Format(Resource.ScheduleCreated);
                if (model.IsVirtualVisit)
                {
                    bool isSentEmail = SendVirtualVisitsNotification(data, model.ReferralID, model.EmployeeID);
                    if (isSentEmail)
                    {
                    }

                }
            }
            response.Data = data;
            //response.Message = Resource.SchdueleCreated;
            response.IsSuccess = true;
            return response;
        }

        private bool SendVirtualVisitsNotification(long scheduleID, long referralID, long employeeID)
        {
            try
            {
                CacheHelper _cacheHelper = new CacheHelper();

                //Get referral details
                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "ReferralID", Value = referralID.ToString() });
                ReferralDetailsModel referralContact = GetMultipleEntity<ReferralDetailsModel>(StoredProcedure.GetReferralDetails, searchList);

                //Get employee details
                List<SearchValueData> searchEmployee = new List<SearchValueData> { new SearchValueData { Name = "EmployeeID", Value = employeeID.ToString() } };
                var employeeContact = GetEntity<Employee>(StoredProcedure.GetEmployeeDetails, searchEmployee);

                //Get schedule details
                var scheduleMaster = GetEntity<ScheduleMaster>(scheduleID);

                if (referralContact?.EmergencyContactDetails != null || employeeContact?.Email != null)
                {
                    string url = string.Format("{0}{1}{2}/{3}", _cacheHelper.SiteBaseURLMonile, Constants.HC_JoinMeeting, referralID, scheduleID);
                    string urlPatient = string.Format("{0}{1}{2}/{3}/{4}", _cacheHelper.SiteBaseURLMonile, Constants.HC_JoinMeeting_Patient, SessionHelper.CompanyName, referralID, scheduleID);
                    string strSubject = "Virtual Meeting Scheduled - myEZcare";

                    //Employee
                    string path = HttpContext.Current.Server.MapCustomPath("~/Assets/emailtemplates/schedule_visit.html");
                    var builder = new StringBuilder();
                    using (StreamReader SourceReader = File.OpenText(path))
                    {
                        builder.Append(SourceReader.ReadToEnd());
                    }
                    builder.Replace("##Link##", url);
                    builder.Replace("##GreetingName##", employeeContact?.FirstName);
                    builder.Replace("##StartTime##", Convert.ToDateTime(scheduleMaster?.StartDate).ToString());
                    builder.Replace("##EndTime##", Convert.ToDateTime(scheduleMaster?.EndDate).ToString());
                    builder.Replace("##EmployeeName##", employeeContact?.EmpGeneralNameFormat);
                    builder.Replace("##PatientName##", referralContact?.EmergencyContactDetails?.FullName);
                    builder.Replace("##OrgName##", _cacheHelper.SiteName);

                    //Patient
                    var builderPatient = new StringBuilder();
                    string pathPatient = HttpContext.Current.Server.MapCustomPath("~/Assets/emailtemplates/schedule_visit_patient.html");
                    using (StreamReader SourceReader = File.OpenText(pathPatient))
                    {
                        builderPatient.Append(SourceReader.ReadToEnd());
                    }
                    builderPatient.Replace("##Link##", urlPatient);
                    builderPatient.Replace("##GreetingName##", referralContact?.EmergencyContactDetails?.FirstName);
                    builderPatient.Replace("##StartTime##", Convert.ToDateTime(scheduleMaster?.StartDate).ToString());
                    builderPatient.Replace("##EndTime##", Convert.ToDateTime(scheduleMaster?.EndDate).ToString());
                    builderPatient.Replace("##EmployeeName##", employeeContact?.EmpGeneralNameFormat);
                    builderPatient.Replace("##PatientName##", referralContact?.EmergencyContactDetails?.FullName);
                    builderPatient.Replace("##OrgName##", _cacheHelper.SiteName);

                    //Employee
                    if (employeeContact?.Email != null)
                    {
                        Common.SendEmail(strSubject, _cacheHelper.FromEmail, employeeContact?.Email, builder.ToString(), orgTemplateWrapper: true);
                    }
                    if (employeeContact?.PhoneWork != null)
                    {
                        Common.SendSms(employeeContact?.PhoneWork, builder.ToString(), EnumEmailType.HomeCare_ScheduleVisit_SMS.ToString());
                    }
                    //Patient
                    if (referralContact?.EmergencyContactDetails?.Email != null)
                    {
                        Common.SendEmail(strSubject, _cacheHelper.FromEmail, referralContact?.EmergencyContactDetails?.Email, builderPatient.ToString(), orgTemplateWrapper: true);
                    }
                    if (referralContact?.EmergencyContactDetails?.Phone1 != null)
                    {
                        Common.SendSms(referralContact?.EmergencyContactDetails?.Phone1, builderPatient.ToString(), EnumEmailType.HomeCare_ScheduleVisit_SMS.ToString());
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public ServiceResponse SendVirtualVisitsReminderNotification(long scheduleID, long referralID, long employeeID, bool sendSMS, bool sendEmail)
        {
            var response = new ServiceResponse();
            try
            {
                CacheHelper _cacheHelper = new CacheHelper();

                //Get referral details
                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "ReferralID", Value = referralID.ToString() });
                ReferralDetailsModel referralContact = GetMultipleEntity<ReferralDetailsModel>(StoredProcedure.GetReferralDetails, searchList);

                //Get employee details
                List<SearchValueData> searchEmployee = new List<SearchValueData> { new SearchValueData { Name = "EmployeeID", Value = employeeID.ToString() } };
                var employeeContact = GetEntity<Employee>(StoredProcedure.GetEmployeeDetails, searchEmployee);

                //Get schedule details
                var scheduleMaster = GetEntity<ScheduleMaster>(scheduleID);

                if (referralContact?.EmergencyContactDetails != null || employeeContact?.Email != null)
                {
                    string url = string.Format("{0}{1}{2}/{3}", _cacheHelper.SiteBaseURLMonile, Constants.HC_JoinMeeting, referralID, scheduleID);
                    string urlPatient = string.Format("{0}{1}{2}/{3}/{4}", _cacheHelper.SiteBaseURLMonile, Constants.HC_JoinMeeting_Patient, SessionHelper.CompanyName, referralID, scheduleID);
                    string strSubject = "Virtual Meeting Sceduled - myEZcare";

                    //Employee
                    var builder = new StringBuilder();
                    string path = HttpContext.Current.Server.MapCustomPath("~/Assets/emailtemplates/schedule_visit_reminder.html");
                    using (StreamReader SourceReader = File.OpenText(path))
                    {
                        builder.Append(SourceReader.ReadToEnd());
                    }
                    builder.Replace("##Link##", url);
                    builder.Replace("##GreetingName##", employeeContact?.FirstName);
                    builder.Replace("##StartTime##", Convert.ToDateTime(scheduleMaster?.StartDate).ToString());
                    builder.Replace("##EndTime##", Convert.ToDateTime(scheduleMaster?.EndDate).ToString());
                    builder.Replace("##EmployeeName##", employeeContact?.EmpGeneralNameFormat);
                    builder.Replace("##PatientName##", referralContact?.EmergencyContactDetails?.FullName);
                    builder.Replace("##OrgName##", _cacheHelper.SiteName);

                    //Patient
                    var builderPatient = new StringBuilder();
                    string pathPatient = HttpContext.Current.Server.MapCustomPath("~/Assets/emailtemplates/schedule_visit_patient_reminder.html");
                    using (StreamReader SourceReader = File.OpenText(pathPatient))
                    {
                        builderPatient.Append(SourceReader.ReadToEnd());
                    }
                    builderPatient.Replace("##Link##", urlPatient);
                    builderPatient.Replace("##GreetingName##", referralContact?.EmergencyContactDetails?.FirstName);
                    builderPatient.Replace("##StartTime##", Convert.ToDateTime(scheduleMaster?.StartDate).ToString());
                    builderPatient.Replace("##EndTime##", Convert.ToDateTime(scheduleMaster?.EndDate).ToString());
                    builderPatient.Replace("##EmployeeName##", employeeContact?.EmpGeneralNameFormat);
                    builderPatient.Replace("##PatientName##", referralContact?.EmergencyContactDetails?.FullName);
                    builderPatient.Replace("##OrgName##", _cacheHelper.SiteName);

                    //Employee
                    if (sendEmail && employeeContact?.Email != null)
                    {
                        Common.SendEmail(strSubject, _cacheHelper.FromEmail, employeeContact?.Email, builder.ToString(), orgTemplateWrapper: true);
                    }
                    if (sendSMS && employeeContact?.PhoneWork != null)
                    {
                        Common.SendSms(employeeContact?.PhoneWork, builder.ToString(), EnumEmailType.HomeCare_ScheduleVisit_SMS.ToString());
                    }
                    //Patient
                    if (sendEmail && referralContact?.EmergencyContactDetails?.Email != null)
                    {
                        Common.SendEmail(strSubject, _cacheHelper.FromEmail, referralContact?.EmergencyContactDetails?.Email, builderPatient.ToString(), orgTemplateWrapper: true);
                    }
                    if (sendSMS && referralContact?.EmergencyContactDetails?.Phone1 != null)
                    {
                        Common.SendSms(referralContact?.EmergencyContactDetails?.Phone1, builderPatient.ToString(), EnumEmailType.HomeCare_ScheduleVisit_SMS.ToString());
                    }

                    response.Data = true;
                    response.Message = "Reminder has been sent.";
                    response.IsSuccess = true;
                    return response;
                }
                response.Data = false;
                response.Message = "Error occured while sending reminder.";
                response.IsSuccess = false;

                return response;
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Message = "Error occured while sending reminder. " + ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public ServiceResponse HC_PrivateDuty_CreateBulkSchedule(SearchEmpRefSchOption model, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();



            // EXEC HELP NEEDED
            if (model != null)
            {

                searchList.Add(new SearchValueData { Name = "PayorID", Value = Convert.ToString(model.PayorID) });
                searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
                searchList.Add(new SearchValueData { Name = "NewEmployeeID", Value = Convert.ToString(model.EmployeeID) });
                searchList.Add(new SearchValueData { Name = "ScheduleID", Value = Convert.ToString(model.ScheduleID) });
                searchList.Add(new SearchValueData { Name = "EmployeeTimeSlotDetailIDs", Value = model.EmployeeTimeSlotDetailIDs });
                if (model.StartDate.HasValue)
                    searchList.Add(new SearchValueData { Name = "StartDate", Value = model.StartDate.Value.ToString(Constants.DbDateTimeFormat) });
                if (model.EndDate.HasValue)
                    searchList.Add(new SearchValueData { Name = "EndDate", Value = model.EndDate.Value.ToString(Constants.DbDateTimeFormat) });

                searchList.Add(new SearchValueData { Name = "ScheduleStatusID", Value = Convert.ToString((int)ScheduleStatus.ScheduleStatuses.Confirmed) });
                searchList.Add(new SearchValueData { Name = "loggedInId", Value = Convert.ToString(loggedInId) });
                searchList.Add(new SearchValueData { Name = "SystemID", Value = Common.GetHostAddress() });
                searchList.Add(new SearchValueData { Name = "SameDateWithTimeSlot", Value = Convert.ToString(model.SameDateWithTimeSlot) });
                searchList.Add(new SearchValueData { Name = "ReferralTimeSlotDetailIDs", Value = Convert.ToString(model.ReferralTimeSlotDetailIDs) });
                searchList.Add(new SearchValueData { Name = "IsRescheduleAction", Value = Convert.ToString(model.IsRescheduleAction) });



            }


            var data = (int)GetScalar(StoredProcedure.HC_PrivateDuty_CreateBulkSchedules, searchList);

            if (data == -1)
                response.Message = Resource.EmployeeIsBlocked;

            if (data == -2)
                response.Message = Resource.EmployeeOnPTO;


            if (data < 0)
                return response;

            if (data == 0)
            {
                if (model.SameDateWithTimeSlot)
                {
                    response.Message = Resource.SchdueleNotUpdated;
                }
                else
                {

                    response.Message = model.ScheduleID > 0
                        ? Resource.SchdueleNotUpdated
                        : Resource.SchdueleNotCreated;
                }
                response.ErrorCode = Constants.ErrorCode_Warning;
            }

            if (data > 0)
            {
                response.Message = String.Format(Resource.ScheduleCreated);


            }
            response.Data = data;
            //response.Message = Resource.SchdueleCreated;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse HC_SaveReferralCSVFile(HttpRequestBase httpRequestBase, long loggedInUserID)
        {
            var response = new ServiceResponse();
            string basePath = String.Format(_cacheHelper.ScheduleDayCareUploadPath, _cacheHelper.Domain);

            HttpPostedFileBase file = httpRequestBase.Files[0];
            response = Common.SaveFile(file, basePath);
            response.Message = response.IsSuccess ? Resource.CSVFileUploaded : Resource.FileUploadFailedNoFileSelected;
            return response;
        }

        public ServiceResponse CreateBulkScheduleUsingCSV(ReferralCsvModel referralCsvModel, long loggedInUserID)
        {
            var response = new ServiceResponse();
            var path = HttpContext.Current.Server.MapCustomPath(referralCsvModel.FilePath);
            DataImportDataProvider dataprovider = new DataImportDataProvider();

            List<SearchValueData> schParam = new List<SearchValueData>();
            schParam.Add(new SearchValueData("ScheduleDate", referralCsvModel.ScheduleDate.ToString(Constants.DbDateFormat)));
            schParam.Add(new SearchValueData("FacilityID", Convert.ToString(referralCsvModel.FacilityID)));
            schParam.Add(new SearchValueData("SystemID", Common.GetHostAddress()));
            response = dataprovider.ReadAndValidateExcel(path, Constants.TempReferralTable, Constants.TempReferralColumns, SessionHelper.LoggedInID, schParam);
            if (response.IsSuccess == false)
            {
                List<TempReferral> tempReferral = GetEntityList<TempReferral>("SELECT * FROM TempReferral WHERE CreatedBy=" + SessionHelper.LoggedInID + " AND ErrorMessage != 'Done'");
                response.Data = tempReferral;
            }

            File.Delete(path);
            response.Message = response.IsSuccess ? Resource.ScheduleCreated : Resource.SomethingWentWrong;

            return response;
        }

        public ServiceResponse HC_DayCare_CreateBulkSchedule(SearchEmpRefSchOption model, long loggedInId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            // EXEC HELP NEEDED
            if (model != null)
            {

                searchList.Add(new SearchValueData { Name = "PayorID", Value = Convert.ToString(model.PayorID) });
                searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
                searchList.Add(new SearchValueData { Name = "FacilityID", Value = Convert.ToString(model.FacilityID) });
                searchList.Add(new SearchValueData { Name = "ScheduleID", Value = Convert.ToString(model.ScheduleID) });

                if (model.StartDate.HasValue)
                    searchList.Add(new SearchValueData { Name = "StartDate", Value = model.StartDate.Value.ToString(Constants.DbDateTimeFormat) });
                if (model.EndDate.HasValue)
                    searchList.Add(new SearchValueData { Name = "EndDate", Value = model.EndDate.Value.ToString(Constants.DbDateTimeFormat) });

                searchList.Add(new SearchValueData { Name = "ScheduleStatusID", Value = Convert.ToString((int)ScheduleStatus.ScheduleStatuses.Confirmed) });
                searchList.Add(new SearchValueData { Name = "loggedInId", Value = Convert.ToString(loggedInId) });
                searchList.Add(new SearchValueData { Name = "SystemID", Value = Common.GetHostAddress() });
                searchList.Add(new SearchValueData { Name = "SameDateWithTimeSlot", Value = Convert.ToString(model.SameDateWithTimeSlot) });
                searchList.Add(new SearchValueData { Name = "ReferralTimeSlotDetailIDs", Value = Convert.ToString(model.ReferralTimeSlotDetailIDs) });
                searchList.Add(new SearchValueData { Name = "IsRescheduleAction", Value = Convert.ToString(model.IsRescheduleAction) });
                searchList.Add(new SearchValueData { Name = "ReferralBillingAuthorizationID", Value = Convert.ToString(model.ReferralBillingAuthorizationID) });


            }


            var data = (int)GetScalar(StoredProcedure.HC_DayCare_CreateBulkSchedules, searchList);



            if (data < 0)
                return response;

            if (data == 0)
            {
                if (model.SameDateWithTimeSlot)
                {
                    response.Message = Resource.SchdueleNotUpdated;
                }
                else
                {

                    response.Message = model.ScheduleID > 0
                        ? Resource.SchdueleNotUpdated
                        : Resource.SchdueleNotCreated;
                }
                response.ErrorCode = Constants.ErrorCode_Warning;
            }

            if (data > 0)
            {
                response.Message = String.Format(Resource.ScheduleCreated);


            }
            response.Data = data;
            //response.Message = Resource.SchdueleCreated;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse GetSchEmpRefSkills(SearchEmpRefMatchModel model)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
            searchList.Add(new SearchValueData { Name = "EmployeeID", Value = Convert.ToString(model.EmployeeID) });
            searchList.Add(new SearchValueData { Name = "PreferenceType", Value = model.MatchType });

            SchEmpRefModel data = GetMultipleEntity<SchEmpRefModel>(StoredProcedure.GetSchEmpRefSkills, searchList);
            data.SearchEmpRefMatchModel = model;

            response.Data = data;
            response.IsSuccess = true;
            return response;
        }



        public ServiceResponse HC_PrivateDuty_GetSchEmpRefSkills(SearchEmpRefMatchModel model)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
            searchList.Add(new SearchValueData { Name = "EmployeeID", Value = Convert.ToString(model.EmployeeID) });
            searchList.Add(new SearchValueData { Name = "PreferenceType", Value = model.MatchType });

            SchEmpRefModel data = GetMultipleEntity<SchEmpRefModel>(StoredProcedure.HC_PrivateDuty_GetSchEmpRefSkills, searchList);
            data.SearchEmpRefMatchModel = model;

            response.Data = data;
            response.IsSuccess = true;
            return response;
        }


        public ServiceResponse DeleteEmpRefSchedule(DeleteEmpRefScheduleModel model, long loggedInId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ReferralTimeSlotDetailID", Value = Convert.ToString(model.ReferralTimeSlotDetailID) });
            searchList.Add(new SearchValueData { Name = "ReferralTimeSlotMasterID", Value = Convert.ToString(model.ReferralTimeSlotMasterID) });
            searchList.Add(new SearchValueData { Name = "Day", Value = Convert.ToString(model.Day) });
            searchList.Add(new SearchValueData { Name = "loggedInId", Value = Convert.ToString(loggedInId) });
            //searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToString(model.StartDate) });
            //searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToString(model.EndDate) });

            if (model.StartDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = model.StartDate.Value.ToString(Constants.DbDateFormat) });
            if (model.EndDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = model.EndDate.Value.ToString(Constants.DbDateFormat) });


            //searchList.Add(new SearchValueData { Name = "StartTime", Value = Convert.ToString(model.StartTime) });
            //searchList.Add(new SearchValueData { Name = "EndTime", Value = Convert.ToString(model.EndTime) });

            int data = (int)GetScalar(StoredProcedure.DeleteEmpRefSchedule, searchList);

            if (data == 1)
            {
                response.Message = Resource.ScheduleUnassignedSuccessfully;
                response.IsSuccess = true;
            }
            else
            {
                response.IsSuccess = false;
            }
            response.Data = data;

            return response;
        }


        public ServiceResponse HC_PrivateDuty_DeleteEmpRefSchedule(DeleteEmpRefScheduleModel model, long loggedInId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ReferralTimeSlotDetailID", Value = Convert.ToString(model.ReferralTimeSlotDetailID) });
            searchList.Add(new SearchValueData { Name = "ReferralTimeSlotMasterID", Value = Convert.ToString(model.ReferralTimeSlotMasterID) });
            searchList.Add(new SearchValueData { Name = "Day", Value = Convert.ToString(model.Day) });
            searchList.Add(new SearchValueData { Name = "loggedInId", Value = Convert.ToString(loggedInId) });
            //searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToString(model.StartDate) });
            //searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToString(model.EndDate) });

            if (model.StartDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = model.StartDate.Value.ToString(Constants.DbDateFormat) });
            if (model.EndDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = model.EndDate.Value.ToString(Constants.DbDateFormat) });


            //searchList.Add(new SearchValueData { Name = "StartTime", Value = Convert.ToString(model.StartTime) });
            //searchList.Add(new SearchValueData { Name = "EndTime", Value = Convert.ToString(model.EndTime) });

            int data = (int)GetScalar(StoredProcedure.HC_PrivateDuty_DeleteEmpRefSchedule, searchList);

            if (data == 1)
            {
                response.Message = Resource.ScheduleUnassignedSuccessfully;
                response.IsSuccess = true;
            }
            else
            {
                response.IsSuccess = false;
            }
            response.Data = data;

            return response;
        }

        public ServiceResponse HC_DayCare_DeleteEmpRefSchedule(DeleteEmpRefScheduleModel model, long loggedInId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ReferralTimeSlotDetailID", Value = Convert.ToString(model.ReferralTimeSlotDetailID) });
            searchList.Add(new SearchValueData { Name = "ReferralTimeSlotMasterID", Value = Convert.ToString(model.ReferralTimeSlotMasterID) });
            searchList.Add(new SearchValueData { Name = "Day", Value = Convert.ToString(model.Day) });
            searchList.Add(new SearchValueData { Name = "loggedInId", Value = Convert.ToString(loggedInId) });
            //searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToString(model.StartDate) });
            //searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToString(model.EndDate) });

            if (model.StartDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = model.StartDate.Value.ToString(Constants.DbDateFormat) });
            if (model.EndDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = model.EndDate.Value.ToString(Constants.DbDateFormat) });


            //searchList.Add(new SearchValueData { Name = "StartTime", Value = Convert.ToString(model.StartTime) });
            //searchList.Add(new SearchValueData { Name = "EndTime", Value = Convert.ToString(model.EndTime) });

            int data = (int)GetScalar(StoredProcedure.HC_DayCare_DeleteEmpRefSchedule, searchList);

            if (data == 1)
            {
                response.Message = Resource.ScheduleUnassignedSuccessfully;
                response.IsSuccess = true;
            }
            else
            {
                response.IsSuccess = false;
            }
            response.Data = data;

            return response;
        }

        public ServiceResponse GetAssignedEmployees(ReferralTimeSlotModel model)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ReferralTimeSlotDetailID", Value = Convert.ToString(model.ReferralTimeSlotDetailID) });
            searchList.Add(new SearchValueData { Name = "ReferralTimeSlotMasterID", Value = Convert.ToString(model.ReferralTimeSlotMasterID) });
            //searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
            searchList.Add(new SearchValueData { Name = "Day", Value = Convert.ToString(model.Day) });
            searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToString(model.StartDate.ToString(Constants.DbDateFormat)) });
            searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToString(model.EndDate.ToString(Constants.DbDateFormat)) });


            List<GetAssignedEmployeeModel> assignedEmployees = GetEntityList<GetAssignedEmployeeModel>(StoredProcedure.GetAssignedEmployees, searchList);
            //List<GetAssignedEmployee> assignedEmployees = GetEntityList<GetAssignedEmployee>(StoredProcedure.GetAssignedEmployees, searchList);
            //List<GetAssignedEmployeeGroup> data = assignedEmployees.GroupBy(c => new
            //                                                                {
            //                                                                    c.EmployeeID,
            //                                                                    c.EmployeeName,
            //                                                                    c.MobileNumber,
            //                                                                    c.EncryptedEmployeeID
            //                                                                }).Select(
            //                                                                    grp => new GetAssignedEmployeeGroup
            //                                                                    {
            //                                                                        EmployeeID = grp.Key.EmployeeID,
            //                                                                        EmployeeName = grp.Key.EmployeeName,
            //                                                                        MobileNumber = grp.Key.MobileNumber,
            //                                                                        //EncryptedEmployeeID = Crypto.Encrypt(Convert.ToString(grp.Key.EmployeeID)),
            //                                                                        GetAssignedEmployee = grp.ToList()
            //                                                                    }
            //                                                                    ).ToList();


            //foreach (var getAssignedEmployeeGroup in data)
            //    getAssignedEmployeeGroup.EncryptedEmployeeID = Crypto.Encrypt(Convert.ToString(getAssignedEmployeeGroup.EmployeeID));

            response.IsSuccess = true;
            response.Data = assignedEmployees;

            return response;
        }



        public ServiceResponse HC_PrivateDuty_GetAssignedEmployees(ReferralTimeSlotModel model)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ReferralTimeSlotDetailID", Value = Convert.ToString(model.ReferralTimeSlotDetailID) });
            searchList.Add(new SearchValueData { Name = "ReferralTimeSlotMasterID", Value = Convert.ToString(model.ReferralTimeSlotMasterID) });
            //searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
            searchList.Add(new SearchValueData { Name = "Day", Value = Convert.ToString(model.Day) });
            searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToString(model.StartDate) });
            searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToString(model.EndDate) });


            List<GetAssignedEmployeeModel> assignedEmployees = GetEntityList<GetAssignedEmployeeModel>(StoredProcedure.HC_PrivateDuty_GetAssignedEmployees, searchList);

            response.IsSuccess = true;
            response.Data = assignedEmployees;

            return response;
        }


        public ServiceResponse HC_DayCare_GetAssignedFacilities(ReferralTimeSlotModel model)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ReferralTimeSlotDetailID", Value = Convert.ToString(model.ReferralTimeSlotDetailID) });
            searchList.Add(new SearchValueData { Name = "ReferralTimeSlotMasterID", Value = Convert.ToString(model.ReferralTimeSlotMasterID) });
            //searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
            searchList.Add(new SearchValueData { Name = "Day", Value = Convert.ToString(model.Day) });
            searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToString(model.StartDate) });
            searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToString(model.EndDate) });

            List<GetAssignedFacilityModel> assignedEmployees = GetEntityList<GetAssignedFacilityModel>(StoredProcedure.HC_DayCare_GetAssignedFacilities, searchList);

            response.IsSuccess = true;
            response.Data = assignedEmployees;

            return response;
        }

        public ServiceResponse OnHoldUnHoldAction(PatientHoldDetail model, long loggedInId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ReferralOnHoldDetailID", Value = Convert.ToString(model.ReferralOnHoldDetailID) });
            searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
            searchList.Add(new SearchValueData { Name = "PatientOnHoldAction", Value = Convert.ToString(model.PatientOnHoldAction) });
            searchList.Add(new SearchValueData { Name = "PatientOnHoldReason", Value = Convert.ToString(model.PatientOnHoldReason) });
            searchList.Add(new SearchValueData { Name = "loggedInId", Value = Convert.ToString(loggedInId) });
            searchList.Add(new SearchValueData { Name = "SystemID", Value = Common.GetHostAddress() });

            if (model.StartDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = model.StartDate.Value.ToString(Constants.DbDateFormat) });
            if (model.EndDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = model.EndDate.Value.ToString(Constants.DbDateFormat) });
            if (model.UnHoldDate.HasValue)
                searchList.Add(new SearchValueData { Name = "UnHoldDate", Value = model.UnHoldDate.Value.ToString(Constants.DbDateFormat) });


            //searchList.Add(new SearchValueData { Name = "StartTime", Value = Convert.ToString(model.StartTime) });
            //searchList.Add(new SearchValueData { Name = "EndTime", Value = Convert.ToString(model.EndTime) });

            PatientOnHoldModel details = GetMultipleEntity<PatientOnHoldModel>(StoredProcedure.OnHoldUnHoldAction, searchList);


            if (details.Result == -1)
                response.Message = Resource.OnHoldDateOverlapping;


            if (details.Result == 1)
            {
                //Notify the employee if scheduled with patient
                if (model.NotifyEmployee && model.ReferralOnHoldDetailID == 0)
                {
                    var domainName = SessionHelper.DomainName;
                    var notificationType = (int)Mobile_Notification.NotificationTypes.PatientOnHoldNotification;
                    //var body = String.Format("Patient on hold from {0} to {1}", model.StartDate.Value.ToString(), model.EndDate.Value.ToString());

                    foreach (DeniedNotificationUserDetails item in details.notificationUserDetails)
                    {
                        var body = String.Format("Patient '{0}' on hold from {1} to {2}", item.PatientName, item.StartDate.ToString(Constants.GlobalDateFormat), item.EndDate.ToString(Constants.GlobalDateFormat));

                        FcmManager fcmManager = new FcmManager(new FcmManagerOptions
                        {
                            AuthenticationKey = ConfigSettings.FcmAuthenticationKey,
                            SenderId = ConfigSettings.FcmSenderId
                        });

                        var fcmResponse = fcmManager.SendMessage(new FcmMessage
                        {
                            RegistrationIds = new List<string> { item.FcmTokenId },
                            Notification = item.DeviceType.ToLower() == Constants.ios ? new FcmNotification
                            {
                                Body = body,
                                Title = domainName
                            } : null,
                            Data = new MobileNotificationModel
                            {
                                SiteName = domainName,
                                Body = body,
                                NotificationType = notificationType
                            },
                        });


                        if (fcmResponse.MessagesSucceededCount > 0)
                        {
                            List<SearchValueData> srchList = new List<SearchValueData>();
                            srchList.Add(new SearchValueData { Name = "Title", Value = body });
                            srchList.Add(new SearchValueData { Name = "EmployeeID", Value = Convert.ToString(item.EmployeeID) });
                            srchList.Add(new SearchValueData { Name = "NotificationType", Value = Convert.ToString(notificationType) });
                            srchList.Add(new SearchValueData { Name = "NotificationStatus", Value = Convert.ToString((int)Mobile_Notification.NotificationStatuses.Sent) });
                            srchList.Add(new SearchValueData { Name = "ServerDateTime", Value = DateTime.UtcNow.ToString(Constants.DbDateTimeFormat) });
                            GetScalar(StoredProcedure.OnHoldNotificationLog, srchList);
                        }
                    }
                }

                response.Message = Resource.ScheduleUnassignedSuccessfully;
                response.IsSuccess = true;
            }
            else
            {
                response.IsSuccess = false;
            }
            response.Data = details.Result;

            return response;
        }


        public ServiceResponse HC_PrivateDuty_OnHoldUnHoldAction(PatientHoldDetail model, long loggedInId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ReferralOnHoldDetailID", Value = Convert.ToString(model.ReferralOnHoldDetailID) });
            searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
            searchList.Add(new SearchValueData { Name = "PatientOnHoldAction", Value = Convert.ToString(model.PatientOnHoldAction) });
            searchList.Add(new SearchValueData { Name = "PatientOnHoldReason", Value = Convert.ToString(model.PatientOnHoldReason) });
            searchList.Add(new SearchValueData { Name = "loggedInId", Value = Convert.ToString(loggedInId) });
            searchList.Add(new SearchValueData { Name = "SystemID", Value = Common.GetHostAddress() });

            if (model.StartDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = model.StartDate.Value.ToString(Constants.DbDateFormat) });
            if (model.EndDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = model.EndDate.Value.ToString(Constants.DbDateFormat) });
            if (model.UnHoldDate.HasValue)
                searchList.Add(new SearchValueData { Name = "UnHoldDate", Value = model.UnHoldDate.Value.ToString(Constants.DbDateFormat) });


            //searchList.Add(new SearchValueData { Name = "StartTime", Value = Convert.ToString(model.StartTime) });
            //searchList.Add(new SearchValueData { Name = "EndTime", Value = Convert.ToString(model.EndTime) });

            PatientOnHoldModel details = GetMultipleEntity<PatientOnHoldModel>(StoredProcedure.HC_PrivateDuty_OnHoldUnHoldAction, searchList);


            if (details.Result == -1)
                response.Message = Resource.OnHoldDateOverlapping;


            if (details.Result == 1)
            {
                //Notify the employee if scheduled with patient
                if (model.NotifyEmployee && model.ReferralOnHoldDetailID == 0)
                {
                    var domainName = SessionHelper.DomainName;
                    var notificationType = (int)Mobile_Notification.NotificationTypes.PatientOnHoldNotification;
                    //var body = String.Format("Patient on hold from {0} to {1}", model.StartDate.Value.ToString(), model.EndDate.Value.ToString());

                    foreach (DeniedNotificationUserDetails item in details.notificationUserDetails)
                    {
                        var body = String.Format("Patient '{0}' on hold from {1} to {2}", item.PatientName, item.StartDate.ToString(Constants.GlobalDateFormat), item.EndDate.ToString(Constants.GlobalDateFormat));

                        FcmManager fcmManager = new FcmManager(new FcmManagerOptions
                        {
                            AuthenticationKey = ConfigSettings.FcmAuthenticationKey,
                            SenderId = ConfigSettings.FcmSenderId
                        });

                        var fcmResponse = fcmManager.SendMessage(new FcmMessage
                        {
                            RegistrationIds = new List<string> { item.FcmTokenId },
                            Notification = item.DeviceType.ToLower() == Constants.ios ? new FcmNotification
                            {
                                Body = body,
                                Title = domainName
                            } : null,
                            Data = new MobileNotificationModel
                            {
                                SiteName = domainName,
                                Body = body,
                                NotificationType = notificationType
                            },
                        });


                        if (fcmResponse.MessagesSucceededCount > 0)
                        {
                            List<SearchValueData> srchList = new List<SearchValueData>();
                            srchList.Add(new SearchValueData { Name = "Title", Value = body });
                            srchList.Add(new SearchValueData { Name = "EmployeeID", Value = Convert.ToString(item.EmployeeID) });
                            srchList.Add(new SearchValueData { Name = "NotificationType", Value = Convert.ToString(notificationType) });
                            srchList.Add(new SearchValueData { Name = "NotificationStatus", Value = Convert.ToString((int)Mobile_Notification.NotificationStatuses.Sent) });
                            srchList.Add(new SearchValueData { Name = "ServerDateTime", Value = DateTime.UtcNow.ToString(Constants.DbDateTimeFormat) });
                            GetScalar(StoredProcedure.OnHoldNotificationLog, srchList);
                        }
                    }
                }

                response.Message = Resource.ScheduleUnassignedSuccessfully;
                response.IsSuccess = true;
            }
            else
            {
                response.IsSuccess = false;
            }
            response.Data = details.Result;

            return response;
        }

        public ServiceResponse HC_DayCare_SavePatientAttendance(ScheduleAttendaceDetail model, long loggedInId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ScheduleID", Value = Convert.ToString(model.ScheduleID) });
            searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
            searchList.Add(new SearchValueData { Name = "IsPatientAttendedSchedule", Value = Convert.ToString(model.IsPatientAttendedSchedule) });
            searchList.Add(new SearchValueData { Name = "PateintAbsentReason", Value = Convert.ToString(model.AbsentReason) });
            searchList.Add(new SearchValueData { Name = "loggedInId", Value = Convert.ToString(loggedInId) });
            searchList.Add(new SearchValueData { Name = "SystemID", Value = Common.GetHostAddress() });
            GetScalar(StoredProcedure.HC_DayCare_SavePatientAttendance, searchList);
            response.Message = Resource.PatientAttendanceMarkedSuccessfully;
            response.IsSuccess = true;
            return response;
        }


        public ServiceResponse HC_DayCare_OnHoldUnHoldAction(PatientHoldDetail model, long loggedInId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ReferralOnHoldDetailID", Value = Convert.ToString(model.ReferralOnHoldDetailID) });
            searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
            searchList.Add(new SearchValueData { Name = "PatientOnHoldAction", Value = Convert.ToString(model.PatientOnHoldAction) });
            searchList.Add(new SearchValueData { Name = "PatientOnHoldReason", Value = Convert.ToString(model.PatientOnHoldReason) });
            searchList.Add(new SearchValueData { Name = "loggedInId", Value = Convert.ToString(loggedInId) });
            searchList.Add(new SearchValueData { Name = "SystemID", Value = Common.GetHostAddress() });

            if (model.StartDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = model.StartDate.Value.ToString(Constants.DbDateFormat) });
            if (model.EndDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = model.EndDate.Value.ToString(Constants.DbDateFormat) });
            if (model.UnHoldDate.HasValue)
                searchList.Add(new SearchValueData { Name = "UnHoldDate", Value = model.UnHoldDate.Value.ToString(Constants.DbDateFormat) });


            int data = (int)GetScalar(StoredProcedure.HC_DayCare_OnHoldUnHoldAction, searchList);


            if (data == -1)
                response.Message = Resource.OnHoldDateOverlapping;


            if (data == 1)
            {
                response.Message = Resource.ScheduleUnassignedSuccessfully;
                response.IsSuccess = true;
            }
            else
            {
                response.IsSuccess = false;
            }
            response.Data = data;

            return response;
        }

        public ServiceResponse HC_CaseManagement_OnHoldUnHoldAction(PatientHoldDetail model, long loggedInId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ReferralOnHoldDetailID", Value = Convert.ToString(model.ReferralOnHoldDetailID) });
            searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(model.ReferralID) });
            searchList.Add(new SearchValueData { Name = "PatientOnHoldAction", Value = Convert.ToString(model.PatientOnHoldAction) });
            searchList.Add(new SearchValueData { Name = "PatientOnHoldReason", Value = Convert.ToString(model.PatientOnHoldReason) });
            searchList.Add(new SearchValueData { Name = "loggedInId", Value = Convert.ToString(loggedInId) });
            searchList.Add(new SearchValueData { Name = "SystemID", Value = Common.GetHostAddress() });

            if (model.StartDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = model.StartDate.Value.ToString(Constants.DbDateFormat) });
            if (model.EndDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = model.EndDate.Value.ToString(Constants.DbDateFormat) });
            if (model.UnHoldDate.HasValue)
                searchList.Add(new SearchValueData { Name = "UnHoldDate", Value = model.UnHoldDate.Value.ToString(Constants.DbDateFormat) });

            int data = (int)GetScalar(StoredProcedure.HC_CaseManagement_OnHoldUnHoldAction, searchList);


            if (data == -1)
                response.Message = Resource.OnHoldDateOverlapping;


            if (data == 1)
            {
                response.Message = Resource.ScheduleUnassignedSuccessfully;
                response.IsSuccess = true;
            }
            else
            {
                response.IsSuccess = false;
            }
            response.Data = data;

            return response;
        }

        public ServiceResponse OnRemoveScheduleAction(RemoveScheduleModel model, long loggedInId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ScheduleID", Value = Convert.ToString(model.ScheduleID) });
            searchList.Add(new SearchValueData { Name = "RemoveScheduleNotes", Value = Convert.ToString(model.RemoveScheduleReason) });


            if (model.IsSaveNoteOnly)
            {
                searchList.Add(new SearchValueData { Name = "isSaveNoteOnly", Value = "true", DataType = "bit" });
            }

            searchList.Add(new SearchValueData { Name = "loggedInId", Value = Convert.ToString(loggedInId) });
            searchList.Add(new SearchValueData { Name = "SystemID", Value = Common.GetHostAddress() });
            searchList.Add(new SearchValueData { Name = "ReferralTSDateID", Value = Convert.ToString(model.ReferralTSDateID) });




            //searchList.Add(new SearchValueData { Name = "StartTime", Value = Convert.ToString(model.StartTime) });
            //searchList.Add(new SearchValueData { Name = "EndTime", Value = Convert.ToString(model.EndTime) });

            int data = (int)GetScalar(StoredProcedure.OnRemoveScheduleAction, searchList);
            if (data == 1)
            {
                response.Message = model.IsSaveNoteOnly ? Resource.NoteSavedSuccessfully : Resource.ScheduleUnassignedSuccessfully;
                response.IsSuccess = true;
            }
            return response;
        }

        public ServiceResponse HC_SaveNewSchedule(ChangeScheduleModel model, long loggedInId)
        {
            var response = new ServiceResponse();
            try
            {

                TimeSpan StartTime = new TimeSpan();
                TimeSpan EndTime = new TimeSpan();
                if (!string.IsNullOrEmpty(model.StartTime))
                {
                    string[] timeparts = model.StartTime.Split(':');
                    timeparts[0] = timeparts[0] != null && timeparts[0].Length == 1 ? timeparts[0].Insert(0, "0") : timeparts[0];
                    model.StartTime = timeparts[0] + ":" + timeparts[1];
                    DateTime timeOnly = DateTime.ParseExact(model.StartTime.ToLower(), "hh:mm tt",
                                                            System.Globalization.CultureInfo.CurrentCulture);
                    StartTime = timeOnly.TimeOfDay;
                }

                if (!string.IsNullOrEmpty(model.EndTime))
                {
                    string[] timeparts = model.EndTime.Split(':');
                    timeparts[0] = timeparts[0] != null && timeparts[0].Length == 1 ? timeparts[0].Insert(0, "0") : timeparts[0];
                    model.EndTime = timeparts[0] + ":" + timeparts[1];
                    DateTime timeOnly = DateTime.ParseExact(model.EndTime.ToLower(), "hh:mm tt",
                                                            System.Globalization.CultureInfo.CurrentCulture);
                    EndTime = timeOnly.TimeOfDay;
                }

                int data = (int)GetScalar(StoredProcedure.ChangeSaveNewSchedule, new List<SearchValueData>
                {
                    new SearchValueData {Name = "ScheduleID",Value = Convert.ToString(model.ScheduleID)},
                    new SearchValueData {Name = "StartTime",Value = Convert.ToString(StartTime)},
                    new SearchValueData {Name = "EndTime",Value = Convert.ToString(EndTime)},
                    new SearchValueData {Name = "EmployeeID",Value = Convert.ToString(model.EmployeeID)},

                    new SearchValueData {Name = "UpdatedBy",Value = Convert.ToString(loggedInId)}
                });

                if (data != 1)
                {
                    response.IsSuccess = false;
                    response.Message = Resource.SchdueleNotUpdated;
                }
                else
                {
                    response.IsSuccess = true;
                    response.Data = data;
                    response.Message = Resource.ScheduledUpdatedSuccessfully;
                }

            }

            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }
        public ServiceResponse HC_PrivateDuty_OnRemoveScheduleAction(RemoveScheduleModel model, long loggedInId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ScheduleID", Value = Convert.ToString(model.ScheduleID) });
            searchList.Add(new SearchValueData { Name = "RemoveScheduleNotes", Value = Convert.ToString(model.RemoveScheduleReason) });


            if (model.IsSaveNoteOnly)
            {
                searchList.Add(new SearchValueData { Name = "isSaveNoteOnly", Value = "true", DataType = "bit" });
            }

            searchList.Add(new SearchValueData { Name = "loggedInId", Value = Convert.ToString(loggedInId) });
            searchList.Add(new SearchValueData { Name = "SystemID", Value = Common.GetHostAddress() });
            searchList.Add(new SearchValueData { Name = "ReferralTSDateID", Value = Convert.ToString(model.ReferralTSDateID) });




            //searchList.Add(new SearchValueData { Name = "StartTime", Value = Convert.ToString(model.StartTime) });
            //searchList.Add(new SearchValueData { Name = "EndTime", Value = Convert.ToString(model.EndTime) });

            int data = (int)GetScalar(StoredProcedure.HC_PrivateDuty_OnRemoveScheduleAction, searchList);
            if (data == 1)
            {
                response.Message = model.IsSaveNoteOnly ? Resource.NoteSavedSuccessfully : Resource.ScheduleUnassignedSuccessfully;
                response.IsSuccess = true;
            }
            return response;
        }



        public ServiceResponse HC_DayCare_OnRemoveScheduleAction(RemoveScheduleModel model, long loggedInId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ScheduleID", Value = Convert.ToString(model.ScheduleID) });
            searchList.Add(new SearchValueData { Name = "RemoveScheduleNotes", Value = Convert.ToString(model.RemoveScheduleReason) });


            if (model.IsSaveNoteOnly)
            {
                searchList.Add(new SearchValueData { Name = "isSaveNoteOnly", Value = "true", DataType = "bit" });
            }

            searchList.Add(new SearchValueData { Name = "loggedInId", Value = Convert.ToString(loggedInId) });
            searchList.Add(new SearchValueData { Name = "SystemID", Value = Common.GetHostAddress() });
            searchList.Add(new SearchValueData { Name = "ReferralTSDateID", Value = Convert.ToString(model.ReferralTSDateID) });




            //searchList.Add(new SearchValueData { Name = "StartTime", Value = Convert.ToString(model.StartTime) });
            //searchList.Add(new SearchValueData { Name = "EndTime", Value = Convert.ToString(model.EndTime) });

            int data = (int)GetScalar(StoredProcedure.HC_DayCare_OnRemoveScheduleAction, searchList);
            if (data == 1)
            {
                response.Message = model.IsSaveNoteOnly ? Resource.NoteSavedSuccessfully : Resource.ScheduleUnassignedSuccessfully;
                response.IsSuccess = true;
            }
            return response;
        }


        public ServiceResponse HC_CaseManagement_OnRemoveScheduleAction(RemoveScheduleModel model, long loggedInId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ScheduleID", Value = Convert.ToString(model.ScheduleID) });
            searchList.Add(new SearchValueData { Name = "RemoveScheduleNotes", Value = Convert.ToString(model.RemoveScheduleReason) });


            if (model.IsSaveNoteOnly)
            {
                searchList.Add(new SearchValueData { Name = "isSaveNoteOnly", Value = "true", DataType = "bit" });
            }

            searchList.Add(new SearchValueData { Name = "loggedInId", Value = Convert.ToString(loggedInId) });
            searchList.Add(new SearchValueData { Name = "SystemID", Value = Common.GetHostAddress() });
            searchList.Add(new SearchValueData { Name = "ReferralTSDateID", Value = Convert.ToString(model.ReferralTSDateID) });




            //searchList.Add(new SearchValueData { Name = "StartTime", Value = Convert.ToString(model.StartTime) });
            //searchList.Add(new SearchValueData { Name = "EndTime", Value = Convert.ToString(model.EndTime) });

            int data = (int)GetScalar(StoredProcedure.HC_DayCare_OnRemoveScheduleAction, searchList);
            if (data == 1)
            {
                response.Message = model.IsSaveNoteOnly ? Resource.NoteSavedSuccessfully : Resource.ScheduleUnassignedSuccessfully;
                response.IsSuccess = true;
            }
            return response;
        }
        #endregion




        #endregion


        #region Pending Schedules

        public ServiceResponse HC_PendingSchedules()
        {
            ServiceResponse response = new ServiceResponse();
            PendingSchedulesPageModel model = GetMultipleEntity<PendingSchedulesPageModel>(StoredProcedure.HC_SetPendingSchedulesPage, new List<SearchValueData>());
            if (model.SearchPendingScheduleListPage == null)
                model.SearchPendingScheduleListPage = new SearchPendingSchedules();
            response.IsSuccess = true;
            response.Data = model;
            return response;
        }


        public ServiceResponse HC_GetPendingScheduleList(SearchPendingSchedules model, int pageIndex, int pageSize, string sortIndex, string sortDirection,
                                                     long loggedInId)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            if (model != null)
            {
                SetSearchFilterForPendingScheduleList(model, searchList, loggedInId);

            }


            Page<PendingScheduleListModel> page = GetEntityPageList<PendingScheduleListModel>(StoredProcedure.HC_GetPendingScheduleList, searchList, pageSize, pageIndex, sortIndex, sortDirection);
            response.Data = page;
            response.IsSuccess = true;
            return response;
        }


        private static void SetSearchFilterForPendingScheduleList(SearchPendingSchedules model, List<SearchValueData> searchList, long loggedInId)
        {
            if (model.StartDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = model.StartDate.Value.ToString(Constants.DbDateFormat) });

            if (model.EndDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = model.EndDate.Value.ToString(Constants.DbDateFormat) });

            searchList.Add(new SearchValueData { Name = "PatientName", Value = Convert.ToString(model.PatientName) });
            searchList.Add(new SearchValueData { Name = "EmployeeID", Value = Convert.ToString(model.EmployeeID) });
            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(model.IsDeleted) });


        }


        public ServiceResponse HC_DeletePendingSchedule(SearchPendingSchedules model, int pageIndex, int pageSize, string sortIndex, string sortDirection,
                                                   long loggedInId)
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);
            SetSearchFilterForPendingScheduleList(model, searchList, loggedInId);
            if (!string.IsNullOrEmpty(model.ListOfIdsInCsv))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = model.ListOfIdsInCsv });
            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(1) });
            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInId) });


            List<PendingScheduleListModel> totalData = GetEntityList<PendingScheduleListModel>(StoredProcedure.HC_DeletePendingScheduleList, searchList);



            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<PendingScheduleListModel> list = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = list;
            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.PatientSchedule);
            return response;
        }


        public ServiceResponse HC_ProcessPendingSchedule(PendingScheduleListModel model, long loggedInId)
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "PendingScheduleID", Value = Convert.ToString(model.PendingScheduleID) });
            searchList.Add(new SearchValueData { Name = "RoleID", Value = Convert.ToString(SessionHelper.RoleID) });
            searchList.Add(new SearchValueData { Name = "ApprovalRequiredIVRBypassPermission", Value = Constants.Mobile_ApprovalRequired_IVR_Bypass_ClockInOut });
            searchList.Add(new SearchValueData { Name = "BypassAction", Value = Convert.ToString((int)EmployeeVisit.BypassActions.Pending) });
            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInId) });
            searchList.Add(new SearchValueData { Name = "SystemID", Value = Common.GetMAcAddress() });
            int data = (int)GetScalar(StoredProcedure.HC_ProcessPendingSchedule, searchList);

            if (data == 1)
            {
                response.IsSuccess = true;
                response.Message = Resource.ScheduleCreatedSuccessfully;
            }
            if (data == -1)
            {
                response.IsSuccess = false;
                response.Message = Resource.NoPendingSchedule;
            }
            if (data == -2)
            {
                response.IsSuccess = false;
                response.Message = Resource.AnEmpProcessed;
            }

            if (data == -5)
            {
                response.IsSuccess = false;
                response.Message = Resource.ThisEmpProcessed;
            }


            if (data == -3)
            {
                response.IsSuccess = false;
                response.Message = Resource.SetPrimaryPayorFirst;
            }

            if (data == -4)
            {
                response.IsSuccess = false;
                response.Message = Resource.PendingSchAlreadyProcessed;
            }

            return response;
        }

        #endregion

        #region Listen to events

        public ServiceResponse ProcessEvent(EventData model)
        {
            string LINE = string.Concat(Enumerable.Repeat("=", 80));
            StringBuilder content = new StringBuilder(string.Empty);
            content.AppendFormat("Process Event - Start, Date Time: {0}{1}", DateTime.Now, Environment.NewLine);
            content.AppendFormat("Data: {0}{1}", Common.SerializeObject(model), Environment.NewLine);
            var response = new ServiceResponse();
            try
            {
                bool isOrgHasAggregator = Common.IsOrgHasAggregator();
                if (isOrgHasAggregator)
                {
                    //Call to generate data CSV for HHAExchange
                    GenerateHHAXAggregatorDataFile(model, content);

                    //Call to generate data CSV for CareBridge
                    GenerateCareBridgeAggregatorDataFile(model, content);

                    //Call to generate data CSV for Tellus
                    GenerateTellusAggregatorDataFile(model, content);

                    //Call to generate data CSV for Sandata
                    GenerateSandataAggregatorDataFile(model, content);
                }
                else
                {
                    content.AppendFormat("Invalid Region & State.{0}", Environment.NewLine);
                }
                response.IsSuccess = true;
                response.Message = Resource.Processed;
                content.AppendFormat("Event processed successfully.{0}", Environment.NewLine);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
                content.AppendFormat("Some error occurred while processing event.{1}Message: {0}{1}", ex.Message, Environment.NewLine);
            }
            finally
            {
                content.AppendFormat("Process Event - End, Date Time: {0}{1}", DateTime.Now, Environment.NewLine);
                content.AppendFormat("{0}{1}", LINE, Environment.NewLine);
                string logPath = HttpContext.Current.Server.MapCustomPath(ConfigSettings.ProcessEventLogPath);
                Directory.CreateDirectory(logPath);
                string logFilename = string.Format("{0}{1}_{2}{3}", ConfigSettings.ProcessEventLogFileNamePrefix, Organization.OrganizationID, DateTime.Now.ToString("yyyyMMdd"), Constants.Extention_txt);
                Common.SyncAppendAllText(string.Format("{0}\\{1}", logPath, logFilename), content.ToString());
            }
            return response;
        }

        private void GenerateHHAXAggregatorDataFile(EventData model, StringBuilder content)
        {
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "EventName", Value = model.EventName });
                searchList.Add(new SearchValueData { Name = "ScheduleID", Value = Convert.ToString(model.ScheduleID) });
                searchList.Add(new SearchValueData { Name = "ReasonCode", Value = model.ReasonCode });
                searchList.Add(new SearchValueData { Name = "ActionCode", Value = model.ActionCode });
                searchList.Add(new SearchValueData { Name = "OrganizationID", Value = Convert.ToString(Organization.OrganizationID) });
                searchList.Add(new SearchValueData { Name = "HHAXAggregator", Value = Common.PayorClaimProcessors.HHAExchange.Value });

                AggregatorVisitData<HHAXData> hhaxData = GetMultipleEntity<AggregatorVisitData<HHAXData>>(StoredProcedure.HHAXAggregatorVisitData, searchList);
                if (hhaxData.ResultID == 1 && hhaxData.Data.Count > 0)
                {
                    bool isValid = hhaxData.Data?.All(d => d.OrganizationID == Organization.OrganizationID && d.ScheduleID == model.ScheduleID) ?? false;
                    if (isValid)
                    {
                        string hhaxPath = HttpContext.Current.Server.MapCustomPath(ConfigSettings.HHAXPath);
                        Directory.CreateDirectory(hhaxPath);
                        var hhaxFilename = string.Format("{0}{1}_{2}{3}", ConfigSettings.HHAXFileNamePrefix, Organization.OrganizationID, model.ScheduleID, Constants.Extention_csv);
                        var absolutePath = string.Format("{0}\\{1}", hhaxPath, hhaxFilename);
                        CreateExcelFile.CreateCsvFromList(hhaxData.Data, absolutePath, quoteString: true);
                        content.AppendFormat("File Created, Path: {0}{1}", absolutePath, Environment.NewLine);
                    }
                    else
                    { throw new Exception("Invalid Organization or Schedule."); }
                }
                else
                {
                    content.AppendFormat("No Data available for HHAXAggregator. Detail: {0}{1}", Common.SerializeObject(hhaxData.Detail), Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                content.AppendFormat("Error in GenerateHHAXAggregatorDataFile(), Message: {0}{1}", ex.Message, Environment.NewLine);
                throw ex;
            }
        }

        private void GenerateCareBridgeAggregatorDataFile(EventData model, StringBuilder content)
        {
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "EventName", Value = model.EventName });
                searchList.Add(new SearchValueData { Name = "ScheduleID", Value = Convert.ToString(model.ScheduleID) });
                searchList.Add(new SearchValueData { Name = "ReasonCode", Value = model.ReasonCode });
                searchList.Add(new SearchValueData { Name = "ActionCode", Value = model.ActionCode });
                searchList.Add(new SearchValueData { Name = "OrganizationID", Value = Convert.ToString(Organization.OrganizationID) });
                searchList.Add(new SearchValueData { Name = "CareBridgeAggregator", Value = Common.PayorClaimProcessors.Carebridge.Value });

                AggregatorVisitData<CareBridgeData> careBridgeData = GetMultipleEntity<AggregatorVisitData<CareBridgeData>>(StoredProcedure.CareBridgeAggregatorVisitData, searchList);
                if (careBridgeData.ResultID == 1 && careBridgeData.Data.Count > 0)
                {
                    bool isValid = careBridgeData.Data?.All(d => d.OrganizationID == Organization.OrganizationID && d.ScheduleID == model.ScheduleID) ?? false;
                    if (isValid)
                    {
                        string careBridgePath = HttpContext.Current.Server.MapCustomPath(ConfigSettings.CareBridgePath);
                        Directory.CreateDirectory(careBridgePath);
                        var careBridgeFilename = string.Format("{0}{1}_{2}{3}", ConfigSettings.CareBridgeFileNamePrefix, Organization.OrganizationID, model.ScheduleID, Constants.Extention_csv);
                        var absolutePath = string.Format("{0}\\{1}", careBridgePath, careBridgeFilename);
                        CreateExcelFile.CreateCsvFromList(careBridgeData.Data, absolutePath, quoteString: true);
                        content.AppendFormat("File Created, Path: {0}{1}", absolutePath, Environment.NewLine);
                    }
                    else
                    { throw new Exception("Invalid Organization or Schedule."); }
                }
                else
                {
                    content.AppendFormat("No Data available for CareBridgeAggregator. Detail: {0}{1}", Common.SerializeObject(careBridgeData.Detail), Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                content.AppendFormat("Error in GenerateCareBridgeAggregatorDataFile(), Message: {0}{1}", ex.Message, Environment.NewLine);
                throw ex;
            }
        }

        private void GenerateTellusAggregatorDataFile(EventData model, StringBuilder content)
        {
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "EventName", Value = model.EventName });
                searchList.Add(new SearchValueData { Name = "ScheduleID", Value = Convert.ToString(model.ScheduleID) });
                searchList.Add(new SearchValueData { Name = "ReasonCode", Value = model.ReasonCode });
                searchList.Add(new SearchValueData { Name = "ActionCode", Value = model.ActionCode });
                searchList.Add(new SearchValueData { Name = "OrganizationID", Value = Convert.ToString(Organization.OrganizationID) });
                searchList.Add(new SearchValueData { Name = "TellusAggregator", Value = Common.PayorClaimProcessors.Tellus.Value });

                AggregatorVisitData<TellusData> tellusData = GetMultipleEntity<AggregatorVisitData<TellusData>>(StoredProcedure.TellusAggregatorVisitData, searchList);
                if (tellusData.ResultID == 1 && tellusData.Data.Count > 0)
                {
                    bool isValid = tellusData.Data?.All(d => d.OrganizationID == Organization.OrganizationID && d.ScheduleID == model.ScheduleID) ?? false;
                    if (isValid)
                    {
                        string tellusPath = HttpContext.Current.Server.MapCustomPath(ConfigSettings.TellusPath);
                        Directory.CreateDirectory(tellusPath);
                        var tellusFilename = string.Format("{0}{1}_{2}{3}", ConfigSettings.TellusFileNamePrefix, Organization.OrganizationID, model.ScheduleID, Constants.Extention_csv);
                        var absolutePath = string.Format("{0}\\{1}", tellusPath, tellusFilename);
                        CreateExcelFile.CreateCsvFromList(tellusData.Data, absolutePath, quoteString: true);
                        content.AppendFormat("File Created, Path: {0}{1}", absolutePath, Environment.NewLine);
                    }
                    else
                    { throw new Exception("Invalid Organization or Schedule."); }
                }
                else
                {
                    content.AppendFormat("No Data available for TellusAggregator. Detail: {0}{1}", Common.SerializeObject(tellusData.Detail), Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                content.AppendFormat("Error in GenerateTellusAggregatorDataFile(), Message: {0}{1}", ex.Message, Environment.NewLine);
                throw ex;
            }
        }

        private void GenerateSandataAggregatorDataFile(EventData model, StringBuilder content)
        {
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "EventName", Value = model.EventName });
                searchList.Add(new SearchValueData { Name = "ScheduleID", Value = Convert.ToString(model.ScheduleID) });
                searchList.Add(new SearchValueData { Name = "ReasonCode", Value = model.ReasonCode });
                searchList.Add(new SearchValueData { Name = "ActionCode", Value = model.ActionCode });
                searchList.Add(new SearchValueData { Name = "OrganizationID", Value = Convert.ToString(Organization.OrganizationID) });
                searchList.Add(new SearchValueData { Name = "SandataAggregator", Value = Common.PayorClaimProcessors.Sandata.Value });

                AggregatorVisitData<SandataData> sandataData = GetMultipleEntity<AggregatorVisitData<SandataData>>(StoredProcedure.SandataAggregatorVisitData, searchList);
                if (sandataData.ResultID == 1 && sandataData.Data.Count > 0)
                {
                    bool isValid = sandataData.Data?.All(d => d.OrganizationID == Organization.OrganizationID && d.ScheduleID == model.ScheduleID) ?? false;
                    if (isValid)
                    {
                        string sandataPath = HttpContext.Current.Server.MapCustomPath(ConfigSettings.SandataPath);
                        Directory.CreateDirectory(sandataPath);
                        var sandataFilename = string.Format("{0}{1}_{2}{3}", ConfigSettings.SandataFileNamePrefix, Organization.OrganizationID, model.ScheduleID, Constants.Extention_csv);
                        var absolutePath = string.Format("{0}\\{1}", sandataPath, sandataFilename);
                        CreateExcelFile.CreateCsvFromList(sandataData.Data, absolutePath, quoteString: true);
                        content.AppendFormat("File Created, Path: {0}{1}", absolutePath, Environment.NewLine);
                    }
                    else
                    { throw new Exception("Invalid Organization or Schedule."); }
                }
                else
                {
                    content.AppendFormat("No Data available for SandataAggregator. Detail: {0}{1}", Common.SerializeObject(sandataData.Detail), Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                content.AppendFormat("Error in GenerateSandataAggregatorDataFile(), Message: {0}{1}", ex.Message, Environment.NewLine);
                throw ex;
            }
        }

        #endregion

        #region Visit Reason
        public ServiceResponse GetVisitReasonList(GetVisitReasonModel model)
        {
            ServiceResponse response = new ServiceResponse();

            var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "Type", Value = model.Type},
                    new SearchValueData {Name = "CompanyName", Value = model.CompanyName},
                };
            List<VisitReason> list = GetEntityList<VisitReason>(StoredProcedure.GetVisitReasonList, searchlist);
            response.IsSuccess = true;
            response.Data = list;

            return response;
        }

        #endregion

        #region "Nurse Scheduler"

        public ServiceResponse GetCareTypes()
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

        public ServiceResponse GetReferralsByCareTypeId(string careTypeId)
        {
            var response = new ServiceResponse();
            List<ReferralNurseSchModel> detailModel = GetEntityList<ReferralNurseSchModel>(StoredProcedure.GetReferralsByCareTypeId,
                new List<SearchValueData>{
                        new SearchValueData{Name = "CareTypeId",Value = careTypeId}
                    });

            response.Data = detailModel;
            response.IsSuccess = true;
            return response;
        }

        public long AddScheduleToReferralTimeSlotMaster(ReferralTimeSlotMaster schedule, long loggedInUserID)
        {
            List<SearchValueData> param = new List<SearchValueData>
                    {
                        new SearchValueData {Name = "ReferralID", Value = schedule.ReferralID.ToString()},
                        new SearchValueData {Name = "CreatedBy", Value = loggedInUserID.ToString() },
                        new SearchValueData {Name = "UpdatedBy", Value = loggedInUserID.ToString() },
                        new SearchValueData {Name = "CreatedDate", Value = DateTime.Now.ToString() },
                        new SearchValueData {Name = "UpdatedDate", Value = DateTime.Now.ToString() },
                        new SearchValueData {Name = "StartDate", Value = schedule.StartDate.ToString()},
                        new SearchValueData {Name = "EndDate", Value = schedule.EndDate.ToString()},
                        new SearchValueData {Name = "ReferralBillingAuthorizationID", Value = schedule.ReferralBillingAuthorizationID.ToString()},
                        new SearchValueData {Name = "SystemID", Value = schedule.SystemID},
                        new SearchValueData {Name = "IsAnyDay", Value = schedule.IsAnyDay.ToString()},
                        new SearchValueData {Name = "IsEndDateAvailable", Value = schedule.IsEndDateAvailable.ToString()}
                    };
            long referralTimeSlotMasterID = (long)GetScalar(StoredProcedure.AddScheduleToReferralTimeSlotMaster, param);

            return referralTimeSlotMasterID;
        }

        public long AddScheduleToReferralTimeSlotDetails(ReferralTimeSlotDetail schedule, long loggedInUserID)
        {
            List<SearchValueData> param = new List<SearchValueData>
                    {
                        new SearchValueData {Name = "ReferralTimeSlotMasterID", Value = schedule.ReferralTimeSlotMasterID.ToString()},
                        new SearchValueData {Name = "CreatedBy", Value = loggedInUserID.ToString() },
                        new SearchValueData {Name = "UpdatedBy", Value = loggedInUserID.ToString() },
                        new SearchValueData {Name = "CreatedDate", Value = DateTime.Now.ToString() },
                        new SearchValueData {Name = "UpdatedDate", Value = DateTime.Now.ToString() },
                        new SearchValueData {Name = "Day", Value = schedule.Day.ToString()},
                        new SearchValueData {Name = "StartTime", Value = schedule.StartTime.ToString()},
                        new SearchValueData {Name = "EndTime", Value = schedule.EndTime.ToString()},
                        new SearchValueData {Name = "SystemID", Value = schedule.SystemID},
                        new SearchValueData {Name = "Notes", Value = String.IsNullOrEmpty(schedule.Notes) ? null :  schedule.Notes.ToString() },
                        new SearchValueData {Name = "UsedInScheduling", Value = schedule.UsedInScheduling.ToString()},
                        new SearchValueData {Name = "CareTypeId", Value = schedule.CareTypeId.ToString()},
                        new SearchValueData {Name = "AnyTimeClockIn", Value = schedule.AnyTimeClockIn.ToString()}
                    };
            long referralTimeSlotDetailID = (long)GetScalar(StoredProcedure.AddScheduleToReferralTimeSlotDetails, param);

            return referralTimeSlotDetailID;
        }

        public long AddScheduleToReferralTimeSlotDates(ReferralTimeSlotDates schedule, long loggedInUserID)
        {
            List<SearchValueData> param = new List<SearchValueData>
                    {
                        new SearchValueData {Name = "ReferralID", Value = schedule.ReferralID.ToString()},
                        new SearchValueData {Name = "ReferralTimeSlotMasterID", Value = schedule.ReferralTimeSlotMasterID.ToString() },
                        new SearchValueData {Name = "ReferralTSDate", Value = schedule.ReferralTSDate.ToString() },
                        new SearchValueData {Name = "ReferralTSStartTime", Value = schedule.ReferralTSStartTime.ToString() },
                        new SearchValueData {Name = "ReferralTSEndTime", Value = schedule.ReferralTSEndTime.ToString() },
                        new SearchValueData {Name = "UsedInScheduling", Value = schedule.UsedInScheduling.ToString() },
                        new SearchValueData {Name = "Notes", Value = String.IsNullOrEmpty(schedule.Notes) ? null :  schedule.Notes.ToString() },
                        new SearchValueData {Name = "DayNumber", Value = schedule.DayNumber.ToString() },
                        new SearchValueData {Name = "ReferralTimeSlotDetailID", Value = schedule.ReferralTimeSlotDetailID.ToString() },
                        new SearchValueData {Name = "OnHold", Value = schedule.OnHold.ToString() },
                        new SearchValueData {Name = "IsDenied", Value = schedule.IsDenied.ToString() }
                    };
            long referralTimeSlotDetailID = (long)GetScalar(StoredProcedure.AddScheduleToReferralTimeSlotDates, param);

            return referralTimeSlotDetailID;
        }

        public bool DeleteScheduleFromReferralTimeSlotTables(long referralId, DateTime startDate, DateTime endDate)
        {
            List<SearchValueData> param = new List<SearchValueData>
                    {
                        new SearchValueData {Name = "ReferralID", Value = referralId.ToString()},
                        new SearchValueData {Name = "StartDate", Value = startDate.Date.ToString() },
                        new SearchValueData {Name = "EndDate", Value = endDate.Date.ToString() }
                    };

            long refferalId = (long)GetScalar(StoredProcedure.DeleteScheduleFromReferralTimeSlotTables, param);
            return true;
        }

        public bool DeleteBulkNurseSchedule(long scheduleID)
        {
            List<SearchValueData> param = new List<SearchValueData>
                    {
                        new SearchValueData {Name = "ScheduleId", Value = scheduleID.ToString()}
                    };

            long nurseScheduleId = (long)GetScalar(StoredProcedure.DeleteBulkNurseSchedule, param);
            return true;
        }

        public ScheduleMaster GetScheduleMasterById(long scheduleId)
        {
            var response = new ServiceResponse();
            ScheduleMaster schedule = GetEntity<ScheduleMaster>(StoredProcedure.GetScheduleMasterById,
                new List<SearchValueData>{
                        new SearchValueData{Name = "ScheduleID",Value = scheduleId.ToString()}
                    });
            return schedule;
        }

        public ServiceResponse CreateBulkNurseSchedule(ScheduleDTO schedule, long loggedInUserID)
        {
            var response = new ServiceResponse();
            StringBuilder scheduleList = null;
            List<CalenderObject> calendarObjects = new List<CalenderObject>();

            if (schedule.ScheduleRecurrence == RecurrencePattern.OneTime)
            {
                schedule.EndDate = schedule.StartDate.Date + new TimeSpan(23, 59, 59);
                calendarObjects.Add(new CalenderObject
                {
                    ID = schedule.ScheduleID,
                    StartDate = schedule.StartDate,
                    EndDate = schedule.EndDate.Value,
                });
            }
            else
            {
                if (schedule.EndDate == null)
                    schedule.EndDate = schedule.StartDate.AddYears(1);
                schedule.EndDate = schedule.EndDate.Value.Date + new TimeSpan(23, 59, 59);

                var range = new DateRange()
                {
                    StartDateTime = new DateTime(schedule.StartDate.Year, schedule.StartDate.Month, schedule.StartDate.Day),
                    EndDateTime = new DateTime(schedule.EndDate.Value.Year, schedule.EndDate.Value.Month, schedule.EndDate.Value.Day, 23, 59, 59)
                };

                calendarObjects
                   .AddRange(schedule.NumberOfOccurrences > 0
                   ? GetSpecificNumberOfOccurrencesForDateRange(schedule, range)
                   : GetAllOccurrencesForDateRange(schedule, range));
            }

            if (calendarObjects.Count == 0)
            {
                response.Data = -2;
                response.IsSuccess = false;
            }
            else
            {
                schedule.StartDate = schedule.StartDate.Date + (schedule.AnyTimeClockIn ? new TimeSpan(0, 0, 0) : TimeSpan.Parse(schedule.ClockInStartTime));
                schedule.EndDate = schedule.EndDate.Value.Date + (schedule.AnyTimeClockIn ? new TimeSpan(23, 59, 59) : TimeSpan.Parse(schedule.ClockInEndTime));

                scheduleList = new StringBuilder();
                foreach (var obj in calendarObjects)
                {
                    scheduleList.Append(obj.StartDate.ToString(Constants.DbDateFormat) + ",");
                }

                List<SearchValueData> paramFindConflict = new List<SearchValueData>
                    {
                        new SearchValueData {Name = "ScheduleDayList", Value = scheduleList.ToString()},
                        new SearchValueData {Name = "EmployeeId", Value = schedule.EmployeeId.ToString()},
                        new SearchValueData {Name = "StartTime", Value = schedule.AnyTimeClockIn ? "00:00:00" :  schedule.ClockInStartTime.ToString() },
                        new SearchValueData {Name = "EndTime", Value =  schedule.AnyTimeClockIn ? "23:59:59" :  schedule.ClockInEndTime.ToString()}
                    };

                int returnVal = (int)GetScalar(StoredProcedure.FindNurseScheduleConflict, paramFindConflict);

                if (returnVal == -1) //schedule conflict exists
                {
                    response.Data = returnVal;
                    response.IsSuccess = false;
                }
                else
                {
                    List<SearchValueData> param = new List<SearchValueData>
                    {
                        new SearchValueData {Name = "EmployeeId", Value = schedule.EmployeeId.ToString()},
                        new SearchValueData {Name = "ReferralId", Value = schedule.ReferralId.ToString()},
                        new SearchValueData {Name = "LoggedInUserId", Value = loggedInUserID.ToString() },
                        new SearchValueData {Name = "StartDate", Value = Convert.ToDateTime(schedule.StartDate).ToString(Constants.DbDateFormat) },
                        new SearchValueData {Name = "EndDate", Value = Convert.ToDateTime(schedule.EndDate).ToString(Constants.DbDateFormat) },
                        new SearchValueData {Name = "FrequencyChoice", Value = schedule.FrequencyChoice.ToString()},
                        new SearchValueData {Name = "Frequency", Value = schedule.Frequency.ToString()},
                        new SearchValueData {Name = "DaysOfWeek", Value = schedule.DaysOfWeek.ToString()},
                        new SearchValueData {Name = "DayOfMonth", Value = schedule.DayOfMonth.ToString()},
                        new SearchValueData {Name = "IsMonthlyDaySelection", Value = schedule.IsMonthlyDaySelection.ToString()},
                        new SearchValueData {Name = "DailyInterval", Value = schedule.DailyInterval.ToString()},
                        new SearchValueData {Name = "WeeklyInterval", Value = schedule.WeeklyInterval.ToString()},
                        new SearchValueData {Name = "MonthlyInterval", Value = schedule.MonthlyInterval.ToString()},
                        new SearchValueData {Name = "IsSundaySelected", Value = schedule.IsSundaySelected.ToString()},
                        new SearchValueData {Name = "IsMondaySelected", Value = schedule.IsMondaySelected.ToString()},
                        new SearchValueData {Name = "IsTuesdaySelected", Value = schedule.IsTuesdaySelected.ToString()},
                        new SearchValueData {Name = "IsWednesdaySelected", Value = schedule.IsWednesdaySelected.ToString()},
                        new SearchValueData {Name = "IsThursdaySelected", Value = schedule.IsThursdaySelected.ToString()},
                        new SearchValueData {Name = "IsFridaySelected", Value = schedule.IsFridaySelected.ToString()},
                        new SearchValueData {Name = "IsSaturdaySelected", Value = schedule.IsSaturdaySelected.ToString()},
                        new SearchValueData {Name = "IsFirstWeekOfMonthSelected", Value = schedule.IsFirstWeekOfMonthSelected.ToString()},
                        new SearchValueData {Name = "IsSecondWeekOfMonthSelected", Value = schedule.IsSecondWeekOfMonthSelected.ToString()},
                        new SearchValueData {Name = "IsThirdWeekOfMonthSelected", Value = schedule.IsThirdWeekOfMonthSelected.ToString()},
                        new SearchValueData {Name = "IsFourthWeekOfMonthSelected", Value = schedule.IsFourthWeekOfMonthSelected.ToString()},
                        new SearchValueData {Name = "IsLastWeekOfMonthSelected", Value = schedule.IsLastWeekOfMonthSelected.ToString()},
                        new SearchValueData {Name = "FrequencyTypeOptions", Value = schedule.FrequencyTypeOptions.ToString()},
                        new SearchValueData {Name = "MonthlyIntervalOptions", Value = schedule.MonthlyIntervalOptions.ToString()},
                        new SearchValueData {Name = "ScheduleRecurrence", Value = schedule.ScheduleRecurrence.ToString()},
                        new SearchValueData {Name = "CareTypeId", Value = schedule.CareTypeId.ToString()},
                        new SearchValueData {Name = "DaysOfWeekOptions", Value = schedule.DaysOfWeekOptions.ToString()},
                        new SearchValueData {Name = "PayorID", Value = schedule.PayorId.ToString()},
                        new SearchValueData {Name = "ReferralBillingAuthorizationID", Value = schedule.ReferralBillingAuthorizationID.ToString()},
                        new SearchValueData {Name = "IsVirtualVisit", Value = schedule.IsVirtualVisit.ToString()},
                        new SearchValueData {Name = "Notes", Value = schedule.Notes != null ? schedule.Notes.ToString() : null},
                        new SearchValueData {Name = "AnyTimeClockIn", Value = schedule.AnyTimeClockIn.ToString()},
                        new SearchValueData {Name = "StartTime", Value = schedule.AnyTimeClockIn ? "00:00:00" :  schedule.ClockInStartTime.ToString() },
                        new SearchValueData {Name = "EndTime", Value =  schedule.AnyTimeClockIn ? "23:59:59" :  schedule.ClockInEndTime.ToString()},
                        new SearchValueData {Name = "AnniversaryDay", Value = schedule.AnniversaryDay.ToString()},
                        new SearchValueData {Name = "AnniversaryMonth", Value = schedule.AnniversaryMonth.ToString()},
                        new SearchValueData {Name = "SystemID", Value = System.Web.HttpContext.Current.Request.UserHostAddress},
                        new SearchValueData {Name = "IsAnyDay", Value = schedule.IsAnyDay.ToString()},
                        new SearchValueData {Name = "ScheduleDayList", Value = scheduleList.ToString()}
                    };
                    long scheduleId = (long)GetScalar(StoredProcedure.AddBulkNurseSchedule, param);
                    response.Data = scheduleId;
                    response.IsSuccess = true;
                }
            }

            return response;
        }

        public ServiceResponse UpdateBulkNurseSchedule(ScheduleDTO schedule, long loggedInUserID)
        {
            var response = new ServiceResponse();
            StringBuilder scheduleList = null;
            List<CalenderObject> calendarObjects = new List<CalenderObject>();

            if (schedule.ScheduleRecurrence == RecurrencePattern.OneTime)
            {
                schedule.EndDate = schedule.StartDate.Date + new TimeSpan(23, 59, 59);
                calendarObjects.Add(new CalenderObject
                {
                    ID = schedule.ScheduleID,
                    StartDate = schedule.StartDate,
                    EndDate = schedule.EndDate.Value,
                });
            }
            else
            {
                if (schedule.EndDate == null)
                    schedule.EndDate = schedule.StartDate.AddYears(1);
                schedule.EndDate = schedule.EndDate.Value.Date + new TimeSpan(23, 59, 59);

                var range = new DateRange()
                {
                    StartDateTime = new DateTime(schedule.StartDate.Year, schedule.StartDate.Month, schedule.StartDate.Day),
                    EndDateTime = new DateTime(schedule.EndDate.Value.Year, schedule.EndDate.Value.Month, schedule.EndDate.Value.Day, 23, 59, 59)
                };

                calendarObjects
                   .AddRange(schedule.NumberOfOccurrences > 0
                   ? GetSpecificNumberOfOccurrencesForDateRange(schedule, range)
                   : GetAllOccurrencesForDateRange(schedule, range));
            }

            if (calendarObjects.Count == 0)
            {
                response.Data = -2;
                response.IsSuccess = false;
            }
            else
            {

                schedule.StartDate = schedule.StartDate.Date + (schedule.AnyTimeClockIn ? new TimeSpan(0, 0, 0) : TimeSpan.Parse(schedule.ClockInStartTime));
                schedule.EndDate = schedule.EndDate.Value.Date + (schedule.AnyTimeClockIn ? new TimeSpan(23, 59, 59) : TimeSpan.Parse(schedule.ClockInEndTime));

                scheduleList = new StringBuilder();
                foreach (var obj in calendarObjects)
                {
                    scheduleList.Append(obj.StartDate.ToString(Constants.DbDateFormat) + ",");
                }

                List<SearchValueData> paramFindConflict = new List<SearchValueData>
                    {
                        new SearchValueData {Name = "ScheduleDayList", Value = scheduleList.ToString()},
                        new SearchValueData {Name = "EmployeeId", Value = schedule.EmployeeId.ToString()},
                        new SearchValueData {Name = "StartTime", Value = schedule.AnyTimeClockIn ? "00:00:00" :  schedule.ClockInStartTime.ToString() },
                        new SearchValueData {Name = "EndTime", Value =  schedule.AnyTimeClockIn ? "23:59:59" :  schedule.ClockInEndTime.ToString()}
                    };

                int returnVal = (int)GetScalar(StoredProcedure.FindNurseScheduleConflict, paramFindConflict);

                if (returnVal == -1) //schedule conflict exists
                {
                    response.Data = returnVal;
                    response.IsSuccess = false;
                }
                else
                {
                    List<SearchValueData> param = new List<SearchValueData>
                    {
                        new SearchValueData {Name = "EmployeeId", Value = schedule.EmployeeId.ToString()},
                        new SearchValueData {Name = "ReferralId", Value = schedule.ReferralId.ToString()},
                        new SearchValueData {Name = "LoggedInUserId", Value = loggedInUserID.ToString() },
                        new SearchValueData {Name = "ScheduleId", Value = schedule.ScheduleID.ToString() },
                        new SearchValueData {Name = "NurseScheduleId", Value = schedule.NurseScheduleID.ToString()},
                        new SearchValueData {Name = "StartDate", Value = Convert.ToDateTime(schedule.StartDate).ToString(Constants.DbDateFormat) },
                        new SearchValueData {Name = "EndDate", Value = Convert.ToDateTime(schedule.EndDate).ToString(Constants.DbDateFormat) },
                        new SearchValueData {Name = "FrequencyChoice", Value = schedule.FrequencyChoice.ToString()},
                        new SearchValueData {Name = "Frequency", Value = schedule.Frequency.ToString()},
                        new SearchValueData {Name = "DaysOfWeek", Value = schedule.DaysOfWeek.ToString()},
                        new SearchValueData {Name = "DayOfMonth", Value = schedule.DayOfMonth.ToString()},
                        new SearchValueData {Name = "IsMonthlyDaySelection", Value = schedule.IsMonthlyDaySelection.ToString()},
                        new SearchValueData {Name = "DailyInterval", Value = schedule.DailyInterval.ToString()},
                        new SearchValueData {Name = "WeeklyInterval", Value = schedule.WeeklyInterval.ToString()},
                        new SearchValueData {Name = "MonthlyInterval", Value = schedule.MonthlyInterval.ToString()},
                        new SearchValueData {Name = "IsSundaySelected", Value = schedule.IsSundaySelected.ToString()},
                        new SearchValueData {Name = "IsMondaySelected", Value = schedule.IsMondaySelected.ToString()},
                        new SearchValueData {Name = "IsTuesdaySelected", Value = schedule.IsTuesdaySelected.ToString()},
                        new SearchValueData {Name = "IsWednesdaySelected", Value = schedule.IsWednesdaySelected.ToString()},
                        new SearchValueData {Name = "IsThursdaySelected", Value = schedule.IsThursdaySelected.ToString()},
                        new SearchValueData {Name = "IsFridaySelected", Value = schedule.IsFridaySelected.ToString()},
                        new SearchValueData {Name = "IsSaturdaySelected", Value = schedule.IsSaturdaySelected.ToString()},
                        new SearchValueData {Name = "IsFirstWeekOfMonthSelected", Value = schedule.IsFirstWeekOfMonthSelected.ToString()},
                        new SearchValueData {Name = "IsSecondWeekOfMonthSelected", Value = schedule.IsSecondWeekOfMonthSelected.ToString()},
                        new SearchValueData {Name = "IsThirdWeekOfMonthSelected", Value = schedule.IsThirdWeekOfMonthSelected.ToString()},
                        new SearchValueData {Name = "IsFourthWeekOfMonthSelected", Value = schedule.IsFourthWeekOfMonthSelected.ToString()},
                        new SearchValueData {Name = "IsLastWeekOfMonthSelected", Value = schedule.IsLastWeekOfMonthSelected.ToString()},
                        new SearchValueData {Name = "FrequencyTypeOptions", Value = schedule.FrequencyTypeOptions.ToString()},
                        new SearchValueData {Name = "MonthlyIntervalOptions", Value = schedule.MonthlyIntervalOptions.ToString()},
                        new SearchValueData {Name = "ScheduleRecurrence", Value = schedule.ScheduleRecurrence.ToString()},
                        new SearchValueData {Name = "CareTypeId", Value = schedule.CareTypeId.ToString()},
                        new SearchValueData {Name = "DaysOfWeekOptions", Value = schedule.DaysOfWeekOptions.ToString()},
                        new SearchValueData {Name = "PayorID", Value = schedule.PayorId.ToString()},
                        new SearchValueData {Name = "ReferralBillingAuthorizationID", Value = schedule.ReferralBillingAuthorizationID.ToString()},
                        new SearchValueData {Name = "IsVirtualVisit", Value = schedule.IsVirtualVisit.ToString()},
                        new SearchValueData {Name = "Notes", Value = schedule.Notes != null ? schedule.Notes.ToString() : null},
                        new SearchValueData {Name = "AnyTimeClockIn", Value = schedule.AnyTimeClockIn.ToString()},
                        new SearchValueData {Name = "StartTime", Value = schedule.AnyTimeClockIn ? "00:00:00" :  schedule.ClockInStartTime.ToString() },
                        new SearchValueData {Name = "EndTime", Value =  schedule.AnyTimeClockIn ? "23:59:59" :  schedule.ClockInEndTime.ToString()},
                        new SearchValueData {Name = "AnniversaryDay", Value = schedule.AnniversaryDay.ToString()},
                        new SearchValueData {Name = "AnniversaryMonth", Value = schedule.AnniversaryMonth.ToString()},
                        new SearchValueData {Name = "SystemID", Value = System.Web.HttpContext.Current.Request.UserHostAddress},
                        new SearchValueData {Name = "IsAnyDay", Value = schedule.IsAnyDay.ToString()},
                        new SearchValueData {Name = "ScheduleDayList", Value = scheduleList.ToString()}
                    };
                    long scheduleId = (long)GetScalar(StoredProcedure.UpdateBulkNurseSchedule, param);
                    response.Data = scheduleId;
                    response.IsSuccess = true;
                }
            }

            return response;
        }

        //public ServiceResponse AddAppointment(ScheduleDTO schedule, long loggedInUserID)
        //{
        //    var response = new ServiceResponse();
        //    StringBuilder scheduleList = null;

        //    if (schedule.ScheduleRecurrence != RecurrencePattern.OneTime && schedule.EndDate == null)
        //        schedule.EndDate = schedule.StartDate.AddYears(1);

        //    List<CalenderObject> calendarObjects = new List<CalenderObject>();
        //    var range = new DateRange()
        //    {
        //        StartDateTime = new DateTime(schedule.StartDate.Year, schedule.StartDate.Month, schedule.StartDate.Day),
        //        EndDateTime = new DateTime(schedule.ScheduleRecurrence == RecurrencePattern.OneTime ? schedule.StartDate.Year : schedule.EndDate.Value.Year,
        //        schedule.ScheduleRecurrence == RecurrencePattern.OneTime ? schedule.StartDate.Month : schedule.EndDate.Value.Month,
        //        schedule.ScheduleRecurrence == RecurrencePattern.OneTime ? schedule.StartDate.Day : schedule.EndDate.Value.Day)
        //    };

        //    //range.StartDateTime = range.StartDateTime.Date + (schedule.AnyTimeClockIn ? new TimeSpan(0, 0, 0) : TimeSpan.Parse(schedule.ClockInStartTime));
        //    //range.EndDateTime = range.EndDateTime.Date + (schedule.AnyTimeClockIn ? new TimeSpan(23, 59, 59) : TimeSpan.Parse(schedule.ClockInEndTime));

        //    if (schedule.ScheduleRecurrence == RecurrencePattern.OneTime)
        //        schedule.EndDate = schedule.StartDate + new TimeSpan(23, 59, 59);
        //    else if (schedule.ScheduleRecurrence == RecurrencePattern.Repeat)
        //        schedule.EndDate = schedule.EndDate.Value.Date + new TimeSpan(23, 59, 59);    

        //    //if (schedule.EndDate == null)
        //    //    schedule.EndDateTime = null;

        //    calendarObjects
        //       .AddRange(schedule.NumberOfOccurrences > 0
        //       ? GetSpecificNumberOfOccurrencesForDateRange(schedule, range)
        //       : GetAllOccurrencesForDateRange(schedule, range));

        //    schedule.StartDate = schedule.StartDate.Date + (schedule.AnyTimeClockIn ? new TimeSpan(0, 0, 0) : TimeSpan.Parse(schedule.ClockInStartTime));
        //    schedule.EndDate = schedule.EndDate.Value.Date + (schedule.AnyTimeClockIn ? new TimeSpan(23, 59, 59) : TimeSpan.Parse(schedule.ClockInEndTime));

        //    //ReferralTimeSlotMaster referralTimeSlotMaster = new ReferralTimeSlotMaster();
        //    //referralTimeSlotMaster.ReferralID = schedule.ReferralId;
        //    //referralTimeSlotMaster.CreatedBy = schedule.CreatedBy;
        //    //referralTimeSlotMaster.UpdatedBy = schedule.UpdatedBy;
        //    //referralTimeSlotMaster.CreatedDate = schedule.CreatedDate;
        //    //referralTimeSlotMaster.UpdatedDate = schedule.UpdatedDate;
        //    //referralTimeSlotMaster.StartDate = schedule.StartDate.Date;
        //    //referralTimeSlotMaster.EndDate = schedule.EndDate;
        //    //referralTimeSlotMaster.ReferralBillingAuthorizationID = schedule.ReferralBillingAuthorizationID;
        //    //referralTimeSlotMaster.SystemID = null;
        //    //referralTimeSlotMaster.IsAnyDay = schedule.IsAnyDay;
        //    //referralTimeSlotMaster.IsEndDateAvailable = true;

        //    //long referralTimeSlotDetailID;
        //    //long referralTimeSlotMasterID = AddScheduleToReferralTimeSlotMaster(referralTimeSlotMaster, loggedInUserID);

        //    //ReferralTimeSlotDetail referralTimeSlotDetail;
        //    //ReferralTimeSlotDates referralTimeSlotDates;

        //    //foreach (var obj in calendarObjects)
        //    //{
        //    //    referralTimeSlotDetail = new ReferralTimeSlotDetail();
        //    //    referralTimeSlotDetail.ReferralTimeSlotMasterID = referralTimeSlotMasterID;
        //    //    referralTimeSlotDetail.CreatedBy = schedule.CreatedBy;
        //    //    referralTimeSlotDetail.UpdatedBy = schedule.UpdatedBy;
        //    //    referralTimeSlotDetail.CreatedDate = schedule.CreatedDate;
        //    //    referralTimeSlotDetail.UpdatedDate = schedule.UpdatedDate;
        //    //    referralTimeSlotDetail.Day = obj.StartDate.Day;
        //    //    referralTimeSlotDetail.StartTime = new TimeSpan(0, 0, 0);
        //    //    referralTimeSlotDetail.EndTime = new TimeSpan(23, 59, 59);
        //    //    referralTimeSlotDetail.SystemID = null;
        //    //    referralTimeSlotDetail.Notes = schedule.Notes;
        //    //    referralTimeSlotDetail.UsedInScheduling = true;
        //    //    referralTimeSlotDetail.CareTypeId = long.Parse(schedule.CareTypeId);
        //    //    referralTimeSlotDetail.AnyTimeClockIn = true;

        //    //    referralTimeSlotDetailID = AddScheduleToReferralTimeSlotDetails(referralTimeSlotDetail, loggedInUserID);

        //    //    referralTimeSlotDates = new ReferralTimeSlotDates();

        //    //    referralTimeSlotDates.ReferralID = schedule.ReferralId;
        //    //    referralTimeSlotDates.ReferralTimeSlotMasterID = referralTimeSlotMasterID;
        //    //    referralTimeSlotDates.ReferralTimeSlotDetailID = referralTimeSlotDetailID;
        //    //    referralTimeSlotDates.ReferralTSDate = obj.StartDate.Date;
        //    //    referralTimeSlotDates.ReferralTSStartTime = obj.StartDate.Date + new TimeSpan(0, 0, 0);
        //    //    referralTimeSlotDates.ReferralTSEndTime = obj.StartDate.Date + new TimeSpan(23, 59, 59);
        //    //    referralTimeSlotDates.UsedInScheduling = true;
        //    //    referralTimeSlotDates.Notes = schedule.Notes;
        //    //    referralTimeSlotDates.DayNumber = obj.StartDate.Day;
        //    //    referralTimeSlotDates.OnHold = false;
        //    //    referralTimeSlotDates.IsDenied = false;

        //    //    AddScheduleToReferralTimeSlotDates(referralTimeSlotDates, loggedInUserID);

        //    //}

        //    scheduleList = new StringBuilder();
        //    foreach (var obj in calendarObjects)
        //    {
        //        scheduleList.Append(obj.StartDate.ToString(Constants.DbDateFormat) + ",");
        //    }

        //        List<SearchValueData> param = new List<SearchValueData>
        //            {
        //                new SearchValueData {Name = "EmployeeId", Value = schedule.EmployeeId.ToString()},
        //                new SearchValueData {Name = "ReferralId", Value = schedule.ReferralId.ToString()},                        
        //                new SearchValueData {Name = "LoggedInUserId", Value = loggedInUserID.ToString() },
        //                new SearchValueData {Name = "StartDate", Value = schedule.StartDate.ToString()},
        //                new SearchValueData {Name = "EndDate", Value = schedule.EndDate.ToString()},
        //                new SearchValueData {Name = "FrequencyChoice", Value = schedule.FrequencyChoice.ToString()},
        //                new SearchValueData {Name = "Frequency", Value = schedule.Frequency.ToString()},
        //                new SearchValueData {Name = "DaysOfWeek", Value = schedule.DaysOfWeek.ToString()},
        //                new SearchValueData {Name = "WeeklyInterval", Value = schedule.WeeklyInterval.ToString()},
        //                new SearchValueData {Name = "MonthlyInterval", Value = schedule.MonthlyInterval.ToString()},
        //                new SearchValueData {Name = "IsSundaySelected", Value = schedule.IsSundaySelected.ToString()},
        //                new SearchValueData {Name = "IsMondaySelected", Value = schedule.IsMondaySelected.ToString()},
        //                new SearchValueData {Name = "IsTuesdaySelected", Value = schedule.IsTuesdaySelected.ToString()},
        //                new SearchValueData {Name = "IsWednesdaySelected", Value = schedule.IsWednesdaySelected.ToString()},
        //                new SearchValueData {Name = "IsThursdaySelected", Value = schedule.IsThursdaySelected.ToString()},
        //                new SearchValueData {Name = "IsFridaySelected", Value = schedule.IsFridaySelected.ToString()},
        //                new SearchValueData {Name = "IsSaturdaySelected", Value = schedule.IsSaturdaySelected.ToString()},
        //                new SearchValueData {Name = "IsFirstWeekOfMonthSelected", Value = schedule.IsFirstWeekOfMonthSelected.ToString()},
        //                new SearchValueData {Name = "IsSecondWeekOfMonthSelected", Value = schedule.IsSecondWeekOfMonthSelected.ToString()},
        //                new SearchValueData {Name = "IsThirdWeekOfMonthSelected", Value = schedule.IsThirdWeekOfMonthSelected.ToString()},
        //                new SearchValueData {Name = "IsFourthWeekOfMonthSelected", Value = schedule.IsFourthWeekOfMonthSelected.ToString()},
        //                new SearchValueData {Name = "IsLastWeekOfMonthSelected", Value = schedule.IsLastWeekOfMonthSelected.ToString()},
        //                new SearchValueData {Name = "FrequencyTypeOptions", Value = schedule.FrequencyTypeOptions.ToString()},
        //                new SearchValueData {Name = "MonthlyIntervalOptions", Value = schedule.MonthlyIntervalOptions.ToString()},
        //                new SearchValueData {Name = "ScheduleRecurrence", Value = schedule.ScheduleRecurrence.ToString()},
        //                new SearchValueData {Name = "CareTypeId", Value = schedule.CareTypeId.ToString()},
        //                new SearchValueData {Name = "DaysOfWeekOptions", Value = schedule.DaysOfWeekOptions.ToString()},
        //                new SearchValueData {Name = "PayorID", Value = schedule.PayorId.ToString()},
        //                new SearchValueData {Name = "ReferralBillingAuthorizationID", Value = schedule.ReferralBillingAuthorizationID.ToString()},
        //                new SearchValueData {Name = "IsVirtualVisit", Value = schedule.IsVirtualVisit.ToString()},
        //                new SearchValueData {Name = "Notes", Value = schedule.Notes != null ? schedule.Notes.ToString() : null},
        //                new SearchValueData {Name = "AnyTimeClockIn", Value = schedule.AnyTimeClockIn.ToString()},
        //                new SearchValueData {Name = "StartTime", Value = schedule.AnyTimeClockIn ? "00:00:00" :  schedule.ClockInStartTime.ToString() },
        //                new SearchValueData {Name = "EndTime", Value =  schedule.AnyTimeClockIn ? "23:59:59" :  schedule.ClockInEndTime.ToString()},
        //                new SearchValueData {Name = "AnniversaryDay", Value = schedule.AnniversaryDay.ToString()},
        //                new SearchValueData {Name = "AnniversaryMonth", Value = schedule.AnniversaryMonth.ToString()},
        //                new SearchValueData {Name = "SystemID", Value = System.Web.HttpContext.Current.Request.UserHostAddress},
        //                new SearchValueData {Name = "IsAnyDay", Value = schedule.IsAnyDay.ToString()},
        //                new SearchValueData {Name = "ScheduleDayList", Value = scheduleList.ToString()}
        //            };
        //    long scheduleId = (long)GetScalar(StoredProcedure.AddAppointmentToScheduleMaster, param);
        //    response.Data = scheduleId;
        //    response.IsSuccess = true;

        //    return response;
        //}

        private IEnumerable<CalenderObject> GetSpecificNumberOfOccurrencesForDateRange(ScheduleDTO scheduleDTO, DateRange range)
        {
            var calendarObjects = new List<CalenderObject>();
            var occurrences = scheduleDTO.Schedule.Occurrences(range).ToArray();
            for (var i = 0; i < scheduleDTO.NumberOfOccurrences; i++)
            {
                var date = occurrences.ElementAtOrDefault(i);
                calendarObjects.Add(new CalenderObject
                {
                    ID = scheduleDTO.ScheduleID,
                    StartDate = date + scheduleDTO.StartTime,
                    EndDate = date + scheduleDTO.EndTime,
                });
            }
            return calendarObjects;
        }

        private IEnumerable<CalenderObject> GetAllOccurrencesForDateRange(ScheduleDTO scheduleDTO, DateRange range)
        {
            var calendarObjects = new List<CalenderObject>();
            foreach (var date in scheduleDTO.Schedule.Occurrences(range))
            {
                calendarObjects.Add(new CalenderObject
                {
                    ID = scheduleDTO.ScheduleID,
                    StartDate = date + scheduleDTO.StartTime,
                    EndDate = date + scheduleDTO.EndTime,
                });
            }

            return calendarObjects;
        }



        //public ServiceResponse UpdateAppointment(ScheduleDTO schedule, long loggedInUserID)
        //{
        //    var response = new ServiceResponse();
        //    ScheduleMaster existingSchedule = GetScheduleMasterById(schedule.ID);

        //    DeleteScheduleFromReferralTimeSlotTables(existingSchedule.ReferralID, existingSchedule.StartDate.Date, existingSchedule.EndDate.Date);

        //    if (schedule.ScheduleRecurrence != RecurrencePattern.OneTime && schedule.EndDate == null)
        //        schedule.EndDate = schedule.StartDate.AddYears(1);

        //    List<CalenderObject> calendarObjects = new List<CalenderObject>();
        //    var range = new DateRange()
        //    {
        //        StartDateTime = new DateTime(schedule.StartDate.Year, schedule.StartDate.Month, schedule.StartDate.Day),
        //        EndDateTime = new DateTime(schedule.ScheduleRecurrence == RecurrencePattern.OneTime ? schedule.StartDate.Year : schedule.EndDate.Value.Year,
        //        schedule.ScheduleRecurrence == RecurrencePattern.OneTime ? schedule.StartDate.Month : schedule.EndDate.Value.Month,
        //        schedule.ScheduleRecurrence == RecurrencePattern.OneTime ? schedule.StartDate.Day : schedule.EndDate.Value.Day)
        //    };

        //    range.StartDateTime = range.StartDateTime.Date + (schedule.AnyTimeClockIn ? new TimeSpan(0, 0, 0) : TimeSpan.Parse(schedule.ClockInStartTime));
        //    range.EndDateTime = range.EndDateTime.Date + (schedule.AnyTimeClockIn ? new TimeSpan(0, 0, 0) : TimeSpan.Parse(schedule.ClockInEndTime));

        //    schedule.StartDate = schedule.StartDate.Date + (schedule.AnyTimeClockIn ? new TimeSpan(0, 0, 0) : TimeSpan.Parse(schedule.ClockInStartTime));

        //    if (schedule.ScheduleRecurrence == RecurrencePattern.OneTime)
        //        schedule.EndDate = schedule.StartDate.Date + (schedule.AnyTimeClockIn ? new TimeSpan(23, 59, 59) : TimeSpan.Parse(schedule.ClockInEndTime));
        //    else if (schedule.ScheduleRecurrence == RecurrencePattern.Repeat)
        //        schedule.EndDate = schedule.EndDate.Value.Date + (schedule.AnyTimeClockIn ? new TimeSpan(23, 59, 59) : TimeSpan.Parse(schedule.ClockInEndTime));

        //    calendarObjects
        //       .AddRange(schedule.NumberOfOccurrences > 0
        //       ? GetSpecificNumberOfOccurrencesForDateRange(schedule, range)
        //       : GetAllOccurrencesForDateRange(schedule, range));

        //    ReferralTimeSlotMaster referralTimeSlotMaster = new ReferralTimeSlotMaster();
        //    referralTimeSlotMaster.ReferralID = schedule.ReferralId;
        //    referralTimeSlotMaster.CreatedBy = schedule.CreatedBy;
        //    referralTimeSlotMaster.UpdatedBy = schedule.UpdatedBy;
        //    referralTimeSlotMaster.CreatedDate = schedule.CreatedDate;
        //    referralTimeSlotMaster.UpdatedDate = schedule.UpdatedDate;
        //    referralTimeSlotMaster.StartDate = schedule.StartDate;
        //    referralTimeSlotMaster.EndDate = schedule.EndDate;
        //    referralTimeSlotMaster.ReferralBillingAuthorizationID = schedule.ReferralBillingAuthorizationID;
        //    referralTimeSlotMaster.SystemID = null;
        //    referralTimeSlotMaster.IsAnyDay = schedule.IsAnyDay;
        //    referralTimeSlotMaster.IsEndDateAvailable = true;

        //    long referralTimeSlotDetailID;
        //    long referralTimeSlotMasterID = AddScheduleToReferralTimeSlotMaster(referralTimeSlotMaster, loggedInUserID);

        //    ReferralTimeSlotDetail referralTimeSlotDetail;
        //    ReferralTimeSlotDates referralTimeSlotDates;

        //    foreach (var obj in calendarObjects)
        //    {
        //        referralTimeSlotDetail = new ReferralTimeSlotDetail();
        //        referralTimeSlotDetail.ReferralTimeSlotMasterID = referralTimeSlotMasterID;
        //        referralTimeSlotDetail.CreatedBy = schedule.CreatedBy;
        //        referralTimeSlotDetail.UpdatedBy = schedule.UpdatedBy;
        //        referralTimeSlotDetail.CreatedDate = schedule.CreatedDate;
        //        referralTimeSlotDetail.UpdatedDate = schedule.UpdatedDate;
        //        referralTimeSlotDetail.Day = obj.StartDate.Day;
        //        referralTimeSlotDetail.StartTime = (schedule.AnyTimeClockIn ? new TimeSpan(0, 0, 0) : TimeSpan.Parse(schedule.ClockInStartTime));
        //        referralTimeSlotDetail.EndTime = (schedule.AnyTimeClockIn ? new TimeSpan(23, 59, 59) : TimeSpan.Parse(schedule.ClockInEndTime));
        //        referralTimeSlotDetail.SystemID = null;
        //        referralTimeSlotDetail.Notes = schedule.Notes;
        //        referralTimeSlotDetail.UsedInScheduling = true;
        //        referralTimeSlotDetail.CareTypeId = long.Parse(schedule.CareTypeId);
        //        referralTimeSlotDetail.AnyTimeClockIn = true;

        //        referralTimeSlotDetailID = AddScheduleToReferralTimeSlotDetails(referralTimeSlotDetail, loggedInUserID);

        //        referralTimeSlotDates = new ReferralTimeSlotDates();

        //        referralTimeSlotDates.ReferralID = schedule.ReferralId;
        //        referralTimeSlotDates.ReferralTimeSlotMasterID = referralTimeSlotMasterID;
        //        referralTimeSlotDates.ReferralTimeSlotDetailID = referralTimeSlotDetailID;
        //        referralTimeSlotDates.ReferralTSDate = obj.StartDate.Date;
        //        referralTimeSlotDates.ReferralTSStartTime = obj.StartDate.Date + new TimeSpan(0, 0, 0);
        //        referralTimeSlotDates.ReferralTSEndTime = obj.StartDate.Date + new TimeSpan(23, 59, 59);
        //        referralTimeSlotDates.UsedInScheduling = true;
        //        referralTimeSlotDates.Notes = schedule.Notes;
        //        referralTimeSlotDates.DayNumber = obj.StartDate.Day;
        //        referralTimeSlotDates.OnHold = false;
        //        referralTimeSlotDates.IsDenied = false;

        //        AddScheduleToReferralTimeSlotDates(referralTimeSlotDates, loggedInUserID);

        //    }
        //    //}

        //    List<SearchValueData> param = new List<SearchValueData>
        //            {
        //                new SearchValueData {Name = "EmployeeId", Value = schedule.EmployeeId.ToString()},
        //                new SearchValueData {Name = "ReferralId", Value = schedule.ReferralId.ToString()},
        //                new SearchValueData {Name = "ScheduleId", Value = schedule.ID.ToString()},
        //                new SearchValueData {Name = "UpdatedBy", Value = loggedInUserID.ToString() },
        //                new SearchValueData {Name = "UpdatedDate", Value = DateTime.Now.ToString() },
        //                new SearchValueData {Name = "StartDate", Value = schedule.StartDate.ToString(Constants.DbDateFormat) + (schedule.AnyTimeClockIn ? new TimeSpan(23, 59, 59) : TimeSpan.Parse(schedule.ClockInEndTime))},
        //                new SearchValueData {Name = "EndDate", Value = schedule.EndDate.ToString()},
        //                new SearchValueData {Name = "FrequencyChoice", Value = schedule.FrequencyChoice.ToString()},
        //                new SearchValueData {Name = "Frequency", Value = schedule.Frequency.ToString()},
        //                new SearchValueData {Name = "DaysOfWeek", Value = schedule.DaysOfWeek.ToString()},
        //                new SearchValueData {Name = "WeeklyInterval", Value = schedule.WeeklyInterval.ToString()},
        //                new SearchValueData {Name = "MonthlyInterval", Value = schedule.MonthlyInterval.ToString()},
        //                new SearchValueData {Name = "IsSundaySelected", Value = schedule.IsSundaySelected.ToString()},
        //                new SearchValueData {Name = "IsMondaySelected", Value = schedule.IsMondaySelected.ToString()},
        //                new SearchValueData {Name = "IsTuesdaySelected", Value = schedule.IsTuesdaySelected.ToString()},
        //                new SearchValueData {Name = "IsWednesdaySelected", Value = schedule.IsWednesdaySelected.ToString()},
        //                new SearchValueData {Name = "IsThursdaySelected", Value = schedule.IsThursdaySelected.ToString()},
        //                new SearchValueData {Name = "IsFridaySelected", Value = schedule.IsFridaySelected.ToString()},
        //                new SearchValueData {Name = "IsSaturdaySelected", Value = schedule.IsSaturdaySelected.ToString()},
        //                new SearchValueData {Name = "IsFirstWeekOfMonthSelected", Value = schedule.IsFirstWeekOfMonthSelected.ToString()},
        //                new SearchValueData {Name = "IsSecondWeekOfMonthSelected", Value = schedule.IsSecondWeekOfMonthSelected.ToString()},
        //                new SearchValueData {Name = "IsThirdWeekOfMonthSelected", Value = schedule.IsThirdWeekOfMonthSelected.ToString()},
        //                new SearchValueData {Name = "IsFourthWeekOfMonthSelected", Value = schedule.IsFourthWeekOfMonthSelected.ToString()},
        //                new SearchValueData {Name = "IsLastWeekOfMonthSelected", Value = schedule.IsLastWeekOfMonthSelected.ToString()},
        //                new SearchValueData {Name = "FrequencyTypeOptions", Value = schedule.FrequencyTypeOptions.ToString()},
        //                new SearchValueData {Name = "MonthlyIntervalOptions", Value = schedule.MonthlyIntervalOptions.ToString()},
        //                new SearchValueData {Name = "ScheduleRecurrence", Value = schedule.ScheduleRecurrence.ToString()},
        //                new SearchValueData {Name = "CareTypeId", Value = schedule.CareTypeId.ToString()},
        //                new SearchValueData {Name = "DaysOfWeekOptions", Value = schedule.DaysOfWeekOptions.ToString()},
        //                new SearchValueData {Name = "PayorID", Value = schedule.PayorId.ToString()},
        //                new SearchValueData {Name = "ReferralBillingAuthorizationID", Value = schedule.ReferralBillingAuthorizationID.ToString()},
        //                new SearchValueData {Name = "IsVirtualVisit", Value = schedule.IsVirtualVisit.ToString()},
        //                new SearchValueData {Name = "ReferralTSMasterID", Value = referralTimeSlotMasterID.ToString()},
        //                new SearchValueData {Name = "Notes", Value = schedule.Notes != null ? schedule.Notes.ToString() : null},
        //                new SearchValueData {Name = "AnyTimeClockIn", Value = schedule.AnyTimeClockIn.ToString()},
        //                new SearchValueData {Name = "StartTime", Value = schedule.AnyTimeClockIn ? "00:00:00" :  schedule.ClockInStartTime.ToString() },
        //                new SearchValueData {Name = "EndTime", Value =  schedule.AnyTimeClockIn ? "23:59:59" :  schedule.ClockInEndTime.ToString()},
        //                new SearchValueData {Name = "AnniversaryDay", Value = schedule.AnniversaryDay.ToString()},
        //                new SearchValueData {Name = "AnniversaryMonth", Value = schedule.AnniversaryMonth.ToString()}
        //            };
        //    long scheduleId = (long)GetScalar(StoredProcedure.UpdateAppointmentToScheduleMaster, param);
        //    response.Data = scheduleId;
        //    response.IsSuccess = true;

        //    return response;
        //}

        public ServiceResponse GetNurseSchedules(string careTypeIds, string employeeIds, string referralIds)
        {
            var response = new ServiceResponse();
            List<ScheduleViewModel> detailModel = GetEntityList<ScheduleViewModel>(StoredProcedure.GetNurseScheduleMaster,
                new List<SearchValueData>{
                        new SearchValueData{Name = "CareTypeIds",Value = careTypeIds},
                        new SearchValueData{Name = "EmployeeIds",Value = employeeIds},
                        new SearchValueData{Name = "ReferralIds",Value = referralIds}
                    });

            response.Data = detailModel;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse SaveVisitReason(VisitReasonModel model, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();

            var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ScheduleID", Value = Convert.ToString(model.ScheduleID)},
                    new SearchValueData {Name = "ReasonType", Value = model.ReasonType},
                    new SearchValueData {Name = "ReasonCode", Value = model.ReasonCode},
                    new SearchValueData {Name = "ActionCode", Value = model.ActionCode},
                    new SearchValueData {Name = "CompanyName", Value = model.CompanyName},
                    new SearchValueData {Name = "LoggedInUserID", Value = Convert.ToString(loggedInUserID)},
                };

            long data = Convert.ToInt64(GetScalar(StoredProcedure.SaveVisitReason, searchlist));
            response.Data = data;
            response.IsSuccess = true;

            return response;
        }

        public ServiceResponse GetVisitReasonModalDetail(long scheduleID)
        {
            ServiceResponse response = new ServiceResponse();

            var searchlist = new List<SearchValueData>
                {
                    new SearchValueData { Name = "ScheduleID", Value = Convert.ToString(scheduleID)},
                    new SearchValueData { Name = "HHAXAggregator", Value = Common.PayorClaimProcessors.HHAExchange.Value },
                    new SearchValueData { Name = "CareBridgeAggregator", Value = Common.PayorClaimProcessors.Carebridge.Value },
                    new SearchValueData { Name = "TellusAggregator", Value = Common.PayorClaimProcessors.Tellus.Value },
                    new SearchValueData { Name = "SandataAggregator", Value = Common.PayorClaimProcessors.Sandata.Value }
                };

            var data = GetEntity<VisitReasonModalDetail>(StoredProcedure.GetVisitReasonModalDetail, searchlist);
            response.Data = data;
            response.IsSuccess = true;

            return response;
        }
        #endregion

    }
}