
using System.Collections.Generic;
using System.Data;
using Zarephath.Core.Models;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public interface IInvoiceDataProvider
    {
        ServiceResponse SetInvoiceListPage();
        ServiceResponse GetInvoiceDetail(long invoiceId, long payorId = 0);

        ServiceResponse PayInvoiceAmount(PayInvoiceAmountDetail payInvoiceAmountDetail, long loggedInID);
        ServiceResponse GetInvoiceList(SearchInvoiceListPage searchInvoiceListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse GenerateInvoices(GenerateInvoicesCriteria criteria);
        ServiceResponse DeleteInvoices(DeleteInvoices criteria);
        ServiceResponse GetALLFilterInvoiceList(InvoiceViewModel model, int pageIndex, int pageSize,
            string sortIndex, string sortDirection);
        List<InvoiceViewModel> GetUnPaidInvoiceByOrganizationId(string orgId);
        InvoiceViewModel UpdateInvoiceByInvoiceNumber(InvoiceViewModel invoiceViewModel);
        ServiceResponse NinjaInvoice(long OrgID);
        List<InvoiceMod> NinjaInvoiceList();
        List<InvoiceModBilling> NinjaInvoiceListBilling();
        ServiceResponse UpdateInvoiceByInvoiceNumberWithPaymentStatus(InvoiceViewModel invoiceViewModel);
        ServiceResponse UpdateInvoiceStatus(string invoiceId, string transId, string amount, string client_id);
        ServiceResponse UpdateInvoiceDueDate(UpdateInvoiceDueDate updateInvoiceDueDate);
    }
        
}
