namespace QuizAPI.Domain.Entities;

public class PassedQuiz : EntityBase
{
  public DateTime PassedDate { get; set; } = DateTime.UtcNow;
  public Guid MemberId { get; set; }
  public Guid QuizId { get; set; }
  public double TotalScore { get; set; }
  public double MaxScore { get; set; }
  
  // Relations
  public virtual IList<MemberAnswer> MemberAnswers { get; set; } = null!;
}