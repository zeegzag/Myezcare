using Myezcare_Admin.Infrastructure;
using Myezcare_Admin.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Myezcare_Admin.Models.ViewModel
{
    public class PermissionsModel
    {
        public PermissionsModel()
        {
            Permissions = new Permissions();
            PermissionsModules = new List<PermissionsModule>();
        }
        public Permissions Permissions { get; set; }
        public List<PermissionsModule> PermissionsModules { get; set; }
    }
    
    public class SetAddPermissionsModel
    {
        public SetAddPermissionsModel()
        {
            Permissions = new Permissions();
            PermissionsModules = new List<PermissionsModule>();
        }
        public Permissions Permissions { get; set; }
        public List<PermissionsModule> PermissionsModules { get; set; }
    }

    public class PermissionsModule
    {
        public long PermissionID { get; set; }
        public string PermissionName { get; set; }
        public string Description { get; set; }
        public int ParentID { get; set; }
        public int OrderID { get; set; }
        public string PermissionCode { get; set; }
        public string PermissionPlatform { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class PermissionsListModule
    {
        public long PermissionID { get; set; }
        public string PermissionName { get; set; }
        public string Description { get; set; }
        public int ParentID { get; set; }
        public int OrderID { get; set; }
        public string PermissionCode { get; set; }
        public string PermissionPlatform { get; set; }
        public bool IsDeleted { get; set; }
        public int Row { get; set; }
        public int Count { get; set; }
    }

    public class SearchPermissionsModel
    {
        public long PermissionID { get; set; }
        public string PermissionName { get; set; }
        public string Description { get; set; }
        public int ParentID { get; set; }
        public int OrderID { get; set; }
        public string PermissionCode { get; set; }
        public string PermissionPlatform { get; set; }
        public long IsDeleted { get; set; }
    }

    public class SetPermissionsListModel
    {
        public SetPermissionsListModel()
        {
            SearchPermissionsModel = new SearchPermissionsModel();
            ActiveFilter = new List<NameValueData>();
        }
        public SearchPermissionsModel SearchPermissionsModel { get; set; }
        public List<NameValueData> ActiveFilter { get; set; }
    }
    public class ParentPermissionsModelList
    {
        public List<ParentPermissionsModel> parentPermissions { get; set; }
    }

    public class ParentPermissionsModel
    {
        public long PermissionID { get; set; }
        public string PermissionName { get; set; }
    }


}