using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using System.Security.Principal;

namespace PittyLove.Model
{
    public static class UserExtensions
    {
        public static IPrincipal ToClaimsPrincipal(this User user)
        {
            var claims = new List<Claim>
                {
                    new Claim("UserName", user.UserName),
                    new Claim("UserId", user.Id.ToString(CultureInfo.InvariantCulture)),
                };

            var claimsIdentity = new ClaimsIdentity(claims, "Basic", "UserName", ClaimTypes.Role);
            return new ClaimsPrincipal(claimsIdentity);
        }

        public static IPrincipal ToApiKeyClaimsPrincipal(this Device device)
        {
            var claims = new List<Claim>
                {
                    new Claim("UserName", device.PublicKey),
                    new Claim("UserId", device.PublicKey),
                };

            var claimsIdentity = new ClaimsIdentity(claims, "ApiKey", "UserName", ClaimTypes.Role);
            return new ClaimsPrincipal(claimsIdentity);
        }

        public static IPrincipal ToTokenClaimsPrincipal(this string token)
        {
            var claims = new List<Claim>
                {
                    new Claim("UserName", "ShareADog"),
                    new Claim("UserId", "ShareADog"),
                };

            var claimsIdentity = new ClaimsIdentity(claims, "Token", "UserName", ClaimTypes.Role);
            return new ClaimsPrincipal(claimsIdentity);
        }
    }
}
