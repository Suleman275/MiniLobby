using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniLobby.Data;
using MiniLobby.Dtos;
using MiniLobby.Models;

namespace MiniLobby.Controllers {
    [Route("api/lobbies")]
    [ApiController]
    public class LobbiesController : ControllerBase {
        private readonly ApplicationDbContext _context;

        public LobbiesController(ApplicationDbContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetPublicLobbies() {
            var lobbies = await _context.Lobbies.Where(l => l.IsPrivate == false).ToListAsync();

            return Ok(lobbies);
        }

        [HttpGet("{Id:guid}")]
        public async Task<IActionResult> GetLobbyById([FromRoute] Guid Id) {
            var lobby = await _context.Lobbies.FindAsync(Id);

            if (lobby == null) { 
                return NotFound();
            }

            return Ok(lobby);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLobby([FromBody] CreateLobbyRequestDto requestDto) { //Todo : Create with data
            if (!ModelState.IsValid) {
                return BadRequest("One or more missing parameters");
            }

            var lobby = new Lobby {
                Id = Guid.NewGuid(),
                HostId = requestDto.RequestSenderId,
                Name = requestDto.LobbyName,
                MemberLimit = requestDto.MemberLimit,
                IsPrivate = requestDto.IsPrivate,
            };

            await _context.Lobbies.AddAsync(lobby);
            await _context.LobbyMembers.AddAsync(new LobbyMember {CurrentLobbyId = lobby.Id, Id = requestDto.RequestSenderId});

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLobbyById), new { id = lobby.Id}, lobby);
        }

        [HttpDelete("{Id:guid}")]
        public async Task<IActionResult> DeleteLobby([FromRoute] Guid Id, [FromBody] DeleteLobbyRequestDto requestDto) {
            var lobby = await _context.Lobbies.FindAsync(Id);

            if (lobby == null) {
                return NotFound();
            }

            if (requestDto.RequestSenderId != lobby.HostId) { 
                return Forbid("Only lobby host can delete lobby"); //what is the difference between forbid and unauthorized?
            }

            _context.Lobbies.Remove(lobby);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
