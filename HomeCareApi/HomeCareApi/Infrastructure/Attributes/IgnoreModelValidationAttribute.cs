using System;

namespace HomeCareApi.Infrastructure.Attributes
{
    /// <summary>
    /// This attribute will be used if you don't want to pass data attribute/ validate your data attribute in incoming request.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class IgnoreModelValidationAttribute : Attribute
    {
        public IgnoreModelValidationAttribute(bool ignoreModelValidation)
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