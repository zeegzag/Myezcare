using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure.Utility.CareGiverApi;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class OrgPreferenceDataProvider : BaseDataProvider, IOrgPreferenceDataProvider
    {
        public ServiceResponse GetPreference(long organizationID)
        {
            var response = new ServiceResponse();

            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData>();
                searchParam.Add(new SearchValueData { Name = "OrganizationID", Value = Convert.ToString(organizationID) });
                var model = GetMultipleEntityAdmin<OrgPreferenceModel>(StoredProcedure.GetOrganizationPreference, searchParam);
                //var model = GetEntity<OrgPreferenceModel>(organizationID);

                if (model.OrganizationPreference == null)
                    model.OrganizationPreference = new OrganizationPreference();

                response.IsSuccess = true;
                response.Data = model;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, e.Message);
            }
            return response;
        }

        public ServiceResponse SavePreference(OrganizationPreference preference, long loggedInUserId)
        {
            var response = new ServiceResponse();
            try
            {

                var dataList = new List<SearchValueData>();
                dataList.Add(new SearchValueData { Name = "OrganizationID", Value = Convert.ToString(preference.OrganizationID) });
                dataList.Add(new SearchValueData { Name = "DateFormat", Value = preference.DateFormat });
                dataList.Add(new SearchValueData { Name = "Currency", Value = preference.Currency });
                dataList.Add(new SearchValueData { Name = "Region", Value = preference.Region });
                dataList.Add(new SearchValueData { Name = "Language", Value = preference.Language });
                dataList.Add(new SearchValueData { Name = "NameDisplayFormat", Value = preference.NameDisplayFormat });
                dataList.Add(new SearchValueData { Name = "CssFilePath", Value = preference.CssFilePath });
                dataList.Add(new SearchValueData { Name = "WeekStartDay", Value = preference.WeekStartDay });
                dataList.Add(new SearchValueData { Name = "loggedInUserId", Value = Convert.ToString(loggedInUserId) });
                dataList.Add(new SearchValueData { Name = "SystemID", Value = Common.GetHostAddress() });

                int data = (int)GetScalarAdmin(StoredProcedure.SaveOrganizationPreference, dataList);

                response.IsSuccess = true;
                response.Message = preference.OrganizationID > 0 ?
                    string.Format(Resource.RecordUpdatedSuccessfully, Resource.Organization_Preference) :
                    string.Format(Resource.RecordCreatedSuccessfully, Resource.Organization_Preference);
                SessionHelper.OrganizationPreference = null;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public OrganizationPreference Preferences(long organizationID)
        {
            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData>();
                searchParam.Add(new SearchValueData { Name = "OrganizationID", Value = Convert.ToString(organizationID) });
                var model = GetMultipleEntityAdmin<OrgPreferenceModel>(StoredProcedure.GetOrganizationPreference, searchParam);

                return model.OrganizationPreference;
            }
            catch { }

            return null;
        }
    }
}
