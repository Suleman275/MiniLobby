
using MiniLobby.Dtos;
using MiniLobby.Models;

namespace MiniLobby.Mappers {
    public static class LobbyResponseMapper {

        public static LobbyResponseDto ToLobbyResponseDto(this Lobby lobby) {
            return new LobbyResponseDto(lobby);
        }
    }
}
