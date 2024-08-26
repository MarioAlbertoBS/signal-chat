using System.ComponentModel.DataAnnotations;

namespace Chat.Models;

public class Room
{
    [Key]
    public string Id { get; set; } = String.Empty;
    
    [Required]
    public string Name { get; set; } = String.Empty;

    public IEnumerable<Participant> ?Participants { get; set; }

    public IEnumerable<Message> ?Messages { get; set; }
}