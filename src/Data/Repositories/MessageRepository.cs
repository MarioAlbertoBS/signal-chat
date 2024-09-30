using Chat.Models;

namespace Chat.Data.Repositories;

public class MessageRepository
{
    private readonly ChatContext _context;

    public MessageRepository(ChatContext context) {
        _context = context;
    }

    public async Task<Message?> Create(User user, Room room, string messageBody) {
        Message message = new Message {
            User = user,
            Room = room,
            Body = messageBody
        };

        _context.Add(message);

        if (await _context.SaveChangesAsync() == 0) {
            throw new Exception("Message cannot be created");
        }

        return message;
    }
}