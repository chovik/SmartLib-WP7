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
using Coding4Fun.Phone.Controls;

namespace SmartLib
{
    public class ToastMessageService : IMessageService
    {
        public static Color GetColorFromHexa(string hexaColor)
        {
            if (hexaColor == null)
                throw new ArgumentNullException("hexaColor");

            return Color.FromArgb(
                    Convert.ToByte(hexaColor.Substring(1, 2), 16),
                    Convert.ToByte(hexaColor.Substring(3, 2), 16),
                    Convert.ToByte(hexaColor.Substring(5, 2), 16),
                    Convert.ToByte(hexaColor.Substring(7, 2), 16)
                    );
        }

        private void ShowMessage(string msg, string caption, Color bgColor, Color fgColor)
        {
            ArgumentValidator.AssertNotNull(msg, "msg");
            ArgumentValidator.AssertNotNull(caption, "caption");

            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    ToastPrompt toast = new ToastPrompt()
                    {
                        Title = caption,
                        Message = msg,
                        Background = new SolidColorBrush(bgColor),
                        Foreground = new SolidColorBrush(fgColor),
                        FontSize = 40,
                        TextOrientation = System.Windows.Controls.Orientation.Vertical
                    };
                    toast.Show();
                });

            
        }

        public void ShowSuccessMessage(string msg, string caption)
        {
            ShowMessage(msg, caption, Colors.Green, Colors.White);
        }

        public void ShowWarningMessage(string warningMsg, string caption)
        {
            ShowMessage(warningMsg, caption, Colors.Yellow, Colors.White);

        }

        public void ShowErrorMessage(string errorMsg, string caption)
        {
            ShowMessage(errorMsg, caption, Colors.Red, Colors.White);
        }
    }
}
