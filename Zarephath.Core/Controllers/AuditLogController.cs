using System;
using System.Web;
using System.Web.Mvc;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Controllers
{
    [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
    public class AuditLogController : BaseController
    {
        private IAuditLogDataProvider _iAuditLogDataProvider;

        [HttpPost]
        public JsonResult GetTableDisplayValue(AuditDeltaModel model)
        {
            _iAuditLogDataProvider = new AuditLogDataProvider();
            ServiceResponse response = _iAuditLogDataProvider.GetTableDisplayValue(model);
            return JsonSerializer(response);
        }

    }
}
