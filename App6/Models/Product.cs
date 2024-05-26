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
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Bgu { get; set; }
        public double Kcal { get; set; }

        public double GetProtein()
        {
            return Convert.ToDouble(Bgu.Replace(",", " ").Replace(".", ",").Split(" ")[0]);
        }
        public double GetFat()
        {
            return Convert.ToDouble(Bgu.Replace(",", " ").Replace(".", ",").Split(" ")[1]);
        }
        public double GetCarb()
        {
            return Convert.ToDouble(Bgu.Replace(",", " ").Replace(".", ",").Split(" ")[2]);
        }
    }
}