using System.Security.Claims;
using Chat.Data.Dtos;
using Chat.Data.Repositories;
using Chat.Models;
using Chat.Services.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Controllers;

[ApiController]
[Route("api/messages")]
public class MessageController : ControllerBase
{
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IMessageService _messageService;
    private readonly UserManager<User> _userManager;
    private readonly RoomRepository _roomRepository;
    private readonly MessageRepository _messageRepository;
    private const string MessageEvent = "ReceiveMessage";

    public MessageController(IHubContext<ChatHub> hubContext, IMessageService messageService, UserManager<User> userManager, RoomRepository roomRepository, MessageRepository messageRepository) {
        _hubContext = hubContext;
        _messageService = messageService;
        _userManager = userManager;
        _roomRepository = roomRepository;
        _messageRepository = messageRepository;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> SendMessage([FromBody] MessageRequestDto messageRequestDto) {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null) {
            return NotFound(new {message = "User Not Found"});
        }
        
        var user = await _userManager.FindByIdAsync(userId);

        var room = await _roomRepository.Get(messageRequestDto.RoomId.ToString());

        if (room == null) {
            return NotFound(new {message = "Room Not Found"});
        }

        try {
            var message = await _messageRepository.Create(user, room, messageRequestDto.Message);
            if (message == null) {
                throw new Exception("Cannot store message in database");
            }

            await _messageService.SendMessageAsync(message.Room.Id.ToString(), message.Body);
        } catch(Exception ex) {
            Console.WriteLine(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, "Error sending the message.");
        }

        return Ok(new {message = "Message sent."});
    }
}