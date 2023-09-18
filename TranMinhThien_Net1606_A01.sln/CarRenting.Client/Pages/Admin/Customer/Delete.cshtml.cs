﻿using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CarRenting.DTOs;

namespace CarRenting.Client.Pages.Admin.Customer
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient _client;
        private string _productApiUrl;

        public DeleteModel()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _productApiUrl = Constants.ApiAdminCustomer;
        }

        [BindProperty] public CustomerDto Customer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            HttpResponseMessage response = await _client.GetAsync(_productApiUrl + "/" + id);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var dataResponse = response.Content.ReadFromJsonAsync<CustomerDto>().Result;
                if (dataResponse != null)
                {
                    Customer = dataResponse;
                    return Page();
                }
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            await _client.DeleteAsync(_productApiUrl + "/" + id);
            return RedirectToPage("./Index");
        }
    }
}