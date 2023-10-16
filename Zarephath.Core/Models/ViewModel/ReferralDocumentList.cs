using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Models.Entity;

namespace Zarephath.Core.Models.ViewModel
{
    [StoreProcedure(StoredProcedure.HC_GetReferralDocumentList)]
    [Sort("ReferralDocumentID", "DESC")]
    public class ReferralDocumentList : ReferralDocument
    {
        public int Row { get; set; }
        public int Count { get; set; }

        public string DocumentTypeName { get; set; }
        public string Name { get; set; }

    }
}
