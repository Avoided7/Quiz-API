using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizAPI.Domain.Entities;
using QuizAPI.Services.Interfaces;
using QuizAPI.Web.ViewModels;

namespace QuizAPI.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizzesController : ControllerBase
{
  private readonly IQuizService _quizService;
  private readonly IMapper _mapper;

  public QuizzesController(
    IQuizService quizService,
    IMapper mapper)
  {
    _quizService = quizService;
    _mapper = mapper;
  }
  
  [HttpGet]
  [ProducesResponseType(StatusCodes.Status200OK)]
  public ActionResult<IEnumerable<Quiz>> GetAll(int page = 1, int pageSize = 10)
  {
    IEnumerable<Quiz> quizzes = _quizService.Get(page, pageSize);

    IEnumerable<QuizVm> quizzesVm = quizzes.Select(quiz => _mapper.Map<QuizVm>(quiz));

    return Ok(quizzesVm);
  }

  [HttpGet("{id:guid}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<Quiz>> GetById(Guid id)
  {
    Quiz? quiz = await _quizService.TryFindAsync(id);

    if (quiz is null)
    {
      return NotFound();
    }

    QuizVm quizVm = _mapper.Map<QuizVm>(quiz);

    return Ok(quizVm);
  }

  [HttpPost]
  [ProducesResponseType(StatusCodes.Status201Created)]
  public async Task<ActionResult> CreateQuiz([FromBody] CreateQuizVm quizVm)
  {
    Quiz quiz = _mapper.Map<Quiz>(quizVm);

    await _quizService.CreateAsync(quiz);
    
    return CreatedAtAction(nameof(GetById), new { quiz.Id }, quiz);
  }

  [HttpPost("{id:guid}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  public async Task<ActionResult> PassQuiz([FromBody] PassedQuizVm passedQuizVm)
  {
    PassedQuiz passedQuiz = _mapper.Map<PassedQuiz>(passedQuizVm);

    await _quizService.PassQuizAsync(passedQuiz);
    
    return Ok(passedQuiz);
  }

  [HttpDelete("{id:guid}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  public async Task<ActionResult> DeleteQuiz(Guid id)
  {
    await _quizService.DeleteAsync(id);

    return Ok();
  }
}