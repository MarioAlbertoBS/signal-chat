using System.Security.Claims;
using Chat.Data.Dtos;
using Chat.Data.Repositories;
using Chat.Models;
using Chat.Services.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Controllers;

[ApiController]
[Route("api/rooms")]
public class RoomController : ControllerBase
{
    private readonly RoomRepository _roomRepository;
    private readonly UserManager<User> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RoomController(RoomRepository roomRepository, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor) {
        _roomRepository = roomRepository;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(RoomCreateRequestDto roomCreateRequestDto) {
        try {
            var room = await _roomRepository.Create(roomCreateRequestDto.Name);

            if (room == null) {
                return BadRequest(new { Message = "Cannot Create a Room" });
            }

            RoomCreateResponseDto response = new RoomCreateResponseDto {
                Id = room.Id,
                Name = room.Name
            };

            return CreatedAtAction(nameof(Create), response);
        } catch (Exception ex) {
            return BadRequest(new { Message = "Cannot Create a Room" });
        }
    }

    [HttpPost("{roomName}/join")]
    [Authorize]
    public async Task<IActionResult> JoinRoom(string roomName) {
        // Search the room exists
        var room = await _roomRepository.GetByName(roomName);
        if (room == null) {
            return NotFound(new { Message = "The Room does not exists" });
        }

        // Get authenticated user
        var user = await _userManager.GetUserAsync(User);
        if (user == null) {
            return BadRequest(new { Message = "User cannot be identified" });
        }
        
        try {
            if (!await _roomRepository.JoinRoom(user, room)) {
                return Ok(new JoinRoomResponseDto {
                    RoomId = room.Id,
                    UserId = user.Id,
                    Status = false
                });
            }
        } catch (Exception ex) {
            // Join the room
            return BadRequest(new { Message = "Cannot join to Room" });
        }
        
        return Ok(new JoinRoomResponseDto {
            RoomId = room.Id,
            UserId = user.Id,
            Status = true
        });
    }

    [HttpPost("{roomName}/leave")]
    [Authorize]
    public async Task<IActionResult> LeaveRoom(string roomName) {
        return BadRequest();
    }
}