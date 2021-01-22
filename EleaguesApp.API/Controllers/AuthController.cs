using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using EleaguesApp.API.Data;
using EleaguesApp.API.Dtos;
using EleaguesApp.API.Helpers;
using EleaguesApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EleaguesApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;
        public AuthController(IAuthRepository repo, IConfiguration config, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _cloudinaryConfig = cloudinaryConfig;
            _mapper = mapper;
            _config = config;
            _repo = repo;

            Account acc = new Account(
               _cloudinaryConfig.Value.CloudName,
               _cloudinaryConfig.Value.ApiKey,
               _cloudinaryConfig.Value.ApiSecret
           );

            _cloudinary = new Cloudinary(acc);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm]UserForRegisterDto userForRegisterDto)
        {
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (await _repo.UserExists(userForRegisterDto.Username, userForRegisterDto.KnownAs, userForRegisterDto.PSN))
                return BadRequest("User already exists");

            var file = userForRegisterDto.File;
            var uploadResult = new ImageUploadResult();
            if (file != null && file.Length > 0)
            {
                if (new ImageHelper().Validate(file))
                {
                    using (var stream = file.OpenReadStream())
                    {
                        var uploadParams = new ImageUploadParams
                        {
                            File = new FileDescription(file.Name, stream),
                            Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                        };
                        uploadResult = _cloudinary.Upload(uploadParams);
                    }

                    userForRegisterDto.PhotoUrl = uploadResult.Uri.ToString();
                    // userForRegisterDto.PublicId = uploadResult.PublicId;
                }
                else
                    return BadRequest("Invalid File");
            }

            var userToCreate = _mapper.Map<User>(userForRegisterDto);
            userToCreate.IsActive = false;

            var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);

            var userToReturn = _mapper.Map<UserForDetailedDto>(createdUser);

            return CreatedAtRoute("GetUser", new
            {
                controller = "Users",
                id = createdUser.Id
            }, userToReturn);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            // new TwitterHandler().TweetText("Test Successful via New App");

            var userFromRepo = await _repo.Login(userForLoginDto.Username
                .ToLower(), userForLoginDto.Password);

            if (userFromRepo == null)
                return BadRequest("Invalid Credentials");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var user = _mapper.Map<UserForDetailedDto>(userFromRepo);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                user
            });
        }
    }
}