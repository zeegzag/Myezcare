using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure.Utility.CareGiverApi;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;
using Elmah;
namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class BillingInformationDataProvider : BaseDataProvider, IBillingInformationDataProvider
    {

        public BillingInformationDataProvider(string conString)
            : base(conString)
        {
        }

        public BillingInformationDataProvider() { }


        public ServiceResponse AddBillingDetail(BillingInformationModel addBillingInformationModel)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {

                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "OrganizationId", Value = Convert.ToString(addBillingInformationModel.OrganizationId)},
                    new SearchValueData {Name = "CardNumber", Value = Convert.ToString(addBillingInformationModel.CardNumber)},
                    new SearchValueData {Name = "ExpirationDate", Value = Convert.ToString(addBillingInformationModel.ExpirationDate)},
                    new SearchValueData {Name = "AccountNumber", Value = Convert.ToString(addBillingInformationModel.AccountNumber)},
                    new SearchValueData {Name = "RoutingNumber", Value = Convert.ToString(addBillingInformationModel.RoutingNumber)},
                    new SearchValueData {Name = "NameOnAccount", Value = Convert.ToString(addBillingInformationModel.NameOnAccount)},
                    new SearchValueData {Name = "BankName", Value = Convert.ToString(addBillingInformationModel.BankName)},
                    new SearchValueData {Name = "customerProfileId", Value = Convert.ToString(addBillingInformationModel.customerProfileId)},
                    new SearchValueData {Name = "customerPaymentProfileId", Value = Convert.ToString(addBillingInformationModel.customerPaymentProfileId)},
                    new SearchValueData {Name = "customerShippingAddressId", Value = Convert.ToString(addBillingInformationModel.customerShippingAddressId)},
                    new SearchValueData {Name = "Statuscode", Value = Convert.ToString(addBillingInformationModel.Statuscode)},
                    new SearchValueData {Name = "ErrorCode", Value = Convert.ToString(addBillingInformationModel.ErrorCode)},
                    new SearchValueData {Name = "ErrorText", Value = Convert.ToString(addBillingInformationModel.ErrorText)},


                };

                GetScalarAdmin(StoredProcedure.SaveBillingAuthorizeNetDetail, searchlist);
                response.IsSuccess = true;
                response.Message = "Saved Successfully";
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return response;
        }

        public ServiceResponse GetBillingDetail(BillingInformationModel addBillingInformationModel)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {

                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "OrganizationId", Value = Convert.ToString(addBillingInformationModel.OrganizationId)},

                  };
                List<BillingInformationModel> BillingData = GetEntityListAdmin<BillingInformationModel>(StoredProcedure.GetBillingAuthorizeNetDetail, searchlist);
                response.Data = BillingData;
                response.IsSuccess = true;
                response.Message = "Saved Successfully";
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return response;
        }



    }
}
