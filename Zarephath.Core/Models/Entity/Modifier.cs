using PetaPoco;
using System.ComponentModel.DataAnnotations;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ServiceCodeTypes")]
    [PrimaryKey("ServiceCodeTypeID")]
    [Sort("ServiceCodeTypeID", "DESC")]
    public class Modifier
    {
        public long ModifierID { get; set; }

        [Required(ErrorMessageResourceName = "ModifierCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string ModifierCode { get; set; }
        [Required(ErrorMessageResourceName = "ModifierNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string ModifierName { get; set; }

        public bool IsDeleted { get; set; }
    }
}
