using System.Collections.Generic;

namespace FilmstudionAPI.Models.Film
{
    public class UpdateFilm : IFilm
    {
        public int FilmId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }
        public string Director { get; set; }
        public List<Filmcopy.FilmCopy> FilmCopies { get; set; }
        public int NumberOfCopies { get; set; }
    }
}
