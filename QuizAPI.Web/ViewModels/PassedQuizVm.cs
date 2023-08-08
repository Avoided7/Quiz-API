namespace QuizAPI.Web.ViewModels;

public class PassedQuizVm
{
  public Guid QuizId { get; set; }
  public Guid MemberId { get; set; }

  public List<MemberAnswerVm> MemberAnswers { get; set; } = null!;
}

public class MemberAnswerVm
{
  public Guid QuestionId { get; set; }
  public Guid OptionId { get; set; }
}