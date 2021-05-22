using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportsPro.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace SportsPro.Controllers
{
    public class RegistrationController : Controller
    {
        //controller starts with a private property named context of the SportsProContext type
        private SportsProContext context { get; set; }
        public RegistrationViewModel viewModel;

        //constructor accepts a SportsProContext Object and assigns it to the context property
        //Allows other methods in this class to easily access the SportsProContext Object
        //Works because of the dependecy injection code in the Startup.cs
        public RegistrationController(SportsProContext ctx)
        {
            context = ctx;
            viewModel = new RegistrationViewModel();
        }

        public ViewResult Index()
        {
            var data = new RegistrationViewModel()
            {
                Customer = new Customer { CustomerID = 1002 }
            };

            IQueryable<Customer> query = context.Customers;

            data.Customers = query.ToList();
            return View(data);
        }

        [HttpPost]
        /*
         * store selected Customer in session state.
         */
        public IActionResult Index(RegistrationViewModel selectedCustomer)
        {
            var session = new MySession(HttpContext.Session);
            var sessionCustomer = session.GetCustomer();
            sessionCustomer = context.Customers.Find(selectedCustomer.Customer.CustomerID);
            session.SetCustomer(sessionCustomer);


            return RedirectToAction("RegProduct", "Registration");
        }

        public IActionResult RegProduct()
        {
            return View();
        }
        
    }
}
