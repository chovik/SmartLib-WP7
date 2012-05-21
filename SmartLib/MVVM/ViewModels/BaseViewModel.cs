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
using System.ComponentModel;
using SmartLib.Helpers;
using System.Runtime.Serialization;
using Infrastructure.Validation;

namespace SmartLib.ViewModels
{
    /// <summary>
    /// Base ViewModel. Contains common methods, properties of all ViewModels.
    /// </summary>
    [DataContract]
    [KnownType(typeof(BooksViewModel))]
    [KnownType(typeof(BookViewModel))]
    [KnownType(typeof(ReviewViewModel))]
    public abstract class BaseViewModel : ValidationViewModel, INotifyPropertyChanged
    {

        private bool connectionErrorOccured = false;
        
        /// <summary>
        /// Gets or sets a value indicating whether connection error occured.
        /// </summary>
        public bool ConnectionErrorOccured
        {
            get
            {
                return connectionErrorOccured;
            }
            set
            {
                if(value != connectionErrorOccured)
                {
                    connectionErrorOccured = value;
                    OnNotifyPropertyChanged("ConnectionErrorOccured");
                }
            }
        }
    }
}
