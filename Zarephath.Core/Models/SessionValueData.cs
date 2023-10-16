using System.Collections.Generic;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Models
{
    public class SessionValueData
    {
        public string UserName { get; set; }
        public string EmpCredential { get; set; }
        public string FirstName { get; set; }
        public string MiddelName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public long UserID { get; set; }
        public long EmployeeID { get; set; }
        public long ReferralID { get; set; }
        public long EmployeeSignatureID { get; set; }
        public long RoleID { get; set; }
        public bool IsSecurityQuestionSubmitted { get; set; }
        public bool IsCompletedWizard { get; set; }
        public bool IsEmployeeLogin { get; set; }
        public List<PermissionIds> Permissions { get; set; }

        public string DomainName { get; set; }
        
    }
}
