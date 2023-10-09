using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CarRenting.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;
using CarRentingOData.DTOs;

namespace CarRenting.Client.Pages.Admin.CarInformation
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _client;
        private string _carInformationApiUrl;

        public EditModel()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _carInformationApiUrl = Constants.ApiString + Constants.CarInformation;
        }


        [BindProperty] public CarDto CarInformation { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var userId = HttpContext.Session.GetInt32("User");
            if (userId == null || userId != -1)
            {
                return RedirectToPage("../../Login");
            }
            
            HttpResponseMessage response = await _client.GetAsync(_carInformationApiUrl + "/" + id);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var dataResponse = response.Content.ReadFromJsonAsync<CarDto>().Result;
                if (dataResponse != null)
                {
                    CarInformation = dataResponse;
                }
            }


            var manuData = await _client.GetAsync(Constants.ApiString + Constants.CarProducer);

            if (manuData.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data1 = manuData.Content.ReadFromJsonAsync<List<CarProducerDto>>().Result;
                ViewData["ManufacturerId"] =
                    new SelectList(data1, "ManufacturerId", "ManufacturerName");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var error = false;
            //if (CarInformation.Capacity <= 0)
            //{
            //    ModelState.AddModelError("CarInformation.Capacity", "Number of doors must be greater than 0.");
            //    error = true;
            //}

            //if (CarInformation.Capacity <= 0)
            //{
            //    ModelState.AddModelError("CarInformation.SeatingCapacity",
            //        "Number of seating capacity must be greater than 0.");
            //    error = true;
            //}

            //if (CarInformation.CarRentingPricePerDay <= 0)
            //{
            //    ModelState.AddModelError("CarInformation.CarRentingPricePerDay",
            //        "CarRentingPricePerDay must be greater than 0.");
            //    error = true;
            //}

            //if (CarInformation.Year <= 1499 || CarInformation.Year > 2023)
            //{
            //    ModelState.AddModelError("CarInformation.Year", "Year must from 1500 to 2023");
            //    error = true;
            //}

            if (error)
            {

                var manuData = await _client.GetAsync(Constants.ApiString + Constants.CarProducer);

                if (manuData.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var data1 = manuData.Content.ReadFromJsonAsync<List<CarProducerDto>>().Result;
                    ViewData["ProducerID"] =
                        new SelectList(data1, "ProducerID", "ProducerName");
                }

                return Page();
            }

            if (ModelState.IsValid)
            {
                HttpResponseMessage response =
                    await _client.PutAsJsonAsync(_carInformationApiUrl + "/" + CarInformation.CarID, CarInformation);
            }

            return RedirectToPage("./Index");
        }
    }
}