using System.ComponentModel.DataAnnotations;

namespace MiniLobby.Dtos {
    public class LeaveLobbyRequestDto {
        [Required]
        public Guid RequestSenderId { get; set; }
    }
}
