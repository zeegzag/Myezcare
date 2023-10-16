using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Zarephath.Core.Controllers;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;
using Zarephath.Core.Models.ViewModel;


namespace Zarephath.Core.Areas.HomeCare.Controllers
{
    public class CronJobController : BaseController
    {
        ICronJobDataProvider _icronJobDataProvider;
        public ActionResult SyncClaimMessages(string id)
        {
            _icronJobDataProvider = new CronJobDataProvider();
            _icronJobDataProvider.SyncClaimMessages(id);
            return null;
        }

        public ActionResult GenerateEmployeeTimeSchedule()
        {
            _icronJobDataProvider = new CronJobDataProvider();
            _icronJobDataProvider.GenerateEmployeeTimeSchedule();
            return null;
        }


        [HttpPost]
        public JsonResult GeneratePatientTimeSchedule(DayCareTimeSlotModal modal)
        {
            _icronJobDataProvider = new CronJobDataProvider();
            ServiceResponse response=new ServiceResponse();

            if (SessionHelper.IsDayCare)
            {
                modal.GeneratePatientSchedules = true;
                response=_icronJobDataProvider.GeneratePatientTimeSchedule_ForDayCarePatient(modal,
                    ConfigSettings.GenerateEmpRefTimeScheduleDays, SessionHelper.LoggedInID);
            }
            else
            {
                response=_icronJobDataProvider.GeneratePatientTimeSchedule(ConfigSettings.GenerateEmpRefTimeScheduleDays);
            }
            return JsonSerializer(response);
        }



        public ActionResult GeneratePatientTimeSchedule()
        {
            _icronJobDataProvider = new CronJobDataProvider();
            _icronJobDataProvider.GeneratePatientTimeSchedule(ConfigSettings.GenerateEmpRefTimeScheduleDays);
            return null;
        }

        public ActionResult GenerateExtraPatientTimeSchedule()
        {
            _icronJobDataProvider = new CronJobDataProvider();
            _icronJobDataProvider.GeneratePatientTimeSchedule(ConfigSettings.GenerateEmpRefTimeScheduleDaysForNextMonths);
            return null;
        }

        public ActionResult GenerateBulkSchedules()
        {
            _icronJobDataProvider = new CronJobDataProvider();
            _icronJobDataProvider.GenerateBulkSchedules();
            return null;
        }

        public ActionResult UpdateGeoCode()
        {
            _icronJobDataProvider = new CronJobDataProvider();
            _icronJobDataProvider.UpdateGeoCode();
            return null;
        }





        public ActionResult Get_Download_Process_AllERA()
        {
            _icronJobDataProvider = new CronJobDataProvider();
            CronJobServiceModel model = new CronJobServiceModel();
            model.ServiceURL = Constants.HC_CronJob_Download_Process_AllERA_URL;
            model.ServiceProgressURL = Constants.HC_CronJob_Download_Process_AllERA_ProcessUpdate_URL;
            return View("CronJobMessage", model);

        }

        public delegate ServiceResponse  Run(bool IsThreadCall);
        public JsonResult Download_Process_AllERA() {

            _icronJobDataProvider = new CronJobDataProvider();
           // _icronJobDataProvider.Download_Process_AllERA();
            //Run run = new Run(_icronJobDataProvider.Download_Process_AllERA);
            
            //IAsyncResult res = run.BeginInvoke(true,(IAsyncResult ar) =>
            //{
               
            //}, null);

            ServiceResponse response = _icronJobDataProvider.Download_Process_AllERA();
            return JsonSerializer(response);

        }
        public JsonResult Download_Process_AllERA_ProcessUpdate()
        {

            _icronJobDataProvider = new CronJobDataProvider();
            ServiceResponse response = new ServiceResponse();
            response.IsSuccess = true;
            response.Data =(CronJobServiceProgressModel) SessionHelper.SessionObj[Constants.HC_CronJob_Download_Process_AllERA_ProcessName];
            return JsonSerializer(response);

        }


        public JsonResult DownloadProcessAllERA()
        {

            _icronJobDataProvider = new CronJobDataProvider();
            ServiceResponse response =_icronJobDataProvider.Download_Process_AllERA(false);
            //return JsonSerializer(response,);
            return Json(response, JsonRequestBehavior.AllowGet);

        }
    }
}
