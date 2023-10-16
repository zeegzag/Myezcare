using Newtonsoft.Json;
using PdfFormApp.Core;
using PdfFormApp.Core.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TallComponents.PDF;
using TallComponents.PDF.Actions;
using TallComponents.PDF.Forms.Data;
using TallComponents.PDF.Forms.Fields;
using TallComponents.PDF.JavaScript;

namespace PdfFormApp.Controllers
{
    public class PdfFormController : Controller
    {
        #region Properties
        private enum DataType { Referral = 1, Employee = 2 };
        public string OrgId
        {
            get
            {
                string orgId = string.Empty;
                if (Request.QueryString["OrganizationId"] != null)
                {
                    orgId = Request.QueryString["OrganizationId"];
                    TempData["OrganizationId"] = orgId;
                }
                else if(TempData["OrganizationId"] != null)
                {
                    orgId = TempData["OrganizationId"].ToString();
                }
                return orgId;
            }
        }

        public string FormId
        {
            get
            {
                string ebFormId = string.Empty;
                if (Request.QueryString["FormId"] != null)
                {
                    ebFormId = Request.QueryString["FormId"];
                    TempData["FormId"] = ebFormId;
                }
                else if (TempData["FormId"] != null)
                {
                    ebFormId = TempData["FormId"].ToString();
                }
                return ebFormId;
            }
        }

        public int TypeId
        {
            get
            {
                int typeId = 1;
                if(ReferralId > 0)
                {
                    typeId = 2;
                }
                return typeId;
            }
        }
        public long FormDataId
        {
            get
            {
                long id = -1;
                if (Request.QueryString["FormDataId"] != null)
                {
                    id = Convert.ToInt64(Request.QueryString["FormDataId"]);
                }
                return id;
            }
        }


        public int UserId
        {
            get
            {
                int id =0;
                if (Request.QueryString["UserId"] != null)
                {
                    id = Convert.ToInt32(Request.QueryString["UserId"]);
                    TempData["UserId"] = id;
                }
                else if (TempData["UserId"] != null)
                {
                    id = Convert.ToInt32(TempData["UserId"].ToString());
                }
                return id;
            }
        }

        

        public int ReferralId
        {
            get
            {
                int _ReferralId = 0;
                if (Request.QueryString["ReferralId"] != null)
                {
                    _ReferralId = Convert.ToInt32(Request.QueryString["ReferralId"]);
                    TempData["ReferralId"] = _ReferralId;
                }
                else if (TempData["ReferralId"] != null)
                {
                    _ReferralId = Convert.ToInt32(TempData["ReferralId"].ToString());
                }
                return _ReferralId;
            }
        }

        public int EmployeeID
        {
            get
            {
                int _EmployeeId = 0;
                if (Request.QueryString["EmployeeID"] != null)
                {
                    _EmployeeId = Convert.ToInt32(Request.QueryString["EmployeeID"]);
                    TempData["EmployeeID"] = _EmployeeId;
                }
                else if (TempData["EmployeeID"] != null)
                {
                    _EmployeeId = Convert.ToInt32(TempData["EmployeeID"].ToString());
                }
                return _EmployeeId;
            }
        }

        public string EBriggsFormID
        {
            get
            {
                string _EBriggsFormID = string.Empty;
                if (Request.QueryString["EBriggsFormID"] != null)
                {
                    _EBriggsFormID = Request.QueryString["EBriggsFormID"];
                    TempData["EBriggsFormID"] = _EBriggsFormID;
                }
                else if (TempData["EBriggsFormID"] != null)
                {
                    _EBriggsFormID = TempData["EBriggsFormID"].ToString();
                }
                return _EBriggsFormID;
            }
        }

        public string OriginalEBFormID
        {
            get
            {
                string _EBriggsFormID = string.Empty;
                if (Request.QueryString["OriginalEBFormID"] != null)
                {
                    _EBriggsFormID = Request.QueryString["OriginalEBFormID"];
                    TempData["OriginalEBFormID"] = _EBriggsFormID;
                }
                else if (TempData["OriginalEBFormID"] != null)
                {
                    _EBriggsFormID = TempData["OriginalEBFormID"].ToString();
                }
                return _EBriggsFormID;
            }
        }



