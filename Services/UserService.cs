using Microsoft.EntityFrameworkCore;
using PowerPlant.Entities;
using PowerPlant.Infrastructure;
using PowerPlant.Models;

namespace PowerPlant.Services
{
    public class UserService : IUserService
    {
        private readonly IJwtTools _jwtTools;
        private readonly DataContext _dataContext;

        public UserService(IJwtTools jwtTools, DataContext dataContext)
        {
            _jwtTools = jwtTools;
            _dataContext = dataContext;
        }

        public async Task<string> Login(LoginModel model)
        {
            User? userEntity = await _dataContext.Users.Where(x => x.Username == model.UserName).FirstOrDefaultAsync();

            if (userEntity != null)
            {
                var verify = PasswordHashingHelper.ArePasswordsEqual(model.Password!, userEntity.PasswordHash!);

                if (verify)
                {
                    var token = _jwtTools.GenerateToken(userEntity);
                    return token;
                }
            }

            throw new AppException("Username or password is incorrect");
        }

        public async Task Register(RegisterModel model)
        {
            User? userEntity = await _dataContext.Users.Where(x => x.Username == model.Username).FirstOrDefaultAsync();

            if (userEntity != null)
            {
                throw new AppException("Username already in use.");
            }

            var hashed = PasswordHashingHelper.HashPassword(model.Password!);

            var res = await _dataContext.AddAsync(new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Username = model.Username!,
                PasswordHash = hashed.passwordHash,
                Salt = hashed.salt
            });

            _dataContext.SaveChanges();
        }

    }
}
