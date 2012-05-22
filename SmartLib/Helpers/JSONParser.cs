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
using System.Runtime.Serialization.Json;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace SmartLib.Helpers
{
    public static class JSONParser
    {
        /// <summary>
        /// Parses JSON data to object of specified type.
        /// </summary>
        /// <typeparam name="T">type of returned object</typeparam>
        /// <param name="json">JSON data</param>
        /// <returns>object of specified type parsed from JSON data</returns>
        public static async Task<T> ParseDataAsync<T>(string json)
        {
            //if (json == null)
            //    throw new ArgumentNullException("json");

            Debug.WriteLine("Parsing data. JSON:\n {0}\n", json);

            T data = default(T);
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
                //{
                data = await TaskEx.Run(() => (T)serializer.ReadObject(ms)); //why?
                Debug.WriteLine("Parsing data - Successful. JSON:\n {0}\n", json);
                //}
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Parsing data - Error. JSON:\n {0}\n Exceptin:\n {1}\n", json, ex.Message);
                Debug.WriteLine("Parsing exception: {0}", ex);
            }

            return data;
        }
    }
}
