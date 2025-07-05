using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Yugen.HomeBudget.Data.Models;
using Yugen.HomeBudget.Shared.Contants;
using Yugen.HomeBudget.Shared.Models.Authentication;

namespace Yugen.HomeBudget.Server.Controllers;

[ApiController]
[Route($"{EndpointConstants.Prefix}/[controller]/[action]")]
public class AuthenticationController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthenticationController(
        UserManager<ApplicationUser> userManager, 
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return BadRequest("User does not exist");
        }

        var singInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!singInResult.Succeeded)
        {
            return BadRequest("Invalid password");
        }

        await _signInManager.SignInAsync(user, request.RememberMe);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterRequest parameters)
    {
        var user = new ApplicationUser
        {
            Email = parameters.Email
        };

        var result = await _userManager.CreateAsync(user, parameters.Password);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.FirstOrDefault()?.Description);
        }

        return await Login(new LoginRequest
        {
            Email = parameters.Email,
            Password = parameters.Password
        });
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }

    [HttpGet]
    public CurrentUser CurrentUserInfo()
    {
        var currentUser = new CurrentUser
        {
            IsAuthenticated = User.Identity?.IsAuthenticated ?? false,
            UserName = User.Identity?.Name ?? string.Empty,
            Claims = User.Claims.ToDictionary(c => c.Type, c => c.Value)
        };

        KeyValuePair<string, string>? currentUserIdentifier = currentUser.Claims.FirstOrDefault(c => c.Key.Contains("nameidentifier"));
        var success = int.TryParse(currentUserIdentifier?.Value, out var currentUserId);
        if (success)
        {
            currentUser.Id = currentUserId;
        }

        return currentUser;
    }
}