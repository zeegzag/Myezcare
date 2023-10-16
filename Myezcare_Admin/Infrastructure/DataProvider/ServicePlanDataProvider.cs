using System;
using System.Collections.Generic;
using System.Linq;
using PetaPoco;
using Myezcare_Admin.Models;
using Myezcare_Admin.Models.Entity;
using Myezcare_Admin.Models.ViewModel;
using Myezcare_Admin.Resources;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;


namespace Myezcare_Admin.Infrastructure.DataProvider
{
    public class ServicePlanDataProvider : BaseDataProvider, IServicePlanDataProvider
    {
        public ServiceResponse SetAddServicePlanPage(long servicePlanId)
        {
            var response = new ServiceResponse();
            try
            {
                SetAddServicePlanModel setAddServicePlanModel = new SetAddServicePlanModel();
                List<SearchValueData> searchList = new List<SearchValueData>
                    {
                        new SearchValueData { Name = "ServicePlanID", Value = Convert.ToString(servicePlanId),IsEqual=true }
                    };

                ServicePlanDetails servicePlanDetails = GetMultipleEntity<ServicePlanDetails>(StoredProcedure.GetServicePlanDetails, searchList);

                setAddServicePlanModel.ServicePlan = servicePlanDetails.ServicePlan == null ? new ServicePlan() : servicePlanDetails.ServicePlan;
                setAddServicePlanModel.ServicePlanComponentList = servicePlanDetails.ServicePlanComponentList;

                if (setAddServicePlanModel.ServicePlan.ServicePlanID > 0)
                {
                    setAddServicePlanModel.ServicePlanModules = servicePlanDetails.ServicePlanModules;

                    foreach (var item in setAddServicePlanModel.ServicePlanModules)
                    {
                        if (item.ModuleID == (int) Common.ServicePlanModuleEnum.Patient)
                        {
                            item.ModuleHelpText = Resource.SPPatientHelpText;
                            item.ModuleDisplayName = Resource.HashPatients;
                        }
                        else if (item.ModuleID == (int) Common.ServicePlanModuleEnum.Facility)
                        {
                            item.ModuleHelpText = Resource.SPFacilityHelpText;
                            item.ModuleDisplayName = Resource.HashFacilities;
                        }
                        else if (item.ModuleID == (int) Common.ServicePlanModuleEnum.Task)
                        {
                            item.ModuleHelpText = Resource.SPTasksHelpText;
                            item.ModuleDisplayName = Resource.HashTasks;
                        }
                        else if (item.ModuleID == (int) Common.ServicePlanModuleEnum.Employee)
                        {
                            item.ModuleHelpText = Resource.SPEmployeeHelpText;
                            item.ModuleDisplayName = Resource.HashEmployees;
                        }
                        else if (item.ModuleID == (int) Common.ServicePlanModuleEnum.Billing)
                        {
                            item.ModuleHelpText = Resource.SPBillingHelpText;
                            item.ModuleDisplayName = Resource.PercentBilling;
                        }
                    }

                }
                else
                {
                    var moduleList = Enum.GetValues(typeof(Common.ServicePlanModuleEnum))
                           .Cast<Common.ServicePlanModuleEnum>()
                          .Select(t => new ServicePlanModule
                          {
                              ModuleID = ((int)t),
                              ModuleName = t.ToString(),//Common.GetEnumDisplayValue(t),
                              ModuleDisplayName = Common.GetEnumDisplayValue(t),
                              ModuleHelpText = Common.GetEnumHelpTextValue(t),
                              ModuleRequiredText = Common.GetEnumRequiredTextValue(t) 
                          }).ToList();
                    setAddServicePlanModel.ServicePlanModules = moduleList;
                }

                setAddServicePlanModel.SetRolePermissionModel = GetMultipleEntity<SetRolePermissionModel>(StoredProcedure.SetRolePermissionPage, searchList);

                response.Data = setAddServicePlanModel;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }
            return response;
        }

