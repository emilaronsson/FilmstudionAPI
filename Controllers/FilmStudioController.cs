using FilmstudionAPI.Models.FilmStudio;
using FilmstudionAPI.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using AutoMapper;
using System;
using System.Collections.Generic;

namespace FilmstudionAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilmStudioController : ControllerBase
    {
        readonly IFilmStudioRepository filmStudioRepository;
        readonly IMapper mapper;
        readonly IUserRepository userRepository;

        public FilmStudioController(IFilmStudioRepository _filmStudioRepository, IMapper _mapper, IUserRepository _userRepository)
        {
            filmStudioRepository = _filmStudioRepository;
            mapper = _mapper;
            userRepository = _userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {

            try
            {

                var filmstudios = await filmStudioRepository.GetAllFilmStudiosAsync();
                if (filmstudios == null) return NotFound("Filmstudio not found");
                

                if(User.Identity.Name != null)
                {
                    var user = await userRepository.GetUserByName(User.Identity.Name);
                    if(user == null) return NotFound("User not found");
                    if(user.Role == "Admin")
                    {
                        return Ok(filmstudios);
                    }
                }
                
                return Ok(mapper.Map<FilmStudio[], UnAuthFilmStudio[]>(filmstudios));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<IFilmStudio>> Post(RegisterFilmStudio registerFilmStudio)
        {
            try
            {
                    var existing = await filmStudioRepository.GetFilmStudioAsync(registerFilmStudio.Name);

                    if (existing != null) return BadRequest("Filmstudio already exists");

                    FilmStudio filmStudio = mapper.Map<RegisterFilmStudio, FilmStudio>(registerFilmStudio);

                    filmStudioRepository.Add(filmStudio);

                    if (await filmStudioRepository.SaveChangesAsync())
                    {
                        return Ok(filmStudio);
                    }
                return BadRequest();
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FilmStudio>> Get(int id)
        {
            try
            {
                var filmStudio = await filmStudioRepository.GetFilmStudioByIdAsync(id);
                if (filmStudio == null) return NotFound("Filmstudio not found");

                if(User.Identity.Name != null)
                {
                    var user = await userRepository.GetUserByName(User.Identity.Name);
                    if(user == null) return NotFound("User not found");

                    if(user.Role == "Admin" || user.FilmStudioId == filmStudio.FilmStudioId)
                    {
                        return Ok(filmStudio);
                    }
                }

                return Ok(mapper.Map<FilmStudio, UnAuthFilmStudio>(filmStudio));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
        
    }
}
