using Microsoft.EntityFrameworkCore;
using MiniLobby.Models;

namespace MiniLobby.Data {
    public class ApplicationDbContext : DbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {
            
        }

        public DbSet<Lobby> Lobbies { get; set; }
        public DbSet<LobbyData> LobbyData { get; set; }
        public DbSet<LobbyMember> LobbyMembers { get; set; }
        public DbSet<MemberData> MemberData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LobbyData>().HasKey(e => new { e.LobbyId, e.Key});
            modelBuilder.Entity<LobbyMember>().HasKey(e => new { e.CurrentLobbyId, e.MemberId});
            modelBuilder.Entity<MemberData>().HasKey(e => new { e.MemberId, e.Key});
        }

    }
}
