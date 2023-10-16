using System.Collections.Generic;
using Zarephath.Core.Models.Entity;
using PetaPoco;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Zarephath.Core.Resources;
using Zarephath.Core.Infrastructure;

namespace Zarephath.Core.Models.ViewModel
{
    public class DDMasterModel
    {
        public DDMasterModel()
        {
            DDMaster = new DDMaster();
            TypeList = new List<DDMasterType>();
            DeleteFilter = new List<NameValueData>();
            SearchDDMasterListPage = new SearchDDMasterListPage();
            MappingDDMaster = new MappingDDMaster();
            IsPartial = true;
        }
        public DDMaster DDMaster { get; set; }
        public List<DDMasterType> TypeList { get; set; }
        public List<NameValueData> DeleteFilter { get; set; }
        public SearchDDMasterListPage SearchDDMasterListPage { get; set; }

        [Ignore]
        public bool IsShowButtonForDisplay { get; set; }
        [Ignore]
        public MappingDDMaster MappingDDMaster { get; set; }
        public bool IsPartial { get; set; }

    }

    public class SearchDDMasterListPage
    {
        public string ItemType { get; set; }
        public string Title { get; set; }
        public string ListOfIdsInCsv { get; set; }
        public int IsDeleted { get; set; }
        public string Value { get; set; }
    }

    public class DDMasterListTransactionModel
    {
        public DDMasterListTransactionModel()
        {
            DDMasterListModel = new List<DDMasterListModel>();
        }
        public int ResultId { get; set; }
        public List<DDMasterListModel> DDMasterListModel { get; set; }
    }


    public class DDMasterListModel
    {
        public long DDMasterID { get; set; }
        public string ItemType { get; set; }
        public string Title { get; set; }
        public long DDMasterTypeID { get; set; }
        public bool IsDeleted { get; set; }
        public string Value { get; set; }
        public int Row { get; set; }
        public int Count { get; set; }
    }


    public class ParentChildMappingModel
    {
        
        public ParentChildMappingModel()
        {
            ParentChildList = new List<ParentChildDDLModel>();
            DDMasterList = new List<DDMaster>();
        }
        public List<ParentChildDDLModel> ParentChildList { get; set; }
        public List<DDMaster> DDMasterList { get; set; }
        public bool IsMultiSelect { get; set; }

        [Ignore]
        public List<long> SelectedParentChildValueItems { get { return DDMasterList.Where(x => x.ParentID != 0).Select(x => x.DDMasterID).ToList(); } }

    }

    public class ParentChildDDLModel
    {
        public long DDMasterTypeID { get; set; }
        public string Name { get; set; }
        public bool IsChild { get; set; }
        public int SortOrder { get; set; }
    }

    public class DDMasterTypeModel
    {
        public long DDMasterID { get; set; }
        public long DDMasterTypeID { get; set; }
    }


    public class CareType
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }

    public class ServiceType
    {
        public string Value { get; set; }
        public string Name { get; set; }
    }

    public class RevenueCode
    {
        public string Value { get; set; }
        public string Name { get; set; }
    }
    public class ServiceCode
    {
        public string Value { get; set; }
        public string Name { get; set; }
    }
    //public class Designation
    //{
    //    public long ID { get; set; }
    //    public string Name { get; set; }
    //}

    public class MappingDDMaster
    {
        public MappingDDMaster()
        {
            ParentTypeList = new List<DDMasterType>();
        }

        [Required(ErrorMessageResourceType = typeof(Resource),ErrorMessageResourceName = "ParentItemTypeIsRequired")]
        public string LuDDTypesParent { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "ChildItemTypeIsRequired")]
        public string LuDDTypesChild { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "ParentTaskIsRequired")]
        public string DDMasterIDParent { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "ChildTaskIsRequired")]
        public string DDMasterIDChild { get; set; }
        public List<DDMasterType> ParentTypeList { get; set; }
    }

    public class ParentGeneralDetailForMapping
    {
        public ParentGeneralDetailForMapping()
        {
            DDMasterTypesList = new List<DDMasterType>();
            DDMasterList = new List<DDMaster>();
        }
        public List<DDMasterType> DDMasterTypesList { get; set; }
        public List<DDMaster> DDMasterList { get; set; }

        [Ignore]
        public List<long> DDMasterIDChild { get { return DDMasterList.Where(x => x.ParentID != 0).Select(x => x.DDMasterID).ToList(); } }
    }
}
