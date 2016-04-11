using Constants;
using IdentityModel;
using IdentityModel.Client;
using IdentityModel.Extensions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ResourceOwner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var response = RequestToken();
            ShowResponse(response);

            CallService(response.AccessToken);
            Console.ReadLine();
        }

        static TokenResponse RequestToken()
        {
            var client = new TokenClient(
                Configuration.TokenEndpoint,
                "padmate_resourceowner",
                "padmate_resourceowner_secret");


            return client.RequestResourceOwnerPasswordAsync("Admin", "admin123", "dpcontrolapiscope").Result;
        }

        static void CallService(string token)
        {
            var baseAddress = Configuration.RSBaseAddress;

            var client = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };

            client.SetBearerToken(token);

            var response = Task.Run<string>(() => client.GetStringAsync(Configuration.CustomersAPI)).Result;

            "\n\nService claims:".ConsoleGreen();
            Console.WriteLine(response.ToString());
        }

        private static void ShowResponse(TokenResponse response)
        {
            if (!response.IsError)
            {
                "Token response:".ConsoleGreen();
                Console.WriteLine(response.Json);

                if (response.AccessToken.Contains("."))
                {
                    "\nAccess Token (decoded):".ConsoleGreen();

                    var parts = response.AccessToken.Split('.');
                    var header = parts[0];
                    var claims = parts[1];

                    Console.WriteLine(JObject.Parse(Encoding.UTF8.GetString(Base64Url.Decode(header))));
                    Console.WriteLine(JObject.Parse(Encoding.UTF8.GetString(Base64Url.Decode(claims))));
                }
            }
            else
            {
                if (response.IsHttpError)
                {
                    "HTTP error: ".ConsoleGreen();
                    Console.WriteLine(response.HttpErrorStatusCode);
                    "HTTP error reason: ".ConsoleGreen();
                    Console.WriteLine(response.HttpErrorReason);
                }
                else
                {
                    "Protocol error response:".ConsoleGreen();
                    Console.WriteLine(response.Json);
                }
            }
        }
    }
}
