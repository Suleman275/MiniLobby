namespace MiniLobby.Models {
    public class LobbyMember {
        public Guid MemberId { get; set; }
        public Guid CurrentLobbyId { get; set; }
    }
}
