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
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using SmartLib.Models;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using WP7.ScanBarCode;
using System.Windows.Threading;
using com.google.zxing;

namespace SmartLib.ViewModels
{
    [DataContract]
    public class SearchViewModel : BaseViewModel
    {
        /// <summary>
        /// Previous searched titles.
        /// </summary>
        public ObservableCollection<string> SearchedTitles
        {
            get
            {
                return App.CurrentApplication.SearchedStrings[App.TITLE_KEY];
            }
        }

        /// <summary>
        /// Previous searched authors.
        /// </summary>
        public ObservableCollection<string> SearchedAuthors
        {
            get
            {
                return App.CurrentApplication.SearchedStrings[App.AUTHOR_KEY];
            }
        }

        /// <summary>
        /// Previous searched isbns.
        /// </summary>
        public ObservableCollection<string> SearchedIsbns
        {
            get
            {
                return App.CurrentApplication.SearchedStrings[App.ISBN_KEY];
            }
        }

        private string title;

        [DataMember]
        public string Title
        {
            get { return title; }
            set
            {
                if (value != title)
                {
                    title = value;
                    OnNotifyPropertyChanged("Title");
                }
            }
        }


        private string author;

        [DataMember]
        public string Author
        {
            get { return author; }
            set
            {
                if (value != author)
                {
                    author = value;
                    OnNotifyPropertyChanged("Author");
                }
            }
        }

        private string isbn;

        [DataMember]
        public string Isbn
        {
            get { return isbn; }
            set
            {
                if (value != isbn)
                {
                    isbn = value;
                    OnNotifyPropertyChanged("Isbn");
                }
            }
        }

        /// <summary>
        /// If command is executed, Search method will be called.
        /// </summary>
        public RelayCommand SearchCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// If command is executed, ScanBarcode method will be called.
        /// </summary>
        public RelayCommand ScanBarcodeCommand
        {
            get;
            private set;
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchViewModel()
        {
            SearchCommand = new RelayCommand(() => Search());
            ScanBarcodeCommand = new RelayCommand(() => ScanBarcode());
        }

        /// <summary>
        /// Validate form inputs. Returns true, if one of title, author or isbn value is filled.
        /// Shows successful/failure of validation to user.
        /// </summary>
        /// <returns>true if form inputs are valid, false otherwise</returns>
        public bool ValidateInputs()
        {
            bool isValid = true;

            List<string> invalidInputs = new List<string>();

            if (string.IsNullOrWhiteSpace(this.Isbn)
                && string.IsNullOrWhiteSpace(this.Title)
                && string.IsNullOrWhiteSpace(this.Author))
            {
                isValid = false;

                var msg = "One of fields must be filled.";

                App.CurrentApplication.MessageService.ShowWarningMessage(msg, "Search");
            }

            return isValid;
        }

        public async void ScanBarcode()
        {
             BarCodeManager.StartScan(async result =>
                {
                    if (result != null)
                    {
                        Debug.WriteLine("Barcode Type: {0}", result.BarcodeFormat);
                        Debug.WriteLine("Barcode Value: {0}", result.Text);
                        // if(Smartlib.Isbn.
                        if (result.BarcodeFormat == BarcodeFormat.EAN_13)
                        {
                            if (SmartLib.Isbn.IsValidIsbn13(result.Text))
                            {
                                Debug.WriteLine("{0} is ISBN13", result.Text);
                                Book book = await App.CurrentApplication.BookRequestManager.GetBookBy(BookIdentifier.Isbn, result.Text);

                                if (book != null)
                                {
                                    var root = Application.Current.RootVisual as Frame;
                                    root.DataContext = new BookViewModel(book);
                                    root.Navigate(new Uri("/MVVM/Views/BookPage.xaml", UriKind.Relative));
                                }
                                else
                                {
                                    string isbn10 = result.Text.Substring(3, 9);
                                    Debug.WriteLine("ISBN10 9 chars: {0}", isbn10);
                                    int sum = 0;
                                    for (int i = 0; i < 9; i++)
                                    {
                                        sum += Convert.ToInt16(isbn10[i]) * (10 - i);
                                    }

                                    isbn10 += 11 - (sum % 11);

                                    Debug.WriteLine("ISBN10 complete: {0}", isbn10);

                                    book = await App.CurrentApplication.BookRequestManager.GetBookBy(BookIdentifier.Isbn, result.Text);
                                    if (book != null)
                                    {
                                        var root = Application.Current.RootVisual as Frame;
                                        root.DataContext = new BookViewModel(book);
                                        root.Navigate(new Uri("/MVVM/Views/BookPage.xaml", UriKind.Relative));
                                    }
                                    else
                                    {
                                        App.CurrentApplication.MessageService.ShowSuccessMessage("No results", "Search");
                                    }
                                }
                            }
                        }
                        else if (result.BarcodeFormat == BarcodeFormat.CODE_39)
                        {
                            Book book = await App.CurrentApplication.BookRequestManager.GetBookBy(BookIdentifier.SysNo, result.Text);

                            if (book != null)
                            {
                                var root = Application.Current.RootVisual as Frame;
                                root.DataContext = new BookViewModel(book);
                                root.Navigate(new Uri("/MVVM/Views/BookPage.xaml", UriKind.Relative));
                            }
                            else
                            {
                                App.CurrentApplication.MessageService.ShowSuccessMessage("No results", "Search");
                            }
                        }
                    }

                    //Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show(result));

                }, 
                err => { return; });
        }

        /// <summary>
        /// Tries to find book according filled parameters. 
        /// If ISBN is filled, parameters author and title are ignored.
        /// </summary>
        public async void Search()
        {
            if (this.ValidateInputs())
            {
                if (string.IsNullOrWhiteSpace(Isbn))
                {
                    if (!string.IsNullOrWhiteSpace(Title))
                    {
                        if(!this.SearchedTitles.Contains(this.Title))
                            this.SearchedTitles.Add(this.Title);
                    }

                    if (!string.IsNullOrWhiteSpace(Author))
                    {
                        if (!this.SearchedTitles.Contains(this.Author))
                            this.SearchedAuthors.Add(this.Author);
                    }

                    var root = Application.Current.RootVisual as Frame;
                    root.Navigate(new Uri(string.Format("/MVVM/Views/ResultsPage.xaml?title={0}&author={1}", this.Title, this.Author), UriKind.Relative));
                }
                else
                {
                    if (!this.SearchedTitles.Contains(this.Isbn))
                        this.SearchedIsbns.Add(this.Isbn);
                    
                    Book book = await App.CurrentApplication.BookRequestManager.GetBookBy(BookIdentifier.Isbn, this.Isbn);

                    if (book != null)
                    {
                        var root = Application.Current.RootVisual as Frame;
                        root.DataContext = new BookViewModel(book);
                        root.Navigate(new Uri("/MVVM/Views/BookPage.xaml", UriKind.Relative));
                    }
                    else
                    {
                        App.CurrentApplication.MessageService.ShowSuccessMessage("No results", "Search");
                    }
                }
            }
        }
    }
}
