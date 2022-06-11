using System;
using System.ComponentModel.DataAnnotations;

namespace FilmstudionAPI.Models.Film
{
    public interface ICreateFilm
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string Director { get; set; }
        [Required]
        public int NumberOfCopies { get; set; }
    }
}
