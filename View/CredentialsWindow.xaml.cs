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
using System.Windows.Shapes;
using Microsoft.Extensions.DependencyInjection;

namespace ExamifyX.View
{
    /// <summary>
    /// Interaction logic for CredentialsWindow.xaml
    /// </summary>
    public partial class CredentialsWindow : Window
	{
		private readonly UserService _userService;
		private readonly LoginService _loginService;

        public CredentialsWindow(IServiceProvider serviceProvider)
		{
			InitializeComponent();

			_userService = serviceProvider.GetRequiredService<UserService>();
			_loginService = serviceProvider.GetRequiredService<LoginService>();

			var loginViewModel = new LoginViewModel(serviceProvider);
			DataContext = loginViewModel;

			loginViewModel.OpenRegisterWindowRequested += HandleOpenRegisterWindowRequested;
			//Unsubscribes when the window is closed
			this.Closed += (s, e) => loginViewModel.OpenRegisterWindowRequested -= HandleOpenRegisterWindowRequested;
		}

		private void HandleOpenRegisterWindowRequested()
		{
			var registrationWindow = new RegistrationWindow(_userService);
			registrationWindow.Show();
		}

		/// <summary>
		/// shuts down the whole application
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void QuitButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
        }

		private void LoginPassword_PasswordChanged(object sender, RoutedEventArgs e)
		{
			if(DataContext is LoginViewModel viewModel)
			{
				viewModel.Password = LoginPassword.Password;
			}
		}
	}
}
