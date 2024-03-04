using ExamifyX.Model;
using ExamifyX.Model.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExamifyX.ViewModel
{
	public class ExamCreationViewModel : INotifyPropertyChanged
	{
		private ObservableCollection<Question> _questions { get; set; } = new ObservableCollection<Question>();

		private TimeSpan _examDuration;
		public TimeSpan ExamDuration
		{
			get => _examDuration;
			set 
			{
				if (_examDuration != value)
				{
					_examDuration = value;
					OnPropertyChanged(nameof(ExamDuration));
				}
			}

		}
		public ObservableCollection<Question> Questions
		{
			get => _questions;
			set
			{
				_questions = value;
				OnPropertyChanged(nameof(Questions));
				OnPropertyChanged(nameof(CurrentQuestion));
				OnPropertyChanged(nameof(QuestionCount));
			}
		}

		private int _currentQuestionIndex = -1;
		public int CurrentQuestionIndex
		{
			get => _currentQuestionIndex;
			set
			{
				_currentQuestionIndex = value;
				OnPropertyChanged(nameof(CurrentQuestionIndex));
				OnPropertyChanged(nameof(CurrentQuestion));
				OnPropertyChanged(nameof(QuestionCount));
			}
		}

		public Question? CurrentQuestion => Questions.Count > 0 && _currentQuestionIndex >= 0 ? Questions[_currentQuestionIndex] : null;

		public ICommand AddQuestionCommand { get; }
		public ICommand RemoveQuestionCommand { get; }
		public ICommand NextQuestionCommand { get; }
		public ICommand PreviousQuestionCommand { get; }

		public ExamCreationViewModel()
		{
			AddQuestionCommand = new RelayCommand(AddQuestion);
			RemoveQuestionCommand = new RelayCommand(RemoveQuestion, CanRemoveQuestion);
			NextQuestionCommand = new RelayCommand(NextQuestion, CanGoToNextQuestion);
			PreviousQuestionCommand = new RelayCommand(PreviousQuestion, CanGoToPreviousQuestion);
			BackCommand = new RelayCommand(BackAction);
		}

		private bool CanGoToPreviousQuestion()
		{
			return CurrentQuestionIndex > 0;
		}

		private bool CanGoToNextQuestion()
		{
			return CurrentQuestionIndex < Questions.Count - 1;
		}

		private bool CanRemoveQuestion()
		{
			return CurrentQuestion != null;
		}

		private void AddQuestion()
		{
			var newQuestion = new Question { QuestionText = "Question Example" };
			Questions.Add(newQuestion);
			CurrentQuestionIndex = Questions.Count - 1;
		}

		private void RemoveQuestion()
		{
			if (CurrentQuestion != null && CurrentQuestionIndex >= 0 && Questions.Count > 0)
			{
				Questions.RemoveAt(CurrentQuestionIndex);

				if (CurrentQuestionIndex >= Questions.Count)
				{
					CurrentQuestionIndex = Questions.Count - 1;
				}

				if (Questions.Count == 0)
				{
					CurrentQuestionIndex = -1;
				}

				OnPropertyChanged(nameof(CurrentQuestion));
				OnPropertyChanged(nameof(QuestionCount));
			}

			
		}

		private void NextQuestion()
		{
			if (CanGoToNextQuestion())
			{
				CurrentQuestionIndex++;
			}
		}

		private void PreviousQuestion()
		{
			if (CanGoToPreviousQuestion())
			{
				CurrentQuestionIndex--;
			}
		}
		public string QuestionCount => $"{CurrentQuestionIndex + 1}/{Questions.Count}";

		public ICommand BackCommand { get; }

		private void BackAction()
		{
			OnRequestClose?.Invoke();
		}

		public event Action OnRequestClose;
		public event PropertyChangedEventHandler? PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
