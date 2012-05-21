using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using SmartLib.ViewModels;
using System.Runtime.Serialization;

namespace SmartLib
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        private readonly string transientKey = "ViewModel";

        private bool isNewPageInstance = false;

       // private MainViewModel viewModel;

        public SettingsViewModel ViewModel
        {
            get 
            {
                return DataContext as SettingsViewModel;
            }
        }
        

        // Constructor
        public SettingsPage()
        {
            isNewPageInstance = true;
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            if (e.NavigationMode != System.Windows.Navigation.NavigationMode.Back)
            {
                if (State.ContainsKey(transientKey))
                {
                    State[transientKey] = ViewModel;
                }
                else
                {
                    State.Add(transientKey, ViewModel);
                }
            }
            else
            {
                ViewModel.SaveSettings();
            }

        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (isNewPageInstance)
            {
                if (ViewModel == null)
                {
                    object viewModel = null;

                    if(State.TryGetValue(transientKey, out viewModel))
                    {
                        DataContext = viewModel;
                    }
                    else
                    {
                        DataContext = new SettingsViewModel();
                    }
                }
            }

            isNewPageInstance = false;
        }
    }
}