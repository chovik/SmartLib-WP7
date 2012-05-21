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

namespace SmartLib.Models
{
    [DataContract]
    public class Author
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "author_type")]
        public string Type { get; set; }

        public override string ToString()
        {
            return string.Format("{0}", this.Name);
        }
    }
}
