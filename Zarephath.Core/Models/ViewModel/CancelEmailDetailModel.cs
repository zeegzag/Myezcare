using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;
using Zarephath.Core.Models.Entity;

namespace Zarephath.Core.Models.ViewModel
{
    public class CancelEmailDetailModel
    {
        public CancelEmailDetailModel()
        {
            EncryptedMailMessageToken = new EncryptedMailMessageToken();
            ScheduleMaster = new ScheduleMaster();
        }

        public ScheduleMaster ScheduleMaster { get; set; }

        public EncryptedMailMessageToken EncryptedMailMessageToken { get; set; }
    }


}
