﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zarephath.Core.Models
{
    public class ReportInfo
    {
        public int ReportId { get; set; }
        public string ReportName { get; set; }
        public string ReportDescription { get; set; }
        public string ReportURL { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string ReportSummary { get; set; }
    }
}
