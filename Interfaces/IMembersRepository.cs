using MiniLobby.Models;

namespace MiniLobby.Interfaces {
    public interface IMembersRepository {
        public Task<List<LobbyMember>> GetLobbyMembers(Guid lobbyId);
    }
}
