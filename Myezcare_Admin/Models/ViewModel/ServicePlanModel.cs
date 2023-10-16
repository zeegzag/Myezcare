using Myezcare_Admin.Infrastructure;
using Myezcare_Admin.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Myezcare_Admin.Models.ViewModel
{
    public class ServicePlanModel
    {
        public ServicePlanModel()
        {
            ServicePlan = new ServicePlan();
            ServicePlanModules = new List<ServicePlanModule>();
            ServicePlanComponentList = new List<ServicePlanComponentModel>();
        }
        public ServicePlan ServicePlan { get; set; }
        public string ListOfPermissionIdInCsv { get; set; }
        public string ListOfMobilePermissionIdInCsv { get; set; }
        public List<ServicePlanModule> ServicePlanModules { get; set; }
        public List<ServicePlanComponentModel> ServicePlanComponentList { get; set; }
    }
    
    public class SetAddServicePlanModel
    {
        public SetAddServicePlanModel()
        {
            ServicePlan = new ServicePlan();
            SetRolePermissionModel = new SetRolePermissionModel();
            ServicePlanModules = new List<ServicePlanModule>();
            ServicePlanComponentList = new List<ServicePlanComponentModel>();
        }
        public ServicePlan ServicePlan { get; set; }
        public SetRolePermissionModel SetRolePermissionModel { get; set; }
        public List<ServicePlanModule> ServicePlanModules { get; set; }
        public List<ServicePlanComponentModel> ServicePlanComponentList { get; set; }
    }

    public class ServicePlanModule
    {
        public int ModuleID { get; set; }
        public string ModuleName { get; set; }
        public string ModuleDisplayName { get; set; }
        public string ModuleHelpText { get; set; }
        public int? MaximumAllowedNumber { get; set; }
        public string ModuleRequiredText { get; set; }
    }

    public class ServicePlanDetails
    {
        public ServicePlanDetails()
        {
            ServicePlan = new ServicePlan();
            ServicePlanModules = new List<ServicePlanModule>();
            ServicePlanComponentList = new List<ServicePlanComponentModel>();
        }
        public ServicePlan ServicePlan { get; set; }
        public List<ServicePlanModule> ServicePlanModules { get; set; }
        public List<ServicePlanComponentModel> ServicePlanComponentList { get; set; }
    }

    public class ServicePlanListModel
    {
        public long ServicePlanID { get; set; }
        public string ServicePlanName { get; set; }
        public float PerPatientPrice { get; set; }
        public double? SetupFees { get; set; }
        public int NumberOfDaysForBilling { get; set; }
        public int Patient { get; set; }
        public int Facility { get; set; }
        public int Task { get; set; }
        public int Employee { get; set; }
        public int? Billing { get; set; }
        public bool IsDeleted { get; set; }
        public int Row { get; set; }
        public int Count { get; set; }
        public string EncryptedServicePlanID { get { return ServicePlanID > 0 ? Crypto.Encrypt(Convert.ToString(ServicePlanID)) : ""; } }
    }

    public class SearchServicePlanModel
    {
        public string ServicePlanName { get; set; }
        public string SetupFees { get; set; }
        public string PerPatientPrice { get; set; }
        public string NumberOfDaysForBilling { get; set; }
        public long IsDeleted { get; set; }
        public string ListOfIdsInCsv { get; set; }
    }

    public class SetServicePlanListModel
    {
        public SetServicePlanListModel()
        {
            SearchServicePlanModel = new SearchServicePlanModel();
            ActiveFilter = new List<NameValueData>();
        }
        public SearchServicePlanModel SearchServicePlanModel { get; set; }
        public List<NameValueData> ActiveFilter { get; set; }
    }

    public class ServicePlanComponentModel
    {
        public long ServicePlanComponentID { get; set; }
        public long ServicePlanID { get; set; }
        public long DDMasterID { get; set; }
        public string Title { get; set; }
    }

}