using MiniLobby.Misc;
using MiniLobby.Models;

namespace MiniLobby.Dtos {
    public class CompleteLobbyResponseDto {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int MemberLimit { get; set; }
        public Dictionary<string, DataPoint> Data { get; set; }
        public List<MemberDto> Members { get; set; }
    }


    public class MemberDto {
        public Guid MemberId { get; set; }
        public Dictionary<string, DataPoint> Data { get; set; }
    }
}
