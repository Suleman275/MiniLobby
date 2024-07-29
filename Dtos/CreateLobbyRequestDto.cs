using System.ComponentModel.DataAnnotations;

namespace MiniLobby.Dtos {
    public class CreateLobbyRequestDto : BaseRequestDto {        
        [Required]
        public string LobbyName { get; set; }
        
        [Required]
        [Range(2, 10, ErrorMessage = "MemberLimit must be between 2 and 10.")]
        public int MemberLimit { get; set; }
        
        public bool IsPrivate { get; set; }

        public bool IsHostMigrationEnabled { get; set; }
    }
}
