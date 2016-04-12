using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using System.Net.Http;
using Constants;
using System.IO;
using IdentityModel;
using IdentityModel.Client;
using System.Diagnostics;

namespace AuthorizationCode.Controllers
{

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            

            return View();
        }


        public async Task CallBack(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                var state = CryptoRandom.CreateRandomKeyString(64);
                var nonce = CryptoRandom.CreateRandomKeyString(64);

                var request = new AuthorizeRequest("http://localhost:11242/connect/authorize");
                var url = request.CreateAuthorizeUrl(
                    clientId: "padmate_AuthorizationCode",
                    responseType: "code",
                    scope: "dpcontrolapiscope",
                    redirectUri: "http://localhost:27898/Home/CallBack",
                    state: state,
                    nonce: nonce);

                Process.Start(url);
            }
            else
            {
                var client = new TokenClient(
                 Configuration.TokenEndpoint,
                "padmate_AuthorizationCode",
                "padmate_authorizationcode_secret");

                var response = await client.RequestAuthorizationCodeAsync(code, "http://localhost:27898/Home/CallBack");
                await this.CallApi(response.AccessToken);
            }
        }

        [Authorize]
        public async Task<IActionResult> CallApi(string token)
        {
            var client = new HttpClient();
            client.SetBearerToken(token);

            string customerApiUrl = Path.Combine(Configuration.RSBaseAddress, Configuration.CustomersAPI);

            var response = await client.GetStringAsync(customerApiUrl);
            ViewBag.Json = response.ToString();

            return View();
        }


        [Authorize]
        public IActionResult Secure()
        {
            return View();
        }
        

        public async Task<IActionResult> Logout()
        {
            await HttpContext.Authentication.SignOutAsync("cookies");
            return Redirect("~/");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
