using System;

namespace HomeCareApi.Infrastructure.Attributes
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