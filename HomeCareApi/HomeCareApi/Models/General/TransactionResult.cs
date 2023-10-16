namespace HomeCareApi.Models.General
{
    public class TransactionResult
    {
        public int TransactionResultId { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsDuplicate { get; set; }
        public long TablePrimaryId { get; set; }
        public bool IsOTPNotFound { get; set; }
        public bool IsOtpExpire { get; set; }
    }
}