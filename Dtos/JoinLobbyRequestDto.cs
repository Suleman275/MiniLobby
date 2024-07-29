using MiniLobby.Misc;

namespace MiniLobby.Dtos {
   public class JoinLobbyRequestDto : BaseRequestDto {
        public Dictionary<string, DataPoint>? Data { get; set; }
    }
}
