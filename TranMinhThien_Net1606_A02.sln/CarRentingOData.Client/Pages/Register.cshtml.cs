using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CarRentingOData.DTOs;

namespace CarRenting.Client.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly HttpClient _client;
        private string _apiUrl;

        public RegisterModel()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _apiUrl = Constants.ApiString + Constants.Customer;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty] public CustomerDto Customer { get; set; } = default!;


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _client.PostAsJsonAsync(_apiUrl, Customer);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var dataResponse = result.Content.ReadFromJsonAsync<CustomerDto>().Result;
                if (dataResponse == null)
                {
                    ModelState.AddModelError(Customer.Email, "Duplicated email");
                }
            }else if (result.StatusCode == HttpStatusCode.NoContent)
            {
                ModelState.AddModelError("Customer.Email", "Duplicated email");
                return Page();
            }
            return RedirectToPage("./Login");
        }
    }
}