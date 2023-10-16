using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Zarephath.Core.Controllers;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Areas.HomeCare.Controllers
{
    public class MedicationController : BaseController
    {
        private readonly IMedicationDataProvider _IMedicationDataProvider = null;

        public MedicationController()
        {
            //_IMedicationDataProvider = new MedicationDataProvider();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddMedication()
        {
            //_dxCodeDataProvider = new DxCodeDataProvider();
            //return Json(_dxCodeDataProvider.HC_AddDxCode(addDxCodeModel, SessionHelper.LoggedInID));
            return View();
        }
    }
}
