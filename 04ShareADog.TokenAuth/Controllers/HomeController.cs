using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
using _04ShareADog.TokenAuth.Models;

namespace _04ShareADog.TokenAuth.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to Share-A-Dog! Where your dogs are their dogs!";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Share()
        {
            ViewBag.Title = "Please Share-A-Dog!";
            ViewBag.Message = "You are sharing a dog with PittyLove.Org (You made the correct choice)";
            return View();
        }

        public ActionResult Create(Pitbull pitbull)
        {
            //Step #1: Get an authentication token from ACS
            var token = GetTokenFromAcs();

            /* Decoded Token Looks Like This
             * -----------------------------------------------------------------
             * nameidentifier---->bigdog
             * identityprovider-->https://pittylove.accesscontrol.windows.net/
             * Audience(RP)------>http://localhost:1320/&ExpiresOn=1379175400
             * Issuer------------>https://pittylove.accesscontrol.windows.net/
             * HMACSHA256-------->4MdeRcpOUTXkBGD/5heNhAbYr7jt2hYkDRaSUtyaR/M=
             */

            //Step #2 Create the HttpClient
            var client = new HttpClient();

            //Step #3 Add the token to auth header
            client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", token);

            //Step #4 Make the hppt post request to pitty love
            var result = client
                .PostAsJsonAsync("http://localhost:1320/api/pitbull/", pitbull)
                .Result;

            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new Exception("No Soup For You!");
            }

            //Step #5 Redirect to Index Page
            return View("Index");
        }

     

        private static string GetTokenFromAcs()
        {
            //Step #1 Create web client
            var client = new WebClient
                {
                    BaseAddress = "https://pittylove.accesscontrol.windows.net"
                };

            //Step #2 Add credentials to token request
            var values = new NameValueCollection
                {
                    {"wrap_name", "bigdog"},
                    {"wrap_password", "password"},
                    {"wrap_scope", "http://localhost:1320/"}
                };

            //Step #3 Make request
            var responseBytes = client.UploadValues("WRAPv0.9/", "POST", values);

            //Step #4 Get string from response bytes
            var response = Encoding.UTF8.GetString(responseBytes);

            //Step #5 Extract token from response
            return HttpUtility.UrlDecode(
                response
                    .Split('&')
                    .Single(value => value.StartsWith("wrap_access_token=", StringComparison.OrdinalIgnoreCase))
                    .Split('=')[1]);
        }
    }
}