using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechMarket.Models.Siniflar
{
    public class OrderDetails
    {
        public int OrderDetailsId { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }
    }
}