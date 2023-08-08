using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizAPI.Domain.Entities;
using QuizAPI.Services.Interfaces;

namespace QuizAPI.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembersController : ControllerBase
{
  private readonly IMemberService _memberService;

  public MembersController(IMemberService memberService)
  {
    _memberService = memberService;
  }

  [HttpGet("{id:guid}")]
  public async Task<ActionResult> GetById(Guid id)
  {
    Member? member = await _memberService.GetByIdAsync(id);

    if (member is null)
    {
      return NotFound();
    }
    
    return Ok(member);
  }
}