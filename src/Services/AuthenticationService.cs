using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Chat.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Chat.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    
    public AuthenticationService(UserManager<User> userManager, IConfiguration configuration) {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<User> RegisterAsync(string userName, string password)
    {
        User user = new User{
            UserName = userName
        };

        IdentityResult result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded) {
            throw new Exception("User cannot be created");
        }

        return user;
    }
    
    public async Task<string> LoginAsync(string userName, string password)
    {
        User ?user = await _userManager.FindByNameAsync(userName);

        if (user == null || !await _userManager.CheckPasswordAsync(user, password) || user.GetType() != typeof(User)) {
            throw new Exception("User not found");
        }

        return this.GenerateToken(user);
    }

    public string GenerateToken(User user)
    {
        if (user.UserName == null) {
            throw new ArgumentNullException("user");
        }

        Claim[] claims = new[]{
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public bool ValidateToken()
    {
        throw new NotImplementedException();
    }
}