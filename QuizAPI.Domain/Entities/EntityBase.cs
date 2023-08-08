using QuizAPI.Domain.Interfaces;

namespace QuizAPI.Domain.Entities;

public abstract class EntityBase : IEntity
{
  public Guid Id { get; set; } = Guid.NewGuid();
}