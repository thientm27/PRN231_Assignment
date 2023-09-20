using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CarRenting.BusinessObjects.Models;
using CarRenting.Repositories.Context;

namespace CarRenting.Client.Pages.Customer
{
    public class CreateModel : PageModel
    {
        private readonly CarRenting.Repositories.Context.FUCarRentingManagementContext _context;

        public CreateModel(CarRenting.Repositories.Context.FUCarRentingManagementContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Email");
            return Page();
        }

        [BindProperty]
        public RentingTransaction RentingTransaction { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.RentingTransactions == null || RentingTransaction == null)
            {
                return Page();
            }

            _context.RentingTransactions.Add(RentingTransaction);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
