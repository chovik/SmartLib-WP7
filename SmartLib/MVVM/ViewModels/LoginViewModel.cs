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
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Linq;

namespace SmartLib.ViewModels
{
    [DataContract]
    public class LoginViewModel : BaseViewModel
    {
        private string nextPageURI = null;

        private string uco = App.CurrentApplication.UCO ?? "";

        /// <summary>
        /// Uco
        /// </summary>
        [DataMember]
        public string Uco
        {
            get { return uco; }
            set 
            {
                if (value != uco)
                {
                    uco = value;
                    OnNotifyPropertyChanged("Uco");
                }
            }
        }

        private bool savePassword = false;

        /// <summary>
        /// Gets or sets a value indicating whether the password will be saved to phone memory.
        /// </summary>
        [DataMember]
        public bool SavePassword
        {
            get { return savePassword; }
            set
            {
                if (value != savePassword)
                {
                    savePassword = value;
                    OnNotifyPropertyChanged("SavePassword");
                }
            }
        }


        private string password = App.CurrentApplication.Password ?? "";

        /// <summary>
        /// User password.
        /// </summary>
        [DataMember]
        public string Password
        {
            get { return password; }
            set 
            {
                if (value != password)
                {
                    password = value;
                    OnNotifyPropertyChanged("Password");
                }
            }
        }

        /// <summary>
        /// If command is executed, Login method will be called.
        /// </summary>
        public RelayCommand LoginCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="nextPageURI">page, where user will be navigated. if parameter is null, user will be navigated back.</param>
        public LoginViewModel(string nextPageURI)
        {
            this.validator.AddValidationFor(() => this.Uco).Must(() => this.uco.Count() > 0).Show("Enter the UČO");
           // this.validator.AddValidationFor(() => this.Uco).Must(() => Convert.this.uco.Count() > 0).Show("Inalid format.");
            this.validator.AddValidationFor(() => this.Password).NotEmpty().Show("Enter the password");
            LoginCommand = new RelayCommand(() => Login());
            this.nextPageURI = nextPageURI;
        }

        /// <summary>
        /// Logs user in.
        /// </summary>
        public void Login()
        {
            if (!this.ValidateAll())
                return;

            if (SavePassword)
            {
                App.CurrentApplication.UCO = Uco;
                App.CurrentApplication.Password = Password;
            }

            App.CurrentApplication.Login(this.Uco, this.Password, nextPageURI);
        }
        
        
    }
}
