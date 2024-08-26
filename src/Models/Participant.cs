using System.ComponentModel.DataAnnotations;

namespace Chat.Models;

public class Participant
{
    [Key]
    public int Id { get; set; }

    [Required]
    public Room Room { get; set; } = new Room();

    [Required]
    public User User { get; set; } = new User();
}