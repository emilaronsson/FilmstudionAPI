using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FilmstudionAPI.Models.FilmStudio
{
    public class UnAuthFilmStudio : IFilmStudio
    {
        public int FilmStudioId { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public string City { get; set; }

        [JsonIgnore]
        public List<Filmcopy.FilmCopy> RentedFilmCopies { get; set; }
    }
}
