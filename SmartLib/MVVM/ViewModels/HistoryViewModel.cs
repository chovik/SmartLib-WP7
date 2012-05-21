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

namespace SmartLib.ViewModels
{
    public class HistoryViewModel : BooksViewModel
    {
        /// <summary>
        /// Gets list (type ObservableCollection) of last viewed books. 
        /// These books were loaded from phone memory during application launching.
        /// </summary>
        public override System.Collections.ObjectModel.ObservableCollection<BookViewModel> Books
        {
            get
            {
                return App.CurrentApplication.History;
            }
        }
    }
}
