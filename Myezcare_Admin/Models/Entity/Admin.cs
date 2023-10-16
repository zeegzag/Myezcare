using PetaPoco;

namespace Myezcare_Admin.Models.Entity
{
    [TableName("AdminReps")]
    [PrimaryKey("AdminID")]
    public class Admin
    {
        public long AdminID { get; set;}
        public string FirstName { get; set;}
        public string LastName { get; set;}
        public string Email { get; set; }
        public string UserName { get; set;}
        public string Password { get; set;}
        public bool IsActive { get; set; }
        public long RoleID { get; set; }
    }
}