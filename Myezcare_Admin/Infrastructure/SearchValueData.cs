namespace Myezcare_Admin.Infrastructure
{
    public class SearchValueData
    {
        public SearchValueData()
        {
            DataType = Constants.DataTypeString;
        }
        public SearchValueData(string name, string value)
        {
            Name = name;
            Value = value;
        }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool IsEqual { get; set; }
        public bool IsNotEqual { get; set; }
        public string DataType { get; set; }
        public decimal DecimalValue { get; set; }
    }

    public class SearchModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public int OperatorId { get; set; }
    }
}