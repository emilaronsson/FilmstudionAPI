using System.Text.Json.Serialization;

namespace FilmstudionAPI.Models.User
{
    public class RegisterUnAuthUser : IUser
    {
        public string UserId { get; set;}
        public string Username { get; set;}
        public string Role { get; set;}
        [JsonIgnore]
        public string Password { get; set;}
        [JsonIgnore]
        public int FilmStudioId { get; set;}
        [JsonIgnore]
        public FilmStudio.FilmStudio FilmStudio { get; set;}
        [JsonIgnore]
        public string Token { get; set;}
    }
}
