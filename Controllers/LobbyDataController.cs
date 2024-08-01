using Microsoft.AspNetCore.Mvc;
using MiniLobby.Dtos;
using MiniLobby.Enums;
using MiniLobby.Interfaces;
using MiniLobby.Models;

namespace MiniLobby.Controllers {
    [Route("api/lobbies")]
    [ApiController]
    public class LobbyDataController : ControllerBase {
        private readonly ILobbyRepository _lobbyRepo;
        private readonly IMembersRepository _membersRepo;
        private readonly ILobbyDataRepository _lobbyDataRepo;

        public LobbyDataController(ILobbyRepository lobbyRepository, IMembersRepository membersRepository, ILobbyDataRepository lobbyDataRepository) {
            _lobbyRepo = lobbyRepository;
            _membersRepo = membersRepository;
            _lobbyDataRepo = lobbyDataRepository;
        }

        [HttpGet("{Id:guid}/data")]
        public async Task<IActionResult> GetLobbyData([FromRoute] Guid Id, [FromBody] BaseRequestDto requestDto) {
            var lobby = await _lobbyRepo.GetById(Id);

            if (lobby == null) {
                return NotFound();
            }

            //var lobbyMembers = await _membersRepo.GetLobbyMembers(Id);

            List<LobbyData> lobbyData;

            if (lobby.HostId == requestDto.RequestSenderId) { //is lobby host -> show all data
                lobbyData = await _lobbyDataRepo.GetLobbyData(Id, DataFilterOptions.Owner);
            }
            else if (await _membersRepo.IsMemberOfThisLobby(Id, requestDto.RequestSenderId)) { //is member but not host -> dont show private data
                lobbyData = await _lobbyDataRepo.GetLobbyData(Id, DataFilterOptions.Member);
            }
            else { //is outsider -> show only public data 
                lobbyData = await _lobbyDataRepo.GetLobbyData(Id, DataFilterOptions.Outsider);
            }

            //return Ok(lobbyData); 
            //return Ok(lobbyData.Select(d => new LobbyDataResponseDto(d)));
            
            var lobbyDataResponse = new DataResponseDto_Dictionary(lobbyData);
            return Ok(lobbyDataResponse);
        }


        [HttpPut("{Id:guid}/data")]
        public async Task<IActionResult> UpseretLobbyData([FromRoute] Guid Id, [FromBody] UpdateDataRequestDto requestDto) {
            if (!ModelState.IsValid) {
                return BadRequest("Invalid request data");
            }

            var lobby = await _lobbyRepo.GetById(Id);

            if (lobby == null) {
                return NotFound();
            }

            // Check if the request sender is the host
            if (lobby.HostId != requestDto.RequestSenderId) {
                return Unauthorized("Only the lobby host can update data");
            }

            await _lobbyDataRepo.UpdateLobbyData(Id, requestDto.Data);

            return NoContent();
        }

        [HttpDelete("{Id:guid}/data")]
        public async Task<IActionResult> DeleteLobbyData([FromRoute] Guid Id, [FromBody] DeleteDataRequestDto requestDto) {
            if (!ModelState.IsValid) {
                return BadRequest("Invalid request data");
            }

            var lobby = await _lobbyRepo.GetById(Id);
            if (lobby == null) {
                return NotFound();
            }

            // Check if the request sender is the host
            if (lobby.HostId != requestDto.RequestSenderId) {
                return Unauthorized("Only the lobby host can delete data");
            }

            await _lobbyDataRepo.DeteleLobbyData(Id, requestDto.Keys);

            return NoContent();
        }
    }
}
