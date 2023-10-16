using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("Agencies")]
    [PrimaryKey("AgencyID")]
    [Sort("AgencyID", "DESC")]
    public class Agency : BaseEntity
    {
        public long AgencyID { get; set; }

        [Required(ErrorMessageResourceName = "AgencyTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string AgencyType { get; set; }

        //[Required]
        [RegularExpression(Constants.RegxNumericTenDigit, ErrorMessageResourceName = "OnlyNumberTenDigit", ErrorMessageResourceType = typeof(Resource))]
        public string NPI { get; set; }

        //[Required(ErrorMessageResourceName = "AgencyFirstNameRequired", ErrorMessageResourceType = typeof(Resource))]
        //public string FirstName { get; set; }
        
        //public string MiddleName { get; set; }

        //[Required(ErrorMessageResourceName = "AgencyLastNameRequired", ErrorMessageResourceType = typeof(Resource))]
        //public string LastName { get; set; }

        [Required(ErrorMessageResourceName = "AgencyNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string NickName { get; set; }

        //[Required(ErrorMessageResourceName = "AgencyShortNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string ShortName { get; set; }

        //[Required(ErrorMessageResourceName = "RegionRequired", ErrorMessageResourceType = typeof(Resource))]
        public long RegionID { get; set; }

        [RegularExpression(Constants.RegxEmail, ErrorMessageResourceName = "EmailInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string Email { get; set; }

        public string ContactName { get; set; }

        public string TIN { get; set; }
        public string EIN { get; set; }

        [RegularExpression(Constants.RegxPhone, ErrorMessageResourceName = "InvalidCell", ErrorMessageResourceType = typeof(Resource))]
        public string Phone { get; set; }

        [RegularExpression(Constants.RegxPhone, ErrorMessageResourceName = "InvalidCell", ErrorMessageResourceType = typeof(Resource))]
        public string Mobile { get; set; }

        [RegularExpression(Constants.RegxPhone, ErrorMessageResourceName = "InvalidFax", ErrorMessageResourceType = typeof(Resource))]
        public string Fax { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string ZipCode { get; set; }
        public bool IsDeleted { get; set; }

    }
}
