using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CarRenting.DTOs.Request;
using RazorPage.ViewModels;
using CarRentingOData.DTOs;
using CarRentingOData.DTOs.Request;
using CarRentingOData.DTOs.Response;

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
            _api = Constants.ApiString + Constants.CarRental;
            CarAvailable = new List<CarDto>();
        }

        [BindProperty] public IList<CarRentalDto> RentingTemp { get; set; } = default!;
        [BindProperty] public CarRentalDto RentingTransaction { get; set; } = default!;
        [BindProperty] public IList<CarDto> CarAvailable { get; set; } = default!;

        [BindProperty] public string Error { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime? StartDay { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime? EndDay { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = HttpContext.Session.GetInt32("User");
            if (userId == null || userId < 0)
            {
                return RedirectToPage("../Login");
            }

            RentingTemp = HttpContext.Session.GetObjectFromJson<NewRenting>("Cart")?.Values ??
                            new List<CarRentalDto>();

            RentingTransaction = new CarRentalDto();
            return Page();
        }


        public async Task<IActionResult> OnPostSearch()
        {
            var userId = HttpContext.Session.GetInt32("User");
            if (userId == null || userId < 0)
            {
                return RedirectToPage("../Login");
            }

            if (StartDay == null || EndDay == null)
            {
                Error = "Pleas pick date to search car";
                return await OnGetAsync();
            }

            if (EndDay <= StartDay)
            {
                Error = "EndDay must > StartDay";
                return await OnGetAsync();
            }

            Error = "";
            string filterString = "?$filter=status ne '0'&$expand=CarProducer";
            string requestUrl = Constants.OdataString + Constants.CarInformation + filterString;
            HttpResponseMessage responseCar = await _client.GetAsync(requestUrl);
            var availableCar = new ODataResponse<CarDto>();
            if (responseCar.IsSuccessStatusCode)
            {
                availableCar = await responseCar.Content.ReadFromJsonAsync<ODataResponse<CarDto>>();

            }
            ////////
            DateTime startDay = (DateTime)StartDay;
            DateTime endDay = (DateTime)EndDay;
            string formattedStartDay = startDay.ToString("yyyy-MM-dd");
            string formattedEndDay = endDay.ToString("yyyy-MM-dd");
            string statusCondition = "status ne '0'";  // Corrected status condition

            string filterString2 = $"?$filter={statusCondition} and ((PickupDate ge {formattedStartDay} and PickupDate le {formattedEndDay}) or (ReturnDate ge {formattedStartDay} and ReturnDate le {formattedEndDay}) or (PickupDate le {formattedStartDay} and ReturnDate ge {formattedEndDay}))";
            string requestUrl2 = Constants.OdataString + Constants.CarRental + filterString2;

            var ignoreList = await _client.GetAsync(requestUrl2);
            ////
            if (ignoreList.IsSuccessStatusCode)
            {
                var ignoreId = await ignoreList.Content.ReadFromJsonAsync<ODataResponse<CarRentalDto>>();
                if (ignoreId != null && ignoreId.Value.Count > 0)
                {
                    var tmp = ignoreId.Value.Select(o => o.CarID).ToList();
                    var result = availableCar.Value.Where(o => !tmp.Contains(o.CarID)).ToList();
                    availableCar.Value = result;

                }
            }

            // Store picker
            HttpContext.Session.SetObjectAsJson("StartDay", StartDay);
            HttpContext.Session.SetObjectAsJson("EndDay", EndDay);

            // Remove car in cart
            RentingTemp = HttpContext.Session.GetObjectFromJson<NewRenting>("Cart")?.Values ??
                       new List<CarRentalDto>();

            var ignoreCarTemp = RentingTemp
                .Where(o => (StartDay >= o.PickupDate && StartDay <= o.ReturnDate)
                            || (o.PickupDate >= StartDay && o.ReturnDate <= EndDay)
                            || (o.PickupDate <= EndDay && o.ReturnDate >= EndDay))
                .Select(o => o.CarID)
                .ToList();

            // finish
            if (availableCar != null)
            {
                var result = availableCar.Value.Where(o => !ignoreCarTemp.Contains(o.CarID)).ToList();
                CarAvailable = result;
            }
            else
            {
                CarAvailable = new List<CarDto>();
            }


            return Page();
        }

        public async Task<IActionResult> OnPostAdd(int? id)
        {
            var userId = HttpContext.Session.GetInt32("User");
            if (userId == null || userId < 0)
            {
                return RedirectToPage("../Login");
            }

            var sessionData = HttpContext.Session.GetObjectFromJson<NewRenting>("Cart") ??
                       new NewRenting();

            HttpResponseMessage response = await _client.GetAsync(Constants.OdataString + Constants.CarInformation + "?$filter=carID eq " + id + "&$expand=CarProducer");
            var addedItem = response.Content.ReadFromJsonAsync<ODataResponse<CarDto>>().Result;
            if (addedItem.Value != null)
            {
                TimeSpan rentalDuration = (DateTime)EndDay - (DateTime)StartDay;
                int numberOfDays = (int)rentalDuration.TotalDays;
                var rentingDetail = new CarRentalDto
                {
                    CustomerID = (int)userId,
                    CarID = (int)id,
                    PickupDate = (DateTime)StartDay,
                    ReturnDate = (DateTime)EndDay,
                    RentPrice = (numberOfDays * addedItem.Value[0].RentPrice),
                    Status = "Pending",
                    Car = addedItem.Value[0]
                };

                sessionData.Values.Add(rentingDetail);
            }

            HttpContext.Session.SetObjectAsJson("Cart", sessionData);

            return await OnGetAsync();
        }

        public async Task<IActionResult> OnPostRemove(int? id)
        {
            var userId = HttpContext.Session.GetInt32("User");
            if (userId == null || userId < 0)
            {
                return RedirectToPage("../Login");

            }
            var sessionData = HttpContext.Session.GetObjectFromJson<NewRenting>("Cart") ??
                                  new NewRenting();

            sessionData.Values.RemoveAt((int)id);
            HttpContext.Session.SetObjectAsJson("Cart", sessionData);
            return await OnGetAsync();
        }


        public async Task<IActionResult> OnPostSubmit()
        {
            var userId = HttpContext.Session.GetInt32("User");
            if (userId == null || userId < 0)
            {
                HttpContext.Session.SetObjectAsJson("Cart", null);
                return RedirectToPage("../Login");
            }

            var sessionData = HttpContext.Session.GetObjectFromJson<NewRenting>("Cart") ??
                                        new NewRenting();

            if (sessionData.Values.Count > 0)
            {

                await _client.PostAsJsonAsync(_api, sessionData);
            }

            HttpContext.Session.SetObjectAsJson("Cart", null);
            return RedirectToPage("./Index");
        }
    }

}