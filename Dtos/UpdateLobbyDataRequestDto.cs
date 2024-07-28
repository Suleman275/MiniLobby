using MiniLobby.Misc;

namespace MiniLobby.Dtos {
    public class UpdateLobbyDataRequestDto {
        public Guid RequestSenderId { get; set; }
        public Dictionary<string, DataPoint> Data { get; set; }
    }
}
