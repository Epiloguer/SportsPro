using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    public class TechnicianController : Controller
    {
        //controller starts with a private property named context of the SportsProContext type
        private SportsProContext context { get; set; }

        //constructor accepts a SportsProContext Object and assigns it to the context property
        //Allows other methods in this class to easily access the SportsProContext Object
        //Works because of the dependecy injection code in the Startup.cs
        public TechnicianController(SportsProContext ctx)
        {
            context = ctx;
        }

        //uses the context property to get a collection of Technician objects from the database.
        //Sorts the objects alphabetically by Technician Name.
        //Finally it passes the collection to the view.
        public IActionResult TechnicianManager()
        {
            var technicians = context.Technicians.OrderBy(t => t.Name).ToList();
            return View(technicians);
        }

        /*Action Method Add() only handles GET requests. since the Add() and Edit() both use
            the Technician/Edit view.
        For the GET request both the Add() and Edit() actions set a ViewBag property named
            Action and pass a Technician object to the view.
        Add() action passes an empty Technician object.*/
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            return View("Edit", new Technician());
        }

        /*the Edit() action passes a Technician object with data for an existing Technician by
            passing the id parameter to the Find() method to retrieve a Technician from the
            database.*/
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            var technician = context.Technicians.Find(id);
            return View(technician);
        }

        /*starts by checking if the user entered valid data to the model. If so, the code 
            checks the value of the TechnicianID property of the Technician object.
          If the value is zero, it creates a new Technician passed into the Add() action.
            Otherwise, its an existing Technician, the code passes it to the Update().*/
        [HttpPost]
        public IActionResult Edit(Technician technician)
        {
            if (ModelState.IsValid)
            {
                if (technician.TechnicianID == 0)
                    context.Technicians.Add(technician);
                else
                    context.Technicians.Update(technician);
                context.SaveChanges();
                return RedirectToAction("TechnicianManager", "Technician");
            }
            else
            {
                ViewBag.Action = (technician.TechnicianID == 0) ? "Add" : "Edit";
                return View(technician);
            }
        }

        /*uses id parameter to retrieve a Technician object for the specified Technician from the
          database. Then passes the object to the view.*/
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var technician = context.Technicians.Find(id);
            return View(technician);
        }

        /*passes the Technician object it receives from the view to the Remove(). After which
            it calls the SaveChanges() to delete the Technician from the database.
          Finally it redirects the user back to the TechnicianManager action.*/
        [HttpPost]
        public IActionResult Delete(Technician technician)
        {
            context.Technicians.Remove(technician);
            context.SaveChanges();
            return RedirectToAction("TechnicianManager", "Technician");
        }
    }
}
