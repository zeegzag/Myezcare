using Myezcare_Admin.Models;
using Myezcare_Admin.Models.Entity;
using Myezcare_Admin.Models.ViewModel;
using Myezcare_Admin.Resources;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace Myezcare_Admin.Infrastructure.DataProvider
{
    public class InvoiceDataProvider : BaseDataProvider, IinvoiceDataProvider
    {
        public ServiceResponse AddInvoice(InvoiceModel model, HttpFileCollectionBase file)
        {

            var response = new ServiceResponse();
            #region check If invoice is already exisit for month and organization

            List<SearchValueData> searchList = new List<SearchValueData>
                    {
                        new SearchValueData { Name = "OrganizationId", Value = Convert.ToString(model.OrganizationId) },
                        new SearchValueData{ Name="MonthId",Value=model.MonthId}
                    };

            List<Invoice> invoiceObj = GetEntityList<Invoice>(searchList);
            if (invoiceObj != null && invoiceObj.Count > 0)
            {
                response.Message = string.Format(Resource.AlreadyExists, Resource.Invoice);
                response.IsSuccess = false;
                response.Data = model;
                return response;
            }
            #endregion

            searchList = new List<SearchValueData>
                    {
                        new SearchValueData { Name = "OrganizationID", Value = Convert.ToString(model.OrganizationId) },
                    };

            MyEzcareOrganization organization = GetEntity<MyEzcareOrganization>(searchList);
            if (organization == null)
            {
                response.IsSuccess = false;
                response.Message = Resource.CanNotSave;
                return response;
            }




            try
            {


                var data = SaveEntity(MapInvoiceEntity(model, false));
                if (data == null || data.InvoiceNumber <= 0)
                {
                    response.IsSuccess = false;
                    response.Message = Resource.CanNotSave;
                    return response;
                }
                else
                {
                    response.Message = string.Format(Resource.RecordCreatedSuccessfully, Resource.Invoice);
                    response.IsSuccess = true;
                    model.InvoiceNumber = Convert.ToString(data.InvoiceNumber);
                    model.OrganizationType = organization.OrganizationType;
                    model.DomainName = organization.DomainName;
                    response.Data = model;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
            }

            return response;

        }

        public ServiceResponse UpdateInvoice(InvoiceModel model)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>
                    {
                        new SearchValueData { Name = "OrganizationID", Value = Convert.ToString(model.OrganizationId) },
                    };


                MyEzcareOrganization organization = GetEntity<MyEzcareOrganization>(searchList);
                if (organization == null)
                    organization = new MyEzcareOrganization();
                if (model == null || string.IsNullOrEmpty(model.InvoiceNumber))
                {
                    response.IsSuccess = false;
                    response.Message = Resource.CanNotSave;
                    return response;
                }
                var data = SaveEntity(MapInvoiceEntity(model, true));
                if (data == null || data.InvoiceNumber <= 0)
                {
                    response.IsSuccess = false;
                    response.Message = Resource.CanNotSave;
                    return response;
                }
                else
                {
                    response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.Invoice);
                    response.IsSuccess = true;
                    var dataMap = MapInvoiceEntity(data, true);
                    dataMap.DomainName = organization.DomainName;
                    dataMap.OrganizationType = organization.OrganizationType;
                    response.Data = dataMap;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
            }
            return response;

        }

        private Invoice MapInvoiceEntity(InvoiceModel model, bool isUpdate)
        {
            Invoice invoice = new Invoice();

            invoice.MonthId = Convert.ToInt32(model.MonthId);
            invoice.OrganizationId = Convert.ToInt64(model.OrganizationId);
            invoice.InvoiceDate = Convert.ToDateTime(model.InvoiceDate);
            invoice.DueDate = Convert.ToDateTime(model.DueDate);
            invoice.PlanName = model.PlanName;

            invoice.ActivePatientQuantity = Convert.ToInt16(model.ActivePatientQuantity);
            invoice.ActivePatientUnit = Convert.ToDecimal(model.ActivePatientUnit);
            invoice.ActivePatientAmount = Convert.ToDecimal(model.ActivePatientAmount);

            invoice.IVRQuantity = Convert.ToInt16(model.IVRQuantity);
            invoice.IVRUnit = Convert.ToDecimal(model.IVRUnit);
            invoice.IVRAmount = Convert.ToDecimal(model.IVRAmount);


            invoice.NumberOfTimeSheetQuantity = Convert.ToInt16(model.NumberOfTimeSheetQuantity);
            invoice.NumberOfTimeSheetUnit = Convert.ToDecimal(model.NumberOfTimeSheetUnit);
            invoice.NumberOfTimeSheetAmount = Convert.ToDecimal(model.NumberOfTimeSheetAmount);

            invoice.MessageQuantity = Convert.ToInt16(model.MessageQuantity);
            invoice.MessageUnit = Convert.ToDecimal(model.MessageUnit);
            invoice.MessageAmount = Convert.ToDecimal(model.MessageAmount);

            invoice.ClaimsQuantity = Convert.ToInt16(model.ClaimsQuantity);
            invoice.ClaimsUnit = Convert.ToDecimal(model.ClaimsUnit);
            invoice.ClaimsAmount = Convert.ToDecimal(model.ClaimsAmount);


            invoice.FormsQuantity = Convert.ToInt16(model.FormsQuantity);
            invoice.FormsUnit = Convert.ToDecimal(model.FormsUnit);
            invoice.FormsAmount = Convert.ToDecimal(model.FormsAmount);


            invoice.InvoiceAmount = Convert.ToDecimal(model.InvoiceAmount);
            invoice.Status = model.Status;
            invoice.InvoiceStatus = model.InvoiceStatus;
            invoice.FilePath = model.FilePath;
            invoice.OrginalFileName = model.OrginalFileName;
            invoice.IsPaid = model.IsPaid;
            if (isUpdate)
            {

            }
            invoice.InvoiceNumber = Convert.ToInt64(model.InvoiceNumber);

            return invoice;
        }

        private InvoiceModel MapInvoiceEntity(Invoice model, bool isUpdate)
        {
            InvoiceModel invoice = new InvoiceModel();

            invoice.MonthId = Convert.ToString(model.MonthId);
            invoice.OrganizationId = Convert.ToString(model.OrganizationId);
            invoice.InvoiceDate = Convert.ToString(model.InvoiceDate);
            invoice.DueDate = Convert.ToString(model.DueDate);
            invoice.PlanName = model.PlanName;

            invoice.ActivePatientQuantity = Convert.ToString(model.ActivePatientQuantity);
            invoice.ActivePatientUnit = Convert.ToString(model.ActivePatientUnit);
            invoice.ActivePatientAmount = Convert.ToString(model.ActivePatientAmount);

            invoice.IVRQuantity = Convert.ToString(model.IVRQuantity);
            invoice.IVRUnit = Convert.ToString(model.IVRUnit);
            invoice.IVRAmount = Convert.ToString(model.IVRAmount);


            invoice.NumberOfTimeSheetQuantity = Convert.ToString(model.NumberOfTimeSheetQuantity);
            invoice.NumberOfTimeSheetUnit = Convert.ToString(model.NumberOfTimeSheetUnit);
            invoice.NumberOfTimeSheetAmount = Convert.ToString(model.NumberOfTimeSheetAmount);

            invoice.MessageQuantity = Convert.ToString(model.MessageQuantity);
            invoice.MessageUnit = Convert.ToString(model.MessageUnit);
            invoice.MessageAmount = Convert.ToString(model.MessageAmount);

            invoice.ClaimsQuantity = Convert.ToString(model.ClaimsQuantity);
            invoice.ClaimsUnit = Convert.ToString(model.ClaimsUnit);
            invoice.ClaimsAmount = Convert.ToString(model.ClaimsAmount);


            invoice.FormsQuantity = Convert.ToString(model.FormsQuantity);
            invoice.FormsUnit = Convert.ToString(model.FormsUnit);
            invoice.FormsAmount = Convert.ToString(model.FormsAmount);


            invoice.InvoiceAmount = Convert.ToString(model.InvoiceAmount);
            invoice.Status = model.Status;
            invoice.InvoiceStatus = model.InvoiceStatus;
            invoice.FilePath = model.FilePath;
            invoice.OrginalFileName = model.OrginalFileName;

            invoice.InvoiceNumber = Convert.ToString(model.InvoiceNumber);
            return invoice;
        }
        public long GetInvoiceNumber()
        {
            var data = GetScalar(StoredProcedure.GetInvoiceNumber);
            long num = Convert.ToInt64(data);
            return num;
        }
        public InvoiceModel GetInvoiceByInvoiceNumber(long invoiceNumber)
        {
            var response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>
                    {
                        new SearchValueData { Name = "InvoiceNumber", Value = invoiceNumber.ToString()},
                    };


            List<InvoiceModel> totalData = GetEntityList<InvoiceModel>(StoredProcedure.GetInvoiceByInvoiceNumber, searchList);
            //response.Data = totalData;
            //response.IsSuccess = true;
            return totalData != null && totalData.Count > 0 ? totalData[0] : null;
        }

        public ServiceResponse InvoiceList1(InvoiceModel model)
        {

            var response = new ServiceResponse();
            List<SearchValueData> searchModel = new List<SearchValueData>();
            var listModel = GetDataSet(StoredProcedure.GetALLInvoiceList);
            //SetOrganizationListModel setOrganizationListModel = new SetOrganizationListModel();
            //listModel = new SearchOrganizationModel { IsDeleted = 0 };
            //setOrganizationListModel.ActiveFilter = Common.SetDeleteFilter();
            var _dt_Applications = listModel.Tables[0];//.AsEnumerable().ToList();
            var data = (from DataRow dr in _dt_Applications.Rows
                        select new InvoiceModel()
                        {
                            MonthId = dr["MonthId"] != null ? dr["MonthId"].ToString() : "",
                            OrganizationName = dr["OrganizationName"] != null ? dr["OrganizationName"].ToString() : "",
                            OrganizationId = dr["OrganizationId"] != null ? dr["OrganizationId"].ToString() : "",
                            InvoiceNumber = dr["InvoiceNumber"] != null ? dr["InvoiceNumber"].ToString() : "",
                        }).ToList();
            response.Data = data;
            return response;
        }

        public ServiceResponse InvoiceList(InvoiceModel invoiceModel, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            var response = new ServiceResponse();

            // List<SearchValueData> searchList = new List<SearchValueData>();
            //SetSearchFilterForOrganizationListPage(invoiceModel, searchList);
            //searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));
            List<SearchValueData> searchList = new List<SearchValueData>
                    {
                        new SearchValueData { Name = "CompanyName", Value =!string.IsNullOrEmpty(invoiceModel.OrganizationName)?invoiceModel.OrganizationName:null},
                        new SearchValueData { Name = "InvoiceDate", Value =!string.IsNullOrEmpty(invoiceModel.InvoiceDate)? Convert.ToDateTime( invoiceModel.InvoiceDate).ToString():null},
                        new SearchValueData { Name = "DueDate", Value =!string.IsNullOrEmpty(invoiceModel.DueDate)? Convert.ToDateTime( invoiceModel.DueDate).ToString():null },
                        new SearchValueData { Name = "InvoiceStatus", Value =!string.IsNullOrEmpty(invoiceModel.InvoiceStatus)? Convert.ToString(invoiceModel.InvoiceStatus):null },
                        new SearchValueData { Name = "IsAll", IsEqual = invoiceModel.IsAll },
                        new SearchValueData { Name = "InvoiceAmount", Value =!string.IsNullOrEmpty(invoiceModel.InvoiceAmount)? ( invoiceModel.InvoiceAmount):null},
                        new SearchValueData { Name = "PaidAmount", Value = !string.IsNullOrEmpty( invoiceModel.PaidAmount)?invoiceModel.PaidAmount:null},
                        new SearchValueData { Name = "IsPaid", IsEqual =  invoiceModel.IsPaid},
                    };
            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));
            List<InvoiceModel> totalData = new List<InvoiceModel>();
            totalData = GetEntityList<InvoiceModel>(StoredProcedure.GetALLFilterInvoiceList, searchList);

            if (totalData != null && totalData.Count > 0)
            {
                totalData.ForEach(x => { x.InvoiceStatusName = GetInvoiceStatusName(x.InvoiceStatus); });
            }


            int count = 0;
            //if (totalData != null && totalData.Count > 0)
            //count = totalData.First().Count;

            Page<InvoiceModel> getOrganizationList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);

            response.IsSuccess = true;
            response.Data = getOrganizationList;
            return response;
        }

        private static void SetSearchFilterForOrganizationListPage(InvoiceModel InvoiceModel, List<SearchValueData> searchList)
        {
            //searchList.Add(new SearchValueData { Name = "OrganizationType", Value = searchOrganizationModel.OrganizationType });
            //searchList.Add(new SearchValueData { Name = "OrganizationTypeID", Value = Convert.ToString(searchOrganizationModel.OrganizationTypeID) });

        }


        private string GetInvoiceStatusName(string id)
        {
            string name = "";
            switch (id)
            {
                case "1":
                    name = "Paid";
                    break;
                case "2":
                    name = "Cancelled";
                    break;
                case "3":
                    name = "Unpaid";
                    break;
                case "4":
                    name = "Overdue";
                    break;
                case "5":
                    name = "WriteOff";
                    break;

            }
            return name;
        }
    }
}