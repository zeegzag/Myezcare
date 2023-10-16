using System;

namespace HomeCareApi.Infrastructure.Attributes
{
    /// <summary>
    /// This attribute will be used if you don't want to pass any request/model data in your method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class IgnoreActionForIncomingRequestAttribute : Attribute
    {
        public IgnoreActionForIncomingRequestAttribute(bool ignoreModelValidation)
        {
            Value = ignoreModelValidation;
        }

        public bool Value
        {
            get;
            private set;
        }
    }
}