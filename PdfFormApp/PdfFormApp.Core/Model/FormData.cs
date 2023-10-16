using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfFormApp.Core.Model
{
    public class FormDataEntity
    {
        public int OrganizationFormDataId
        {
            get;
            set;
        }

        public string FormId
        {
            get;
            set;
        }

        public string OrganizationId
        {
            get;
            set;
        }

        public string FormData
        {
            get;
            set;
        }
        public int TypeId
        {
            get;
            set;
        }

        public string FormUniqueId
        {
            get;
            set;
        }

        public int ReferralId
        {
            get;
            set;
        }

        public int EmployeeId
        {
            get;
            set;
        }

        public int UserId
        {
            get;
            set;
        }

        public string EBriggsFormID
        {
            get;
            set;
        }


        public string OriginalEBFormID
        {
            get;
            set;
        }



        public string SubSectionID
        {
            get;
            set;
        }

        public string FormName
        {
            get;
            set;
        }
        public string UpdateFormName
        {
            get;
            set;
        }

        public long EbriggsFormMppingID
        {
            get;
            set;
        }

        
    }
}
