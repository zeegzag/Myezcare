using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using TallComponents.PDF.Forms.Data;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;
using Zarephath.Core.Models.ViewModel;
using System.IO;
using TallComponents.PDF;
using TallComponents.PDF.Shapes;
using TallComponents.PDF.Fonts;

namespace Zarephath.Core.Controllers
{
    public class PizzaController : BaseController
    {
        [HttpPost]
        public ActionResult Order3()
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


            Document document02 = new Document();
            Page page = new Page(PageSize.Letter);
            document02.Pages.Add(page);

            double margin = 72; // points
            MultilineTextShape text = new MultilineTextShape(
                margin, page.Height - margin, page.Width - 2 * margin);
            page.Overlay.Add(text);
            Fragment fragment = new Fragment(
                string.Format("Hi thanks for ordering a pizza!"),
                Font.Helvetica,
                16);
            text.Fragments.Add(fragment);

            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-disposition", "inline; filename=thankyou.pdf");
            document02.Write(Response.OutputStream);

            return null;
        }


    }
}
