using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Net.Configuration;
using System.Net.Mail;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using Zarephath.Core.Infrastructure;

namespace Zarephath.Core.Helpers
{
    public class EmailHelper
    {
        public enum SMTPSetting
        {
            GeneralEmailSetting = 1,
            EncryptedEmailSetting = 2
        }


        public static bool SendEmail(MailMessage mailer, int smtpSettingNumer = (int) SMTPSetting.GeneralEmailSetting)
        {
            SmtpClient client = new SmtpClient();
           
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();

            //Configuration configurationFile =
            //    WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);

            Configuration configurationFile;
            if (HttpContext.Current != null)
            {
                configurationFile =
                WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            }
            else
            {
                configurationFile =
                ConfigurationManager.OpenExeConfiguration(Assembly.GetEntryAssembly().Location);
            }

            //SmtpSection mailSettings;
            //if(smtpSettingNumer==(int)SMTPSetting.EncryptedEmailSetting)
            //    mailSettings = (SmtpSection)ConfigurationManager.GetSection("customMailSettings/smtp_encryption");
            //else
            //    mailSettings = (SmtpSection)ConfigurationManager.GetSection("customMailSettings/smtp_general");

            MailSettingsSectionGroup mailSettings =
                configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
           

            #region Update Setting From Organization Setting
            CacheHelper _cacheHelper = new CacheHelper();

            if (string.IsNullOrEmpty(_cacheHelper.NetworkHost) || string.IsNullOrEmpty(_cacheHelper.NetworkPort) ||
                string.IsNullOrEmpty(_cacheHelper.FromTitle)
                || string.IsNullOrEmpty(_cacheHelper.FromEmail) || string.IsNullOrEmpty(_cacheHelper.FromEmailPassword))
            {
                return false;
            }



            if (!string.IsNullOrEmpty(_cacheHelper.NetworkHost))
                mailSettings.Smtp.Network.Host = _cacheHelper.NetworkHost;

            if (!string.IsNullOrEmpty(_cacheHelper.FromTitle))
                mailSettings.Smtp.From = String.Format("{0} <{1}>", _cacheHelper.FromTitle, _cacheHelper.FromEmail);

            if (!string.IsNullOrEmpty(_cacheHelper.FromEmail))
                mailSettings.Smtp.Network.UserName = _cacheHelper.FromEmail;

            if (!string.IsNullOrEmpty(_cacheHelper.FromEmailPassword))
                mailSettings.Smtp.Network.Password = _cacheHelper.FromEmailPassword;

            if (!string.IsNullOrEmpty(_cacheHelper.NetworkPort))
                mailSettings.Smtp.Network.Port = Convert.ToInt16(_cacheHelper.NetworkPort);

            mailSettings.Smtp.Network.EnableSsl = _cacheHelper.EnableSSL;

            #endregion

            // if (mailSettings != null)
            // mailer.SmtpHost = mailSettings.Smtp.Network.Host;
            // mailSettings.Network.Host;// mailSettings.Smtp.Network.Host;

            mailer.SmtpHost = mailSettings.Smtp.Network.Host;
            mailer.From = mailSettings.Smtp.From; //mailSettings.From;// mailSettings.Smtp.From;

            client.Host = mailer.SmtpHost;
            bool bOut;
            if (mailer.ToList != null && mailer.ToList.Count > 0)
            {
                foreach (string to in mailer.ToList)
                {
                    message.To.Add(new MailAddress(to));
                }
            }
            if (mailer.CcList != null && mailer.CcList.Count > 0)
            {
                foreach (string cc in mailer.CcList)
                {
                    message.CC.Add(new MailAddress(cc));
                }
            }
            if (mailer.BccList != null && mailer.BccList.Count > 0)
            {
                foreach (string bcc in mailer.BccList)
                {
                    message.Bcc.Add(new MailAddress(bcc));
                }
            }
            if (mailer.AttachmentList != null && mailer.AttachmentList.Count > 0)
            {
                foreach (string attachment in mailer.AttachmentList)
                {
                    message.Attachments.Add(new Attachment(attachment));
                    
                }
            }

            if (smtpSettingNumer == (int) SMTPSetting.EncryptedEmailSetting)
            {
                if (!mailer.Subject.ToLower().Contains(Constants.SecureText.ToLower()))
                    mailer.Subject = string.Format("{0} {1}", mailer.Subject, Constants.SecureText);
            }


            message.Subject = mailer.Subject;
            message.IsBodyHtml = mailer.IsHtml;
            message.Body = mailer.Body;
            message.From = new MailAddress(mailer.From);
            

            //Object state = message;
            //event handler for asynchronous call 
            //client.SendCompleted += smtpClient_SendCompleted;
            try
            {
                //client.Send(message);
               
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(mailSettings.Smtp.Network.UserName, mailSettings.Smtp.Network.Password);
                //System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(mailSettings.Network.UserName, mailSettings.Network.Password);
                client.Credentials = credentials;
                
                client.Send(message);
                bOut = true;
            }
            catch(Exception ex)
            {
                throw ex;
                bOut = false;
            }
            finally
            {
                message.Dispose();
            }
            return bOut;
            //}
        }

        //private static void smtpClient_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        //{
        //    MailMessage mail = e.UserState as MailMessage;

        //}
    }

    public class MailMessage
    {
        #region properties

        public string SmtpHost { get; set; }

        public List<string> ToList { get; set; }

        public List<string> CcList { get; set; }

        public List<string> BccList { get; set; }

        public List<string> AttachmentList { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string From { get; set; }

        public bool IsHtml { get; set; }

        #endregion
        public MailMessage()
        {
            ToList = new List<string>();
            CcList = new List<string>();
            BccList = new List<string>();
            AttachmentList = new List<string>();
        }
        public MailMessage(List<string> toList, List<string> ccList, List<string> bccList, List<string> attachList, string subject, string body, string @from, bool isHtml)
        {
            ToList = toList;
            CcList = ccList;
            BccList = bccList;
            Subject = subject;
            Body = body;
            From = @from;
            IsHtml = isHtml;
            AttachmentList = attachList;
        }
        public MailMessage(List<string> toList, List<string> ccList, List<string> bccList, string subject, string body, string @from, bool isHtml)
        {
            ToList = toList;
            CcList = ccList;
            BccList = bccList;
            Subject = subject;
            Body = body;
            From = @from;
            IsHtml = isHtml;
        }
        public MailMessage(List<string> toList, string subject, string body, string @from, bool isHtml)
        {
            ToList = toList;
            Subject = subject;
            Body = body;
            From = @from;
            IsHtml = isHtml;
        }
        public bool SendMessage()
        {
            return EmailHelper.SendEmail(this);
        }
    }
}
