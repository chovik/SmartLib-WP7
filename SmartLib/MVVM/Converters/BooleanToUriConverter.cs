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
using System.Windows.Data;
using System.Globalization;
using System.Diagnostics;

namespace SmartLib.Helpers
{
    public class BooleanToUriConverter : IValueConverter
    {
        public string IconUriOnTrue 
        {
            get; 
            set; 
        }
        
        public string IconUriOnFalse 
        { 
            get; 
            set; 
        }

        public BooleanToUriConverter() : this("","")
        {

        }

        public BooleanToUriConverter(string iconUriOntrue, string iconUriOnFalse)
        {
            if (iconUriOntrue == null)
                throw new ArgumentNullException("iconUriOntrue");
            if (iconUriOnFalse == null)
                throw new ArgumentNullException("iconUriOnFalse");

            this.IconUriOnTrue = iconUriOntrue;
            this.IconUriOnFalse = iconUriOnFalse;

        }

        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            Debug.WriteLine(value);
            if (targetType != typeof(Uri))
                throw new InvalidOperationException("The target must be a Uri");
            return (bool)value ? new Uri(IconUriOnTrue, UriKind.Relative) : new Uri(IconUriOnFalse, UriKind.Relative);
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            Debug.WriteLine(value);
            // Do the conversion from visibility to bool
            if (targetType != typeof(bool))
                throw new InvalidOperationException("The target must be a bool");

            return ((Uri)value).OriginalString == IconUriOnTrue;
        }
    }
}
