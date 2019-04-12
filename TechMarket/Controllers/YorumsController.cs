using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TechMarket.Models;
using TechMarket.Models.Siniflar;

namespace TechMarket.Controllers
{
    public class YorumsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Yorums
        public ActionResult Index(int id)
        {
            return PartialView(db.Yorums.Where(x => x.Product.ProductId == id).ToList());
        }

        // GET: Yorums/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yorum yorum = db.Yorums.Find(id);
            if (yorum == null)
            {
                return HttpNotFound();
            }
            return View(yorum);
        }
        [ChildActionOnly]
        // GET: Yorums/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: Yorums/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ChildActionOnly]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "YorumId,Aciklama")] Yorum yorum, int id)
        {          
            if (ModelState.IsValid)
            {
                try
                {
                    string usid = User.Identity.GetUserId();
                    var us = (from a in db.Users
                              where a.Id == usid
                              select a).FirstOrDefault();
                    yorum.ApplicationUser = us;
                    var pro = (from a in db.Products
                               where a.ProductId == id
                               select a).FirstOrDefault();
                    yorum.Product = pro;
                    db.Yorums.Add(yorum);
                    db.SaveChanges();
                    if (Request.Cookies["UserSettings"] != null)
                    {
                        HttpCookie myCookie = new HttpCookie("UserSettings");
                        myCookie.Expires = DateTime.Now.AddDays(-1d);
                        Response.Cookies.Add(myCookie);

                    }
                    return View();
                }
                catch
                {
                    return View(yorum);
                }
            }
            return View(yorum);
        }
        // GET: Yorums/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yorum yorum = db.Yorums.Find(id);
            if (yorum == null)
            {
                return HttpNotFound();
            }
            return View(yorum);
        }

        // POST: Yorums/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "YorumId,Aciklama")] Yorum yorum)
        {
            if (ModelState.IsValid)
            {
                db.Entry(yorum).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(yorum);
        }

        // GET: Yorums/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yorum yorum = db.Yorums.Find(id);
            if (yorum == null)
            {
                return HttpNotFound();
            }
            return View(yorum);
        }

        // POST: Yorums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Yorum yorum = db.Yorums.Find(id);
            db.Yorums.Remove(yorum);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [HttpGet]
        public JsonResult Idyegore(int id)
        {
            var yrm = db.Yorums.Where(x => x.Product.ProductId == id).ToList();
            return Json(yrm,JsonRequestBehavior.AllowGet);
        }
    }
}
