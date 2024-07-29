using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniLobby.Data;
using MiniLobby.Dtos;
using MiniLobby.Interfaces;
using MiniLobby.Mappers;
using MiniLobby.Models;

namespace MiniLobby.Repositories {
    public class LobbyRepository : ILobbyRepository {
        private readonly ApplicationDbContext _context;
        public LobbyRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<List<Lobby>> GetPublicLobbies() {
            return await _context.Lobbies.Where(l => l.IsPrivate == false).ToListAsync();
        }

        public async Task<Lobby> GetById(Guid lobbyId) {
            return await _context.Lobbies.FindAsync(lobbyId);
        }

        public async Task<Lobby> CreateLobby(CreateLobbyRequestDto requestDto) {
            var lobby = new Lobby {
                Id = Guid.NewGuid(),
                HostId = requestDto.RequestSenderId,
                Name = requestDto.LobbyName,
                MemberLimit = requestDto.MemberLimit,
                IsPrivate = requestDto.IsPrivate,
            };

            await _context.Lobbies.AddAsync(lobby);
            await _context.LobbyMembers.AddAsync(new LobbyMember { CurrentLobbyId = lobby.Id, MemberId = requestDto.RequestSenderId });

            if (requestDto.Data != null && requestDto.Data.Any()) {
                foreach (var kvp in requestDto.Data) {
                    var lobbyData = new LobbyData {
                        LobbyId = lobby.Id,
                        Key = kvp.Key,
                        Value = kvp.Value.Value,
                        Visibility = kvp.Value.Visibility
                    };
                    await _context.LobbyData.AddAsync(lobbyData);
                }
            }

            await _context.SaveChangesAsync();

            return lobby;
        }

        public async Task DeleteLobby(Lobby lobby) {
            //Delete lobby data
            var lobbyData = await _context.LobbyData.Where(d => d.LobbyId == lobby.Id).ToListAsync();
            _context.LobbyData.RemoveRange(lobbyData);

            //find lobby members
            var lobbyMembers = await _context.LobbyMembers.Where(m => m.CurrentLobbyId == lobby.Id).ToListAsync();

            //delete member data
            foreach (var member in lobbyMembers) {
                var memberData = await _context.MemberData.Where(md => md.MemberId == member.MemberId).ToListAsync();
                _context.MemberData.RemoveRange(memberData);
            }

            //delete members
            _context.LobbyMembers.RemoveRange(lobbyMembers);

            //delete lobby
            _context.Lobbies.Remove(lobby);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> DoesLobbyExist(Guid lobbyId) {
            return await _context.Lobbies.AnyAsync(l => l.Id == lobbyId);
        }

        public async Task UpdateLobby(Lobby lobby) {
            _context.Lobbies.Update(lobby);
            await _context.SaveChangesAsync();
        }
    }
}
