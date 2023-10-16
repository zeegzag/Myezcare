using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Myezcare_Admin.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SortAttribute : Attribute
    {
        public SortAttribute(string sortKey, string sortDirection)
        {
            KeyValue = sortKey;
            DirectionValue = sortDirection;
        }

        public string KeyValue
        {
            get;
            private set;
        }

        public string DirectionValue
        {
            get;
            private set;
        }
    }
}