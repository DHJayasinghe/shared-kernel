using System;
using System.ComponentModel;
using System.Linq;

namespace SharedKernel.Helpers;

public static class EnumHelperExtension
{
    public static string GetDescription(this Enum value)
    {
        DescriptionAttribute attribute = value.GetType()
            .GetField(value.ToString())
            .GetCustomAttributes(typeof(DescriptionAttribute), false)
            .SingleOrDefault() as DescriptionAttribute;
        return attribute == null ? value.ToString() : attribute.Description;
    }
}