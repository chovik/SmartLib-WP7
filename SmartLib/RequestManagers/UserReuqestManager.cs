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

namespace SmartLib.DataManagers
{
    public class UserReuqestManager : BaseDataManager
    {

        /// <summary>
        /// Constructor. For more information see parent constructor.
        /// </summary>
        /// <param name="requestManager"></param>
        /// <param name="serverAddress"></param>
        public UserReuqestManager(RequestManager requestManager, string serverAddress)
                : base(requestManager, serverAddress)
        {

        }

        /// <summary>
        /// Creates URL used to check if user is logged in.
        /// </summary>
        /// <returns>URL used to check if user is logged in</returns>
        private string CreateCheckAuthenticationURL()
        {
            return string.Format("http://{0}/api/user/authentication",
                ServerAddress);
        }

        /// <summary>
        /// Creates URL used to log user in.
        /// </summary>
        /// <returns>URL used to log user in</returns>
        private string CreateLoginURL()
        {
            return string.Format("http://{0}/api/user/login",
                ServerAddress);
        }

        /// <summary>
        /// Creates URL used to change user password.
        /// </summary>
        /// <returns>URL used to change user password</returns>
        private string CreateChangePasswordURL()
        {
            return string.Format("http://{0}/api/user/changepassword",
                ServerAddress);
        }

        /// <summary>
        /// Creaters URL used to sign user up.
        /// </summary>
        /// <returns>URL used to sign user up</returns>
        private string CreateRegistrationURL()
        {
            return string.Format("http://{0}/api/user/registration",
                ServerAddress);
        }

        /// <summary>
        /// Sends request to change password.
        /// </summary>
        /// <param name="uco">user uco</param>
        /// <param name="oldPassword">user old password</param>
        /// <param name="newPassword">user new password</param>
        /// <returns>status code of server response. 
        /// 200 OK - success. 409 Unauthorized - bad login</returns>
        public async Task<HttpStatusCode> ChangePassword(string uco, string oldPassword, string newPassword)
        {
            if (uco == null)
                throw new ArgumentNullException("uco");
            if (oldPassword == null)
                throw new ArgumentNullException("oldPassword");
            if (newPassword == null)
                throw new ArgumentNullException("newPassword");

            string url = CreateChangePasswordURL();

            var postData = string.Format("uco={0}&oldPassword={1}&newPassword={2}", uco, oldPassword, newPassword);

            return await RequestManager.SendPostRequestAsync(url, postData);
        }

        /// <summary>
        /// Sends request to log user in.
        /// </summary>
        /// <param name="uco">user uco</param>
        /// <param name="password">user password</param>
        /// <returns>status code of server response. 
        /// 200 OK - success. 409 Unauthorized - bad login</returns>
        public async Task<HttpStatusCode> Login(string uco, string password)
        {
            if (uco == null)
                throw new ArgumentNullException("uco");
            if (password == null)
                throw new ArgumentNullException("password");

            string url = CreateLoginURL();

            var postData = string.Format("uco={0}&password={1}", uco, password);

            return await RequestManager.SendPostRequestAsync(url, postData);
        }

        /// <summary>
        /// Checks if user is logged in.
        /// </summary>
        /// <returns>status code of server response. 
        /// 200 OK - is logged in. 409 - is not logged in</returns>
        public async Task<HttpStatusCode> CheckAuthentication()
        {
            string url = CreateCheckAuthenticationURL();

            return await RequestManager.SendPostRequestAsync(url, null);
        }

        /// <summary>
        /// Sends request to singn user up.
        /// </summary>
        /// <param name="uco">uco</param>
        /// <param name="firstName">firstName</param>
        /// <param name="lastName">lastName</param>
        /// <returns>status code of server response. 
        /// 200 OK - success. 400 Bad Request - bad sent data. 409 - uco is already signed up</returns>
        public async Task<HttpStatusCode> RegisterNewUser(uint uco, string firstName, string lastName)
        {
            if (firstName == null)
                throw new ArgumentNullException("firstName");
            if (lastName == null)
                throw new ArgumentNullException("lastName");

            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("firstName");
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("lastName");

            string url = CreateRegistrationURL();

            var postData = string.Format("uco={0}&firstName={1}&lastName={2}", uco, firstName, lastName);

            return await RequestManager.SendPostRequestAsync(url, postData);
        }
    }
}
