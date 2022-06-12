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
                    Title = "Tenet",
                    Description = "Armed with only one word, Tenet, and fighting for the survival of the entire world, a Protagonist journeys through a twilight world of international espionage on a mission that will unfold in something beyond real time.",
                    Director = "Cristopher Nolan",
                    Country = "United States"
                },
                new Film
                {
                    FilmId = 2,
                    Title = "Dunkirk",
                    Description = "Allied soldiers from Belgium, the British Commonwealth and Empire, and France are surrounded by the German Army and evacuated during a fierce battle in World War II.",
                    Director = "Cristopher Nolan",
                    Country = "United Kingdom"
                },
                new Film
                {
                    FilmId = 3,
                    Title = "Interstellar",
                    Description = "A team of explorers travel through a wormhole in space in an attempt to ensure humanity's survival.",
                    Director = "Cristopher Nolan",
                    Country = "United States"
                },
                new Film
                {
                    FilmId = 4,
                    Title = "Inception",
                    Description = "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O., but his tragic past may doom the project and his team to disaster.",
                    Director = "Cristopher Nolan",
                    Country = "United States"
                },
                new Film
                {
                    FilmId = 5,
                    Title = "The Prestige",
                    Description = "After a tragic accident, two stage magicians in 1890s London engage in a battle to create the ultimate illusion while sacrificing everything they have to outwit each other.",
                    Director = "Cristopher Nolan",
                    Country = "United Kingdom"
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
