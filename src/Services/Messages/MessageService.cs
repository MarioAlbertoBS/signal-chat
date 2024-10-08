
using Chat.Data.Dtos;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Services.Messages;

public class MessageService : IMessageService
{
    private readonly IHubContext<ChatHub> _hubContext;

    public MessageService(IHubContext<ChatHub> hubContext) {
        _hubContext = hubContext;
    }

    public async Task JoinRoomAsync(string roomName)
    {
        throw new NotImplementedException();
    }

    public Task LeaveRoomAsync(string roomName)
    {
        throw new NotImplementedException();
    }

    public async Task SendMessageAsync(string roomName, MessageResponseDto message)
    {
        await _hubContext.Clients.Group(roomName).SendAsync(ChatHub.MessageEvent, message);
    }
}