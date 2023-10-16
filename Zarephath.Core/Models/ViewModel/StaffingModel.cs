using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ExpressiveAnnotations.Attributes;
using Newtonsoft.Json;
using PetaPoco;
using Zarephath.Core.Controllers;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Resources;
using System.Linq;
using System.Web;

namespace Zarephath.Core.Models.ViewModel
{


    #region Add Facility
    public class SetContactListPage
    {
        public SetContactListPage()
        {
            //EmployeeGroupList = new List<EmployeeGroupList>();
            EmployeeGroup = new EmployeeGroupList();
            EmployeeList = new List<EmployeeList>();
            EmployeeListAssignedGroup = new List<EmployeeListAssignedGroup>();
          
        }

        // public List<EmployeeGroupList> EmployeeGroupList { get; set; }
        public EmployeeGroupList EmployeeGroup { get; set; }
        public List<EmployeeList> EmployeeList { get; set; }
        public List<EmployeeListAssignedGroup> EmployeeListAssignedGroup { get; set; }
      
       
    }

    public class EmployeeGroupList
    {
        public string Value { get; set; }
        public string Name { get; set; }
    }
    public class EmployeeList
    {
        public long EmployeeID { get; set; }
        public string EmployeeUniqueID { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string GroupNames { get; set; }
        public bool IsChecked { get; set; }


    }
    public class EmployeeListAssignedGroup
    {
        public long EmployeeID { get; set; }
        public string EmployeeUniqueID { get; set; }
        public string EmployeeName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string GroupIDs { get; set; }
       
    }
   
    #endregion Add Referral














}
