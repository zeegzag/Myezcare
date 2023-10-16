using PetaPoco;
using System;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.ViewModel
{
    public class ScheduleAggregatorLogsList
    {
        public long ScheduleID { get; set; }
        public long ReferralID { get; set; }
        public string EncryptedReferralID { get { return Crypto.Encrypt(ReferralID.ToString()); } }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Medicaid { get; set; }
        public string Name { get; set; }
        public long Row { get; set; }
        public int Count { get; set; }

        public long? EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string PatAddress { get; set; }

        public DateTime? LastSent { get; set; }
        public string Aggregator { get; set; }
        [Ignore]
        public string AggregatorText
        {
            get
            {
                return Common.SetPayorClaimProcessorList().Find(i => !string.IsNullOrEmpty(Aggregator) && i.Value.ToLower() == Aggregator.ToLower())?.Name;
            }
        }
        public bool? LastStatus { get; set; }
        public bool? IsWaitingForResponse { get; set; }
        [Ignore]
        public string LastStatusText
        {
            get
            {
                string text = null;
                if (IsWaitingForResponse.HasValue && IsWaitingForResponse.Value)
                { text = Resource.WaitingForResponse; }
                else if (LastSent.HasValue)
                {
                    text = Resource.Pending;
                    if (LastStatus.HasValue) { text = LastStatus.Value ? Resource.Success : Resource.Failed; }
                }
                return text;
            }
        }

    }

    public class ScheduleAggregatorLogsDetails
    {
        public long ScheduleDataEventProcessLogID { get; set; }
        public long OrganizationID { get; set; }
        public string TransactionID { get; set; }
        public long ScheduleID { get; set; }
        public string Aggregator { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsSuccess { get; set; }
        public bool? IsWaitingForResponse { get; set; }
        public string Messages { get; set; }

        [Ignore]
        public string LogFilePath
        {
            get
            { return Common.GetAggregatorLogPath(FileName, Aggregator, IsSuccess, IsWaitingForResponse, OrganizationID); }
        }

        [Ignore]
        public string AggregatorText
        {
            get
            {
                return Common.SetPayorClaimProcessorList().Find(i => !string.IsNullOrEmpty(Aggregator) && i.Value.ToLower() == Aggregator.ToLower())?.Name;
            }
        }

        [Ignore]
        public string StatusText
        {
            get
            {
                string text = Resource.Pending;
                if (IsWaitingForResponse.HasValue && IsWaitingForResponse.Value)
                { text = Resource.WaitingForResponse; }
                else if (IsSuccess.HasValue) { text = IsSuccess.Value ? Resource.Success : Resource.Failed; }
                return text;
            }
        }

    }
}

