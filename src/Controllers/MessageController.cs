using System.Security.Claims;
using Chat.Data.Dtos;
using Chat.Services.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Controllers;

[ApiController]
[Route("api/messages")]
public class MessageController : ControllerBase
{
    private readonly IHubContext<ChatHub> _hubContext;

    public MessageController(IHubContext<ChatHub> hubContext) {
        _hubContext = hubContext;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> SendMessage([FromBody] MessageRequestDto messageRequestDto) {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null) {
            return BadRequest("User Not Found");
        }

        await _hubContext.Clients.Group(messageRequestDto.RoomId.ToString()).SendAsync("ReceiveMessage", messageRequestDto.Message);

        return Ok();
    }
}