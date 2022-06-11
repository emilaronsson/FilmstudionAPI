using System.ComponentModel.DataAnnotations;

namespace FilmstudionAPI.Models.User
{
    public class UserRegisterRequest : IUserRegister
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public bool IsAdmin { get; set; }

    }
}
