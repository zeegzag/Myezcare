using System;

namespace HomeCareApi.Infrastructure.Attributes
{
    /// <summary>
    /// This attribute will be used if you don't want to pass token value in incoming request.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class IgnoreAuthenticationAttribute : Attribute
    {
        public IgnoreAuthenticationAttribute(bool ignoreAuthentication)
        {
            Value = ignoreAuthentication;
        }

        public bool Value
        {
            get;
            private set;
        }
    }
}