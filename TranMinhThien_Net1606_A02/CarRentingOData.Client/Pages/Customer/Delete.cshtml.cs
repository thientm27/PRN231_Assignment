using System.Net.Http.Headers;
using CarRentingOData.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPage.ViewModels;

namespace CarRenting.Client.Pages.Customer
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient _client;
        private string _api;

        public DeleteModel()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _api = Constants.ApiString + Constants.CarRental;
        }

        [BindProperty] public CarRentalDto RentingTransaction { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = HttpContext.Session.GetInt32("User");
            if (userId == null || userId < 0)
            {
                return RedirectToPage("../Login");

            }

            var deleteObj = HttpContext.Session.GetObjectFromJson<CarRentalDto>("Cancel");
            if (deleteObj == null)
            {
                return NotFound();
            }
                    
            RentingTransaction = deleteObj;
                

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            var userId = HttpContext.Session.GetInt32("User");
            if (userId == null || userId < 0)
            {
                return RedirectToPage("../Login");

            }

            var deleteObj = HttpContext.Session.GetObjectFromJson<CarRentalDto>("Cancel");
            if (deleteObj == null)
            {
                return NotFound();
            }

            await _client.PutAsJsonAsync(_api, deleteObj);
            return RedirectToPage("./Index");
        }
    }
}