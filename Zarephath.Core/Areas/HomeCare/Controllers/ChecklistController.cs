using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Controllers;

namespace Zarephath.Core.Areas.HomeCare.Controllers
{
    public class ChecklistController : BaseController
    {
        private IChecklistDataProvider _iChecklistDataProvider;

        [HttpPost]
        public JsonResult GetChecklistItemTypes()
        {
            _iChecklistDataProvider = new ChecklistDataProvider();
            return Json(_iChecklistDataProvider.GetChecklistItemTypes());
        }

        [HttpPost]
        public JsonResult GetChecklistItems(ChecklistItemModel model)
        {
            _iChecklistDataProvider = new ChecklistDataProvider();
            return Json(_iChecklistDataProvider.GetChecklistItems(model));
        }

        [HttpPost]
        public JsonResult SaveChecklistItems(SaveChecklistItemModel model)
        {
            _iChecklistDataProvider = new ChecklistDataProvider();
            return Json(_iChecklistDataProvider.SaveChecklistItems(model, SessionHelper.LoggedInID));
        }

        [HttpPost]
        public JsonResult GetIsChecklistRemaining(ChecklistItemModel model)
        {
            _iChecklistDataProvider = new ChecklistDataProvider();
            return Json(_iChecklistDataProvider.GetIsChecklistRemaining(model));
        }

        [HttpPost]
        public JsonResult GetVisitChecklistItems(ChecklistItemModel model)
        {
            _iChecklistDataProvider = new ChecklistDataProvider();
            return Json(_iChecklistDataProvider.GetVisitChecklistItems(model));
        }

        [HttpPost]
        public JsonResult GetVisitChecklistItemDetail(VisitChecklistItemModel model)
        {
            _iChecklistDataProvider = new ChecklistDataProvider();
            return Json(_iChecklistDataProvider.GetVisitChecklistItemDetail(model));
        }
    }
}
