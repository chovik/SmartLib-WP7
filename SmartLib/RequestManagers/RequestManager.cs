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
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Net.Browser;
using System.Diagnostics;
using System.Linq;
using SmartLib.Helpers;

namespace SmartLib.DataManagers
{
    public class RequestManager
    {
        // does not work
        //private CookieContainer cookies = new CookieContainer();

        ///// <summary>
        ///// Cookies containing authorization data. They are used in request, that requires authetization.
        ///// </summary>
        //private CookieContainer Cookies
        //{
        //    get
        //    {
        //        return cookies;
        //    }
        //}

        /// <summary>
        /// Cookies containing authorization data. They are used in request, that requires authetization.
        /// </summary>
        private string cookiesHeader = null;

        /// <summary>
        /// Returns status code of web response.
        /// </summary>
        /// <param name="response">web response</param>
        /// <returns>status code of web response</returns>
        private static HttpStatusCode WebResponseToHTTPStatusCode(WebResponse response)
        {
            if (response == null)
                throw new ArgumentNullException("response");

            if (response.Headers.AllKeys.Contains("StatusCode"))
            {
                return (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), response.Headers["StatusCode"], true);
            }

            return HttpStatusCode.Forbidden;
        }

        /// <summary>
        /// Sends asynchronous request to specified url and with specified POST parameters.
        /// Method of sending request is POST.
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="postData">POST parameters</param>
        /// <returns>satus code of server response</returns>
        public async Task<HttpStatusCode> SendPostRequestAsync(string url, string postData)
        {
            ArgumentValidator.AssertNotNull(url, "url");
            ArgumentValidator.AssertNotNullOrWhiteSpaceString(url, "url");

            bool httpResult = HttpWebRequest.RegisterPrefix("http://", WebRequestCreator.ClientHttp);
            HttpStatusCode statusCodeToReturn = HttpStatusCode.Forbidden;

            ///prepare request
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            webRequest.Method = "POST";
            //webRequest.CookieContainer = Cookies;

            if (cookiesHeader != null)
            {
                Debug.WriteLine("POST - Preparing cookies to send to {0}.:", url);
                Debug.WriteLine(cookiesHeader);
                webRequest.Headers["Cookie"] = cookiesHeader;
            }

            ///add POST parameters to send
            if (!string.IsNullOrWhiteSpace(postData))
            {
                Debug.WriteLine("POST - Preparing data to send to {0}", url);
                Debug.WriteLine(postData);
                using (var requestStream = await webRequest.GetRequestStreamAsync())
                {
                    var bytes = Encoding.UTF8.GetBytes(postData);
                    requestStream.Write(bytes, 0, bytes.Length);
                }
            }

            ///send request and wait for response
            Debug.WriteLine("POST - Sending data to {0}.", url);
            using (WebResponse response = await webRequest.GetResponseAsync())
            {
                Debug.WriteLine("POST - Received response from {0}.", url);
                if (response.Headers.AllKeys.Contains("Set-Cookie"))
                {
                    ///save cookies
                    cookiesHeader = response.Headers["Set-Cookie"];
                    Debug.WriteLine("Set-Cookie Header:");
                    Debug.WriteLine(response.Headers["Set-Cookie"]);
                }

                //time synchronization. it is important for relativetime convertor. future time is not supported.
                if (response.Headers.AllKeys.Contains("Date"))
                {
                    string serverDateString = response.Headers["Date"];
                    var serverDateTime = DateTime.Parse(serverDateString);
                    App.CurrentApplication.TimeDifference = serverDateTime - DateTime.Now;

                }
                ///get status code of server response
                statusCodeToReturn = WebResponseToHTTPStatusCode(response);
            }

            Debug.WriteLine("POST - Response status code: {0} ({1})", statusCodeToReturn, url);
            return statusCodeToReturn;
        }

        /// <summary>
        /// Download content returns by sending GET request to specified URL.
        /// </summary>
        /// <typeparam name="T">type of returned data</typeparam>
        /// <param name="url">url</param>
        /// <returns>returned data</returns>
        public async Task<T> DownloadDataAsync<T>(string url)
        {
            if (url == null)
                throw new ArgumentNullException("url");

            var newUrl = url.Contains("?") ?
                string.Format("{0}&d={1}", url, DateTime.Now) :
                 string.Format("{0}?d={1}", url, DateTime.Now); //to avoid caching

            string data = null;

            ///prepare web request
            WebRequest webRequest = WebRequest.CreateHttp(newUrl);

            ///send request and wait for response
            Debug.WriteLine("GET - Waiting for server response {0}.", newUrl);
            using (WebResponse response = await webRequest.GetResponseAsync())
            {
                HttpStatusCode statusCode = WebResponseToHTTPStatusCode(response);

                Debug.WriteLine("GET - status code {0} ({1}).", statusCode, newUrl);

                ///if received status code is OK, try to get content of response
                if (statusCode == HttpStatusCode.OK)
                {
                    using (var stm = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stm))
                        {
                            data = await reader.ReadToEndAsync();
                        }
                    }
                }

                if (response.Headers.AllKeys.Contains("Date"))
                {
                    string serverDateString = response.Headers["Date"];
                    var serverDateTime = DateTime.Parse(serverDateString);
                    App.CurrentApplication.TimeDifference = serverDateTime - DateTime.Now;
                }
            }

            if (string.IsNullOrWhiteSpace(data))
            {
                Debug.WriteLine("GET - Data received from server are empty. (url: {0})", newUrl);
            }

            ///parse received data to type T
            var books = await JSONParser.ParseDataAsync<T>(data);
            return books;
        }
    }
}
