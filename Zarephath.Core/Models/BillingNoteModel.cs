using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zarephath.Core.Models
{
    public class BillingNoteModel
    {
        public long BillingNoteID { get; set; }
        public long BatchID { get; set; }
        public string BillingNote { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long UpdatedBy { get; set; }
        public Boolean IsDeleted { get; set; }
        public string FirstName  { get; set; }
        public string LastName { get; set; }

    }
}
