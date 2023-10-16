using System;
using System.Collections.Generic;
using Zarephath.Core.Models;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class OnboardinDataProvider : BaseDataProvider, IOnboardinDataProvider
    {
        public OnboardinDataProvider()
        { }
        public OnboardinDataProvider(string conString)
            : base(conString)
        {
        }
        public ServiceResponse SetWizardStatus(long orgId, string menu, bool isCompleted, long loggedInUserId)
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            try
            {
                searchList.Add(new SearchValueData("Menu", menu));
                searchList.Add(new SearchValueData("OrganizationID", Convert.ToString(orgId)));
                searchList.Add(new SearchValueData("IsCompleted", Convert.ToString(isCompleted)));
                searchList.Add(new SearchValueData("loggedInUserId", Convert.ToString(loggedInUserId)));
                searchList.Add(new SearchValueData("SystemID", Common.GetHostAddress()));

                int data = (int)GetScalar(StoredProcedure.SetWizardStatus, searchList);
                if (data == -1)
                {
                    response.IsSuccess = false;
                    return response;
                }
                response.IsSuccess = true;
                response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.OnboardingInformation);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public List<OnboardingViewModel> GetWizardStatus(long orgId)
        {
            List<OnboardingViewModel> data = new List<OnboardingViewModel>();
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            try
            {
                searchList.Add(new SearchValueData("OrganizationID", Convert.ToString(orgId)));
                data = GetEntityList<OnboardingViewModel>(StoredProcedure.GetWizardStatus, searchList);
            }
            catch (Exception ex)
            {
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return data;
        }
    }
}
