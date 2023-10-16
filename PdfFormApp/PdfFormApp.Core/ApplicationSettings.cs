using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfFormApp.Core
{
    public static class ApplicationSettings
    {
        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["MyezcareOrganization"].ConnectionString;
            }
        }
    }
}
