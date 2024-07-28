using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniLobby.Data;
using MiniLobby.Dtos;
using MiniLobby.Enums;
using MiniLobby.Models;

namespace MiniLobby.Controllers {
    [Route("api/lobbies")]
    [ApiController]
    public class LobbyDataController : ControllerBase {
        private readonly ApplicationDbContext _context;

        public LobbyDataController(ApplicationDbContext context) {
            _context = context;
        }

        [HttpGet("{Id:guid}/data")]
        public async Task<IActionResult> GetLobbyData([FromRoute] Guid Id, [FromBody] GetLobbyDataRequestDto requestDto) {
            var lobby = await _context.Lobbies.FindAsync(Id);

            if (lobby == null) {
                return NotFound();
            }

            var lobbyMembers = await _context.LobbyMembers.Where(m => m.CurrentLobbyId == Id).ToListAsync();

            List<LobbyData> lobbyData;

            if (lobby.HostId == requestDto.RequestSenderId) { //is lobby host -> show all data
                lobbyData = await _context.LobbyData.Where(d => d.LobbyId == Id).ToListAsync();
            }
            else if (lobbyMembers.Any(m => m.MemberId == requestDto.RequestSenderId)) { //is member but not host -> dont show private data
                lobbyData = await _context.LobbyData.Where(d => d.LobbyId == Id).Where(d => d.Visibility != VisibilityOptions.Private).ToListAsync();
            }
            else { //is outsider -> show only public data 
                lobbyData = await _context.LobbyData.Where(d => d.LobbyId == Id).Where(d => d.Visibility == VisibilityOptions.Public).ToListAsync();
            }

            //return Ok(lobbyData); 
            //return Ok(lobbyData.Select(d => new LobbyDataResponseDto(d)));
            
            var lobbyDataResponse = new LobbyDataResponseDto_Dictionary(lobbyData);
            return Ok(lobbyDataResponse);
        }


        [HttpPut("{Id:guid}/data")]
        public async Task<IActionResult> UpseretLobbyData([FromRoute] Guid Id, [FromBody] UpdateDataRequestDto requestDto) {
            if (!ModelState.IsValid) {
                return BadRequest("Invalid request data");
            }

            var lobby = await _context.Lobbies.FindAsync(Id);
            if (lobby == null) {
                return NotFound();
            }

            // Check if the request sender is the host
            if (lobby.HostId != requestDto.RequestSenderId) {
                return Forbid("Only the lobby host can update data");
            }

            var existingData = await _context.LobbyData.Where(d => d.LobbyId == Id).ToListAsync();

            foreach (var key in requestDto.Data.Keys) {
                var dataPoint = requestDto.Data[key];
                var existingDataPoint = existingData.FirstOrDefault(d => d.Key == key);

                if (existingDataPoint != null) {
                    // Update existing data point
                    existingDataPoint.Value = dataPoint.Value;
                    existingDataPoint.Visibility = dataPoint.Visibility;
                }
                else {
                    // Add new data point
                    _context.LobbyData.Add(new LobbyData {
                        LobbyId = Id,
                        Key = key,
                        Value = dataPoint.Value,
                        Visibility = dataPoint.Visibility
                    });
                }
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{Id:guid}/data")]
        public async Task<IActionResult> DeleteLobbyData([FromRoute] Guid Id, [FromBody] DeleteDataRequestDto requestDto) {
            if (!ModelState.IsValid) {
                return BadRequest("Invalid request data");
            }

            var lobby = await _context.Lobbies.FindAsync(Id);
            if (lobby == null) {
                return NotFound();
            }

            // Check if the request sender is the host
            if (lobby.HostId != requestDto.RequestSenderId) {
                return Forbid("Only the lobby host can delete data");
            }

            var dataPoints = await _context.LobbyData
                .Where(d => d.LobbyId == Id && requestDto.Keys.Contains(d.Key))
                .ToListAsync();

            if (!dataPoints.Any()) {
                return NotFound("No data points found");
            }

            _context.LobbyData.RemoveRange(dataPoints);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
