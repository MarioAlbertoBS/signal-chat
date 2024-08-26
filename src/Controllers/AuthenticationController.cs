namespace Chat.Controllers;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Chat.Data.Dtos;
using Chat.Models;
using Chat.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("api")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _jwtService;

    public AuthenticationController(IAuthenticationService jwtService) {
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserAuthenticationRequest userLoginDto) {
        try {
            User user = await _jwtService.RegisterAsync(userLoginDto.UserName, userLoginDto.Password);
            return CreatedAtAction(nameof(Register), new UserRegisterResponseDto{
                Id = user.Id,
                UserName = user.UserName
            });
        } catch(Exception ex) {
            return BadRequest(new { message = "Cannot Register User!"} );
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserAuthenticationRequest userLoginDto)
    {
        try {
            string token = await _jwtService.LoginAsync(userLoginDto.UserName, userLoginDto.Password);
            return Ok(new { token = token });
        } catch (Exception ex) {
            return BadRequest(new { message = "Cannot Login!" });
        }
    }
}