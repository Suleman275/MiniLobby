using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniLobby.Data;
using MiniLobby.Dtos;
using MiniLobby.Enums;
using MiniLobby.Models;

namespace MiniLobby.Controllers {
    [Route("api/lobbies")]
    [ApiController]
    public class MemberDataController : ControllerBase {
        private readonly ApplicationDbContext _context;

        public MemberDataController (ApplicationDbContext context) {
            _context = context;
        }

        [HttpGet("{Id:guid}/members/{memberId:guid}/data")]
        public async Task<IActionResult> GetMemberData([FromRoute] Guid Id, [FromRoute] Guid memberId, [FromBody] BaseRequestDto requestDto) {
            if (!_context.Lobbies.Any(l => l.Id == Id)) {
                return NotFound();
            }

            if (!_context.LobbyMembers.Any(m => m.CurrentLobbyId == Id && m.MemberId == memberId)) {
                return BadRequest("This member is not a part of this lobby");
            }

            List<MemberData> memberData;

            if (requestDto.RequestSenderId == memberId) { //owner of data -> show all 
                memberData = await _context.MemberData.Where(md => md.MemberId == memberId).ToListAsync();
            }
            else if (_context.LobbyMembers.Any(m => m.CurrentLobbyId == Id && m.MemberId == memberId)) { //another lobby member requesting -> hide private data
                memberData = await _context.MemberData.Where(md => md.MemberId == memberId).Where(d => d.Visibility != VisibilityOptions.Private).ToListAsync();
            }
            else { //outsider requesting -> show only public data
                memberData = await _context.MemberData.Where(md => md.MemberId == memberId).Where(d => d.Visibility == VisibilityOptions.Public).ToListAsync();
            }

            return Ok(memberData);
        }

        [HttpPut("{Id:guid}/members/{memberId:guid}/data")]
        public async Task<IActionResult> UpsertMemberData([FromRoute] Guid Id, [FromRoute] Guid memberId, [FromBody] UpdateMemberDataRequestDto requestDto) {
            if (!ModelState.IsValid) {
                return BadRequest("Invalid request data");
            }

            var lobby = await _context.Lobbies.FindAsync(Id);
            if (lobby == null) {
                return NotFound("Lobby not found");
            }

            if (!_context.LobbyMembers.Any(m => m.CurrentLobbyId == Id && m.MemberId == memberId)) {
                return BadRequest("This member is not a part of this lobby");
            }

            if (requestDto.RequestSenderId != memberId) {
                return Forbid("Only the owner of the data can update it");
            }

            var existingData = await _context.MemberData.Where(md => md.MemberId == memberId).ToListAsync();

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
                    _context.MemberData.Add(new MemberData {
                        MemberId = memberId,
                        Key = key,
                        Value = dataPoint.Value,
                        Visibility = dataPoint.Visibility
                    });
                }
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{Id:guid}/members/{memberId:guid}/data")]
        public async Task<IActionResult> DeleteMemberData([FromRoute] Guid Id, [FromRoute] Guid memberId, [FromBody] DeleteDataRequestDto requestDto) {
            if (!ModelState.IsValid) {
                return BadRequest("Invalid request data");
            }

            var lobby = await _context.Lobbies.FindAsync(Id);
            if (lobby == null) {
                return NotFound("Lobby not found");
            }

            if (!_context.LobbyMembers.Any(m => m.CurrentLobbyId == Id && m.MemberId == memberId)) {
                return BadRequest("This member is not a part of this lobby");
            }

            if (requestDto.RequestSenderId != memberId) {
                return Forbid("Only the owner of the data can delete it");
            }

            var existingData = await _context.MemberData.Where(md => md.MemberId == memberId && requestDto.Keys.Contains(md.Key)).ToListAsync();

            _context.MemberData.RemoveRange(existingData);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
