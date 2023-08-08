using Microsoft.Extensions.Configuration;
using QuizAPI.Domain.Entities;
using QuizAPI.Services.Dto;

namespace QuizAPI.Services.Interfaces;
public interface ITokenService
{
  Task<TokenDto> CreateJwtTokenAsync(ApplicationUser user);
  Task<TokenDto?> UpdateExpiredJwtTokenAsync(TokenDto token);
}
