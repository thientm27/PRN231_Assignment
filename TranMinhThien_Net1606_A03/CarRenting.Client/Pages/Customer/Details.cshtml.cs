using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CarRenting.BusinessObjects.Models;
using CarRenting.DTOs;

namespace CarRenting.Client.Pages.Customer
{
    public class DetailsModel : PageModel
    {
        private readonly HttpClient _client;
        private string _api;

        public DetailsModel()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _api = Constants.ApiRenting;
        }

        public RentingDto RentingTransaction { get; set; } = default!;
        [BindProperty] public IList<RentingDetailDto> RentingDetail { get; set; } = default!;
        public bool DeleteAble { get; set; }

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

            HttpResponseMessage detailResponse = await _client.GetAsync(_api + "/RentingDetail/" + id);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var dataResponse = detailResponse.Content.ReadFromJsonAsync<List<RentingDetailDto>>().Result;
                if (dataResponse != null)
                {
                    RentingDetail = dataResponse;
                }
            }

            DeleteAble = true;
            var today = DateTime.Today;
            foreach (var o in RentingDetail)
            {
                if (o.StartDate <= today)
                {
                    DeleteAble = false;
                    break; 
                }
            }

            return Page();
        }
    }
}