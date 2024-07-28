using System.ComponentModel.DataAnnotations;

namespace MiniLobby.Models {
    public class Lobby {
        [Required]
        public Guid Id{ get; set; }
        [Required]
        public Guid HostId{ get; set; }
        [Required]
        public string Name{ get; set; }
        [Required]
        [Range(2, 10, ErrorMessage = "MemberLimit must be between 2 and 10.")]
        public int MemberLimit{ get; set; }
        public bool IsPrivate{ get; set; }
    }
}
