namespace Zarephath.Core.Infrastructure
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
        /// <summary>
        /// Created another constructor for NVarchar for store procedure
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public SearchValueData(string name, string value, string dataType)
        {
            Name = name;
            Value = value;
            DataType = dataType;
        }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool IsEqual { get; set; }
        public bool IsNotEqual { get; set; }
        public string DataType { get; set; }
    }

    public class SearchModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public int OperatorId { get; set; }
    }
}