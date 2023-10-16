using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace Myezcare_Admin.Helpers
{
    public static class EnumHelper<T>
    {
        public static IList<T> GetValues(Enum value)
        {
            var enumValues = new List<T>();

            foreach (FieldInfo fi in value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                enumValues.Add((T)Enum.Parse(value.GetType(), fi.Name, false));
            }
            return enumValues;
        }

        public static T Parse(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static IList<string> GetNames(Enum value)
        {
            return value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => fi.Name).ToList();
        }

        //public static IList<string> GetDisplayValues(Enum value)
        //{
        //    return GetNames(value).Select(obj => GetDisplayValue(Parse(obj))).ToList();
        //}

        public static string GetDisplayValue(T value)
        {

            var fieldInfo = value.GetType().GetField(value.ToString());

            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (descriptionAttributes == null) return string.Empty;
           // return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString();
            var resource = new ResourceManager(descriptionAttributes[0].ResourceType);
            string displayName = resource.GetString(descriptionAttributes[0].Name);

            return string.IsNullOrEmpty(displayName)
                ? string.Format("[[{0}]]", descriptionAttributes[0].Name)
                : displayName;
        }


        public static string GetHelpTextValue(T value)
        {

            var fieldInfo = value.GetType().GetField(value.ToString());

            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(HelpTextAttribute), false) as HelpTextAttribute[];

            if (descriptionAttributes == null) return string.Empty;
            // return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString();
            var resource = new ResourceManager(descriptionAttributes[0].ResourceType);
            string displayName = resource.GetString(descriptionAttributes[0].Name);

            return string.IsNullOrEmpty(displayName)
                ? string.Format("[[{0}]]", descriptionAttributes[0].Name)
                : displayName;
        }


        public static string GetRequiredTextValue(T value)
        {

            var fieldInfo = value.GetType().GetField(value.ToString());

            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(RequiredTextAttribute), false) as RequiredTextAttribute[];

            if (descriptionAttributes == null) return string.Empty;
            // return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString();
            var resource = new ResourceManager(descriptionAttributes[0].ResourceType);
            string displayName = resource.GetString(descriptionAttributes[0].Name);

            return string.IsNullOrEmpty(displayName)
                ? string.Format("[[{0}]]", descriptionAttributes[0].Name)
                : displayName;
        }

        
    }
    

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class RequiredTextAttribute : Attribute
    {

        public bool AutoGenerateField { get; set; }
        public bool AutoGenerateFilter { get; set; }
        public string Description { get; set; }
        public string GroupName { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public string Prompt { get; set; }
        public Type ResourceType { get; set; }
        public string ShortName { get; set; }

    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class HelpTextAttribute : Attribute
    {

        public bool AutoGenerateField { get; set; }
        public bool AutoGenerateFilter { get; set; }
        public string Description { get; set; }
        public string GroupName { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public string Prompt { get; set; }
        public Type ResourceType { get; set; }
        public string ShortName { get; set; }

    }
}
