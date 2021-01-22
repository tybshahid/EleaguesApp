using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using EleaguesApp.API.Data;
using EleaguesApp.API.Dtos;
using EleaguesApp.API.Helpers;
using EleaguesApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EleaguesApp.API.Controllers
{
    // [ServiceFilter(typeof(LogUserActivity))]
    // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IEleaguesRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;
        public UsersController(IEleaguesRepository repo, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _cloudinaryConfig = cloudinaryConfig;
            _mapper = mapper;
            _repo = repo;

            Account acc = new Account(
              _cloudinaryConfig.Value.CloudName,
              _cloudinaryConfig.Value.ApiKey,
              _cloudinaryConfig.Value.ApiSecret
          );

            _cloudinary = new Cloudinary(acc);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);

            if (user == null)
                return BadRequest();

            var userToReturn = _mapper.Map<UserForDetailedDto>(user);
            return Ok(userToReturn);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            var users = await _repo.GetUsers(userParams);
            
            // foreach (var item in users)
            //     if (!item.IsActive)
            //         item.PhotoUrl = null;

            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
            Response.AddPagination(users.CurrentPage, users.PageSize,
                 users.TotalCount, users.TotalPages);

            return Ok(usersToReturn);
        }

        [Authorize, HttpPost("update")]
        public async Task<IActionResult> UpdateUser(
            [FromForm]int Id,
            [FromForm]string Password,
            [FromForm]bool IsActive,
            [FromForm]IFormFile File)
        {
            var user = await _repo.GetUser(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            if (user == null || !user.IsAdmin)
                return Unauthorized();

            // Uploading Photo to Cloudinary
            string photoUrl = "";
            var uploadResult = new ImageUploadResult();
            if (File != null && File.Length > 0)
            {
                if (new ImageHelper().Validate(File))
                {
                    using (var stream = File.OpenReadStream())
                    {
                        var uploadParams = new ImageUploadParams
                        {
                            //Proxy = "https://winproxyus1.server.lan:3128",
                            File = new FileDescription(File.Name, stream),
                            Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                        };
                        uploadResult = _cloudinary.Upload(uploadParams);
                    }

                    photoUrl = uploadResult.Uri.ToString();
                }
                else
                    return BadRequest("Invalid File");
            }

            if (Id != 0)
            {
                if (_repo.UpdateUser(Id, Password, IsActive, photoUrl, user.Username))
                {
                    if (await _repo.SaveAll())
                        return Ok();
                }
            }

            return BadRequest("Failed to update user");
        }
    }
}