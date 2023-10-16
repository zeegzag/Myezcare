using System;

namespace HomeCareApi.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class CombinedUniqueAttribute : Attribute
    {
        public CombinedUniqueAttribute(string withRespectToProperty = "")
        {
            WithRespectToProperty = withRespectToProperty;
        }

        public string WithRespectToProperty { get; set; }
    }
}