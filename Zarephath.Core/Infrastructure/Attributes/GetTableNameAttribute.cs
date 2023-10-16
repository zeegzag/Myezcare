using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zarephath.Core.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class UniqueAttribute : Attribute
    {
        public UniqueAttribute(string errorMessage,string withRespectToProperty = "")
        {
            WithRespectToProperty = withRespectToProperty;
            ErrorMessage = errorMessage;
        }

        public string WithRespectToProperty { get; set; }
        public string ErrorMessage { get; set; }

    }
}