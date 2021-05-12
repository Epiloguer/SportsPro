using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportsPro.Models;
using Microsoft.EntityFrameworkCore;

namespace SportsPro.Controllers
{
    public class CustomerController : Controller
    {
        private SportsProContext context { get; set; }

        public CustomerController(SportsProContext ctx)
        {
            context = ctx;
        }

        public IActionResult CustomerManager()
        {
            var customer = context.Customers.Include(c => c.Country)
                .OrderBy(c => c.FirstName).ToList();
            return View(customer);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Countries = context.Countries.OrderBy(c => c.Name).ToList();
            ViewBag.Action = "Add";
            return View("Edit", new Customer());
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Countries = context.Countries.OrderBy(c => c.Name).ToList();
            ViewBag.Action = "Edit";
            var customer = context.Customers.Find(id);
            return View(customer);
        }

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

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var customer = context.Customers.Find(id);
            return View(customer);
        }

        [HttpPost]
        public IActionResult Delete(Customer customer)
        {
            context.Customers.Remove(customer);
            context.SaveChanges();
            return RedirectToAction("CustomerManager", "Customer");
        }
    }
}
