using Chat.Models;
using Microsoft.EntityFrameworkCore;

namespace Chat.Data;

public class Seeder
{
    public async static Task Seed(ChatContext context) {
        // Create default room if not exists
        bool defaultRoomExists = await context.Rooms.Where(room => room.Name == "Default").AnyAsync();

        if (!defaultRoomExists) {
            Room room = new Room {
                Id = "Default",
                Name = "Default"
            };

            context.Add(room);

            try {
                await context.SaveChangesAsync();
            } catch (Exception ex) {
                Console.WriteLine("Cannot create Default Room", ex.Message);
            }

            Console.WriteLine("Created Default Room.");
        }

        Console.WriteLine("Seeder finished.");
    }
}