        public ServiceResponse AddServicePlan(ServicePlanModel servicePlan, long loggedInUserId)
        {
            string systemId = System.Web.HttpContext.Current.Request.UserHostAddress;
            var response = new ServiceResponse();
            try
            {
                bool editMode = servicePlan.ServicePlan.ServicePlanID > 0;

                DataTable dataTbl = Common.ListToDataTable(servicePlan.ServicePlanModules);

                string conStr = ConfigurationManager.ConnectionStrings[Constants.MyezcareOrganizationConnectionString].ConnectionString;
                SqlConnection con = new SqlConnection(conStr);
                con.Open();
                SqlCommand cmd = new SqlCommand(StoredProcedure.SaveServicePlanData, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ServicePlanID", servicePlan.ServicePlan.ServicePlanID);
                cmd.Parameters.AddWithValue("@ServicePlanName", servicePlan.ServicePlan.ServicePlanName);
                cmd.Parameters.AddWithValue("@PerPatientPrice", servicePlan.ServicePlan.PerPatientPrice);
                cmd.Parameters.AddWithValue("@SetupFees", (object)servicePlan.ServicePlan.SetupFees ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@NumberOfDaysForBilling", servicePlan.ServicePlan.NumberOfDaysForBilling);
                cmd.Parameters.AddWithValue("@ListOfPermissionIdInCsv", servicePlan.ListOfPermissionIdInCsv);
                cmd.Parameters.AddWithValue("@ListOfMobilePermissionIdInCsv", servicePlan.ListOfMobilePermissionIdInCsv);
                cmd.Parameters.AddWithValue("@SelectedComponentIds", string.Join(",", servicePlan.ServicePlanComponentList.Select(m => m.DDMasterID)));
                cmd.Parameters.AddWithValue("@UDTServicePlanModules", dataTbl);
                cmd.Parameters.AddWithValue("@LoggedInUserId", loggedInUserId);
                cmd.Parameters.AddWithValue("@SystemID", systemId);

                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable();
                da.SelectCommand = cmd;
                da.Fill(dt);

                con.Close();

                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dt.Rows[0]["TransactionResultId"]) == -1)
                    {
                        response.IsSuccess = false;
                        response.Message = Resource.ServicePlanDuplicateErrorMessage;
                        return response;
                    }
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = Resource.ErrorOccured;
                    return response;
                }
                //List<SearchValueData> searchList = new List<SearchValueData>
                //        {
                //            new SearchValueData { Name = "ServicePlanID", Value = Convert.ToString(servicePlan.ServicePlan.ServicePlanID)},
                //            new SearchValueData { Name = "ServicePlanName", Value = servicePlan.ServicePlan.ServicePlanName},
                //            new SearchValueData { Name = "PerPatientPrice", Value = Convert.ToString(servicePlan.ServicePlan.PerPatientPrice)},
                //            new SearchValueData { Name = "NumberOfDaysForBilling", Value = Convert.ToString(servicePlan.ServicePlan.NumberOfDaysForBilling) },
                //            new SearchValueData { Name = "ListOfPermissionIdInCsv", Value = servicePlan.ListOfPermissionIdInCsv },
                //            new SearchValueData { Name = "ListOfMobilePermissionIdInCsv", Value = servicePlan.ListOfMobilePermissionIdInCsv },
                //            //new SearchValueData { Name = "UDTServicePlanModules", Value = servicePlan.ServicePlanModules, DataType = System.Data.SqlDbType.Structured },
                //            new SearchValueData { Name = "LoggedInUserId", Value = Convert.ToString(loggedInUserId) },
                //            new SearchValueData {Name = "SystemID", Value = Convert.ToString(systemId)}
                //        };

                //TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.SaveServicePlanData, searchList);
                //if(result.TransactionResultId == -1)
                //{
                //    response.IsSuccess = false;
                //    response.Message = Resource.ServicePlanDuplicateErrorMessage;
                //    return response;
                //}
                response.Message = editMode ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.ServicePlan) : string.Format(Resource.RecordCreatedSuccessfully, Resource.ServicePlan);
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                string message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
                #if DEBUG
                    message += ex.Message;
                #endif
                response.IsSuccess = false;
                response.Message = message;
            }
            return response;
        }

        public ServiceResponse SetServicePlanListPage()
        {
            var response = new ServiceResponse();
            //SetFacilityHouseListModel setEmployeeListPage = GetMultipleEntity<SetFacilityHouseListModel>(StoredProcedure.SetFacilityHouseListPage);
            SetServicePlanListModel setServicePlanListModel = new SetServicePlanListModel();
            setServicePlanListModel.SearchServicePlanModel = new SearchServicePlanModel { IsDeleted = 0 };
            setServicePlanListModel.ActiveFilter = Common.SetDeleteFilter();
            response.Data = setServicePlanListModel;
            return response;
        }

        public ServiceResponse GetServicePlanList(SearchServicePlanModel searchServicePlanModel, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();
            SetSearchFilterForServicePlanListPage(searchServicePlanModel, searchList);
            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<ServicePlanListModel> totalData = GetEntityList<ServicePlanListModel>(StoredProcedure.GetServicePlanList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ServicePlanListModel> getServicePlanList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.IsSuccess = true;
            response.Data = getServicePlanList;
            return response;
        }

        public ServiceResponse DeleteServicePlan(SearchServicePlanModel searchServicePlanModel, int pageIndex,
                                              int pageSize, string sortIndex, string sortDirection, long loggedInId)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

                SetSearchFilterForServicePlanListPage(searchServicePlanModel, searchList);

                if (!string.IsNullOrEmpty(searchServicePlanModel.ListOfIdsInCsv))
                    searchList.Add(new SearchValueData { Name = "ListOfIdsInCSV", Value = searchServicePlanModel.ListOfIdsInCsv });

                searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
                searchList.Add(new SearchValueData { Name = "LoggedInID", Value = Convert.ToString(loggedInId) });

                List<ServicePlanListModel> totalData = GetEntityList<ServicePlanListModel>(StoredProcedure.DeleteServicePlan, searchList);

                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<ServicePlanListModel> servicePlanList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);

                response.Data = servicePlanList;
                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.ServicePlan);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        private static void SetSearchFilterForServicePlanListPage(SearchServicePlanModel searchServicePlanModel, List<SearchValueData> searchList)
        {
            int perPatientPrice = 0, numberOfDaysForBilling = 0;
            int.TryParse(searchServicePlanModel.PerPatientPrice, out perPatientPrice);
            int.TryParse(searchServicePlanModel.NumberOfDaysForBilling, out numberOfDaysForBilling);

            searchList.Add(new SearchValueData { Name = "ServicePlanName", Value = searchServicePlanModel.ServicePlanName });
            searchList.Add(new SearchValueData { Name = "PerPatientPrice", Value = Convert.ToString(perPatientPrice) });
            searchList.Add(new SearchValueData { Name = "NumberOfDaysForBilling", Value = Convert.ToString(numberOfDaysForBilling) });
            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchServicePlanModel.IsDeleted) });            
        }

        public List<ServicePlanComponentModel> GetServicePlanComponent(int pageSize, string searchText = null)
        {
            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData {Name = "SearchText", Value = searchText},
                    new SearchValueData {Name = "PageSize", Value =pageSize.ToString()},
                    new SearchValueData {Name = "ItemTypeID", Value = Convert.ToString(Common.GeneralMasterEnum.ServicePlanComponents.GetHashCode())}
                };

            List<ServicePlanComponentModel> model = GetEntityList<ServicePlanComponentModel>(StoredProcedure.GetServicePlanComponent, searchParam) ?? new List<ServicePlanComponentModel>();

            //if (model.Count == 0 || model.Count(c => c.ItemTypeName == searchText) == 0)
            //    model.Insert(0, new ServicePlanComponentModel { ItemTypeName = searchText });

            return model;
        }
    }
}
