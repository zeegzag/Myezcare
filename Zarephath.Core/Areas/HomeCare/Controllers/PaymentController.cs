using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Areas.HomeCare.Controllers
{
    public class PaymentController : Controller
    {
        private IInvoiceDataProvider _iInvoiceDataProvider;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PaymentNow()
        {
            return View();
        }

        public ActionResult PaymentBill(string bid, string am, string bm)
        {
            ViewBag.amount = Crypto.Decrypt(am);
            return View();
        }

        [HttpPost]
        public JsonResult PaymentFinal(PaymentVM paymentVM)
        {
            //string _API_LOGIN_ID = "98Gv9T2b2"; //"4T7uWP2Wx3C";
            //string _TRANSACTION_KEY = "83c9fGA77uNU8Htw";// "8racW94M8YN5z5hK";
            ServiceResponse serviceResponse = new ServiceResponse();

            string _API_LOGIN_ID = "4T7uWP2Wx3C";
            string _TRANSACTION_KEY = "8racW94M8YN5z5hK";
            paymentVM.BillingMonthDate = Convert.ToDateTime(Crypto.Decrypt(paymentVM.BillingMonthDate)).ToString();
            paymentVM.CompanyName = SessionHelper.DomainName + "_" + SessionHelper.OrganizationId + "_" + Crypto.Decrypt(paymentVM.InvoiceNumber);
            var data = ChargeCreditCard.Run(paymentVM, _API_LOGIN_ID, _TRANSACTION_KEY);
            if (data.ResponseCode == "I00001")
            {
                _iInvoiceDataProvider = new InvoiceDataProvider(Constants.MyezcareOrganizationConnectionString);
                InvoiceViewModel model = new InvoiceViewModel();
                model.InvoiceAmount = Crypto.Decrypt(paymentVM.Amount);
                model.InvoiceNumber = Crypto.Decrypt(paymentVM.InvoiceNumber);
                model.IsPaid = true;
                model.InvoiceStatus = "1";
                model.TransactionId = data.TransactionId;
                var data1 = _iInvoiceDataProvider.UpdateInvoiceByInvoiceNumber(model);
                serviceResponse.IsSuccess = data1 != null ? true : false;
                serviceResponse.Message = data1 != null ? data.Text : "Tranaction not completed";
                
                serviceResponse.Data = data;
                ViewBag.SucceessMess = data.TransactionId;
                HttpContext.Session["CompanyInvoiceDetails"] = null;
                Session.Remove("CompanyInvoiceDetails");
            }
            else
            {
                _iInvoiceDataProvider = new InvoiceDataProvider(Constants.MyezcareOrganizationConnectionString);
                InvoiceViewModel model = new InvoiceViewModel();
                model.InvoiceAmount = Crypto.Decrypt(paymentVM.Amount);
                model.InvoiceNumber = Crypto.Decrypt(paymentVM.InvoiceNumber);
                model.IsPaid = false;
                model.InvoiceStatus = "3";
                model.TransactionId = data.TransactionId;
                var data1 = _iInvoiceDataProvider.UpdateInvoiceByInvoiceNumber(model);
                serviceResponse.Data = data1;
                serviceResponse.IsSuccess = false;
                serviceResponse.Message = data.ErrorText;
            }
            return Json(serviceResponse, JsonRequestBehavior.AllowGet);
        }
    }
}
