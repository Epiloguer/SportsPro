using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportsPro.Models;
using Microsoft.EntityFrameworkCore;
//Using directive for the EF Core namespace. See next comment.

namespace SportsPro.Controllers
{
    public class CustomerController : Controller
    {
        //controller starts with a private property named context of the SportsProContext type
        private SportsProContext context { get; set; }

        //constructor accepts a SportsProContext Object and assigns it to the context property
        //Allows other methods in this class to easily access the SportsProContext Object
        //Works because of the dependecy injection code in the Startup.cs
        public CustomerController(SportsProContext ctx)
        {
            context = ctx;
        }

        //uses the context property to get a collection of Customer objects from the database.
        //Sorts the objects alphabetically by Customer Name.
        //Finally it passes the collection to the view.
        //CustomerManager() uses the Include() method to select the Country data related to each Customer.
        public IActionResult CustomerManager()
        {
            var customer = context.Customers.Include(c => c.Country)
                .OrderBy(c => c.FirstName).ToList();
            return View(customer);
        }

        /*Action Method Add() only handles GET requests. since the Add() and Edit() both use
            the Customer/Edit view.
        For the GET request both the Add() and Edit() actions set a ViewBag property named
            Action and pass a Customer object to the view.
        Add() action passes an empty Customer object.*/
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Countries = context.Countries.OrderBy(c => c.Name).ToList();
            ViewBag.Action = "Add";
            return View("Edit", new Customer());
        }

        /*the Edit() action passes a Customer object with data for an existing Customer by
            passing the id parameter to the Find() method to retrieve a Customer from the
            database.*/
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Countries = context.Countries.OrderBy(c => c.Name).ToList();
            ViewBag.Action = "Edit";
            var customer = context.Customers.Find(id);
            return View(customer);
        }

        /*starts by checking if the user entered valid data to the model. If so, the code 
            checks the value of the CustomerID property of the Customer object.
          If the value is zero, it creates a new Customer passed into the Add() action.
            Otherwise, its an existing Customer, the code passes it to the Update().*/
        [HttpPost]
        public IActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                if (customer.CustomerID == 0)
                    context.Customers.Add(customer);
                else
                    context.Customers.Update(customer);
                context.SaveChanges();
                return RedirectToAction("CustomerManager", "Customer");
            }
            else
            {
                ViewBag.Action = (customer.CustomerID == 0) ? "Add" : "Edit";
                return View(customer);
            }
        }

        /*uses id parameter to retrieve a Customer object for the specified Customer from the
           database. Then passes the object to the view.*/
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var customer = context.Customers.Find(id);
            return View(customer);
        }

        /*passes the Customer object it receives from the view to the Remove(). After which
            it calls the SaveChanges() to delete the Customer from the database.
          Finally it redirects the user back to the CustomerManager action.*/
        [HttpPost]
        public IActionResult Delete(Customer customer)
        {
            context.Customers.Remove(customer);
            context.SaveChanges();
            return RedirectToAction("CustomerManager", "Customer");
        }
    }
}
