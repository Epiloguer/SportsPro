using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsPro.ViewComponents
{

    

    public class ProductViewComponent : ViewComponent
    {
        private SportsProContext _context;
        public ProductViewComponent(SportsProContext context)
        {
            _context = context;
        }
        
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _context.Products.ToListAsync());
        }
    }
}
