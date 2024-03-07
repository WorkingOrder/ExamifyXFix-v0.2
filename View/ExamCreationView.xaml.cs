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
	/// Interaction logic for ExamCreationView.xaml
	/// </summary>
	public partial class ExamCreationView : UserControl
	{
		public ExamCreationView()
		{
			InitializeComponent();
			this.DataContext = new ExamCreationViewModel();
		}
		private void PublishTimeTextBox_Pasting(object send, DataObjectPastingEventArgs e)
		{
			if (e.DataObject.GetDataPresent(typeof(string)))
			{
				string text = (String)e.DataObject.GetData(typeof(string));
				if (!IsTextAllowed(text))
				{
					e.CancelCommand();
				}
			}
			else
			{
				e.CancelCommand();
			}
		}

		private void Time_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			var textBox = sender as TextBox;
			string currentText = textBox.Text;
			int caretIndex = textBox.CaretIndex;


			string fullTextBeforeInput = currentText.Insert(caretIndex, e.Text);

			if (fullTextBeforeInput.Length == 2 && !fullTextBeforeInput.Contains(":") && int.TryParse(fullTextBeforeInput, out _))
			{
				Dispatcher.BeginInvoke(new Action(() =>
				{
					textBox.Text = fullTextBeforeInput + ":";
					textBox.CaretIndex = textBox.Text.Length;
					e.Handled = true;
				}));
				return;
			}
			else
			{
				e.Handled = !IsTextAllowed(fullTextBeforeInput);
			}


		}

		private bool IsTextAllowed(string text)
		{
			if (text.Length > 5)
				return false;

			int colonIndex = text.IndexOf(':');
			if (colonIndex > -1)
			{
				if (colonIndex != 1 && colonIndex != 2) return false;
				if (text.Count(f => f == ':') > 1) return false;
				if (text.Length - colonIndex - 1 > 2) return false;
			}

			return text.Replace(":", "").All(char.IsDigit);
		}
	}
}
