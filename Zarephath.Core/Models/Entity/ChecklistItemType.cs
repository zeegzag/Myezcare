using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ChecklistItemTypes")]
    [PrimaryKey("ChecklistItemTypeID")]
    [Sort("ChecklistItemTypeID", "DESC")]
    public class ChecklistItemType
    {
        public int ChecklistItemTypeID { get; set; }

        public string ChecklistItemTypeName { get; set; }
    }
}
