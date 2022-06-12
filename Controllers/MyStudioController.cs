using FilmstudionAPI.Data.Repositories;
using FilmstudionAPI.Models.Filmcopy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FilmstudionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyStudioController : ControllerBase
    {
        readonly IUserRepository userRepository;
        readonly IFilmStudioRepository filmStudioRepository;

        public MyStudioController(IUserRepository _userRepository, IFilmStudioRepository _filmStudioRepository)
        {
            userRepository = _userRepository;
            filmStudioRepository = _filmStudioRepository;
        }

        [HttpGet("rentals")]
        [Authorize]
        public async Task<ActionResult<IFilmCopy[]>> GetRetals()
        {
            try
            {
                var user = await userRepository.GetUserByName(User.Identity.Name);

                if(user.Role != "filmstudio")
                {
                    return Unauthorized("You are not a filmstudio");
                }

                var filmStudio = await filmStudioRepository.GetFilmStudioByIdAsync(user.FilmStudioId);

                if(filmStudio == null)
                {
                    return NotFound("Filmstudio not found");
                }
                return Ok(filmStudio.RentedFilmCopies.ToArray());
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
    }
}
