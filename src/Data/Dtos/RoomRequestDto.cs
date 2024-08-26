namespace Chat.Data.Dtos;

public class RoomRequestDto
{
    public string Name { get; set; } = String.Empty;
}

public class JoinRoomRequestDto
{
    public string Name { get; set; } = String.Empty;
    public string UserId { get; set; } = String.Empty;
}