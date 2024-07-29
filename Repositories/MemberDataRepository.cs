using Microsoft.EntityFrameworkCore;
using MiniLobby.Data;
using MiniLobby.Interfaces;

namespace MiniLobby.Repositories {
    public class MemberDataRepository : IMemberDataRepository {
        private readonly ApplicationDbContext _context;

        public MemberDataRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task DeleteMemberData(Guid memberId) {
            // Delete member's data
            var memberData = await _context.MemberData
                .Where(md => md.MemberId == memberId)
                .ToListAsync();
            _context.MemberData.RemoveRange(memberData);

            await _context.SaveChangesAsync();
        }
    }
}
