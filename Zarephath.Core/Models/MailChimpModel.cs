namespace Zarephath.Core.Models
{
    public class MailChimpModel 
    {
        public string email_address { get; set; }

        public string status { get; set; }

        public MergeFields merge_fields { get; set; }
    }

    public class MergeFields
    {
        public string FNAME { get; set; }
        public string LNAME { get; set; }
        public string ADDRESS { get; set; }
        public string PHONE { get; set; }
        public string MMERGE9 { get; set; }
        public string MMERGE5 { get; set; }
        public string MMERGE6 { get; set; }
        public string MMERGE7 { get; set; }
    }
}
