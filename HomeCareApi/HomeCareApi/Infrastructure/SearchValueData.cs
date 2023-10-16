using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeCareApi.Infrastructure
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
            DataType = Constants.DataTypeString;
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