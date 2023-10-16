using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("EmployeeCertificates")]
    [PrimaryKey("CertificateID")]
    [Sort("CertificateID", "DESC")]
    public class EmployeeCertificates : BaseEntity
    {
        public long CertificateID { get; set; }

       // [Required(ErrorMessageResourceName = "AgencyTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string CertificatePath { get; set; }
        [Required(ErrorMessageResourceName = "NameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long EmployeeID { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public HttpPostedFileBase[] CFile { get; set; }
        public string CertificateAuthority { get; set; }


    }
}
