using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("CaseManagers")]
    [PrimaryKey("CaseManagerID")]
    [Sort("CaseManagerID", "DESC")]
    public class CaseManager : BaseEntity
    {
        public long CaseManagerID { get; set; }

        [Required(ErrorMessageResourceName = "FirstNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceName = "LastNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string LastName { get; set; }

        [Ignore]
        public string FullName {
            get { return Common.GetGenericNameFormat(FirstName,"", LastName); }
        }
        
        [RegularExpression(Constants.RegxPhone, ErrorMessageResourceName = "PhoneInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string Phone { get; set; }
        public string Extension { get; set; }

        [RegularExpression(Constants.RegxPhone, ErrorMessageResourceName = "InvalidFax", ErrorMessageResourceType = typeof(Resource))]
        public string Fax { get; set; }
        [RegularExpression(Constants.RegxPhone, ErrorMessageResourceName = "InvalidCell", ErrorMessageResourceType = typeof(Resource))]
        public string Cell { get; set; }

        [RegularExpression(Constants.RegxEmail, ErrorMessageResourceName = "EmailInvalid", ErrorMessageResourceType = typeof(Resource))]
        //[Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Email { get; set; }
        public string Notes { get; set; }

        [Required(ErrorMessageResourceName = "AgencyRequired", ErrorMessageResourceType = typeof(Resource))]
        public long AgencyID { get; set; }

        [Required(ErrorMessageResourceName = "AgencyLocationRequired", ErrorMessageResourceType = typeof(Resource))]
        public long? AgencyLocationID { get; set; }

        public bool IsDeleted { get; set; }

        public string CaseWorkerID { get; set; }
    }
}
