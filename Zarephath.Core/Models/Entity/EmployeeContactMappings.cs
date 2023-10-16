using System;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    
    [TableName("EmployeeContactMappings")]
    [PrimaryKey("ContactMappingID")]
    [Sort("ContactMappingID", "DESC")]
    public class EmployeeContactMappings : BaseEntity
    {
        [IgnoreInAudit]
        public long ContactMappingID { get; set; }
        [IgnoreInAudit]
        public long EmployeeID { get; set; }
        [IgnoreInAudit]
        public long ClientID { get; set; }


        [GetTableNameAttribute("ContactTypes")]
        [Display(ResourceType = typeof(Resource), Name = "ContactType")]
        [Required(ErrorMessageResourceName = "ContactTypeIDRequired", ErrorMessageResourceType = typeof(Resource))]
        public long ContactTypeID { get; set; }

        [GetTableNameAttribute("Contacts")]
        [Display(ResourceType = typeof(Resource), Name = "ContactDetails")]
        public long ContactID { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IsEmergencyContact")]
        public bool IsEmergencyContact { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IsPrimaryContactLegalGuardian")]
        public bool IsPrimaryPlacementLegalGuardian { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IsDCSLegalGuardian")]
        public bool IsDCSLegalGuardian { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IsNoticeProviderOnFile")]
        public bool IsNoticeProviderOnFile { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ROIExpireDate")]
        public DateTime? ROIExpireDate { get; set; }


        [GetTableNameAttribute("ROITypes")]
        [Display(ResourceType = typeof(Resource), Name = "ROIType")]
        public int ROIType { get; set; }

        public string Relation { get; set; }

        [Ignore]
        public bool IsPrimaryContactValid { get; set; }

        [Ignore]
        public bool IsLegalContactValid { get; set; }

        //[Ignore]
        //[Required(ErrorMessageResourceName = "ContactTypeIDRequired", ErrorMessageResourceType = typeof(Resource))]
        //public string ContactValidationMessage { get; set; }

    }
}
