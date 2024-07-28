using MiniLobby.Enums;

namespace MiniLobby.Models {
    public class LobbyData {
        public Guid LobbyId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public VisibilityOptions Visibility { get; set; }
    }
}
