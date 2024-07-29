using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniLobby.Data;
using MiniLobby.Dtos;
using MiniLobby.Interfaces;
using MiniLobby.Models;

namespace MiniLobby.Controllers {
    [Route("api/lobbies")]
    [ApiController]
    public class MembersController : ControllerBase {
        private readonly IMembersRepository _membersRepo;
        private readonly IMemberDataRepository _memberDataRepo;
        private readonly ILobbyRepository _lobbyRepo;

        public MembersController(IMembersRepository membersRepository, ILobbyRepository lobbyRepository, IMemberDataRepository memberDataRepository) {
            _membersRepo = membersRepository;
            _lobbyRepo = lobbyRepository;
            _memberDataRepo = memberDataRepository;
        }

        [HttpGet("{Id:guid}/members")]
        public async Task<IActionResult> GetLobbyMembers([FromRoute] Guid Id) {
            if (!await _lobbyRepo.DoesLobbyExist(Id)) {
                return NotFound();
            }

            var lobbyMembers = await _membersRepo.GetLobbyMembers(Id);

            //return Ok(lobbyMembers);
            return Ok(lobbyMembers.Select(m => new LobbyMemberResponseDto(m)));
        }

        [HttpPost("{Id:guid}/join")]
        public async Task<IActionResult> JoinLobby([FromRoute] Guid Id, [FromBody] BaseRequestDto requestDto) { //Todo : join with data
            if (!ModelState.IsValid) {
                return BadRequest("Invalid request data");
            }

            var lobby = await _lobbyRepo.GetById(Id);
            if (lobby == null) {
                return NotFound("Lobby not found");
            }

            // Check if the member limit has been reached
            var currentMemberCount = await _membersRepo.GetLobbyMemberCount(Id);
            if (currentMemberCount >= lobby.MemberLimit) {
                return BadRequest("Lobby member limit reached. Cannot Join Lobby");
            }

            // Check if the user is already a member of the lobby
            var existingMember = await _membersRepo.GetLobbyMember(Id, requestDto.RequestSenderId);
            if (existingMember != null) {
                return BadRequest("User is already a member of the lobby");
            }

            // Check if the user is a member of another lobby
            var otherLobbyMembership = await _membersRepo.IsMemberOfOtherLobby(Id, requestDto.RequestSenderId);
            if (otherLobbyMembership) {
                return BadRequest("User is already a member of another lobby");
            }

            // Add the new member to the lobby
            await _membersRepo.AddMemberToLobby(Id, requestDto.RequestSenderId);

            return Ok("Joined Lobby Successfully");
        }

        [HttpPost("{Id:guid}/leave")]
        public async Task<IActionResult> LeaveLobby([FromRoute] Guid Id, [FromBody] BaseRequestDto requestDto) { //todo: check if is lobby host
            if (!ModelState.IsValid) {
                return BadRequest("Invalid request data");
            }

            // Check if the lobby exists
            //var lobby = await _context.Lobbies.FindAsync(Id);
            //if (lobby == null) {
            //    return NotFound("Lobby not found");
            //}

            if (!await _lobbyRepo.DoesLobbyExist(Id)) {
                return NotFound("Lobby not found");
            }

            // Check if the user is a member of the lobby
            var existingMember = await _membersRepo.GetLobbyMember(Id, requestDto.RequestSenderId);
            if (existingMember == null) {
                return BadRequest("User is not a member of the lobby");
            }

            await _memberDataRepo.DeleteAllMemberData(requestDto.RequestSenderId);

            await _membersRepo.RemoveMemberFromLobby(Id, requestDto.RequestSenderId);

            return NoContent();
        }
    }
}
