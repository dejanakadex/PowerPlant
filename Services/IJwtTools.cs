using PowerPlant.Entities;

namespace PowerPlant.Services
{
    public interface IJwtTools
    {
        public string GenerateToken(User user);
    }
}
