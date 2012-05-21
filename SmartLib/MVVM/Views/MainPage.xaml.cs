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
using Microsoft.Phone.Shell;
using Phone7.Fx.Controls;
using System.Windows.Data;
using SmartLib.Helpers;

namespace SmartLib
{
    public partial class MainPage : PhoneApplicationPage
    {

        private readonly string transientKey = "ViewModel";

        private bool isNewPageInstance = false;

       // private MainViewModel viewModel;

        public MainViewModel ViewModel
        {
            get 
            {
                return DataContext as MainViewModel;
            }
        }
        

        // Constructor
        public MainPage()
        {
            isNewPageInstance = true;
            InitializeComponent();

            App.CurrentApplication.LoginChanged += (s, e) =>
                {
                    //if (e != null)
                    //{
                    //    if (e.IsLoggedIn)
                    //    {
                    //        changePasswordMenuItem.Visibility = System.Windows.Visibility.Visible;
                    //        logOutMenuItem.Visibility = System.Windows.Visibility.Visible;

                    //        logInMenuItem.Visibility = System.Windows.Visibility.Collapsed;
                    //        signUpMenuItem.Visibility = System.Windows.Visibility.Collapsed;
                    //    }
                    //    else
                    //    {
                    //        changePasswordMenuItem.Visibility = System.Windows.Visibility.Collapsed;
                    //        logOutMenuItem.Visibility = System.Windows.Visibility.Collapsed;

                    //        logInMenuItem.Visibility = System.Windows.Visibility.Visible;
                    //        signUpMenuItem.Visibility = System.Windows.Visibility.Visible;
                    //    }

                    //}
                };
            // Set the data context of the listbox control to the sample data
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
        //    changePasswordMenuItem.Visibility = System.Windows.Visibility.Collapsed;
        //    logOutMenuItem.Visibility = System.Windows.Visibility.Collapsed;

        //    logInMenuItem.Visibility = System.Windows.Visibility.Visible;
        //    signUpMenuItem.Visibility = System.Windows.Visibility.Visible;
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
                        DataContext = new MainViewModel();
                        ViewModel.LoadDataFromServer();
                    }
                }
            }

            isNewPageInstance = false;
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {

        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (selectionModeAppButton != null)
            {
                if (((Pivot)sender).SelectedIndex == 3)
                {
                    selectionModeAppButton.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    selectionModeAppButton.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        private void AdvancedApplicationBar_Loaded(object sender, RoutedEventArgs e)
        {
            //(sender as AdvancedApplicationBar).BackgroundColor = Color.FromArgb(255,33,33,33);
            //(sender as AdvancedApplicationBar).ForegroundColor = Colors.White;
        }
    }
}