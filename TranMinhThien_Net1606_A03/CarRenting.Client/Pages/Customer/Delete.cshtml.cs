using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CarRenting.BusinessObjects.Models;
using CarRenting.DTOs;
using CarRenting.Repositories.Context;

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
            _api = Constants.ApiRenting;
        }

        [BindProperty] public RentingDto RentingTransaction { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            HttpResponseMessage response = await _client.GetAsync(_api + "/Renting/" + id);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var dataResponse = response.Content.ReadFromJsonAsync<RentingDto>().Result;
                if (dataResponse != null)
                {
                    RentingTransaction = dataResponse;
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            await _client.DeleteAsync(_api + "/" + id);
            return RedirectToPage("./Index");
        }
    }
}