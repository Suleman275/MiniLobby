using System.ComponentModel.DataAnnotations;

namespace MiniLobby.Dtos {
    public class DeleteDataRequestDto {
        [Required]
        public Guid RequestSenderId { get; set; }

        [Required]
        public List<string> Keys { get; set; }
    }
}
