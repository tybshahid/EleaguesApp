using System.Collections.Generic;
using System.Threading.Tasks;
using EleaguesApp.API.Dtos;
using EleaguesApp.API.Helpers;
using EleaguesApp.API.Models;

namespace EleaguesApp.API.Data
{
    public interface IEleaguesRepository
    {
        void Add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<bool> LeagueExists(string name);
        Task<League> GetLeague(int id);
        Task<PagedList<User>> GetUsers(UserParams userParams);
        Task<User> GetUser(int id);
        Task<IEnumerable<DropdownListDto>> GetLeaguesList();
        Task<PagedList<League>> GetLeagues(UserParams userParams);
        Task<GameManagerDto> GetGameManager(bool isAdmin, int userId);
        Task<Game> GetGame(int leagueId, int winnerId, int opponentId, string stage);
        Task<PagedList<Game>> GetGames(UserParams userParams);
        PagedList<LeaderboardDto> GetLeaderboard(UserParams userParams);
        bool UpdateUser(int id, string password, bool isActive, string photoUrl, string actionBy);
    }
}