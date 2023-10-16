using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Helpers
{
    public class PaymentFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (SessionHelper.CompanyInvoiceInfo != null && SessionHelper.CompanyInvoiceInfo.Count > 0)
            {
                var isOverDue = checkOverDuePayment();
                if (isOverDue.Count > 0)
                {
                    filterContext.Result = new ViewResult() { ViewName = "~/AccessDenied.cshtml" };
                }

                base.OnResultExecuting(filterContext);
            }
            //You may fetch data from database here 
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (SessionHelper.CompanyInvoiceInfo != null && SessionHelper.CompanyInvoiceInfo.Count > 0)
            {
                var isOverDue = checkOverDuePayment();
                if (isOverDue.Count > 0)
                {
                    filterContext.Result = new ViewResult() { ViewName = "~/AccessDenied.cshtml" };
                }
                base.OnActionExecuting(filterContext);
            }
        }

        private List<InvoiceMod> checkOverDuePayment()
        {
            List<InvoiceMod> checkPaymentDueDateOver3 = SessionHelper.CompanyInvoiceInfo != null && SessionHelper.CompanyInvoiceInfo.Count > 0 ?
        SessionHelper.CompanyInvoiceInfo.Where(x => DateTime.UtcNow.Date > Convert.ToDateTime(x.DueDate).AddDays(3).Date).ToList() :
        new List<InvoiceMod>();
            if (checkPaymentDueDateOver3.Count > 0)
            {
            }
            return checkPaymentDueDateOver3;
        }
    }
}
