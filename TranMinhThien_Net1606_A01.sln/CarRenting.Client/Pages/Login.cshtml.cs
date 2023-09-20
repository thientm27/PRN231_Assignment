using System.Net.Http.Headers;
using CarRenting.DTOs;
using CarRenting.DTOs.Request;
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
            _apiUrl = Constants.ApiCustomer;
        }

        [BindProperty] public CustomerDto MemberAccount { get; set; } = default!;

        public async Task<IActionResult> OnPostLogin()
        {
            var response = await _client.PostAsJsonAsync(_apiUrl + "/login", new LoginRequest()
            {
                Email = MemberAccount.Email,
                Password = MemberAccount.Password
            });
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var dataResponse = response.Content.ReadFromJsonAsync<CustomerDto>().Result;
                if (dataResponse.CustomerId == -1)
                {
                    HttpContext.Session.SetObjectAsJson("User", dataResponse);
                    return RedirectToPage("./Admin/Customer/Index");
                }

                HttpContext.Session.SetObjectAsJson("User", dataResponse);
                return RedirectToPage("./Admin/Customer/Index");
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