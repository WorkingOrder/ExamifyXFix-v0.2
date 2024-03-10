using ExamifyX.Model.Commands;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using ExamifyX.View;
using System.Windows;
using ExamifyX.Model.Services;
using ExamifyX.Model;


namespace ExamifyX.ViewModel
{
	//The view model for the Teacher Window that it shows initially on loading up
	public class TeacherViewModel : INotifyPropertyChanged
	{

		public event EventHandler OnLogoutRequested;
		public event PropertyChangedEventHandler? PropertyChanged;

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

		public ICommand OpenMainMenuCommand { get; private set; }
		public int HandleNavigateBack { get; private set; }

		private readonly string _name;
		private readonly string _surname;
		private readonly UserService _userService;
		private readonly LoginService _loginService;

		public TeacherViewModel(UserService userService, LoginService loginService)
		{
			_userService = userService;
			_loginService = loginService;
			_name = SessionContext.UserName ?? throw new ArgumentException(nameof(SessionContext.UserName));
			_surname = SessionContext.UserSurname ?? throw new ArgumentException(nameof(SessionContext.UserSurname));

			OnLogoutRequested = delegate { };
			_currentView = new UserControl();
			OpenMainMenuCommand = new RelayCommand(UpdateCurrentViewToMainMenu);

			UpdateCurrentViewToMainMenu();
		}

		private void UpdateCurrentViewToMainMenu()
		{
			var mainMenuViewModel = new TeacherMainMenuViewModel(_userService,_loginService);
			mainMenuViewModel.LogOutRequested += () => OnLogoutRequested?.Invoke(this, EventArgs.Empty);
			mainMenuViewModel.OpenExamCreationRequested += SwitchToExamCreationView;
			mainMenuViewModel.OpenExamStatusRequested += SwitchToExamStatusView;
			CurrentView = new TeacherMainMenuView (_userService, _loginService) { DataContext = mainMenuViewModel };
		}

		private void SwitchToExamCreationView()
		{
			var examCreationViewModel = new ExamCreationViewModel();
			examCreationViewModel.OnRequestClose += () => UpdateCurrentViewToMainMenu();
			CurrentView = new ExamCreationView { DataContext = examCreationViewModel};
		}
		private void SwitchToExamStatusView()
		{
			var examStatusViewModel = new ExamStatusViewModel();
			var examStatusView = new ExamStatusView();
			examStatusView.DataContext = examStatusViewModel;
			examStatusViewModel.RequestNavigateBack += HandleStatusNavigateBack;

			CurrentView = examStatusView;
		}

		private void HandleStatusNavigateBack()
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
