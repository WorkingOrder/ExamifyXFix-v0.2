using ExamifyX.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;

namespace ExamifyX.Model.Services
{
	/// <summary>
	/// This UserService class is part of a service layer in an application,
    /// which encapsulates business logic,
    /// particularly around user management.
    /// It interacts with a database context (MyDbContext) to perform operations related to User entities
	/// </summary>
	public class UserService
    {
		//_dbContext: An instance of MyDbContext, which is used to interact with the database.
		private readonly MyDbContext _dbContext;

		//Accepts an instance of MyDbContext and assigns it to _dbContext.
        //This setup allows the service to perform database operations.
		public UserService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

		/// <summary>
		/// Takes a User object as a parameter and adds it to the Users DbSet in the context. It saves changes to the database immediately afterward.
		///If the user's role is Student, it also creates a new Student entity, sets the UserId property to the ID of the newly added user, and adds this entity to the Students DbSet.
		///Similarly, if the user's role is Teacher, it creates a new Teacher entity, links it to the user, and adds it to the Teachers DbSet.
		///Calls _dbContext.SaveChanges() again to commit any changes made after the user is added.This approach means there are two calls to SaveChanges(), which could be optimized to a single call after all changes are made to reduce database transactions.
		/// </summary>
		/// <param name="user"></param>
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
		//Checks if username is taken or not
		public bool IsUsernameAvailable(string username)
		{
			return !_dbContext.Users.Any(u => u.Username == username);
		}
		//Checks if email is taken or not
		public bool IsEmailAvailable(string email)
		{
			return !_dbContext.Users.Any(e => e.Email == email);
		}
    }
}