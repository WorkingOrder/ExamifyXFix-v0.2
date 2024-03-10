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
using Microsoft.Extensions.DependencyInjection;

namespace ExamifyX.ViewModel
{
	//A view model that sets up the structure and commands for the CredentialsWindow
	public class LoginViewModel : INotifyPropertyChanged
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly UserService _userService;
		private readonly LoginService _loginService;
		
		private string _username = string.Empty;
		private string _password = string.Empty;
		private string _usernameError = string.Empty;
		private string _passwordError = string.Empty;
		
		public event PropertyChangedEventHandler? PropertyChanged;

		public event Action<string, string, UserRole>? LoginSuccessful;

		public event Action? OpenRegisterWindowRequested;

		public string Username
		{
			get => _username;

			set
			{
				_username = value;
				OnPropertyChanged(nameof(Username));
			}
		}

		public string Password
		{
			get => _password;
			set
			{
				_password = value;
				OnPropertyChanged(nameof(Password));
			}
		}

		public string UsernameError
		{
			get => _usernameError;
			set
			{
				_usernameError = value;
				OnPropertyChanged(nameof(UsernameError));
			}
		}

		public string PasswordError
		{
			get => _passwordError;
			set
			{
				_passwordError = value;
				OnPropertyChanged(nameof(PasswordError));
			}
		}

		public ICommand LoginCommand { get;}

		public ICommand OpenRegisterWindowCommand { get; }

		public LoginViewModel(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
			
			var userService = serviceProvider.GetRequiredService<UserService>();
			var loginService = serviceProvider.GetRequiredService<LoginService>();
			
			// Ensures that userService is not null; throws ArgumentNullException if it is.
			_userService = userService ?? throw new ArgumentException(nameof(userService));
			// Ensures that loginService is not null; throws ArgumentNullException if it is.
			_loginService = loginService ?? throw new ArgumentException(nameof(loginService));
			LoginCommand = new RelayCommand(async () => await LoginAsync());
			OpenRegisterWindowCommand = new RelayCommand(OpenRegisterWindow);
			
			LoginSuccessful += (name, surname, role) => HandleLoginSuccess(role);
		}

		private void OpenRegisterWindow()
		{
			Console.WriteLine(this.ToString());
			OpenRegisterWindowRequested?.Invoke();
		}

		private async Task LoginAsync()
		{
			//Reset error messages
			UsernameError = string.Empty;
			PasswordError = string.Empty;

			//check for empty fields
			if(string.IsNullOrEmpty(Username))
			{
				UsernameError = "Username is required";
				return;
			}

			if (string.IsNullOrEmpty(Password))
			{
				PasswordError = "Password is required";
				return;
			}

			//Check if user exists
			var userExist = await _loginService.GetUserByUsernameAsync(Username);
			if(userExist == null)
			{
				UsernameError = "Username not found";
				return;
			}

			bool isValidPassword = await _loginService.ValidateCredentialsAsync(Username, Password);
			//checks if password matches
			if(!isValidPassword)
			{
				PasswordError = "Invalid password";
				return;
			}

			//Successful login - Navigate to the appropriate windows per role
			var user = await _loginService.GetUserByUsernameAsync(Username);
            if (user != null)
            {
				SessionContext.UserName = user.Name;
				SessionContext.UserSurname = user.Surname;

					LoginSuccessful?.Invoke(user.Name, user.Surname, user.UserRole);
				

				//Closes the current login window
				Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is CredentialsWindow)?.Close();
            }
        }

		private void HandleLoginSuccess(UserRole role)
		{
			Application.Current.Dispatcher.InvokeAsync(() =>
			{
				string? userName = SessionContext.UserName;
				string? userSurname = SessionContext.UserSurname;

				// Close the current login window
				Application.Current.Windows.OfType<CredentialsWindow>().FirstOrDefault()?.Close();

				switch (role)
				{
					case UserRole.Teacher:
						OpenTeacherWindow();
						
						break;

					case UserRole.Student:
						OpenStudentWindow();

						break;

				}
				
			});
		}

		public void OpenTeacherWindow()
		{
			var teacherViewModel = new TeacherViewModel(_userService, _loginService);
			teacherViewModel.OnLogoutRequested += HandleLogout;

			var teacherWindow = new TeacherWindow
			{
				DataContext = teacherViewModel
			};

			teacherWindow.Show();
		}

		public void OpenStudentWindow()
		{
			var studentViewModel = new StudentViewModel(_userService, _loginService);
			studentViewModel.OnLogoutRequested += HandleLogout;

			var studentWindow = new StudentWindow
			{
				DataContext = studentViewModel
			};

			studentWindow.Show();
		}

		private void HandleLogout(object? sender, EventArgs e)
		{
			//Reopen the CredentialsWindow
			var credentialsWindow = new CredentialsWindow(_serviceProvider);
			credentialsWindow.Show();

			//Close the TeacherWindow
			Application.Current.Windows
				.OfType<TeacherWindow>()
				.FirstOrDefault()?.Close();
			Application.Current.Windows
				.OfType<StudentWindow>()
				.FirstOrDefault()?.Close();
		}
		
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
