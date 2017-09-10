using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
//using System.Web;
using System.Web.Mvc;
using LanguageFeatures.Models;

namespace LanguageFeatures.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public string Index()
        {
            return "Navigate to a URL to show an example";
        }

        public ViewResult AutoProperty()
        {
            //Create a new Product object
            Product myProduct = new Product();

            //Set the value property
            myProduct.Name = "Kayak";

            //Get the property
            string productName = myProduct.Name;

            //Generate the view
            return View("Result", (object)String.Format("Product name: {0}", productName));
        }

        public ViewResult CreateProduct()
        {
            //Create and populate a new product object
            Product myProduct = new Product
            {
                ProductID = 100,
                Name = "Kayak",
                Description = "A boat for one person",
                Price = 275M,
                Category = "Watersports"
            };

            return View("Result", (object)String.Format("Category: {0}", myProduct.Category));
        }

        public ViewResult CreateCollection()
        {
            string[] stringArray = { "apple", "orange", "plum" };

            List<int> intList = new List<int> { 10, 20, 30, 40 };

            Dictionary<string, int> myDict = new Dictionary<string, int>
            {
                {"apple", 10 }, {"orange", 20}, {"plum", 30}
            };

            return View("Result", (object)stringArray[1]);
        }

        public ViewResult UseExtension()
        {
            //Create and populate a shopping cart
            ShoppingCart cart = new ShoppingCart()
            {
                Products = new List<Product>
                {
                    new Product {Name="Kayak", Price = 275M},
                    new Product {Name="Life Jacket", Price = 48.95M},
                    new Product {Name="Soccer Ball", Price = 19.50M},
                    new Product {Name="Corner Flag", Price = 34.95M}
                }
            };

            //Get the total value of the products in the cart
            decimal cartTotal = cart.TotalPrices();

            return View("Result", (object)String.Format("Total: {0:c}", cartTotal));
        }

        public ViewResult UseExtensionEnumerable()
        {
            IEnumerable<Product> products = new ShoppingCart
            {
                Products = new List<Product>
                {
                    new Product {Name = "Kayak", Price = 275M},
                    new Product {Name = "Lifejacket", Price = 48.95M},
                    new Product {Name = "Soccer ball", Price = 19.50M},
                    new Product {Name = "Corner flag", Price = 34.95M}
                }
            };

            //Create and populate an array of product objects
            Product[] productArray =
            {
                new Product {Name = "Kayak", Price = 275M},
                new Product {Name = "Lifejacket", Price = 48.95M},
                new Product {Name = "Soccer ball", Price = 19.50M},
                new Product {Name = "Corner flag", Price = 34.95M}
            };


            //Get total values of the products in the cart
            decimal cartTotal = products.TotalPrices();
            decimal arrayTotal = productArray.TotalPrices();

            return View("Result", (object)String.Format("Cart Total: {0}, Array Total: {1}", cartTotal, arrayTotal));
        }

        public ViewResult UseFilterExtensionMethod()
        {
            IEnumerable<Product> products = new ShoppingCart
            {
                Products = new List<Product>
                {
                    new Product {Name = "Kayak", Category="Watersports", Price = 275M},
                    new Product {Name = "Lifejacket", Category="Watersports",  Price = 48.95M},
                    new Product {Name = "Soccer ball", Category="Soccer", Price = 19.50M},
                    new Product {Name = "Corner flag", Category="Soccer", Price = 34.95M}
                }
            };

            //Func<Product, bool> categoryFilter = delegate (Product prod)
            //{
            //    return prod.Category == "Soccer";
            //};

            //Func<Product, bool> categoryFilter = prod => prod.Category == "Soccer";

            //decimal total = 0;
            //foreach (Product prod in products.Filter(categoryFilter))
            //{
            //    total += prod.Price;
            //}

            decimal total = 0;
            foreach (Product prod in products.Filter(prod => prod.Category == "Soccer" || prod.Price > 20))
            {
                total += prod.Price;
            }

            return View("Result", (object)String.Format("Total: {0}", total));
        }

        public ViewResult CreateAnonArray()
        {
            var oddsAndEnds = new[]
            {
                new {Name="MVC", Category="Pattern"},
                new {Name="Hat", Category="Clothing"},
                new {Name="Apple", Category="Fruit"}
            };

            StringBuilder result = new StringBuilder();
            foreach (var item in oddsAndEnds)
            {
                result.Append(item.Name).Append(" ");
            }

            return View("Result", (object)result.ToString());
        }

        public ViewResult FindProducts()
        {
            Product[] products = {
                new Product {Name = "Kayak", Category = "Watersports", Price = 275M},
                new Product {Name = "Lifejacket", Category = "Watersports", Price = 48.95M},
                new Product {Name = "Soccer ball", Category = "Soccer", Price = 19.50M},
                new Product {Name = "Corner flag", Category = "Soccer", Price = 34.95M}
            };

            ////Define the array to hold the products
            //Product[] foundProducts = new Product[3];
            ////Sort the contents of the array
            //Array.Sort(products, (item1, item2) =>
            //{
            //    return Comparer<decimal>.Default.Compare(item1.Price, item1.Price);
            //});
            ////Get the first 3 items in the array as the results
            //Array.Copy(products, foundProducts, 3);

            //var foundProducts = from match in products
            //                    orderby match.Price descending
            //                    select new { match.Name, match.Price };
            var foundProducts = products.OrderByDescending(e => e.Price).Take(3)
                .Select(e => new { e.Name, e.Price });

            products[2] = new Product { Name = "Stadium", Price = 79600M };

            //Create the result
            int count = 0;
            StringBuilder result = new StringBuilder();
            //foreach(Product p in foundProducts)
            //{
            //    result.AppendFormat("Price: {0} ", p.Price);
            //}
            foreach(var p in foundProducts)
            {
                result.AppendFormat("Price: {0} ", p.Price);
                if(++count == 3)
                {
                    break;
                }
            }

            return View("Result", (object)result.ToString());
        }

        public ViewResult SumProducts()
        {
            Product[] products = {
                new Product {Name = "Kayak", Category = "Watersports", Price = 275M},
                new Product {Name = "Lifejacket", Category = "Watersports", Price = 48.95M},
                new Product {Name = "Soccer ball", Category = "Soccer", Price = 19.50M},
                new Product {Name = "Corner flag", Category = "Soccer", Price = 34.95M}
            };

            var foundProducts = products.OrderByDescending(e => e.Price).Take(3)
                .Select(e => new { e.Name, e.Price });

            var results = products.Sum(e => e.Price);

            products[2] = new Product { Name = "Stadium", Price = 79600M };

            return View("Result", (object)String.Format("Sum: {0:c}", results));
        }
    }
}