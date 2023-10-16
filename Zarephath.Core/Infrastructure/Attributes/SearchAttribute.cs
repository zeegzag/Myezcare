using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zarephath.Core.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class SearchAttribute : Attribute
    {
    }
}