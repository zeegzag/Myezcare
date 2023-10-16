using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models.Entity;

namespace Zarephath.Core.Models.ViewModel
{
    public class ChecklistItemModel
    {
        public int ChecklistItemTypeID { get; set; }
        public long ChecklistItemTypePrimaryID
        {
            get
            {
                return EncryptedPrimaryID != "" ? Convert.ToInt64(Crypto.Decrypt(EncryptedPrimaryID)) : 0;
            }
        }
        public string EncryptedPrimaryID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int VisitType { get; set; }
    }

    public class ChklistItemModel
    {
        public ChklistItemModel()
        {
            ChecklistItems = new List<ChecklistItemRecord>();
            TransactionResult = new TransactionResult();
        }

        public List<ChecklistItemRecord> ChecklistItems { get; set; }
        public TransactionResult TransactionResult { get; set; }
    }

    public class VisitChecklistItemModel
    {
        public long? EmployeeVisitID { get; set; }
        public long ReferralID { get; set; }
        public long ScheduleID { get; set; }
        public long EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeMobileNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsCareTypeAssigned { get; set; }
        public DateTime? ClockInTime { get; set; }
        public DateTime? ClockOutTime { get; set; }
        public bool? IsPCACompleted { get; set; }
        public bool? IsSigned { get; set; }
        public bool IsCompleted { get; set; }        
    }

    public class VisitChecklistItemDetail
    {
        public long EmployeeVisitID { get; set; }
        public bool IsCareTypeAssigned { get; set; }
        public bool IsClockInCompleted { get; set; }
        public bool IsClockOutCompleted { get; set; }
        public bool IsSigned { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class ChecklistItemRecord : ChecklistItem
    { 
        public string DocumentName { get; set; }
        public bool IsDocumentUploaded { get; set; }
        public bool IsChecked { get; set; }
    }

    public class SaveChecklistItemRecord
    {
        public int ChecklistItemTypeID { get; set; }
        public long ChecklistItemID { get; set; }
        public long ChecklistItemTypePrimaryID { get; set; }
        public bool IsChecked { get; set; }
    }

    public class SaveChecklistItemModel
    {
        public SaveChecklistItemModel()
        {
            ChecklistItems = new List<SaveChecklistItemRecord>();
        }

        public List<SaveChecklistItemRecord> ChecklistItems { get; set; }
        public string EncryptedPrimaryID { get; set; }
        public int ChecklistItemTypeID { get; set; }
    }

}
