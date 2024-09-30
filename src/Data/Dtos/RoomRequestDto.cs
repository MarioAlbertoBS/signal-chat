namespace Chat.Data.Dtos;

public class RoomCreateRequestDto
{
    public string Name { get; set; } = String.Empty;
}

public class RoomCreateResponseDto
{
    public string Id { get; set; } = String.Empty;
    public string Name { get; set; } = String.Empty;
}

public class JoinRoomRequestDto
{
    public string Name { get; set; } = String.Empty;
    public string UserId { get; set; } = String.Empty;
}

public class JoinRoomResponseDto
{
    public bool Status { get; set; } = false;
    public string RoomId { get; set; } = String.Empty;
    public string UserId { get; set; } = String.Empty;
}