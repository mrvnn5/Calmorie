using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using TCoYB.Server.Models;

namespace App6.Models
{
    public class AppUser
    {
        public string Username { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public Sex Sex { get; set; }
        public Activity Activity { get; set; }
        public Plan Plan { get; set; }
        public DateTime BirthDate { get; set; }
        public string? PasswordHash { get; set; }
        public UserToken? UserToken { get; set; }
        public List<FoodItem>? FoodItems { get; set; } = new List<FoodItem>();
        public List<WaterItem>? WaterItems { get; set; } = new List<WaterItem>();

        [JsonConstructor]
        public AppUser(string username, int height, int weight, Sex sex, Activity activity, Plan plan, DateTime birthDate, string passwordHash, UserToken userToken, List<FoodItem> foodItems, List<WaterItem> waterItems)
        {
            Username = username;
            Height = height;
            Weight = weight;
            Sex = sex;
            Activity = activity;
            Plan = plan;
            BirthDate = birthDate;
            PasswordHash = passwordHash;
            UserToken = userToken;
            FoodItems = foodItems;
            WaterItems = waterItems;
        }

        public AppUser(string username)
        {
            Username = username;
            Height = 0;
            Weight = 0;
            Sex = 0;
            Activity = 0;
            Plan = 0;
            BirthDate = DateTime.Now;
            PasswordHash = "";
        }
        public AppUser()
        {
            Username = "";
            Height = 0;
            Weight = 0;
            Sex = 0;
            Activity = 0;
            Plan = 0;
            BirthDate = DateTime.Now;
            PasswordHash = "";
        }


    }
    public enum Sex
    {
        Male = 0,
        Female = 1
    }
    public enum Activity
    {
        Low = 0,
        Middle = 1,
        High = 2,
        VeryHigh = 3
    }
    public enum Plan
    {
        Loss = 0,
        Support = 1,
        Gain = 2
    }
}
