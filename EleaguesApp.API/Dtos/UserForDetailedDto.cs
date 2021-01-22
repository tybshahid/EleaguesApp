using System;

namespace EleaguesApp.API.Dtos
{
    public class UserForDetailedDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string KnownAs { get; set; }
        public string PSN { get; set; }
        public int Age { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool isAdmin { get; set; }
        public bool isActive { get; set; }
    }
}