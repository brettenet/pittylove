using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using PittyLove.Model;

namespace PittyLove.FormsAuth.MessageHandlers
{
    public class ApiKeyHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request
            , CancellationToken cancellationToken)
        {
            /*
             * Step #1: Look for the presence of the authorization 
             * header with a scheme of apikey
             */
             if (request.Headers.Authorization != null
                && request
                    .Headers
                    .Authorization
                    .Scheme
                    .Equals("apikey", StringComparison.OrdinalIgnoreCase))
            {
                //Step #2: Look for presence of public api key and signature
                if (request.Headers.Contains("X-ApiKey")
                    && !string.IsNullOrWhiteSpace(request.Headers.Authorization.Parameter))
                {
                    //Step #3: Get the Signature from header
                    var inboundSignature = request.Headers.Authorization.Parameter;

                    //Step #4: Get the public api key from header
                    var publicKey = request.Headers.GetValues("X-ApiKey").First();

                    //Step #5: Look up private key based on public key
                    var device = new EfUnitOfWork()
                        .Devices
                        .GetByPublicKey(publicKey);
                    if (device != null)
                    {
                        var computeSignature = CreateSignature(device.PrivateKey
                                                               , device.PublicKey
                                                               , request.RequestUri.ToString()
                                                               , request.Method);

                        //Step #6: Compare the computed hash with inbound hash
                        if (computeSignature == inboundSignature)
                        {
                            //Step #7: Set Claims Principal
                            HttpContext.Current.User = device.ToApiKeyClaimsPrincipal();
                            Thread.CurrentPrincipal = HttpContext.Current.User;
                        }
                    }
                }

                //Step #8: Pass the request to next handler
                return await base.SendAsync(request, cancellationToken);
            }

            /*
            * Step #9 if the client was not authenticated, and the response
            * from our API is a 401, append the 'www-authenticate' header
            * with a scheme of apikey. This indicates to the client that
            * its needs to authenticate.
            */
             var httpResponse = await base.SendAsync(request, cancellationToken);
             if (httpResponse.StatusCode.Equals(HttpStatusCode.Unauthorized))
             {
                 httpResponse.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue("apikey"));
             }

             return httpResponse;
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