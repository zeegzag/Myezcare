using System.Collections.Generic;
using Zarephath.Core.Models.Entity;
using PetaPoco;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Zarephath.Core.Resources;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Utility;

namespace Zarephath.Core.Models.ViewModel
{
    public class ComplianceModel
    {
        public ComplianceModel()
        {
            Compliance = new Compliance();
            SearchComplianceListPage = new SearchComplianceListPage();
            DeleteFilter = new List<NameValueData>();
            UserTypeList = new List<NameValueData>();
            DocumentationTypeList = new List<NameValueData>();
            DirectoryList = new List<NameValueData>();
            ConfigEBFormModel = new ConfigEBFormModel();
            RolesList = new List<NameValueData>();
            SubSectionList = new List<NameValueData>();
        }
        public Compliance Compliance { get; set; }
        public SearchComplianceListPage SearchComplianceListPage { get; set; }
        public List<NameValueData> DeleteFilter { get; set; }
        public List<NameValueData> UserTypeList { get; set; }
        public List<NameValueData> DocumentationTypeList { get; set; }
        public List<NameValueData> DirectoryList { get; set; }
        public ConfigEBFormModel ConfigEBFormModel { get; set; }
        public List<NameValueData> RolesList { get; set; }
        public List<NameValueData> SubSectionList { get; set; }
    }

    public class SetCompliancePage
    {
        public SetCompliancePage()
        {
            SectionList = new List<NameValueData>();
            SubSectionList = new List<SubSectionData>();
        }
        public List<NameValueData> SectionList { get; set; }
        public List<SubSectionData> SubSectionList { get; set; }
    }

    public class SubSectionData
    {
        public long Value { get; set; }
        public string Name { get; set; }
        public long ParentID { get; set; }
    }

    public class SearchComplianceListPage
    {
        public string DocumentName { get; set; }
        public int? UserType { get; set; }
        public int? DocumentationType { get; set; }
        public int? IsTimeBased { get; set; }
        public int IsDeleted { get; set; }
        public string ListOfIdsInCsv { get; set; }
        public string ParentName { get; set; }

        public string Type { get; set; }
        public long SectionID { get; set; }
        public long SubSectionID { get; set; }
        public int ShowToAll { get; set; }
    }

    public class ComplianceListModel
    {
        public long ComplianceID { get; set; }
        public int UserType { get; set; }
        public int DocumentationType { get; set; }
        public string DocumentName { get; set; }
        public bool IsTimeBased { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public long ParentID { get; set; }
        public string ParentName { get; set; }

        public int DocumentCount { get; set; }

        //Start EBForm Properties
        public string EBFormID { get; set; }
        public string FormId { get; set; }
        public string FormName { get; set; }
        public string NameForUrl { get; set; }
        public string Version { get; set; }
        public string InternalFormPath { get; set; }
        public string IsInternalForm { get; set; }
        public string IsOrbeonForm { get; set; }
        //End EBForm Properties

        public bool IsDeleted { get; set; }
        public int Row { get; set; }
        public int Count { get; set; }

        public string SelectedRoles { get; set; }
        public long SortingID { get; set; }
        public bool ShowToAll { get; set; }
        public long Assignee { get; set; }

        public string str_UserType
        {
            get { return EnumHelper<Common.UserType>.GetDisplayValue((Common.UserType)UserType); }
        }
        public string str_DocumentationType
        {
            get { return EnumHelper<Common.DocumentationType>.GetDisplayValue((Common.DocumentationType)DocumentationType); }
        }
        public string str_IsTimeBased
        {
            get
            {
                return IsTimeBased ? "Yes" : "No";
            }
        }
    }

    public class DocumentSection
    {
        public DocumentSection()
        {
            DocumentSubSectionList = new List<DocumentSubSection>();
        }
        public string SectionName { get; set; }
        public List<DocumentSubSection> DocumentSubSectionList { get; set; }
        public List<ReferralComplianceDetails> DocumentList { get; set; }
    }

    public class DocumentSubSection
    {
        public DocumentSubSection()
        {
            DocumentList = new List<ReferralComplianceDetails>();
        }
        public string SubSectionName { get; set; }
        public List<ReferralComplianceDetails> DocumentList { get; set; }
    }
    public class ChangeSortingOrderModel
    {
        public long ComplianceID { get; set; }
        public long originID { get; set; }
        public long destinationID { get; set; }
    }
}  