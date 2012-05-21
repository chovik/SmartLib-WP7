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
using Microsoft.Devices;

namespace SmartLib
{
    public partial class ScanPage : PhoneApplicationPage
    {
        private readonly string transientKey = "ViewModel";

        private bool isNewPageInstance = false;

       // private MainViewModel viewModel;

        public ScannerViewModel ViewModel
        {
            get 
            {
                return DataContext as ScannerViewModel;
            }
        }
        

        // Constructor
        public ScanPage()
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
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (isNewPageInstance)
            {
                DataContext = new ScannerViewModel();
                ViewModel.Initialize();
            }
            previewVideo.SetSource(ViewModel.PhotoCamera);

            isNewPageInstance = false;
        }
    }
}