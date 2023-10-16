using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Myezcare_Admin.Models.Entity;

namespace Myezcare_Admin.Models.ViewModel
{
    public class AddreleaseNoteModel
    {
        public AddreleaseNoteModel()
        {
            ReleaseNote = new ReleaseNote();
        }
        public ReleaseNote ReleaseNote { get; set; }
    }
}