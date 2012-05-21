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

namespace SmartLib.Helpers
{
    public static class ArgumentValidator
    {
        public static T AssertNotNull<T>(T value, string parameterName) where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            return value;
        }

        public static string AssertNotNullOrWhiteSpaceString(string value, string parameterName)
        {
            if(string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(parameterName);

            return value;
        }
    }
}
