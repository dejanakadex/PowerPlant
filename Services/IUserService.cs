using PowerPlant.Models;

namespace PowerPlant.Services
{
    public interface IUserService
    {
        public Task<string> Login(LoginModel model);
        public Task Register(RegisterModel model);
    }
}
