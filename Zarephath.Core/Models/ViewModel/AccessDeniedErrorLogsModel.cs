using System;
using System.Collections.Generic;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models.Entity;

namespace Zarephath.Core.Models.ViewModel
{
    public class AccessDeniedErrorLogsModel
    {
        public long AccessDeniedID { get; set; }
        public string PermissionID { get; set; }
        public string PermissionName { get; set; }
        public string Domain { get; set; }
        public long EmployeeID { get; set; }
        public DateTime? Date { get; set; }
    }

}
