using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CarRenting.DTOs;
using Microsoft.AspNetCore.Mvc;
using CarRentingOData.DTOs;
using CarRentingOData.DTOs.Response;
using CarRentingOData.DTOs.Request;
using RazorPage.ViewModels;

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
        [BindProperty] public int DeleteObjIndex { get; set; } = default!;

        [BindProperty] public IList<CarRentalDto> RentingTransaction { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = HttpContext.Session.GetInt32("User");
            
            if (userId == null || userId < 0)
            {
                return RedirectToPage("../Login");
            }

            string filterString = $"?$filter=customerID eq {userId} and status ne '0'&$expand=Car";
            string requestUrl = _api + filterString;
            HttpResponseMessage response = await _client.GetAsync(requestUrl); 
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

        public async Task<IActionResult> OnPostCancel()
        {
            var userId = HttpContext.Session.GetInt32("User");
            if (userId == null || userId < 0)
            {
                return RedirectToPage("../Login");

            }

            HttpResponseMessage response = await _client.GetAsync(_api + "?$filter=customerID eq " + userId + "and status ne '0'&$expand=Car");
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
                return NotFound();
            }
            if(RentingTransaction.Count -1 < DeleteObjIndex)
            {
                return NotFound();
            }
            var deleteObj = RentingTransaction[DeleteObjIndex];
            HttpContext.Session.SetObjectAsJson("Cancel", deleteObj);
            return RedirectToPage("./Delete");
        }
    }
}