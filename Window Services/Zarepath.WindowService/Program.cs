using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
namespace Zarepath.WindowServices
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new RespiteHoursReset(),
                new Edi835FileProcess(),
                new DeleteEDIFileLog(),
                
                new ScheduleNotification() ,
                new ScheduleBatchServices(),

                
                new SendMissingDocumentEmail(),
                new CM_Attendance_Service(), 
                new CM_ServicePlan_Service()
               
            };
            ServiceBase.Run(ServicesToRun);

        }
    }
}
