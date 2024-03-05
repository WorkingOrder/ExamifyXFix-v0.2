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
			this.DataContext = new ExamsPanelViewModel();
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

		private void PublishTime_PreviewTextInput(object sender, TextCompositionEventArgs e)
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
			// Check length constraint for the entire string.
			if (text.Length > 5) return false;

			// Find the index of the colon.
			int colonIndex = text.IndexOf(':');
			if (colonIndex > -1)
			{
				// Ensure only one colon exists and it's in an expected position.
				if (colonIndex != 1 && colonIndex != 2) return false;
				if (text.Count(f => f == ':') > 1) return false;
				if (text.Length - colonIndex - 1 > 2) return false;

				// Split the string into hour and minute components.
				string[] parts = text.Split(':');
				if (parts.Length == 2)
				{
					// Validate the hour part.
					if (int.TryParse(parts[0], out int hour))
					{
						if (hour < 0 || hour > 23) return false;
					}
					else
					{
						// Hour part isn't a valid integer.
						return false;
					}

					// Validate the minute part if it exists.
					if (parts[1].Length > 0)
					{
						if (int.TryParse(parts[1], out int minute))
						{
							if (minute < 0 || minute > 59) return false;
						}
						else
						{
							// Minute part isn't a valid integer.
							return false;
						}
					}
				}
			}
			else
			{
				// Handle case where the user hasn't typed a colon yet but the digits exceed the 24-hour range.
				if (text.Length == 2 && int.TryParse(text, out int possibleHour))
				{
					if (possibleHour > 23) return false;
				}
			}

			// If no colon is present yet, ensure all characters are digits.
			return text.Replace(":", "").All(char.IsDigit);
		}
	}
}
