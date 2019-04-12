using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using TechMarket.Models;
using TechMarket.Models.Enums;
using TechMarket.Models.Siniflar;

namespace TechMarket.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Products
        public ActionResult Index()
        {
            string ilanveren = User.Identity.GetUserId();
            var product = db.Products.Where(x => x.ApplicationUserId == ilanveren).ToList();
            return View(product);
        }
        public ActionResult ProductList(string category)
        {
            var list = db.Products.Where(x => x.CategoryString == category).ToList();
            return View(list.ToList());
        }
        public ActionResult TekUrun(int id)
        {
            var ac = db.Products.Where(x => x.ProductId == id).FirstOrDefault();
            return View(ac);
        }
        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,Category,Marka,Model,Photo,Description,Price")] Product product, HttpPostedFileBase image)
        {
            if (image != null)
            {
                WebImage img = new WebImage(image.InputStream);
                FileInfo fotoinfo = new FileInfo(image.FileName);
                string newfoto = Guid.NewGuid().ToString() + fotoinfo.Extension;
                img.Resize(500, 775);
                img.Save("../Uploads/Photo/" + newfoto);
                product.Photo = "../Uploads/Photo/" + newfoto;
                string id = User.Identity.GetUserId();
                var insan = db.Users.Where(x => x.Id == id).FirstOrDefault();
                product.ApplicationUser = insan;
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // GET: Products/Edit/5
        public ActionResult getbyID(int? id)
        {
            int id2 = Convert.ToInt32(id);
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            Product product = db.Products.Find(id2);
            if (product == null)
            {
                return HttpNotFound();
            }
            return Json(product, JsonRequestBehavior.AllowGet);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            try
            {
                var pro = db.Products.Where(x => x.ProductId == product.ProductId).FirstOrDefault();
                pro.Marka = product.Marka;
                pro.Model = product.Model;
                pro.Description = product.Description;
                pro.Category = product.Category;
                pro.ApplicationUser = product.ApplicationUser;
                pro.ApplicationUserId = product.ApplicationUserId;
                pro.Price = product.Price;
                //pro.Photo = product.Photo;
                db.SaveChanges();
                return Json(pro, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // GET: Products/Delete/5
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost]

        public ActionResult DeleteConfirmed(Product product)
        {
            int id = Convert.ToInt32(product.ProductId);
            Product product2 = db.Products.Find(id);
            var res = (from a in db.Yorums
                       where a.Product.ProductId == id
                       select a).ToList();
            db.Yorums.RemoveRange(res);
            db.Products.Remove(product2);
            db.SaveChanges();
            return Json(JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public JsonResult GetAll()
        {
            var productList = db.Products.Select(x => new
            {
                x.ProductId,
                x.Marka,
                x.Model,
                x.Description,
                x.CategoryString,
                x.Price,
                x.Photo

            }).ToList();
            return Json(productList, JsonRequestBehavior.AllowGet);
        }
    }
}
