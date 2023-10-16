namespace Myezcare_Admin.Models
{
    public class NameValueData
    {
        public string Name { get; set; }

        public long Value { get; set; }

    }

    public class NameValueDataBoolean
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }

    public class NameValueDataInString
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class NameValueForSpDropdown : NameValueData
    {
        public bool IsDeleted { get; set; }
    }
}
