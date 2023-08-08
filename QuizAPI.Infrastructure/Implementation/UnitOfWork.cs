using Microsoft.Extensions.DependencyInjection;
using QuizAPI.Domain.Interfaces;
using QuizAPI.Infrastructure.Data;

namespace QuizAPI.Infrastructure.Implementation;

public class UnitOfWork : IUnitOfWork
{
  private readonly IServiceProvider _serviceProvider;
  private readonly QuizContext _dbContext;

  public UnitOfWork(IServiceProvider serviceProvider, QuizContext dbContext)
  {
    _serviceProvider = serviceProvider;
    _dbContext = dbContext;
  }
  
  public Task SaveChangesAsync()
  {
    return _dbContext.SaveChangesAsync();
  }

  public IRepository<T> GetRequiredRepository<T>() where T: class, IEntity
  {
    return _serviceProvider.GetRequiredService<IRepository<T>>();
  }
}