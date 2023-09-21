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
            CarAvailable = new List<CarInformationDto>();
        }

        [BindProperty] public IList<RentingDetailDto> RentingDetail { get; set; } = default!;
        [BindProperty] public RentingTransaction RentingTransaction { get; set; } = default!;
        [BindProperty] public IList<CarInformationDto> CarAvailable { get; set; } = default!;

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime? StartDay { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime? EndDay { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            RentingTransaction = new RentingTransaction();
            RentingTransaction.RentingDate = DateTime.Today;
            RentingDetail = HttpContext.Session.GetObjectFromJson<CartItem>("Cart")?.Items ??
                            new List<RentingDetailDto>();
            return Page();
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

            RentingDetail = HttpContext.Session.GetObjectFromJson<CartItem>("Cart")?.Items ??
                            new List<RentingDetailDto>();

            if (response.IsSuccessStatusCode)
            {
                var ignoreCarTemp = RentingDetail
                    .Where(o => (StartDay >= o.StartDate && StartDay <= o.EndDate)
                                || (o.StartDate >= StartDay && o.EndDate <= EndDay)
                                || (o.StartDate <= EndDay && o.EndDate >= EndDay))
                    .Select(o => o.CarId)
                    .ToList();

                var availableCar = await response.Content.ReadFromJsonAsync<List<CarInformationDto>>();
                if (availableCar != null)
                {
                    var result = availableCar.Where(o => !ignoreCarTemp.Contains(o.CarId)).ToList();
                    CarAvailable = result;
                }
                else
                {
                    CarAvailable = new List<CarInformationDto>();
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAdd(int? id)
        {
            var sessionData = HttpContext.Session.GetObjectFromJson<CartItem>("Cart") ??
                              new CartItem();
            HttpResponseMessage response = await _client.GetAsync(Constants.ApiCarInformation + "/" + id);
            var addedItem = response.Content.ReadFromJsonAsync<CarInformationDto>().Result;
            if (addedItem != null)
            {
                TimeSpan rentalDuration = (DateTime)EndDay - (DateTime)StartDay;
                int numberOfDays = (int)rentalDuration.TotalDays;

                var rentingDetail = new RentingDetailDto
                {
                    CarId = (int)id,
                    StartDate = (DateTime)StartDay,
                    EndDate = (DateTime)EndDay,
                    Price = numberOfDays * addedItem.CarRentingPricePerDay,
                    CarName = addedItem.CarName
                };

                sessionData.Items.Add(rentingDetail);
            }

            HttpContext.Session.SetObjectAsJson("Cart", sessionData);

            return await OnGetAsync();
        }

        public async Task<IActionResult> OnPostRemove(int? id)
        {
            var sessionData = HttpContext.Session.GetObjectFromJson<CartItem>("Cart") ??
                              new CartItem();
            sessionData.Items.RemoveAt((int)id);
            HttpContext.Session.SetObjectAsJson("Cart", sessionData);
            return await OnGetAsync();
        }
    }

    public class CartItem
    {
        public List<RentingDetailDto> Items { get; set; } = new();
    }
}