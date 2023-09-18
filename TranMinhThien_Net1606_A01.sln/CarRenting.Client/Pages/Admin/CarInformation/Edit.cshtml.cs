using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CarRenting.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            _carInformationApiUrl = Constants.ApiAdminCarInformation;
        }


        [BindProperty] public CarInformationDto CarInformation { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            HttpResponseMessage response = await _client.GetAsync(_carInformationApiUrl + "/" + id);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var dataResponse = response.Content.ReadFromJsonAsync<CarInformationDto>().Result;
                if (dataResponse != null)
                {
                    CarInformation = dataResponse;
                }
            }


            var manuData = await _client.GetAsync(Constants.ApiManufacture);
            var supplierData = await _client.GetAsync(Constants.ApiSupplier);

            if (manuData.StatusCode == System.Net.HttpStatusCode.OK
                && supplierData.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data1 = manuData.Content.ReadFromJsonAsync<List<ManufacturerDto>>().Result;
                var data2 = supplierData.Content.ReadFromJsonAsync<List<SupplierDto>>().Result;
                ViewData["ManufacturerId"] =
                    new SelectList(data1, "ManufacturerId", "ManufacturerName");
                ViewData["SupplierId"] = new SelectList(data2, "SupplierId", "SupplierName");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage response =
                    await _client.PutAsJsonAsync(_carInformationApiUrl + "/" + CarInformation.CarId, CarInformation);
            }

            return RedirectToPage("./Index");
        }
    }
}