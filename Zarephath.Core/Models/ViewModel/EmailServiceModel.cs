using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models.Entity;

namespace Zarephath.Core.Models.ViewModel
{
    public class SendEmailServiceModel
    {
        public SendEmailServiceModel()
        {
            EmailTemplate = new EmailTemplate();
            ListEmailService = new List<ListEmailService>();
        }
        public EmailTemplate EmailTemplate { get; set; }
        public List<ListEmailService> ListEmailService { get; set; }
    }

    public class ListEmailService : EmailToken
    {
        CacheHelper _cacheHelper = new CacheHelper();

        public long ReferralID { get; set; }
        public long ScheduleID { get; set; }
        public string ScheduleStatusID { get; set; }
        public string FirstName { get; set; }
        public string ClientName { get; set; }
        public string FacilityID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DropOffImage { get; set; }
        public string PickUpImage { get; set; }

        public string DropOffLocation { get; set; }
        public string DropOffAddress { get; set; }
        public string DropOffCity { get; set; }
        public string DropOffZip { get; set; }

        public string PickUPLocation { get; set; }
        public string PickAddress { get; set; }
        public string PickCity { get; set; }
        public string PickZip { get; set; }

        public string MondayPickUp { get; set; }
        public string TuesdayPickUp { get; set; }
        public string WednesdayPickUp { get; set; }
        public string ThursdayPickUp { get; set; }
        public string FridayPickUp { get; set; }
        public string SaturdayPickUp { get; set; }
        public string SundayPickUp { get; set; }
        public string MondayDropOff { get; set; }
        public string TuesdayDropOff { get; set; }
        public string WednesdayDropOff { get; set; }
        public string ThursdayDropOff { get; set; }
        public string FridayDropOff { get; set; }
        public string SaturdayDropOff { get; set; }
        public string SundayDropOff { get; set; }

        public DateTime? WeekSMSDate { get; set; }
        public DateTime? WeekEmailDate { get; set; }
        public string Email { get; set; }
        public string MailStatus { get; set; }

        public string ConfirmationUrl { get; set; }
        public string CancellatioUrl { get; set; }
        public string LastWeekGenrateDate { get; set; }
        public string DropOffTime { get; set; }
        public string DropOffDay { get; set; }
        public string PickUpTime { get; set; }
        public string PickUpDay { get; set; }

        public string DropOffStateCode { get; set; }
        public string PickUpStateCode { get; set; }

        public string ZerpathLogoImage { get; set; }

        public string DropOffFullAddress
        {
            get
            {
                var lst = new List<string>
                    {
                        DropOffLocation,
                        DropOffAddress,
                        (string.IsNullOrEmpty(DropOffCity) ? "" : (DropOffCity)) +
                        (string.IsNullOrEmpty(DropOffStateCode) ? "" : (", " + DropOffStateCode)) +
                        (string.IsNullOrEmpty(DropOffZip) ? "" : (" " + DropOffZip + "."))
                    };
                return string.Join(",<br/>", lst.Where(m => !string.IsNullOrEmpty(m)).ToList());
            }
        }

        public string PickUpFullAddress
        {
            get
            {
                var lst = new List<string>
                    {
                        PickUPLocation,
                        PickAddress,
                        (string.IsNullOrEmpty(PickCity) ? "" : (PickCity)) +(string.IsNullOrEmpty(PickUpStateCode) ? "" : (", " + PickUpStateCode)) +
                        (string.IsNullOrEmpty(PickZip) ? "" : (" " + PickZip + "."))
                    };
                return string.Join(",<br/>", lst.Where(m => !string.IsNullOrEmpty(m)).ToList());
            }
        }

        public string DropOffFullAddressforURL
        {
            get
            {
                var lst = new List<string>
                {
                    DropOffLocation, 
                    DropOffAddress,
                    DropOffCity ,
                    DropOffStateCode + (string.IsNullOrEmpty(DropOffZip)?"":("+" + DropOffZip))
                };
                return string.Join("+", lst.Where(m => !string.IsNullOrEmpty(m)).ToList());
            }
        }

        public string PickUpFullAddressforURL
        {
            get
            {
                var lst = new List<string>
                {
                    PickUPLocation, 
                    PickAddress, 
                    PickCity ,
                    PickUpStateCode +( string.IsNullOrEmpty(PickZip)?"":("+"+PickZip ))     
                };
                return string.Join("+", lst.Where(m => !string.IsNullOrEmpty(m)).ToList());

            }
        }

        public bool PermissionForEmail { get; set; }
        public bool PermissionForMail { get; set; }
        public bool PermissionForSMS { get; set; }
        public string Phone1 { get; set; }
        public string to { get; set; }
        public string ScheduleDateString { get; set; }

        public string DropOffPhone { get; set; }
        public string PickUpPhone { get; set; }

        public string StrPickUpPhone
        {
            get
            {
                if (!string.IsNullOrEmpty(PickUpPhone))
                {
                    return "24 Hour Line: " + Convert.ToInt64(PickUpPhone).ToString("(###) ###-####");
                }
                return null;
            }
        }

        public string strDropOffPhone
        {
            get
            {
                if (!string.IsNullOrEmpty(DropOffPhone))
                {
                    return "24 Hour Line: " + Convert.ToInt64(DropOffPhone).ToString("(###) ###-####");
                }
                return null;
            }
        }

        public string ParentLastName { get; set; }
        public string ParentFirstName { get; set; }

        public string AtPickUp { get; set; }
        public string AtDropOff { get; set; }
        public string ClientNickName { get; set; }

        public string ParentAddress { get; set; }
        public string ParentCity { get; set; }
        public string ParentZipCode { get; set; }
        public string ParenStateName { get; set; }

        public string TagZerpathLogoImage
        {
            get
            {
                CacheHelper cacheHelper = new CacheHelper();
                return "<img src='" + cacheHelper.TemplateLogo + "' width='300' style='float:right;'/>";
            }
        }
        public string TempZerpathLogoImage
        {
            get
            {
                CacheHelper cacheHelper = new CacheHelper();
                return "<img src='" + cacheHelper.TemplateLogo + "' width='300'/>";
            }
        }




        public string ParentName
        {
            get { return ParentFirstName + ' ' + ParentLastName; }
        }

        public string TagPickUpImage
        {
            get
            {
                return "<img src='" + PickUpImage + "' height='250' width='280' style='float:right;'/>";
            }
        }

        public string TagDropOffImage
        {
            get
            {
                return "<img src='" + DropOffImage + "' height='250' width='280' style='float:right;'/>";
            }
        }

        public string FaceBookImage
        {
            get
            {
                return "<img src='" + _cacheHelper.SiteBaseURL + Constants.FaceBookLogoImage + "' height='30' width='30' style='float:left;'/>";
            }
        }

        public string FaceBookImageforEmail
        {
            get
            {
                return "<img src='" + _cacheHelper.SiteBaseURL + Constants.FaceBookLogoImage + "' height='30' width='30' />";
            }
        }
        public string SiblingIDs { get; set; }
        public bool RemoveINNotice { get; set; }
        public bool IsProcessed { get; set; }
        public long DropTransportLocationID { get; set; }
        public long PickupTransportLocationID { get; set; }
        public string child { get; set; }
        public string ParentEmail { get; set; }
        public string ParentPhone { get; set; }

        public bool PCMEmail { get; set; }
        public bool PCMMail { get; set; }
        public bool PCMSMS { get; set; }
        public bool PCMVoiceMail { get; set; }

        public bool EmailSent { get; set; }
        public bool SMSSent { get; set; }
        public bool NoticeSent { get; set; }

    }
}
