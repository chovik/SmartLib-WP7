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
    public class Review
    {
        [DataMember(Name = "commentId")]
        public int ID { get; set; }

        [DataMember(Name = "user")]
        public User User { get; set; }

        [DataMember(Name = "text")]
        public string Text { get; set; }

        [DataMember(Name = "rating")]
        public double Rating { get; set; }

        [DataMember(Name = "date")]
        public DateTime Date { get; set; }
    }
}
