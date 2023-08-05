namespace QuizAPI.Domain.Entities;

public class MemberAnswer : EntityBase
{
  public Guid QuestionId { get; set; }
  public Guid OptionId { get; set; }
  public double Value { get; set; }
  
  // Relations
  public Guid PassedQuizId { get; set; }
  public PassedQuiz PassedQuiz { get; set; } = null!;
}