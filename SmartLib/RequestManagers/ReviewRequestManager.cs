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
using System.Threading.Tasks;
using System.Collections.Generic;
using SmartLib.Models;
using SmartLib.Helpers;

namespace SmartLib.DataManagers
{
    public class ReviewRequestManager : BaseDataManager
    {
        /// <summary>
        /// Constructor. For more information see parent constructor.
        /// </summary>
        /// <param name="requestManager"></param>
        /// <param name="server"></param>
        public ReviewRequestManager(RequestManager requestManager, string server)
                : base(requestManager, server)
        {
        }

        /// <summary>
        /// Creates URL used to send review.
        /// </summary>
        /// <param name="sysno">database id of book to review</param>
        /// <returns>URL used to send review</returns>
        public string CreatePostReviewURL(string sysno)
        {
            ArgumentValidator.AssertNotNull(sysno, "sysno");
            ArgumentValidator.AssertNotNullOrWhiteSpaceString(sysno, "sysno");

            return string.Format("http://{0}/api/books/{1}/reviews", ServerAddress, sysno);
        }

        /// <summary>
        /// Creates URL used to receive book reviews.
        /// </summary>
        /// <param name="sysno">database id of book</param>
        /// <param name="limit">number of results</param>
        /// <param name="offset">number of skipped results</param>
        /// <returns>URL used to receive book reviews</returns>
        private string CreateBookReviewsURL(string sysno, uint limit, uint offset)
        {
            return string.Format("http://{0}/api/books/{1}/reviews?limit={2}&offset={3}",
                ServerAddress,
                sysno,
                limit,
                offset);
        }

        /// <summary>
        /// Creates URL used to receive book ratings.
        /// </summary>
        /// <param name="sysno">database id of book</param>
        /// <returns>book ratings</returns>
        private string CreateBookRatingsURL(string sysno)
        {
            return string.Format("http://{0}/api/books/{1}/ratings", ServerAddress, sysno);
        }

        /// <summary>
        /// Sends request to save review.
        /// </summary>
        /// <param name="sysno">database id of book to review</param>
        /// <param name="review">book review</param>
        /// <param name="rating">user rating</param>
        /// <returns>status code of server response</returns>
        public async Task<HttpStatusCode> SaveReview(string sysno, string review, uint rating)
        {
            if (sysno == null)
                throw new ArgumentNullException("sysno");
            if (string.IsNullOrWhiteSpace(sysno))
                throw new ArgumentException("sysno");

            if (review == null)
                throw new ArgumentNullException("review");
            if (string.IsNullOrWhiteSpace(review))
                throw new ArgumentException("review");

            if (rating < 0 || rating > 5)
                throw new ArgumentException("rating");


            string url = CreatePostReviewURL(sysno);
            var postData = string.Format("text={0}&rating={1}", review, rating);
            return await RequestManager.SendPostRequestAsync(url, postData);
        }

        /// <summary>
        /// Sends request to receive book reviews.
        /// </summary>
        /// <param name="sysno">database id of book</param>
        /// <param name="limit">number of results</param>
        /// <param name="offset">number of skipped results</param>
        /// <returns>book reviews</returns>
        public async Task<IEnumerable<Review>> GetBookReviews(string sysno, uint limit, uint offset)
        {
            if (sysno == null)
                throw new ArgumentNullException("sysno");
            if (string.IsNullOrWhiteSpace(sysno))
                throw new ArgumentException("sysno");

            string url = CreateBookReviewsURL(sysno, limit, offset);

            var reviews = await RequestManager.DownloadDataAsync<IEnumerable<Review>>(url);
            return reviews;
        }

        /// <summary>
        /// Sends request to receive book ratings.
        /// </summary>
        /// <param name="sysno">database id of book</param>
        /// <returns>book ratings</returns>
        public async Task<IEnumerable<int>> GetBookRatings(string sysno)
        {
            if (sysno == null)
                throw new ArgumentNullException("sysno");
            if (string.IsNullOrWhiteSpace(sysno))
                throw new ArgumentException("sysno");

            string url = CreateBookRatingsURL(sysno);

            var ratings = await RequestManager.DownloadDataAsync<IEnumerable<int>>(url);
            return ratings;
        }
    }
}
