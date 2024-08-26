using Microsoft.AspNetCore.SignalR;

namespace Chat.Services.Messages;

public class ChatHub : Hub
{
    public async Task JoinRoom(string roomName) {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
    }

    public async Task LeaveRoom(string roomName) {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
    }

    public async Task SendMessageToRoom(string roomName, string message) {
        await Clients.Group(roomName).SendAsync("ReceiveMessage", message);
    }
}