using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CarRenting.BusinessObjects.Models;
using CarRenting.DTOs;
using CarRenting.DTOs.Request;
using RazorPage.ViewModels;

namespace CarRenting.Client.Pages.Customer
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _client;
        private string _api;

        public CreateModel()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _api = Constants.ApiRenting;
        }

        [BindProperty] public RentingTransaction RentingTransaction { get; set; } = default!;
        [BindProperty] public IList<CarInformationDto> CarRenting { get; set; } = default!;
        [BindProperty] public IList<CarInformationDto> CarAvailable { get; set; } = default!;
        [BindProperty]    [DataType(DataType.Date)]  public DateTime? StartDay { get; set; }
        [BindProperty]    [DataType(DataType.Date)]  public DateTime? EndDay { get; set; }

        public void OnGetAsync()
        {
            RentingTransaction = new RentingTransaction();
            RentingTransaction.RentingDate = DateTime.Today;
            CarRenting = new List<CarInformationDto>();
            CarAvailable = new List<CarInformationDto>();
            // Set old time
            var dateStart = HttpContext.Session.GetObjectFromJson<DateTime>("StartDay");
            var dateEnd = HttpContext.Session.GetObjectFromJson<DateTime>("EndDay");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || RentingTransaction == null)
            {
                return Page();
            }

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostSearch()
        {
            if (StartDay == null || EndDay == null)
            {
                return await OnPostAsync();
            }

            if (EndDay <= StartDay)
            {
                return await OnPostAsync();
            }

            var response = await _client.PostAsJsonAsync(_api + "/AvailableCar", new GetAvailableCarRequest()
            {
                StartDateTime = (DateTime)StartDay,
                EndDateTime = (DateTime)EndDay
            });

            HttpContext.Session.SetObjectAsJson("StartDay", StartDay);
            HttpContext.Session.SetObjectAsJson("EndDay", EndDay);
            if (response.IsSuccessStatusCode)
            {
                List<CarInformationDto>? dataResponse =
                    response.Content.ReadFromJsonAsync<List<CarInformationDto>>().Result;
                if (dataResponse != null)
                {
                    CarAvailable = dataResponse;
                }
            }

            return Page();
        }
    }
}