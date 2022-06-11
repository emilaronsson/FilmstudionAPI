using System.ComponentModel.DataAnnotations;

namespace FilmstudionAPI.Models.FilmStudio
{
    public class RegisterFilmStudio : IRegisterFilmStudio
    {
        [Required]
        [MinLength(2), MaxLength(15)]
        public string Name { get; set; }

        [Required]
        [MinLength(2), MaxLength(20)]
        public string City { get; set; }
    }
}
