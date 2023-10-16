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
using Zarephath.Core.Models.Entity;
using System.Net.Http;
using Newtonsoft.Json;

namespace Zarephath.Core.Areas.HomeCare.Controllers
{
    public class UIKitController : BaseController
    {
        private IAgencyDataProvider _iagencyDataProvider;

        #region UI-KIT

        [HttpGet]
        public ActionResult UIKit(string id)
        {
            return View();
        }
       

        #endregion

    }
}
