using QuizAPI.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace QuizAPI.Services.Extensions;

internal static class UserExtensions
{
  public static List<Claim> GetClaims(this ApplicationUser user)
  {
    List<Claim> claims = new List<Claim>()
    {
      new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
      new (JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
      new (ClaimTypes.Email, user.Email!),
      new (ClaimTypes.NameIdentifier, user.Id!.ToString()),
      new (ClaimTypes.Name, user.UserName!),
    };

    return claims;
  }
}
