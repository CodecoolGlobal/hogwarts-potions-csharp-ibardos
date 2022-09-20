using HogwartsPotions.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HogwartsPotions.Data
{
    public class HogwartsContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Room> Rooms { get; set; }

        public HogwartsContext(DbContextOptions<HogwartsContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<Room>().ToTable("Room");
        }
    }
}
