using ExamifyX.Model;
using ExamifyX.Model.Commands;
using ExamifyX.Model.Services;
using ExamifyX.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ExamifyX.ViewModel
{
	//The view model of the Teacher main menu where the teacher can navigate to different views and
	//functions of the view
	public class TeacherMainMenuViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;
		public event Action OpenExamCreationRequested;
		public event Action OpenExamStatusRequested;
		public event Action LogOutRequested;

		public ICommand OpenExamCreationCommand { get; private set; }
		public ICommand OpenExamStatusCommand { get; private set; }
		public ICommand LogOutCommand { get; private set; }

		private readonly UserService _userService;
		private readonly LoginService _loginService;

		private string? _greeting;
		public string? Greeting
		{
			get => _greeting;
			set
			{
				_greeting = value;
				OnPropertyChanged(nameof(Greeting));
			}
		}



		public ICommand QuitCommand { get; }

		public TeacherMainMenuViewModel(UserService userService,LoginService loginService)
		{
			_userService = userService;
			_loginService = loginService;

			var userName = SessionContext.UserName;
			var userSurname = SessionContext.UserSurname;

			Greeting = $"Welcome, {userName}, {userSurname}";

			OpenExamCreationCommand = new RelayCommand(ExecuteOpenExamCreation);
			OpenExamStatusCommand  = new RelayCommand(ExecuteOpenExamStatus);
			LogOutCommand = new RelayCommand(ExecuteLogOut);
			QuitCommand = new RelayCommand(QuitApplication);
		}

		private void ExecuteOpenExamCreation()
		{
			OpenExamCreationRequested?.Invoke();
		}

		private void ExecuteOpenExamStatus()
		{
			OpenExamStatusRequested?.Invoke();
		}

		private void ExecuteLogOut()
		{
			LogOutRequested?.Invoke();
		}

		private void QuitApplication()
		{
			Application.Current.Shutdown() ;
		}
		
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
