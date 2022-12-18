using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BlogReader.ValueConverters
{
    public class IsStringEmptyOrNullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                if (parameter != null && parameter.ToString() == "hidden")
                    return Visibility.Hidden;

                return Visibility.Collapsed;
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
