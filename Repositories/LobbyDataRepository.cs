using Microsoft.EntityFrameworkCore;
using MiniLobby.Data;
using MiniLobby.Enums;
using MiniLobby.Interfaces;
using MiniLobby.Misc;
using MiniLobby.Models;

namespace MiniLobby.Repositories {
    public class LobbyDataRepository : ILobbyDataRepository {
        private readonly ApplicationDbContext _context;
        public LobbyDataRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<List<LobbyData>> GetLobbyData(Guid lobbyId, DataFilterOptions filterOptions) {
            var query =  _context.LobbyData.Where(d => d.LobbyId == lobbyId);

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

        public async Task UpdateLobbyData(Guid lobbyId, Dictionary<string, DataPoint> data) {
            var existingData = await _context.LobbyData.Where(d => d.LobbyId == lobbyId).ToListAsync();

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
                    _context.LobbyData.Add(new LobbyData {
                        LobbyId = lobbyId,
                        Key = key,
                        Value = dataPoint.Value,
                        Visibility = dataPoint.Visibility
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeteleLobbyData(Guid lobbyId, List<string> keys) {
            var dataPoints = await _context.LobbyData
                .Where(d => d.LobbyId == lobbyId && keys.Contains(d.Key))
                .ToListAsync();

            _context.LobbyData.RemoveRange(dataPoints);
            await _context.SaveChangesAsync();
        }
    }
}
