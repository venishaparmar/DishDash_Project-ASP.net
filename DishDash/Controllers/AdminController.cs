using DishDash.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DishDash.Controllers
{
    public class AdminController : Controller
    {
        private DishDashEntities data;
        public AdminController()
        {
            data = new DishDashEntities();
        }
        public ActionResult Index()
        {
            int numberOfCategories = data.Categories.Count(); 
            int numberOfOrders = data.Orders.Count(); 
            int numberOfProducts = data.Products.Count();
            decimal? totalRevenueNullable = data.Orders.Select(o => o.total_amount).Sum();
            decimal totalRevenue = totalRevenueNullable ?? 0m;


            ViewBag.NumberOfCategories = numberOfCategories;
            ViewBag.NumberOfOrders = numberOfOrders;
            ViewBag.NumberOfProducts = numberOfProducts;
            ViewBag.TotalRevenue = totalRevenue;

            return View();
        }
        public ActionResult add_category()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCategory()
        {
            var title = Request.Form["title"];
            var active = Convert.ToBoolean(Request.Form["active"]);

            HttpPostedFileBase image = Request.Files["image"];
            byte[] imageData = null;

            if (image != null && image.ContentLength > 0)
            {
                using (var binaryReader = new System.IO.BinaryReader(image.InputStream))
                {
                    imageData = binaryReader.ReadBytes(image.ContentLength);
                }
            }

            var newCategory = new Category
            {
                title = title,
                active = active,
                image = imageData,
                create_at = DateTime.Now
            };

            data.Categories.Add(newCategory);
            data.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult DeleteCategory(int categoryId)
        {
            var category = data.Categories.Find(categoryId);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCategory(string categoryId)
        {
            int id = Convert.ToInt32(categoryId);
            //var category = data.Categories.Find(id);
            var category = data.Categories.Include("Products").SingleOrDefault(c => c.id == id);
            if (category == null)
            {
                return HttpNotFound();
            }
            else if (category.Products.Any())
            {
                TempData["ErrorMessage"] = "This category cannot be deleted because it has associated products. Delete the products first.";
                return RedirectToAction("manage_food"); // Adjust the action name accordingly
            }

            else
            {
                data.Categories.Remove(category);
                data.SaveChanges();
            }


            return RedirectToAction("Index");
        }
        public ActionResult add_food()
        {
            List<Category> categories = data.Categories.ToList();
            
            return View(categories);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFood()
        {
            var title = Request.Form["title"];
            var price = Convert.ToDecimal(Request.Form["price"]);
            var categoryId = Convert.ToInt32(Request.Form["category"]);
            var active = Convert.ToBoolean(Request.Form["active"]);

            HttpPostedFileBase image = Request.Files["image"];
            byte[] imageData = null;

            if (image != null && image.ContentLength > 0)
            {
                using (var binaryReader = new System.IO.BinaryReader(image.InputStream))
                {
                    imageData = binaryReader.ReadBytes(image.ContentLength);
                }
            }

            var newFood = new Product
            {
                title = title,
                price = price,
                category_id = categoryId,
                active = active,
                image = imageData,
                create_at = DateTime.Now
            };

            data.Products.Add(newFood);
            data.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult manage_category()
        {
            List<Category> categories = data.Categories.ToList();

            return View(categories);
        }

        
        public ActionResult manage_food()
        {
            List<Product> foods = data.Products.ToList();

            return View(foods);
        }
        public ActionResult manage_order()
        {
            var ordersData = (from oi in data.Ordered_Items
                              join p in data.Products on oi.product_id equals p.id
                              select new OrderViewModel
                              {
                                  id = oi.id,
                                  ProductName = p.title,
                                  ProductPrice = p.price,
                                  Quantity = (int)oi.quantity,
                                  TotalAmount = (decimal)(oi.quantity * p.price),
                                  OrderDate = (DateTime)oi.order_date_time, 
                                  Status = oi.status
                              }).ToList();

            return View(ordersData);
        }
        public ActionResult update_category()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult update_category(int categoryId)
        {
            var title = Request.Form["title"];
            var active = Convert.ToBoolean(Request.Form["active"]);

            HttpPostedFileBase image = Request.Files["image"];
            byte[] imageData = null;

            if (image != null && image.ContentLength > 0)
            {
                using (var binaryReader = new System.IO.BinaryReader(image.InputStream))
                {
                    imageData = binaryReader.ReadBytes(image.ContentLength);
                }
            }

            Category existingCategory = data.Categories.FirstOrDefault(c => c.id == categoryId);

            if (existingCategory != null)
            {
                existingCategory.title = title;
                existingCategory.active = active;
                existingCategory.image = imageData;

                data.SaveChanges();

                return RedirectToAction("Index");
            }
            return RedirectToAction("update_category");
        }
        [HttpGet]
        public ActionResult update_category(string Id)
        {
            int categoryId = Convert.ToInt32(Id);
            Category category = data.Categories.FirstOrDefault(c => c.id == categoryId);

            if (category == null)
            {

                return RedirectToAction("manage_category");
            }

            return View(category);
        }
        public ActionResult update_food()
        {
            return View();
        }
        [HttpGet]
        public ActionResult update_food(int foodId)
        {
            var food = data.Products.FirstOrDefault(f => f.id == foodId);

            if (food == null)
            {
                return RedirectToAction("manage_food");
            }

            return View(food);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult updateFood(int foodId)
        {
            var title = Request.Form["title"];
            var price = decimal.Parse(Request.Form["price"]);
            var active = Convert.ToBoolean(Request.Form["active"]);

            HttpPostedFileBase image = Request.Files["image"];
            byte[] imageData = null;

            if (image != null && image.ContentLength > 0)
            {
                using (var binaryReader = new System.IO.BinaryReader(image.InputStream))
                {
                    imageData = binaryReader.ReadBytes(image.ContentLength);
                }
            }

            var existingFood = data.Products.FirstOrDefault(f => f.id == foodId);

            if (existingFood != null)
            {
                existingFood.title = title;
                existingFood.price = price;
                existingFood.active = active;
                existingFood.image = imageData;

                data.SaveChanges();

                return RedirectToAction("manage_food");
            }
            return RedirectToAction("updateFood", new { foodId = foodId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult delete_food(string foodId)
        {
            int id = Convert.ToInt32(foodId); 
            var food = data.Products.Find(id);
            if (food == null)
            {
                return HttpNotFound();
            }

            data.Products.Remove(food);
            data.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult delete_food(int foodId)
        {
            var food = data.Products.Find(foodId);
            if (food == null)
            {
                return HttpNotFound();
            }
            return View(food);
        }
        public ActionResult update_order()
        {
            return View();
        }
        [HttpGet]
        public ActionResult update_order(int orderId)
        {
            var ordersData = (from oi in data.Ordered_Items
                              join p in data.Products on oi.product_id equals p.id
                              where oi.id == orderId // Filter by orderId
                              select new OrderViewModel
                              {
                                  id = oi.id,
                                  ProductName = p.title,
                                  ProductPrice = p.price,
                                  Quantity = (int)oi.quantity,
                                  TotalAmount = (decimal)(oi.quantity * p.price),
                                  OrderDate = (DateTime)oi.order_date_time,
                                  Status = oi.status
                              }).ToList();

            return View(ordersData);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult updateOrder(int orderId)
        {
            var status = Request.Form["status"];

            var existingOrder = data.Ordered_Items.FirstOrDefault(o => o.id == orderId);

            if (existingOrder != null)
            {
                existingOrder.status = status; 

                data.SaveChanges();

                return RedirectToAction("manage_order");
            }

            return RedirectToAction("manage_order");
        }

        public ActionResult Logout()
        {
            Session["isAuth"] = false;
            return RedirectToAction("Index", "Home");
        }
    }
}