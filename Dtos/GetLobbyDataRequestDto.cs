using System.ComponentModel.DataAnnotations;

namespace MiniLobby.Dtos {
    public class GetLobbyDataRequestDto {
        [Required]
        public Guid RequestSenderId { get; set; }
    }
}
