using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using SmartLib.Models;
using System.Windows.Navigation;
using System.Runtime.Serialization;
using System.Linq;


namespace SmartLib.ViewModels
{
    [DataContract]
    public class MainViewModel : BaseViewModel
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MainViewModel()
        {
            SearchCommand = new RelayCommand(() => GoToSearchPage());
            RegisterCommand = new RelayCommand(() => GoToRegistrationPage());
            LoginCommand = new RelayCommand(() => GoToLoginPage());
            SelectionModeCommand = new RelayCommand(() => SwitchSelectionMode());
            RemoveSelectedCommand = new RelayCommand(() => RemoveSelectedFavourites());
            SettingsCommand = new RelayCommand(() => GoToSettingsPage());
            ChangePasswordCommand = new RelayCommand(() => GoToChangePasswordPage());
        }

        /// <summary>
        /// If command is executed, GoToChangePasswordPage method will be called.
        /// </summary>
        public RelayCommand ChangePasswordCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// If command is executed, GoToLoginPage method will be called.
        /// </summary>
        public RelayCommand LoginCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// If command is executed, RemoveSelectedFavourites method will be called.
        /// </summary>
        public RelayCommand RemoveSelectedCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// If command is executed, GoToRegistrationPage method will be called.
        /// </summary>
        public RelayCommand RegisterCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// If command is executed, GoToSearchPage method will be called.
        /// </summary>
        public RelayCommand SearchCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// If command is executed, GoToSettingsPage method will be called.
        /// </summary>
        public RelayCommand SettingsCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// If command is executed, SwitchSelectionMode method will be called.
        /// </summary>
        public RelayCommand SelectionModeCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Navigates to SearchPage.
        /// </summary>
        public void GoToSearchPage()
        {
                var root = Application.Current.RootVisual as Frame;
                root.Navigate(new Uri("/MVVM/Views/SearchPage.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Navigates to ScanPage.
        /// </summary>
        public void GoToRegistrationPage()
        {
            var root = Application.Current.RootVisual as Frame;
            root.Navigate(new Uri("/MVVM/Views/RegistrationPage.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Navigates to LoginPage.
        /// </summary>
        public void GoToLoginPage()
        {
            var root = Application.Current.RootVisual as Frame;
            root.Navigate(new Uri("/MVVM/Views/LoginPage.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Navigates to SettingsPage.
        /// </summary>
        public void GoToSettingsPage()
        {
            var root = Application.Current.RootVisual as Frame;
            root.Navigate(new Uri("/MVVM/Views/SettingsPage.xaml", UriKind.Relative));
        }

        public void GoToChangePasswordPage()
        {
            var root = Application.Current.RootVisual as Frame;
            root.Navigate(new Uri("/MVVM/Views/ChangePasswordPage.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Gets a value indicating whether selection mode is on.
        /// True if selection mode is on. Otherwise false.
        /// </summary>
        public bool IsSelectionMode
        {
            get
            {
                return FavouriteBooksViewModel.IsSelectionMode;
            }
        }

        /// <summary>
        /// Turn on/off selection mode. It is mode to edit list of favourite books.
        /// </summary>
        public void SwitchSelectionMode()
        {
            if (FavouriteBooksViewModel.IsSelectionMode)
            {
                foreach (var book in FavouriteBooksViewModel.Books)
                {
                    book.IsBookmarked = true;
                }
            }
            FavouriteBooksViewModel.IsSelectionMode = !FavouriteBooksViewModel.IsSelectionMode;
            OnNotifyPropertyChanged("IsSelectionMode");
        }
        
        /// <summary>
        /// Removes selected books from favourite books.
        /// </summary>
        public void RemoveSelectedFavourites()
        {
            var booksToRemove = FavouriteBooksViewModel.Books.Where(book => !book.IsBookmarked).ToArray();
            foreach (var book in booksToRemove)
            {
                FavouriteBooksViewModel.Books.Remove(book);
            }            

            SwitchSelectionMode();
        }

        private ServerBooksViewModel newBooksViewModel = new ServerBooksViewModel(BookListCategory.News);

        [DataMember]
        public ServerBooksViewModel NewBooksViewModel
        {
            get { return newBooksViewModel; }
            set { newBooksViewModel = value; }
        }

        private ServerBooksViewModel topBooksViewModel = new ServerBooksViewModel(BookListCategory.Top);

        [DataMember]
        public ServerBooksViewModel TopBooksViewModel
        {
            get { return topBooksViewModel; }
            set { topBooksViewModel = value; }
        }

        private BookmarksViewModel favouriteBooksViewModel = new BookmarksViewModel();
        [DataMember]
        public BookmarksViewModel FavouriteBooksViewModel
        {
            get
            {
                return favouriteBooksViewModel;
            }
            set { favouriteBooksViewModel = value; }
        }

        private HistoryViewModel historyViewModel = new HistoryViewModel();

        [DataMember]
        public HistoryViewModel HistoryViewModel
        {
            get
            {
                return historyViewModel;
            }
            set { historyViewModel = value; }
        }


        /// <summary>
        /// Updates top and new books.
        /// </summary>
        public void LoadDataFromServer()
        {
            NewBooksViewModel.UpdateBooks();
            TopBooksViewModel.UpdateBooks();
        }
    }
}