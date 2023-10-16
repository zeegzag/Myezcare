using Myezcare_Admin.Models;
using Myezcare_Admin.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Myezcare_Admin.Infrastructure.DataProvider
{
    public interface IinvoiceDataProvider
    {
        ServiceResponse AddInvoice(InvoiceModel model, HttpFileCollectionBase file);
        ServiceResponse UpdateInvoice(InvoiceModel model);
        long GetInvoiceNumber();
        InvoiceModel GetInvoiceByInvoiceNumber(long invoiceNumber);
        ServiceResponse InvoiceList(InvoiceModel invoiceModel, int pageIndex, int pageSize, string sortIndex, string sortDirection);
    }
}
