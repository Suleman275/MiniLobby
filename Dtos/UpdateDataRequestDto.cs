using MiniLobby.Misc;
using System.ComponentModel.DataAnnotations;

namespace MiniLobby.Dtos {
    public class UpdateDataRequestDto : BaseRequestDto {
        public Dictionary<string, DataPoint> Data { get; set; }
    }
}
