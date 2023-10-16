using HomeCareApi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeCareApi.Models.ViewModel
{
    public class ValidateIVRCodeModel
    {
        public ValidateIVRCodeModel()
        {
            Employee = new Employee();
        }
        public long PermissionID { get; set; }
        public Employee Employee { get; set; }
    }

    public enum BypassActions
    {
        All = 0,
        Pending = 1,
        Approved,
        Rejected
    }

}