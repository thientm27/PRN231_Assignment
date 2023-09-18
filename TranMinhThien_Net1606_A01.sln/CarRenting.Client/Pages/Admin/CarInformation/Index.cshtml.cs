using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CarRenting.DTOs;

namespace CarRenting.Client.Pages.Admin.CarInformation
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _client ;
        private string _carInformationApiUrl;

        public IndexModel()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _carInformationApiUrl = Constants.ApiAdminCarInformation;
        }

        public IList<CarInformationDto> CarInformation { get;set; } = default!;

        public async Task OnGetAsync()
        {
            HttpResponseMessage response = await _client.GetAsync(_carInformationApiUrl);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                List<CarInformationDto>? dataResponse = response.Content.ReadFromJsonAsync<List<CarInformationDto>>().Result;
                if (dataResponse != null)
                {
                    CarInformation = dataResponse;
                }
            }
        }
    }
}
