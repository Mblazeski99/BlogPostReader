using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace BlogReader.ValueConverters
{
    public class EnumToItemSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));

            Type enumType = parameter as Type;
            if (enumType == null || !enumType.IsEnum)
                throw new ArgumentException("Parameter must be an Enum type");

            return Enum.GetValues(enumType)
                       .Cast<Enum>()
                       .Select(e => new { Value = e, DisplayName = GetEnumDescription(e) })
                       .ToList();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private string GetEnumDescription(Enum enumValue)
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            var attribute = (System.ComponentModel.DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(System.ComponentModel.DescriptionAttribute));
            return attribute == null ? enumValue.ToString() : attribute.Description;
        }
    }
}
