using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using QuizAPI.Domain.Entities;
using QuizAPI.Domain.Interfaces;
using QuizAPI.Infrastructure.Data;

namespace QuizAPI.Infrastructure.Implementation;

public class Repository<T> : IRepository<T>
  where T: EntityBase
{
  private readonly QuizContext _dbContext;

  public Repository(QuizContext dbContext)
  {
    _dbContext = dbContext;
  }
  
  public IQueryable<T> Get(params string[] includes)
  {
    IQueryable<T> result = _dbContext.Set<T>();

    foreach (string include in includes)
    {
      result = result.Include(include);
    }
    
    return result;
  }

  public IQueryable<T> Get(Expression<Func<T, bool>> expression, params string[] includes)
  {
    IQueryable<T> result = _dbContext.Set<T>().Where(expression);
    
    foreach (string include in includes)
    {
      result = result.Include(include);
    }
    
    return result;
  }

  public Task<T?> FindAsync(Expression<Func<T, bool>> expression, params string[] includes)
  {
    IQueryable<T> result = _dbContext.Set<T>();
    
    foreach (string include in includes)
    {
      result = result.Include(include);
    }
    
    return result.FirstOrDefaultAsync(expression);
  }

  public void Create(T entity)
  {
    _dbContext.Set<T>().Add(entity);
  }

  public void Update(T entity)
  {
    _dbContext.Set<T>().Update(entity);
  }

  public void Delete(T entity)
  {
    _dbContext.Set<T>().Remove(entity);
  }
}