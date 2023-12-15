using DishDash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DishDash.Controllers
{
    public class HomeController : Controller
    {
        private DishDashEntities data;
        public HomeController()
        {
            data = new DishDashEntities();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Who()
        {
            return View();
        }
        public ActionResult gallery()
        {
            return View();
        }
        public ActionResult userDetails()
        {
            return View();
        }

        public ActionResult login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var admin = data.Admins.FirstOrDefault(a => a.username == email && a.password == password);

            if (admin != null)
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                var user = data.Users.FirstOrDefault(u => u.email == email && u.password == password);

                if (user != null)
                {
                    Session["isAuth"] = true;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Error = "Please! check username and password!";
                    return View("Login");
                }
            }
        }

        public ActionResult storelocator()
        {
            return View();
        }

        public ActionResult terms()
        {
            return View();
        }
        public ActionResult signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult signup(User newUser)
        {
            if (ModelState.IsValid)
            {
                newUser.create_at = DateTime.Now;

                data.Users.Add(newUser);
                data.SaveChanges();
                Session["isAuth"] = true;
                return RedirectToAction("Index", "Home");
            }

            return View(newUser);
        }
        public ActionResult Menu(int? categoryId)
        {
            //if (Session["isAuth"] != null)
            {
                var viewModel = new DishDash.Models.MenuModel();

                if (categoryId.HasValue)
                {
                    var category = data.Categories
                        .Include("Products")
                        .FirstOrDefault(c => c.id == categoryId);

                    if (category != null)
                    {
                        viewModel.Categories = data.Categories.Include("Products").ToList();
                        viewModel.Products = category.Products.ToList();
                    }
                }
                else
                {
                    viewModel.Categories = data.Categories.Include("Products").ToList();
                    viewModel.Products = data.Products.ToList();
                }
                int userId = 1;
                decimal totalAmount = 0;
                var userCartItems = data.AddToCarts.Where(c => c.user_id == userId && c.quantity > 0).ToList();

                if (userCartItems.Any())
                {
                    foreach (var item in userCartItems)
                    {
                        var product = data.Products.FirstOrDefault(p => p.id == item.product_id);

                        if (product != null)
                        {
                            totalAmount += item.quantity * product.price;
                        }
                    }
                }

                ViewBag.TotalCartAmount = totalAmount;

                var cartItems = userCartItems
                    .GroupBy(item => item.product_id)
                    .Select(group => new { ProductId = group.Key, Quantity = group.Sum(item => item.quantity) })
                    .ToList();

                viewModel.AddCartProduct = viewModel.Products
                    .Where(product => cartItems.Any(cart => cart.ProductId == product.id))
                    .Select(product => new DishDash.Models.ProductViewModel(product, cartItems.FirstOrDefault(cart => cart.ProductId == product.id)?.Quantity ?? 0))
                    .ToList();

                return View(viewModel);
            }
            //return View("Login");
        }
        public ActionResult AddToCart(int productId)
        {
            int userId = 1;
            var existingCartItem = data.AddToCarts.FirstOrDefault(c => c.user_id == userId && c.product_id == productId);

            if (existingCartItem != null)
            {
                return RedirectToAction("Menu");
            }
            else
            {
                var productToAdd = data.Products.FirstOrDefault(p => p.id == productId);

                if (productToAdd != null)
                {
                    var addToCart = new AddToCart
                    {
                        user_id = userId,
                        product_id = productId,
                        quantity = 1,
                        added_at = DateTime.Now
                    };

                    data.AddToCarts.Add(addToCart);
                    data.SaveChanges();
                }
                else
                {
                    ViewBag.Error = "Product not found!";
                    return View("Error");
                }
            }

            return RedirectToAction("Menu");
        }

        public ActionResult UpdateCartItem(int productId, int quantityChange)
        {
            int userId = 1;

            var existingCartItem = data.AddToCarts.FirstOrDefault(c => c.user_id == userId && c.product_id == productId);

            if (existingCartItem != null)
            {
                existingCartItem.quantity += quantityChange;
                data.SaveChanges();
                return RedirectToAction("Menu");
            }
            return RedirectToAction("Menu");
        }

        public ActionResult DeleteAllCartItems()
        {
            int userId = 1;
            var userCartItems = data.AddToCarts.Where(c => c.user_id == userId).ToList();

            data.AddToCarts.RemoveRange(userCartItems);
            data.SaveChanges();
            return RedirectToAction("Menu");
        }
        public ActionResult feedback()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session["isAuth"] = false;
            return View("Index");
        }
        [HttpPost]
        public ActionResult AddCustomer(string name, string contact, string houseDetails, string areaDetails, string landmark, string pincode, string city)
        {
            if (ModelState.IsValid)
            {
                int userId = 1;
                if (name == "")
                {
                    var user = data.Users.FirstOrDefault(u => u.id == userId);
                    name = user.name;
                }

                var newCustomer = new Customer
                {
                    user_id = userId,
                    address = houseDetails + " " + areaDetails + " " + landmark + " " + pincode + " " + city,
                    name = name,
                    contact = contact,
                };
                data.Customers.Add(newCustomer);
                data.SaveChanges();

                return RedirectToAction("payment", "Home");
            }
            else
            {
                return View();
            }
        }
        public ActionResult payment()
        {
            int userId = 1;
            decimal totalAmount = 0;
            var userCartItems = data.AddToCarts.Where(c => c.user_id == userId && c.quantity > 0).ToList();

            if (userCartItems.Any())
            {
                foreach (var item in userCartItems)
                {
                    var product = data.Products.FirstOrDefault(p => p.id == item.product_id);

                    if (product != null)
                    {
                        totalAmount += item.quantity * product.price;
                    }
                }
            }
            ViewBag.TotalAmount = totalAmount;
            decimal tax = totalAmount * 18 / 100;
            ViewBag.Tax = tax;
            decimal finalBill = totalAmount + tax;
            ViewBag.FinalBill = finalBill;
            return View();
        }

        [HttpPost]
        public ActionResult ProcessPayment(string selectedPayment, decimal finalBill)
        {
            if (ModelState.IsValid)
            {
                int userId = 1;

                var latestCustomerId = data.Customers
                   .Where(c => c.user_id == userId)
                   .OrderByDescending(c => c.id)
                   .Select(c => c.id)
                   .FirstOrDefault();

                var newPayment = new Payment
                {
                    customer_id = latestCustomerId,
                    amount = finalBill,
                    method = selectedPayment,
                    create_at = DateTime.Now
                };
                data.Payments.Add(newPayment);
                data.SaveChanges();

                int totalItems = data.AddToCarts
                    .Where(cart => cart.user_id == userId)
                    .Sum(cart => cart.quantity);

                var order = new Order
                {
                    no_of_items = totalItems,
                    total_amount = finalBill,
                    payment_id = newPayment.id
                };
                data.Orders.Add(order);
                data.SaveChanges();

                var cartItems = data.AddToCarts.Where(cart => cart.user_id == userId && cart.quantity > 0).ToList();

                foreach (var cartItem in cartItems)
                {
                    var product = data.Products.FirstOrDefault(p => p.id == cartItem.product_id);

                    if (product != null)
                    {
                        var orderedItem = new Ordered_Items
                        {
                            product_id = cartItem.product_id,
                            quantity = cartItem.quantity,
                            order_id = order.id,
                            price = product.price,
                            status = "not Ready",
                            order_date_time = DateTime.Now
                        };
                        data.Ordered_Items.Add(orderedItem);
                    }
                }

                data.SaveChanges();

                data.AddToCarts.RemoveRange(cartItems);
                data.SaveChanges();


                var newReceipt = new Receipt
                {
                    ordered_id = order.id,
                    customer_id = latestCustomerId,
                    payment_id = newPayment.id,
                    create_at = DateTime.Now
                };

                data.Receipts.Add(newReceipt);
                data.SaveChanges();

                return RedirectToAction("receipt", "Home", new {receiptId = newReceipt.id });
            }

            return RedirectToAction("Index", "Home");
        }
        public ActionResult receipt(int receiptId )
        {
            var receipt = data.Receipts.FirstOrDefault(r => r.id == receiptId);
            if (receipt != null)
            {
                var orderedItems = data.Ordered_Items
                    .Where(oi => oi.order_id == receipt.ordered_id)
                    .ToList();
                decimal amount=0;
                List<ReceiptProductViewModel> products = new List<ReceiptProductViewModel>();

                foreach (var orderedItem in orderedItems)
                {
                    var product = data.Products.FirstOrDefault(p => p.id == orderedItem.product_id);
                    if (product != null)
                    {
                        var totalAmount = orderedItem.quantity * product.price;
                        amount = (decimal)(amount + totalAmount);
                        var productViewModel = new ReceiptProductViewModel
                        {
                            ProductName = product.title,
                            Quantity = (int)orderedItem.quantity,
                            Price = product.price * (int)orderedItem.quantity
                        };

                        products.Add(productViewModel);
                    }
                }
                ViewBag.FinalAmount = amount;
                return View(products);
            }
            return RedirectToAction("Index", "Home");
        }
    }
    
}