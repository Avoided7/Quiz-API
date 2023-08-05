using QuizAPI.Domain.Entities;

namespace QuizAPI.Services.Interfaces;

public interface IQuizService
{
  IEnumerable<Quiz> Get(int page = 1, int pageSize = 10);
  
  Task<Quiz?> TryFindAsync(Guid id);
  
  Task CreateAsync(Quiz quiz);
  Task UpdateAsync(Quiz quiz);
  Task DeleteAsync(Guid id);
}