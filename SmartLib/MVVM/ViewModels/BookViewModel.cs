using System;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Collections.ObjectModel;
using SmartLib.Helpers;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;

namespace SmartLib.ViewModels
{
    [DataContract]
    public class BookViewModel : BaseViewModel
    {
        const string transientStateKey = "BookViewModel_Book";

        private Book book;
        /// <summary>
        /// Book.
        /// </summary>
        [DataMember]
        public Book Book
        {
            get
            {
                return book;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("book");

                book = value;
                OnNotifyPropertyChanged("Book");
            }
        }

        private ObservableCollection<ReviewViewModel> reviews = new ObservableCollection<ReviewViewModel>();

        /// <summary>
        /// Book Reviews.
        /// </summary>
        [DataMember]
        public ObservableCollection<ReviewViewModel> Reviews
        {
            get
            {
                return reviews;
            }
            set
            {
                if (value != reviews)
                {
                    reviews = value;
                    OnNotifyPropertyChanged("Reviews");
                }
            }
        }

        private bool isBookmarked = false;

        /// <summary>
        /// Gets or sets a value indicating whether the book is favourite or not.
        /// True if book is favourite. Otherwise false.
        /// </summary>
        [DataMember]
        public bool IsBookmarked
        {
            get { return isBookmarked; }
            set 
            {
                if (value != isBookmarked)
                {
                    isBookmarked = value;
                    OnNotifyPropertyChanged("IsBookmarked");
                }
            }
        }

        /// <summary>
        /// Checks cover existence. If server does not return cover, the default cover will be set;
        /// </summary>
        public void CheckCoverExistence()
        {
            //determine if cover was load
            if (string.IsNullOrWhiteSpace(this.Book.CoverURL))
            {
                //set default cover
                //var uri = new Uri(".", UriKind.Relative);
                this.Book.CoverURL = "/Images/no_cover_thumb.png";
            }
        }
	

        /// <summary>
        /// Gets formatted string of book authors.
        /// </summary>
		public string Authors
        {
            get
            {
                string authorsString = "no authors";

                if (book != null
                    && book.Authors != null
                    && book.Authors.Count > 0)
                {
                    authorsString = string.Join(", ", book.Authors);
                }
                return authorsString;
            }
        }

        private RelayCommand addReviewCommand;

        /// <summary>
        /// If command is executed, GoToAddReviewPage method will be called.
        /// </summary>
        public RelayCommand AddReviewCommand
        {
            get
            {
                if (addReviewCommand == null) // RelayCommand nieje serializovatelny a pri nacitani historie a oblubenych knih sposobovalo chybu
                    addReviewCommand = new RelayCommand(() => GoToAddReviewPage());

                return addReviewCommand;
            }
            private set
            {
                if (value != addReviewCommand)
                    addReviewCommand = value;
            }
        }

        private RelayCommand bookmarkCommand;

        /// <summary>
        /// If command is executed, AddToFavourites method will be called.
        /// </summary>
        public RelayCommand BookmarkCommand
        {
            get
            {
                if (bookmarkCommand == null) // RelayCommand nieje serializovatelny a pri nacitani historie a oblubenych knih sposobovalo chybu
                    bookmarkCommand = new RelayCommand(() => Bookmark());

                return bookmarkCommand;
            }
            private set
            {
                if (value != bookmarkCommand)
                    bookmarkCommand = value;
            }
        }

        /// <summary>
        /// Number of votes.
        /// </summary>
        public uint RatingCount
        {
            get { return this.Book.RatingCount; }
            set
            {
                if (value != this.Book.RatingCount)
                {
                    this.Book.RatingCount = value;
                    OnNotifyPropertyChanged("RatingCount");
                }
            }
        }

        /// <summary>
        /// Average rating.
        /// </summary>
        public uint Rating
        {
            get { return this.Book.Rating; }
            set
            {
                if (value != this.Book.Rating)
                {
                    this.Book.Rating = value;
                    OnNotifyPropertyChanged("Rating");
                }
            }
        }

