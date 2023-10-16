using System.Configuration;
using Zarephath.Core.Infrastructure;

namespace Zarepath.WindowServices
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.WindowServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.ScheduleNotification = new System.ServiceProcess.ServiceInstaller();
            this.RespiteHoursReset = new System.ServiceProcess.ServiceInstaller();
            this.Edi835FileProcess = new System.ServiceProcess.ServiceInstaller();
            this.DeleteEDIFileLog = new System.ServiceProcess.ServiceInstaller();
            this.ScheduleBatchServices = new System.ServiceProcess.ServiceInstaller();
            this.SendMissingDocumentEmail = new System.ServiceProcess.ServiceInstaller();
            this.cM_Attendance_Service1 = new System.ServiceProcess.ServiceInstaller();
            this.cM_ServicePlan_Service1 = new System.ServiceProcess.ServiceInstaller();
            // 
            // WindowServiceProcessInstaller
            // 
            this.WindowServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalService;
            this.WindowServiceProcessInstaller.Password = null;
            this.WindowServiceProcessInstaller.Username = null;
            // 
            // ScheduleNotification
            // 
            this.ScheduleNotification.ServiceName = "ZP_Live_ScheduleNotification";
            // 
            // RespiteHoursReset
            // 
            this.RespiteHoursReset.ServiceName = "ZP_Live_RespiteHoursReset";
            // 
            // Edi835FileProcess
            // 
            this.Edi835FileProcess.ServiceName = "ZP_Live_Edi835FileProcess";
            // 
            // DeleteEDIFileLog
            // 
            this.DeleteEDIFileLog.ServiceName = "ZP_Live_DeleteEDIFileLog";
            // 
            // ScheduleBatchServices
            // 
            this.ScheduleBatchServices.ServiceName = "ZP_Live_ScheduleBatchServices";
            // 
            // SendMissingDocumentEmail
            // 
            this.SendMissingDocumentEmail.ServiceName = "ZP_Live_SendMissingDocumentEmail";
            // 
            // cM_Attendance_Service1
            // 
            //this.cM_Attendance_Service1.ExitCode = 0;
            this.cM_Attendance_Service1.ServiceName = "ZP_Live_CM_Attendance_Service";
            // 
            // cM_ServicePlan_Service1
            // 
            //this.cM_ServicePlan_Service1.ExitCode = 0;
            this.cM_ServicePlan_Service1.ServiceName = "ZP_Live_CM_ServicePlan_Service";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.WindowServiceProcessInstaller,
            this.ScheduleNotification,
            this.RespiteHoursReset,
            this.Edi835FileProcess,
            this.DeleteEDIFileLog,
            this.ScheduleBatchServices,
            this.SendMissingDocumentEmail,
            this.cM_Attendance_Service1,
            this.cM_ServicePlan_Service1});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller WindowServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller ScheduleNotification;
        private System.ServiceProcess.ServiceInstaller RespiteHoursReset;
        //private System.ServiceProcess.ServiceInstaller serviceInstaller3;
        private System.ServiceProcess.ServiceInstaller Edi835FileProcess;
        private System.ServiceProcess.ServiceInstaller DeleteEDIFileLog;
        private System.ServiceProcess.ServiceInstaller ScheduleBatchServices;
        private System.ServiceProcess.ServiceInstaller SendMissingDocumentEmail;
        private System.ServiceProcess.ServiceInstaller cM_Attendance_Service1;
        private System.ServiceProcess.ServiceInstaller cM_ServicePlan_Service1;

    }
}