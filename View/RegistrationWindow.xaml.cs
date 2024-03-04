using Azure;
using ExamifyX.Model.Services;
using ExamifyX.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ExamifyX.View
{
    /// <summary>
    /// Interaction logic for RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
	{

		private readonly UserService _userService;
		private readonly RegisterViewModel _viewModel;

		public RegistrationWindow(UserService userService)
		{
			InitializeComponent();
			_userService = userService;

			_viewModel = new RegisterViewModel(userService);
			_viewModel.OnRegistrationCompleted += ShowMessage;
			DataContext = _viewModel;
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			_viewModel.OnRegistrationCompleted -= ShowMessage;
		}


		/// <summary>
		/// Generic backbutton with a close method 
		/// that closes and shows the CredentialsWindow
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
        }

		private void ShowMessage(string message)
		{
			MessageBox.Show(message);
		}

		/// <summary>
		/// temporary solution for the finialize button (checks if fields are empty or not and responds accordingly)
		/// TODO: Authorization for account
		/// TODO: conformation ID for the two types of accounts
		/// TODO: Probably best to put it in a database
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FinalizeButton_Click(object sender, RoutedEventArgs e)
		{

			var viewModel = (RegisterViewModel)this.DataContext;
			viewModel.Password = RegPassword.Password;
			viewModel.ConfirmPassword = ConfirmRegPassword.Password;
			viewModel.RegisterCommand.Execute(null);
		}

		private void RegPassword_PasswordChanged(object sender, RoutedEventArgs e)
		{
			if(DataContext is RegisterViewModel viewModel)
			{
				viewModel.Password = RegPassword.Password;
			}
        }

		private void ConfirmRegPassword_PasswordChanged(object sender, RoutedEventArgs e)
		{
			if(DataContext is RegisterViewModel viewModel)
			{
				viewModel.ConfirmPassword = ConfirmRegPassword.Password;
			}
		}


	}
}
