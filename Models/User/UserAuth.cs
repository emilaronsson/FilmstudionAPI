using System.ComponentModel.DataAnnotations;


namespace FilmstudionAPI.Models.User
{
    public class UserAuth : IUserAuth
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
