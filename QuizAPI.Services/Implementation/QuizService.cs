using QuizAPI.Domain.Entities;
using QuizAPI.Domain.Interfaces;
using QuizAPI.Services.Interfaces;

namespace QuizAPI.Services.Implementation;

public class QuizService : IQuizService
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly IRepository<Quiz> _quizRepository;

  public QuizService(IUnitOfWork unitOfWork)
  {
    _unitOfWork = unitOfWork;
    
    _quizRepository = _unitOfWork.GetRequiredRepository<Quiz>();
  }
  
  public IEnumerable<Quiz> Get(int page = 1, int pageSize = 10)
  {
    return _quizRepository
      .Get("Questions", "Questions.Options")
      .OrderByDescending(quiz => quiz.CreationDate)
      .Skip((page - 1) * pageSize)
      .Take(pageSize);
  }

  public Task<Quiz?> TryFindAsync(Guid id)
  {
    return _quizRepository.FindAsync(quiz => quiz.Id == id);
  }

  public async Task CreateAsync(Quiz quiz)
  {
    // TODO: Validation
    IRepository<Question> questionsRepository = _unitOfWork.GetRequiredRepository<Question>();
    IRepository<Option> optionsRepository = _unitOfWork.GetRequiredRepository<Option>();

    foreach (Question question in quiz.Questions)
    {
      question.QuizId = quiz.Id;
      questionsRepository.Create(question);
      
      foreach (Option option in question.Options)
      {
        option.QuestionId = question.Id;
        optionsRepository.Create(option);
      }
    }
    
    _quizRepository.Create(quiz);
    await _unitOfWork.SaveChangesAsync();
  }

  public Task UpdateAsync(Quiz quiz)
  {
    // TODO: Validation
    _quizRepository.Update(quiz);
    _unitOfWork.SaveChangesAsync();

    return Task.CompletedTask;
  }

  public async Task DeleteAsync(Guid id)
  {
    // TODO: Return result
    Quiz? quiz = await _quizRepository.FindAsync(quiz => quiz.Id == id);

    if (quiz is null)
    {
      return;
    }
    
    _quizRepository.Delete(quiz);
    await _unitOfWork.SaveChangesAsync();
  }
}