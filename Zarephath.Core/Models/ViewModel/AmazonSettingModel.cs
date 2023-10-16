using Zarephath.Core.Infrastructure;

namespace Zarephath.Core.Models.ViewModel
{
    public class AmazonSettingModel
    {
        public string URL { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string ACL { get; set; }
        public string Policy { get; set; }
        public string Signature { get; set; }
        public string SuccessAction { get; set; }
        public string Folder { get; set; }
        public long UserID { get; set; }
    }
}
