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
            //var user = (User)HttpContext.Items["User"];
            try
            {
                var films = await filmRepository.GetAllFilms();
                //if(user == null)
                //{
                //    return Ok(mapper.Map < Film[], UnAuthFilm[] > (films));
                //}
                //else if(user != null)
                //{
                //    return Ok(films);
                //}


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
            //return BadRequest();
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
                    return StatusCode(StatusCodes.Status401Unauthorized, "Unauthorized");
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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPatch("{id}")]
        [Authorize]
        public async Task<ActionResult<Film>> Patch(int id, [FromBody] UpdateFilm updateFilm)
        {
            try
            {
                var user = await userRepository.GetUserByName(User.Identity.Name);
                if(user == null) return NotFound("User not found");

                if(user.Role == "Admin")
                {
                    var film = await filmRepository.GetFilmByIdAsync(id);
                    if (film == null) return BadRequest("Could not find film");

                    updateFilm.FilmId = film.FilmId;

                    if(await filmRepository.UpdateAsync(updateFilm))
                    {
                        return Ok(film);
                    }
                    return BadRequest("Update failed");
                }
                else
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, "Unauthorized");
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
    }
}
