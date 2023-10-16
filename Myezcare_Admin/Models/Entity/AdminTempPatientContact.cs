using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myezcare_Admin.Models.Entity
{
    [TableName("AdminTempPatientContact")]
    public class AdminTempPatientContact
    {
        public long AdminTempPatientContactID { get; set; }
        public string PatientID { get; set; }
        public string ContactType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string LanguagePreference { get; set; }
        public string IsEmergencyContact { get; set; }
        public string ErrorMessage { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
