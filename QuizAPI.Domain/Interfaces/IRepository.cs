using System.Linq.Expressions;
using QuizAPI.Domain.Entities;

namespace QuizAPI.Domain.Interfaces;

public interface IRepository<T>
  where T: EntityBase
{
  IQueryable<T> Get(params string[] includes);
  IQueryable<T> Get(Expression<Func<T, bool>> expression, params string[] includes);

  Task<T?> FindAsync(Expression<Func<T, bool>> expression, params string[] includes);

  void Create(T entity);
  void Update(T entity);
  void Delete(T entity);
}