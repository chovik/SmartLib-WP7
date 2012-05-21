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
using System.Runtime.Serialization;
using SmartLib.Models;

namespace SmartLib.ViewModels
{
    [DataContract]
    public class ServerBooksViewModel : BooksViewModel
    {
        /// <summary>
        /// Category of loaded books. (News or Top)
        /// </summary>
        [DataMember]
        public BookListCategory BooksCategory { get; set; }

        public ServerBooksViewModel(BookListCategory category)
        {
            this.BooksCategory = category;
        }

        /// <summary>
        /// Updates books.
        /// </summary>
        public virtual async void UpdateBooks()
        {
            ConnectionErrorOccured = false;
            Loaded = false;

            try
            {
                var books = await App.CurrentApplication.BookRequestManager.GetBooksByCategory(this.BooksCategory, 10, 0);

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
                ConnectionErrorOccured = true;
            }
            finally
            {
                Loaded = true;
            }

        }
    }
}
