using Android.Hardware.Lights;
using App6.Models;
using App6.Singleton;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace TCoYB.Server.Models
{
    public class WaterItem
    {
        [JsonIgnore]
        private ProductService ProductService;
        public Guid Id { get; set; }
        public string Username { get; set; }
        public int Volume { get; set; }
        public DateTime Date { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            return Id == (obj as WaterItem).Id;
        }

        [JsonConstructor]
        public WaterItem(Guid id, string username, int volume, DateTime date)
        {
            ProductService = ProductService.getInstance();
            Id = id;
            Username = username;
            Volume = volume;
            Date = date;
        }
    }
}
