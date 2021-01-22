using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EleaguesApp.API.Dtos;
using EleaguesApp.API.Helpers;
using EleaguesApp.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EleaguesApp.API.Data
{
    public class EleaguesRepository : IEleaguesRepository
    {
        private readonly DataContext _context;
        public EleaguesRepository(DataContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }
        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }
        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }
        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> LeagueExists(string name)
        {
            if (await _context.Leagues.AnyAsync(x => x.Name.ToLower() == name.ToLower()))
                return true;

            return false;
        }
        public async Task<IEnumerable<DropdownListDto>> GetLeaguesList()
        {
            var league = await _context.Leagues.OrderBy(p => p.Name)
                .Select(p => new DropdownListDto()
                {
                    Id = p.Id,
                    Value = p.Name
                }).ToListAsync();

            return league;
        }
        public async Task<League> GetLeague(int id)
        {
            var league = await _context.Leagues.FirstOrDefaultAsync(u => u.Id == id);
            return league;
        }
        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }
        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users = _context.Users.OrderBy(u => u.CreatedOn).AsQueryable();

            if (userParams.UserName != null)
                users = users.Where(u => u.Username.ToLower().Contains(userParams.UserName.ToLower()));
            if (userParams.KnownAs != null)
                users = users.Where(u => u.KnownAs.ToLower().Contains(userParams.KnownAs.ToLower()));
            if (userParams.Country != null)
                users = users.Where(u => u.Country.ToLower().Contains(userParams.Country.ToLower()));

            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }
        public async Task<PagedList<League>> GetLeagues(UserParams userParams)
        {
            var leagues = _context.Leagues.OrderByDescending(u => u.CreatedOn).AsQueryable();
            return await PagedList<League>.CreateAsync(leagues, userParams.PageNumber, userParams.PageSize);
        }
        public async Task<Game> GetGame(int leagueId, int winnerId, int opponentId, string stage)
        {
            var game = await _context.Games
                .Include(x => x.League)
                .OrderByDescending(p => p.Id)
                .FirstOrDefaultAsync(p => p.LeagueId == leagueId &&
                    (p.WinnerId == winnerId || p.WinnerId == opponentId) &&
                    (p.OpponentId == opponentId || p.OpponentId == winnerId) &&
                    p.Stage == stage);

            return game;
        }
        public async Task<GameManagerDto> GetGameManager(bool isAdmin, int userId)
        {
            var gameManagerDto = new GameManagerDto();
            gameManagerDto.Leagues = await _context.Leagues.Where(p => !p.IsClosed).OrderByDescending(p => p.StartDate)
                .Select(p => new DropdownListDto()
                {
                    Id = p.Id,
                    Value = p.Name
                }).ToListAsync();

            if (isAdmin)
            {
                gameManagerDto.Winners = await _context.Users.Where(p => p.IsActive).OrderBy(p => p.KnownAs)
                    .Select(p => new DropdownListDto()
                    {
                        Id = p.Id,
                        Value = p.KnownAs
                    }).ToListAsync();

                gameManagerDto.Opponents = gameManagerDto.Winners;
            }
            else
            {
                gameManagerDto.Winners = await _context.Users.Where(p => p.IsActive && p.Id == userId)
                    .Select(p => new DropdownListDto()
                    {
                        Id = p.Id,
                        Value = p.KnownAs
                    }).ToListAsync();

                gameManagerDto.Opponents = await _context.Users.Where(p => p.IsActive && p.Id != userId).OrderBy(p => p.KnownAs)
                .Select(p => new DropdownListDto()
                {
                    Id = p.Id,
                    Value = p.KnownAs
                }).ToListAsync();
            }

            return gameManagerDto;
        }
        public async Task<PagedList<Game>> GetGames(UserParams userParams)
        {
            var games = _context.Games
                .Include(g => g.League)
                .Include(g => g.Winner)
                .Include(g => g.Opponent)
                .OrderByDescending(g => g.Id)
                .AsQueryable();

            if (userParams.LeagueId != 0)
                games = games.Where(g => g.LeagueId == userParams.LeagueId);

            if (userParams.WinnerId == userParams.OpponentId && userParams.WinnerId != 0 && userParams.OpponentId != 0)
                games = games.Where(g => g.WinnerId == userParams.WinnerId || g.OpponentId == userParams.OpponentId);
            else
            {
                if (userParams.WinnerId != 0)
                    games = games.Where(g => g.WinnerId == userParams.WinnerId);

                if (userParams.OpponentId != 0)
                    games = games.Where(g => g.OpponentId == userParams.OpponentId);
            }

            return await PagedList<Game>.CreateAsync(games, userParams.PageNumber, userParams.PageSize);
        }
        public PagedList<LeaderboardDto> GetLeaderboard(UserParams userParams)
        {
            List<LeaderboardDto> lstResult = new List<LeaderboardDto>();

            var players = _context.Users
                .Where(p => p.IsActive == true)
                .AsQueryable();

            if (userParams.WinnerId != 0)
                players = players.Where(u => u.Id == userParams.WinnerId);

            var games = _context.Games
                .Include(g => g.League)
                .Include(g => g.Winner)
                .Include(g => g.Opponent)
                .AsQueryable();

            if (userParams.LeagueId != 0)
                games = games.Where(g => g.League.Id == userParams.LeagueId);

            if (players.Count() > 0)
            {
                foreach (var player in players)
                {
                    int Played = 0;
                    int Won = 0;
                    int Lost = 0;
                    int Points = 0;
                    string History = "";

                    int RunsScored = 0;
                    decimal OversPlayed = 0;
                    int WicketsLost = 0;

                    int Fifties = 0;
                    int Hundreds = 0;
                    int FiveWickets = 0;

                    int RunsConceded = 0;
                    decimal OversBowled = 0;
                    int WicketsTaken = 0;

                    if (games.Count() > 0)
                    {
                        foreach (var game in games)
                        {
                            // Games Won
                            if (game.Winner.Id == player.Id)
                            {
                                Played += 1;
                                Won += 1;
                                History += "W|";
                                Points += 2;

                                Fifties += game.Fifties;
                                Hundreds += game.Hundreds;
                                FiveWickets += game.FiveWickets;

                                RunsScored += game.Runs;

                                if (game.Wickets != 10)
                                    OversPlayed += game.Overs;
                                else
                                    OversPlayed += Convert.ToDecimal(Regex.Match(game.League.Format, @"\d+").Value);

                                WicketsLost += game.Wickets;
                                RunsConceded += game.OpponentRuns;
                                //OversBowled += Convert.ToDecimal(game.OpponentOvers);
                                OversBowled += Convert.ToDecimal(Regex.Match(game.League.Format, @"\d+").Value);
                                WicketsTaken += game.OpponentWickets;
                            }

                            // Games Lost
                            if (game.OpponentId == player.Id)
                            {
                                Played += 1;
                                Lost += 1;
                                History += "L|";

                                Fifties += game.OpponentFifties;
                                Hundreds += game.OpponentHundreds;
                                FiveWickets += game.OpponentFiveWickets;

                                RunsScored += game.OpponentRuns;
                                //OversPlayed += Convert.ToDecimal(game.OpponentOvers);
                                OversPlayed += Convert.ToDecimal(Regex.Match(game.League.Format, @"\d+").Value);
                                WicketsLost += game.OpponentWickets;
                                RunsConceded += game.Runs;
                                OversBowled += Convert.ToDecimal(game.Overs);
                                WicketsTaken += game.Wickets;
                            }
                        }
                    }

                    if (Played > 0)
                    {
                        LeaderboardDto objLeaderBoard = new LeaderboardDto();
                        objLeaderBoard.Player = player.KnownAs;
                        objLeaderBoard.Country = player.Country;
                        // objLeaderBoard.GroupName = LeagueID == null  "N/A" : db.LeaguesPlayersMaster.FirstOrDefault(p => p.LeagueID == LeagueID && p.PlayerID == player.RecordID).Groups.GroupName;
                        objLeaderBoard.Played = Played;
                        objLeaderBoard.Won = Won;
                        objLeaderBoard.Lost = Lost;
                        objLeaderBoard.Points = Points;
                        // objLeaderBoard.History = History;
                        objLeaderBoard.RunsScored = RunsScored;
                        objLeaderBoard.OversPlayed = Math.Truncate(OversPlayed * 100) / 100;
                        objLeaderBoard.WicketsLost = WicketsLost;
                        objLeaderBoard.Fifties = Fifties;
                        objLeaderBoard.Hundreds = Hundreds;
                        objLeaderBoard.FiveWickets = FiveWickets;
                        objLeaderBoard.RunsConceded = RunsConceded;
                        objLeaderBoard.OversBowled = Math.Truncate(OversBowled * 100) / 100;
                        objLeaderBoard.WicketsTaken = WicketsTaken;
                        objLeaderBoard.WinPercentage = Math.Truncate((Won > 0 ? (Won * 100.00) / Played : 0.00) * 100) / 100;

                        objLeaderBoard.Average = Math.Truncate((RunsScored / OversPlayed) * 100) / 100;
                        objLeaderBoard.Economy = Math.Truncate((RunsConceded / OversBowled) * 100) / 100;

                        try
                        {
                            objLeaderBoard.NRR = Math.Truncate(((RunsScored / OversPlayed) - (RunsConceded / OversBowled)) * 100) / 100;
                        }
                        catch
                        {
                            objLeaderBoard.NRR = 0;
                        }

                        lstResult.Add(objLeaderBoard);
                    }

                }
            }

            if (lstResult.Count == 0)
                lstResult.Add(new LeaderboardDto());

            return PagedList<LeaderboardDto>.CreatePagedList(lstResult.OrderByDescending(p => p.Points).ThenByDescending(p => p.NRR), userParams.PageNumber, userParams.PageSize);
        }
        #region UpdateUser
        public bool UpdateUser(int id, string password, bool isActive, string photoUrl, string actionBy)
        {
            var userToUpdate = _context.Users.FirstOrDefault(u => u.Id == id);
            if (userToUpdate != null)
            {
                if (!String.IsNullOrEmpty(password))
                {
                    // Generate Password
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash(password, out passwordHash, out passwordSalt);
                    userToUpdate.PasswordHash = passwordHash;
                    userToUpdate.PasswordSalt = passwordSalt;
                }

                // if (isActive != userToUpdate.IsActive)
                //     userToUpdate.IsActive = isActive;

                if (!String.IsNullOrEmpty(photoUrl))
                    userToUpdate.PhotoUrl = photoUrl;

                userToUpdate.CreatedOn = DateTime.Now;
                userToUpdate.CreatedBy = actionBy.ToLower();

                Update(userToUpdate);
                return true;
            }

            return false;
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        #endregion
    }
}