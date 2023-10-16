using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PetaPoco;
using Zarephath.Core.Controllers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Utility;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.ViewModel
{
    #region addPayor Detail && Service Code Detail

    public class LayoutDetailModel
    {
        public LayoutDetailModel()
        {
            InterNalMessageNotificationList = new List<InterNalMessageNotification>();
            BillingNotesNotificationList= new List<BillingNotesNotification>();
        }

        public List<InterNalMessageNotification> InterNalMessageNotificationList { get; set; }
        public int TotalPendingMessagesCount { get; set; }
        public int NewMessagesCount { get; set; }
        public int ResolvedMessagesCount { get; set; }
        public int PendingVisitCount { get; set; }
        public List<BillingNotesNotification> BillingNotesNotificationList { get; set; }
        public int AssignedBillingNotesCount { get; set; }
        

        [Ignore]
        public string NewMessagesCountMessage { get; set; }
        [Ignore]
        public string ResolvedMessagesCountMessage { get; set; }
        [Ignore]
        public string PendingVisitCountMessage { get; set; }
        [Ignore]
        public string NewCheckTime { get; set; }
        [Ignore]
        public string NewCheckTimeForPendingVisit { get; set; }
        [Ignore]
        public bool CanHaveApprovePermission { get; set; }
    }

    public class InterNalMessageNotification
    {
       
        public long ReferralInternalMessageID { get; set; }
        public long ReferralID { get; set; }
        public string AHCCCSID { get; set; }
        public string ClientName { get; set; }
        public string InternalMessage { get; set; }
        public long AssigneeID { get; set; }
        public string Assignee { get; set; }
        public long CreatedID { get; set; }
        public string CreatedBY { get; set; }
        public string CreatedBYUserName { get; set; }

        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime CreatedDate { get; set; }

        [Ignore]
        public string EncryptedReferralID { get { return Crypto.Encrypt(ReferralID.ToString()); } }
    }



    public class BillingNotesNotification
    {

        public long NoteID { get; set; }
        public long ReferralID { get; set; }
        public string AHCCCSID { get; set; }
        public string ClientName { get; set; }
        public string NoteComments { get; set; }
        public string AssigneeBy { get; set; }

        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime? NoteAssignedDate { get; set; }

        [Ignore]
        public string EncryptedReferralID { get { return Crypto.Encrypt(ReferralID.ToString()); } }
        [Ignore]
        public string EncryptedNoteID { get { return Crypto.Encrypt(NoteID.ToString()); } }
    }


    public class SearchLayoutDetailModel
    {
        public int PageSize { get; set; }
        public long AssineeID { get; set; }
        public long CreatedID { get; set; }
        public string AssigneeLastCheckTime { get; set; }
        public string ResolvedLastCheckTime { get; set; }
        public string PendingVisitLastCheckTime { get; set; }
    }

  
    #endregion
}

