using System;

namespace EleaguesApp.API.Dtos
{
    public class UserForListDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int Age { get; set; }
        public string KnownAs { get; set; }
        public string PSN { get; set; }
        public string Country { get; set; }
        public DateTime CreatedOn { get; set; }
        public string PhotoUrl { get; set; }
    }
}