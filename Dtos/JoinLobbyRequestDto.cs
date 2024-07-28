using System.ComponentModel.DataAnnotations;

namespace MiniLobby.Dtos {
    public class JoinLobbyRequestDto {
        [Required]
        public Guid RequestSenderId { get; set; }
    }
}
