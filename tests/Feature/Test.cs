namespace Chat.Tests.Feature;

using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Chat.Data;
using Chat.Models;
using Chat.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Xunit;

public class Test : IClassFixture<TestApplicationFactory>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly UserManager<User> _userManager;

    protected readonly HttpClient _client;
    protected readonly TestApplicationFactory _factory;
    protected readonly ChatContext _context;
    protected User _authenticatedUser;

    public Test(TestApplicationFactory factory) {
        _factory = factory;
        _client = factory.CreateClient();

        _authenticationService = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<IAuthenticationService>();
        _userManager = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<UserManager<User>>();
        _context = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<ChatContext>();
    }

    protected async Task<User> GenerateUser(string userName, string password) {
        _authenticatedUser = await _authenticationService.RegisterAsync(userName, password);
        return _authenticatedUser;
    }

    protected async Task<string> GenerateLoginToken(string userName, string password) {
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null) {
            _authenticatedUser = await _authenticationService.RegisterAsync(userName, password);
        }
        return await _authenticationService.LoginAsync(userName, password);
    }

    protected StringContent GenerateBodyRequest<T>(T dto) {
        return new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
    }
}