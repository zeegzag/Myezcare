using System;
using System.Collections.Generic;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models.Entity;

namespace Zarephath.Core.Models.ViewModel
{
    public class VisitReasonModalDetail
    {
        public string ClaimProcessor { get; set; }
        public bool ByPassModal { get; set; }
        public bool HideReasonType { get; set; }
        public bool HideActionType { get; set; }
    }
}
