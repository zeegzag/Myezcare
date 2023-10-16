
using System;

namespace Zarephath.Core.Models.ViewModel
{
    public class DMTSearchReferralListModel
    {
        public string AHCCCSID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Dob { get; set; }
    }

    public class DMTReferralDocumentUploadStatus
    {
        public string AHCCCSID { get; set; }
        public long ReferralID { get; set; }
        public bool UploadStatus { get; set; }
    }

    public class DMTReferralList
    {
        public long ReferralID { get; set; }
        public string AHCCCSID { get; set; }
        public string FirstName { get; set; }
        public bool UploadStatus { get; set; }
        public string LastName { get; set; }
        public DateTime Dob { get; set; }

    }
}