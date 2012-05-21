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
using Microsoft.Phone.Controls.Primitives;

namespace SmartLib.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        
        readonly NumbersDataSource resultsCount
                    = new NumbersDataSource { Minimum = 1, Maximum = 30 };

        /// <summary>
        /// Source of possible number of results.
        /// </summary>
        public ILoopingSelectorDataSource ResultsCount
        {
            get
            {
                return resultsCount;
            }
        }        

        /// <summary>
        /// Uco to be saved.
        /// </summary>
        public string Uco
        {
            get { return App.CurrentApplication.UCO; }
            set 
            {
                if (value != App.CurrentApplication.UCO)
                {
                    App.CurrentApplication.UCO = value;
                    OnNotifyPropertyChanged("UCO");
                }
            }
        }

        /// <summary>
        /// Password to be saved.
        /// </summary>
        public string Password
        {
            get { return App.CurrentApplication.Password; }
            set
            {
                if (value != App.CurrentApplication.Password)
                {
                    App.CurrentApplication.Password = value;
                    OnNotifyPropertyChanged("Password");
                }
            }
        }

        /// <summary>
        /// If command is executed, SaveSettings method will be called.
        /// </summary>
        public RelayCommand SaveCommand
        {
            get;
            private set;
        }

        public SettingsViewModel()
        {
            SaveCommand = new RelayCommand(() => SaveSettings());
            //if number of results changedl, the value will be saved to application settings.
            resultsCount.SelectionChanged += (s,e) => App.CurrentApplication.ResultsCount = Convert.ToUInt16(resultsCount.SelectedItem);
            resultsCount.SelectedItem = App.CurrentApplication.ResultsCount;
        }

        /// <summary>
        /// Saves settings
        /// </summary>
        public void SaveSettings()
        {
            var persistentDataStorage = App.CurrentApplication.PersistentDataStorage;
            persistentDataStorage.Backup("uco", this.Uco);
            persistentDataStorage.Backup("password", this.Password);
            persistentDataStorage.Backup("resultsCount", App.CurrentApplication.ResultsCount);

            //var root = Application.Current.RootVisual as Frame;
            //root.GoBack();
        }
    }
}
