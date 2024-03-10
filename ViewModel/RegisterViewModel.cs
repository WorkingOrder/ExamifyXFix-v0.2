using ExamifyX.Model;
using ExamifyX.Model.Commands;
using ExamifyX.Model.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ExamifyX.ViewModel
{
	//The view model that represents the registration window
    public enum UserRole
	{
		Unselected = 0,
		Student,
		Teacher
	}



	public class RegisterViewModel : INotifyPropertyChanged
	{

		private readonly UserService _userService;

		public event PropertyChangedEventHandler? PropertyChanged;
		public Action<string>? OnRegistrationCompleted { get; set; }

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#region --Props--
		private string? _name { get; set; }
		private string? _surname { get; set; }
		private string? _email { get; set; }
		private string? _username { get; set; }
		private string? _password { get; set; }
		private string? _confirmPassword { get; set; }
		private string? _nameError { get; set; }
		private string? _surnameError { get; set; }
		private string? _emailError { get; set; }
		private string? _usernameError { get; set; }
		private string? _passwordError { get; set; }
		private string? _confirmPasswordError { get; set; }
		private string? _userRoleError { get; set; }

		private UserRole _selectedRole;

		public bool HasErrors =>
		!string.IsNullOrEmpty(NameError) ||
		!string.IsNullOrEmpty(SurnameError) ||
		!string.IsNullOrEmpty(EmailError) ||
		!string.IsNullOrEmpty(UsernameError) ||
		!string.IsNullOrEmpty(PasswordError) ||
		!string.IsNullOrEmpty(ConfirmPasswordError) ||
		!string.IsNullOrEmpty(UserRoleError);
		#endregion

		public string? Name
		{
			get => _name;
			set
			{
				_name = value; OnPropertyChanged(nameof(Name));
			}
		}

		public string? Surname
		{
			get => _surname;
			set
			{
				_surname = value; OnPropertyChanged(nameof(Surname));
			}
		}

		public string? Email
		{
			get => _email;
			set
			{
				_email = value; OnPropertyChanged(nameof(Email));
			}
		}

		public string? Username
		{
			get => _username;
			set
			{
				_username = value; OnPropertyChanged(nameof(Username));
			}
		}

		public string? Password
		{
			get => _password;
			set
			{
				_password = value; OnPropertyChanged(nameof(Password));
			}
		}

		public string? ConfirmPassword
		{
			get => _confirmPassword;
			set
			{
				_confirmPassword = value; OnPropertyChanged(nameof(_confirmPassword));
			}
		}

		public string? NameError
		{
			get => _nameError;
			set
			{
				_nameError = value; OnPropertyChanged(nameof(NameError));
			}
		}

		public string? SurnameError
		{
			get => _surnameError;
			set
			{
				_surnameError = value; OnPropertyChanged(nameof(SurnameError));
			}
		}

		public string? EmailError
		{
			get => _emailError;
			set
			{
				_emailError = value; OnPropertyChanged(nameof(EmailError));
			}
		}
		public string? UsernameError
		{
			get => _usernameError;
			set
			{
				_usernameError = value; OnPropertyChanged(nameof(UsernameError));
			}
		}
		public string? PasswordError
		{
			get => _passwordError;
			set
			{
				_passwordError = value; OnPropertyChanged(nameof(PasswordError));
			}
		}
		public string? ConfirmPasswordError
		{
			get => _confirmPasswordError;
			set
			{
				_confirmPasswordError = value; OnPropertyChanged(nameof(ConfirmPasswordError));
			}
		}
		public string? UserRoleError
		{
			get => _userRoleError;
			set
			{
				_userRoleError = value; OnPropertyChanged(nameof(UserRoleError));
			}
		}


		public UserRole SelectedRole
		{
			get => _selectedRole;
			set
			{
				_selectedRole = value; OnPropertyChanged(nameof(SelectedRole));
			}
		}

		public ICommand RegisterCommand { get; private set; }

		public RegisterViewModel(UserService userService)
		{
			SelectedRole = UserRole.Unselected;
			_userService = userService;
			RegisterCommand = new RelayCommand(RegisterUser);
		}

		public void ValidateInput()
		{

			//Validate Name
			if (string.IsNullOrWhiteSpace(Name))
			{
				NameError = "Name is required!";
			}
			else
			{
				NameError = string.Empty; //Clear error message if validation
			}

			//Validate Surname
			if (string.IsNullOrWhiteSpace(Surname))
			{
				SurnameError = "Surname is required!";
			}
			else
			{
				SurnameError = string.Empty;
			}

			//Validate Email
			if (string.IsNullOrWhiteSpace(Email))
			{
				EmailError = "Email is required!";
			}
			else if (!IsValidEmail(Email))
			{
				EmailError = "Invalid email format";
			}
			else
			{
				EmailError = string.Empty;
			}

			//Validate Username
			if (string.IsNullOrEmpty(Username))
			{
				UsernameError = "Username is required!";
			}
			else
			{
				UsernameError = string.Empty;
			}

			// Validate Password
			if (string.IsNullOrEmpty(Password))
			{
				PasswordError = "Password is required!";
			}
			else
			{
				PasswordError = string.Empty;
			}

			// Validate Confirm Password
			if (string.IsNullOrEmpty(ConfirmPassword))
			{
				ConfirmPasswordError = "Confirm Password is required!";
			}
			else if (ConfirmPassword != Password)
			{
				ConfirmPasswordError = "Passwords do not match!";
			}
			else
			{
				ConfirmPasswordError = string.Empty;
			}

			//Validate if Radio Button are selected
			if (SelectedRole == UserRole.Unselected)
			{
				UserRoleError = "One of them must be selected!";
			}
			else if ((SelectedRole == UserRole.Student) || (SelectedRole == UserRole.Teacher))
			{
				UserRoleError = string.Empty;
			}
		}

		private bool IsValidEmail(string email)
		{
			try
			{
				var addr = new System.Net.Mail.MailAddress(email);
				return addr.Address == email;
			}
			catch
			{
				return false;
			}
		}

		public void RegisterUser()
		{
			ValidateInput();

			if (HasErrors)
			{
				MessageBox.Show("Missing or incorrect fields");
				return;
			}

			//Check if username already exists
			if(!_userService.IsUsernameAvailable(Username))
			{
				MessageBox.Show("Username already taken!");
				return;
			}

            if (!_userService.IsEmailAvailable(Email))
            {
				MessageBox.Show("Email is already taken!");
				return;
            }

            //Password hashing (retrieve password securely from PasswordBox)
            string hashedPassword = PasswordHasher.HashPassword(_password);



			//Create user entity
			User newUser = new User
			{
				Name = Name,
				Surname = Surname,
				Email = Email,
				Username = Username,
				PasswordHash = hashedPassword,
				UserRole = SelectedRole
			};

			

				//Save newUser to the database
				_userService.AddUser(newUser);

			OnRegistrationCompleted?.Invoke("Registration Successful!");

		}
	}
}