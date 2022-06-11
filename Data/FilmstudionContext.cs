using Microsoft.EntityFrameworkCore;
using System;
using FilmstudionAPI.Models.Film;
using FilmstudionAPI.Models.FilmStudio;
using FilmstudionAPI.Models.User;
using FilmstudionAPI.Models.Filmcopy;

namespace FilmstudionAPI.Data
{
    public class FilmstudionContext : DbContext
    {
        public FilmstudionContext(DbContextOptions<FilmstudionContext> options) : base(options)
        {
            this.Database.EnsureCreated();
        }

        public DbSet<Film> Films { get; set; }
        public DbSet<FilmStudio> FilmStudios { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            string newAdminId = Guid.NewGuid().ToString();

            builder.Entity<FilmStudio>().HasData(
                new FilmStudio
                {
                    FilmStudioId = 1,
                    Name = "Teststudion",
                    City = "Los Angeles"
                },
                new FilmStudio
                {
                    FilmStudioId = 2,
                    Name = "Studiohej",
                    City = "Stockholm"
                });

            builder.Entity<User>().HasData(
                new User
                {
                    UserId = newAdminId,
                    Username = "admin",
                    Password = "admin",
                    Role = "Admin"
                },
                new User
                {
                    UserId = Guid.NewGuid().ToString(),
                    FilmStudioId = 1,
                    Username = "studio",
                    Password = "studio",
                    Role = "filmstudio"
                });

            builder.Entity<Film>().HasData(
                new Film
                {
                    FilmId = 1,
                    Title = "",
                    Description = "",
                    Director = "",
                    Country = ""
                },
                new Film
                {
                    FilmId = 2,
                    Title = "",
                    Description = "",
                    Director = "",
                    Country = ""
                },
                new Film
                {
                    FilmId = 3,
                    Title = "",
                    Description = "",
                    Director = "",
                    Country = ""
                },
                new Film
                {
                    FilmId = 4,
                    Title = "",
                    Description = "",
                    Director = "",
                    Country = ""
                },
                new Film
                {
                    FilmId = 5,
                    Title = "",
                    Description = "",
                    Director = "",
                    Country = ""
                }
                );

            builder.Entity<FilmCopy>().HasData(
               new FilmCopy
               {
                   FilmCopyId = 1,
                   FilmId = 1,
                   RentedOut = false
               }, new FilmCopy
               {
                   FilmCopyId = 2,
                   FilmId = 1,
                   RentedOut = false
               }, new FilmCopy
               {
                   FilmCopyId = 3,
                   FilmId = 2,
                   RentedOut = true,
                   FilmStudioId = 1
               }
           );
        }
    }
}
