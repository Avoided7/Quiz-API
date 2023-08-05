using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuizAPI.Domain.Interfaces;
using QuizAPI.Infrastructure.Data;
using QuizAPI.Infrastructure.Implementation;

namespace QuizAPI.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
  public static IServiceCollection RegisterDbContext(this IServiceCollection serviceCollection, string connectionString)
  {
    ArgumentException.ThrowIfNullOrEmpty(connectionString);
    serviceCollection.AddDbContext<QuizContext>(options =>
    {
      options.UseSqlServer(connectionString);
    });

    return serviceCollection;
  }

  public static IServiceCollection RegisterRepositories(this IServiceCollection serviceCollection)
  {
    serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
    serviceCollection.AddScoped(typeof(IRepository<>), typeof(Repository<>));

    return serviceCollection;
  }
}