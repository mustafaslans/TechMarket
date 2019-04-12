using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TechMarket.Models.Siniflar
{
    public class ShoppingCart
    {
        ApplicationDbContext db = new ApplicationDbContext();
        string ShoppingCartId { get; set; }
        public const string CartsessionKey = "CartId";
        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var Cart = new ShoppingCart();
            Cart.ShoppingCartId = Cart.GetCartId(context);
            return Cart;
        }
        // Helper method to simplify shopping Cart calls
        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }
        public void AddToCart(Product product)
        {
            // Get the matching Cart and album instances
            var CartItem = db.Carts.SingleOrDefault(
                c => c.CartId == ShoppingCartId
                && c.ProductId == product.ProductId);

            if (CartItem == null)
            {
                // Create a new Cart item if no Cart item exists
                CartItem = new Cart
                {
                    ProductId = product.ProductId,
                    CartId = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
            };
                db.Carts.Add(CartItem);
            }
            else
            {
                // If the item does exist in the Cart, 
                CartItem.Count++;
            }
            db.SaveChanges();
        }
        public int RemoveFromCart(int id)
        {
            var CartItem = db.Carts.Single(
                Cart => Cart.CartId == ShoppingCartId
                && Cart.RecordId == id);

            int itemCount = 0;

            if (CartItem != null)
            {
                if (CartItem.Count > 1)
                {
                    CartItem.Count--;
                    itemCount = CartItem.Count;
                }
                else
                {
                    db.Carts.Remove(CartItem);
                }
                db.SaveChanges();
            }
            return itemCount;
        }
        public void EmptyCart()
        {
            var CartItems = db.Carts.Where(
                Cart => Cart.CartId == ShoppingCartId);

            foreach (var CartItem in CartItems)
            {
                db.Carts.Remove(CartItem);
            }
            db.SaveChanges();
        }
        public List<Cart> GetCartItems()
        {
            return db.Carts.Where(
                Cart => Cart.CartId == ShoppingCartId).ToList();
        }
        public int GetCount()
        {
            // Get the count of each item in the Cart and sum them up
            int? count = (from CartItems in db.Carts
                          where CartItems.CartId == ShoppingCartId
                          select (int?)CartItems.Count).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }
        public decimal GetTotal()
        {
            // Multiply album price by count of that album to get 
            // the current price for each of those albums in the Cart
            // sum all album price totals to get the Cart total
            decimal? total = (from CartItems in db.Carts
                              where CartItems.CartId == ShoppingCartId
                              select (int?)CartItems.Count *
                              CartItems.Product.Price).Sum();

            return total ?? decimal.Zero;
        }
        public int CreateOrder(Order order)
        {
            decimal orderTotal = 0;

            var CartItems = GetCartItems();
            // Iterate over the items in the Cart, 
            // adding the order details for each
            foreach (var item in CartItems)
            {
                var orderDetail = new OrderDetails
                {
                    ProductId = item.ProductId,
                    OrderId = order.OrderId,
                    UnitPrice = item.Product.Price,
                    Quantity = item.Count
                };
                // Set the order total of the shopping Cart
                orderTotal += (item.Count * item.Product.Price);
                db.OrderDetails.Add(orderDetail);
            }
            // Set the order's total to the orderTotal count
            order.Total = orderTotal;
            db.SaveChanges();
            // Empty the shopping Cart
            EmptyCart();
            // Return the OrderId as the confirmation number
            return order.OrderId;
        }
        // We're using HttpContextBase to allow access to cookies.
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartsessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartsessionKey] =
                        context.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();
                    // Send tempCartId back to client as a cookie
                    context.Session[CartsessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartsessionKey].ToString();
        }
        // When a user has logged in, migrate their shopping Cart to
        // be associated with their username
        public void MigrateCart(string userName)
        {
            var shoppingCart = db.Carts.Where(
                c => c.CartId == ShoppingCartId);

            foreach (Cart item in shoppingCart)
            {
                item.CartId = userName;
            }
            db.SaveChanges();
        }
    }
}