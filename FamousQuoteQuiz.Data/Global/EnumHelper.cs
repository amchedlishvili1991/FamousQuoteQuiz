using System;
using System.ComponentModel;
using System.Reflection;

namespace FamousQuoteQuiz.Data.Global
{
    public static class EnumHelper
    {
        public static string GetEnumDescription(this Enum enumValue)
        {
            if (enumValue == null)
                return null;

            FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return enumValue.ToString();
        }
    }
}