        /// <summary>
        /// Updates Rating and RatingCount value.
        /// </summary>
        public virtual async void UpdateRating()
        {
            try
            {
                ConnectionErrorOccured = false;

                //send request "get ratings" to server
                var ratings = await App.CurrentApplication.ReviewRequestManager.GetBookRatings(this.Book.Sysno);

                if (ratings != null
                    && ratings.Count() > 0)
                {
                    int average = 0;

                    int i = 1;
                    foreach (var rating in ratings)
                    {
                        average += rating * i;
                        i++;
                    }

                    int ratingCount = ratings.Sum();
                    average = average / ratingCount;

                    this.Rating = (uint)average;
                    this.RatingCount = (uint)ratingCount;
                }
            }
            catch (WebException webEx)
            {
                ConnectionErrorOccured = true;
            }
            finally
            {
            }
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="book">book to wrap</param>
        public BookViewModel(Book book)
        {
            Book = book;            
        }


        /// <summary>
        /// Adds/removes book from favourites.
        /// </summary>
        public void Bookmark()
        {
            var favourites = App.CurrentApplication.FavouriteBooks;
            IsBookmarked = !favourites.Contains(this);
            if (!IsBookmarked)
            {
                favourites.Remove(this);
                App.CurrentApplication.MessageService.ShowSuccessMessage("Book has been removed from favourites.", "Favourite Books");
            }
            else
            {
                favourites.Add(this);
                App.CurrentApplication.MessageService.ShowSuccessMessage("Book has been added to favourites.", "Favourite Books");
            }
        }

        /// <summary>
        /// Navigates to AddReviewPage or to LoginPage.
        /// If user is already logged in, it navigates to AddReviewPage. Otherwise to LoginPage.
        /// </summary>
        public void GoToAddReviewPage()
        {
            if (App.CurrentApplication.LoggedIn)
            {
                var root = Application.Current.RootVisual as Frame;
                root.Navigate(new Uri(string.Format("/MVVM/Views/AddReviewPage.xaml?sysno={0}", this.Book.Sysno), UriKind.Relative));
            }
            else
            {
                var root = Application.Current.RootVisual as Frame;
                root.Navigate(new Uri(string.Format("/MVVM/Views/LoginPage.xaml?nextPageURI=/MVVM/Views/AddReviewPage.xaml?sysno={0}", this.Book.Sysno), UriKind.Relative));
            }
        }

        /// <summary>
        /// Updates book details. It is used to update information, that has not been received by searching.
        /// </summary>
        public async void UpdateBookDetails()
        {
            try
            {
                ConnectionErrorOccured = false;
                var filledBook = await App.CurrentApplication.BookRequestManager.GetBookDetails(this.Book);
                this.Book = filledBook;
            }
            catch (WebException webEx)
            {
                ConnectionErrorOccured = true;
            }
            finally
            {
            }
        }

        /// <summary>
        /// Updates Book Reviews.
        /// </summary>
        public async void UpdateReviews()
        {
            Reviews.Clear();

            try
            {
                ConnectionErrorOccured = false;

                IEnumerable<Review> newReviews = null;

                newReviews = await App.CurrentApplication.ReviewRequestManager.GetBookReviews(this.Book.Sysno, 10, 0);

                if (newReviews != null)
                {
                    foreach (var review in newReviews)
                    {
                        Reviews.Add(new ReviewViewModel(review));
                    }
                }
            }
            catch (WebException webEx)
            {
                ConnectionErrorOccured = true;
            }
            finally
            {
            }
        }

        /// <summary>
        /// Adds Book to last viewed books. If Book is already in the list, 
        /// it will be inserted to the beginning of the list.
        /// </summary>
        public void AddToHistory()
        {
            var history = App.CurrentApplication.History;

            //get list of book with the same sysno value
            var foundBooksInHistory = history.Where(bookTemp => bookTemp.Book.Sysno == book.Sysno);

            if (foundBooksInHistory.Count() > 0)
            {
                history.Remove(foundBooksInHistory.First());
            }

            history.Insert(0, this);

            if (history.Count > 10)
            {
                history.Remove(history.Last());
            }
        }
    }
}