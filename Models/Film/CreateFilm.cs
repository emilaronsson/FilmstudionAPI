namespace FilmstudionAPI.Models.Film
{
    public class CreateFilm : ICreateFilm
    {
        public string Title { get; set; }
        public string Country { get; set; }
        public string Director { get; set; }
        public int NumberOfCopies { get; set; }
    }
}
