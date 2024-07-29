using Microsoft.EntityFrameworkCore;
using MiniLobby.Data;
using MiniLobby.Enums;
using MiniLobby.Interfaces;
using MiniLobby.Misc;
using MiniLobby.Models;

namespace MiniLobby.Repositories {
    public class MemberDataRepository : IMemberDataRepository {
        private readonly ApplicationDbContext _context;

        public MemberDataRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task DeleteAllMemberData(Guid memberId) {
            // Delete member's data
            var memberData = await _context.MemberData
                .Where(md => md.MemberId == memberId)
                .ToListAsync();
            _context.MemberData.RemoveRange(memberData);

            await _context.SaveChangesAsync();
        }

        public async Task DeteleMemberData(Guid memberId, List<string> keys) {
            var dataPoints = await _context.MemberData
                .Where(d => d.MemberId == memberId && keys.Contains(d.Key))
                .ToListAsync();

            _context.MemberData.RemoveRange(dataPoints);
            await _context.SaveChangesAsync();
        }

        public async Task<List<MemberData>> GetMemberData(Guid memberId, DataFilterOptions filterOptions) {
            var query = _context.MemberData.Where(d => d.MemberId == memberId);

            switch (filterOptions) {
                case DataFilterOptions.Owner:
                    return await query.ToListAsync();
                case DataFilterOptions.Member:
                    return await query.Where(d => d.Visibility != VisibilityOptions.Private).ToListAsync();
                case DataFilterOptions.Outsider:
                    return await query.Where(d => d.Visibility == VisibilityOptions.Public).ToListAsync();
                default:
                    return await query.Where(d => d.Visibility == VisibilityOptions.Public).ToListAsync();
            }
        }

        public async Task UpdateMemberData(Guid memberId, Dictionary<string, DataPoint> data) {
            var existingData = await _context.MemberData.Where(d => d.MemberId == memberId).ToListAsync();

            foreach (var key in data.Keys) {
                var dataPoint = data[key];
                var existingDataPoint = existingData.FirstOrDefault(d => d.Key == key);

                if (existingDataPoint != null) {
                    // Update existing data point
                    existingDataPoint.Value = dataPoint.Value;
                    existingDataPoint.Visibility = dataPoint.Visibility;
                }
                else {
                    // Add new data point
                    _context.MemberData.Add(new MemberData {
                        MemberId = memberId,
                        Key = key,
                        Value = dataPoint.Value,
                        Visibility = dataPoint.Visibility
                    });
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
