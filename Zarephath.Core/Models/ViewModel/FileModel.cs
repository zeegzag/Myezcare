using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zarephath.Core.Models.ViewModel
{
    public class FileModel
    {
        public string FileName { get; set; }
        public string FilePath { set; get; }
    }

    public class UploadedFileModel
    {
        public string FileOriginalName { get; set; }
        public string TempFilePath { set; get; }
        public string TempFileName { set; get; }
        public string GoogleFileJson { get; set; }
    }
}
