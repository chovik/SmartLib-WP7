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
using System.Runtime.Serialization;

namespace SmartLib.ViewModels
{
    [DataContract]
    public class ReviewViewModel : BaseViewModel
    {
        private Review review;
        [DataMember]
        public Review Review 
        { 
            get
            {
                return review;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("review");

                if(value != review)
                {
                    review = value;
                    OnNotifyPropertyChanged("Reviews");
                }
            }
        }

        /// <summary>
        /// Formatted string of user. "#first_name #last_name"
        /// </summary>
        public string User
        {
            get
            {
                return string.Format("{0} {1}", this.Review.User.FirstName, this.Review.User.LastName);
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="review">review to wrap</param>
        public ReviewViewModel(Review review)
        {
            Review = review;
        }

    }
}
