using System.ComponentModel.DataAnnotations;

namespace QuizAPI.Domain.Entities;

public class Option : EntityBase
{
  [MaxLength(100)]
  public string Title { get; set; } = string.Empty;
  public double Value { get; set; } = 0;
  
  // Relations
  public Guid QuestionId { get; set; }
}