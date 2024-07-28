using MiniLobby.Enums;
using MiniLobby.Models;

namespace MiniLobby.Dtos {
    public class LobbyDataResponseDto {
        public string Key { get; set; }
        public string Value { get; set; }
        public VisibilityOptions Visibility { get; set; }

        public LobbyDataResponseDto(LobbyData data) {
            Key = data.Key;
            Value = data.Value;
            Visibility = data.Visibility;
        }
    }
}
