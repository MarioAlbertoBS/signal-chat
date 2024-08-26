using System.Net.Http.Headers;
using System.Text;
using Chat.Data.Dtos;
using Chat.Models;
using Newtonsoft.Json;
using Xunit;

namespace Chat.Tests.Feature;

public class MessageTest : Test
{
    public MessageTest(TestApplicationFactory factory) : base(factory) {}

    [Theory]
    [InlineData("MarioB", "Password123!", "Hello World!", 1)]
    public async Task TestSendMessage(string userName, string password, string message, int roomId) {
        string token = await GenerateLoginToken(userName, password);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        MessageRequestDto messageRequestDto = new MessageRequestDto{
            Message = message,
            RoomId = roomId
        };

        StringContent content = GenerateBodyRequest(messageRequestDto);

        HttpResponseMessage response = await _client.PostAsync("api/messages", content);

        response.EnsureSuccessStatusCode();
    }
}