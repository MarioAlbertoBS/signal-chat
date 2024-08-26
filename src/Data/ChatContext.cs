namespace Chat.Data;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Chat.Models;

public class ChatContext : IdentityDbContext<User>
{
    public ChatContext(DbContextOptions<ChatContext> options) : base(options)
    {   
    }

    public DbSet<Room> Rooms { get; set; }
    public DbSet<Participant> Participants { get; set;}
    public DbSet<Message> Messages { get; set;}
}