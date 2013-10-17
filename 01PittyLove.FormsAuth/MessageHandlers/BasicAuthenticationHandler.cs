using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using PittyLove.Model;

namespace PittyLove.FormsAuth.MessageHandlers
{
    public class BasicAuthenticationHandler : DelegatingHandler
    {
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request
            , CancellationToken cancellationToken)
        {
            /*
             * Step #1: Look for the presence of the authorization 
             * header with a scheme of basic
             */
            if (request.Headers.Authorization != null
                && request
                    .Headers
                    .Authorization
                    .Scheme
                    .Equals("basic", StringComparison.OrdinalIgnoreCase))
            {
                /*
                 * Step #2: Extract credentials from authorization header
                 * format will be username:password (should be base 64 encoded)
                 */
                var credentials = request.Headers.Authorization.Parameter.Split(':');

                /*
                 * Step #3 Attempt to locate user in database
                 */
                var user = new EfUnitOfWork()
                    .Users
                    .GetByCredentials(credentials[0], credentials[1]);
                if (user != null)
                {
                    /*
                     * Step #4 If we found a user, convert user
                     * into a claims principal, and assign to current thread.
                     */
                    HttpContext.Current.User = user.ToClaimsPrincipal();
                    Thread.CurrentPrincipal = HttpContext.Current.User;
                }
            }

            /*
             * Step #5 if the client was not authenticated, and the response
             * from our API is a 401, append the 'www-authenticate' header
             * with a scheme of basic. This indicates to the client that
             * its needs to authenticate.
             */
            var httpResponse = await base.SendAsync(request, cancellationToken);
            if (httpResponse.StatusCode.Equals(HttpStatusCode.Unauthorized))
            {
                httpResponse
                    .Headers
                    .WwwAuthenticate
                    .Add(new AuthenticationHeaderValue("basic"));
            }

            return httpResponse;
        }
    }
}