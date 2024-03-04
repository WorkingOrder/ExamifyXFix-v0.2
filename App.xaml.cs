using ExamifyX.Model;
using ExamifyX.Model.Services;
using ExamifyX.View;
using ExamifyX.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace ExamifyX
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
	{

		/// <summary>
		/// ensures that when the application starts it checks if there are any
		/// pending database migrations and applies them
		/// </summary>
		/// <param name="e"></param>
	
		
		private readonly IServiceProvider _serviceProvider;

		public App()
		{
			var serviceCollection = new ServiceCollection();
			ConfigureServices(serviceCollection);
			_serviceProvider = serviceCollection.BuildServiceProvider();
		}

		private static void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<MyDbContext>();
			services.AddScoped<UserService>();
			services.AddScoped<LoginService>();
			services.AddScoped<CredentialsWindow>();
		}

		protected override async void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			//Ensure the database is up-to-date
			var context = _serviceProvider.GetRequiredService<MyDbContext>();
			await context.Database.MigrateAsync();
			
			Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
			
			//Resolve CredentialsWindow from the service provider
			var credentialsWindow = new CredentialsWindow(_serviceProvider);
			credentialsWindow.Show();
		}
	}
}