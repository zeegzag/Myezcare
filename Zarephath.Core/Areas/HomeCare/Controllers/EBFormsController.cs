using System;
using System.IO;
using System.Web.Mvc;
using Zarephath.Core.Controllers;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Areas.HomeCare.Controllers
{
    public class EBFormsController : BaseController
    {
        private IEBFormsDataProvider _ebformsDataProvider;

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public ActionResult EBFormslist()
        {
            _ebformsDataProvider = new EBFormsDataProvider(Constants.MyezcareOrganizationConnectionString);
            return View(_ebformsDataProvider.SetEBFormsListPage().Data);
        }

        //[HttpGet]
        //// [CustomAuthorize(Permissions = Constants.HC_Permission_Physician_AddUpdate)]
        //public ActionResult AddEBForms(string id)
        //{

        //    string FormId = !string.IsNullOrEmpty(id) ? Crypto.Decrypt(id) : "0";
        //    _ebformsDataProvider = new EBFormsDataProvider(Constants.MyezcareOrganizationConnectionString);
        //    ServiceResponse response = _ebformsDataProvider.AddEBForms(FormId, 0);
        //    return ShowUserFriendlyPages(response) ?? View(response.Data);
        //}
        public byte[] StrToByteArray(string str)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            return encoding.GetBytes(str);
        }
        public string GetPDFFile(string FilePath, string FileName)
        {
            int v_length = 0;
            string v_file_name = "";
            byte[] v_file_data = null;

            try
            {

                //Declare Web Service Variables


                v_file_data = StrToByteArray(FilePath);
                v_file_name = FileName;
                v_length = Convert.ToInt32(v_file_data.Length);
                FileInfo fi = new FileInfo(FileName);
                string strFileExtension = fi.Extension;

                // Load the file in the Memory Stream
                MemoryStream ms = new MemoryStream(v_file_data, 0, v_file_data.Length);
                ms.Write(v_file_data, 0, v_file_data.Length);
                // ~/assets/include/internalForms/1147_Form_DHS.pdf
                // Open the file stream in ordre to save on the local disk
                string path = Server.MapCustomPath("~/assets/include/internalForms/").ToString();
                

                string filenamenew = v_file_name.Replace(strFileExtension, "_1") + strFileExtension;
                FileStream fs = new FileStream(path + "/" + filenamenew  , FileMode.OpenOrCreate, FileAccess.Write);
                //   FileStream fs = File.OpenWrite(path + "/" + v_file_name + "_1");
                fs.Write(v_file_data, 0, v_file_data.Length);
                fs.Close();

                // Return True if no errors occured
                return "~/assets/include/internalForms/" + filenamenew;



            }

            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
                //return "~/images/noPhoto.gif";
            }
        }

        //[HttpPost]
        //// [CustomAuthorize(Permissions = Constants.HC_Permission_EBCategory_AddUpdate)]
        //public JsonResult AddEBForms(EBForms forms)
        //{
        //    _ebformsDataProvider = new EBFormsDataProvider(Constants.MyezcareOrganizationConnectionString);
        //    int ISiNSuP = 0;
        //    if (Convert.ToInt32(forms.Id) > 0)
        //        ISiNSuP = Convert.ToInt32(forms.Id);

        //    forms.InternalFormPath = GetPDFFile(forms.FilePath, forms.FileName);
        //    forms.NewPdfURI = forms.InternalFormPath;
        //    forms.IsInternalForm = forms.IsInternalForm;
        //    return Json(_ebformsDataProvider.AddEBForms(forms, ISiNSuP, SessionHelper.LoggedInID));
        //}


        [HttpPost]
        //  [CustomAuthorize(Permissions = Constants.HC_Permission_EBCategory_List)]SearchEBMarketListPage
        public ContentResult EBFormsList(SearchEBFormsListPage SearchEBFormsListPage, int pageIndex = 1,
                                             int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _ebformsDataProvider = new EBFormsDataProvider(Constants.MyezcareOrganizationConnectionString);
            return CustJson(_ebformsDataProvider.GetEBFormsList(SearchEBFormsListPage, pageIndex, pageSize, sortIndex, sortDirection));
        }

        [HttpPost]
        // [CustomAuthorize(Permissions = Constants.HC_Permission_EBCategory_Delete)]
        public ContentResult DeleteEBForms(SearchEBFormsListPage SearchEBFormsListPage, int pageSize = 10, int pageIndex = 1, string sortIndex = "", string sortDirection = "")
        {
            _ebformsDataProvider = new EBFormsDataProvider(Constants.MyezcareOrganizationConnectionString);
            return CustJson(_ebformsDataProvider.DeleteEBForms(SearchEBFormsListPage, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID));
        }

        //[HttpPost]
        //public JsonResult GetPhysicianListForAutoComplete(string searchText, int pageSize)
        //{
        //    _physicianDataProvider = new PhysicianDataProvider();
        //    return Json(_physicianDataProvider.HC_GetPhysicianListForAutoComplete(searchText, pageSize));
        //}
    }
}
