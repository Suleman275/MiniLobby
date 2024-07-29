using Microsoft.EntityFrameworkCore;
using MiniLobby.Data;
using MiniLobby.Interfaces;
using MiniLobby.Misc;
using MiniLobby.Models;

namespace MiniLobby.Repositories {
    public class MembersRepository : IMembersRepository {
        private readonly ApplicationDbContext _context;
        public MembersRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task AddMemberToLobby(Guid lobbyId, Guid memberId, Dictionary<string, DataPoint> data) {
            var newMember = new LobbyMember {
                MemberId = memberId,
                CurrentLobbyId = lobbyId
            };

            await _context.LobbyMembers.AddAsync(newMember);

            if (data != null && data.Any()) {
                foreach (var kvp in data) {
                    var memberData = new MemberData {
                        MemberId = memberId,
                        Key = kvp.Key,
                        Value = kvp.Value.Value,
                        Visibility = kvp.Value.Visibility
                    };
                    await _context.MemberData.AddAsync(memberData);
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task<LobbyMember> GetLobbyMember(Guid lobbyId, Guid memberId) {
            return await _context.LobbyMembers.FirstOrDefaultAsync(m => m.CurrentLobbyId == lobbyId && m.MemberId == memberId);
        }

        public async Task<List<LobbyMember>> GetLobbyMembers(Guid lobbyId) {
            return await _context.LobbyMembers.Where(m => m.CurrentLobbyId == lobbyId).ToListAsync();
        }

        public async Task<int> GetLobbyMemberCount(Guid lobbyId) {
            return await _context.LobbyMembers.CountAsync(m => m.CurrentLobbyId == lobbyId);
        }

        public async Task<bool> IsMemberOfOtherLobby(Guid excludedLobbyId, Guid memberId) {
            return await _context.LobbyMembers.AnyAsync(m => m.MemberId == memberId && m.CurrentLobbyId != excludedLobbyId);
        }

        public async Task RemoveMemberFromLobby(Guid lobbyId, Guid memberId) {
            var member = await GetLobbyMember(lobbyId, memberId);

            _context.LobbyMembers.Remove(member);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsMemberOfThisLobby(Guid lobbyId, Guid memberId) {
            return await _context.LobbyMembers.AnyAsync(m => m.CurrentLobbyId == lobbyId && m.MemberId == memberId);
        }
    }
}
