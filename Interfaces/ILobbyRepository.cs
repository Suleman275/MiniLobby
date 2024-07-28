using MiniLobby.Dtos;
using MiniLobby.Models;

namespace MiniLobby.Interfaces {
    public interface ILobbyRepository { 
        public Task<List<Lobby>> GetPublicLobbies();

        public Task<Lobby> GetById(Guid lobbyId);

        public Task<Lobby> CreateLobby(CreateLobbyRequestDto requestDto);

        public Task DeleteLobby(Lobby lobby);
    }
}
