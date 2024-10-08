using Chat.Data.Dtos;

namespace Chat.Services.Messages;

public interface IMessageService
{
    Task JoinRoomAsync(string roomName);
    Task LeaveRoomAsync(string roomName);
    Task SendMessageAsync(string roomName, MessageResponseDto message);
}