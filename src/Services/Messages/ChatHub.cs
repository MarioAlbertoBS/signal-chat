using System.Reflection.Metadata;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Services.Messages;

public class ChatHub : Hub
{
    public const string MessageEvent = "ReceiveMessage";

    public Task JoinRoom(string roomName)
    {
        return Groups.AddToGroupAsync(Context.ConnectionId, roomName);
    }

    public Task LeaveRoom(string roomName) {
        return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
    }

    public Task SendMessage(string roomName, string message) {
        return Clients.Group(roomName).SendAsync("ReceiveMessage", message);
    }
}