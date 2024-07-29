using MiniLobby.Misc;
using MiniLobby.Models;

namespace MiniLobby.Interfaces {
    public interface IMembersRepository {
        public Task<List<LobbyMember>> GetLobbyMembers(Guid lobbyId);

        public Task<int> GetLobbyMemberCount(Guid lobbyId);
        public Task<LobbyMember> GetLobbyMember(Guid lobbyId, Guid memberId);

        public Task<bool> IsMemberOfOtherLobby(Guid excludedLobbyId, Guid memberId);
        public Task<bool> IsMemberOfThisLobby(Guid lobbyId, Guid memberId);

        public Task AddMemberToLobby(Guid lobbyId, Guid memberId, Dictionary<string, DataPoint> data);
        public Task RemoveMemberFromLobby(Guid lobbyId, Guid memberId);

    }
}
