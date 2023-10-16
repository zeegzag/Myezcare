using System;

namespace HomeCareApi.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class UniqueAttribute : Attribute
    {
        public UniqueAttribute(string withRespectToProperty = "")
        {
            WithRespectToProperty = withRespectToProperty;
        }

        public string WithRespectToProperty { get; set; }
    }
}