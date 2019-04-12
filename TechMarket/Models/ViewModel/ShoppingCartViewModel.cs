using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TechMarket.Models.Siniflar;

namespace TechMarket.Models.ViewModel
{
    public class ShoppingCartViewModel
    {
        public List<Cart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }
}