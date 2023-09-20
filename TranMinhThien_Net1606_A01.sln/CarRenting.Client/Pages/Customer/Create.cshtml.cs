using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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
        private readonly HttpClient _client;
        private string _api;

        public CreateModel()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _api = Constants.ApiRenting;
        }
        
        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty] public RentingTransaction RentingTransaction { get; set; } = default!;
        [BindProperty] public IList<CarInformation> CarRenting { get; set; } = default!;
        [BindProperty] public IList<CarInformation> CarAvailable { get; set; } = default!;
        [BindProperty] public DateTime? StartDay { get; set; }
        [BindProperty] public DateTime? EndDay { get; set; }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || RentingTransaction == null)
            {
                return Page();
            }
            

            return RedirectToPage("./Index");
        }
        
        public async Task<IActionResult> OnPostSearchAsync()
        {
            if (StartDay == null || EndDay == null)
            {
                return await OnPostAsync();
            }

            if (EndDay <= StartDay)
            {
                return await OnPostAsync();
            }

            return Page();
        }
    }
}