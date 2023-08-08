using Microsoft.AspNetCore.Identity;
using QuizAPI.Domain.Interfaces;

namespace QuizAPI.Domain.Entities;
public class ApplicationUser : IdentityUser<Guid>, IEntity
{
  public override Guid Id { get; set; } = Guid.NewGuid();

  public string? RefreshToken { get; set; }
  public DateTime TokenExpireTime { get; set; }
}
