using System.ComponentModel.DataAnnotations;

namespace MiniLobby.Dtos {
    public class DeleteLobbyRequestDto {
        [Required]
        public Guid RequestSenderId { get; set; }
    }
}
