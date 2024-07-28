using MiniLobby.Models;
using System.ComponentModel.DataAnnotations;

namespace MiniLobby.Dtos {
    public class LobbyResponseDto {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int MemberLimit { get; set; }

        public LobbyResponseDto(Lobby lobby) {
            Id = lobby.Id;
            Name = lobby.Name;
            MemberLimit = lobby.MemberLimit;
        }
    }
}
