using EleaguesApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EleaguesApp.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<League> Leagues { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasMany(c => c.Winners)
                .WithOne(e => e.Winner)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User>()
                .HasMany(c => c.Opponents)
                .WithOne(e => e.Opponent)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}