using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartLib.Helpers
{
    public interface IMessageService
    {
        /// <summary>
        /// Shows message about success.
        /// </summary>
        /// <param name="msg">message to show</param>
        /// <param name="caption">message title</param>
        void ShowSuccessMessage(string msg, string caption);

        /// <summary>
        /// Shows message about warning.
        /// </summary>
        /// <param name="msg">message to show</param>
        /// <param name="caption">message title</param>
        void ShowWarningMessage(string warningMsg, string caption);

        /// <summary>
        /// Shows message about error.
        /// </summary>
        /// <param name="msg">message to show</param>
        /// <param name="caption">message title</param>
        void ShowErrorMessage(string errorMsg, string caption);
    }
}
