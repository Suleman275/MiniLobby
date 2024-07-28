using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniLobby.Data;
using MiniLobby.Dtos;
using MiniLobby.Models;

namespace MiniLobby.Controllers {
    [Route("api/lobbies")]
    [ApiController]
    public class LobbiesController_Members : ControllerBase {
        private readonly ApplicationDbContext _context;

        public LobbiesController_Members(ApplicationDbContext context) {
            _context = context;
        }

        [HttpGet("{Id:guid}/members")]
        public async Task<IActionResult> GetLobbyMembers([FromRoute] Guid Id) {
            if (!_context.Lobbies.Any(l => l.Id == Id)) {
                return NotFound();
            }

            var lobbyMembers = await _context.LobbyMembers.Where(m => m.CurrentLobbyId == Id).ToListAsync();

            //return Ok(lobbyMembers);
            return Ok(lobbyMembers.Select(m => new LobbyMemberResponseDto(m)));
        }

        [HttpPost("{Id:guid}/join")]
        public async Task<IActionResult> JoinLobby([FromRoute] Guid Id, [FromBody] JoinLobbyRequestDto requestDto) { //Todo : join with data
            if (!ModelState.IsValid) {
                return BadRequest("Invalid request data");
            }

            var lobby = await _context.Lobbies.FindAsync(Id);
            if (lobby == null) {
                return NotFound("Lobby not found");
            }

            // Check if the member limit has been reached
            var currentMemberCount = await _context.LobbyMembers.CountAsync(m => m.CurrentLobbyId == Id);
            if (currentMemberCount >= lobby.MemberLimit) {
                return BadRequest("Lobby member limit reached. Cannot Join Lobby");
            }

            // Check if the user is already a member of the lobby
            var existingMember = await _context.LobbyMembers
                .FirstOrDefaultAsync(m => m.CurrentLobbyId == Id && m.Id == requestDto.RequestSenderId);
            if (existingMember != null) {
                return BadRequest("User is already a member of the lobby");
            }

            // Add the new member to the lobby
            var newMember = new LobbyMember {
                Id = requestDto.RequestSenderId,
                CurrentLobbyId = Id
            };

            await _context.LobbyMembers.AddAsync(newMember);
            await _context.SaveChangesAsync();

            return Ok("Joined Lobby Successfully");
        }

        [HttpPost("{Id:guid}/leave")]
        public async Task<IActionResult> LeaveLobby([FromRoute] Guid Id, [FromBody] LeaveLobbyRequestDto requestDto) {
            if (!ModelState.IsValid) {
                return BadRequest("Invalid request data");
            }

            var lobby = await _context.Lobbies.FindAsync(Id);
            if (lobby == null) {
                return NotFound("Lobby not found");
            }

            // Check if the user is a member of the lobby
            var existingMember = await _context.LobbyMembers
                .FirstOrDefaultAsync(m => m.CurrentLobbyId == Id && m.Id == requestDto.RequestSenderId);
            if (existingMember == null) {
                return BadRequest("User is not a member of the lobby");
            }

            // Remove the member from the lobby
            _context.LobbyMembers.Remove(existingMember);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
