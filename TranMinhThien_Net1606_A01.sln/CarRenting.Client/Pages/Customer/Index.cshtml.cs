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
    public class IndexModel : PageModel
    {
        private readonly CarRenting.Repositories.Context.FUCarRentingManagementContext _context;

        public IndexModel(CarRenting.Repositories.Context.FUCarRentingManagementContext context)
        {
            _context = context;
        }

        public IList<RentingTransaction> RentingTransaction { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.RentingTransactions != null)
            {
                RentingTransaction = await _context.RentingTransactions
                .Include(r => r.Customer).ToListAsync();
            }
        }
    }
}
