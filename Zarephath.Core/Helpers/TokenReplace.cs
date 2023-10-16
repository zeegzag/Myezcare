using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Zarephath.Core.Helpers
{
    public static class TokenReplace
    {
        private const string TokenPattern = "##{0}##";

        public static string ReplaceTokens<T>(string baseText, T item) where T : class , new()
        {
            if (string.IsNullOrEmpty(baseText))
                return baseText;

            foreach (PropertyInfo property in item.GetType().GetProperties())
            {
                object itemValue = property.GetValue(item, null);
                string pattern = string.Format(TokenPattern, property.Name.ToUpper());
                if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                {
                    object attributeValue = GetAttributeValue(item.GetType(), property.Name,
                                                              typeof(DisplayFormatAttribute), "DataFormatString");
                    itemValue = attributeValue != null
                                    ? string.Format(attributeValue.ToString(), itemValue)
                                    : Convert.ToDateTime(itemValue).ToShortDateString();
                }
                else if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?))
                {
                    object attributeValue = GetAttributeValue(item.GetType(), property.Name,
                                                              typeof(DisplayFormatAttribute), "DataFormatString");
                    itemValue = attributeValue != null
                                    ? string.Format(attributeValue.ToString(), itemValue)
                                    : itemValue;
                }

                baseText = Regex.Replace(baseText, pattern, Convert.ToString(itemValue).Replace("$", "$$"),
                    RegexOptions.IgnoreCase);
            }

            return baseText;
        }

        public static object GetAttributeValue(Type objectType, string propertyName, Type attributeType, string attributePropertyName)
        {
            var propertyInfo = objectType.GetProperty(propertyName);
            if (propertyInfo != null)
            {
                if (Attribute.IsDefined(propertyInfo, attributeType))
                {
                    var attributeInstance = Attribute.GetCustomAttribute(propertyInfo, attributeType);
                    if (attributeInstance != null)
                    {
                        foreach (PropertyInfo info in attributeType.GetProperties())
                        {
                            if (info.CanRead &&
                            String.Compare(info.Name, attributePropertyName,
                            StringComparison.InvariantCultureIgnoreCase) == 0)
                            {
                                return info.GetValue(attributeInstance, null);
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}
