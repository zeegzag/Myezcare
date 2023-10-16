using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Resources;


namespace Zarephath.Core.Models.ViewModel
{
    public class PCAModel
    {
        public PCAModel()
        {
            PCADetail = new PCADetail();
            TaskList = new List<TaskLists>();
            ConclusionList = new List<TaskLists>();
            PcaTaskList = new List<Categories>();
            PcaConclusionList = new List<Categories>();
            SearchEmployeeVisitNoteListPage = new SearchEmployeeVisitNoteListPage();
            DeleteVisitNoteFilter = new List<NameValueData>();

        }
        public PCADetail PCADetail { get; set; }
        public List<TaskLists> TaskList { get; set; }
        public List<TaskLists> ConclusionList { get; set; }

        [Ignore]
        public List<Categories> PcaTaskList { get; set; }
        [Ignore]
        public List<Categories> PcaConclusionList { get; set; }
        [Ignore]
        public SearchEmployeeVisitNoteListPage SearchEmployeeVisitNoteListPage { get; set; }
        [Ignore]
        public List<NameValueData> DeleteVisitNoteFilter { get; set; }


    }

    public class PCADetail
    {
        public long EmployeeVisitID { get; set; }
        public string BeneficiaryName { get; set; }
        public string EmployeeName { get; set; }
        public string BeneficiaryID { get; set; }
        public string EmployeeUniqueID { get; set; }
        public string PlaceOfService { get; set; }
        public string HHA_PCA_Name { get; set; }
        public string HHA_PCA_NP { get; set; }
        public string OtherActivity { get; set; }
        public long OtherActivityTime { get; set; }
        public string PatientSignature { get; set; }
        public string PatientSignature_ClockOut { get; set; }
        public string Name { get; set; }
        public string Relation { get; set; }
        public bool IsSelf { get; set; }
        public string EmployeeSignature { get; set; }
        public string ClockInTime { get; set; }
        public string ClockOutTime { get; set; }
        public DateTime Date { get; set; }

        public string ServiceDate
        {

            get { return Date.ToString(Constants.GlobalDateFormat); }
        }
        public string DayOfWeek { get; set; }

        public string TotalTime { get; set; }
        
    }


    public class TaskLists
    {
        public string VisitTaskDetail { get; set; }
        public string CategoryName { get; set; }
        public long? CategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public long? SubCategoryId { get; set; }
        public long ReferralTaskMappingID { get; set; }
        public long MinimumTimeRequired { get; set; }
        public string Answer { get; set; }
        public long ServiceTime { get; set; }

        public bool ConclusionAnswer { get; set; }
        public bool IsRequired { get; set; }
        public bool SimpleTaskType { get; set; }
    }


    public class Categories
    {
        public Categories()
        {
            SubCategory = new List<SubCategory>();
        }
        public long? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<SubCategory> SubCategory { get; set; }
        public List<TaskLists> TaskLists { get; set; }
    }

    public class SubCategory
    {
        public SubCategory()
        {
            TaskLists = new List<TaskLists>();
        }
        public long? SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public List<TaskLists> TaskLists { get; set; }
    }

    public class Task
    {
        public string VisitTaskDetail { get; set; }
    }

    public class MultiplePCA
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }

}
