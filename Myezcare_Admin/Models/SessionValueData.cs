using Myezcare_Admin.Models.ViewModel;
using System.Collections.Generic;
namespace Myezcare_Admin.Models
{
    public class SessionValueData
    {
        public string UserName { get; set; }
        public string EmpCredential { get; set; }
        public string FirstName { get; set; }
        public string MiddelName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public long AdminID { get; set; }
        public long EmployeeSignatureID { get; set; }
        public long RoleID { get; set; }
        public bool IsSecurityQuestionSubmitted { get; set; }
        public List<PermissionIds> Permissions { get; set; }
        public string DomainName { get; set; }
        
    }
}
