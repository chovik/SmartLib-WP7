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
using Microsoft.Phone.Controls;
using SmartLib.ViewModels;

namespace SmartLib.Views
{
    public class BasePhoneApplicationPage : PhoneApplicationPage
    {
        protected const string transientKey = "ViewModel";
        public BaseViewModel ViewModel
        {
            get
            {
                return this.DataContext as BaseViewModel;
            }
        }

        private bool isNewPageInstance = false;
        public bool IsNewPageInstance
        {
            get
            {
                return isNewPageInstance;
            }

            set
            {
                if (value != isNewPageInstance)
                {
                    isNewPageInstance = value;
                }
            }
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

            if (IsNewPageInstance)
            {
                if (ViewModel == null)
                {
                    object viewModel = null;

                    if (State.TryGetValue(transientKey, out viewModel))
                    {
                        DataContext = viewModel;
                    }
                    else
                    {
                        DataContext = new MainViewModel();
                       // ViewModel.LoadData();
                    }
                }
            }

            IsNewPageInstance = false;
        }
    }
}
