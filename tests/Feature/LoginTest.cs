using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using Chat.Data.Dtos;
using Chat.Models;
using Chat.Services;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Xunit;

namespace Chat.Tests.Feature;

public class LoginTest : Test
{
    public LoginTest(TestApplicationFactory factory) : base(factory)
    {}

    [Theory]
    [InlineData("MarioB", "Password123@!")]
    public async Task TestRegisterUser(string userName, string password) {
        UserAuthenticationRequest userLoginDto = new UserAuthenticationRequest {
            UserName = userName,
            Password = password
        };

        StringContent content = new StringContent(JsonConvert.SerializeObject(userLoginDto), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _client.PostAsync("/api/register", content);

        response.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        
        Assert.NotNull(response.Content);

        string responseString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<UserRegisterResponseDto>(responseString);

        Assert.NotNull(responseObject);
        Assert.Equal(userLoginDto.UserName, responseObject.UserName);
        Assert.False(string.IsNullOrEmpty(responseObject.Id));
    }

    [Theory]
    [InlineData("MarioB", "Password123@!")]
    public async Task TestLoginUser(string userName, string password) {
        IAuthenticationService jwtService = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<IAuthenticationService>();
        User testUser = await jwtService.RegisterAsync(userName, password);
        
        Assert.NotNull(testUser);

        UserAuthenticationRequest userLoginDto = new UserAuthenticationRequest {
            UserName = userName,
            Password = password
        };

        StringContent content = new StringContent(JsonConvert.SerializeObject(userLoginDto), Encoding.UTF8, "application/json");
        
        HttpResponseMessage response = await _client.PostAsync("/api/login", content);

        response.EnsureSuccessStatusCode();
    }

    [Theory]
    [InlineData("MarioB", "Password123@!")]
    public async Task HelloWorkAuthenticated(string userName, string password) {
        // Create user
        User testUser = await GenerateUser(userName, password);
        Assert.NotNull(testUser);

        // Login User to generate token
        string token = await GenerateLoginToken(userName, password);
        Assert.NotEmpty(token);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        HttpResponseMessage response = await _client.GetAsync("/api/hello-world");
        response.EnsureSuccessStatusCode();
    }
}