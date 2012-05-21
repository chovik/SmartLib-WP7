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
using System.Collections.Generic;
using SmartLib.ViewModels;
using SmartLib.Helpers;
using System.Linq;

namespace SmartLib
{
    public class BookmarkedBooks
    {
        private const string dataStorageKey = "Favourite_Books";
        private static ObservableCollection<BookViewModel> books = new ObservableCollection<BookViewModel>();
        public static ObservableCollection<BookViewModel> Books
        {
            get
            {
                return books;
            }
        }

        public static void Save()
        {
            PersistentDataStorage dataStorage = new PersistentDataStorage();
            dataStorage.Backup(dataStorageKey, Books.Select(bookViewModel => bookViewModel.Book));
        }

        public static void Load()
        {
            PersistentDataStorage dataStorage = new PersistentDataStorage();

            var loadedBooks = dataStorage.Restore<IEnumerable<Book>>(dataStorageKey);

            if (loadedBooks != null)
            {
                foreach (Book book in loadedBooks)
                {
                    Books.Add(new BookViewModel(book) {IsBookmarked = true });
                }
            }
        }
    }
}
