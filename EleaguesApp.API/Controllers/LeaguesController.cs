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
    public class LeaguesController : ControllerBase
    {
        private readonly IEleaguesRepository _repo;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public LeaguesController(IEleaguesRepository repo, IMapper mapper, IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetLeagues([FromQuery]UserParams userParams)
        {
            var leagues = await _repo.GetLeagues(userParams);
            // var leaguesToReturn = _mapper.Map<IEnumerable<LeagueForListDto>>(leagues);

            Response.AddPagination(leagues.CurrentPage, leagues.PageSize,
                 leagues.TotalCount, leagues.TotalPages);

            return Ok(leagues);
        }

        [Authorize, HttpGet("leaguesList")]
        public async Task<IActionResult> GetLeaguesList()
        {
            var user = await _repo.GetUser(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            if (user == null || !user.IsAdmin)
                return Unauthorized();

            var leagues = await _repo.GetLeaguesList();
            return Ok(leagues);
        }

        [Authorize, HttpGet("{id}", Name = "GetLeague")]
        public async Task<IActionResult> GetLeague(int id)
        {
            var user = await _repo.GetUser(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            if (user == null || !user.IsAdmin)
                return Unauthorized();

            if (id == 0)
                return Ok(new League());
            var league = await _repo.GetLeague(id);
            return Ok(league);
        }

        [Authorize, HttpPost]
        public async Task<IActionResult> SaveLeague([FromForm]League leagueForRepo, IFormFile file)
        {
            var user = await _repo.GetUser(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            if (user == null || !user.IsAdmin)
                return Unauthorized();

            leagueForRepo.RulesFileName = "";
            if (file != null && file.Length > 0)
            {

                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, @"Uploads\Rules", fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                leagueForRepo.RulesFileName = fileName;
            }

            leagueForRepo.CreatedBy = user.Username;
            leagueForRepo.CreatedOn = DateTime.Now;

            if (leagueForRepo.Id == 0)
            {
                _repo.Add(leagueForRepo);
            }
            else
            {
                var leagueFromRepo = await _repo.GetLeague(leagueForRepo.Id);
                _mapper.Map(leagueForRepo, leagueFromRepo);
            }

            try
            {
                await _repo.SaveAll();
            }
            catch
            {
                throw new Exception("Failed on save");
            }

            return Ok();
        }
    }
}