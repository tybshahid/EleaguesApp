using System;
using System.Collections.Generic;

namespace EleaguesApp.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string KnownAs { get; set; }
        public string PSN { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
        public string PhotoUrl { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public virtual ICollection<Game> Winners { get; set; }
        public virtual ICollection<Game> Opponents { get; set; }
        public User()
        {
            CreatedBy = "System";
            CreatedOn = DateTime.Now;
        }
    }
}