using PetaPoco;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Infrastructure;

namespace Zarephath.Core.Models.ViewModel
{
    public class SetGroupTimesheetPage
    {
        public SetGroupTimesheetPage()
        {
            SearchGroupTimesheetListPage = new SearchGroupTimesheetListPage();
            EmployeeList = new List<EmployeeListModel>();
            FacilityList = new List<FacilityListModel>();
            ReferralList = new List<ReferralListModel>();
            PayorList = new List<PayorModelList>();
            CareTypeList = new List<CareTypemodel>();
            //TypesOfTimeSheet = new List<TypesOfTimeSheet>();
        }
        [Ignore]
        public SearchGroupTimesheetListPage SearchGroupTimesheetListPage { get; set; }
        public List<EmployeeListModel> EmployeeList { get; set; }
        public List<FacilityListModel> FacilityList { get; set; }
        public List<ReferralListModel> ReferralList { get; set; }
        public List<PayorModelList> PayorList { get; set; }
        public List<CareTypemodel> CareTypeList { get; set; }
        //public List<TypesOfTimeSheet> TypesOfTimeSheet { get; set; }
    }


    public class SearchGroupTimesheetListPage
    {
        public string EmployeeIDs { get; set; }
        public string FacilityIDs { get; set; }
        public string ReferralIDs { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PayorIDs { get; set; }
        public string CareTypeIDs { get; set; }
        public string TypesOfTimeSheet { get; set; }
    }

    public class SaveGroupTimesheet
    {
        public long ScheduleID { get; set; }
        public DateTime? ClockInDateTime { get; set; }
        public DateTime? ClockOutDateTime { get; set; }
    }

    public class SaveGroupTimesheetList
    {
        public string Remarks { get; set; }
        public List<SaveGroupVisitTask> TaskList { get; set; }
        public List<SaveGroupTimesheet> TimesheetDetails { get; set; }
    }

    public class SaveGroupVisitTask
    {
        public long VisitTaskID { get; set; }
        public bool IsDetail { get; set; }
        public int ServiceTime { get; set; }
        public string Remarks { get; set; }
        public string TaskOption { get; set; }

    }


    public class GroupVisitTask : SaveGroupVisitTask
    {
        public string VisitTaskDetail { get; set; }
    }
    public class GroupVisitTaskModels
    {
        public GroupVisitTaskModels()
        {

            //DayCare_GetSchedulePatientTask = new DayCare_GetSchedulePatientTask();
            SaveGroupVisitTask = new List<SaveGroupVisitTask>();
            VisitTaskOptionList = new List<VisitTaskOptionList>();

        }
        //public DayCare_GetSchedulePatientTask DayCare_GetSchedulePatientTask { get; set; }
        public List<SaveGroupVisitTask> SaveGroupVisitTask { get; set; }
        public List<VisitTaskOptionList> VisitTaskOptionList { get; set; }
    }


    public class GroupVisitTaskModel
    {
        public List<GroupVisitTask> VisitTaskList { get; set; }
        public List<NameValueData> HourList { get; set; }
        public List<NameValueData> MinuteList { get; set; }
    }
}
