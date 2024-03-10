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
    //The view model that represents the main menu of the student and its functions
	public class StudentMainMenuViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;

		public event Action LogOutRequested;
        public event Action TakeExamRequested;

		private readonly UserService _userService;
        private readonly LoginService _loginService;

        public ICommand QuitCommand { get; }
        public ICommand LogOutCommand { get; private set; }
        public ICommand TakeExamCommand { get; private set; }


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

        public StudentMainMenuViewModel(UserService userService, LoginService loginService)
        {
            _userService = userService;
            _loginService = loginService;

            var userName = SessionContext.UserName;
            var userSurname = SessionContext.UserSurname;

            Greeting = $"Welcome, {userName}, {userSurname}";

			LogOutCommand = new RelayCommand(ExecuteLogOut);
			QuitCommand = new RelayCommand(QuitApplication);
            TakeExamCommand = new RelayCommand(TakeExamFunction);
        }

		private void TakeExamFunction()
		{
			TakeExamRequested?.Invoke();
		}

		private void ExecuteLogOut()
		{
			LogOutRequested?.Invoke();
		}

		private void QuitApplication()
		{
			Application.Current.Shutdown();
		}

		protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
