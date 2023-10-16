using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeCareApi.Models.General
{
    public class PostFileModel
    {
        public string PostItemType { get; set; }
        public long PostItemId { get; set; }
        public long UserId { get; set; }
        public long? ToUserId { get; set; }
        public int RandomNo { get; set; }
        public bool IsEdit { get; set; }
        public long? ParentItemId { get; set; }
        public long EmployeeVisitID { get; set; }
        public long EmployeeSignatureID { get; set; }
        public long ScheduleID { get; set; }
        public long EmployeeID { get; set; }
        public bool IvrClockout { get; set; }
        public decimal Lat { get; set; }
        public decimal Long { get; set; }
        public string IPAddress { get; set; }
    }

    public class PostEmpSignatureModel
    {
        public long EmployeeID { get; set; }
        public long EncryptedEmployeeID { get; set; }
        public long? EmployeeSignatureID { get; set; }
        public bool IsFingerPrintAuth { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string MobileNumber { get; set; }
        public string IVRPin { get; set; }
        public string Password { get; set; }
    }

    public class PostRefSignatureModel
    {
        public long ReferralID { get; set; }
    }

    public class FileModel
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string TempFilePath { get; set; }
        public string TempFileName { get; set; }
        public long ParentId { get; set; }
        public long PrimaryId { get; set; }
        public bool IsCoverImage { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsFirmCoverImage { get; set; }
    }

    public class DeleteFileModel : FileModel
    {
        public string PostItemType { get; set; }
    }

    public class UploadedFileModel
    {
        public string FileOriginalName { get; set; }
        public string TempFilePath { set; get; }
        public string TempFileName { set; get; }
    }

    public class FileListModel
    {
        public FileListModel()
        {
            FileList = new List<FileModel>();
        }

        public List<FileModel> FileList { get; set; }
    }
}