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

namespace SmartLib
{
    public partial class SearchPage : PhoneApplicationPage
    {
        private readonly string transientKey = "ViewModel";

        private bool isNewPageInstance = false;

       // private MainViewModel viewModel;

        public SearchViewModel ViewModel
        {
            get 
            {
                return DataContext as SearchViewModel;
            }
        }
        

        // Constructor
        public SearchPage()
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
                    }else
                    {
                        DataContext = new SearchViewModel();
                    }
                }
            }

            isNewPageInstance = false;
        }
    }
}