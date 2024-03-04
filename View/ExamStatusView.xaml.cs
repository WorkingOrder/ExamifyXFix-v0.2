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
	/// Interaction logic for ExamStatusView.xaml
	/// </summary>
	public partial class ExamStatusView : UserControl
	{
		public ExamStatusView()
		{
			InitializeComponent();
			this.DataContext = new ExamStatusViewModel();
		}
	}
}
