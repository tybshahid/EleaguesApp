using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using EleaguesApp.API.Data;
using EleaguesApp.API.Dtos;
using EleaguesApp.API.Helpers;
using EleaguesApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EleaguesApp.API.Controllers
{
    // [ServiceFilter(typeof(LogUserActivity))]
    // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IEleaguesRepository _repo;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public GamesController(
            IEleaguesRepository repo,
            IMapper mapper,
            IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _mapper = mapper;
            _repo = repo;
        }

        [Authorize, HttpGet("manager")]
        public async Task<IActionResult> GetGameManager()
        {
            var user = await _repo.GetUser(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            if (user == null)
                return Unauthorized();

            var result = await _repo.GetGameManager(user.IsAdmin, user.Id);
            return Ok(result);
        }

        [HttpGet("manager/public")]
        public async Task<IActionResult> GetGameManagerPublic()
        {
            var result = await _repo.GetGameManager(false, 0);
            return Ok(result);
        }

        // Save the game
        [Authorize, HttpPost]
        public async Task<IActionResult> SaveGame([FromForm]Game gameForRepo, IFormFile file)
        {
            var user = await _repo.GetUser(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            if (user == null)
                return Unauthorized();

            if (gameForRepo.WinnerId == gameForRepo.OpponentId)
                return BadRequest("Opponent same as Winner");

            if (file == null)
                return BadRequest("Scorecard not attached");

            if (file != null && file.Length > 0)
            {
                if (new ImageHelper().Validate(file))
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, @"Uploads\Games", fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    gameForRepo.FileName = fileName;
                }
                else
                {
                    return BadRequest("Invalid File");
                }
            }

            // Set Round
            var gameFromRepo = await _repo.GetGame(gameForRepo.LeagueId, gameForRepo.WinnerId, gameForRepo.OpponentId, gameForRepo.Stage);
            if (gameFromRepo != null)
            {
                if (gameFromRepo.League.Rounds > gameFromRepo.Round)
                {
                    int roundsFromRepo = gameFromRepo.Round;
                    gameForRepo.Round = roundsFromRepo += 1;
                }
                else
                    return BadRequest("Rounds already completed");
            }
            else
                gameForRepo.Round = 1;

            gameForRepo.Overs = Math.Truncate(100 * gameForRepo.Overs) / 100;
            gameForRepo.OpponentOvers = Math.Truncate(100 * gameForRepo.OpponentOvers) / 100;
            gameForRepo.IsApproved = false;
            gameForRepo.WinnerGroup = "N/A";
            gameForRepo.WinnerGroup = "N/A";
            gameForRepo.CreatedBy = user.Username;
            gameForRepo.CreatedOn = DateTime.Now;
            _repo.Add(gameForRepo);

            if (await _repo.SaveAll())
                return Ok();

            return BadRequest("Failed to save game");
        }

        // Get Games
        [HttpGet]
        public async Task<IActionResult> GetGames([FromQuery]UserParams userParams)
        {
            var games = await _repo.GetGames(userParams);

            var gamesToReturn = _mapper.Map<IEnumerable<GameForListDto>>(games);
            Response.AddPagination(games.CurrentPage, games.PageSize,
                games.TotalCount, games.TotalPages);

            return Ok(gamesToReturn);
        }
    }
}