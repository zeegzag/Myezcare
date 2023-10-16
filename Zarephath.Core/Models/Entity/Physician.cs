using PetaPoco;
using System.ComponentModel.DataAnnotations;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("Physicians")]
    [PrimaryKey("PhysicianID")]
    [Sort("PhysicianID", "DESC")]
    public class Physician : BaseEntity
    {
        public long PhysicianID { get; set; }

        [Required(ErrorMessageResourceName = "FirstNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required(ErrorMessageResourceName = "LastNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string LastName { get; set; }

        //[Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxEmail, ErrorMessageResourceName = "EmailInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string Email { get; set; }

        [RegularExpression(Constants.RegxPhone, ErrorMessageResourceName = "InvalidPhoneNumber", ErrorMessageResourceType = typeof(Resource))]
        public string Phone { get; set; }

        [RegularExpression(Constants.RegxPhone, ErrorMessageResourceName = "InvalidMobileNumber", ErrorMessageResourceType = typeof(Resource))]
        public string Mobile { get; set; }

        //[Required(ErrorMessageResourceName = "NPINumberRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(10, ErrorMessageResourceName = "NPILength", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxNPI, ErrorMessageResourceName = "NPIInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string NPINumber { get; set; }

        [Required(ErrorMessageResourceName = "AddressRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Address { get; set; }

        [Required(ErrorMessageResourceName = "CityRequired", ErrorMessageResourceType = typeof(Resource))]
        public string City { get; set; }

        [Required(ErrorMessageResourceName = "StateRequired", ErrorMessageResourceType = typeof(Resource))]
        public string StateCode { get; set; }

        [Required(ErrorMessageResourceName = "ZipCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string ZipCode { get; set; }

        public bool IsDeleted { get; set; }
        public string PhysicianTypeID { get; set; }
        public string PhysicianTypeName { get; set; }
    }
    public class Physicians
    {
        public long PhysicianID { get; set; }
        public string FirstName { get; set; }

        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string NPINumber { get; set; }
        
        public string Address { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string ZipCode { get; set; }
        public bool IsDeleted { get; set; }
        public string PhysicianTypeID { get; set; }
        public string PhysicianTypeName { get; set; }
    }
}

