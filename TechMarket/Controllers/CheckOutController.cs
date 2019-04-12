using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using TechMarket.Models;
using TechMarket.Models.Siniflar;
using TechMarket.Models.ViewModel;

namespace TechMarket.Controllers
{
    public class CheckOutController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        const string PromoCode = "FREE";
        // GET: CheckOut
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AddressAndPayment(decimal total)
        {
            ViewBag.Total = total;
            return View();
        }
        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection values, decimal total)
        {
            var order = new Order();
            TryUpdateModel(order);
            try
            {
                order.UserName = User.Identity.Name;
                order.OrderDate = DateTime.Now;
                order.Total = total;
                string[] args = { "E-Ticaret", "İyi günlerde kullanın" };
                Main(args);
                //Save Order
                db.Orders.Add(order);
                db.SaveChanges();
                //Process the order
                var cart = ShoppingCart.GetCart(this.HttpContext);
                cart.CreateOrder(order);
                return RedirectToAction("Complete", new { id = order.OrderId });
            }
            catch
            {
                //Invalid - redisplay with errors
                return View(order);
            }
        }
        public ActionResult Complete(int id)
        {
            // Validate customer owns this order
            bool isValid = db.Orders.Any(
                o => o.OrderId == id &&
                o.UserName == User.Identity.Name);

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }
        public void Main(string[] args)
        {
            string email = User.Identity.Name;
            SmtpClient sc = new SmtpClient();
            sc.Port = 587;
            sc.Host = "smtp.gmail.com";
            sc.EnableSsl = true;
            sc.Credentials = new NetworkCredential("aslantechweb@gmail.com", "aslan12345");
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("aslantechweb@gmail.com", "AslanTech");
            mail.To.Add(email);
            mail.Subject = args[0];
            mail.IsBodyHtml = true;
            mail.Body = args[1];
            sc.Send(mail);
        }
    }
}