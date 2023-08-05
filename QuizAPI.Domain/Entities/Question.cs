using System.ComponentModel.DataAnnotations;

namespace QuizAPI.Domain.Entities;

public class Question : EntityBase
{
  [MaxLength(256)]
  public string Title { get; set; } = string.Empty;
  public double Value => Options?.Sum(option => option.Value) ?? 0;
  
  // Relations
  public virtual IList<Option> Options { get; set; } = null!;

  public Guid QuizId { get; set; }
}