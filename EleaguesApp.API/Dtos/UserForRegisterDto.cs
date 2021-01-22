using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace EleaguesApp.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "You must specify password between 4 and 8 characters")]
        public string Password { get; set; }

        [Required]
        public string KnownAs { get; set; }

        [Required]
        public string PSN { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Mobile { get; set; }

        public string PhotoUrl { get; set; }

        public IFormFile File { get; set; }

        public bool IsActive { get; set; }

        public bool IsAdmin { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public UserForRegisterDto()
        {
            IsActive = true;
            IsAdmin = false;
            CreatedBy = "System";
            CreatedOn = DateTime.Now;
        }
    }
}