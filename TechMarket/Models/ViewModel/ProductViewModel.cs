using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechMarket.Models.ViewModel
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public string Photo { get; set; }
        public string Description { get; set; }
        public string CategoryString { get; set; }
        public decimal Price { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int? min { get; set; }
        public int? max { get; set; }
    }
}