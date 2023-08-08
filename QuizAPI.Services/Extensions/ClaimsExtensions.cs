using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace QuizAPI.Services.Extensions;
internal static class ClaimsExtensions
{
  private static TimeSpan _expireTime = TimeSpan.FromMinutes(60);
  public static JwtSecurityToken CreateToken(this IEnumerable<Claim> claims, IConfiguration configuration)
  {
    JwtSecurityToken token = new JwtSecurityToken(
      configuration["JwtOptions:Issuer"],
      configuration["JwtOptions:Audience"],
      claims,
      expires: DateTime.UtcNow.Add(_expireTime),
      signingCredentials: configuration.CreateSigningCredentials());

    return token;
  }
}
