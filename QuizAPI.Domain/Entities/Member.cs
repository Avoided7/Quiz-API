using System.ComponentModel.DataAnnotations;

namespace QuizAPI.Domain.Entities;

public class Member : EntityBase
{
  [MaxLength(32)]
  public string Username { get; set; } = string.Empty;
  
  [MaxLength(64)]
  public string Email { get; set; } = string.Empty;
  
  // Relations
  public virtual IList<PassedQuiz> PassedQuizzes { get; set; } = null!;
}