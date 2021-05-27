using Microsoft.AspNetCore.Mvc;
using SportsPro.Areas.Admin.Components;
using SportsPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsPro
{
    public class CountryDropDown:ViewComponent
    {
        private IRepository<Country> data { get; set; }
        public CountryDropDown(IRepository<Country> rep) => data = rep;

        public IViewComponentResult Invoke(string selectedValue)
        {
            var countries = data.List(new QueryOptions<Country>
            {
                OrderBy = c => c.Name
            });

            var vm = new DropDownViewModel
            {
                SelectedValue = selectedValue,
                DefaultValue = "All",
                Items = countries.ToDictionary(
                    c => c.CountryID.ToString(), c => c.Name)
            };

            return View(SharedPath.Select, vm);
        }
    }
}
