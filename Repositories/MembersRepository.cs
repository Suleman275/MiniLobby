using Microsoft.EntityFrameworkCore;
using MiniLobby.Data;
using MiniLobby.Interfaces;
using MiniLobby.Models;

namespace MiniLobby.Repositories {
    public class MembersRepository : IMembersRepository {
        private readonly ApplicationDbContext _context;
        public MembersRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<List<LobbyMember>> GetLobbyMembers(Guid lobbyId) {
            return await _context.LobbyMembers.Where(m => m.CurrentLobbyId == lobbyId).ToListAsync();
        }
    }
}
