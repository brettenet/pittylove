using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Windows.UI.Popups;
using _02PittyLove.WinRT2.Model;

namespace _02PittyLove.WinRT2.Services
{
    public class PittyLoveService : IPittyLoveService
    {
        public List<Pitbull> GetDogs()
        {
            /*
             * Step #1: Create HttpClient instance
             */
            using (var client = new HttpClient())
            {
                /*
                 * Step #2: Make Http Get Request
                 */
                var response = client.GetAsync("http://localhost:1320/api/pitbull").Result;

                /*
                 * Step #3: Deserialize Http response and send back to client
                 */
                return JsonConvert.DeserializeObject<List<Pitbull>>(response.Content.ReadAsStringAsync().Result);
            }
        }

        public Pitbull Save(Pitbull pitbull)
        {
            /*
             * Step #1: Create HttpClient instance
             */
            using (var client = new HttpClient())
            {
                /*
                 * Step #2: Set Authorization header to
                 * basic scheme, with a parameter of user:password
                 * in real life this value should be base 64 encoded.
                 */
                client.DefaultRequestHeaders.Authorization
                    = new AuthenticationHeaderValue("basic", "bigdog:password");
             
                /*
                 * Step #3: Create request content
                 */
                var content = new StringContent(JsonConvert.SerializeObject(pitbull)
                    , Encoding.UTF8
                    , "application/json");
                
                /*
                 * Step #4: Make Http Put Request
                 */
                var response = client.PutAsync(new Uri("http://localhost:1320/api/pitbull")
                    , content).Result;

                /*
                 * Step #5: Prompt for no access
                 */
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var confirmSaveDialog = new MessageDialog("No Soup For You!");
                    confirmSaveDialog.Commands.Add(new UICommand("OK"));
                    confirmSaveDialog.ShowAsync();
                }

                /*
                * Step #6: Deserialize Http response and send back to client
                */
                return JsonConvert.DeserializeObject<Pitbull>(response.Content.ReadAsStringAsync().Result);
            }
        }
    }
}
