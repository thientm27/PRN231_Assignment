using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CarRenting.BusinessObjects.Models;
using CarRenting.Repositories.Context;

namespace CarRenting.Client.Pages.Customer
{
    public class DetailsModel : PageModel
    {
        private readonly CarRenting.Repositories.Context.FUCarRentingManagementContext _context;

        public DetailsModel(CarRenting.Repositories.Context.FUCarRentingManagementContext context)
        {
            _context = context;
        }

      public RentingTransaction RentingTransaction { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.RentingTransactions == null)
            {
                return NotFound();
            }

            var rentingtransaction = await _context.RentingTransactions.FirstOrDefaultAsync(m => m.RentingTransationId == id);
            if (rentingtransaction == null)
            {
                return NotFound();
            }
            else 
            {
                RentingTransaction = rentingtransaction;
            }
            return Page();
        }
    }
}
