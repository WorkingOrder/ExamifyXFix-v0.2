using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamifyX.Model
{
	//A class depicting the structure of Student(saved to database)
	public class Student
	{
		public int StudentId { get; set; }
		public int UserId { get; set; }
		public User? User { get; set; }
	}
}
