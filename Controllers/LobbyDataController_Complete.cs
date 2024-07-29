using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniLobby.Dtos;
using MiniLobby.Enums;
using MiniLobby.Interfaces;
using MiniLobby.Misc;
using MiniLobby.Models;

namespace MiniLobby.Controllers {
    [Route("api/lobbies")]
    [ApiController]
    public class LobbyDataController_Complete : ControllerBase {
        private readonly IMembersRepository _membersRepo;
        private readonly IMemberDataRepository _memberDataRepo;
        private readonly ILobbyRepository _lobbyRepo;
        private readonly ILobbyDataRepository _lobbyDataRepo;
        public LobbyDataController_Complete(ILobbyRepository lobbyRepository, ILobbyDataRepository lobbyDataRepository, IMembersRepository membersRepository, IMemberDataRepository memberDataRepository) {
            _lobbyDataRepo = lobbyDataRepository;
            _membersRepo = membersRepository;
            _memberDataRepo = memberDataRepository;
            _lobbyRepo = lobbyRepository;
        }


        [HttpGet("{Id:guid}/full")]
        public async Task<IActionResult> GetFullLobbyData([FromRoute] Guid Id, [FromBody] BaseRequestDto requestDto) {
            if (!await _lobbyRepo.DoesLobbyExist(Id)) {
                return NotFound("Lobby not found");
            }

            var lobby = await _lobbyRepo.GetById(Id);

            if (lobby == null) {
                return NotFound("Lobby not found");
            }

            var response = new CompleteLobbyResponseDto {
                Id = lobby.Id,
                Name = lobby.Name,
                MemberLimit = lobby.MemberLimit,
                Data = new Dictionary<string, DataPoint>(),
                Members = new List<MemberDto>()
            };

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

            foreach (var item in lobbyData) {
                response.Data.Add(item.Key, new DataPoint(item.Value, item.Visibility));
            }

            var members = await _membersRepo.GetLobbyMembers(Id);

            foreach (var member in members) {
                List<MemberData> memberData;

                if (requestDto.RequestSenderId == member.MemberId) { //owner of data -> show all 
                    memberData = await _memberDataRepo.GetMemberData(member.MemberId, DataFilterOptions.Owner);
                }
                else if (await _membersRepo.IsMemberOfThisLobby(Id, requestDto.RequestSenderId)) { //another lobby member requesting -> hide private data
                    memberData = await _memberDataRepo.GetMemberData(member.MemberId, DataFilterOptions.Member);
                }
                else { //outsider requesting -> show only public data
                    memberData = await _memberDataRepo.GetMemberData(member.MemberId, DataFilterOptions.Outsider);
                }

                var memberDto = new MemberDto {
                    MemberId = member.MemberId,
                    Data = new Dictionary<string, DataPoint>()
                };

                foreach (var item in memberData) {
                    memberDto.Data.Add(item.Key, new DataPoint(item.Value, item.Visibility));
                }

                response.Members.Add(memberDto);
            }

            return Ok(response);
        }

    }
}
