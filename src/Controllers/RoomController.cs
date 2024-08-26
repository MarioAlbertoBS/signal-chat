using Chat.Services.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Controllers;

[ApiController]
[Route("api/rooms")]
public class RoomController : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create() {
        return BadRequest();
    }

    [HttpPost("{roomId}/join")]
    [Authorize]
    public async Task<IActionResult> JoinRoom(string roomId) {
        return BadRequest();
    }

    [HttpPost("{roomId}/leave")]
    [Authorize]
    public async Task<IActionResult> LeaveRoom(string roomId) {
        return BadRequest();
    }
}