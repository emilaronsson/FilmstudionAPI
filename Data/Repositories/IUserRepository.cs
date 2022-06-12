using FilmstudionAPI.Models.User;
using System.Threading.Tasks;

namespace FilmstudionAPI.Data.Repositories
{
    public interface IUserRepository
    {
        Task<User> Register(UserRegisterRequest userRegister);
        Task<User> GetUserByName(string username);

        Task<User> Authenticate(string username, string password);
        Task<bool> SaveChangesAsync();
    }
}
