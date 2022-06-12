using System.ComponentModel.DataAnnotations;

namespace FilmstudionAPI.Models.FilmStudio
{
    public class RegisterFilmStudio : IRegisterFilmStudio
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string City { get; set; }
    }
}
