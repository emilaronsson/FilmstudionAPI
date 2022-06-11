using FilmstudionAPI.Models.User;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System;

namespace FilmstudionAPI.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        readonly FilmstudionContext context;
        readonly IMapper mapper;

        public UserRepository(FilmstudionContext _context, IMapper _mapper)
        {
            context = _context;
            mapper = _mapper;
        }
        public void AddUser(User user)
        {
            context.Users.Add(user);
        }

        public Task<User> Register(UserRegisterRequest userRegister)
        {
            var user = mapper.Map<UserRegisterRequest, User>(userRegister);
            user.Role = "Admin";
            user.UserId = Guid.NewGuid().ToString();

            context.Users.Add(user);
            return Task.FromResult(user);

        }
        public async Task<User> Authenticate(string username, string password)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }

        public async Task<User> GetUserByName(string username)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return(await context.SaveChangesAsync()) >0;
        }
    }
}
