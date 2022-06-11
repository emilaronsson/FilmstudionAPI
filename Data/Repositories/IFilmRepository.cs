using FilmstudionAPI.Models.Film;
using FilmstudionAPI.Models.Filmcopy;
using FilmstudionAPI.Models.FilmStudio;
using System.Threading.Tasks;

namespace FilmstudionAPI.Data.Repositories
{
    public interface IFilmRepository
    {
        void Add(Film film);
        Task<Film[]> GetAllFilms();
        Task<Film> GetFilmByIdAsync(int id);
        Task<Film> GetFilmAsync(string title);
        Task<bool> UpdateAsync(UpdateFilm film);
        Task<bool> SaveChangesAsync();
    }
}
