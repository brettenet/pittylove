using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;

namespace _03PittyLove.ApiKeyAuth
{
    class Program
    {
        static void Main(string[] args)
        {
            //Hard coded public private key pair
            const string publicKey = "px2+TlBhiPHhL0c/G7OEOMkycdQh+9ua6PeeHpzc7VXE9vLavAVacA+ke6pGmze2TBMi8Jb0Em3BkrNk6GsA7g==";
            const string privateKey = "oXpP7hFfeYB36bILwarmJGpBgH3ZihKYuXPWmnYtaq4aI7f7s25Zgx5cGpI4yFJGtKFd8sJrZZ2BwlqiEN1uWw==";

            //Endpoint
            const string uri = "http://localhost:1320/api/pitbull/{0}/Meal/{1}";

            var exit = false;
            Console.WriteLine("Welcome to DogFeeder Pro!");
            while (!exit)
            {
                Console.WriteLine("\nWho would you like to feed?");
                Console.WriteLine("1. Olive");
                Console.WriteLine("2. Jibbom");
                Console.WriteLine("3. Oscar");
                var key = Console.ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.D2:
                    case ConsoleKey.D3:

                        var client = new HttpClient();

                        //Step #1: Assemble uri
                        var requestUri = string.Format(uri
                            , key.KeyChar
                            , DateTime.Now.ToString("MM-dd-yyyy"));

                        //Step #2: Add public Key to header
                        client.DefaultRequestHeaders
                            .Add("X-ApiKey", publicKey);

                        //Step #3: Create a hashed signature
                        var signature = CreateSignature(privateKey
                            , publicKey
                            , requestUri
                            , HttpMethod.Put);

                        //Step #4: Add hashed signature to headers
                        client.DefaultRequestHeaders.Authorization
                            = new AuthenticationHeaderValue("apikey", signature);

                        //Step #5: Make the PUT request
                        var result = client.PutAsync(requestUri, null).Result;

                        //Step #6 Look at response
                        if (result.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            Console.WriteLine("\n\nNO SOUP FOR YOU!!!!!");
                        }
                        else
                        {
                            Console.WriteLine("\n\nDog was fed... You are a good person.");
                        }
                        break;
                    case ConsoleKey.E:
                        exit = true;
                        break;
                }
            }
        }

        private static string CreateSignature(string privateKey, string publicKey, string requestUri, HttpMethod requestMethod)
        {
            var privateKeyAsBytes = new byte[privateKey.Length * sizeof(char)];
            Buffer.BlockCopy(privateKey.ToCharArray(), 0, privateKeyAsBytes, 0, privateKeyAsBytes.Length);

            var contentToHash = string.Concat(publicKey, requestUri, requestMethod);
            var contentToHashAsBytes = new byte[contentToHash.Length * sizeof(char)];
            Buffer.BlockCopy(contentToHash.ToCharArray(), 0, contentToHashAsBytes, 0, contentToHashAsBytes.Length);
            using (var hmac = new HMACSHA256(privateKeyAsBytes))
            {
                var signatureBytes = hmac.ComputeHash(contentToHashAsBytes);
                return Convert.ToBase64String(signatureBytes);
            }
        }
    }
}
