﻿using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CarRenting.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarRenting.Client.Pages.Admin.CarInformation
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _client;
        private string _carInformationApiUrl;

        public CreateModel()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _carInformationApiUrl = Constants.ApiAdminCarInformation;
        }

        public async Task<IActionResult> OnGet()
        {
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

        [BindProperty] public CarInformationDto CarInformation { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            var error = false;
            if (CarInformation.NumberOfDoors <= 0)
            {
                ModelState.AddModelError("CarInformation.NumberOfDoors", "Number of doors must be greater than 0.");
                error = true;
            }

            if (CarInformation.SeatingCapacity <= 0)
            {
                ModelState.AddModelError("CarInformation.SeatingCapacity",
                    "Number of seating capacity must be greater than 0.");
                error = true;
            }

            if (CarInformation.CarRentingPricePerDay <= 0)
            {
                ModelState.AddModelError("CarInformation.CarRentingPricePerDay",
                    "CarRentingPricePerDay must be greater than 0.");
                error = true;
            }

            if (CarInformation.Year <= 1499 || CarInformation.Year > 2023)
            {
                ModelState.AddModelError("CarInformation.Year", "Year must from 1500 to 2023");
                error = true;
            }

            if (error)
            {
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

            HttpResponseMessage response = await _client.PostAsJsonAsync(_carInformationApiUrl, CarInformation);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }

            return RedirectToPage("./Index");
        }
    }
}