using MiniLobby.Models;

namespace MiniLobby.Dtos {
    public class LobbyMemberResponseDto {
        public Guid Id { get; set; }

        public LobbyMemberResponseDto(LobbyMember member) {
            Id = member.Id;
        }
    }
}
