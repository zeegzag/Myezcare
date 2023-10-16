using System;
using System.Collections.Generic;
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
    public class OnboardingController : BaseController
    {
        private IOnboardinDataProvider _onboardinDataProvider;
        private ISettingDataProvider _settingDataProvider;
        private IGeneralMasterDataProvider _ddMasterDataProvider;
        private IServiceCodeDataProvider _iServiceCodeDataProvider;
        private IPayorDataProvider _iPayorDataProvider;
        private IVisitTaskDataProvider _visitQuestionDataProvider;
        public OnboardingController()
        {
            _onboardinDataProvider = new OnboardinDataProvider();
            _settingDataProvider = new SettingDataProvider();
            _ddMasterDataProvider = new GeneralMasterDataProvider();
            _iServiceCodeDataProvider = new ServiceCoceDataProvider();
            _iPayorDataProvider = new PayorDataProvider();
            _visitQuestionDataProvider = new VisitTaskDataProvider();
        }

        [HttpGet]
        public ActionResult GetStarted(string id)
        {
            return View(new OnboardingModel()
            {
                CurrentStep = !string.IsNullOrEmpty(id) ? id : "Organization Details",
                ActiveSteps = _onboardinDataProvider.GetWizardStatus(SessionHelper.OrganizationId)
                .Where(x => x.IsCompleted == true)
                .Select(x => x.Menu).ToList()
            });
        }

        #region Organization
        /// <summary>
        /// This method will return setting page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult OrganizationDetails()
        {
            var data = _settingDataProvider.GetSettings().Data;
            return PartialView("Partial/_OrganizationDetails", data);
        }

        /// <summary>
        /// This method will return setting page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult BillingSettings()
        {
            var data = _settingDataProvider.GetSettings().Data;
            return PartialView("Partial/_BillingSettings", data);
        }
        #endregion

        #region General Master Code
        /// <summary>
        /// This method will return setting page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult VisitType()
        {
            int ddTypeId = (int)Common.DDType.VisitType; //VisitType
            _ddMasterDataProvider = new GeneralMasterDataProvider();
            ServiceResponse response = _ddMasterDataProvider.AddGeneralMaster(ddTypeId);
            return PartialView("Partial/SVCCode/_VisitType", response.Data);
        }
        /// <summary>
        /// This method will return setting page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult CareType()
        {
            int ddTypeId = (int)Common.DDType.CareType; //CareType
            _ddMasterDataProvider = new GeneralMasterDataProvider();
            ServiceResponse response = _ddMasterDataProvider.AddGeneralMaster(ddTypeId);
            return PartialView("Partial/SVCCode/_CareType", response.Data);
        }

        /// <summary>
        /// This method will return setting page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult NPIOption()
        {
            int ddTypeId = (int)Common.DDType.NPIOptions; //NPIOption
            _ddMasterDataProvider = new GeneralMasterDataProvider();
            ServiceResponse response = _ddMasterDataProvider.AddGeneralMaster(ddTypeId);
            return PartialView("Partial/Payor/Partial/GeneralMaster/_NPIOption", response.Data);
        }
        /// <summary>
        /// This method will return setting page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult PayorGroup()
        {
            int ddTypeId = (int)Common.DDType.PayerGroup; //PayorGroup
            _ddMasterDataProvider = new GeneralMasterDataProvider();
            ServiceResponse response = _ddMasterDataProvider.AddGeneralMaster(ddTypeId);
            return PartialView("Partial/Payor/Partial/GeneralMaster/_PayorGroup", response.Data);
        }
        /// <summary>
        /// This method will return setting page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult BusinessLine()
        {
            int ddTypeId = (int)Common.DDType.BussinessLine; //BusinessLine
            _ddMasterDataProvider = new GeneralMasterDataProvider();
            ServiceResponse response = _ddMasterDataProvider.AddGeneralMaster(ddTypeId);
            return PartialView("Partial/Payor/Partial/GeneralMaster/_BusinessLine", response.Data);
        }
        #endregion

        #region Service Code
        /// <summary>
        /// This method will return setting page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult ServiceCodeList()
        {
            _ddMasterDataProvider = new GeneralMasterDataProvider();
            _iServiceCodeDataProvider = new ServiceCoceDataProvider();
            var response = _iServiceCodeDataProvider.HC_SetServiceCodeList();
            return PartialView("Partial/_ServiceCodeList", response.Data);
        }
        /// <summary>
        /// This method will return setting page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult AddServiceCode(string id)
        {
            _iServiceCodeDataProvider = new ServiceCoceDataProvider();
            long serviceCodeId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            ServiceResponse response = _iServiceCodeDataProvider.HC_SetServiceCode(serviceCodeId);
            return PartialView("Partial/_AddServiceCode", response.Data);
        }
        #endregion

        #region Payor Start
        /// <summary>
        /// This method will return setting page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult PayorList()
        {
            _iPayorDataProvider = new PayorDataProvider();
            var response = _iPayorDataProvider.HC_SetPayorListPage();
            return PartialView("Partial/Payor/_PayorList", response.Data);
        }
        /// <summary>
        /// This method will return setting page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult AddPayor(string id)
        {
            _iPayorDataProvider = new PayorDataProvider();
            long payorId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            ServiceResponse response = _iPayorDataProvider.HC_SetAddPayorPage(payorId);
            return PartialView("Partial/Payor/_AddPayor", response.Data);
        }
        #endregion

        #region Visit Task
        /// <summary>
        /// This method will return setting page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult VisitTaskList()
        {
            _iPayorDataProvider = new PayorDataProvider();
            var response = _visitQuestionDataProvider.SetVisitTaskListPage();
            return PartialView("Partial/VisitTask/_VisitTaskList", response.Data);
        }
        /// <summary>
        /// This method will return setting page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult AddVisitTask(string id)
        {
            long visitTaskId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _visitQuestionDataProvider = new VisitTaskDataProvider();
            ServiceResponse response = _visitQuestionDataProvider.AddVisitTask(visitTaskId);
            return PartialView("Partial/VisitTask/_AddVisitTask", response.Data);
        }
        #endregion

        #region Navigation Methods
        [HttpGet]
        public ActionResult MoveNext(string id)
        {
            string current = id;
            OnboardingModel model = new OnboardingModel();
            if (SetWizardStatus(current, true))
            {
                if (current == "Add Visit Task")
                    return RedirectToAction("dashboard", "home", new { area = "HomeCare" });

                return RedirectToAction("GetStarted", new { id = model.Steps[model.Steps.IndexOf(current) + 1] });
            }
            else
                return RedirectToAction("GetStarted", new { id = current });
        }

        public ActionResult MovePrevious(string current)
        {
            OnboardingModel model = new OnboardingModel();
            //List<string> keys = new List<string>(model.Wizard.Keys);
            //var newKey = keys.IndexOf(current) - 1;
            int index = model.Steps.IndexOf(current);
            string strPre = model.Steps[index - 1];
            return RedirectToAction("getstarted", new { id = strPre });
        }

        private bool SetWizardStatus(string menu, bool flag)
        {
            if (SessionHelper.OrganizationId == 0)
                return false;
            _onboardinDataProvider = new OnboardinDataProvider();
            var response = _onboardinDataProvider.SetWizardStatus(SessionHelper.OrganizationId, menu, flag, SessionHelper.LoggedInID);
            return response.IsSuccess;
        }
        #endregion
    }
}
