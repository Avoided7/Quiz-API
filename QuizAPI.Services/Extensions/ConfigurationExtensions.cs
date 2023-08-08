using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QuizAPI.Services.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace QuizAPI.Services.Extensions;

internal static class ConfigurationExtensions
{
  public static SigningCredentials CreateSigningCredentials(this IConfiguration configuration)
  {
    return new SigningCredentials(
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtOptions:Secret"]!)),
        SecurityAlgorithms.HmacSha256);
  }

  public static string CreateRefreshToken(this IConfiguration configuration)
  {
    byte[] refreshTokenBytes = RandomNumberGenerator.GetBytes(64);

    return Convert.ToBase64String(refreshTokenBytes);
  }

  public static ClaimsPrincipal? GetPrincipalFromExpiredToken(this IConfiguration configuration, TokenDto token)
  {
    TokenValidationParameters validationParameters = new TokenValidationParameters()
    {
      ValidateAudience = true,
      ValidateIssuer = true,
      ValidateLifetime = true,
      ValidateIssuerSigningKey = true,
      ValidAudience = configuration["JwtOptions:Audience"],
      ValidIssuer = configuration["JwtOptions:Issuer"],
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtOptions:Secret"]!)),
    };

    JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
    try
    {
      ClaimsPrincipal principal = handler.ValidateToken(token.JwtToken, validationParameters, out SecurityToken validatedToken);

      if (validatedToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCulture))
      {
        throw new Exception("Invalid token.");
      }

      return principal;
    }
    catch
    {
      return null!;
    }
  }
}
