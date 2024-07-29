using System.ComponentModel.DataAnnotations;

namespace MiniLobby.Dtos {
    public class DeleteDataRequestDto : BaseRequestDto {
        [Required]
        public List<string> Keys { get; set; }
    }
}
