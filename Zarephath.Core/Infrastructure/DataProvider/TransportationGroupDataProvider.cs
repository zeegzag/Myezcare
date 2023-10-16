using System;
using System.Collections.Generic;
using PetaPoco;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;
using System.Linq;
using Zarephath.Core.Resources;
using iTextSharp.text;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class TransportationGroupDataProvider : BaseDataProvider, ITransportationGroupDataProvider
    {

        #region Transportation Assignment
        public ServiceResponse SetTransPortationGroup()
        {
            ServiceResponse response = new ServiceResponse();
            TransportationAssignmentModel setTransportatioGroupListPage = GetMultipleEntity<TransportationAssignmentModel>(StoredProcedure.SetTransportaionAssignmnetModel);
            //setTransportatioGroupListPage.SearchRefrralForTransportatiAssignment.StartDate = DateTime.UtcNow;
            setTransportatioGroupListPage.SearchRefrralForTransportatiAssignment.TripDirection =
                TransportationGroup.TripDirectionUp;
            response.Data = setTransportatioGroupListPage;
            return response;
        }

        public ServiceResponse GetReferralListForTransportationAssignment(SearchReferralListForTransportationAssignment searchTransportatioGroupList,
            int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchTransportatioGroupList != null)
                SetSearchFilterForReferralList(searchTransportatioGroupList, searchList);

            Page<ReferralListForTransportationAssignment> page = GetEntityPageList<ReferralListForTransportationAssignment>(StoredProcedure.GetReferralListForTransportationAssignment, searchList, pageSize, pageIndex, sortIndex, sortDirection);
            response.Data = page;
            response.IsSuccess = true;
            return response;
        }

        private static void SetSearchFilterForReferralList(SearchReferralListForTransportationAssignment searchTransportatioGroupList, List<SearchValueData> searchList)
        {
            if (searchTransportatioGroupList.FacilityID > 0)
                searchList.Add(new SearchValueData
                {
                    Name = "FacilityID",
                    Value = Convert.ToString(searchTransportatioGroupList.FacilityID)
                });

            if (searchTransportatioGroupList.TransportLocationID > 0)
                searchList.Add(new SearchValueData
                {
                    Name = "TransportLocationID",
                    Value = Convert.ToString(searchTransportatioGroupList.TransportLocationID)
                });

            if (searchTransportatioGroupList.StartDate != null)
                searchList.Add(new SearchValueData
                {
                    Name = "StartDate",
                    Value = searchTransportatioGroupList.StartDate.Value.ToString(Constants.DbDateFormat)
                });
            if (searchTransportatioGroupList.TripDirection != null)
                searchList.Add(new SearchValueData
                {
                    Name = "TripDirection",
                    Value = searchTransportatioGroupList.TripDirection
                });

            searchList.Add(new SearchValueData
                {
                    Name = "DisallowScheduleStatuses",
                    Value = string.Join(",", new List<String>
                        {
                            ((int) ScheduleStatus.ScheduleStatuses.Cancelled).ToString(),
                            ((int) ScheduleStatus.ScheduleStatuses.No_Confirmation).ToString(),
                            //((int) ScheduleStatus.ScheduleStatuses.No_Show).ToString(),
                            //((int) ScheduleStatus.ScheduleStatuses.Waiting_List).ToString(),
                        })
                });

            searchList.Add(new SearchValueData
            {
                Name = "ContactTypeID",
                Value = Convert.ToString((int)Common.ContactTypes.PrimaryPlacement)
            });
        }

        public ServiceResponse SaveTransportationGroup(AddTransportationGroupModel transportationGroup,
            SearchAssignedClientListForTransportationAssignment searchTransportatioGroupList, long loggedInUser)
        {
            ServiceResponse response = new ServiceResponse();
            bool isediting = transportationGroup.TransportationGroup.TransportationGroupID > 0;

            if (transportationGroup.TransportationGroup != null)
            {
                // As Client Request Removed staff validation.
                //response = ValidateTransportationGroup(transportationGroup);
                ////Validate Transportation Group. For Is Staff already assigned to another Group

                //if (!response.IsSuccess)
                //{
                //    return response;
                //}

                if (!isediting)
                {
                    transportationGroup.TransportationGroup.Capacity = ConfigSettings.TransportationGroupCapacity;
                }
                SaveObject(transportationGroup.TransportationGroup, loggedInUser);

                List<TransportationGroupStaff> currentSelectedStaff =
                    GetEntityList<TransportationGroupStaff>(new List<SearchValueData>
                            {
                                new SearchValueData
                                    {
                                        Name = "TransportationGroupID",
                                        Value = transportationGroup.TransportationGroup.TransportationGroupID.ToString(),
                                        IsEqual = true
                                    }
                            });

                //Remove Staff Entry which is deselected.
                foreach (TransportationGroupStaff deletedStaff in currentSelectedStaff.Where(m => !transportationGroup.SelectedStaffs.Contains(m.StaffID)))
                {
                    DeleteEntity<TransportationGroupStaff>(deletedStaff.TransportationGroupStaffID);
                }
                //Add Newly selected Transportation Group.
                foreach (int selecteStaffID in transportationGroup.SelectedStaffs.Where(m => !(currentSelectedStaff.Select(s => s.StaffID).Contains(m))))
                {
                    TransportationGroupStaff newSelectedStaff = new TransportationGroupStaff
                        {
                            TransportationGroupID = transportationGroup.TransportationGroup.TransportationGroupID,
                            StaffID = selecteStaffID
                        };
                    SaveObject(newSelectedStaff, loggedInUser);
                }
                response.IsSuccess = true;
                response.Message = isediting
                                       ? Resource.TransportationGroupUpdatedSuccess
                                       : Resource.TransportationGroupAddedSuccess;
                response.Data = GetAssignedClientListForTransportationAssignment(searchTransportatioGroupList).Data;

            }
            else
            {
                response.Message = Common.MessageWithTitle(string.Format(Resource.CreateFailed, Resource.TransportationGroups),
                                            Resource.ExceptionMessage);
            }
            return response;
        }


        public ServiceResponse RemoveTransportationGroup(long transportationGroupID,
            SearchAssignedClientListForTransportationAssignment searchTransportatioGroupList, long loggedInUser)
        {
            ServiceResponse response = new ServiceResponse();

            //Check whether Transportation Group have assigned clients.
            List<TransportationGroupClient> clients = GetEntityList<TransportationGroupClient>(new List<SearchValueData>
                {
                    new SearchValueData
                        {
                            Name = "TransportationGroupID",
                            Value = transportationGroupID.ToString(),
                            IsEqual = true
                        }
                });

            //First Remove All Client of That Transportation Group.
            foreach (TransportationGroupClient transportationGroupClient in clients)
            {
                RemoveTransportationGroupClient(transportationGroupClient.TransportationGroupClientID, loggedInUser);
            }
            //if (count > 0) // If Client Assigneed To Transportation Group 
            //{
            //    response.Message = Resource.CanNotDeleteTransportationGroup;
            //}
            //else
            //{
            TransportationGroup transportationGroup = GetEntity<TransportationGroup>(transportationGroupID);
            if (transportationGroup != null)
            {
                transportationGroup.IsDeleted = true;
                SaveObject(transportationGroup, loggedInUser);
                response.IsSuccess = true;
                response.Message = Resource.TransportationGroupRemovedSuccess;
                response.Data = GetAssignedClientListForTransportationAssignment(searchTransportatioGroupList).Data;
            }
            else
            {
                response.Message = Resource.ErrorOccured;
            }
            //}
            return response;
        }


        public ServiceResponse GetAssignedClientListForTransportationAssignment(SearchAssignedClientListForTransportationAssignment searchTransportatioGroupList)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>
                {
                    new SearchValueData
                        {
                            Name = "Date",
                            Value = searchTransportatioGroupList.Date.ToString(Constants.DbDateFormat)
                        }
                };
            List<AssignedClientListForTransportationAssignment> list = GetEntityList<AssignedClientListForTransportationAssignment>(StoredProcedure.GetAssignedClientListForTransportationAssignment, searchList);
            var nlist = (from l in list
                         group l by l.TransportationGroupID
                             into g
                             select new TransportationGroupList
                             {
                                 TransportationGroup = g.FirstOrDefault(m => m.TransportationGroupID == g.Key),
                                 ClientList = g.Where(m => m.TransportationGroupClientID != 0).OrderBy(m => m.Name).ToList()
                             }).ToList();
            response.Data = new
            {
                UpList = nlist.Where(m => m.TransportationGroup.TripDirection == TransportationGroup.TripDirectionUp).OrderBy(m=> m.TransportationGroup.TransportationGroupID),
                DownList = nlist.Where(m => m.TransportationGroup.TripDirection == TransportationGroup.TripDirectionDown).OrderBy(m => m.TransportationGroup.TransportationGroupID)
            }
            ;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse SaveTransportationGroupClient(TransportationGroupClient transportationGroupClient,
                                 long loggedInUser)
        {
            ServiceResponse response = new ServiceResponse();
            if (transportationGroupClient != null)
            {
                bool isediting = transportationGroupClient.TransportationGroupClientID > 0;

                
                
                
                int count =
                    GetEntityCount<TransportationGroupClient>(new List<SearchValueData>
                        {
                            new SearchValueData
                                {
                                    Name = "TransportationGroupID",
                                    Value = transportationGroupClient.TransportationGroupID.ToString()
                                }
                        });
                if (count >= ConfigSettings.TransportationGroupCapacity)
                {
                    response.Message = "You can not assign more than " + ConfigSettings.TransportationGroupCapacity + " clients in one transportation group";
                    return response;
                }

                if (!isediting)
                {
                    int cnt =
                    GetEntityCount<TransportationGroupClient>(new List<SearchValueData>
                        {
                            new SearchValueData
                                {
                                    Name = "TransportationGroupID",
                                    Value = transportationGroupClient.TransportationGroupID.ToString()
                                },
                                new SearchValueData
                                {
                                    Name = "ScheduleID",
                                    Value = transportationGroupClient.ScheduleID.ToString()
                                }
                        });
                    if (cnt >0)
                    {
                        response.Message = Resource.ClientAlreadyAssignedToTransportaionGroup;
                        return response;
                    }

                }




                ScheduleMaster schedule = GetEntity<ScheduleMaster>(transportationGroupClient.ScheduleID);
                TransportationGroup transportationGroup = GetEntity<TransportationGroup>(transportationGroupClient.TransportationGroupID);
                if (schedule == null || transportationGroup == null)//Schedule Not Exist Send Error Message.
                {
                    response.Message = Common.MessageWithTitle(string.Format(Resource.CreateFailed, Resource.TransportationGroups), Resource.ExceptionMessage);
                    return response;
                }

                SaveObject(transportationGroupClient, loggedInUser);

                if (transportationGroup.TripDirection == TransportationGroup.TripDirectionUp)
                {
                    List<SearchValueData> searchParamTransportation = new List<SearchValueData>
                        {
                            new SearchValueData
                                {
                                    Name = "TransportationGroupClientID",
                                    Value = transportationGroupClient.TransportationGroupClientID.ToString()
                                },
                            new SearchValueData
                                {
                                    Name = "LoggedInUserID",
                                    Value = transportationGroupClient.TransportationGroupClientID.ToString()
                                },
                        };

                    GetScalar(StoredProcedure.GenerateDownTransportationGroupAndClient, searchParamTransportation);
                }

                //Update Schedule and Set IsAssignedToTransportationGroup flag.
                if (transportationGroup.TripDirection == TransportationGroup.TripDirectionUp)
                {
                    schedule.IsAssignedToTransportationGroupUp = true;
                    schedule.IsAssignedToTransportationGroupDown = true;
                }
                if (transportationGroup.TripDirection == TransportationGroup.TripDirectionDown)
                {
                    schedule.IsAssignedToTransportationGroupDown = true;
                }
                SaveObject(schedule, loggedInUser);

                //Add Record in AttendanceMaster And TransportationGroupFilters For Seat Preference .
                if (!isediting)
                {
                    if (transportationGroup.TripDirection == TransportationGroup.TripDirectionUp)
                    {
                        AttendanceMaster attendanceMaster = new AttendanceMaster
                            {
                                ReferralID = schedule.ReferralID,
                                ScheduleMasterID = schedule.ScheduleID,
                                StartDate = schedule.StartDate,
                                EndDate = schedule.EndDate
                            };
                        SaveObject(attendanceMaster, loggedInUser);
                    }

                    #region Code for seat preference (Add Transportation Group Filters)

                    GetScalar(StoredProcedure.SetTransportationGroupClientFilter, new List<SearchValueData>
                        {
                            new SearchValueData {Name = "ReferralID", Value = schedule.ReferralID.ToString()},
                            new SearchValueData
                                {
                                    Name = "TransportationGroupClientID",
                                    Value = transportationGroupClient.TransportationGroupClientID.ToString()
                                }
                        });


                    #endregion
                }
                response.IsSuccess = true;
                response.Message = isediting
                                       ? Resource.TransportationGroupClientUpdatedSuccess
                                       : Resource.TransportationGroupClientAddSuccess;
                //response.Data = GetAssignedClientListForTransportationAssignment(searchTransportatioGroupList).Data;
            }
           else
                response.Message = Common.MessageWithTitle(string.Format(Resource.CreateFailed, Resource.TransportationGroups), Resource.ExceptionMessage);

            return response;
        }


        public ServiceResponse SaveTransportationGroupMultipleClient(TransportationGroupClient transportationGroupClient,
                               SearchAssignedClientListForTransportationAssignment searchTransportatioGroupList, long loggedInUser)
        {
            ServiceResponse response = new ServiceResponse();
            if (transportationGroupClient != null)
            {
                transportationGroupClient.ScheduleIDs = transportationGroupClient.ScheduleIDs ?? new List<long>();
                if ( !transportationGroupClient.ScheduleIDs.Any(m => m == transportationGroupClient.ScheduleID))
                {
                    transportationGroupClient.ScheduleIDs.Add(transportationGroupClient.ScheduleID);
                }

                bool isediting = transportationGroupClient.TransportationGroupClientID > 0;
                int count =
                    GetEntityCount<TransportationGroupClient>(new List<SearchValueData>
                        {
                            new SearchValueData
                                {
                                    Name = "TransportationGroupID",
                                    Value = transportationGroupClient.TransportationGroupID.ToString()
                                }
                        });
                if (count + transportationGroupClient.ScheduleIDs.Count > ConfigSettings.TransportationGroupCapacity)
                {
                    response.Message = "You can not assign more than " + ConfigSettings.TransportationGroupCapacity + " clients in one transportation group";
                    return response;
                }

                
                List<long> unsuccessScheduleID = new List<long>();
                foreach (long scheduleID in transportationGroupClient.ScheduleIDs)
                {
                    ServiceResponse innerresponse =SaveTransportationGroupClient(new TransportationGroupClient
                        {
                            ScheduleID = scheduleID,
                            TransportationGroupID = transportationGroupClient.TransportationGroupID
                        }, loggedInUser);
                    if (!innerresponse.IsSuccess)
                    {
                        if (transportationGroupClient.ScheduleIDs.Count == 1)
                        {
                            return innerresponse;
                        }
                        unsuccessScheduleID.Add(scheduleID);
                    }
                }


                if (unsuccessScheduleID.Any())
                {
                    response.ErrorCode = Constants.ErrorCode_Warning;
                    response.IsSuccess = false;
                    response.Message = string.Format(Resource.AllClientNotAssignedToTransportaionGroup, (transportationGroupClient.ScheduleIDs.Count-unsuccessScheduleID.Count));
                    response.Data = unsuccessScheduleID;
                    return response;
                }


                response.IsSuccess = true;
                response.Message = isediting
                                       ? Resource.TransportationGroupClientUpdatedSuccess
                                       : Resource.TransportationGroupClientAddSuccess;
                response.Data = GetAssignedClientListForTransportationAssignment(searchTransportatioGroupList).Data;

            }
            else
                response.Message = Common.MessageWithTitle(string.Format(Resource.CreateFailed, Resource.TransportationGroups), Resource.ExceptionMessage);

            return response;
        }

        public ServiceResponse RemoveTransportationGroupClient(long transportationGroupClientID, long loggedInUser)
        {
            ServiceResponse response = new ServiceResponse();
            TransportationGroupClient transportationGroupClient = GetEntity<TransportationGroupClient>(transportationGroupClientID);

            if (transportationGroupClient != null && transportationGroupClient.TransportationGroupClientID > 0)
            {
                //Update Schedule Master
                ScheduleMaster scheduleMaster = GetEntity<ScheduleMaster>(transportationGroupClient.ScheduleID);
                TransportationGroup transportationGroup = GetEntity<TransportationGroup>(transportationGroupClient.TransportationGroupID);
                if (scheduleMaster == null || transportationGroup == null)//Schedule Not Exist Send Error Message.
                {
                    response.Message = Resource.ErrorOccured;
                    return response;
                }

                //Update Schedule and Set IsAssignedToTransportationGroup flag.
                if (transportationGroup.TripDirection == TransportationGroup.TripDirectionUp)
                {
                    scheduleMaster.IsAssignedToTransportationGroupUp = false;
                    scheduleMaster.IsAssignedToTransportationGroupDown = false;
                    //Remove Attendance Master Record
                    List<AttendanceMaster> attendanceMaster = GetEntityList<AttendanceMaster>(new List<SearchValueData>
                    {
                        new SearchValueData
                            {
                                Name = "ScheduleMasterID",
                                Value = scheduleMaster.ScheduleID.ToString()
                            }
                    });

                    if (attendanceMaster != null)
                    {
                        foreach (var master in attendanceMaster)
                        {
                            DeleteEntity<AttendanceMaster>(master.AttendanceMasterID);
                            
                        }
                    }
                }
                if (transportationGroup.TripDirection == TransportationGroup.TripDirectionDown)
                {
                    scheduleMaster.IsAssignedToTransportationGroupDown = false;
                }
                SaveObject(scheduleMaster, loggedInUser);



                /* Delete assigned client from transportation group, its filter mapping. 
                  It will also remove from Down transportation assignment if removing from UP Transportation group*/
                GetScalar(StoredProcedure.DeleteTransportationGroupClientForDownDirection,
                          new List<SearchValueData>
                              {
                                  new SearchValueData
                                      {
                                          Name = "TransportationGroupClientID",
                                          Value = transportationGroupClient.TransportationGroupClientID.ToString()
                                      }
                              });




                response.IsSuccess = true;
                response.Message = Resource.TransportationGroupClientRemoved;
            }
            else
            {
                response.Message = Resource.ErrorOccured;
                return response;
            }

            return response;
        }


        private ServiceResponse ValidateTransportationGroup(AddTransportationGroupModel transportationGroup)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchModel = new List<SearchValueData>
                { 
                    new SearchValueData { Name = "TransportationGroupID", Value = transportationGroup.TransportationGroup.TransportationGroupID.ToString()},
                    new SearchValueData{Name = "StaffID",Value =  string.Join(",", transportationGroup.SelectedStaffs.ToArray())},
                    new SearchValueData{Name = "TripDirection",Value =  string.Join(",", transportationGroup.TransportationGroup.TripDirection)},
                    new SearchValueData{Name = "TransportationDate ",Value =Convert.ToDateTime(transportationGroup.TransportationGroup.TransportationDate).ToString(Constants.DbDateFormat)},
                };
            List<TransportationGroup> list = GetEntityList<TransportationGroup>(StoredProcedure.ValidateTransportationGroup, searchModel);
            if (list.Count > 0)
            {
                response.Data = list;
                response.Message = string.Format(Resource.StaffAlreadyAssignedWithOtherGroup, string.Join(",", list.Select(m => m.GroupName).ToList()));
                response.IsSuccess = false;
            }
            else
            {
                response.IsSuccess = true;
            }
            return response;
        }


        public ServiceResponse SaveTransportationGroupFilter(SaveTransportationGroupFilter model, SearchAssignedClientListForTransportationAssignment searchTransportatioGroupList, long loggedInUser)
        {
            ServiceResponse response = new ServiceResponse();

            List<TransportationGroupFilterMapping> currentSelectedFilters =
                    GetEntityList<TransportationGroupFilterMapping>(new List<SearchValueData>
                            {
                                new SearchValueData
                                    {
                                        Name = "TransportationGroupClientID",
                                        Value = model.TransportationGroupClientID.ToString(),
                                        IsEqual = true
                                    }
                            });

            //Remove Staff Entry which is deselected.
            foreach (TransportationGroupFilterMapping deletedFilter in currentSelectedFilters.Where(m => !model.TransportationFilters.Contains(m.TransportationFilterID)))
            {
                DeleteEntity<TransportationGroupFilterMapping>(deletedFilter.TransportationGroupFilterMappingID);
            }
            //Add Newly selected Transportation Group.
            foreach (int selecteFilterID in model.TransportationFilters.Where(m => !(currentSelectedFilters.Select(s => s.TransportationFilterID).Contains(m))))
            {
                TransportationGroupFilterMapping newTransportationGroupFilterMapping = new TransportationGroupFilterMapping
                {
                    TransportationGroupClientID = model.TransportationGroupClientID,
                    TransportationFilterID = selecteFilterID
                };
                SaveEntity(newTransportationGroupFilterMapping);
            }

            response.Data = GetAssignedClientListForTransportationAssignment(searchTransportatioGroupList).Data;
            response.IsSuccess = true;
            response.Message = Resource.TransportationFilterSaved;

            return response;
        }

        #endregion
    }
}
