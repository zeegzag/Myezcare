using PetaPoco;
using System.ComponentModel.DataAnnotations;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("VisitTaskCategories")]
    [PrimaryKey("VisitTaskCategoryID")]
    [Sort("VisitTaskCategoryID", "DESC")]
    public class VisitTaskCategory
    {
        public long VisitTaskCategoryID { get; set; }

        public string VisitTaskCategoryName { get; set; }

        public string VisitTaskCategoryType { get; set; }

        public long? ParentCategoryLevel { get; set; }
        public string ParentTaskType { get; set; }

        public string ParentCategoryName { get; set; }

        public enum CategoryTypeEnum
        {
            Task = 1,
            Conclusion = 2
        }
    }
}
