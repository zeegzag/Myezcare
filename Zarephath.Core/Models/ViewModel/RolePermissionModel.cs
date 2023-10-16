using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.ViewModel
{
    public class SetRolePermissionModel
    {
        public SetRolePermissionModel()
        {
            RoleList = new List<Role>();
            PermissionList = new List<PermissionTreeModel>();
            MobilePermissionList=new List<PermissionTreeModel>();
            RoleWisePermissionList = new List<RolePermissionModel>();
        }
        public List<Role> RoleList { get; set; }
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
    }

    public class PermissionTreeModel
    {
        public string id { get; set; }
        public string text { get; set; }
        public string parent { get; set; }
        public string Description { get; set; }
        public string PermissionCode { get; set; }
    }

    //public class HC_SaveRoleWisePermisson
    //{
    //    public HC_SaveRoleWisePermisson()
    //    {
    //        PermissionList = new List<PermissionTreeModel>();
    //        MobilePermissionList = new List<PermissionTreeModel>();
    //    }
    //    public List<PermissionTreeModel> PermissionList { get; set; }
    //    public List<PermissionTreeModel> MobilePermissionList { get; set; }
    //}

    public class SearchRolePermissionModel
    {
        public long RoleID { get; set; }

        [Required(ErrorMessageResourceName = "RoleNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string RoleName { get; set; }
        public long PermissionID { get; set; }
        public string ListOfPermissionIdInCsv { get; set; }
        public string ListOfPermissionIdInCsvSelected { get; set; }
        public string ListOfPermissionIdInCsvNotSelected { get; set; }
        public bool IsSetToTrue { get; set; }

        // for add new role in popup
        [Display(Name = "RoleName", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RoleNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string AddNewRoleName { get; set; }
    }

    public class MapReportModel
    {
        public long RoleID { get; set; }
        public string ReportID { get; set; }
        public bool IsSetToTrue { get; set; }
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
