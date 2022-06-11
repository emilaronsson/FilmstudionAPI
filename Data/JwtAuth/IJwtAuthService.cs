using FilmstudionAPI.Models.User;

namespace FilmstudionAPI.Data.JwtAuth
{
    public interface IJwtAuthService
    {
        string GetToken(User user);
    }
}
