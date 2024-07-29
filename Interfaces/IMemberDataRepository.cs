namespace MiniLobby.Interfaces {
    public interface IMemberDataRepository {

        public Task DeleteMemberData(Guid memberId);
    }
}
