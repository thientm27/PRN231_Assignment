﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarRenting.BusinessObjects.Models;
using CarRenting.Repositories.Context;

namespace CarRenting.Client.Pages.Customer
{
    public class EditModel : PageModel
    {
        private readonly CarRenting.Repositories.Context.FUCarRentingManagementContext _context;

        public EditModel(CarRenting.Repositories.Context.FUCarRentingManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RentingTransaction RentingTransaction { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.RentingTransactions == null)
            {
                return NotFound();
            }

            var rentingtransaction =  await _context.RentingTransactions.FirstOrDefaultAsync(m => m.RentingTransationId == id);
            if (rentingtransaction == null)
            {
                return NotFound();
            }
            RentingTransaction = rentingtransaction;
           ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Email");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(RentingTransaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RentingTransactionExists(RentingTransaction.RentingTransationId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool RentingTransactionExists(int id)
        {
          return (_context.RentingTransactions?.Any(e => e.RentingTransationId == id)).GetValueOrDefault();
        }
    }
}
