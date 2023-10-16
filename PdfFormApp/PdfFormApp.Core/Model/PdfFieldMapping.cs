using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfFormApp.Core.Model
{
    public class PDFFieldMapping
    {
        public string PDFFieldName
        {
            get;
            set;
        }

        public string DBFieldName
        {
            get;
            set;
        }

        public string DBValue
        {
            get;
            set;
        }
    }
}
