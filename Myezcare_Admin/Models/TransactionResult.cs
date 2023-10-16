using System.Collections.Generic;

namespace Myezcare_Admin.Models
{
    public class TransactionResult
    {
        public int TransactionResultId { get; set; }
        public long TablePrimaryId { get; set; }
        public string ErrorMessage { get; set; }
    }
}
