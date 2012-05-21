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
using System.Collections.Generic;

namespace SmartLib.Models
{
    [DataContract]
    public class Book
    {
        [DataMember(Name = "sysno")]
        public string Sysno { get; set; }

        [DataMember(Name = "isbn")]
        public string ISBN { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "authors")]
        public List<Author> Authors { get; set; }

        [DataMember(Name = "publisher")]
        public string Publisher { get; set; }

        [DataMember(Name = "publishedDate")]
        public string PublishedDate { get; set; }

        [DataMember(Name = "pageCount")]
        public uint PageCount { get; set; }

        [DataMember(Name = "pageType")]
        public string PageType { get; set; }

        [DataMember(Name = "pageDesc")]
        public string PageDescription { get; set; }

        [DataMember(Name = "language")]
        public string Language { get; set; }

        [DataMember(Name = "averageRating")]
        public uint Rating { get; set; }

        [DataMember(Name = "ratingCount")]
        public uint RatingCount { get; set; }

        [DataMember(Name = "coverUrl")]
        public string CoverURL { get; set; }

        [DataMember(Name = "comments")]
        public IEnumerable<Review> Comments { get; set; }

        // override object.Equals
        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            // TODO: write your implementation of Equals() here
            return this.Sysno == (obj as Book).Sysno;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            if (this.Sysno == null)
                return 0;
            return this.Sysno.GetHashCode();
        }

        //public static void FillBookWithDetails(Book bookToFill, Book details)
        //{
        //    if (bookToFill == null)
        //        throw new ArgumentException("bookToFill");
        //    if (details == null)
        //        throw new ArgumentException("details");

        //    bookToFill.Publisher = details.Publisher;
        //    bookToFill.PublishedDate = details.PublishedDate;
        //    bookToFill.Language = details.Language;
        //    bookToFill.PageType = details.PageType;
        //    bookToFill.PageCount = details.PageCount;
        //}
    }
}
