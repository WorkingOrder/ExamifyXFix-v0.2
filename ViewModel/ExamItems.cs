using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamifyX.ViewModel
{
	//a class made for the items that are needed to me shown in an exam
	public class ExamItems
	{
		public int ExamId { get; set; }
		public string TestName { get; set; }
		public DateTime TestDate { get; set; }
		public string Duration { get; set; }
		public string TestSubject { get; set; }
		public string TestTeacher { get; set; }
		public bool IsPublished { get; set; }
		public string Status { get; set; }
	}
}
