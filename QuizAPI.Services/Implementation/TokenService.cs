using Microsoft.Extensions.Configuration;
using QuizAPI.Domain.Entities;
using QuizAPI.Domain.Interfaces;
using QuizAPI.Services.Dto;
using QuizAPI.Services.Extensions;
using QuizAPI.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace QuizAPI.Services.Implementation;
public class TokenService : ITokenService
{
  private readonly IConfiguration _configuration;
  private readonly IUnitOfWork _unitOfWork;

  private readonly IRepository<ApplicationUser> _userRepository;

  private readonly TimeSpan _refreshTokenExpireTime = TimeSpan.FromDays(7);

  public TokenService(IConfiguration configuration, IUnitOfWork unitOfWork)
  {
    _configuration = configuration;
    _unitOfWork = unitOfWork;

    _userRepository = _unitOfWork.GetRequiredRepository<ApplicationUser>();

  }
  public async Task<TokenDto> CreateJwtTokenAsync(ApplicationUser user)
  {
    JwtSecurityToken token = user
      .GetClaims()
      .CreateToken(_configuration);

    JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

    string accessToken = handler.WriteToken(token);
    string refreshToken = _configuration.CreateRefreshToken();


    user.RefreshToken = refreshToken;
    user.TokenExpireTime = DateTime.UtcNow.Add(_refreshTokenExpireTime);

    _userRepository.Update(user);
    await _unitOfWork.SaveChangesAsync();

    return new TokenDto() {
      JwtToken = accessToken,
      RefreshToken = refreshToken
    };
  }

  public async Task<TokenDto?> UpdateExpiredJwtTokenAsync(TokenDto token)
  {
    string accessToken = token.JwtToken;
    string refreshToken = token.RefreshToken;

    ClaimsPrincipal? principal =  _configuration.GetPrincipalFromExpiredToken(token);

    if(principal is null)
    {
      return null;
    }

    ApplicationUser? user = await _userRepository.FindAsync(user => user.UserName == principal.Identity!.Name);

    if (user is null || refreshToken != user.RefreshToken || user.TokenExpireTime <= DateTime.UtcNow)
    {
      return null;
    }

    TokenDto newToken = await CreateJwtTokenAsync(user);

    return newToken;
  }
}
