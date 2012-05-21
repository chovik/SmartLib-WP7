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
using SmartLib.Models;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace SmartLib.ViewModels
{
    [DataContract]
    public class ResultsViewModel : BooksViewModel
    {
        private string title;

        [DataMember]
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                this.title = value;
            }
        }
        private string author;

        [DataMember]
        public string Author
        {
            get
            {
                return author;
            }
            set
            {
                this.author = value;
            }
        }

        private RelayCommand fetchMoreBooksCommand;

        /// <summary>
        /// If command is executed, FetchMoreBooks method will be called.
        /// </summary>
        public RelayCommand FetchMoreBooksCommand
        {
            get
            {
                if (fetchMoreBooksCommand == null) // RelayCommand is not serializable.
                    fetchMoreBooksCommand = new RelayCommand(() => FetchMoreBooks());

                return fetchMoreBooksCommand;
            }
            private set
            {
                if (value != fetchMoreBooksCommand)
                    fetchMoreBooksCommand = value;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="title">title to use as search parameter</param>
        /// <param name="author">author to use as search parameter</param>
        public ResultsViewModel(string title, string author)
        {
            if (string.IsNullOrWhiteSpace(title)
                && string.IsNullOrWhiteSpace(author))
                throw new ArgumentException("Title or author can not be empty.");

            this.title = title;
            this.author = author;
        }

        /// <summary>
        /// Loads more #number_of_results results. Method is called, when user reached last item in list.
        /// </summary>
        public virtual async void FetchMoreBooks()
        {
            var resultsCount = App.CurrentApplication.ResultsCount;
            if (Books.Count % resultsCount == 0) // true, if it is possible to load more books
            {
                try
                {
                    ConnectionErrorOccured = false;
                    Loaded = false;

                    var books = await App.CurrentApplication.BookRequestManager.SearchBooks(title, author, (uint)resultsCount, (uint)Books.Count);

                    if (books != null)
                    {
                        foreach (Book book in books)
                        {
                            BookViewModel bookViewModel = new BookViewModel(book);
                            bookViewModel.CheckCoverExistence();
                            this.Books.Add(bookViewModel);
                        }
                    }
                }
                catch (WebException webEx)
                {
                    //App.CurrentApplication.MessageService.ShowErrorMessage("Server is down.", "Error");
                    ConnectionErrorOccured = true;
                }
                finally
                {
                    Loaded = true;
                }
            }
        }
    }
}
