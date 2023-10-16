using Myezcare_Admin.Infrastructure.Attributes;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Myezcare_Admin.Resources;
using Myezcare_Admin.Infrastructure;

namespace Myezcare_Admin.Models.Entity
{
    [TableName("Organizations")]
    [PrimaryKey("OrganizationID")]
    [Sort("OrganizationID", "DESC")]
    public class MyEzcareOrganization
    {
        public long OrganizationID { get; set; }

        [Required(ErrorMessageResourceName = "DisplayNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string DisplayName { get; set; }

        [Required(ErrorMessageResourceName = "CompanyNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string CompanyName { get; set; }

        [Required(ErrorMessageResourceName = "DomainNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string DomainName { get; set; }

        [Required(ErrorMessageResourceName = "DBServerRequired", ErrorMessageResourceType = typeof(Resource))]
        public string DBServer { get; set; }
        [Required(ErrorMessageResourceName = "DBNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string DBName { get; set; }
        [Required(ErrorMessageResourceName = "DBUserNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string DBUserName { get; set; }

        [StringLength(20, MinimumLength = 8, ErrorMessageResourceName = "PasswordMaxLength", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPassword, ErrorMessageResourceName = "PasswordInvalid", ErrorMessageResourceType = typeof(Resource))]
        [DataType(DataType.Password)]
        public string DBPassword { get; set; }

        [Required(ErrorMessageResourceName = "DBProviderNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string DBProviderName { get; set; }

        [Required(ErrorMessageResourceName = "StartDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; }

        [Required(ErrorMessageResourceName = "OrganizationTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string OrganizationType { get; set; }
    }
    
    
    public enum AgencyType
    {
        HomeCare = 1,
        DayCare = 2
    }


    public enum EnumOrganizationStatus
    {
        Not_Started_Yet_New = 1,
        In_Process_Esign_Form_Created = 2,
        In_Process_Esign_Email_Sent = 3,
        In_Process_Esign_Completed = 4,
        In_Process_Site_Setup_Running = 5,
        Done = 6,
    }
}