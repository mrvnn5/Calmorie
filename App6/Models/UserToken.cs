using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App6.Models
{
    public class UserToken
    {
        public string Username { get; set; }
        public string AccessToken { get; set; }
        public DateTime ExpiredAt { get; set; }
    }
}