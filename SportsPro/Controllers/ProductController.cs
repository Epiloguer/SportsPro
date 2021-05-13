using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    public class ProductController : Controller
    {
        //controller starts with a private property named context of the SportsProContext type
        private SportsProContext context { get; set; }

        //constructor accepts a SportsProContext Object and assigns it to the context property
        //Allows other methods in this class to easily access the SportsProContext Object
        //Works because of the dependecy injection code in the Startup.cs
        public ProductController(SportsProContext ctx)
        {
            context = ctx;
        }

        //uses the context property to get a collection of Product objects from the database.
        //Sorts the objects alphabetically by Product Name.
        //Finally it passes the collection to the view.
        public IActionResult ProductManager()
        {
            var products = context.Products.OrderBy(p => p.Name).ToList();
            return View(products);
        }

        /*Action Method Add() only handles GET requests. since the Add() and Edit() both use
            the Product/Edit view.
        For the GET request both the Add() and Edit() actions set a ViewBag property named
            Action and pass a Product object to the view.
        Add() action passes an empty Product object.*/
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            return View("Edit", new Product());
        }

        /*the Edit() action passes a Product object with data for an existing product by
            passing the id parameter to the Find() method to retrieve a product from the
            database.*/
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            var product = context.Products.Find(id);
            return View(product);
        }

        /*starts by checking if the user entered valid data to the model. If so, the code 
            checks the value of the ProductID property of the Product object.
          If the value is zero, it creates a new product passed into the Add() action.
            Otherwise, its an existing product, the code passes it to the Update().*/
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.ProductID == 0)
                    context.Products.Add(product);
                else
                    context.Products.Update(product);
                context.SaveChanges();
                return RedirectToAction("ProductManager", "Product");
            }
            else
            {
                ViewBag.Action = (product.ProductID == 0) ? "Add" : "Edit";
                return View(product);
            }
        }

        /*uses id parameter to retrieve a Product object for the specified product from the
           database. Then passes the object to the view.*/
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var product = context.Products.Find(id);
            return View(product);
        }

        /*passes the Product object it receives from the view to the Remove(). After which
            it calls the SaveChanges() to delete the product from the database.
          Finally it redirects the user back to the ProductManager action.*/
        [HttpPost]
        public IActionResult Delete(Product product)
        {
            context.Products.Remove(product);
            context.SaveChanges();
            return RedirectToAction("ProductManager", "Product");
        }
    }
}
