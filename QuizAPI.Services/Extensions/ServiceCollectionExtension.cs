using Microsoft.Extensions.DependencyInjection;
using QuizAPI.Services.Implementation;
using QuizAPI.Services.Interfaces;

namespace QuizAPI.Services.Extensions;

public static class ServiceCollectionExtension
{
  public static IServiceCollection RegisterServices(this IServiceCollection serviceCollection)
  {
    serviceCollection
      .AddScoped<IQuizService, QuizService>()
      .AddScoped<IMemberService, MemberService>()
      .AddScoped<ITokenService, TokenService>();

    return serviceCollection;
  }
}