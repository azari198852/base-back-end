using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HandCarftBaseServer.Tools
{
    public static class ClaimPrincipalFactory
    {
        public static long GetUserId(ClaimsPrincipal claimsPrincipal)
        {
            return long.Parse(claimsPrincipal.Claims.Where(c => c.Type == "Id").Select(x => x.Value).DefaultIfEmpty("0").FirstOrDefault());
        }
        public static long GetFullName(ClaimsPrincipal claimsPrincipal)
        {
            return long.Parse(claimsPrincipal.Claims.Where(c => c.Type == "FullName").Select(x => x.Value).DefaultIfEmpty("0").FirstOrDefault());
        }
    }
}
