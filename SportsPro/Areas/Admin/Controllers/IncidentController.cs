using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportsPro.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace SportsPro.Controllers
{
    [Area("Admin")]
    public class IncidentController : Controller
    {
        //controller starts with a private property named context of the SportsProContext type
        private SportsProContext context { get; set; }

        //constructor accepts a SportsProContext Object and assigns it to the context property
        //Allows other methods in this class to easily access the SportsProContext Object
        //Works because of the dependecy injection code in the Startup.cs
        public IncidentController(SportsProContext ctx)
        {
            context = ctx;
        }

        //uses the context property to get a collection of Incident objects from the database.
        //Sorts the objects alphabetically by Incident Name.
        //Finally it passes the collection to the view.
        [Authorize(Roles = "Admin, Technician")]
        public IActionResult Index()
        {
            ViewBag.filter = Request.Query["filter"];
            string filter= Request.Query["filter"];
            var data = new IncidentListViewModel()
            {
                MyFilter = filter
            };

            IQueryable<Incident> query = context.Incidents;
            query = query.Include(c => c.Customer)
                .Include(p => p.Product)
                .Include(t => t.Technician)
                .OrderBy(i => i.DateOpened);

            if (filter == "unassigned")
                query = query.Where(
                    i => i.TechnicianID == null);
            if (filter == "open")
                query = query.Where(
                    i => i.DateClosed == null);
            data.Incidents = query.ToList();
            return View(data);
        }

        /*Action Method Add() only handles GET requests. since the Add() and Edit() both use
            the Incident/Edit view.
        For the GET request both the Add() and Edit() actions set a ViewBag property named
            Action and pass a Incident object to the view.
        Add() action passes an empty Incident object.
        Using ViewModel(IncidentViewModel) pass list of customers, products and techinicians
        to the view page.*/
        [Authorize(Roles = "Admin, Technician")]
        [HttpGet]
        public IActionResult Add()
        {
            var data = new IncidentViewModel
            {
                Customers = context.Customers.OrderBy(c => c.FirstName).ToList(),
                Products = context.Products.OrderBy(p => p.Name).ToList(),
                Technicians = context.Technicians.OrderBy(t => t.Name).ToList(),
                DesiredAction = "Add",
                Incident = new Incident()

            };

            return View("Edit", data);
        }

        /*the Edit() action passes a Incident object with data for an existing Incident by
                    passing the id parameter to the Find() method to retrieve a Incident from the
                    database.
        Using ViewModel(IncidentViewModel) pass list of customers, products and techinicians
        to the view page.*/
        [Authorize(Roles = "Admin, Technician")]
        [HttpGet]
        public IActionResult Edit(int id = 1)
        {
            var data = new IncidentViewModel
            {
                Customers = context.Customers.OrderBy(c => c.FirstName).ToList(),
                Products = context.Products.OrderBy(p => p.Name).ToList(),
                Technicians = context.Technicians.OrderBy(t => t.Name).ToList(),
                DesiredAction = "Edit",
                Incident = context.Incidents.Find(id)

            };
            return View(data);
        }

        /*starts by checking if the user entered valid data to the model. If so, the code 
            checks the value of the IncidentID property of the Incident object.
          If the value is zero, it creates a new Incident passed into the Add() action.
            Otherwise, its an existing Incident, the code passes it to the Update().*/
        [Authorize(Roles = "Admin, Technician")]
        [HttpPost]
        public IActionResult Edit(Incident incident)
        {
            if (ModelState.IsValid)
            {
                if (incident.IncidentID == 0)
                    context.Incidents.Add(incident);
                else
                    context.Incidents.Update(incident);
                context.SaveChanges();
                return RedirectToAction("Index", "Incident");
            }
            else
            {
                ViewBag.Action = (incident.IncidentID == 0) ? "Add" : "Edit";
                return View(incident);
            }
        }

        /*uses id parameter to retrieve a Incident object for the specified Incident from the
           database. Then passes the object to the view.*/
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Delete(int id = 1)
        {
            var incident = context.Incidents.Find(id);
            return View(incident);
        }

        /*passes the Incident object it receives from the view to the Remove(). After which
            it calls the SaveChanges() to delete the Incident from the database.
          Finally it redirects the user back to the Index action.*/
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Delete(Incident incident)
        {
            context.Incidents.Remove(incident);
            context.SaveChanges();
            return RedirectToAction("Index", "Incident");
        }
    }
}
