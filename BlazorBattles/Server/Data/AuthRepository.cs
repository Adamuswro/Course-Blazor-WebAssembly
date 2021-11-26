using BlazorBattles.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace BlazorBattles.Server.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext context;

        public AuthRepository(DataContext dataContext)
        {
            this.context = dataContext;
        }

        public Task<string> Login(string email, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Register(User user, string passsword)
        {
            if (await UserExist(user.Email))
            {
                return -1;
            }

            CreatePasswordHash(passsword, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return user.Id;
        }

        public async Task<bool> UserExist(string email)
        {
            if (await context.Users.AnyAsync(u=>u.Email.ToLower() == email.ToLower()))
            {
                return true;
            }

            return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
