using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("DXCodes")]
    [PrimaryKey("DXCodeID")]
    [Sort("DXCodeID", "DESC")]
    public class DXCode
    {
        public long DXCodeID { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "DXCode")]
        [Required(ErrorMessageResourceName = "DXCodeNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string DXCodeName { get; set; }


        [Display(ResourceType = typeof(Resource), Name = "DXCodeWithoutDot")]
        [Required(ErrorMessageResourceName = "DXCodeWithoutDotRequired", ErrorMessageResourceType = typeof(Resource))]
        public string DXCodeWithoutDot { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "DXCodeType")]
        [Required(ErrorMessageResourceName = "DXCodeTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string DxCodeType { get; set; }

        public string Description { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "StartDate")]
        [Required(ErrorMessageResourceName = "StartDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime EffectiveFrom { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "EndDate")]
        [Required(ErrorMessageResourceName = "EndDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime EffectiveTo { get; set; }

        public bool IsDeleted { get; set; }

        [ResultColumn]
        public string DxCodeShortName { get; set; }
    }

   

}
