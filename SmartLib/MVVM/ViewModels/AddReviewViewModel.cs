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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SmartLib.ViewModels
{
    /// <summary>
    /// ViewModel of AddReviewPage. Contains data (like rating, text). 
    /// It provides method for save/change review.
    /// </summary>
    public class AddReviewViewModel : BaseViewModel
    {
        private bool ratingValidationErrorOccured = false;

        public bool RatingValidationErrorOccured
        {
            get { return ratingValidationErrorOccured; }
            set 
            {
                if (value != ratingValidationErrorOccured)
                {
                    ratingValidationErrorOccured = value;
                    OnNotifyPropertyChanged("RatingValidationErrorOccured");
                }
            }
        }
        

        private uint rating;

        /// <summary>
        /// User rating
        /// </summary>
        public uint Rating
        {
            get { return rating; }
            set
            {
                if (value != rating)
                {
                    rating = value;
                    OnNotifyPropertyChanged("Rating");
                }
            }
        }


        private string text = "";

        /// <summary>
        ///  Review text
        /// </summary>
        public string Text
        {
            get { return text; }
            set
            {
                if (value != text)
                {
                    text = value;
                    OnNotifyPropertyChanged("Text");
                }
            }
        }

        /// <summary>
        /// If command is executed, SaveReview method will be called.
        /// </summary>
        public RelayCommand SaveReviewCommand
        {
            get;
            private set;
        }

        private string bookSysno;

        /// <summary>
        /// Database Id of Book that is reviewed by user.
        /// </summary>
        public string BookSysno
        {
            get { return bookSysno; }
            set 
            {
                if (value == null)
                    throw new ArgumentNullException("bookSysno");
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("bookSysno");
                this.bookSysno = value;
            }
        }
        

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="bookSysno">Database Id of Book that will be reviewed by user.</param>
        public AddReviewViewModel(string bookSysno)
        {
            //setup a validation
            this.validator.AddValidationFor(() => this.Rating).Must(() => this.Rating >= 1 && this.Rating <=5).Show("Rate the book.");
            this.validator.AddValidationFor(() => this.Text).Must(() => this.Text.Count() >= 10)
                .Show("Text is too short.");

            BookSysno = bookSysno;
            SaveReviewCommand = new RelayCommand(() => SaveReview());
        }

        /// <summary>
        /// Saves review. Review will be saved just in case that form inputs are valid.
        /// Shows successful/failure of request to user.
        /// </summary>
        public async void SaveReview()
        {
            //data validation
            bool isValid = true;

            //validate review text
            if (!this.ValidateAll())
                isValid = false;

            //validate user rating
            if (this.Rating < 1
                || this.Rating > 5)
            {
                RatingValidationErrorOccured = true;
                isValid = false;
            }

            //check if is everything ok
            if (!isValid)
                return;

            //send request "save review" to server
            HttpStatusCode statusCode = await App.CurrentApplication.ReviewRequestManager.SaveReview(bookSysno, this.Text, this.Rating);

            //process returned status code (from server response)
            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    App.CurrentApplication.MessageService.ShowSuccessMessage("Review has been saved.", "Review");

                    var root = Application.Current.RootVisual as Frame;
                    root.GoBack();
                    break;
                case HttpStatusCode.Unauthorized:
                    App.CurrentApplication.MessageService.ShowErrorMessage("Need authorization.", "Review");
                    break;
                default:
                    Debug.WriteLine("Review - Unxpected Status Code '{0}'.", statusCode);
                    break;
            }            
        }
    }
}