        //@SubSectionID
        //    @FormName
        //@UpdateFormName

        public string SubSectionID
        {
            get
            {
                string _SubSectionID = string.Empty;
                if (Request.QueryString["SubSectionID"] != null)
                {
                    _SubSectionID = Request.QueryString["SubSectionID"];
                    TempData["SubSectionID"] = _SubSectionID;
                }
                else if (TempData["SubSectionID"] != null)
                {
                    _SubSectionID = TempData["SubSectionID"].ToString();
                }
                return _SubSectionID;
            }
        }

        public string FormName
        {
            get
            {
                string _FormName = string.Empty;
                if (Request.QueryString["FormName"] != null)
                {
                    _FormName = Request.QueryString["FormName"];
                    TempData["FormName"] = _FormName;
                }
                else if (TempData["FormName"] != null)
                {
                    _FormName = TempData["FormName"].ToString();
                }
                return _FormName;
            }
        }

        public string UpdateFormName
        {
            get
            {
                string _UpdateFormName = string.Empty;
                if (Request.QueryString["UpdateFormName"] != null)
                {
                    _UpdateFormName = Request.QueryString["UpdateFormName"];
                    TempData["UpdateFormName"] = _UpdateFormName;
                }
                else if (TempData["UpdateFormName"] != null)
                {
                    _UpdateFormName = TempData["UpdateFormName"].ToString();
                }
                return _UpdateFormName;
            }
        }



        public long EbriggsFormMppingID
        {
            get
            {
                long _EbriggsFormMppingID = 0;
                if (Request.QueryString["EbriggsFormMppingID"] != null)
                {
                    _EbriggsFormMppingID = Convert.ToInt64(Request.QueryString["EbriggsFormMppingID"]);
                    TempData["EbriggsFormMppingID"] = _EbriggsFormMppingID;
                }
                else if (TempData["EbriggsFormMppingID"] != null)
                {
                    _EbriggsFormMppingID = Convert.ToInt64(TempData["EbriggsFormMppingID"].ToString());
                }
                return _EbriggsFormMppingID;
            }
        }


        #endregion


        // GET: PdfForm
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SavePdfForm()
        {
            Utility utility = new Utility();
            string localFilePath = utility.GetPdfFormPath(FormId);
            localFilePath = Server.MapPath(localFilePath);
            string temporaryPath = System.IO.Path.GetTempPath();
            temporaryPath += Guid.NewGuid().ToString() + ".pdf";

            using (FileStream file = new FileStream(localFilePath, FileMode.Open, FileAccess.Read))
            {

                Document document = new Document(file);
                FormData data = FormData.Create(System.Web.HttpContext.Current.Request);
                document.Import(data);

                using (FileStream file1 = new FileStream(temporaryPath, FileMode.Append, FileAccess.Write))
                {
                    document.Write(file1);

                }
                byte[] bytes = System.IO.File.ReadAllBytes(temporaryPath);
                string byteData = Convert.ToBase64String(bytes);//System.Text.Encoding.Default.GetString(bytes);


                if (System.IO.File.Exists(temporaryPath))
                    System.IO.File.Delete(temporaryPath);

                FormDataEntity formData = new FormDataEntity();
                formData.FormId = FormId;
                formData.OrganizationId = OrgId;
                formData.FormData = byteData;
                formData.TypeId = TypeId;

                formData.ReferralId = ReferralId;
                formData.EmployeeId = EmployeeID;
                formData.UserId = UserId;
                //if( !string.IsNullOrEmpty(EBriggsFormID) && Convert.ToInt32(EBriggsFormID)>0)
                //{
                //    formData.EBriggsFormID = EBriggsFormID;
                //}
                //else
                //    formData.EBriggsFormID = OriginalEBFormID;

                formData.FormUniqueId = string.Empty;


                formData.EBriggsFormID = EBriggsFormID;
                formData.OriginalEBFormID = OriginalEBFormID;

                formData.SubSectionID = SubSectionID;
                formData.FormName = FormName;
                formData.UpdateFormName = UpdateFormName;
                formData.EbriggsFormMppingID = EbriggsFormMppingID;
                

                utility.EditFormData(formData);
            }
            return null;

        }

