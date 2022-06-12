using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using FilmstudionAPI.Data;
using FilmstudionAPI.Models;
using FilmstudionAPI.Data.Repositories;
using System.Threading.Tasks;
using FilmstudionAPI.Models.Film;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using FilmstudionAPI.Models.User;
using System.Collections.Generic;
using FilmstudionAPI.Models.Filmcopy;
using System.Linq;

namespace FilmstudionAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FilmsController : ControllerBase
    {
        readonly IFilmRepository filmRepository;
        readonly IMapper mapper;
        readonly IFilmStudioRepository filmStudioRepository;
        readonly IUserRepository userRepository;

        public FilmsController(IFilmRepository _filmRepository, IMapper _mapper, IFilmStudioRepository _filmStudioRepository, IUserRepository _userRepository)
        {
            filmRepository = _filmRepository;
            mapper = _mapper;
            filmStudioRepository = _filmStudioRepository;
            userRepository = _userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IFilm>> Get()
        {
            try
            {
                var films = await filmRepository.GetAllFilms();

                if (User.Identity.IsAuthenticated)
                {
                    return Ok(films);
                }
                else
                {
                    return Ok(mapper.Map<Film[], UnAuthFilm[]>(films));
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult<ICreateFilm>> CreateFilm(CreateFilm createFilm)
        {
            try
            {
                var user = await userRepository.GetUserByName(User.Identity.Name);
                if (user == null) return NotFound("User not found");

                if (user.Role == "Admin")
                {
                    var existing = await filmRepository.GetFilmAsync(createFilm.Title);
                    if (existing != null)
                    {
                        return BadRequest("Film already exists");
                    }

                    Film film = mapper.Map<CreateFilm, Film>(createFilm);

                    var filmCopies = new List<FilmCopy>();
                    for (int i = 0; i < createFilm.NumberOfCopies; i++)
                    {
                        filmCopies.Add(new FilmCopy { });
                    }

                    filmRepository.Add(film);
                    film.FilmCopies = filmCopies;

                    if (await filmRepository.SaveChangesAsync())
                    {
                        return Ok(film);
                    }

                    return BadRequest();
                }
                else
                {
                    return Unauthorized("Unauthorized");
                }

            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IFilm>> Get(int id)
        {
            try
            {
                var film = await filmRepository.GetFilmByIdAsync(id);

                if (User.Identity.IsAuthenticated)
                {
                    return Ok(film);
                }
                else
                {
                    return Ok(mapper.Map<Film, UnAuthFilm>(film));
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPatch("{id}")]
        [Authorize]
        public async Task<ActionResult<Film>> EditFilm(int id, [FromBody] UpdateFilm updateFilm)
        {
            try
            {
                var user = await userRepository.GetUserByName(User.Identity.Name);
                if (user == null) return NotFound("User not found");

                if (user.Role == "Admin")
                {
                    var film = await filmRepository.GetFilmByIdAsync(id);
                    if (film == null) return BadRequest("Could not find film");

                    updateFilm.FilmId = film.FilmId;

                    if (await filmRepository.UpdateAsync(updateFilm))
                    {
                        return Ok(film);
                    }
                    return BadRequest("Update failed");
                }
                else
                {
                    return Unauthorized("Unauthorized");
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<Film>> EditNumberOFCopies(int id, [FromBody] UpdateFilm updateFilm)
        {
            var user = await userRepository.GetUserByName(User.Identity.Name);
            if (user == null) return NotFound("User not found");

            if (user.Role == "Admin")
            {
                var film = await filmRepository.GetFilmByIdAsync(id);
                if (film == null) return BadRequest("Film doesnt exist");

                var rentedFilms = film.FilmCopies.Where(f => f.RentedOut).ToList();

                if (rentedFilms.Count > updateFilm.NumberOfCopies)
                {
                    return BadRequest("Number of copies can't be lower than number of films already loaned out");
                }

                var addAmount = updateFilm.NumberOfCopies - rentedFilms.Count;
                for (int i = 0; i < addAmount; i++)
                {
                    rentedFilms.Add(new FilmCopy { FilmId = film.FilmId });
                }

                film.FilmCopies = rentedFilms;

                if (await filmRepository.SaveChangesAsync())
                {
                    return Ok(film);
                }

                return BadRequest("Could not Save");
            }
            else
            {
                return Unauthorized("Unauthorized");
            }
        }

        [HttpPost("rent")]
        [Authorize]
        public async Task<ActionResult> Rent(int filmId, int studioId)
        {
            try
            {
                var user = await userRepository.GetUserByName(User.Identity.Name);
                if (user == null) return NotFound("User not found");

                if (user.FilmStudioId == studioId)
                {
                    var film = await filmRepository.GetFilmByIdAsync(filmId);
                    var filmStudio = await filmStudioRepository.GetFilmStudioByIdAsync(studioId);

                    if (film == null)
                        return Conflict("Film does not exist");
                    if (filmStudio == null)
                        return Conflict("Filmstudio does not exist");

                    var currentlyRenting = filmStudio.RentedFilmCopies.FirstOrDefault(fc => fc.FilmId == filmId);
                    if (currentlyRenting != null) 
                        return Forbid("You already rent a copy of this film");

                    foreach (FilmCopy filmCopy in film.FilmCopies)
                    {
                        if (!filmCopy.RentedOut)
                        {
                            filmCopy.RentedOut = true;
                            filmCopy.FilmStudioId = filmStudio.FilmStudioId;
                            filmStudio.RentedFilmCopies.Add(filmCopy);

                            if (await filmRepository.SaveChangesAsync())
                            {
                                return Ok("Rented film Successfully");
                            }
                            else
                            {
                                return BadRequest("Unable to finalize rent request");
                            }
                        }

                    }
                    return Conflict("No available copies");
                }
                else
                {
                    return Unauthorized("Unauthorized");
                }
            }
            catch
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost("return")]
        [Authorize]
        public async Task<ActionResult> Return(int filmId, int studioId)
        {
            try
            {
                var user = await userRepository.GetUserByName(User.Identity.Name);
                if(user == null) return NotFound("User not found");

                if(user.FilmStudioId == studioId)
                {
                    var film = await filmRepository.GetFilmByIdAsync(filmId);
                    var filmStudio = await filmStudioRepository.GetFilmStudioByIdAsync(studioId);

                    if (film == null)
                        return Conflict("Film does not exist");
                    if (filmStudio == null)
                        return Conflict("Filmstudio does not exist");

                    var rented = filmStudio.RentedFilmCopies.FirstOrDefault(fc => fc.FilmId == filmId);
                    if (rented == null)
                        return Conflict("Film not rented");

                    filmStudio.RentedFilmCopies.Remove(rented);
                    rented.RentedOut = false;

                    if(await filmRepository.SaveChangesAsync())
                    {
                        return Ok("Film return successfull");
                    }
                    else
                    {
                        return BadRequest("Return unsuccessfull");
                    }
                }
                else
                {
                    return Unauthorized("Unautorized");
                }
            }
            catch
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
    }
}
