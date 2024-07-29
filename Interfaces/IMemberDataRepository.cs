using MiniLobby.Enums;
using MiniLobby.Misc;
using MiniLobby.Models;

namespace MiniLobby.Interfaces {
    public interface IMemberDataRepository {

        public Task DeleteAllMemberData(Guid memberId);

        public Task<List<MemberData>> GetMemberData(Guid memberId, DataFilterOptions filterOptions);

        public Task UpdateMemberData(Guid memberId, Dictionary<string, DataPoint> data);

        public Task DeteleMemberData(Guid memberId, List<string> keys);
    }
}
