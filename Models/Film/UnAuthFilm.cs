using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FilmstudionAPI.Models.Film
{
    public class UnAuthFilm : IFilm
    {
        public int FilmId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }
        public string Director { get; set; }
        [JsonIgnore]
        public List<Filmcopy.FilmCopy> FilmCopies { get; set; }
    }
}
