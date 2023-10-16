using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web;
using Elmah;
using HomeCareApi.Infrastructure;
using HomeCareApi.Infrastructure.DataProvider;
using HomeCareApi.Models.ViewModel;

namespace HomeCareApi.Models.General
{
    public class EmailHelper
    {
        public static bool SendEmail(MailMessage mailer)
        {

            SecurityDataProvider sc = new SecurityDataProvider();
            OrganizationSetting orgSettings = sc.GetOrganizationDetail();

            if (string.IsNullOrEmpty(orgSettings.NetworkHost)
                || string.IsNullOrEmpty(orgSettings.NetworkPort)
                || string.IsNullOrEmpty(orgSettings.FromTitle)
                || string.IsNullOrEmpty(orgSettings.FromEmail)
                || string.IsNullOrEmpty(orgSettings.FromEmailPassword))
            {
                return false;
            }










            SmtpClient client = new SmtpClient
            {
                Port = mailer.SmtpSection.Network.Port,
                Host = mailer.SmtpSection.Network.Host,
                EnableSsl = mailer.SmtpSection.Network.EnableSsl,
                UseDefaultCredentials = mailer.SmtpSection.Network.DefaultCredentials,
                Credentials =
                    new NetworkCredential(orgSettings.FromEmail, orgSettings.FromEmailPassword,
                        mailer.SmtpSection.Network.ClientDomain)
            };
            Common.CreateLogFile("client");
            if (mailer.SmtpSection.Network.TargetName != null)
                client.TargetName = mailer.SmtpSection.Network.TargetName;
            client.DeliveryMethod = mailer.SmtpSection.DeliveryMethod;
            if (mailer.SmtpSection.SpecifiedPickupDirectory != null && mailer.SmtpSection.SpecifiedPickupDirectory.PickupDirectoryLocation != null)
                client.PickupDirectoryLocation = mailer.SmtpSection.SpecifiedPickupDirectory.PickupDirectoryLocation;


            using (System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage())
            {
                client.Host = mailer.SmtpSection.Network.Host;

                bool bOut;
                if (mailer.ToList != null && mailer.ToList.Count > 0)
                {
                    foreach (string to in mailer.ToList)
                    {
                        message.To.Add(new MailAddress(to));
                        //message.ReplyToList.Add(new MailAddress(to));
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

                if (!string.IsNullOrEmpty(mailer.ReplyMessageID))
                {
                    message.Headers.Add("In-Reply-To", mailer.ReplyMessageID);
                }
                Common.CreateLogFile("before Subject");
                message.Subject = mailer.Subject;
                message.IsBodyHtml = mailer.IsHtml;
                message.Body = mailer.Body;
                message.From = new MailAddress(orgSettings.FromEmail);
                Common.CreateLogFile("From");
                try
                {
                    client.Send(message);
                    Common.CreateLogFile("Email Send");
                    bOut = true;
                }
                catch (Exception ex)
                {
                    Common.CreateLogFile(ex.Message + Environment.NewLine + ex.StackTrace);
                    ErrorLog.GetDefault(HttpContext.Current).Log(new Error(ex));
                    bOut = false;
                }
                return bOut;
            }
        }
    }

    public class MailMessage
    {
        #region properties

        //public string SmtpHost { get; set; }

        public SmtpSection SmtpSection { get; set; }

        public List<string> ToList { get; set; }

        public List<string> CcList { get; set; }

        public List<string> BccList { get; set; }

        public List<string> AttachmentList { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string From { get; set; }

        public bool IsHtml { get; set; }

        public string ReplyMessageID { get; set; }

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