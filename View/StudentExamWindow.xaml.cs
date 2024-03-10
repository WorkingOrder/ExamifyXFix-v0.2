using ExamifyX.Model;
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

namespace ExamifyX.View
{
	/// <summary>
	/// Interaction logic for StudentExamWindow.xaml
	/// </summary>
	public partial class StudentExamWindow : Window
	{
		public StudentExamWindow(int examId)
		{
			InitializeComponent();
			this.DataContext = new StudentExamViewModel(examId);
		}
	}
}
