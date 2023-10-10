using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CarRenting.DTOs;
using Microsoft.AspNetCore.Mvc;
using CarRentingOData.DTOs;
using CarRentingOData.DTOs.Response;

namespace CarRenting.Client.Pages.Customer
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _client;
        private string _api;

        public IndexModel()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _api = Constants.OdataString + Constants.CarRental;
        }


        public IList<CarRentalDto> RentingTransaction { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = HttpContext.Session.GetInt32("User");
            
            if (userId == null || userId < 0)
            {
                return RedirectToPage("../Login");
            }
            HttpResponseMessage response = await _client.GetAsync(_api + "?$filter=customerID eq " + userId + "&$expand=Car");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var dataResponse = response.Content.ReadFromJsonAsync<ODataResponse<CarRentalDto>>().Result;
                if (dataResponse != null)
                {
                    RentingTransaction = dataResponse.Value;
                }
            }
            else
            {
                RentingTransaction = new List<CarRentalDto>();
            }

            return Page();
        }
    }
}