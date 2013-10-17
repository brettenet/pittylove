using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AccessControl2.SDK;
using PittyLove.Model;

namespace PittyLove.FormsAuth.MessageHandlers
{
    public class TokenHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> 
            SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            /*
             * Step #1 Look for presence of auth header
             * and ensure the scheme is 'bearer'
             */
            if (request.Headers.Authorization != null
               && request
                    .Headers
                    .Authorization
                    .Scheme
                    .Equals("Bearer", StringComparison.OrdinalIgnoreCase))
            {
                //Step #2: Look for presence of the token
                if (!string.IsNullOrWhiteSpace(request.Headers.Authorization.Parameter))
                {
                    //Step #3: Get the Token from header
                    var token = request.Headers.Authorization.Parameter;

                    //Step #4 Validate the token using ACS Token Validator
                    if (new TokenValidator("accesscontrol.windows.net"
                        , "pittylove"
                        , "http://localhost:1320/"
                        , "tWHT7QQIk3YDMpyZmpsxuEADWUm00xfVxf2OgQ+dT5k=").Validate(token))
                    {
                     /*
                      * Step #5 If we found a user, convert user
                      * into a claims principal, and assign to current thread.
                      */
                        HttpContext.Current.User = token.ToTokenClaimsPrincipal();
                        Thread.CurrentPrincipal = HttpContext.Current.User;
                    }
                }
            }

            /*
             * Step #6 if the client was not authenticated, and the response
             * from our API is a 401, append the 'www-authenticate' header
             * with a scheme of apikey. This indicates to the client that
             * its needs to authenticate.
             */
            var httpResponse = await base.SendAsync(request, cancellationToken);
            if (httpResponse.StatusCode.Equals(HttpStatusCode.Unauthorized))
            {
                httpResponse.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue("token"));
            }

            return httpResponse;
        }
    }
}