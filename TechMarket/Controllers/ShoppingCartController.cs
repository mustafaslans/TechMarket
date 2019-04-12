using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TechMarket.Models;
using TechMarket.Models.Siniflar;
using TechMarket.Models.ViewModel;

namespace TechMarket.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            //View model çalışıp bana bilgi vericek
            var cart = ShoppingCart.GetCart(this.HttpContext);
            var cartview = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal(),
               
            };
            ViewBag.Total = cartview.CartTotal;
            return View(cartview);
      
        }
        //Addcart
        public ActionResult AddToCart(int id)
        {
            var eklenen = db.Products.FirstOrDefault(x => x.ProductId == id);
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.AddToCart(eklenen);
            return RedirectToAction("Index", "Home");
            //sepete ekledik
        }

        //remove
        public ActionResult RemoveToCart(int id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            string itemmarka = db.Carts.FirstOrDefault(x => x.RecordId == id).Product.Marka;
            string itemmodel = db.Carts.FirstOrDefault(x => x.RecordId == id).Product.Model;
            int itemcount = cart.RemoveFromCart(id);
            var result = new RemoveViewModel
            {
                CartTotal = cart.GetTotal(),
                Count = cart.GetCount(),
                ItemCount = itemcount,
                DeleteId = id,
                Message = Server.HtmlEncode(itemmarka)+" "+Server.HtmlEncode(itemmodel) + " sepetinizden kaldırıldı"
            };
            return Json(result);
        }
        //cart summary
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}