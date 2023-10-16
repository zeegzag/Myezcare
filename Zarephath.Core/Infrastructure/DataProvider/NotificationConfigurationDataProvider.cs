using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using PetaPoco;
using Zarephath.Core.Helpers;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;
using System.IO;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using ExportToExcel;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class NotificationConfigurationDataProvider : BaseDataProvider, INotificationConfigurationDataProvider
    {
        public ServiceResponse GetNotificationConfigurationDetails(NotificationConfigurationModel notificationConfiguration)
        {
            var response = new ServiceResponse();
            try
            {
                var result = new NotificationConfigurationModel();
                result.RoleID = notificationConfiguration.RoleID;
                var paramList = new List<SearchValueData>() {
                    new SearchValueData { Name = "RoleID", Value = Convert.ToString(notificationConfiguration.RoleID) }
                };
                result.NotificationConfigurationList = GetEntityList<NCItem>(StoredProcedure.GetNotificationConfigurationDetails, paramList);
                response.Data = result;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse SaveNotificationConfigurationDetails(NotificationConfigurationModel notificationConfiguration, long loggedInUserId)
        {
            var response = new ServiceResponse();
            try
            {
                var paramList = new List<SearchValueData>() {
                    new SearchValueData { Name = "RoleID", Value = Convert.ToString(notificationConfiguration.RoleID) },
                    new SearchValueData { Name = "NotificationConfigurationIDs", Value = notificationConfiguration.SelectedNotificationConfigurationIDs },
                    new SearchValueData { Name = "LoggedInUserID", Value = Convert.ToString(loggedInUserId) },
                    new SearchValueData { Name = "SystemID", Value = Common.GetMAcAddress() }
                };
                object notificationConfigurations = GetScalar(StoredProcedure.SaveNotificationConfigurationDetails, paramList);
                response.Data = notificationConfigurations;
                response.Message = string.Format(Resource.RecordProcessedSuccessfully, Resource.NotificationConfiguration);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public List<NameValueData> GetRoles()
        {
            var list = new List<NameValueData>();
            try
            {
                var temps = GetEntityList<Role>();
                foreach (var item in temps)
                { list.Add(new NameValueData() { Name = item.RoleName, Value = item.RoleID }); }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

    }
}
