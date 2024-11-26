using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SurveyPlatform.BLL.Helpers
{
    internal class JwtHelper
    {
        internal static Guid? GetUserIdFromToken(HttpContext httpContext)
        {
            var authHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (authHeader != null && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();
                var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
                var userIdClaim = jwtToken?.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value;

                if (Guid.TryParse(userIdClaim, out var userId))
                {
                    return userId;
                }
            }
            return null;
        }
    }
}
