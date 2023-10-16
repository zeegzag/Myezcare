using System;

namespace Myezcare_Admin.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class SetValueOnAddAttribute : Attribute
    {
        public SetValueOnAddAttribute(int setValue)
        {
            SetValue = setValue;
        }


        public enum SetValueEnum
        {
            CurrentTime = 1, LoggedInUserId
        }

        public int SetValue { get; set; }
    }
}