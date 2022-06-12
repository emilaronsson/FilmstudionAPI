using AutoMapper;
using FilmstudionAPI.Models.Film;
using FilmstudionAPI.Models.FilmStudio;
using FilmstudionAPI.Models.User;

namespace FilmstudionAPI.Data.Mapper
{
    public class FilmStudionMapper : Profile
    {
        public FilmStudionMapper()
        {
            CreateMap<User, UnAuthUser>();

            CreateMap<FilmStudio, UnAuthFilmStudio>();

            CreateMap<RegisterFilmStudio, FilmStudio>();

            CreateMap<User, UserRegisterRequest>().ReverseMap();

            CreateMap<User, UserRegisterResponse>().ReverseMap();

            CreateMap<Film, UnAuthFilm>();

            CreateMap<UpdateFilm, Film>();

            CreateMap<CreateFilm, Film>();
        }
    }
}
