using System;
using System.ComponentModel;
namespace InTN;

public static class InTNApplicationUtil
{
    public static string GetEnumDescription(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        if (field != null)
        {
            var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (attribute != null)
            {
                return attribute.Description;
            }
        }
        return value.ToString();
    }
}