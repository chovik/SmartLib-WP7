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

namespace SmartLib.ViewModels
{
    public class RegistrationViewModel : BaseViewModel
    {

        public RelayCommand SendRegistrationCommand
        {
            get;
            private set;
        }

        private uint uco;

        /// <summary>
        /// Uco.
        /// </summary>
        public uint Uco
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

        private string firstName;

        /// <summary>
        /// First name.
        /// </summary>
        public string FirstName
        {
            get { return firstName; }
            set 
            {
                if (value != firstName)
                {
                    firstName = value;
                    OnNotifyPropertyChanged("FirstName");
                }
            }
        }

        private string lastName;

        /// <summary>
        /// Last name.
        /// </summary>
        public string LastName
        {
            get { return lastName; }
            set
            {
                if (value != lastName)
                {
                    lastName = value;
                    OnNotifyPropertyChanged("LastName");
                }
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public RegistrationViewModel()
        {
            SendRegistrationCommand = new RelayCommand(() => SendRegistration());
        }


        /// <summary>
        /// Validate form inputs. Returns true, if first name, last name and UCO are filled.
        /// Shows successful/failure of validation to user.
        /// </summary>
        /// <returns>true if form inputs are valid, false otherwise</returns>
        public bool ValidateInputs()
        {
            bool isValid = true;

            List<string> invalidInputs = new List<string>();

            if (string.IsNullOrWhiteSpace(this.FirstName))
            {
                isValid = false;
                invalidInputs.Add("first name");
            }

            if (string.IsNullOrWhiteSpace(this.LastName))
            {
                isValid = false;
                invalidInputs.Add("last name");
            }

            if (!isValid)
            {
                var msg = string.Format("Following fields are required:\n{0}\nPlease fill them.",
                            string.Join("\n", invalidInputs));

                App.CurrentApplication.MessageService.ShowWarningMessage(msg, "Registration");
            }

            return isValid;
        }


        /// <summary>
        /// Sends registration. Registration will be sent just in case that form inputs are valid.
        /// Shows successful/failure of request to user.
        /// </summary>
        public async void SendRegistration()
        {
            if (this.ValidateInputs())
            {
                HttpStatusCode statusCode = await App.CurrentApplication.UserRequestManager.RegisterNewUser(Uco, FirstName, LastName);

                switch (statusCode)
                {
                    case HttpStatusCode.OK:
                        App.CurrentApplication.MessageService.ShowSuccessMessage("Registration was successful.", "Registration");
                        break;
                    case HttpStatusCode.BadRequest:
                        App.CurrentApplication.MessageService.ShowErrorMessage("Student with filled UCO does not exists", "Registration");
                        break;
                    case HttpStatusCode.Conflict:
                        App.CurrentApplication.MessageService.ShowErrorMessage("User with the same UCO already exists.", "Registration");
                        break;
                    default:
                        Debug.WriteLine("Unxpected Status Code '{0}'.", statusCode);
                        break;
                }
            }
        }
        
        
    }
}
