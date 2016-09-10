using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace NFugue.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum @enum)
        {
            return @enum.GetType()
                .GetMember(@enum.ToString())
                .First()
                .GetCustomAttribute<DescriptionAttribute>()
                .Description
                ?? @enum.ToString();
        }

        public static T GetEnumValueFromDescription<T>(this string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", nameof(description));
        }
    }
}
