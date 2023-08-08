using QuizAPI.Domain.Entities;

namespace QuizAPI.Domain.Interfaces;

public interface IUnitOfWork
{
  Task SaveChangesAsync();
  IRepository<T> GetRequiredRepository<T>()
    where T: class, IEntity;
}