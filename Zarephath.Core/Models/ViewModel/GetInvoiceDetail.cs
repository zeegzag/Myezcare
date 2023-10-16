using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Utility;
using Zarephath.Core.Models.Entity;

namespace Zarephath.Core.Models.ViewModel
{
    public class GetInvoiceDetail
    {
        public GetInvoiceDetail()
        {
            InvoiceDetailModel = new InvoiceDetailModel();
            InvoiceTransactionDetailModelList = new List<InvoiceTransactionDetailModel>();
            ReferralPaymentHistoriesDetailList = new List<ReferralPaymentHistoriesDetail>();
            Payors = new List<Payor>();
            payorSelected = new Payor();
        }
        public InvoiceDetailModel InvoiceDetailModel { get; set; }
        public List<InvoiceTransactionDetailModel> InvoiceTransactionDetailModelList { get; set; }
        public List<ReferralPaymentHistoriesDetail> ReferralPaymentHistoriesDetailList { get; set; }
        public List<Payor> Payors { get; set; }
        public Payor payorSelected { get; set; }
    }

    public class InvoiceDetailModel
    {
        public long ReferralInvoiceID { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime InvoiceDueDate { get; set; }
        public decimal PayAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public int? InvoiceStatus { get; set; }
        public long ReferralID { get; set; }
        public string ReferralName { get; set; }
        public string ReferralAddress { get; set; }
        public string ReferralCity { get; set; }
        public string ReferralState { get; set; }
        public string ReferralZipCode { get; set; }
        public string AHCCCSID { get; set; }
        public decimal InvoiceTaxRate { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime Dob { get; set; }
        [Ignore]
        public string StrInvoiceStatus
        {
            get { return InvoiceStatus.HasValue ? EnumHelper<Common.InvoiceStatus>.GetDisplayValue((Common.InvoiceStatus)InvoiceStatus) : string.Empty; }
        }

        [Ignore]
        public string StrInvoiceDate
        {
            get { return Common.ConvertToOrgDateFormat(InvoiceDate); }
        }
        [Ignore]
        public string StrInvoiceDueDate
        {
            get { return Common.ConvertToOrgDateFormat(InvoiceDueDate); }
        }

        [Ignore]
        public decimal TotalPayableAmount
        {
            get { return PayAmount - PaidAmount; }
        }
    }

    public class InvoiceTransactionDetailModel
    {
        public long ReferralInvoiceTransactionID { get; set; }
        public long EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public long ScheduleID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CareTypeName { get; set; }
        public decimal Rate { get; set; }
        public decimal? PerUnitQuantity { get; set; }
        public decimal Amount { get; set; }
        public DateTime ServiceDate { get; set; }
        public long ServiceTime { get; set; }
    }

    public class ReferralPaymentHistoriesDetail
    {
        public long ReferralPaymentHistoryId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal PaidAmount { get; set; }
        public bool IsDeleted { get; set; }

        [Ignore]
        public string StrPaymentDate
        {
            get { return Common.ConvertToOrgDateFormat(PaymentDate); }
        }
    }

    public class PayInvoiceAmountDetail
    {
        public int PaymentType { get; set; }
        public long InvoiceId { get; set; }
        public long ReferralId { get; set; }
        public decimal Amount { get; set; }
    }

    public class SetInvoiceListPage
    {
        public SetInvoiceListPage()
        {
            ReferralList = new List<ReferralListModel>();
            CareTypeList = new List<CareTypemodel>();
            SearchInvoiceListPage = new SearchInvoiceListPage();
            InvoicesCriteria = new GenerateInvoicesCriteria();
            DeleteFilter = new List<NameValueData>();
        }
        public List<ReferralListModel> ReferralList { get; set; }
        public List<CareTypemodel> CareTypeList { get; set; }
        [Ignore]
        public SearchInvoiceListPage SearchInvoiceListPage { get; set; }
        [Ignore]
        public GenerateInvoicesCriteria InvoicesCriteria { get; set; }
        [Ignore]
        public List<NameValueData> DeleteFilter { get; set; }
    }

    public class SearchInvoiceListPage
    {
        public DateTime? InvoiceDate { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public decimal? PaidAmount { get; set; }
        public int? InvoiceStatus { get; set; }
        public string PatientName { get; set; }
        public int IsDeleted { get; set; }
        public string ListOfIdsInCsv { get; set; }
    }

    public class GenerateInvoicesCriteria
    {
        public string ReferralIDs { get; set; }
        public string CareTypeIDs { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string InvoiceGenerationFrequency { get; set; }
    }

    public class DeleteInvoices
    {
        public string ReferralInvoiceIDs { get; set; }
    }

    public class ListInvoiceModel
    {
        public long ReferralInvoiceID { get; set; }
        public long ReferralID { get; set; }
        public string PatientName { get; set; }
        public string CareTypeName { get; set; }
        public DateTime ServiceStartDate { get; set; }
        public DateTime ServiceEndDate { get; set; }
        public string ServiceTime { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime InvoiceDueDate { get; set; }
        public float InvoiceAmount { get; set; }
        public float PaidAmount { get; set; }
        public int InvoiceStatus { get; set; }
        public string EncryptedReferralInvoiceID { get { return Crypto.Encrypt(Convert.ToString(ReferralInvoiceID)); } }
        public bool IsDeleted { get; set; }
        public int Row { get; set; }
        public int Count { get; set; }
        public string StrInvoiceStatus { get { return EnumHelper<Common.InvoiceStatus>.GetDisplayValue((Common.InvoiceStatus)InvoiceStatus); } }
    }
    public class UpdateInvoiceDueDate
    {
        public string ReferralInvoiceID { get; set; }
        public string InvoiceDueDate { get; set; }
    }
}
