using System.Net;
using System.Net.Http.Headers;
using Chat.Data.Dtos;
using Chat.Models;
using Chat.Tests.Feature;
using Xunit;

namespace Chat.Tests.Feature;

public class RoomTest : Test
{
    public RoomTest(TestApplicationFactory factory) : base(factory)
    {
    }

    [Theory]
    [InlineData("Test")]
    public async Task CreateRoom(string roomName) {
        string token = await GenerateLoginToken("User", "Password123!");

        RoomRequestDto requestDto = new RoomRequestDto {
            Name = roomName,
        };

        var body = GenerateBodyRequest(requestDto);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.PostAsync("/api/rooms", body);

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Theory]
    [InlineData("Test")]
    public async Task JoinRoom(string roomName) {
        string token = await GenerateLoginToken("User", "Password123!");

        Room room = new Room {
            Name = roomName
        };

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();

        JoinRoomRequestDto requestDto = new JoinRoomRequestDto {
            Name = roomName,
            UserId = _authenticatedUser.Id
        };
        var body = GenerateBodyRequest(requestDto);
        var response = await _client.PostAsync($"/api/rooms/{room.Id}/join", body);

        response.EnsureSuccessStatusCode();
    }

    [Theory]
    [InlineData("Test")]
    public async Task LeaveRoom(string roomName) {
        string token = await GenerateLoginToken("User", "Password123!");

        Room room = new Room {
            Name = roomName
        };

        Participant participant = new Participant {
            Id = 1,
            User = _authenticatedUser,
            Room = room
        };

        _context.Rooms.Add(room);
        _context.Participants.Add(participant);
        await _context.SaveChangesAsync();

        JoinRoomRequestDto requestDto = new JoinRoomRequestDto {
            Name = roomName,
            UserId = _authenticatedUser.Id
        };
        var body = GenerateBodyRequest(requestDto);
        var response = await _client.PostAsync($"/api/rooms/{room.Id}/leave", body);

        response.EnsureSuccessStatusCode();
    }
}