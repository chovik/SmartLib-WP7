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
using System.Runtime.Serialization;
using SmartLib.ViewModels;
using System.Diagnostics;

namespace SmartLib.MVVM.ViewModels
{
    public class ChangePasswordViewModel : BaseViewModel
    {
        private string oldPassword;
        /// <summary>
        /// Old password.
        /// </summary>
        [DataMember]
        public string OldPassword
        {
            get { return oldPassword; }
            set 
            {
                if (value != oldPassword)
                {
                    oldPassword = value;
                    OnNotifyPropertyChanged("OldPassword");
                }
            }
        }

        private string newPassword;

        /// <summary>
        /// New password.
        /// </summary>
        [DataMember]
        public string NewPassword
        {
            get { return newPassword; }
            set
            {
                if (value != newPassword)
                {
                    newPassword = value;
                    OnNotifyPropertyChanged("NewPassword");
                }
            }
        }

        private string newPassword2;

        /// <summary>
        /// New password again.
        /// </summary>
        [DataMember]
        public string NewPassword2
        {
            get { return newPassword2; }
            set
            {
                if (value != newPassword2)
                {
                    newPassword2 = value;
                    OnNotifyPropertyChanged("NewPassword2");
                }
            }
        }

        /// <summary>
        /// If command is executed, ChangePassword method will be called.
        /// </summary>
        public RelayCommand ChangePasswordCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ChangePasswordViewModel()
        {
            this.validator.AddValidationFor(() => this.OldPassword).NotEmpty().Show("Enter the old password.");
            this.validator.AddValidationFor(() => this.NewPassword).NotEmpty().Show("Enter the new password");
            //this.validator.AddValidationFor(() => this.NewPassword).NotEmpty().Show("Enter the new password(again)");
            this.validator.AddValidationFor(() => this.NewPassword2).Must(() => this.NewPassword == this.NewPassword2).Show("Repeated password is different");
            ChangePasswordCommand = new RelayCommand(() => ChangePassword());
        }

        /// <summary>
        /// Logs user in.
        /// </summary>
        public async void ChangePassword()
        {
            //validate form inputs
            if (!this.ValidateAll())
                return;

            //send request "save review" to server
            HttpStatusCode statusCode = await App.CurrentApplication.UserRequestManager.ChangePassword(App.CurrentApplication.LoggedUco, OldPassword, NewPassword);

            //process returned status code (from server response)
            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    App.CurrentApplication.MessageService.ShowSuccessMessage("Password has been changed.", "Password");

                    var root = Application.Current.RootVisual as Frame;
                    root.GoBack();
                    break;
                case HttpStatusCode.Unauthorized:
                    App.CurrentApplication.MessageService.ShowErrorMessage("Need authorization.", "Password");
                    break;
                default:
                    Debug.WriteLine("Change password - Unxpected Status Code '{0}'.", statusCode);
                    break;
            }            
        }
    }
}
