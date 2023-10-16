using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Zarephath.Core.Controllers;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Infrastructure.Utility;
using Zarephath.Core.Infrastructure.Utility.CareGiverApi;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;


using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;
using net.authorize.sample;
using System.IO;
using System.Reflection;
using AuthorizeNet;
using net.authorize.sample.PaymentTransactions;
using System.Threading;
using net.authorize.sample.CustomerProfiles;
using net.authorize.sample.MobileInappTransactions;



namespace Zarephath.Core.Areas.HomeCare.Controllers
{

    public class UserPaymentDetailController : BaseController
    {
        private IInvoiceDataProvider _InvoiceDataProvider;
        private IBillingInformationDataProvider _IBillingInformationProvider;
        [HttpGet]
        public ActionResult AddPaymentDetail(string id,string id1)
        {
            Session["InvoiceID"] = id;
            Session["InvoiceAmount"] = id1;
            return View();
        }


        [HttpPost]
        // [CustomAuthorize(Permissions = Constants.Permission_Employee_AddUpdate)]
        public JsonResult AddPaymentDetail(UserPaymentDetailModel piUserPaymentDetailModel)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                ProcessPayment(piUserPaymentDetailModel);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
            }

            return Json(response.IsSuccess);
        }

      
        private void ProcessPayment(UserPaymentDetailModel piUserPaymentDetailModel)
        {
            string OrganizationId = Convert.ToString(SessionHelper.OrganizationId);
            string EmailID = Convert.ToString(SessionHelper.Email);//"ankursharma981173@gmail.com";// Convert.ToString(SessionHelper.Email);
            string apiLoginId = System.Configuration.ConfigurationManager.AppSettings["AuthorizedNetapiLoginId"].ToString();
            string transactionKey = System.Configuration.ConfigurationManager.AppSettings["AuthorizedNettransactionKey"].ToString();
            string customerProfileId = string.Empty;
            string customerPaymentProfileId = string.Empty;
            string customerShippingAddressId = string.Empty;
         

            decimal invoiceAmounttemp = Convert.ToDecimal(Session["InvoiceAmount"].ToString());
            decimal amount = Math.Round(invoiceAmounttemp, 2);


            string[] inputarrayDetail = new string[13];

            inputarrayDetail[0] = piUserPaymentDetailModel.CardNumber;
            inputarrayDetail[1] = piUserPaymentDetailModel.ExpirationDate;
            inputarrayDetail[2] = piUserPaymentDetailModel.AccountNumber;
            inputarrayDetail[3] = piUserPaymentDetailModel.RoutingNumber;
            inputarrayDetail[4] = piUserPaymentDetailModel.NameOnAccount;
            inputarrayDetail[5] = piUserPaymentDetailModel.BankName;

            inputarrayDetail[6] = piUserPaymentDetailModel.OfficeAddress;
            inputarrayDetail[7] = piUserPaymentDetailModel.OfficeAddressCity;
            inputarrayDetail[8] = piUserPaymentDetailModel.OfficeAddressZip;
            inputarrayDetail[9] = piUserPaymentDetailModel.HomeAddress;
            inputarrayDetail[10] = piUserPaymentDetailModel.HomeAddressCity;
            inputarrayDetail[11] = piUserPaymentDetailModel.OfficeAddressZip;
            inputarrayDetail[12] = OrganizationId;//"Test CustomerID"; //OrganizationId;//"Test CustomerID";


            string[] responseAuthorizeCreditCard = AuthorizeCreditCard.Run(apiLoginId, transactionKey, amount, inputarrayDetail);

            if (responseAuthorizeCreditCard[0].ToLower() == "Failed Transaction")
            {

                string errorcodeprofile = responseAuthorizeCreditCard[7].ToString();
                string errortextprofile = responseAuthorizeCreditCard[8].ToString();

                BillingInformationModel objBillingInformationModel = new BillingInformationModel();
                objBillingInformationModel.OrganizationId = Convert.ToInt64(OrganizationId);
                objBillingInformationModel.CardNumber = inputarrayDetail[0].Substring(12, 4);
                string creditcardDate = inputarrayDetail[1].Substring(0, 2) + "/01/20" + inputarrayDetail[1].Substring(2, 2);
                objBillingInformationModel.ExpirationDate = Convert.ToDateTime(creditcardDate);
                objBillingInformationModel.AccountNumber = "NA";
                objBillingInformationModel.RoutingNumber = "NA";
                objBillingInformationModel.NameOnAccount = "NA";
                objBillingInformationModel.customerProfileId = 0;
                objBillingInformationModel.customerPaymentProfileId = 0;
                objBillingInformationModel.customerShippingAddressId = 0;
                objBillingInformationModel.Statuscode = "Authorize CreditCard Failed";
                objBillingInformationModel.ErrorCode = errorcodeprofile;
                objBillingInformationModel.ErrorText = errortextprofile;

                _IBillingInformationProvider = new BillingInformationDataProvider();
                _IBillingInformationProvider.AddBillingDetail(objBillingInformationModel);

                return;
            }






            string[] responseProfileCreation = CreateCustomerProfile.Run(apiLoginId, transactionKey, EmailID, inputarrayDetail);

            if (responseProfileCreation != null)
            {
                if (responseProfileCreation[0].ToLower() == "success")
                {
                    customerProfileId = responseProfileCreation[1];
                    customerPaymentProfileId = responseProfileCreation[2];
                    customerShippingAddressId = responseProfileCreation[3];


                    BillingInformationModel objBillingInformationModel = new BillingInformationModel();
                    objBillingInformationModel.OrganizationId = Convert.ToInt64(OrganizationId);
                    objBillingInformationModel.CardNumber = inputarrayDetail[0].Substring(12, 4);
                    string creditcardDate = inputarrayDetail[1].Substring(0, 2) + "/01/20" + inputarrayDetail[1].Substring(2, 2);
                    objBillingInformationModel.ExpirationDate = Convert.ToDateTime(creditcardDate);
                    objBillingInformationModel.AccountNumber = "NA";
                    objBillingInformationModel.RoutingNumber = "NA";
                    objBillingInformationModel.NameOnAccount = "NA";
                    objBillingInformationModel.customerProfileId = Convert.ToInt64(customerProfileId);
                    objBillingInformationModel.customerPaymentProfileId = Convert.ToInt64(customerPaymentProfileId);
                    objBillingInformationModel.customerShippingAddressId = Convert.ToInt64(customerShippingAddressId);
                    objBillingInformationModel.Statuscode = "success";
                    objBillingInformationModel.ErrorCode = "NA";
                    objBillingInformationModel.ErrorText = "NA";
                    _IBillingInformationProvider = new BillingInformationDataProvider();
                    _IBillingInformationProvider.AddBillingDetail(objBillingInformationModel);


                }
                else if (responseProfileCreation[0].ToLower() == "Customer Profile Creation Failed")
                {

                    string errorcodeprofile = responseProfileCreation[7].ToString();
                    string errortextprofile = responseProfileCreation[8].ToString();

                    BillingInformationModel objBillingInformationModel = new BillingInformationModel();
                    objBillingInformationModel.OrganizationId = Convert.ToInt64(OrganizationId);
                    objBillingInformationModel.CardNumber = inputarrayDetail[0].Substring(12, 4);
                    string creditcardDate = inputarrayDetail[1].Substring(0, 2) + "/01/20" + inputarrayDetail[1].Substring(2, 2);
                    objBillingInformationModel.ExpirationDate = Convert.ToDateTime(creditcardDate);
                    objBillingInformationModel.AccountNumber = "NA";
                    objBillingInformationModel.RoutingNumber = "NA";
                    objBillingInformationModel.NameOnAccount = "NA";
                    objBillingInformationModel.customerProfileId = 0;
                    objBillingInformationModel.customerPaymentProfileId = 0;
                    objBillingInformationModel.customerShippingAddressId = 0;
                    objBillingInformationModel.Statuscode = "Customer Profile Creation Failed"; 
                    objBillingInformationModel.ErrorCode = errorcodeprofile;
                    objBillingInformationModel.ErrorText = errortextprofile;

                    _IBillingInformationProvider = new BillingInformationDataProvider();
                    _IBillingInformationProvider.AddBillingDetail(objBillingInformationModel);

                    return;
                }
            }

            string[] responsePaymentDone = ChargeCustomerProfile.Run(apiLoginId, transactionKey, customerProfileId, customerPaymentProfileId, amount);
            string transId = string.Empty;
            string responseCode = string.Empty;
            string code = string.Empty;
            string description = string.Empty;
            string authCode = string.Empty;
            // string statuscode= string.Empty;
            string errorcode = string.Empty;
            string errortext = string.Empty;
            if (responsePaymentDone != null)
            {
                if (responsePaymentDone[0].ToLower() == "successfully created transaction")
                {
                    transId = responsePaymentDone[1];
                    responseCode = responsePaymentDone[2];
                    code = responsePaymentDone[3];
                    description = responsePaymentDone[4];
                    authCode = responsePaymentDone[5];



                    InvoiceViewModel objInvoiceViewModel = new InvoiceViewModel();
                    objInvoiceViewModel.InvoiceNumber = Session["InvoiceID"].ToString();//invoiceId;
                    objInvoiceViewModel.TransactionIdAuthNet = transId;
                    objInvoiceViewModel.ResponseCodeAuthNet = responseCode;
                    objInvoiceViewModel.MessageCodeAuthNet = code;
                    objInvoiceViewModel.DescriptionAuthNet = description;
                    objInvoiceViewModel.AuthCodeAuthNet = authCode;
                    objInvoiceViewModel.OrganizationId = OrganizationId;
                    objInvoiceViewModel.Statuscode = "successfully created transaction";
                    objInvoiceViewModel.ErrorCode = "NA";
                    objInvoiceViewModel.ErrorText = "NA";
                    IInvoiceDataProvider _InvoiceDataProvider;
                    _InvoiceDataProvider = new InvoiceDataProvider();

                    ServiceResponse responseUpdateInvoice = _InvoiceDataProvider.UpdateInvoiceByInvoiceNumberWithPaymentStatus(objInvoiceViewModel);

                    ServiceResponse responseNinjaService = _InvoiceDataProvider.UpdateInvoiceStatus(Session["InvoiceID"].ToString(), transId, Session["InvoiceAmount"].ToString(), Session["client_id"].ToString());

                }
                else if (responsePaymentDone[6].ToLower() == "failed transaction")
                {
                    errorcode = responsePaymentDone[7].ToString();
                    errortext = responsePaymentDone[8].ToString();
                    InvoiceViewModel objInvoiceViewModel = new InvoiceViewModel();
                    objInvoiceViewModel.InvoiceNumber = Session["InvoiceID"].ToString(); 
                    //objInvoiceViewModel.TransactionIdAuthNet = transId;
                    //objInvoiceViewModel.ResponseCodeAuthNet = responseCode;
                    //objInvoiceViewModel.MessageCodeAuthNet = code;
                    //objInvoiceViewModel.DescriptionAuthNet = description;
                    //objInvoiceViewModel.AuthCodeAuthNet = authCode;
                    //objInvoiceViewModel.OrganizationId = OrganizationId;
                    objInvoiceViewModel.Statuscode = "failed transaction";
                    objInvoiceViewModel.ErrorCode = errorcode;
                    objInvoiceViewModel.ErrorText = errortext;
                    objInvoiceViewModel.OrganizationId = OrganizationId;
                    IInvoiceDataProvider _InvoiceDataProvider;
                    _InvoiceDataProvider = new InvoiceDataProvider();

                    ServiceResponse responseUpdateInvoice = _InvoiceDataProvider.UpdateInvoiceByInvoiceNumberWithPaymentStatus(objInvoiceViewModel);
                } //else if()
            }

           
        }
    }
}
