using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamifyX.Model
{
	//A class made to save and greet the users name and surname upon log in
	public static class SessionContext
	{
		public static string? UserName { get; set; }
		public static string? UserSurname { get; set; }

		public static void Clear()
		{
			UserName = null;
			UserSurname = null;
		}
	}
}
