using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PacTaskAPI.Extensions
{
    public static class ClaimsExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            // Since this function will only runs when the user exists, i can ignore the null warning
            #pragma warning disable CS8602 // Dereference of a possibly null reference.
            return user.Claims.SingleOrDefault(x => x.Type.Equals(ClaimTypes.GivenName)).Value;
            #pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }
}