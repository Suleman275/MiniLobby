using Microsoft.AspNetCore.Mvc;
using MiniLobby.Data;
using MiniLobby.Dtos;
using MiniLobby.Enums;
using MiniLobby.Interfaces;
using MiniLobby.Models;

namespace MiniLobby.Controllers {
    [Route("api/lobbies")]
    [ApiController]
    public class MemberDataController : ControllerBase {
        private readonly IMembersRepository _membersRepo;
        private readonly IMemberDataRepository _memberDataRepo;
        private readonly ILobbyRepository _lobbyRepo;

        public MemberDataController (ApplicationDbContext context, ILobbyRepository lobbyRepository, IMembersRepository membersRepository, IMemberDataRepository memberDataRepository) {
            _lobbyRepo = lobbyRepository;
            _membersRepo = membersRepository;
            _memberDataRepo = memberDataRepository;
        }

        [HttpGet("{Id:guid}/members/{memberId:guid}/data")]
        public async Task<IActionResult> GetMemberData([FromRoute] Guid Id, [FromRoute] Guid memberId, [FromBody] BaseRequestDto requestDto) {
            if (! await _lobbyRepo.DoesLobbyExist(Id)) {
                return NotFound();
            }

            if (!await _membersRepo.IsMemberOfThisLobby(Id, memberId)) {
                return BadRequest("This member is not a part of this lobby");
            }

            List<MemberData> memberData;

            if (requestDto.RequestSenderId == memberId) { //owner of data -> show all 
                memberData = await _memberDataRepo.GetMemberData(memberId, DataFilterOptions.Owner);
            }
            else if (await _membersRepo.IsMemberOfThisLobby(Id, requestDto.RequestSenderId)) { //another lobby member requesting -> hide private data
                memberData = await _memberDataRepo.GetMemberData(memberId, DataFilterOptions.Member);
            }
            else { //outsider requesting -> show only public data
                memberData = await _memberDataRepo.GetMemberData(memberId, DataFilterOptions.Outsider);
            }

            return Ok(new DataResponseDto_Dictionary(memberData));
        }

        [HttpPut("{Id:guid}/members/{memberId:guid}/data")]
        public async Task<IActionResult> UpsertMemberData([FromRoute] Guid Id, [FromRoute] Guid memberId, [FromBody] UpdateDataRequestDto requestDto) {
            if (!ModelState.IsValid) {
                return BadRequest("Invalid request data");
            }

            if (! await _lobbyRepo.DoesLobbyExist(Id)) {
                return NotFound();
            }

            if (!await _membersRepo.IsMemberOfThisLobby(Id, memberId)) {
                return BadRequest("This member is not a part of this lobby");
            }

            if (requestDto.RequestSenderId != memberId) {
                return Unauthorized("Only the owner of the data can update it");
            }

            await _memberDataRepo.UpdateMemberData(memberId, requestDto.Data);

            return NoContent();
        }

        [HttpDelete("{Id:guid}/members/{memberId:guid}/data")]
        public async Task<IActionResult> DeleteMemberData([FromRoute] Guid Id, [FromRoute] Guid memberId, [FromBody] DeleteDataRequestDto requestDto) {
            if (!ModelState.IsValid) {
                return BadRequest("Invalid request data");
            }

            if (! await _lobbyRepo.DoesLobbyExist(Id)) {
                return NotFound("Lobby not found");
            }

            if (! await _membersRepo.IsMemberOfThisLobby(Id, memberId)) {
                return BadRequest("This member is not a part of this lobby");
            }

            if (requestDto.RequestSenderId != memberId) {
                return Forbid("Only the owner of the data can delete it");
            }

            await _memberDataRepo.DeteleMemberData(memberId, requestDto.Keys);

            return NoContent();
        }
    }
}
