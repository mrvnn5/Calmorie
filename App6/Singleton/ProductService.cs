using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using App6.Models;
using Com.Ajithvgiri.Searchdialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App6.Singleton
{
    public class ProductService
    {
        private static ProductService instance;
        public List<SearchListItem> SearchListItems { get; private set; }

        public List<Product> Products;

        private ProductService(List<Product> products)
        {
            Products = products;
            SearchListItems = new List<SearchListItem>();
            Products.ForEach(p => SearchListItems.Add(
                new SearchListItem(p.Id, p.Name + " (" + p.Kcal + " Ккал б-" + p.GetProtein() + " ж-" + p.GetFat() + " у-" + p.GetCarb() + ")")));
        }

        public static ProductService getInstance(List<Product> products = null)
        {
            if (instance == null || instance.Products == null || instance.Products.Count == 0)
            {
                if (products == null || products.Count == 0) return null;
                instance = new ProductService(products);
            }
            return instance;
        }
    }
}