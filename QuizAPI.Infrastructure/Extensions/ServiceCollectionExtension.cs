using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuizAPI.Domain.Entities;
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

  public static IServiceCollection RegisterIdentity(this IServiceCollection serviceCollection)
  {
    serviceCollection.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
      {
        options.User.RequireUniqueEmail = true;

        options.SignIn.RequireConfirmedPhoneNumber = false;
        options.SignIn.RequireConfirmedEmail = false;
        options.SignIn.RequireConfirmedAccount = false;

        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 4;
        options.Password.RequiredUniqueChars = 0;
      })
      .AddEntityFrameworkStores<QuizContext>();

    return serviceCollection;
  }
}