using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SurveyPlatform.BLL.Interfaces;
using SurveyPlatform.Core;
using SurveyPlatform.DAL.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SurveyPlatform.BLL.Services;
public class TokenService(
        IConfiguration configuration
    ) : ITokenService
{
    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(CreateClaims(user)),
            Expires = DateTime.UtcNow.AddYears(1),
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    private IEnumerable<Claim> CreateClaims(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.AuthorizationDecision , user.IsDeactivated.ToString())
        };
        foreach (var role in user.Roles)
        {
            if (Enum.TryParse<Roles>(role, out var parsedRole))
            {
                claims.Add(new Claim(ClaimTypes.Role, ((int)parsedRole).ToString()));
            }
        }
        return claims;
    }
}
