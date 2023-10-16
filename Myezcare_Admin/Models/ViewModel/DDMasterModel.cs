using System.Collections.Generic;
using Myezcare_Admin.Models.Entity;
using PetaPoco;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Myezcare_Admin.Resources;
using Myezcare_Admin.Infrastructure;

namespace Myezcare_Admin.Models.ViewModel
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
        }
        public DDMaster DDMaster { get; set; }
        public List<DDMasterType> TypeList { get; set; }
        public List<NameValueData> DeleteFilter { get; set; }
        public SearchDDMasterListPage SearchDDMasterListPage { get; set; }

        [Ignore]
        public bool IsShowButtonForDisplay { get; set; }
        [Ignore]
        public MappingDDMaster MappingDDMaster { get; set; }
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

    public class CareType
    {
        public long ID { get; set; }
        public string Name { get; set; }
    }

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
