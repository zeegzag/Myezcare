﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Myezcare_Admin.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class StoreProcedureAttribute : Attribute
    {
        public StoreProcedureAttribute(string spName)
        {
            StoreProcedureName = spName;
        }

        public string StoreProcedureName { get; private set; }
    }
}