using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CarRenting.BusinessObjects.Models;
using CarRenting.DTOs;
using CarRenting.Repositories.Context;

namespace CarRenting.Client.Pages.Admin.CarInformation
{
    public class IndexModel : PageModel
    {
        private readonly CarRenting.Repositories.Context.FUCarRentingManagementContext _context;

        public IndexModel(CarRenting.Repositories.Context.FUCarRentingManagementContext context)
        {
            _context = context;
        }

        public IList<CarInformationDto> CarInformation { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.CarInformations != null)
            {
                // CarInformation = await _context.CarInformations
                // .Include(c => c.Manufacturer)
                // .Include(c => c.Supplier).ToListAsync();
            }
        }
    }
}
