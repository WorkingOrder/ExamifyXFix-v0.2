using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamifyX.Model.Services
{
    //A class that manages the login process in the application
	public class LoginService
	{
        //reference to the database
		private readonly MyDbContext _dbContext;

        //constructor
        public LoginService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //a process that authenticates the user meaning if everything is filled correctly and the user
        //is able to enter their account
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

        //check for the username
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
			return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
		}

        //checks for the whole account and decrypts the password
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