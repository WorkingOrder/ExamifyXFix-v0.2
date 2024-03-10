using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamifyX.Model
{
	//A class made for Depicting a teacher(saved to database)
	public class Teacher
	{
		public int TeacherId { get; set; }
		public int UserId { get; set; }
		public User? User { get; set; }
	}
}
