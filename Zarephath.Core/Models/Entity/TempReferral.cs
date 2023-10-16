using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zarephath.Core.Models.Entity
{
    [TableName("TempReferral")]
    public class TempReferral
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ErrorMessage { get; set; }
        public long CreatedBy { get; set; }
        public bool IsShow { get; set; }

    }
}
