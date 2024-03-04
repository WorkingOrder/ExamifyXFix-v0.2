using ExamifyX.Model;
using ExamifyX.Model.Services;
using ExamifyX.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExamifyX.View
{
	/// <summary>
	/// Interaction logic for TeacherMainMenuView.xaml
	/// </summary>
	public partial class TeacherMainMenuView : UserControl
	{

		private readonly UserService _userService;
		private readonly LoginService _loginService;

		public TeacherMainMenuView(UserService userService, LoginService loginService)
		{
			_userService = userService;
			_loginService = loginService;

			var userName = SessionContext.UserName;
			var userSurname = SessionContext.UserSurname;


			InitializeComponent();
			GreetingLabel.Content = $"Welcome, {userName} {userSurname}";
		}

	}
}
