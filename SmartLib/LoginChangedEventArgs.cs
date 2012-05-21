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

namespace SmartLib
{
    public class LoginChangedEventArgs : EventArgs
    {
        public bool IsLoggedIn { get; private set; }

        public LoginChangedEventArgs(bool isLoggedIn)
        {
            this.IsLoggedIn = isLoggedIn;
        }
    }
}
