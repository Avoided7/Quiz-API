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
      .Get("Questions.Options")
      .OrderByDescending(quiz => quiz.CreationDate)
      .Skip((page - 1) * pageSize)
      .Take(pageSize);
  }

  public Task<Quiz?> TryFindAsync(Guid id)
  {
    return _quizRepository.FindAsync(quiz => quiz.Id == id, "Questions", "Questions.Options");
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
    IRepository<Question> questionsRepository = _unitOfWork.GetRequiredRepository<Question>();
    IRepository<Option> optionsRepository = _unitOfWork.GetRequiredRepository<Option>();
    
    Quiz? quiz = await TryFindAsync(id);

    if (quiz is null)
    {
      return;
    }

    foreach (Question question in quiz.Questions)
    {
      optionsRepository.RemoveRange(question.Options);
    }
    
    questionsRepository.RemoveRange(quiz.Questions);
    _quizRepository.Remove(quiz);
    
    await _unitOfWork.SaveChangesAsync();
  }
  
  // CUSTOM

  public async Task PassQuizAsync(PassedQuiz passedQuiz)
  {
    Quiz? quiz = await TryFindAsync(passedQuiz.QuizId);
    double totalScore = 0d;

    if (quiz is null)
    {
      return;
    }
    
    foreach (MemberAnswer answer in passedQuiz.MemberAnswers)
    {
      answer.PassedQuizId = passedQuiz.Id;
      Question? question = quiz.Questions.FirstOrDefault(question => question.Id == answer.QuestionId);

      if (question is null)
      {
        return;
      }

      Option? option = question.Options.FirstOrDefault(option => option.Id == answer.OptionId);

      if (option is null)
      {
        return;
      }

      answer.Value = option.Value;
      totalScore += option.Value;
    }

    passedQuiz.MaxScore = quiz.MaxScore;
    passedQuiz.TotalScore = totalScore;

    IRepository<PassedQuiz> passedQuizRepository = _unitOfWork.GetRequiredRepository<PassedQuiz>();
    passedQuizRepository.Create(passedQuiz);

    await _unitOfWork.SaveChangesAsync();
  }
}