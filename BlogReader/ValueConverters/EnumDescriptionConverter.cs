using Serilog;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace BlogReader.ValueConverters
{
    public class EnumDescriptionConverter : IValueConverter
    {
        private string GetEnumDescription(Enum enumObj)
        {
            string result = string.Empty;

            try
            {
                FieldInfo fieldInfo = enumObj.GetType().GetField(enumObj.ToString());

                object[] attribArray = fieldInfo.GetCustomAttributes(false);

                if (attribArray.Length == 0)
                {
                    result = enumObj.ToString();
                }
                else
                {
                    DescriptionAttribute attrib = attribArray[0] as DescriptionAttribute;
                    result = attrib.Description;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "EnumDescriptionConverter/GetEnumDescription failed!");
            }

            return result;
        }

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string description = string.Empty;

            if (value != null && value.ToString() != "")
            {
                Enum myEnum = (Enum)value;
                description = GetEnumDescription(myEnum);
            }

            return description;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }
    }
}
