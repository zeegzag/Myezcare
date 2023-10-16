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

using System.Net.Http;
using System.Net.Http.Headers;

namespace Zarephath.Core.Areas.HomeCare.Controllers
{
    //[CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
    //[PaymentFilter]
    public class InvoiceController : BaseController
    {
        private IInvoiceDataProvider _iInvoiceDataProvider;
        CacheHelper _cacheHelper = new CacheHelper();

        [HttpGet]
        public ActionResult InvoiceList()
        {
            _iInvoiceDataProvider = new InvoiceDataProvider();
            return View(_iInvoiceDataProvider.SetInvoiceListPage().Data);
        }

        [HttpGet]
        public ActionResult InvoiceDetail(string id)
        {
            _iInvoiceDataProvider = new InvoiceDataProvider();
            long invoiceID = !string.IsNullOrWhiteSpace(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            ServiceResponse response = _iInvoiceDataProvider.GetInvoiceDetail(invoiceID);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpGet]
        public ActionResult GenerateInvoice(string id, string EncryptedPayorID
            , bool? InvoiceAddressIsIncludePatientAddress
            , bool? InvoiceAddressIsIncludePatientAddressLine1
            , bool? InvoiceAddressIsIncludePatientAddressLine2
            , bool? InvoiceAddressIsIncludePatientAddressZip
            ,bool? InvoiceIsIncludePatientDOB)
        {
            long payorId = 0;
            if (!string.IsNullOrEmpty(EncryptedPayorID))
            {
                payorId = Convert.ToInt64(Crypto.Decrypt(EncryptedPayorID));
            }
            long invoiceID = Convert.ToInt64(id);
            _iInvoiceDataProvider = new InvoiceDataProvider();
            ServiceResponse response = _iInvoiceDataProvider.GetInvoiceDetail(invoiceID, payorId);
            ViewData["InvoiceAddressIsIncludePatientAddress"] = InvoiceAddressIsIncludePatientAddress;
            ViewData["InvoiceIsIncludePatientDOB"] = InvoiceIsIncludePatientDOB;
            ViewData["InvoiceAddressIsIncludePatientAddressLine1"] = InvoiceAddressIsIncludePatientAddressLine1;
            ViewData["InvoiceAddressIsIncludePatientAddressLine2"] = InvoiceAddressIsIncludePatientAddressLine2;
            ViewData["InvoiceAddressIsIncludePatientAddressZip"] = InvoiceAddressIsIncludePatientAddressZip;
            return View(response.Data);
        }

        [HttpGet]
        public ActionResult GenerateInvoicePdf(string id, string EncryptedPayorID
            , bool? InvoiceAddressIsIncludePatientAddress
            , bool? InvoiceAddressIsIncludePatientAddressLine1
            , bool? InvoiceAddressIsIncludePatientAddressLine2
            , bool? InvoiceAddressIsIncludePatientAddressZip,
            bool? InvoiceIsIncludePatientDOB)
        {

            long referralInvoiceID = Convert.ToInt64(Crypto.Decrypt(id));

            if (referralInvoiceID == 0)
                return null;

            string url = string.Format("{0}{1}{2}?EncryptedPayorID={3}", _cacheHelper.SiteBaseURL, Constants.HC_GenerateInvoice, referralInvoiceID, EncryptedPayorID);
            if (InvoiceAddressIsIncludePatientAddress == true)
            {
                url = url + "&InvoiceAddressIsIncludePatientAddress=true";
                if (InvoiceIsIncludePatientDOB == true)
                {
                    url = url + "&InvoiceIsIncludePatientDOB=true";
                }
                if (InvoiceAddressIsIncludePatientAddressLine1 == true)
                {
                    url = url + "&InvoiceAddressIsIncludePatientAddressLine1=true";
                }
                if (InvoiceAddressIsIncludePatientAddressLine2 == true)
                {
                    url = url + "&InvoiceAddressIsIncludePatientAddressLine2=true";
                }
                if (InvoiceAddressIsIncludePatientAddressZip == true)
                {
                    url = url + "&InvoiceAddressIsIncludePatientAddressZip=true";
                }
            }
            SelectHtmlToPdf data = new SelectHtmlToPdf();
          var FileName=  String.Format("{0}_{1}.pdf", "Invoice", DateTime.Now.ToString(Constants.ReadableFileNameDateTimeFormat));
            this.Response.AddHeader("Content-Disposition", "inline;filename=" + FileName);
            byte[] pdf = data.GenerateHtmlUrlToPdf(url);
            // return resulted pdf document
            
            return File(pdf, "application/pdf");

        }

        [HttpPost]
        public JsonResult PayInvoiceAmount(PayInvoiceAmountDetail payInvoiceAmountDetail)
        {
            _iInvoiceDataProvider = new InvoiceDataProvider();
            ServiceResponse response = _iInvoiceDataProvider.PayInvoiceAmount(payInvoiceAmountDetail, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Physician_List)]
        public ContentResult GetInvoiceList(SearchInvoiceListPage searchInvoiceListPage, int pageIndex = 1,
                                             int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iInvoiceDataProvider = new InvoiceDataProvider();
            return CustJson(_iInvoiceDataProvider.GetInvoiceList(searchInvoiceListPage, pageIndex, pageSize, sortIndex, sortDirection));
        }

        [HttpPost]
        public ContentResult GenerateInvoices(GenerateInvoicesCriteria criteria)
        {
            _iInvoiceDataProvider = new InvoiceDataProvider();
            return CustJson(_iInvoiceDataProvider.GenerateInvoices(criteria));
        }

        [HttpPost]
        public ContentResult DeleteInvoices(DeleteInvoices criteria)
        {
            _iInvoiceDataProvider = new InvoiceDataProvider();
            return CustJson(_iInvoiceDataProvider.DeleteInvoices(criteria));
        }

        [HttpPost]
        public ContentResult UpdateInvoiceDueDate(UpdateInvoiceDueDate updateInvoiceDueDate)
        {
            _iInvoiceDataProvider = new InvoiceDataProvider();
            return CustJson(_iInvoiceDataProvider.UpdateInvoiceDueDate(updateInvoiceDueDate));
        }
        #region CompanyInvoice

        public ActionResult CompanyClientInvoice()
        {
            //EPXPaymentGateway _paymentObj = new EPXPaymentGateway();
            ////_paymentObj.PayNow();
            //
            //string _API_LOGIN_ID = "98Gv9T2b2"; //"4T7uWP2Wx3C";
            //string _TRANSACTION_KEY = "83c9fGA77uNU8Htw";// "8racW94M8YN5z5hK";
            ////string _API_LOGIN_ID = "4T7uWP2Wx3C";
            ////string _TRANSACTION_KEY = "8racW94M8YN5z5hK";
            //decimal _amount = 15;
            //var data = ChargeCreditCard.Run(_API_LOGIN_ID, _TRANSACTION_KEY, _amount);

            return View(new InvoiceViewModel());
        }

        [HttpPost]
        public JsonResult CompanyClientInvoiceList(InvoiceViewModel model, int pageIndex = 1,
                                             int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iInvoiceDataProvider = new InvoiceDataProvider(Constants.MyezcareOrganizationConnectionString);
            model.OrganizationId = Convert.ToString(SessionHelper.OrganizationId);
            var data = _iInvoiceDataProvider.GetALLFilterInvoiceList(model, pageIndex, pageSize, sortIndex, sortDirection);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
        [HttpGet]
        public JsonResult NinjaInvoice()
        {
            _iInvoiceDataProvider = new InvoiceDataProvider();
            //CacheHelper _cacheHelper = new CacheHelper();
            //long OrgID = !string.IsNullOrEmpty(_cacheHelper.OrganizationID) ? Convert.ToInt64(_cacheHelper.OrganizationID) : 0;
            CacheHelper_MyezCare ch_MyezcareOrg = new CacheHelper_MyezCare();
            MyEzcareOrganization myOrg = ch_MyezcareOrg.GetCachedData<MyEzcareOrganization>();
            long OrgID = Convert.ToInt64(myOrg.OrganizationID);
            var result = _iInvoiceDataProvider.NinjaInvoice(OrgID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult NinjaInvoiceList()
        
{
            _iInvoiceDataProvider = new InvoiceDataProvider();
            var result = _iInvoiceDataProvider.NinjaInvoiceList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult NinjaInvoiceListBilling()

        {
            _iInvoiceDataProvider = new InvoiceDataProvider();
            var result = _iInvoiceDataProvider.NinjaInvoiceListBilling();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult StartProcessingPayment(string invoiceId,string invoiceAmount, string client_id)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                string OrganizationId = Convert.ToString(SessionHelper.OrganizationId);
                
                decimal invoiceAmounttemp = Convert.ToDecimal(invoiceAmount);
                decimal amount = Math.Round(invoiceAmounttemp, 2);

                BillingInformationModel obj = new BillingInformationModel();
                obj.OrganizationId = Convert.ToInt32(OrganizationId);
                IBillingInformationDataProvider _IBillingInformationProvider;
                _IBillingInformationProvider = new BillingInformationDataProvider();
                ServiceResponse responselist = _IBillingInformationProvider.GetBillingDetail(obj);
                List<BillingInformationModel> objlist = (List<BillingInformationModel>)responselist.Data;
                if (objlist.Count() == 0 || objlist[0].customerProfileId==0)
                {
                    Session["client_id"]= client_id;
                    response.Data = "1";

                } else if (objlist!=null)
                {

                    string apiLoginId = System.Configuration.ConfigurationManager.AppSettings["AuthorizedNetapiLoginId"].ToString();
                    string transactionKey = System.Configuration.ConfigurationManager.AppSettings["AuthorizedNettransactionKey"].ToString();
                    string customerProfileId = string.Empty;
                    string customerPaymentProfileId = string.Empty;
                    string customerShippingAddressId = string.Empty;
                    customerProfileId =Convert.ToString(objlist[0].customerProfileId);
                    customerPaymentProfileId = Convert.ToString(objlist[0].customerPaymentProfileId);
                    customerShippingAddressId = Convert.ToString(objlist[0].customerShippingAddressId);

                    string[] responsePaymentDone = ChargeCustomerProfile.Run(apiLoginId, transactionKey, customerProfileId, customerPaymentProfileId, amount);
                    string transId = string.Empty;
                    string responseCode = string.Empty;
                    string code = string.Empty;
                    string description = string.Empty;
                    string authCode = string.Empty;
                   // string statuscode= string.Empty;
                    string errorcode= string.Empty; 
                    string errortext= string.Empty;

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
                            objInvoiceViewModel.InvoiceNumber = invoiceId;
                            objInvoiceViewModel.TransactionIdAuthNet = transId;
                            objInvoiceViewModel.ResponseCodeAuthNet = responseCode;
                            objInvoiceViewModel.MessageCodeAuthNet = code;
                            objInvoiceViewModel.DescriptionAuthNet = description;
                            objInvoiceViewModel.AuthCodeAuthNet = authCode;
                            objInvoiceViewModel.Statuscode = "successfully created transaction";
                            objInvoiceViewModel.ErrorCode = "NA";
                            objInvoiceViewModel.ErrorText = "NA";
                            objInvoiceViewModel.OrganizationId = OrganizationId;
                            IInvoiceDataProvider _InvoiceDataProvider;
                            _InvoiceDataProvider = new InvoiceDataProvider();

                            ServiceResponse responseUpdateInvoice = _InvoiceDataProvider.UpdateInvoiceByInvoiceNumberWithPaymentStatus(objInvoiceViewModel);
                            _iInvoiceDataProvider = new InvoiceDataProvider();
                            ServiceResponse responseNinjaService = _iInvoiceDataProvider.UpdateInvoiceStatus(invoiceId, transId,amount.ToString(),client_id);
                            
                            response.Data = "2";

                        }
                        else if (responsePaymentDone[6].ToLower() == "failed transaction")
                        {
                            errorcode = responsePaymentDone[7].ToString();
                            errortext = responsePaymentDone[8].ToString();
                            InvoiceViewModel objInvoiceViewModel = new InvoiceViewModel();
                            objInvoiceViewModel.InvoiceNumber = invoiceId;
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

                    //response.IsSuccess = true;
                }

                
            }
            catch (Exception ex)
            {
                response.Data = "3";
            }

            return Json(response.Data);
        }

    }
}
