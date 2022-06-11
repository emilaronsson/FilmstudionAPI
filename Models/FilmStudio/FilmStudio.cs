using System.Collections.Generic;

namespace FilmstudionAPI.Models.FilmStudio
{
    public class FilmStudio : IFilmStudio
    {
        public int FilmStudioId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public List<Filmcopy.FilmCopy> RentedFilmCopies { get; set; }
    }
}
