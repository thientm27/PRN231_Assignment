using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CarRenting.DTOs;

namespace CarRenting.Client.Pages.Admin.Customer
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _client;
        private string _productApiUrl;

        public CreateModel()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _productApiUrl = Constants.ApiString + Constants.Customer;
        }

        public IActionResult OnGet()
        {
            var userId = HttpContext.Session.GetInt32("User");
            if (userId == null || userId != -1)
            {
                return RedirectToPage("../../Login");
            }
            
            return Page();
        }

        [BindProperty]
        public CustomerDto Customer { get; set; } = default!;
        

        public async Task<IActionResult> OnPostAsync()
        {
         
                HttpResponseMessage response = await _client.PostAsJsonAsync(_productApiUrl, Customer);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("./Index");
                }
            
            return RedirectToPage("./Index");
        }
    }
}
