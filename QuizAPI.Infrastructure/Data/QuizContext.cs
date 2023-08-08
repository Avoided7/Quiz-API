using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuizAPI.Domain.Entities;

namespace QuizAPI.Infrastructure.Data;

public class QuizContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
  public DbSet<Quiz> Quizzes { get; set; } = null!;
  public DbSet<Question> Questions { get; set; } = null!;
  public DbSet<Option> Options { get; set; } = null!;
  
  public DbSet<Member> Members { get; set; } = null!;

  public DbSet<PassedQuiz> PassedQuizzes { get; set; } = null!;
  
  public QuizContext(DbContextOptions<QuizContext> options) : base(options) { }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    base.OnConfiguring(optionsBuilder);

    optionsBuilder.UseLazyLoadingProxies();
  }
}