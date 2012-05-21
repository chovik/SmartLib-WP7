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
using SmartLib.ViewModels;
using DanielVaughan.WindowsPhone7Unleashed;

namespace SmartLib
{
    public partial class BooksControl : UserControl
    {
        public BooksControl()
        {
            InitializeComponent();
        }

        //private void newBooksListBox_Loaded(object sender, RoutedEventArgs e)
        //{
        //    //var scrollViewerMonitor = new ScrollViewerMonitor();
        //    ScrollViewerMonitor.SetAtEndCommand(sender as ListBox, (DataContext as BooksViewModel).FetchMoreBooksCommand);
        //}

        //static T FindChildOfType<T>(DependencyObject root) where T : class
        //{
        //    var queue = new Queue<DependencyObject>();
        //    queue.Enqueue(root);

        //    while (queue.Count > 0)
        //    {
        //        DependencyObject current = queue.Dequeue();
        //        for (int i = VisualTreeHelper.GetChildrenCount(current) - 1; 0 <= i; i--)
        //        {
        //            var child = VisualTreeHelper.GetChild(current, i);
        //            var typedChild = child as T;
        //            if (typedChild != null)
        //            {
        //                return typedChild;
        //            }
        //            queue.Enqueue(child);
        //        }
        //    }
        //    return null;
        //}
    }
}
