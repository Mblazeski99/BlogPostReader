using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace BlogReader.ValueConverters
{
    public class IsValueNullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
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
