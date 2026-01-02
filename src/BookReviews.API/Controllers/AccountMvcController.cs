using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using BookReviews.Infrastructure.Identity;
using BookReviews.Application.Features.Auth.Commands.Login;
using BookReviews.Application.Features.Auth.Commands.Register;

public class AccountMvcController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IMediator _mediator;

    public AccountMvcController(
        SignInManager<ApplicationUser> signInManager,
        IMediator mediator)
    {
        _signInManager = signInManager;
        _mediator = mediator;
    }

    [HttpGet("account/login")]
    public IActionResult Login() => View();

    [HttpPost("account/login")]
    public async Task<IActionResult> Login(LoginCommand command)
    {
        var result = await _signInManager.PasswordSignInAsync(command.Email, command.Password, false, false);
        if (result.Succeeded) return RedirectToAction("Index", "BooksMvc");
        
        ModelState.AddModelError("", "Invalid login attempt");
        return View(command);
    }

    [HttpGet("account/register")]
    public IActionResult Register() => View();

    [HttpPost("account/register")]
    public async Task<IActionResult> Register(RegisterCommand command)
    {
        if (!ModelState.IsValid)
        {
            return View(command);
        }

        try
        {
            var userId = await _mediator.Send(command);
            
            var result = await _signInManager.PasswordSignInAsync(command.Email, command.Password, false, false);
            
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "BooksMvc");
            }
            
            return RedirectToAction("Login");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(command);
        }
    }

    [HttpGet("account/logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "BooksMvc");
    }
}