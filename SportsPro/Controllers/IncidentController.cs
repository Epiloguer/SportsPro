using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportsPro.Models;
using Microsoft.EntityFrameworkCore;

namespace SportsPro.Controllers
{
    public class IncidentController : Controller
    {
        private SportsProContext context { get; set; }

        public IncidentController(SportsProContext ctx)
        {
            context = ctx;
        }

        public IActionResult IncidentManager()
        {
            var incident = context.Incidents.Include(c => c.Customer)
                .Include(p => p.Product)
                .Include(t => t.Technician)
                .OrderBy(i => i.DateOpened)
                .ToList();
            return View(incident);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Customers = context.Customers.OrderBy(c => c.FirstName).ToList();
            ViewBag.Products = context.Products.OrderBy(p => p.Name).ToList();
            ViewBag.Technician = context.Technicians.OrderBy(t => t.Name).ToList();
            ViewBag.Action = "Add";
            return View("Edit", new Incident());
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Customers = context.Customers.OrderBy(c => c.FirstName).ToList();
            ViewBag.Products = context.Products.OrderBy(p => p.Name).ToList();
            ViewBag.Technician = context.Technicians.OrderBy(t => t.Name).ToList();
            ViewBag.Action = "Edit";
            var incident = context.Incidents.Find(id);
            return View(incident);
        }

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
                return RedirectToAction("IncidentManager", "Incident");
            }
            else
            {
                ViewBag.Action = (incident.IncidentID == 0) ? "Add" : "Edit";
                return View(incident);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var incident = context.Incidents.Find(id);
            return View(incident);
        }

        [HttpPost]
        public IActionResult Delete(Incident incident)
        {
            context.Incidents.Remove(incident);
            context.SaveChanges();
            return RedirectToAction("IncidentManager", "Incident");
        }
    }
}
