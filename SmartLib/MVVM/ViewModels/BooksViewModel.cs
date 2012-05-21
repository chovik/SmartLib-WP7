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
using System.Collections.ObjectModel;
using SmartLib.Models;
using Microsoft.Phone.Controls;
using System.Collections.Generic;
using SmartLib.Helpers;
using System.Diagnostics;
using System.Linq;
using System.Windows.Data;
using System.Runtime.Serialization;

namespace SmartLib.ViewModels
{
    [DataContract]
    public class BooksViewModel : BaseViewModel
    {            

        private ObservableCollection<BookViewModel> books = new ObservableCollection<BookViewModel>();

        /// <summary>
        /// Gets list (type ObservableCollection) of loaded books. 
        /// </summary>
        [DataMember]
        public virtual ObservableCollection<BookViewModel> Books 
        {
            get
            {
                return books;
            }
            set
            {
                if (books != value)
                {
                    books = value;
                    OnNotifyPropertyChanged("Books");
                }
            }
        }

        private BookViewModel selectedBook = null;

        /// <summary>
        /// Selected book. Its book taped(selected) by user.
        /// </summary>
        [DataMember]
        public BookViewModel SelectedBook 
        {
            get
            {
                return selectedBook;
            }
            set
            {
                if (value != selectedBook)
                {
                    selectedBook = value;
                    OnNotifyPropertyChanged("SelectedBook");
                }
            }
        }

        

        private bool loaded = true;

        /// <summary>
        /// Gets or sets a value indicating whether the list of books is loaded or not.
        /// True if books are loaded, false during loading.
        /// </summary>
        public bool Loaded
        {
            get
            {
                return loaded;
            }
            protected set
            {
                if (loaded != value)
                {
                    this.loaded = value;
                    OnNotifyPropertyChanged("Loaded");
                }
                Debug.WriteLine("******Loaded = " + loaded);
            }
        }

        private bool isSelectionMode = false;

        /// <summary>
        /// Gets or sets a value indicating whether selection mode is on or off. 
        /// True, if selection mode is on. Otherwise selection mode is off.
        /// Selection mode is used to remove favourite books from phone memory.
        /// </summary>
        public bool IsSelectionMode
        {
            get { return isSelectionMode; }
            set
            {
                if (isSelectionMode != value)
                {
                    isSelectionMode = value;
                    OnNotifyPropertyChanged("IsSelectionMode");
                }
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public BooksViewModel()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="books">books to load</param>
        public BooksViewModel(IEnumerable<Book> books)
        {
            if (books == null)
                throw new ArgumentNullException("books");

            var bookViewModels = books.Select(book => new BookViewModel(book));
            this.Books = new ObservableCollection<BookViewModel>(bookViewModels);
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="books">Book ViewModels to load</param>
        public BooksViewModel(IEnumerable<BookViewModel> books)
        {
            if (books == null)
                throw new ArgumentNullException("books");
            this.Books = new ObservableCollection<BookViewModel>(books);
        }    

        /// <summary>
        /// It navigates to page containing details of selected book.
        /// </summary>
        public virtual void ShowSelectedBook()
        {
            if (this.SelectedBook != null)
            {
                var root = Application.Current.RootVisual as Frame;
                root.DataContext = this.SelectedBook;
                root.Navigate(new Uri("/MVVM/Views/BookPage.xaml", UriKind.Relative));
            }
            this.SelectedBook = null;
        }
    }
}
