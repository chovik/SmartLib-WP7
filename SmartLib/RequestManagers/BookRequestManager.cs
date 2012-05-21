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
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using SmartLib.Models;
using System.Diagnostics;
using System.Linq;
using System.Net.Browser;
using SmartLib.DataManagers;

namespace SmartLib
{
    /// <summary>
    /// Book identifiers.
    /// </summary>
    public enum BookIdentifier
    {
        Isbn,
        SysNo,
        Barcode
    }

    public enum BookListCategory
    {
        Top,
        News
    }

    public class BookRequestManager : BaseDataManager
    {
        private static readonly Dictionary<BookIdentifier, string> BookIdentifierToStringDictionary
            = new Dictionary<BookIdentifier, string>()
            {
                { BookIdentifier.Isbn, "isbn" },
                { BookIdentifier.SysNo, "sysno" },
                { BookIdentifier.Barcode, "barcode" }
            };

        private static readonly Dictionary<BookListCategory, string> BookListCategoryToStringDictionary
            = new Dictionary<BookListCategory, string>()
            {
                { BookListCategory.Top, "top" },
                { BookListCategory.News, "news" }
            };

        /// <summary>
        /// Constructor. For more information see parent constructor.
        /// </summary>
        /// <param name="requestManager"></param>
        /// <param name="serverAddress"></param>
        public BookRequestManager(RequestManager requestManager, string serverAddress)
            : base(requestManager, serverAddress)
        {

        }

        /// <summary>
        /// Creates URL used to receive book details.
        /// </summary>
        /// <param name="identifierKind">kind of identifier</param>
        /// <param name="identifierValue">value of identifier</param>
        /// <returns>URL used to receive book details</returns>
        private string CreateBookURL(BookIdentifier identifierKind, string identifierValue)
        {
            return string.Format("http://{0}/api/books/?{1}={2}",
                ServerAddress,
                BookIdentifierToStringDictionary[identifierKind],
                identifierValue);
        }


        /// <summary>
        /// Creates URL used to search book.
        /// </summary>
        /// <param name="title">title used as search parameter</param>
        /// <param name="author">author used as search parameter</param>
        /// <param name="limit">number of results</param>
        /// <param name="offset">number of skipped results</param>
        /// <returns>URL used to search book</returns>
        private string CreateSearchBooksURL(string title, string author, uint limit = 10, uint offset = 0)
        {
            return string.Format("http://{0}/api/books/search?title={1}&author={2}&limit={3}&offset={4}",
                ServerAddress,
                title,
                author,
                limit,
                offset);
        }

        /// <summary>
        /// Creates URL used to receive book details (data not received in search request).
        /// </summary>
        /// <param name="sysno">database id of book</param>
        /// <returns>URL used to receive book details (data not received in search request)</returns>
        public string CreateBookDetailsURL(string sysno)
        {
            if (sysno == null)
                throw new ArgumentNullException("sysno");
            if (string.IsNullOrWhiteSpace(sysno))
                throw new ArgumentException("sysno");

            return string.Format("http://{0}/api/books/{1}/details", ServerAddress, sysno);
        }

        /// <summary>
        /// Creates URL to receive top/new books.
        /// </summary>
        /// <param name="category">books category (top/news)</param>
        /// <param name="limit">number of results</param>
        /// <param name="offset">number of skipped results</param>
        /// <returns>URL to receive top/new books</returns>
        private string CreateBookListURL(BookListCategory category,
                                                uint limit,
                                                uint offset
                                               )
        {
            return string.Format("http://{0}/api/books/list?category={1}",//&limit={2}&offset={3}",
                ServerAddress,
                BookListCategoryToStringDictionary[category]);
        }

        /// <summary>
        /// Sends request to search books according title and author.
        /// </summary>
        /// <param name="title">title used as search parameter</param>
        /// <param name="author">author used as search parameter</param>
        /// <param name="limit">number of results</param>
        /// <param name="offset">number of skipped results</param>
        /// <returns></returns>
        public async Task<IEnumerable<Book>> SearchBooks(string title, string author, uint limit, uint offset)
        {
            if (title == null
                && author == null)
                throw new ArgumentNullException("title or author", "At least one of parameter cannot be null.");

            string url = CreateSearchBooksURL(title, author, limit, offset);

            var books = await RequestManager.DownloadDataAsync<IEnumerable<Book>>(url);
            return books;
        }

        /// <summary>
        /// Sends request to receive another information about book. (additional information).
        /// </summary>
        /// <param name="book">book to be updated</param>
        /// <returns>Book with additional information.</returns>
        public async Task<Book> GetBookDetails(Book book)
        {
            if (book == null)
                throw new ArgumentNullException("book");
            if (book.Sysno == null)
                throw new ArgumentException("book");

            string url = CreateBookDetailsURL(book.Sysno);

            var details = await RequestManager.DownloadDataAsync<Book>(url);

            book.Publisher = details.Publisher;
            book.PublishedDate = details.PublishedDate;
            book.Language = details.Language;
            book.PageType = details.PageType;
            book.PageCount = details.PageCount;

            return book;
        }

        /// <summary>
        /// Sends request to receive book according its identifier.
        /// </summary>
        /// <param name="identifierKind">kind of identifier</param>
        /// <param name="identifierValue">value of identifier</param>
        /// <returns>book with specified identifier</returns>
        public async Task<Book> GetBookBy(BookIdentifier identifierKind, string identifierValue)
        {
            if (identifierValue == null)
                throw new ArgumentNullException("identifierValue");
            if (string.IsNullOrWhiteSpace(identifierValue))
                throw new ArgumentException("identifierValue");

            if (!BookIdentifierToStringDictionary.ContainsKey(identifierKind))
                throw new ArgumentException("identifierKind");

            string url = CreateBookURL(identifierKind, identifierValue);

            var book = await RequestManager.DownloadDataAsync<Book>(url);
            return book;
        }

        /// <summary>
        /// Sends request to receive books of specified category.
        /// </summary>
        /// <param name="category">books category</param>
        /// <param name="limit">number of results</param>
        /// <param name="offset">number of skipped results</param>
        /// <returns>books of specified category</returns>
        public async Task<IEnumerable<Book>> GetBooksByCategory(BookListCategory category, uint limit, uint offset)
        {
            if (!BookListCategoryToStringDictionary.ContainsKey(category))
                throw new ArgumentException("category");

            string url = CreateBookListURL(category, limit, offset);
            var books = await RequestManager.DownloadDataAsync<IEnumerable<Book>>(url);
            return books;

        }
    }
}
