using MiniLobby.Misc;
using MiniLobby.Models;

namespace MiniLobby.Dtos {
    public class DataResponseDto_Dictionary {
        public Dictionary<string, DataPoint> Data { get; set; }

        public DataResponseDto_Dictionary(List<LobbyData> lobbyData) {
            Data = new Dictionary<string, DataPoint>();

            foreach (var item in lobbyData) {
                Data.Add(item.Key, new DataPoint(item.Value, item.Visibility));
            }
        }
        
        public DataResponseDto_Dictionary(List<MemberData> memberData) {
            Data = new Dictionary<string, DataPoint>();

            foreach (var item in memberData) {
                Data.Add(item.Key, new DataPoint(item.Value, item.Visibility));
            }
        }
    }
}
