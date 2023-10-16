using Myezcare_Admin.Infrastructure;
using Myezcare_Admin.Infrastructure.DataProvider;
using Myezcare_Admin.Models;
using Myezcare_Admin.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Myezcare_Admin.Controllers
{
    public class InvoiceController : BaseController
    {
        private IOrganizationDataProvider _IOrganizationDataProvider;
        private IinvoiceDataProvider _IinvoiceDataProvider;

        //
        // GET: /Invoice/

        //private _IOrganizationDataProvider
        public InvoiceController()
        {
            _IOrganizationDataProvider = new OrganizationDataProvider();
            _IinvoiceDataProvider = new InvoiceDataProvider();
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Add(string id = "")
        {

            ServiceResponse response = new ServiceResponse();
            InvoiceModel model = new InvoiceModel();
            if (id == "undefined")
            {
                id = String.Empty;
            }
            var data = id != "" ? _IinvoiceDataProvider.GetInvoiceByInvoiceNumber(Convert.ToInt64(id)) : null;
            ViewBag.addInvoiceData = data != null && data != null ? "" : "";
            if (data != null)
            {
                model = data;
            }
            else
            {
                model.InvoiceNumber = _IinvoiceDataProvider.GetInvoiceNumber().ToString();
            }
            return View(model);
        }


        [HttpPost]
        public JsonResult Add(InvoiceModel model)
        {
            Session["AddDataResInvoice"] = null;
            var dt = HttpContext.Request;
            model.InvoiceNumber = "0";
            var data = _IinvoiceDataProvider.AddInvoice(model, dt.Files);
            if (data.IsSuccess)
            {
                Session["AddDataResInvoice"] = data.Data;
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult InvoiceList()
        {
            // Json(_IinvoiceDataProvider.InvoiceList(), JsonRequestBehavior.AllowGet);
            return View(new InvoiceModel());
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public ContentResult GetInvoiceList(InvoiceModel invoiceModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            var response = _IinvoiceDataProvider.InvoiceList(invoiceModel, pageIndex, pageSize, sortIndex, sortDirection);
            return CustJson(response);
        }
        public JsonResult UpdateInvoice(InvoiceModel model)
        {
            var data = _IinvoiceDataProvider.UpdateInvoice(model);
            if (string.IsNullOrEmpty(model.FilePath) && string.IsNullOrEmpty(model.OrganizationName) && data.IsSuccess)
            {
                Session["AddDataResInvoice"] = data.Data;
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult InvoiceFileUpload()
        {
            var file = HttpContext.Request.Files;
            ServiceResponse response = new ServiceResponse();

            var data = Session["AddDataResInvoice"] as InvoiceModel;
            UploadedFileModel fileModel = new UploadedFileModel();
            if (file.Count > 0 && Session["AddDataResInvoice"] != null)
            {
                for (int i = 0; i < file.Count; i++)
                {
                    HttpPostedFileBase file1 = file[i];
                    fileModel = Common.SaveFileRetrunFilePath(file1, "/uploads/Invoice/" + data.DomainName + "/", "", "pdf");
                }
            }

            if (fileModel != null && !string.IsNullOrEmpty(fileModel.TempFilePath) && !string.IsNullOrEmpty(fileModel.FileOriginalName))
            {
                data.FilePath = fileModel.TempFilePath;
                data.OrginalFileName = fileModel.FileOriginalName;
                var res = _IinvoiceDataProvider.UpdateInvoice(data);
                response = res != null && res.IsSuccess && res.Data != null ? res : response;
            }

            Session["AddDataResInvoice"] = null;
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}
