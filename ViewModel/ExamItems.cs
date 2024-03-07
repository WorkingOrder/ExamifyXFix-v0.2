﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamifyX.ViewModel
{
	public class ExamItems
	{
		public string TestName { get; set; }
		public DateTime TestDate { get; set; }
		public string Duration { get; set; }
		public string TestSubject { get; set; }
		public string TestTeacher { get; set; }
		public bool IsPublished { get; set; }
		public string Status { get; set; }
	}
}
