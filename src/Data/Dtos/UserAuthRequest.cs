namespace Chat.Data.Dtos;

public class UserAuthenticationRequest
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
}

public class UserRegisterResponseDto
{
    public required string Id { get; set; }
    public required string UserName { get; set; }
}