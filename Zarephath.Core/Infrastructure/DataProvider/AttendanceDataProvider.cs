using System.Collections.Generic;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class AttendanceDataProvider : BaseDataProvider, IAttendanceDataProvider
    {
        #region Attendance Master

        public ServiceResponse SetAttendanceMasterModel()
        {
            ServiceResponse response = new ServiceResponse();

            AttendanceMasterPageModel attendanceMasterPageModel = GetMultipleEntity<AttendanceMasterPageModel>(StoredProcedure.SetAttendanceMasterPage);
            attendanceMasterPageModel.CancellationReasons = Common.SetCancellationReasons();
            //attendanceMasterPageModel.Facilities =
            //    GetEntityList<FacilityModel>(new List<SearchValueData>
            //        {
            //            new SearchValueData {Name = "IsDeleted", Value = "0"}
            //        }, "", "FacilityName","ASC");

            attendanceMasterPageModel.AttendanceMasterSearchModel.FacilityID = attendanceMasterPageModel.Facilities[0].FacilityID;
            response.IsSuccess = true;
            response.Data = attendanceMasterPageModel;
            return response;
        }

        public ServiceResponse GetAttendanceListByFacility(AttendanceMasterSearchModel searchParam)
        {
            ServiceResponse response = new ServiceResponse();

            FacilityAttendanceDetails detailModel =
                GetMultipleEntity<FacilityAttendanceDetails>(StoredProcedure.GetAttendanceMasterByFacility, new List<SearchValueData>
                    {
                        new SearchValueData{Name = "FacilityID",Value = searchParam.FacilityID.ToString()},
                        new SearchValueData{Name = "ClientName",Value = searchParam.ClientName},
                        new SearchValueData{Name = "StartDate",Value = searchParam.StartDate.ToString(Constants.DbDateFormat)},
                        new SearchValueData{Name = "EndDate",Value = searchParam.EndDate.ToString(Constants.DbDateFormat)}
                    });

            response.Data = detailModel;
            response.IsSuccess = true;

            return response;
        }

        public ServiceResponse UpdateAttendance(AttendanceDetail model, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();



            if (model != null && model.AttendanceMasterID > 0 && model.ScheduleMasterID > 0)
            {

                AttendanceMaster attendanceMaster = GetEntity<AttendanceMaster>(model.AttendanceMasterID);

                #region Update Schedule Master Details
                ScheduleMaster tempScheduleMaster = GetEntity<ScheduleMaster>(model.ScheduleMasterID);
                if (tempScheduleMaster != null)
                {
                    //if (tempScheduleMaster.ScheduleStatusID == (int)ScheduleStatus.ScheduleStatuses.Confirmed && (tempScheduleMaster.IsAssignedToTransportationGroupDown || tempScheduleMaster.IsAssignedToTransportationGroupUp))
                    //{
                    //    response.Message = Resource.Youcannotchangethestatusasclient;
                    //    return response;
                    //}
                    if (model.ScheduleStatusID != (int)ScheduleStatus.ScheduleStatuses.Cancelled)
                    {
                        tempScheduleMaster.WhoCancelled = null;
                        tempScheduleMaster.WhenCancelled = null;
                        tempScheduleMaster.CancelReason = null;
                    }
                    else
                    {
                        tempScheduleMaster.WhoCancelled = model.WhoCancelled;
                        tempScheduleMaster.WhenCancelled = model.WhenCancelled;
                        tempScheduleMaster.CancelReason = model.CancelReason;
                    }

                    tempScheduleMaster.ScheduleStatusID = attendanceMaster.AttendanceStatus == null ? (int)ScheduleStatus.ScheduleStatuses.Confirmed : model.ScheduleStatusID;
                    tempScheduleMaster.Comments = model.Comments;
                    SaveObject(tempScheduleMaster, loggedInUserID);
                    //tempScheduleMaster.PickUpLocation = scheduleMaster.PickUpLocation;
                    //tempScheduleMaster.DropOffLocation = scheduleMaster.DropOffLocation;
                    //tempScheduleMaster.Comments = scheduleMaster.Comments;
                    //tempScheduleMaster.FacilityID = scheduleMaster.FacilityID;
                    //ServiceResponse res = SaveScheduleMaster(tempScheduleMaster, loggedInUserID);

                    if (tempScheduleMaster.ScheduleStatusID == (int)ScheduleStatus.ScheduleStatuses.Confirmed)
                        attendanceMaster.AttendanceStatus = (int)AttendanceMaster.AttendanceStatuses.Present;
                    else
                        attendanceMaster.AttendanceStatus = (int)AttendanceMaster.AttendanceStatuses.Absent;

                    attendanceMaster.Comment = model.Comments ?? model.CancelReason;
                    SaveObject(attendanceMaster, loggedInUserID);


                    //SET LAST ATTENDANCE AS LAST CONFIRMED STATUS DATE
                    GetScalar(StoredProcedure.UpdateReferralLastAttDate,
                    new List<SearchValueData> { new SearchValueData { Name = "ReferralID", Value = attendanceMaster.ReferralID.ToString() },
                        new SearchValueData { Name = "ScheduleStatusID", Value = ((int)ScheduleStatus.ScheduleStatuses.Confirmed).ToString() } });
                        //new SearchValueData { Name = "PresentStatus", Value = ((int)AttendanceMaster.AttendanceStatuses.Present).ToString() } });


                    model.AttendanceStatus = attendanceMaster.AttendanceStatus;
                    response.IsSuccess = true;
                    response.Data = model;
                }
                else
                    response.Message = Resource.ExceptionMessage;

                #endregion




                //switch (attendanceMaster.AttendanceStatus)
                //{
                //    case null:
                //        attendanceMaster.AttendanceStatus = (int)AttendanceMaster.AttendanceStatuses.Present;
                //        break;
                //    case (int)AttendanceMaster.AttendanceStatuses.Present:
                //        attendanceMaster.AttendanceStatus = (int)AttendanceMaster.AttendanceStatuses.Absent;
                //        break;
                //    case (int)AttendanceMaster.AttendanceStatuses.Absent:
                //        attendanceMaster.AttendanceStatus = (int)AttendanceMaster.AttendanceStatuses.Present;
                //        break;
                //}
                //SaveObject(attendanceMaster, loggedInUserID);

                //GetScalar(StoredProcedure.UpdateReferralLastAttDate,
                //          new List<SearchValueData>
                //              {
                //                  new SearchValueData {Name = "ReferralID", Value = attendanceMaster.ReferralID.ToString()},
                //                  new SearchValueData
                //                      {
                //                          Name = "PresentStatus",
                //                          Value = ((int) AttendanceMaster.AttendanceStatuses.Present).ToString()
                //                      }
                //              });

                //SessionValueData
                //Employee employee = GetEntity<Employee>(attendanceMaster.UpdatedBy);
                //if (employee != null)
                //    attendanceMaster.UpdatedByName = employee.FirstName + ' ' + employee.LastName;


            }
            else
            {
                response.Message = Resource.ExceptionMessage;
            }
            return response;
        }

        public ServiceResponse UpdateCommentForAttendance(AttendanceDetail model, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();
            if (model != null && model.AttendanceMasterID > 0)
            {
                AttendanceMaster attendanceMaster = GetEntity<AttendanceMaster>(model.AttendanceMasterID);
                attendanceMaster.Comment = model.Comment;
                SaveObject(attendanceMaster, loggedInUserID);

                Employee employee = GetEntity<Employee>(attendanceMaster.UpdatedBy);
                if (employee != null)
                    attendanceMaster.UpdatedByName = employee.FirstName + ' ' + employee.LastName;

                response.IsSuccess = true;
                response.Data = attendanceMaster;
                response.Message = Resource.CommentUpdated;
            }
            else
            {
                response.Message = Resource.ExceptionMessage;
            }
            return response;
        }

        #endregion
    }
}
