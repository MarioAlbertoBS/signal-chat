using System.ComponentModel.DataAnnotations;

namespace Chat.Models;

public class Message
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public Room Room { get; set; } = new Room();
    
    [Required]
    public User User { get; set; } = new User();

    [Required]
    public string Body { get; set; } = String.Empty;

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime SentAt { get; set; }
}