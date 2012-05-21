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

namespace SmartLib.ViewModels
{
    public class BookmarksViewModel : BooksViewModel
    {
        /// <summary>
        /// Gets list (type ObservableCollection) of favourites books. 
        /// These books were loaded from phone memory during application launching.
        /// </summary>
        public override System.Collections.ObjectModel.ObservableCollection<BookViewModel> Books
        {
            get
            {
                return App.CurrentApplication.FavouriteBooks;
            }
        }

        /// <summary>
        /// If selection mode is off, it navigates to page containing details of selected (favourite) book.
        /// Otherwise book will be added/remove from favourite books.
        /// </summary>
        public override void ShowSelectedBook()
        {
            if (IsSelectionMode)
            {
                if (SelectedBook != null)
                {
                    //add/remove book from favourite books
                    this.SelectedBook.IsBookmarked = !SelectedBook.IsBookmarked;
                }
                //set to null to determine next click on the same book. (SelectionChanged event)
                SelectedBook = null;
            }
            else
            {
                //show book details
                base.ShowSelectedBook();
            }
        }
    }
}
