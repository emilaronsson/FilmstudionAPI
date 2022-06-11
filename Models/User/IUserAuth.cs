namespace FilmstudionAPI.Models.User
{
    public interface IUserAuth
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
