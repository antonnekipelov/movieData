using RecomendMovie.Models;

namespace RecomendMovie.Services
{
    public interface IUserService
    {
        User Register(string username, string password);
        User Authenticate(string username, string password);
    }
}
