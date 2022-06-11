using FilmstudionAPI.Models.FilmStudio;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmstudionAPI.Data.Repositories
{
    public class FilmStudioRepository : IFilmStudioRepository
    {
        readonly FilmstudionContext filmstudionContext;

        public FilmStudioRepository(FilmstudionContext _filmstudionContext)
        {
            filmstudionContext = _filmstudionContext;
        }

        public void Add(FilmStudio filmStudio)
        {
             filmstudionContext.FilmStudios.Add(filmStudio);     
        }

        public async Task<FilmStudio[]> GetAllFilmStudiosAsync()
        {
            return await filmstudionContext.FilmStudios.ToArrayAsync();
            //throw new System.NotImplementedException();
        }

        public async Task<FilmStudio> GetFilmStudioAsync(string name)
        {
            return await filmstudionContext.FilmStudios
                //.Include(f => f.RentedFilmCopies)
                .FirstOrDefaultAsync(f => f.Name.ToLower() == name.ToLower());
        }

        public async Task<FilmStudio> GetFilmStudioByIdAsync(int id)
        {
            return await filmstudionContext.FilmStudios
                .Where(f => f.FilmStudioId == id)
                .FirstOrDefaultAsync();
            //throw new System.NotImplementedException();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return(await filmstudionContext.SaveChangesAsync()) > 0;
        }

        public FilmStudio GetFilmStudio(int filmStudioId)
        {
            var filmStudio = filmstudionContext.FilmStudios.Find(filmStudioId);
            if (filmStudio == null)
                throw new KeyNotFoundException("Could not find filmstudio");
            return filmStudio;
        }
    }
}
