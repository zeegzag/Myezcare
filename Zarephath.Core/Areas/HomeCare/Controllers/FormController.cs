using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TallComponents.PDF;
using TallComponents.PDF.Actions;
using TallComponents.PDF.Forms.Data;
using TallComponents.PDF.Forms.Fields;
using TallComponents.PDF.JavaScript;
using Zarephath.Core.Controllers;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;
using Zarephath.Core.Models.ViewModel;
using System.Linq;
using SelectPdf;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;

namespace Zarephath.Core.Areas.HomeCare.Controllers
{
    [Authorize]
    public class FormController : BaseController
    {
        private IFormDataProvider _formDataProvider;


        #region Form List For All Users To Save For The Patients & Employees




        [HttpGet]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public ActionResult FormList()
        {
            _formDataProvider = new FormDataProvider(Constants.MyezcareOrganizationConnectionString);
            FormPageModel model = (FormPageModel)_formDataProvider.SetFormListPage().Data;


            #region GET Pateint & Employee List FROM Main Database & Mark the Partial OR Not

            _formDataProvider = new FormDataProvider();
            ServiceResponse res = _formDataProvider.GetPatientEmpInfoModel();
            GetPatientEmpInfoModel data = (GetPatientEmpInfoModel)res.Data;
            model.EmployeeList = data.EmployeeList;
            model.ReferralList = data.ReferralList;

            model.ForPatient = true;
            model.ForEmployee = false;

            #endregion

            return View(model);
        }


