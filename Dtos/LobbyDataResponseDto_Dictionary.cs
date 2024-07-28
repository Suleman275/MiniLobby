using MiniLobby.Misc;
using MiniLobby.Models;

namespace MiniLobby.Dtos {
    public class LobbyDataResponseDto_Dictionary {
        public Dictionary<string, DataPoint> Data { get; set; }

        public LobbyDataResponseDto_Dictionary(List<LobbyData> lobbyData) {
            Data = new Dictionary<string, DataPoint>();

            foreach (var item in lobbyData) {
                Data.Add(item.Key, new DataPoint(item.Value, item.Visibility));
            }
        }
    }
}
