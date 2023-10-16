using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Myezcare_Admin.Models.Entity;
using Myezcare_Admin.Resources;

namespace Myezcare_Admin.Models.ViewModel
{
    public class SetRolePermissionModel
    {
        public SetRolePermissionModel()
        {
            OrganizationList = new List<MyEzcareOrganization>();
            PermissionList = new List<PermissionTreeModel>();
            MobilePermissionList = new List<PermissionTreeModel>();
            RoleWisePermissionList = new List<RolePermissionModel>();
            SearchRolePermissionModel = new SearchRolePermissionModel();
        }
        public List<MyEzcareOrganization> OrganizationList { get; set; }
        public List<PermissionTreeModel> PermissionList { get; set; }
        public List<PermissionTreeModel> MobilePermissionList { get; set; }
        public List<RolePermissionModel> RoleWisePermissionList { get; set; }
        public SearchRolePermissionModel SearchRolePermissionModel { get; set; }
    }

    public class RolePermissionModel
    {
        public long RolePermissionMappingID { get; set; }
        public long RoleID { get; set; }
        public long PermissionID { get; set; }
        public string RoleName { get; set; }
        public string PermissionName { get; set; }
        public long OrganizationID { get; set; }
        public string CompanyName { get; set; }
    }

    public class PermissionTreeModel
    {
        public string id { get; set; }
        public string text { get; set; }
        public string parent { get; set; }
        public string Description { get; set; }
        public long OrganizationID { get; set; }
        public string CompanyName { get; set; }
    }

    public class SearchRolePermissionModel
    {
        public long RoleID { get; set; }
        public long OrganizationID { get; set; }
        public string CompanyName { get; set; }

        public string RoleName { get; set; }
        public long PermissionID { get; set; }
        public string ListOfPermissionIdInCsv { get; set; }
        public string ListOfMobilePermissionIdInCsv { get; set; }
        public string ListOfPermissionIdInCsvSelected { get; set; }
        public string ListOfPermissionIdInCsvNotSelected { get; set; }
        public bool IsSetToTrue { get; set; }
        public string AddNewRoleName { get; set; }
    }

    public class Getlist
    {
        public Getlist()
        {
            Roles =new List<Role>();
        }
        public List<Role> Roles { get; set; }
    }
}
