using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Globalization;
using System.Windows.Data;

namespace SmartLib.Helpers
{
    /// <summary>
    /// Converts a boolean value to a <see cref="Visibility"/> value, 
    /// and vice versa.
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            string paramValue = (string)parameter;

            if (value == null || (bool)value)
            {
                return paramValue == "Collapsed"
                    ? Visibility.Collapsed : Visibility.Visible;
            }

            return paramValue == "Collapsed"
                ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            string paramValue = (string)parameter;
            if (value == null || (Visibility)value == Visibility.Visible)
            {
                return paramValue != "Collapsed";
            }

            return paramValue == "Collapsed";
        }
    }
}
