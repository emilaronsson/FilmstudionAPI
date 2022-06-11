using FilmstudionAPI.Models.FilmStudio;
using System.Threading.Tasks;

namespace FilmstudionAPI.Data.Repositories
{
    public interface IFilmStudioRepository
    {
        void Add(FilmStudio filmStudio);
        Task<FilmStudio[]> GetAllFilmStudiosAsync();
        Task<FilmStudio> GetFilmStudioAsync(string name);
        Task<FilmStudio> GetFilmStudioByIdAsync(int id);
        Task<bool> SaveChangesAsync();
        public FilmStudio GetFilmStudio(int filmStudioId);
    }
}
