using MiniLobby.Enums;
using MiniLobby.Misc;
using MiniLobby.Models;

namespace MiniLobby.Interfaces {
    public interface ILobbyDataRepository {

        public Task<List<LobbyData>> GetLobbyData(Guid lobbyId, DataFilterOptions filterOptions);

        public Task UpdateLobbyData(Guid lobbyId, Dictionary<string, DataPoint> data);
        public Task DeteleLobbyData(Guid lobbyId, List<string> keys);
    }
}
