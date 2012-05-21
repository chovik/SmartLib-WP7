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
using System.IO.IsolatedStorage;

namespace SmartLib.Helpers
{
    public class PersistentDataStorage : IDataStorage
    {

        public bool Backup(string token, object value)
        {
            if (null == value)
                return false;

            var store = IsolatedStorageSettings.ApplicationSettings;
            if (store.Contains(token))
                store[token] = value;
            else
                store.Add(token, value);

            store.Save();
            return true;
        }

        public T Restore<T>(string token)
        {
            var store = IsolatedStorageSettings.ApplicationSettings;
            if (!store.Contains(token))
                return default(T);

            return (T)store[token];
        }
    }
}
