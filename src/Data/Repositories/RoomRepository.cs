using Chat.Data.Dtos;
using Chat.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Chat.Data.Repositories;

public class RoomRepository
{
    private readonly ChatContext _context;
    private readonly UserManager<User> _userManager;

    public RoomRepository(ChatContext context, UserManager<User> userManager) {
        _context = context;
        _userManager = userManager;
    }

    public async Task<Room?> Get(string id) {
        return await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Room?> GetByName(string name) {
        return await _context.Rooms.FirstOrDefaultAsync(r => r.Name == name);
    }

    public async Task<Room> Create(string name) {
        Room room = new Room {
            Name = name
        };

        _context.Add(room);
        if (await _context.SaveChangesAsync() == 0) {
            throw new Exception("Room cannot be created");
        }

        return room;
    }

    public async Task<bool> JoinRoom(User user, Room room) {
        Participant participant = new Participant {
            Room = room,
            User = user
        };

        _context.Participants.Add(participant);

        if (await _context.SaveChangesAsync() == 0) {
            throw new Exception("Cannot join to Room");
        }

        return true;
    }

    public async Task<List<MessageResponseDto>> GetMessages(Room room) {
        var messages = _context.Messages
            .Where(message => message.Room == room)
            .Select(message => new MessageResponseDto {
                Id = message.Id,
                User = message.User.UserName,
                CreatedAt = message.SentAt.ToString("yyyy-MM-dd HH:mm:ss"),
                Message = message.Body
            })
            .ToList();

        return messages;
    }
}