        [HttpGet]
        public ActionResult LoadPdfForm()
        {
            InitializeQuerystring();
            bool isEdit = EbriggsFormMppingID > 0;
            string temporaryPath = string.Empty;

            if (!isEdit)
            {
                Utility utility = new Utility();
                string localFilePath = utility.GetPdfFormPath(FormId);

                if (!string.IsNullOrEmpty(localFilePath))
                {
                    localFilePath = Server.MapPath(localFilePath);
                }
                temporaryPath = System.IO.Path.GetTempPath();

                if (System.IO.File.Exists(localFilePath))
                {
                    using (FileStream fileIn = System.IO.File.OpenRead(localFilePath))
                    {
                        ICollection<PDFFieldMapping> data = utility.GetPdfFieldsData(ReferralId, TypeId,Convert.ToInt32(OrgId));
                        Document document = new Document(fileIn);

                        document = PrefilDocument(document, data);

                        string name = "Submit";

                        PushButtonField button = document.Fields[name] as PushButtonField;

                        button.Widgets[0].MouseDownActions.Add(new JavaScriptAction
                        {
                            JavaScript = new JavaScript("app.alert('Form is submitted successfully. Please close the window and continue.');")
                        });


                        

                        //temporaryPath = temporaryPath + $"temp_{Guid.NewGuid()}.pdf";
                        temporaryPath = temporaryPath + string.Format("temp_{0}.pdf",Guid.NewGuid());

                        using (FileStream fileOut = System.IO.File.OpenWrite(temporaryPath))
                        {
                            document.Write(fileOut);
                        }

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
                    return File(localFilePath, "application/pdf");
                }
            }
            else
            {
                Utility utility = new Utility();
                string pdfFormData = utility.GetPdfFormData(EBriggsFormID, OrgId);
                byte[] byteArray = Convert.FromBase64String(pdfFormData);

                temporaryPath = System.IO.Path.GetTempPath();

                Stream stream = new MemoryStream(byteArray);

                Document document = new Document(stream);

                string name = "Submit";

                PushButtonField button = document.Fields[name] as PushButtonField;

                button.Widgets[0].MouseDownActions.Add(new JavaScriptAction
                {
                    JavaScript = new JavaScript("app.alert('Form is submitted successfully. Please close the window and continue.');")
                });

                //temporaryPath = temporaryPath + $"temp_{Guid.NewGuid()}.pdf";
                temporaryPath = temporaryPath + string.Format("temp_{0}.pdf", Guid.NewGuid());

                using (FileStream fileOut = System.IO.File.OpenWrite(temporaryPath))
                {
                    document.Write(fileOut);
                }

                if (!string.IsNullOrEmpty(temporaryPath))
                {
                    var result = System.IO.File.ReadAllBytes(temporaryPath);

                    System.IO.File.Delete(temporaryPath);

                    return File(result, "application/pdf");
                }

                return File(byteArray, "application/pdf");
            }
        }
        private Document PrefilDocument(Document doc, ICollection<PDFFieldMapping> data)
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


        private void InitializeQuerystring()
        {
            var orgId = OrgId;
            var frmId = FormId;
            var usrId = UserId;
            var ebMId = EbriggsFormMppingID;
            var rfrlId = ReferralId;
            var empId = EmployeeID;
            var ebrgFormId = EBriggsFormID;
            var orgEBFormId = OriginalEBFormID;

            var subSectionID = SubSectionID;
            var formName = FormName;
            var updateFormName = UpdateFormName;
            
        }
    }
}