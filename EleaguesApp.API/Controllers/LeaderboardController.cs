using System.Threading.Tasks;
using EleaguesApp.API.Data;
using EleaguesApp.API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace EleaguesApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {
        private readonly IEleaguesRepository _repo;
        public LeaderboardController(IEleaguesRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult GetLeaderboard([FromQuery]UserParams userParams)
        {
            var result = _repo.GetLeaderboard(userParams);
            Response.AddPagination(result.CurrentPage, result.PageSize,
                result.TotalCount, result.TotalPages);

            return Ok(result);
        }
    }
}