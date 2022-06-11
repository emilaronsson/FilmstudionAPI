using System.ComponentModel.DataAnnotations;

namespace FilmstudionAPI.Models.User
{
    public class UserRegister : IUserRegister
    {
        
            public bool IsAdmin { get; set; } = false;

            [Required]
            public string Username { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            //[Required]
            //[DataType(DataType.Password)]
            //[Compare("Password")]
            //public string ConfirmPassword { get; set; }
        
    }
}
