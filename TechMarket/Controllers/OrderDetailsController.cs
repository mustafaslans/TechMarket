using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TechMarket.Models;
using TechMarket.Models.Siniflar;

namespace TechMarket.Controllers
{
    public class OrderDetailsController : Controller
    {
        // GET: OrderDetails
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SiparisOlustur(int siparis)
        {
            var cart = (from a in db.Products
                          join b in db.Carts on a.ProductId equals b.ProductId
                          where a.ProductId == siparis
                          select b).FirstOrDefault();
            var prod = (from a in db.Products
                        where a.ProductId == siparis
                        select a).FirstOrDefault();
            OrderDetails od = new OrderDetails
            {
                ProductId = siparis,
                Product = cart.Product,
                Quantity = cart.Count,
                UnitPrice = prod.Price
            };
            db.OrderDetails.Add(od);
            db.SaveChanges();
            return Json(od,JsonRequestBehavior.AllowGet);
        }
    }
}