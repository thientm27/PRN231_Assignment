using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CarRenting.DTOs;
using Microsoft.AspNetCore.Mvc;


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
            _api = Constants.ApiRenting;
        }


        public IList<RentingDto> RentingTransaction { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = HttpContext.Session.GetInt32("User");
            
            if (userId == null || userId < 0)
            {
                return RedirectToPage("../Login");
            }

            HttpResponseMessage response = await _client.GetAsync(_api + "/" + userId);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var dataResponse = response.Content.ReadFromJsonAsync<List<RentingDto>>().Result;
                if (dataResponse != null)
                {
                    RentingTransaction = dataResponse;
                }
            }
            else
            {
                RentingTransaction = new List<RentingDto>();
            }

            return Page();
        }
    }
}