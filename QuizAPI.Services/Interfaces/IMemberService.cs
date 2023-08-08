using QuizAPI.Domain.Entities;

namespace QuizAPI.Services.Interfaces;

public interface IMemberService
{
  Task<Member?> GetByIdAsync(Guid id);

  Task CreateMemberAsync(Member member);
}