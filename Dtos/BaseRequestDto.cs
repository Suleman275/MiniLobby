using System.ComponentModel.DataAnnotations;

namespace MiniLobby.Dtos {
    public class BaseRequestDto {
        [Required]
        public Guid RequestSenderId { get; set; }
    }
}
