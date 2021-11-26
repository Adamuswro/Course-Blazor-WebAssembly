using BlazorBattles.Shared;
using System.Threading.Tasks;

namespace BlazorBattles.Server.Data
{
    public interface IAuthRepository
    {
        Task<int> Register(User user, string passsword);
        Task<string> Login(string email, string password);
        Task<bool> UserExist(string email);
    }
}
