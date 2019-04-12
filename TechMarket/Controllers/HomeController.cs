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
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(string category, string markaLap, string markaTel, string marka, int? min, int? max)
        {
            //if (min != null & max != null)
            //{
            //    var list = db.Products.Where(x => x.Price >= min && x.Price <= max).ToList();
            //    return Json(list, JsonRequestBehavior.AllowGet);
            //}
            if (min == null)
            {
                min = 1;
            }
            if (max == null)
            {
                max = 10000;
            }          
            if (!string.IsNullOrEmpty(category))
            {
                var list = db.Products.Where(x => x.CategoryString == category && x.Price >= min && x.Price <= max).ToList();
                return View(list.ToList());
            }
            if (!string.IsNullOrEmpty(markaLap))
            {
                var list = db.Products.Where(x => x.CategoryString == "Laptop" && x.Marka == markaLap && x.Price >= min && x.Price <= max).ToList();
                return View(list.ToList());
            }
            if (!string.IsNullOrEmpty(markaTel))
            {
                var list = db.Products.Where(x => x.CategoryString == "Telefon" && x.Marka == markaTel && x.Price >= min && x.Price <= max).ToList();
                return View(list.ToList());
            }
            if (!string.IsNullOrEmpty(marka))
            {
                var list = db.Products.Where(x => x.Marka == marka && x.Price >= min && x.Price <= max).ToList();
                return View(list.ToList());
            }

            return View(db.Products.Where(x=> x.Price >= min && x.Price <= max).ToList());
        }
    }
}