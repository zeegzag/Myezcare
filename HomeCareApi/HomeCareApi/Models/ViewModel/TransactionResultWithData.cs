using HomeCareApi.Models.Entity;
using HomeCareApi.Models.General;

namespace HomeCareApi.Models.ViewModel
{
    public class EmployeeTransactionResult
    {
        public EmployeeTransactionResult()
        {
            Employee = new Employee();
            TransactionResult = new TransactionResult();
        }
        public Employee Employee { get; set; }
        public TransactionResult TransactionResult { get; set; }
    }
}