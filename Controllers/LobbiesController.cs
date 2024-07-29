using Microsoft.AspNetCore.Mvc;
using MiniLobby.Dtos;
using MiniLobby.Interfaces;
using MiniLobby.Mappers;

namespace MiniLobby.Controllers {
    [Route("api/lobbies")]
    [ApiController]
    public class LobbiesController : ControllerBase {
        private readonly ILobbyRepository _lobbyRepo;

        public LobbiesController(ILobbyRepository lobbyRepo) {
            _lobbyRepo = lobbyRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetPublicLobbies() {
            var lobbies = await _lobbyRepo.GetPublicLobbies();
            return Ok(lobbies.Select(l => new LobbyResponseDto(l)));
        }

        [HttpGet("{Id:guid}")]
        public async Task<IActionResult> GetLobbyById([FromRoute] Guid Id) {
            var lobby = await _lobbyRepo.GetById(Id);

            if (lobby == null) { 
                return NotFound();
            }

            return Ok(lobby.ToLobbyResponseDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateLobby([FromBody] CreateLobbyRequestDto requestDto) { //Todo : Create with data
            if (!ModelState.IsValid) {
                return BadRequest("One or more missing parameters");
            }

            var lobby = await _lobbyRepo.CreateLobby(requestDto);

            return CreatedAtAction(nameof(GetLobbyById), new { id = lobby.Id }, lobby.ToLobbyResponseDto());
        }

        [HttpDelete("{Id:guid}")]
        public async Task<IActionResult> DeleteLobby([FromRoute] Guid Id, [FromBody] BaseRequestDto requestDto) {
            var lobby = await _lobbyRepo.GetById(Id);

            if (lobby == null) {
                return NotFound();
            }

            if (requestDto.RequestSenderId != lobby.HostId) { 
                return Forbid("Only lobby host can delete lobby"); //what is the difference between forbid and unauthorized?
            }

            await _lobbyRepo.DeleteLobby(lobby);

            return NoContent();
        }
    }
}
