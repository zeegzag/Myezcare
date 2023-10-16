using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeCareApi.Models.ViewModel
{
    public class EmployeeIVRModel
    {
        public long EmployeeID { get; set; }
        public string Username { get; set; }
        public string IVRPin { get; set; }
    }

    public class EmployeeProfileImageModel
    {
        public long EmployeeID { get; set; }
    }

    public class EmployeeSignatureModel
    {
        public long EmployeeID { get; set; }
        public long? EmployeeSignatureID { get; set; }
    }

    public class EmployeeAgreementModel
    {
        public long EmployeeID { get; set; }
        public long? EmployeeSignatureID { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
    }
}