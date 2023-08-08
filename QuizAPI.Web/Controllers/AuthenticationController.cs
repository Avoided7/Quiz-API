using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizAPI.Domain.Entities;
using QuizAPI.Services.Dto;
using QuizAPI.Services.Interfaces;
using QuizAPI.Web.ViewModels;

namespace QuizAPI.Web.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly ITokenService _tokenService;
  private readonly IMemberService _memberService;

  public AuthenticationController(
    UserManager<ApplicationUser> userManager, 
    ITokenService tokenService,
    IMemberService memberService)
  {
    _userManager = userManager;
    _tokenService = tokenService;
    _memberService = memberService;
  }

  [HttpPost("login")]
  public async Task<ActionResult> Login([FromBody] LoginVm loginVm)
  {
    ApplicationUser? existedUser = await _userManager.FindByNameAsync(loginVm.Username);

    if(existedUser is null)
    {
      return NotFound("User not found.");
    }

    bool isPasswordCorrect = await _userManager.CheckPasswordAsync(existedUser, loginVm.Password);
    
    if (!isPasswordCorrect)
    {
      return BadRequest("Incorrect password.");
    }

    TokenDto token = await _tokenService.CreateJwtTokenAsync(existedUser);

    Member? member = await _memberService.GetByIdAsync(existedUser.Id);

    if(member is null)
    {
      throw new Exception("Existed user doesn't have member.");
    }

    return Ok(new
    {
      tokens = token,
      member
    });
  }

  [HttpPost("register")]
  public async Task<ActionResult> Register([FromBody] RegisterVm registerVm)
  {
    ApplicationUser user = new ApplicationUser()
    {
      Email = registerVm.Email,
      UserName = registerVm.Username
    };

    IdentityResult registerResult = await _userManager.CreateAsync(user, registerVm.Password);

    if(!registerResult.Succeeded)
    {
      foreach (IdentityError error in registerResult.Errors)
      {
        ModelState.AddModelError("Errors", error.Description);
      }
      
      return BadRequest(ModelState);
    }

    Member member = new Member()
    {
      Id = user.Id,
      Email = user.Email,
      Username = user.UserName
    };

    await _memberService.CreateMemberAsync(member);

    return await Login(new LoginVm()
    {
      Username = registerVm.Username,
      Password = registerVm.Password
    });
  }

  [HttpPost("refresh-token")]
  public async Task<ActionResult> UpdateToken([FromBody] TokenDto token)
  {
    TokenDto? newToken = await _tokenService.UpdateExpiredJwtTokenAsync(token);
    
    if(newToken is null)
    {
      return BadRequest("Invalid access token or refresh token.");
    }
    
    return Ok(newToken);
  }
}
