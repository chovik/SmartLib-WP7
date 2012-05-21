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
using SmartLib.Helpers;

namespace SmartLib.DataManagers
{
    public class BaseDataManager
    {
        private string serverAddress;

        /// <summary>
        /// Address of SmartLib server.
        /// </summary>
        public string ServerAddress
        {
            get { return serverAddress; }
            set 
            {
                ArgumentValidator.AssertNotNull(value, "server");
                ArgumentValidator.AssertNotNullOrWhiteSpaceString(value, "server");
                serverAddress = value; 
            }
        }
        

        private RequestManager requestManager;

        /// <summary>
        /// Manager used tos end/receive data from/to server.
        /// </summary>
        public RequestManager RequestManager 
        {
            get
            {
                return requestManager;
            }
            set
            {
                ArgumentValidator.AssertNotNull(value, "requestManager");
                requestManager = value;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="requestManager">manager used to send/receive data from/to server</param>
        /// <param name="serverAddress">address of SmartLib server</param>
        public BaseDataManager(RequestManager requestManager, string serverAddress)
        {
            RequestManager = requestManager;
            ServerAddress = serverAddress;
        }
    }
}
