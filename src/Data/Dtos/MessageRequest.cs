namespace Chat.Data.Dtos;

public class MessageRequestDto {
    public string Message { get; set; }
    public string RoomId { get; set; }
}

public class MessageResponseDto {
    public int Id { get; set; }
    public string User { get; set;}
    public string CreatedAt { get; set; }
    public string Message { get; set;}
}