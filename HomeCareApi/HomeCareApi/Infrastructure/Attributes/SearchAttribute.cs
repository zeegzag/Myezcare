using System;

namespace HomeCareApi.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class SearchAttribute : Attribute
    {
    }
}