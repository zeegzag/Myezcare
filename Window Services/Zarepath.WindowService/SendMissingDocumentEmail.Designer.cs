using System.IO;
using Zarephath.Core.Infrastructure;

namespace Zarepath.WindowServices
{
    partial class SendMissingDocumentEmail
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
            // 
            // SendMissingDocumentEmail
            // 
            this.ServiceName = "ZP_" + Constants.WinServiceMode + "_SendMissingDocumentEmail";

            //Common.CreateLogFile("SendMissingDocumentEmail Init.", ConfigSettings.SendMissingDocumentEmailFileName, Path.Combine(ConfigSettings.LogPath, ConfigSettings.SendMissingDocumentEmailLog));

        }

        #endregion

    }
}
