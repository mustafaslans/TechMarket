using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TechMarket.Models.Enums;

namespace TechMarket.Models.Siniflar
{
    public class Product
    {
        public int ProductId { get; set; }
        [Column("Category")]
        public string CategoryString
        {
            get { return Category.ToString(); }
            private set { Category = value.ParseEnum<Category>(); }
        }
        [NotMapped]
        public Category Category { get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public string Photo { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
    public static class StringExtensions
    {
        public static T ParseEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }

}