using System;
using System.Collections.Generic;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.ViewModel
{
    public class DepartmentDropdownModel
    {
        public long DepartmentID { get; set; }
        public string DepartmentName { get; set; }
    }

    public class ManagerDropdownModel
    {
        public long EmployeeID { get; set; }
        public string Manager { get; set; }
    }

    /// <summary>
    /// This model is used to read the values from the database and to set the value in the Department list
    /// </summary>

    public class ListDepartmentModel
    {
        public long DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string Manager { get; set; }
        public string Location { get; set; }
        public string Address { get; set; }
        public string EncryptedDepartmentID { get { return Crypto.Encrypt(Convert.ToString(DepartmentID)); } }

        public bool IsChecked { get; set; }
        public int Count { get; set; }
        public int EmpCount { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class SearchDepartmentModel
    {
        public long DepartmentID { get; set; }
        public long EmployeeID { get; set; }
        public string Location { get; set; }
        public string Address { get; set; }
        public string ListOfIdsInCSV { get; set; }
        public bool IsShowList { get; set; }
        public int IsDeleted { get; set; }
    }

    public class SetDepartmentListPage
    {
        public SetDepartmentListPage()
        {
            DepartmentDropdownList = new List<DepartmentDropdownModel>();
            ManagerDropdownModelList = new List<ManagerDropdownModel>();
            SearchDepartmentModel = new SearchDepartmentModel();
            DeleteFilter=new List<NameValueData>();
        }
        public List<DepartmentDropdownModel> DepartmentDropdownList { get; set; }
        public List<ManagerDropdownModel> ManagerDropdownModelList { get; set; }
        
        public SearchDepartmentModel SearchDepartmentModel { get; set; }
        public List<NameValueData> DeleteFilter { get; set; } 
    }
}
