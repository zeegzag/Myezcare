using System;
using System.Linq;
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
    public class OrgPreferenceController : BaseController
    {
        private IOrgPreferenceDataProvider _dataProvider;

        [HttpGet]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Organization_Preference)]
        public ActionResult Preference()//string id)
        {
            //long preferenceID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;

            CacheHelper_MyezCare ch_MyezcareOrg = new CacheHelper_MyezCare();
            MyEzcareOrganization myOrg = ch_MyezcareOrg.GetCachedData<MyEzcareOrganization>();
            long OrgID = Convert.ToInt64(myOrg.OrganizationID);

            _dataProvider = new OrgPreferenceDataProvider();
            ServiceResponse response = _dataProvider.GetPreference(OrgID);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Organization_Preference)]
        public JsonResult Preference(OrganizationPreference preference)
        {
            if (preference != null && preference.OrganizationID <= 0)
            {
                CacheHelper_MyezCare ch_MyezcareOrg = new CacheHelper_MyezCare();
                MyEzcareOrganization myOrg = ch_MyezcareOrg.GetCachedData<MyEzcareOrganization>();
                preference.OrganizationID = Convert.ToInt64(myOrg.OrganizationID);
            }
            _dataProvider = new OrgPreferenceDataProvider();
            return Json(_dataProvider.SavePreference(preference, SessionHelper.LoggedInID));
        }

        [HttpGet]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Organization_Preference)]
        public string PreferenceDateFormat()
        {
            long OrgID = 0;
            CacheHelper_MyezCare ch_MyezcareOrg = new CacheHelper_MyezCare();
            MyEzcareOrganization myOrg = ch_MyezcareOrg.GetCachedData<MyEzcareOrganization>();
            if (myOrg != null)
            {
                OrgID = Convert.ToInt64(myOrg.OrganizationID);
            }
            _dataProvider = new OrgPreferenceDataProvider();
            ServiceResponse response = _dataProvider.GetPreference(OrgID);
            if (response.Data != null)
            {
                return (response.Data as OrgPreferenceModel).OrganizationPreference.DateFormat;
            }
            else
            {
                return null;
            }

        }

        [HttpGet]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Organization_Preference)]
        public string PreferenceCurrencyFormat()
        {
            long OrgID = 0;
            CacheHelper_MyezCare ch_MyezcareOrg = new CacheHelper_MyezCare();
            MyEzcareOrganization myOrg = ch_MyezcareOrg.GetCachedData<MyEzcareOrganization>();
            if (myOrg != null)
            {
                OrgID = Convert.ToInt64(myOrg.OrganizationID);
            }
            _dataProvider = new OrgPreferenceDataProvider();
            ServiceResponse response = _dataProvider.GetPreference(OrgID);
            if (response.Data != null)
            {
                var selectedcurrency = (response.Data as OrgPreferenceModel).CurrencyList.Where(a => a.CurrencyID == Convert.ToInt32((response.Data as OrgPreferenceModel).OrganizationPreference.Currency)).FirstOrDefault();
                if (selectedcurrency != null)
                    return selectedcurrency.Symbol;
                else
                    return null;

            }
            else
            {
                return null;
            }

        }

    }
}
