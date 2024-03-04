using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamifyX.Model.Services
{
	public class LoginService
	{
		private readonly MyDbContext _dbContext;

        public LoginService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> AuthenticateUser(string username, string password)
        {
            //Find the user by username
            var user = await _dbContext.Users.FirstOrDefaultAsync(u =>  u.Username == username);

            //If user not found or password does not match, return null

            if(user == null || !PasswordHasher.VerifyPassword(user.PasswordHash, password))
            {
                return null;
            }

            //User is authenticated
            //Update SessionContext with the user's actual details
            SessionContext.UserName = user.Name;
            SessionContext.UserSurname = user.Surname;


            return user;
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
			return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
		}

        public async Task<bool> ValidateCredentialsAsync(string username, string password)
        {
            var user = await GetUserByUsernameAsync(username);
            if(user != null)
            {
                return PasswordHasher.VerifyPassword(user.PasswordHash, password);
            }
            return false;
        }
    }
}