        [HttpGet]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public ActionResult PartialFormList(string id)
        {
            _formDataProvider = new FormDataProvider(Constants.MyezcareOrganizationConnectionString);
            FormPageModel model = (FormPageModel)_formDataProvider.SetFormListPage().Data;
            model.SearchFormModel.ReferralID = Convert.ToInt64(id);

            #region GET Pateint & Employee List FROM Main Database & Mark the Partial OR Not
            _formDataProvider = new FormDataProvider();
            ServiceResponse res = _formDataProvider.GetPatientEmpInfoModel();
            GetPatientEmpInfoModel data = (GetPatientEmpInfoModel)res.Data;
            model.EmployeeList = data.EmployeeList;
            model.ReferralList = data.ReferralList;

            model.ForPatient = true;
            model.ForEmployee = false;
            model.IsPartial = true;

            #endregion


            return View("FormList", model);

        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public ContentResult GetFormList(SearchFormModel model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _formDataProvider = new FormDataProvider(Constants.MyezcareOrganizationConnectionString);
            var response = _formDataProvider.GetFormList(model, pageIndex, pageSize, sortIndex, sortDirection);
            return CustJson(response);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public ContentResult GetSavedFormList(SearchFormModel model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _formDataProvider = new FormDataProvider();
            ServiceResponse res = _formDataProvider.GetSavedFormMappings(model);
            List<UDT_EBFromMappingTable> mappingData = (List<UDT_EBFromMappingTable>)res.Data;

            _formDataProvider = new FormDataProvider(Constants.MyezcareOrganizationConnectionString);
            var response = _formDataProvider.GetSavedFormList(mappingData, model, pageIndex, pageSize, sortIndex, sortDirection);
            return CustJson(response);
        }



        #endregion

        #region Organition Form Mapping Screen Page - This page Organization will use and add more forms for the Organization's Users

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public ActionResult OrganizationForms()
        {
            _formDataProvider = new FormDataProvider(Constants.MyezcareOrganizationConnectionString);
            OrganizationFormPageModel model = (OrganizationFormPageModel)_formDataProvider.SetOrganizationFormsPage().Data;
            return View(model);
        }


        [HttpGet]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public ActionResult PartialOrganizationForms()
        {
            _formDataProvider = new FormDataProvider(Constants.MyezcareOrganizationConnectionString);
            OrganizationFormPageModel model = (OrganizationFormPageModel)_formDataProvider.SetOrganizationFormsPage().Data;
            model.IsPartialPage = true;
            return View("OrganizationForms", model);
        }



        [HttpPost]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public ContentResult SaveOrganizationFormDetails(List<OrganizationFormModel> model)
        {
            _formDataProvider = new FormDataProvider(Constants.MyezcareOrganizationConnectionString);
            var response = _formDataProvider.SaveOrganizationFormDetails(model, SessionHelper.LoggedInID);
            return CustJson(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public ContentResult SaveOrganizationFormName(OrganizationForm model)
        {
            _formDataProvider = new FormDataProvider(Constants.MyezcareOrganizationConnectionString);
            var response = _formDataProvider.SaveOrganizationFormName(model, SessionHelper.LoggedInID);
            return CustJson(response);
        }


        #endregion

        #region Form Tags
        [HttpPost]
        public JsonResult SearchTag(string searchText, int pageSize)
        {
            _formDataProvider = new FormDataProvider(Constants.MyezcareOrganizationConnectionString);
            return Json(_formDataProvider.GetSearchTag(pageSize, searchText));
        }

        [HttpPost]
        public JsonResult GetOrgFormTagList(string id)
        {
            long OrganizationFormID = Convert.ToInt64(id);
            _formDataProvider = new FormDataProvider(Constants.MyezcareOrganizationConnectionString);
            var response = _formDataProvider.GetOrgFormTagList(OrganizationFormID);
            return Json(response);
        }

        [HttpPost]
        public JsonResult AddOrgFormTag(FormTagModel model)
        {
            _formDataProvider = new FormDataProvider(Constants.MyezcareOrganizationConnectionString);
            var response = _formDataProvider.AddOrgFormTag(model);
            return Json(response);
        }

        [HttpPost]
        public JsonResult DeleteFormTag(string id)
        {
            long OrganizationFormTagID = Convert.ToInt64(id);
            _formDataProvider = new FormDataProvider(Constants.MyezcareOrganizationConnectionString);
            var response = _formDataProvider.DeleteFormTag(OrganizationFormTagID);
            return Json(response);
        }
        #endregion










        #region Load Internal HTML Forms

        [HttpGet]

        public ActionResult LoadHtmlForm()
        {
            string pageID = Request.QueryString["OrgPageID"];
            if (string.IsNullOrEmpty(pageID))
                return View("Error");


            SaveNewEBFormModel obj = new SaveNewEBFormModel();
            obj.EmployeeID = Convert.ToInt64(Request.QueryString["EmployeeID"]);
            obj.ReferralID = Convert.ToInt64(Request.QueryString["ReferralID"]);
            obj.EBriggsFormID = Request.QueryString["EBriggsFormID"];
            obj.OriginalEBFormID = Request.QueryString["OriginalEBFormID"];
            obj.FormId = Request.QueryString["FormId"];
            obj.EbriggsFormMppingID = Convert.ToInt64(Request.QueryString["EbriggsFormMppingID"]);
            obj.IsEditMode = Convert.ToBoolean(Request.QueryString["IsEditMode"]);


            if (obj.EbriggsFormMppingID > 0)
            {
                _formDataProvider = new FormDataProvider();
                var response = _formDataProvider.GetSavedHtmlFormContent(obj.EbriggsFormMppingID);
                NameValueDataInString data = (NameValueDataInString)response.Data;
                obj.HTMLFormContent = data.Value;
            }
            else
            {
                string path = Request.QueryString["FormURL"];
                if (!System.IO.File.Exists(Server.MapCustomPath(path)))
                    return View("Error");
                obj.HTMLFormContent = System.IO.File.ReadAllText(Server.MapCustomPath(path));
            }

            if (obj.ReferralID > 0)
            {
                _formDataProvider = new FormDataProvider();
                var response = _formDataProvider.GetHTMLFormTokenReplaceModel(obj.ReferralID.Value);
                HTMLFormTokenReplaceModel data = (HTMLFormTokenReplaceModel)response.Data;
                obj.HTMLFormContent = TokenReplace.ReplaceTokens(obj.HTMLFormContent, data);
            }
            return View(obj);
        }



        [HttpGet]
        public ActionResult LoadPdfForm()
        {
            string pageID = Request.QueryString["OrgPageID"];
            if (string.IsNullOrEmpty(pageID))
                return View("Error");

            string path = Request.QueryString["FormURL"];
            path = Server.MapCustomPath(path);
            if (!System.IO.File.Exists(path))
                return View("Error");


            SaveNewEBFormModel obj = new SaveNewEBFormModel();
            obj.EmployeeID = Convert.ToInt64(Request.QueryString["EmployeeID"]);
            obj.ReferralID = Convert.ToInt64(Request.QueryString["ReferralID"]);
            obj.EBriggsFormID = Request.QueryString["EBriggsFormID"];
            obj.OriginalEBFormID = Request.QueryString["OriginalEBFormID"];
            obj.FormId = Request.QueryString["FormId"];
            obj.EbriggsFormMppingID = Convert.ToInt64(Request.QueryString["EbriggsFormMppingID"]);
            obj.IsEditMode = Convert.ToBoolean(Request.QueryString["IsEditMode"]);
            obj.OrgPageID = Convert.ToString(Request.QueryString["OrgPageID"]);
            obj.SubSectionID = Convert.ToInt64(Request.QueryString["SubSectionID"]) == 0 ? (long?)null : Convert.ToInt64(Request.QueryString["SubSectionID"]);
            obj.UserType = Convert.ToString(Request.QueryString["UserType"]);
            obj.InternalFilePath = path;

            SessionHelper.TempModel = JsonConvert.SerializeObject(obj);

            /*
             for javascript alert
             */

            string temporaryPath = string.Empty;
            if (obj.EbriggsFormMppingID > 0)
            {
                _formDataProvider = new FormDataProvider();
                var response = _formDataProvider.GetSavedHtmlFormContent(obj.EbriggsFormMppingID);
                NameValueDataInString data = (NameValueDataInString)response.Data;
                byte[] byteArray = Convert.FromBase64String(data.Value);
                //byte[] byteArray = System.Text.Encoding.Default.GetBytes(data.Value);
                return File(byteArray, "application/pdf");

            }
            else
            {
                temporaryPath = System.IO.Path.GetTempPath();
                long? id = 0;

                if (obj.EmployeeID > 0)
                {
                    id = obj.EmployeeID;
                }
                else
                {
                    id = obj.ReferralID;
                }

                _formDataProvider = new FormDataProvider();
                var response = _formDataProvider.GetPdfFieldsData(id, obj.ReferralID > 0 ? 1 : 2);
                List<PDFFieldMapping> data = (List<PDFFieldMapping>)response.Data;


                using (FileStream fileIn = System.IO.File.OpenRead(path))
                {
                    Document document = new Document(fileIn);

                    document = PrefilDocument(document, data);

                    string name = "Submit";

                    PushButtonField button = document.Fields[name] as PushButtonField;

                    button.Widgets[0].MouseDownActions.Add(new JavaScriptAction
                    {
                        JavaScript = new JavaScript("app.alert('Form is submitted successfully. Please close the window and continue.');")
                    });



                    //SubmitFormAction action = new SubmitFormAction();
                    //action.Url = "http://localhost:51285/pizza/success";
                    //action.SubmitFormat = SubmitFormat.Xfdf;

                    //button.Widgets[0].MouseUpActions.Add(action);


                    temporaryPath = temporaryPath + "temp_{Guid.NewGuid()}.pdf";

                    using (FileStream fileOut = System.IO.File.OpenWrite(temporaryPath))
                    {
                        document.Write(fileOut);
                    }

                }

                if (!string.IsNullOrEmpty(temporaryPath))
                {
                    var result = System.IO.File.ReadAllBytes(temporaryPath);

                    System.IO.File.Delete(temporaryPath);

                    return File(result, "application/pdf");
                }
                else
                {
                    return File(path, "application/pdf");
                }
            }
        }

        [HttpPost]
        public ActionResult LoadPdf_Form(string EbriggsFormMppingID, string EmployeeID, string ReferralID, string Form_URL, string File_Name)
        {
            string path = Form_URL;
            path = Server.MapCustomPath(path);

            SaveNewEBFormModel obj = new SaveNewEBFormModel();
            obj.EmployeeID = Convert.ToInt64(EmployeeID);
            obj.ReferralID = Convert.ToInt64(ReferralID);
            obj.EbriggsFormMppingID = Convert.ToInt64(EbriggsFormMppingID);
            obj.InternalFilePath = path;

            string temporaryPath = string.Empty;
            if (obj.EbriggsFormMppingID > 0)
            {
                _formDataProvider = new FormDataProvider();
                var response = _formDataProvider.GetSavedHtmlFormContent(obj.EbriggsFormMppingID);
                NameValueDataInString data = (NameValueDataInString)response.Data;

                var converter = new HtmlToPdf();
                converter.Options.PdfPageSize = PdfPageSize.A4;
                converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape;
                converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.ShrinkOnly;
                converter.Options.MarginLeft = 20;
                converter.Options.MarginTop = 20;
                converter.Options.MarginBottom = 20;
                converter.Options.MarginRight = 20;

                var doc = converter.ConvertHtmlString(data.Value);
                MemoryStream pdfStream = new MemoryStream();

                doc.Save(pdfStream);
                string handle = Guid.NewGuid().ToString();
                TempData[handle] = pdfStream.ToArray();
                pdfStream.Position = 0;
                doc.Close();

                return new JsonResult()
                {
                    Data = new { FileGuid = handle, FileName = File_Name + ".pdf" }
                    //return File(pdfStream.ToArray(), "application/pdf", "test");


                };


            }
            return null;
        }
        [HttpGet]
        public virtual ActionResult Download(string fileGuid, string fileName)
        {
            if (TempData[fileGuid] != null)
            {
                byte[] data = TempData[fileGuid] as byte[];
                return File(data, "application/pdf", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                return new EmptyResult();
            }
        }
        private Document PrefilDocument(Document doc, List<PDFFieldMapping> data)
        {
            var fields = doc.Fields;

            foreach (var f in data)
            {
                var result = fields.Select(t => t.FullName == f.PDFFieldName);
                TextField textField = null;
                if (result != null)
                {
                    textField = doc.Fields[f.PDFFieldName] as TextField;
                    if (textField != null)
                        textField.Value = f.DBValue;
                }
            }
            return doc;
        }


        [HttpPost]
        public ActionResult SavePdfForm()
        {


            SaveNewEBFormModel model = JsonConvert.DeserializeObject<SaveNewEBFormModel>(SessionHelper.TempModel);

            using (FileStream file = new FileStream(model.InternalFilePath, FileMode.Open, FileAccess.Read))
            {
                Document document = new Document(file);
                FormData data = FormData.Create(System.Web.HttpContext.Current.Request);
                document.Import(data);

                string fileName = Path.GetFileNameWithoutExtension(model.InternalFilePath);
                string removeFilePath = model.InternalFilePath.Replace(fileName, Guid.NewGuid().ToString());
                using (FileStream file1 = new FileStream(removeFilePath, FileMode.Append, FileAccess.Write))
                {
                    document.Write(file1);

                }
                byte[] bytes = System.IO.File.ReadAllBytes(removeFilePath);
                string byteData = Convert.ToBase64String(bytes);//System.Text.Encoding.Default.GetString(bytes);
                model.HTMLFormContent = byteData;// JsonConvert.SerializeObject(data); ;
                IReferralDataProvider _referralDataProvider = new ReferralDataProvider();

                if (model.OrgPageID == "ReferralDocument")
                {
                    if (model.EbriggsFormMppingID == 0)
                        model.EBriggsFormID = Guid.NewGuid().ToString();

                    ServiceResponse response =
                        _referralDataProvider.HC_SavedNewHtmlFormWithSubsection(model, SessionHelper.LoggedInID);
                }
                else
                {
                    ServiceResponse response = _referralDataProvider.HC_SaveNewEBForm(model, SessionHelper.LoggedInID);
                }




                if (System.IO.File.Exists(removeFilePath))
                    System.IO.File.Delete(removeFilePath);

            }







            return null;

        }
        #endregion

        #region ----- Orbeon Form -----

        [HttpGet]
        public ActionResult OrbeonLoadHtmlForm()
        {
            string pageID = Request.QueryString["OrgPageID"];
            if (string.IsNullOrEmpty(pageID))
                return View("Error");

            //string orgId = Request.QueryString["OrganizationId"];

            SaveNewEBFormModel obj = new SaveNewEBFormModel();
            obj.EmployeeID = Convert.ToInt64(Request.QueryString["EmployeeID"]);
            obj.ReferralID = Convert.ToInt64(Request.QueryString["ReferralID"]);
            obj.Version= Request.QueryString["Version"];
            obj.IsEditMode = Convert.ToBoolean(Request.QueryString["IsEditMode"]);
            //obj.FormId = Request.QueryString["FormId"];
            //string path = Request.QueryString["FormURL"];
            //obj.HTMLFormContent = "";

            return View(obj);
        }
       
        public ActionResult OrbeonGetFormUrl()
        {
            string orbeonUrl = string.Format("{0}/fr/{1}", ConfigSettings.OrbeonBaseUrl, Request.QueryString["FormURL"]);


            return Json(new { url = orbeonUrl }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> DuplicateOrbeonForm(DuplicateOrbeonForm form)
        {
            using (var client = new HttpClient())
            {
                var uri = new Uri(string.Format("{0}/fr/service{1}/duplicate", ConfigSettings.OrbeonBaseUrl, form.NameForUrl));
                var doc = new DuplicateOrbeonFormDocument() { DocumentID = form.DocumentID };
                var httpContent = new StringContent(Common.SerializeXML(doc), Encoding.UTF8, "application/xml");
                var task = client.PostAsync(uri, httpContent);
                var response = task.Result;
                var serRes = new ServiceResponse();
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    serRes.Data = Common.DeserializeXML<DuplicateOrbeonFormDocument>(result);
                    serRes.IsSuccess = true;
                }
                else
                {
                    serRes.Message = Resources.Resource.ErrorOccured;
                }
                return Json(serRes);
            }
        }

        #endregion


        //[HttpGet]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_FacilityHouse_AddUpdate)]
        public ActionResult AddForm()
        {
            //var response = _formDataProvider.HC_SetAddFacilityHousePage(facilityId);
            //return ShowUserFriendlyPages(response) ?? View(response.Data);
            return View(new FormListModel());
        }       
    }

}
