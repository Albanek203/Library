using System;
using System.Windows.Controls;
using Library.Data.Abstract;
using Library.Data.Enumeration;

namespace Library.Data.Models {
    public class User : Person {
        public int               UserId                 { get; set; }
        public string            Login                  { get; set; }
        public string            Password               { get; set; }
        public string            Email                  { get; set; }
        public Image             Image                  { get; set; }
        public string            Address                { get; set; }
        public string            Phone                  { get; set; }
        public bool              AdvancedAccess         { get; set; }
        public int               Money                  { get; set; }
        public SubscriptionNames SubscriptionName       { get; set; }
        public DateTime          SubscriptionValidUntil { get; set; }
    }
}