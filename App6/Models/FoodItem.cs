using App6.Singleton;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace App6.Models
{
    public class FoodItem
    {
        [JsonIgnore]
        private ProductService ProductService;
        public Guid Id { get; set; }
        public string Username { get; set; }
        public int ProductId { get; set; }
        [JsonIgnore]
        public Product Product { get; set; }
        public int Weight { get; set; }
        public DateTime Date { get; set; }
        public MealType MealType { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            return Id == (obj as FoodItem).Id;
        }

        [JsonConstructor]
        public FoodItem(Guid id, string username, int productId, int weight, DateTime date, MealType mealType) 
        {
            ProductService = ProductService.getInstance();
            Id = id;
            Username = username;
            ProductId = productId;
            Product = ProductService.Products.SingleOrDefault(p => p.Id == productId);
            Weight = weight;
            Date = date;
            MealType = mealType;
        }
    }

    public enum MealType
    {
        Breakfast = 0,
        Lunch = 1,
        Dinner = 2,
        Snack = 3
    }
}
