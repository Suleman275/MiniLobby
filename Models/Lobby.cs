namespace MiniLobby.Models {
    public class Lobby {
        public Guid Id{ get; set; }
        public Guid HostId{ get; set; }
        public string Name{ get; set; }
        public int MemberLimit{ get; set; }
        public bool IsPrivate{ get; set; }
    }
}
