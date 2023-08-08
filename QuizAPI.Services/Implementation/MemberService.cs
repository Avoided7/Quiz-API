using QuizAPI.Domain.Entities;
using QuizAPI.Domain.Interfaces;
using QuizAPI.Services.Interfaces;

namespace QuizAPI.Services.Implementation;

public class MemberService : IMemberService
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly IRepository<Member> _memberRepository;

  public MemberService(IUnitOfWork unitOfWork)
  {
    _unitOfWork = unitOfWork;

    _memberRepository = unitOfWork.GetRequiredRepository<Member>();
  }

  public Task CreateMemberAsync(Member member)
  {
    // TODO: Validate.
    _memberRepository.Create(member);
    return _unitOfWork.SaveChangesAsync();
  }

  public Task<Member?> GetByIdAsync(Guid id)
  {
    return _memberRepository.FindAsync(member => member.Id == id, "PassedQuizzes.MemberAnswers");
  }
}