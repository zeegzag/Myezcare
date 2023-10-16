using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ChecklistItems")]
    [PrimaryKey("ChecklistItemID")]
    [Sort("ChecklistItemID", "DESC")]
    public class ChecklistItem : BaseEntity
    {
        public long ChecklistItemID { get; set; }

        public int ChecklistItemTypeID { get; set; }

        public string StepName { get; set; }

        public string StepDescription { get; set; }

        public string ChecklistTypeControl { get; set; }

        public bool IsDocumentRequired { get; set; }

        public int? DocumentTypeID { get; set; }

        public bool IsMandatory { get; set; }

        public bool IsAutomatic { get; set; }

        public string SPName { get; set; }
    }
}
