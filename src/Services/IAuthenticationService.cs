using Chat.Models;
using Newtonsoft.Json.Bson;

namespace Chat.Services;

public interface IAuthenticationService
{
    public Task<User> RegisterAsync(string userName, string password);
    public Task<string> LoginAsync(string username, string password);
    public string GenerateToken(User user);
    public bool ValidateToken();
}