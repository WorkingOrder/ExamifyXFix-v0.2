using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamifyX.Model
{
	//A class defining an exam(saved to database)
    public class Exam
    {
		[Key]
		public int ExamId { get; set; }
		public string TestName { get; set; }
		public string Subject { get; set; }
		public string TeacherName { get; set; }
		public string Duration { get; set; }
		public DateTime PublishDate { get; set; } // Or separate Date and Time properties if needed
												  // Other relevant properties like 'IsActive', 'IsPublished' etc.

		// Navigation property to Questions
		public virtual ICollection<Question> Questions { get; set; }

		public Exam()
		{
			Questions = new HashSet<Question>();
		}
	}
}
