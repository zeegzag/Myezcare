using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myezcare_Admin.Models.Entity
{
    [TableName("AdminTempPatient")]
    public class AdminTempPatient
    {
        public long AdminTempPatientID { get; set; }
        public string PatientID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Dob { get; set; }
        public string Gender { get; set; }
        public string LanguagePreference { get; set; }
        public string AccountNumber { get; set; }
        public string ErrorMessage { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
