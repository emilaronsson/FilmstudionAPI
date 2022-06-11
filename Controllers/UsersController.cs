using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using FilmstudionAPI.Models.User;
using FilmstudionAPI.Data.Repositories;
using System.Threading.Tasks;
using System;
using FilmstudionAPI.Data.JwtAuth;
using Microsoft.AspNetCore.Http;
using FilmstudionAPI.Models.FilmStudio;

namespace FilmstudionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        readonly IMapper mapper;
        readonly IUserRepository userRepository;
        readonly IJwtAuthService jwtAuthService;
        readonly IFilmStudioRepository filmStudioRepository;

        public UsersController(IMapper _mapper, IUserRepository _userRepository, IJwtAuthService _jwtAuthService, IFilmStudioRepository _filmStudioRepository)
        {
            mapper = _mapper;
            userRepository = _userRepository;
            jwtAuthService = _jwtAuthService;
            filmStudioRepository = _filmStudioRepository;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserRegisterResponse>> Register(UserRegisterRequest userRegister)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    if(userRegister.IsAdmin)
                    {
                        var newUser = new User
                        {
                            Username = userRegister.Username,
                            Password = userRegister.Password,
                            UserId = Guid.NewGuid().ToString()
                        };
                        newUser = await userRepository.Register(userRegister);
                        return mapper.Map<UserRegisterResponse>(newUser);

                        //if (await userRepository.SaveChangesAsync())
                        //{
                        //    return Ok(newUser);

                        //    //await userRepository.AddUser(userRegister);
                        //}
                    }
                }
                catch(Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest("IsAdmin needs to be true");
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<IUser>> Authenticate(UserAuth userAuth)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var user = await userRepository.Authenticate(userAuth.Username, userAuth.Password);

                    if (user == null)
                        return BadRequest("Wrong credentials");

                    var token = jwtAuthService.GetToken(user);
                    user.Token = token;

                    if(user.Role == "filmstudio")
                    {
                        FilmStudio filmstudio = filmStudioRepository.GetFilmStudio(user.FilmStudioId);
                        user.FilmStudio = filmstudio;
                    }

                    if(await userRepository.SaveChangesAsync())
                    {
                        return Ok(mapper.Map<User, UnAuthUser>(user));
                    }
                    return BadRequest();
                }
                else
                {
                    return BadRequest(userAuth);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

    }
}
