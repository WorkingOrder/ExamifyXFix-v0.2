using ExamifyX.Model;
using ExamifyX.Model.Services;
using ExamifyX.View;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ExamifyX.ViewModel
{
	//The view model for the student view where it transitions between views
	//and functions implemented inside it
	public class StudentViewModel : INotifyPropertyChanged
	{
		public event EventHandler OnLogoutRequested;
		public event PropertyChangedEventHandler? PropertyChanged;

		private readonly string _name;
		private readonly string _surname;
		private readonly UserService _userService;
		private readonly LoginService _loginService;

		private System.Windows.Controls.UserControl _currentView;
		public System.Windows.Controls.UserControl CurrentView
		{
			get => _currentView;
			set
			{
				_currentView = value;
				OnPropertyChanged(nameof(CurrentView));
			}
		}

        public StudentViewModel(UserService userService, LoginService loginService)
        {
			_userService = userService;
			_loginService = loginService;

			_name = SessionContext.UserName ?? throw new ArgumentException(nameof(SessionContext.UserName));
			_surname = SessionContext.UserSurname ?? throw new ArgumentException(nameof(SessionContext.UserSurname));

			OnLogoutRequested = delegate { };
			_currentView = new UserControl();
			
			UpdateCurrentViewToMainMenu();
        }

		private void UpdateCurrentViewToMainMenu()
		{
			var studentMainMenuViewModel = new StudentMainMenuViewModel(_userService, _loginService);
			studentMainMenuViewModel.LogOutRequested += () => OnLogoutRequested?.Invoke(this, EventArgs.Empty);
			studentMainMenuViewModel.TakeExamRequested += SwitchToExamPanelView;
			CurrentView = new StudentMainMenuView(_userService,_loginService) { DataContext = studentMainMenuViewModel };
		}

		private void SwitchToExamPanelView()
		{
			var exam = new ExamItems();

			var examPanelViewModel = new ExamsPanelViewModel(exam);
			examPanelViewModel.OnRequestBack += HandleNavigateBack;
			CurrentView = new TakeExamView { DataContext = examPanelViewModel };
		}

		private void HandleNavigateBack()
		{
			UpdateCurrentViewToMainMenu();
		}

		private void MainMenuViewModel_LogOutRequested()
		{
			OnLogoutRequested?.Invoke(this, EventArgs.Empty);
		}

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
