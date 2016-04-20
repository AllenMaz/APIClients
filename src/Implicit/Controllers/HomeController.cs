using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using System.Net.Http;
using Constants;
using System.IO;

namespace Implicit.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult CallBack()
        {
            return Json("CallBack");
        }

        [Authorize]
        public IActionResult Secure()
        {
            return View();
        }
        
        [Authorize]
        public async Task<IActionResult> CallApi()
        {
            var token = User.FindFirst("access_token").Value;

            var client = new HttpClient();
            client.SetBearerToken(token);

            string customerApiUrl = Path.Combine(Configuration.RSBaseAddress, Configuration.CustomersAPI);

            var response = await client.GetStringAsync(customerApiUrl);
            ViewBag.Json = response.ToString();

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.Authentication.SignOutAsync("cookies");
            await HttpContext.Authentication.SignOutAsync("oidc");
            return Redirect("~/");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
