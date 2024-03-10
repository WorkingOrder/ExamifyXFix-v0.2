using ExamifyX.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamifyX.Model
{
	//A class that saves users to database
	public class User
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? Surname { get; set; }
		public string? Email { get; set; }
		public string? Username { get; set; }
		[NotMapped]
		public string? Password { get; set; }
		public string? PasswordHash { get; set; }
		[NotMapped]
		public string? ConfirmPassword { get; set; }
		public UserRole UserRole { get; set; }
	}
}
