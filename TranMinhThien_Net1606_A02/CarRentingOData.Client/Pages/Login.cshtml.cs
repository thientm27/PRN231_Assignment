using System.Net.Http.Headers;
using CarRenting.DTOs.Request;
using CarRentingOData.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPage.ViewModels;

namespace CarRenting.Client.Pages
{
    public class LoginModel : PageModel
    {
        private readonly HttpClient _client;
        private string _apiUrl;

        public LoginModel()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _apiUrl = Constants.ApiString + Constants.Customer;
        }

        [BindProperty] public CustomerDto MemberAccount { get; set; } = default!;

        public async Task<IActionResult> OnPostLogin()
        {
            if (MemberAccount == null)
            {
                return Page();
            }
            var response = await _client.PostAsJsonAsync(_apiUrl + "/login", new LoginRequest()
            {
                Email = MemberAccount.Email,
                Password = MemberAccount.Password
            });
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var dataResponse = response.Content.ReadFromJsonAsync<CustomerDto>().Result;
                if (dataResponse.CustomerID == -1)
                {
                    HttpContext.Session.SetInt32("User", -1);
                    return RedirectToPage("./Admin/Customer/Index");
                }

                HttpContext.Session.SetInt32("User", dataResponse.CustomerID);
                return RedirectToPage("./Customer/Index");
            }

            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                ViewData["notification"] = "Wrong email or password!";
                return Page();
            }
            else
            {
                return Page();
            }
        }

        public IActionResult OnPostLogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Login");
        }
    }
}