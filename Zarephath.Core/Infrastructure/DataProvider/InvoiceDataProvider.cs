using Elmah;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class InvoiceDataProvider : BaseDataProvider, IInvoiceDataProvider
    {
        public InvoiceDataProvider(string conString)
            : base(conString)
        {
        }

        public InvoiceDataProvider() { }

        public ServiceResponse SetInvoiceListPage()
        {
            ServiceResponse response = new ServiceResponse();
            SetInvoiceListPage model = GetMultipleEntity<SetInvoiceListPage>(StoredProcedure.SetInvoiceListPage);
            model.DeleteFilter = Common.SetDeleteFilter();
            model.SearchInvoiceListPage = new SearchInvoiceListPage() { IsDeleted = 0 };
            model.InvoicesCriteria = new GenerateInvoicesCriteria() { InvoiceGenerationFrequency = _cacheHelper.InvoiceGenerationFrequency };
            response.Data = model;
            return response;
        }

        public ServiceResponse GetInvoiceDetail(long invoiceId, long payorId = 0)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                GetInvoiceDetail getInvoiceDetail = GetMultipleEntity<GetInvoiceDetail>(StoredProcedure.HC_GetInvoiceDetail, new List<SearchValueData>
                {
                    new SearchValueData("InvoiceId",Convert.ToString(invoiceId))
                });
                getInvoiceDetail.Payors.ForEach(e => e.EncryptedPayorID = Crypto.Encrypt(Convert.ToString(e.PayorID)));
                if (payorId > 0)
                {
                    getInvoiceDetail.Payors = getInvoiceDetail.Payors.Where(e => e.PayorID == payorId).ToList();
                    getInvoiceDetail.payorSelected.PayorID = payorId;
                    getInvoiceDetail.payorSelected = getInvoiceDetail.Payors.FirstOrDefault();
                }
                getInvoiceDetail.InvoiceDetailModel.TotalAmount = getInvoiceDetail.InvoiceTransactionDetailModelList.Sum(x => x.Amount);
                response.IsSuccess = true;
                response.Data = getInvoiceDetail;
            }
            catch (Exception e)
            {
                response.Message = Resource.ErrorOccured;
                if (HttpContext.Current != null)
                {
                    HttpContext context = HttpContext.Current;
                    var signal = ErrorSignal.FromContext(context);
                    signal.Raise(e, context);
                }
            }
            return response;
        }

        public ServiceResponse PayInvoiceAmount(PayInvoiceAmountDetail payInvoiceAmountDetail, long loggedInID)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                TransactionResultWithGetInvoiceDetail getInvoiceDetail =
                    GetMultipleEntity<TransactionResultWithGetInvoiceDetail>(StoredProcedure.HC_PayInvoiceAmount, new List<SearchValueData>
                {
                    new SearchValueData("PaymentType",Convert.ToString(payInvoiceAmountDetail.PaymentType)),
                    new SearchValueData("InvoiceId",Convert.ToString(payInvoiceAmountDetail.InvoiceId)),
                    new SearchValueData("ReferralId",Convert.ToString(payInvoiceAmountDetail.ReferralId)),
                    new SearchValueData("Amount",Convert.ToString(payInvoiceAmountDetail.Amount)),
                    new SearchValueData("InvoiceStatus_Paid",Convert.ToString((int)Common.InvoiceStatus.Paid)),
                    new SearchValueData("InvoiceStatus_PartialPaid",Convert.ToString((int)Common.InvoiceStatus.PartialPaid)),
                    new SearchValueData("InvoiceStatus_Void",Convert.ToString((int)Common.InvoiceStatus.Void)),
                    new SearchValueData("ServerDateTime",Common.GetOrgCurrentDateTime().ToString(Constants.DbDateTimeFormat)),
                    new SearchValueData("LoggedInID",Convert.ToString(loggedInID))
                });
                getInvoiceDetail.InvoiceDetailModel.TotalAmount = getInvoiceDetail.InvoiceTransactionDetailModelList.Sum(x => x.Amount);
                response.Data = getInvoiceDetail;
                if (getInvoiceDetail.TransactionResult.TransactionResultId > 0)
                {
                    response.IsSuccess = true;
                    if (payInvoiceAmountDetail.PaymentType == (int)Common.InvoiceStatus.Paid ||
                        payInvoiceAmountDetail.PaymentType == (int)Common.InvoiceStatus.PartialPaid)
                    {
                        response.Message = Resource.PaymentDone;
                    }
                    else if (payInvoiceAmountDetail.PaymentType == (int)Common.InvoiceStatus.Void)
                    {
                        response.Message = Resource.InvoiceMarkVoid;
                    }
                }
                else
                {
                    response.Message = Resource.ErrorOccured;
                }
            }
            catch (Exception e)
            {
                response.Message = Resource.ErrorOccured;
                if (HttpContext.Current != null)
                {
                    HttpContext context = HttpContext.Current;
                    var signal = ErrorSignal.FromContext(context);
                    signal.Raise(e, context);
                }
            }
            return response;
        }

        public ServiceResponse GetInvoiceList(SearchInvoiceListPage searchInvoiceListPage, int pageIndex, int pageSize,
            string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchInvoiceListPage != null)
                SetSearchFilterForInvoiceList(searchInvoiceListPage, searchList);

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<ListInvoiceModel> totalData = GetEntityList<ListInvoiceModel>(StoredProcedure.HC_GetInvoiceList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<ListInvoiceModel> invoiceList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = invoiceList;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse GenerateInvoices(GenerateInvoicesCriteria criteria)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "InvoiceGenerationFrequency", Value = Convert.ToString(criteria.InvoiceGenerationFrequency)},
                    new SearchValueData {Name = "WeekStartDay", Value = Convert.ToString(Common.GetCalWeekStartDay())},
                };
                if (!string.IsNullOrEmpty(criteria.ReferralIDs))
                { searchlist.Add(new SearchValueData { Name = "ReferralIDs", Value = Convert.ToString(criteria.ReferralIDs) }); }
                if (!string.IsNullOrEmpty(criteria.CareTypeIDs))
                { searchlist.Add(new SearchValueData { Name = "CareTypeIDs", Value = Convert.ToString(criteria.CareTypeIDs) }); }
                if (criteria.StartDate.HasValue)
                { searchlist.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToString(criteria.StartDate) }); }
                if (criteria.EndDate.HasValue)
                { searchlist.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToString(criteria.EndDate) }); }

                TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.HC_GenerateInvoicesService, searchlist);
                if (result.TransactionResultId == 0)
                {
                    response.IsSuccess = true;
                    response.Message = string.Format(Resource.NoRecordFoundMessage, Resource.Visit);
                    response.ErrorCode = Constants.ErrorCode_Warning;
                }
                else if (result.TransactionResultId == 1)
                {
                    response.IsSuccess = true;
                    response.Message = string.Format(Resource.RecordProcessedSuccessfully, Resource.Visit);
                }
                else
                {
                    response.Message = Resource.SomethingWentWrong;
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return response;
        }

        public ServiceResponse DeleteInvoices(DeleteInvoices criteria)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralInvoiceIDs", Value = criteria.ReferralInvoiceIDs},
                };
                TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.DeleteInvoices, searchlist);
                if (result.TransactionResultId == 1)
                {
                    response.IsSuccess = true;
                    response.Message = string.Format(Resource.RecordDeletedSuccessfully, Resource.Invoice);
                }
                else
                {
                    response.Message = Resource.SomethingWentWrong;
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return response;
        }

        public ServiceResponse UpdateInvoiceDueDate(UpdateInvoiceDueDate updateInvoiceDueDate)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralInvoiceID", Value = updateInvoiceDueDate.ReferralInvoiceID},
                    new SearchValueData {Name = "InvoiceDueDate", Value = updateInvoiceDueDate.InvoiceDueDate},
                };
                TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.UpdateInvoice, searchlist);
                if (result.TransactionResultId == 1)
                {
                    response.IsSuccess = true;
                    response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.Invoice);
                }
                else
                {
                    response.Message = Resource.SomethingWentWrong;
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return response;
        }
        private static void SetSearchFilterForInvoiceList(SearchInvoiceListPage searchInvoiceListPage, List<SearchValueData> searchList)
        {
            searchList.Add(new SearchValueData { Name = "PatientName", Value = searchInvoiceListPage.PatientName });
            if (searchInvoiceListPage.InvoiceDate.HasValue)
                searchList.Add(new SearchValueData { Name = "InvoiceDate", Value = searchInvoiceListPage.InvoiceDate.Value.ToString(Constants.DbDateFormat) });
            if (searchInvoiceListPage.InvoiceAmount.HasValue)
                searchList.Add(new SearchValueData { Name = "InvoiceAmount", Value = Convert.ToString(searchInvoiceListPage.InvoiceAmount) });
            if (searchInvoiceListPage.PaidAmount.HasValue)
                searchList.Add(new SearchValueData { Name = "PaidAmount", Value = Convert.ToString(searchInvoiceListPage.PaidAmount) });
            searchList.Add(new SearchValueData { Name = "InvoiceStatus", Value = Convert.ToString(searchInvoiceListPage.InvoiceStatus) });

            searchList.Add(new SearchValueData { Name = "RoleID", Value = Convert.ToString(SessionHelper.RoleID) });
            searchList.Add(new SearchValueData { Name = "LoggedInID", Value = Convert.ToString(SessionHelper.LoggedInID) });
            searchList.Add(new SearchValueData { Name = "IsPatientLogin", Value = Convert.ToString((SessionHelper.IsEmployeeLogin) ? false : true) });

            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchInvoiceListPage.IsDeleted) });
        }


        public ServiceResponse GetALLFilterInvoiceList(InvoiceViewModel model, int pageIndex, int pageSize,
            string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>
            {
                new SearchValueData { Name = "InvoiceDate", Value = model.InvoiceDate },
                new SearchValueData { Name = "DueDate", Value = model.DueDate },
                new SearchValueData { Name = "OrganizationId", Value = model.OrganizationId },

                new SearchValueData { Name = "PaidAmount", Value = model.PaidAmount },
                new SearchValueData { Name = "InvoiceAmount", Value = model.InvoiceAmount },
                new SearchValueData { Name = "Status", IsEqual = model.Status },
                new SearchValueData { Name = "InvoiceStatus", Value = model.InvoiceStatus }
            };

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));


            List<InvoiceViewModel> totalData = GetEntityList<InvoiceViewModel>(StoredProcedure.GetAllOrganizationInvoiceByOrgId, searchList);
            int count = 0;
            //totalData.ForEach(x =>
            //{
            //    x.EncryptedMonthDate = Crypto.Encrypt(x.DueDate);
            //    x.EncryptedAmount = Crypto.Encrypt(x.InvoiceAmount);
            //    x.EncryptedInvoiceNumber = Crypto.Encrypt(x.InvoiceNumber);
            //});
            Page<InvoiceViewModel> invoiceList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = invoiceList;
            response.IsSuccess = true;
            return response;
        }


        public List<InvoiceViewModel> GetUnPaidInvoiceByOrganizationId(string orgId)
        {
            List<SearchValueData> searchList = new List<SearchValueData>
            {
                new SearchValueData { Name = "OrganizationId", Value = orgId }
            };
            List<InvoiceViewModel> totalData = GetEntityList<InvoiceViewModel>(StoredProcedure.GetUnPaidInvoiceByOrganizationId, searchList);
            return totalData;
        }

        public InvoiceViewModel UpdateInvoiceByInvoiceNumber(InvoiceViewModel invoiceViewModel)
        {
            try
            {
                var incData = GetEntity<Invoice>(Convert.ToInt64(invoiceViewModel.InvoiceNumber));
                incData.PaymentDate = DateTime.Now.ToString();
                incData.UpdatedDate = DateTime.Now.ToString();
                incData.IsPaid = invoiceViewModel.IsPaid;
                incData.InvoiceStatus = invoiceViewModel.InvoiceStatus;
                incData.TransactionId = invoiceViewModel.TransactionId;
                incData.PaidAmount = invoiceViewModel.InvoiceAmount;
                var data = SaveEntity(incData);
                return invoiceViewModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private Invoice MapInvoiceEntity(InvoiceViewModel model)
        {
            Invoice invoice = new Invoice
            {
                MonthId = Convert.ToInt32(model.MonthId),
                OrganizationId = Convert.ToInt64(model.OrganizationId),
                InvoiceDate = Convert.ToDateTime(model.InvoiceDate),
                DueDate = Convert.ToDateTime(model.DueDate),
                PlanName = model.PlanName,

                ActivePatientQuantity = Convert.ToInt16(model.ActivePatientQuantity),
                ActivePatientUnit = Convert.ToDecimal(model.ActivePatientUnit),
                ActivePatientAmount = Convert.ToDecimal(model.ActivePatientAmount),

                IVRQuantity = Convert.ToInt16(model.IVRQuantity),
                IVRUnit = Convert.ToDecimal(model.IVRUnit),
                IVRAmount = Convert.ToDecimal(model.IVRAmount),


                NumberOfTimeSheetQuantity = Convert.ToInt16(model.NumberOfTimeSheetQuantity),
                NumberOfTimeSheetUnit = Convert.ToDecimal(model.NumberOfTimeSheetUnit),
                NumberOfTimeSheetAmount = Convert.ToDecimal(model.NumberOfTimeSheetAmount),

                MessageQuantity = Convert.ToInt16(model.MessageQuantity),
                MessageUnit = Convert.ToDecimal(model.MessageUnit),
                MessageAmount = Convert.ToDecimal(model.MessageAmount),

                ClaimsQuantity = Convert.ToInt16(model.ClaimsQuantity),
                ClaimsUnit = Convert.ToDecimal(model.ClaimsUnit),
                ClaimsAmount = Convert.ToDecimal(model.ClaimsAmount),


                FormsQuantity = Convert.ToInt16(model.FormsQuantity),
                FormsUnit = Convert.ToDecimal(model.FormsUnit),
                FormsAmount = Convert.ToDecimal(model.FormsAmount),


                InvoiceAmount = Convert.ToDecimal(model.InvoiceAmount),
                Status = model.Status,
                InvoiceStatus = model.InvoiceStatus,
                FilePath = model.FilePath,
                OrginalFileName = model.OrginalFileName,

                InvoiceNumber = Convert.ToInt64(model.InvoiceNumber)
            };

            return invoice;
        }
        public ServiceResponse NinjaInvoice(long OrgID)
        {
            {
                CareGiverApi careGiverApi = new CareGiverApi();
                var results = careGiverApi.NinjaInvoice(OrgID);
                return results;

            }
        }
        public List<InvoiceMod> NinjaInvoiceList()
        {
            CareGiverApi careGiverApi = new CareGiverApi();
            CacheHelper_MyezCare ch_MyezcareOrg = new CacheHelper_MyezCare();
            MyEzcareOrganization myOrg = ch_MyezcareOrg.GetCachedData<MyEzcareOrganization>();
            long OrgIDs = Convert.ToInt64(myOrg.OrganizationID);
            string client_id = NinjaInvoice(OrgIDs).Message;
            var InvoiceModlist = new List<InvoiceMod>();
            if (client_id != null)
            {
                InvoiceModlist = careGiverApi.NinjaInvoiceList(client_id);
            }

            return InvoiceModlist;


        }
        public List<InvoiceModBilling> NinjaInvoiceListBilling()
        {
            CareGiverApi careGiverApi = new CareGiverApi();
            CacheHelper_MyezCare ch_MyezcareOrg = new CacheHelper_MyezCare();
            MyEzcareOrganization myOrg = ch_MyezcareOrg.GetCachedData<MyEzcareOrganization>();
            long OrgIDs = Convert.ToInt64(myOrg.OrganizationID);
            string client_id = NinjaInvoice(OrgIDs).Message;
            var InvoiceModBillinglist = new List<InvoiceModBilling>();
            if (client_id != null)
            {
                InvoiceModBillinglist = careGiverApi.NinjaInvoiceListBilling(client_id);
            }

            return InvoiceModBillinglist;


        }
        private bool SendMailForUpdatingNinjaInvoice(string invoiceId, string transId, string amount, string client_id)
        {
            bool isSentMail = false;

            //string client_id =System.Configuration.ConfigurationManager.AppSettings["ClientID"].ToString();
            string toEmailId = System.Configuration.ConfigurationManager.AppSettings["zapirepaymentemailid"].ToString();
            StringBuilder formatEmailBody = new StringBuilder();
            string id = Convert.ToString((Convert.ToInt64(invoiceId) + 1));
            formatEmailBody.Append("id: " + id + "\n");
            formatEmailBody.Append("amount: " + amount + "\n");
            formatEmailBody.Append("client_id: " + client_id + "\n");
            formatEmailBody.Append("invoice_number: " + invoiceId + "\n");
            formatEmailBody.Append("invoice_status_id: 6" + "\n");
            formatEmailBody.Append("Transaction_reference: " + transId + "\n");

            isSentMail = Common.SendEmailWithoutHTMLFormat("PaymentUpdate", null, toEmailId, formatEmailBody.ToString(), EnumEmailType.HomeCare_EmailPayment_Notification.ToString());
            return isSentMail;
        }

        public ServiceResponse UpdateInvoiceStatus(string invoiceId, string transId, string amount, string client_id)
        {
            ServiceResponse objResponse = new ServiceResponse();

            bool value = SendMailForUpdatingNinjaInvoice(invoiceId, transId, amount, client_id);
            objResponse.Data = value;
            objResponse.IsSuccess = true;
            return objResponse;

        }

        public ServiceResponse UpdateInvoiceByInvoiceNumberWithPaymentStatus(InvoiceViewModel invoiceViewModel)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {

                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "NINJAInvoiceNumber", Value = Convert.ToString(invoiceViewModel.InvoiceNumber)},
                    new SearchValueData {Name = "TransactionIdAuthNet", Value = Convert.ToString(invoiceViewModel.TransactionIdAuthNet)},
                    new SearchValueData {Name = "ResponseCodeAuthNet", Value = Convert.ToString(invoiceViewModel.ResponseCodeAuthNet)},
                    new SearchValueData {Name = "MessageCodeAuthNet", Value = Convert.ToString(invoiceViewModel.MessageCodeAuthNet)},
                    new SearchValueData {Name = "DescriptionAuthNet", Value = Convert.ToString(invoiceViewModel.DescriptionAuthNet)},
                    new SearchValueData {Name = "AuthCodeAuthNet", Value = Convert.ToString(invoiceViewModel.AuthCodeAuthNet)},
                    new SearchValueData {Name = "OrganizationId", Value = Convert.ToString(invoiceViewModel.OrganizationId)},
                    new SearchValueData {Name = "Statuscode", Value = Convert.ToString(invoiceViewModel.Statuscode)},
                    new SearchValueData {Name = "ErrorCode", Value = Convert.ToString(invoiceViewModel.ErrorCode)},
                    new SearchValueData {Name = "ErrorText", Value = Convert.ToString(invoiceViewModel.ErrorText)},

                  };

                GetScalarAdmin(StoredProcedure.SaveInvoiceAuthorizeNetDetail, searchlist);
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
