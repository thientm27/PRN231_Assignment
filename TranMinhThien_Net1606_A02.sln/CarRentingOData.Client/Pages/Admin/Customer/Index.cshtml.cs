using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CarRenting.DTOs;

namespace CarRenting.Client.Pages.Admin.Customer
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _client ;
        private string _productApiUrl;


        public IndexModel()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _productApiUrl = Constants.ApiString + Constants.Customer;
        }

        public IList<CustomerDto> Customer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = HttpContext.Session.GetInt32("User");
            if (userId == null || userId != -1)
            {
                return RedirectToPage("../../Login");
            }
            
            HttpResponseMessage response = await _client.GetAsync(_productApiUrl);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                List<CustomerDto>? dataResponse = response.Content.ReadFromJsonAsync<List<CustomerDto>>().Result;
                if (dataResponse != null)
                {
                    Customer = dataResponse;
                }
            }

            return Page();
        }
    }
}