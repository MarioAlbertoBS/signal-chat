using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using Chat.Data;
using Chat.Data.Dtos;
using Chat.Models;
using Chat.Tests.Feature;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

        RoomCreateRequestDto requestDto = new RoomCreateRequestDto {
            Name = roomName,
        };

        var body = GenerateBodyRequest(requestDto);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.PostAsync("/api/rooms", body);

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var responseData = await GetResponseContent<RoomCreateResponseDto>(response);

        Assert.Equal(responseData.Name, roomName);
    }

    [Theory]
    [InlineData("Test")]
    public async Task JoinRoom(string roomName) {
        AuthenticateAs("Mario", "Password123!");

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
        string url = $"/api/rooms/{room.Id}/join";
        var response = await _client.PostAsync($"/api/rooms/{room.Name}/join", body);

        response.EnsureSuccessStatusCode();

        var responseData = await GetResponseContent<JoinRoomResponseDto>(response);

        Assert.True(responseData.Status);
        Assert.Equal(room.Id, responseData.RoomId);
        Assert.Equal(_authenticatedUser.Id, responseData.UserId);
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

    [Theory]
    [InlineData("Default")]
    public async Task GetRoomMessages(string roomName) {
        AuthenticateAs("MarioB", "Password123@!");

        Room room = new Room {
            Name = roomName
        };

        room.Messages = new List<Message> {
            new Message {Body = "Mensaje1", User = _authenticatedUser},
            new Message {Body = "Mensaje2", User = _authenticatedUser},
            new Message {Body = "Mensaje3", User = _authenticatedUser},
        };

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();

        var response = await _client.GetAsync($"/api/rooms/{roomName}/messages");
        response.EnsureSuccessStatusCode();

        var responseData = await GetResponseContent<RoomMessagesResponseDto>(response);

        Assert.NotEmpty(responseData.Messages);
    }
}