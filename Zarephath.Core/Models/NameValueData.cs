namespace Zarephath.Core.Models
{
    public class NameValueData
    {
        public string Name { get; set; }

        public long Value { get; set; }

    }

    public class NameValueStringData
    {
        public string Name { get; set; }

        public string Value { get; set; }

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
