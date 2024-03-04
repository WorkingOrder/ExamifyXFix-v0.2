using ExamifyX.ViewModel;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamifyX.Model.Services
{
    public class UserService
    {
        private readonly MyDbContext _dbContext;

        public UserService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddUser(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            if (user.UserRole == UserRole.Student)
            {
                //Logic to add user to Student table
                var student = new Student { UserId = user.Id };
                _dbContext.Students.Add(student);
            }
            if (user.UserRole == UserRole.Teacher)
            {
                //Logic to add user to Teacher table
                var teacher = new Teacher { UserId = user.Id };
                _dbContext.Teachers.Add(teacher);
            }

            _dbContext.SaveChanges();
        }

        public bool IsUsernameAvailable(string username)
        {
            return !_dbContext.Users.Any(u => u.Username == username);
        }

        public bool IsEmailAvailable(string email)
        {
            return !_dbContext.Users.Any(e => e.Email == email);
        }
    }
}