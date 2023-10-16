using System;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("AccessDeniedErrorLogs")]
    [PrimaryKey("AccessDeniedID")]
    [Sort("AccessDeniedID", "DESC")]
    public class AccessDeniedErrorLogs : BaseEntity
    {
        public long AccessDeniedID { get; set; }
        public string PermissionID { get; set; }
        public string PermissionName { get; set; }
        public string Domain { get; set; }
        public long EmployeeID { get; set;}
        public DateTime? Date { get; set; }

    }
}
