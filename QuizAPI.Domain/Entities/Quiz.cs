using System.ComponentModel.DataAnnotations;

namespace QuizAPI.Domain.Entities;

public class Quiz : EntityBase
{
  [MaxLength(256)]
  public string Title { get; set; } = string.Empty;
  
  [MaxLength(1024)]
  public string Description { get; set; } = string.Empty;

  public int QuestionsCount => Questions?.Count ?? 0;
  public double MaxScore => Questions?.Sum(question => question.Value) ?? 0;

  public DateTime CreationDate { get; set; } = DateTime.UtcNow;
  
  // Relations
  public virtual IList<Question> Questions { get; set; } = null!;
}