using System.ComponentModel.DataAnnotations;

namespace MiniLobby.Dtos {
    public class CreateLobbyRequestDto {
        [Required]
        public Guid RequestSenderId { get; set; }
        
        [Required]
        public string LobbyName { get; set; }
        
        [Required]
        [Range(2, 10, ErrorMessage = "MemberLimit must be between 2 and 10.")]
        public int MemberLimit { get; set; }
        
        public bool IsPrivate { get; set; }
    }
}
