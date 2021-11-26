using BlazorBattles.Client.Shared;
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

        public Task<ServiceResponse<string>> Login(string email, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<int>> Register(User user, string passsword)
        {
            if (await UserExist(user.Email))
            {
                return new ServiceResponse<int> {Success=false, Message="User already exist." };
            }

            CreatePasswordHash(passsword, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return new ServiceResponse<int> { Message = "Registration successful!", Data=user.Id };
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
