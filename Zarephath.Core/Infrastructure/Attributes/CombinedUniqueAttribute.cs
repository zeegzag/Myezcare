using System;
using Zarephath.Core.Helpers;

namespace Zarephath.Core.Infrastructure.Attributes
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




    [AttributeUsage(AttributeTargets.Property)]
    public class TitleAttribute : Attribute
    {
        public TitleAttribute(string title = "",Type resourceType = null)
        {
            Title = resourceType != null ? ResourceHelper.GetResourceLookup(resourceType, title) : title;
        }

        public string Title { get; set; }
    }
   
}