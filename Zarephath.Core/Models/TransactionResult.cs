using PetaPoco;
using System.Collections.Generic;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Models
{
    public class TransactionResult
    {
        public int TransactionResultId { get; set; }
        public long TablePrimaryId { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class TransactionResultWithBroadcastUserDetails
    {
        public TransactionResult TransactionResult { get; set; }
        public string SiteName { get; set; }
        public List<BroadcastNotificationUserDetails> BroadcastNotificationUserDetailList { get; set; }
    }

    public class TransactionResultWithGetInvoiceDetail
    {
        public TransactionResultWithGetInvoiceDetail()
        {
            TransactionResult = new TransactionResult();
            InvoiceDetailModel = new InvoiceDetailModel();
            InvoiceTransactionDetailModelList = new List<InvoiceTransactionDetailModel>();
            ReferralPaymentHistoriesDetailList = new List<ReferralPaymentHistoriesDetail>();
        }
        public TransactionResult TransactionResult { get; set; }
        public InvoiceDetailModel InvoiceDetailModel { get; set; }
        public List<InvoiceTransactionDetailModel> InvoiceTransactionDetailModelList { get; set; }
        public List<ReferralPaymentHistoriesDetail> ReferralPaymentHistoriesDetailList { get; set; }
    }
}
