using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamifyX.ViewModel
{
	//A view model that represents the structure of the questions when the students takes them
	public class QuestionViewModel : INotifyPropertyChanged
	{
		public string QuestionText { get; set; }
		public ObservableCollection<string> Options { get; set; } = new ObservableCollection<string>();
		private string _selectedOption;

		public Dictionary<string, string> OptionIdentifiers { get; private set; } = new Dictionary<string, string>();


		public string SelectedOption
		{
			get => _selectedOption;
			set
			{
				_selectedOption = value;
				OnPropertyChanged(nameof(SelectedOption));
			}
		}

		public string SelectedOptionIdentifier => OptionIdentifiers.ContainsKey(SelectedOption) ? OptionIdentifiers[SelectedOption] : null;

		public event PropertyChangedEventHandler? PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public QuestionViewModel()
		{
		}

		public void SetOptions(string optionA, string optionB, string optionC, string optionD)
		{
			Options.Clear();
			Options.Add(optionA);
			Options.Add(optionB);
			Options.Add(optionC);
			Options.Add(optionD);

			OptionIdentifiers.Clear();
			OptionIdentifiers[optionA] = "A";
			OptionIdentifiers[optionB] = "B";
			OptionIdentifiers[optionC] = "C";
			OptionIdentifiers[optionD] = "D";

			OnPropertyChanged(nameof(Options));
		}
	}
}
