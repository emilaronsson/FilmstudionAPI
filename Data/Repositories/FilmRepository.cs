using FilmstudionAPI.Models.Film;
using FilmstudionAPI.Models.Filmcopy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace FilmstudionAPI.Data.Repositories
{
    public class FilmRepository : IFilmRepository
    {
        readonly FilmstudionContext context;

        public FilmRepository(FilmstudionContext _context)
        {
            context = _context;
        }

        public void Add(Film film)
        {
            context.Add(film);
        }

        public async Task<Film[]> GetAllFilms()
        {
            return await context.Films
                .Include(f => f.FilmCopies)
                .ToArrayAsync();
        }

        public async Task<Film> GetFilmAsync(string title)
        {
            return await context.Films
                .Include(f => f.FilmCopies)
                .FirstOrDefaultAsync(f => f.Title.ToLower() == title.ToLower());
        }

        public async Task<Film> GetFilmByIdAsync(int id)
        {
            return await context.Films
                .Include(f => f.FilmCopies)
                .FirstOrDefaultAsync(f => f.FilmId == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await context.SaveChangesAsync()) > 0;
        }

        public async Task<bool> UpdateAsync(UpdateFilm film)
        {
            var result = await context.Films
                .FirstOrDefaultAsync(f => f.FilmId == film.FilmId);

            if (result != null)
            {
                if (!String.IsNullOrEmpty(film.Title))
                    result.Title = film.Title;

                if (!String.IsNullOrEmpty(film.Description))
                    result.Description = film.Description;

                if (!String.IsNullOrEmpty(film.Country))
                    result.Country = film.Country;

                if (!String.IsNullOrEmpty(film.Director))
                    result.Director = film.Director;

                return (await context.SaveChangesAsync()) > 0;
            }
            else return false;
        }
    }
}
