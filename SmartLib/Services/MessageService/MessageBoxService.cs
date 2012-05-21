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
using SmartLib.Helpers;

namespace SmartLib
{
    public class MessageBoxService : IMessageService
    {

        public void ShowSuccessMessage(string msg, string caption)
        {
            if (msg == null)
                throw new ArgumentNullException("msg");
            if (caption == null)
                throw new ArgumentNullException("caption");

            MessageBox.Show(msg, caption, MessageBoxButton.OK);
        }

        public void ShowWarningMessage(string warningMsg, string caption)
        {
            this.ShowSuccessMessage(warningMsg, caption);
        }

        public void ShowErrorMessage(string errorMsg, string caption)
        {
            this.ShowSuccessMessage(errorMsg, caption);
        }
    }
}
