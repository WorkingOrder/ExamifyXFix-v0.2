using ExamifyX.Model;
using ExamifyX.Model.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ExamifyX.ViewModel
{
	public class ExamCreationViewModel : INotifyPropertyChanged
	{
		private ObservableCollection<Question> _questions { get; set; } = new ObservableCollection<Question>();

		private readonly MyDbContext _context;
		public ICommand SaveAndPublishCommand { get; }



		private DateTime _publishDate;
		public DateTime PublishDate
		{
			get => _publishDate;
			set
			{
				_publishDate = value;
				OnPropertyChanged(nameof(PublishDate));
				ValidatePublishability();
			}
		}

		private string _publishTime = "00:00";
		public string PublishTime
		{
			get => _publishTime;
			set
			{
				string newValue = ModifyTimeInput(value);
				if (_publishTime != value)
				{
					_publishTime = value;
					OnPropertyChanged(nameof(PublishTime));
					ValidatePublishability();
				}
			}
		}

		private bool _isPublishable;
		public bool IsPublishable
		{
			get => _isPublishable;
			set
			{
				_isPublishable = value;
				OnPropertyChanged(nameof(IsPublishable));
			}
		}

		private string _testName;
		public string TestName
		{
			get => _testName;
			set
			{
				_testName = value;
				OnPropertyChanged(nameof(TestName));
			}
		}

		private string _subject;
		public string Subject
		{
			get => _subject;
			set
			{
				_subject = value;
				OnPropertyChanged(nameof(Subject));
			}
		}
		private string _teacherName;
		public string TeacherName
		{
			get => _teacherName;
			set
			{
				_teacherName = value;
				OnPropertyChanged(nameof(TeacherName));
			}
		}

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
			SaveAndPublishCommand = new RelayCommand(async () =>
			{
				try
				{
					await SaveAndPublish();
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error Creating Exam, Missing components", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			},
	() => IsPublishable);
			AddQuestionCommand = new RelayCommand(AddQuestion);
			RemoveQuestionCommand = new RelayCommand(RemoveQuestion, CanRemoveQuestion);
			NextQuestionCommand = new RelayCommand(NextQuestion, CanGoToNextQuestion);
			PreviousQuestionCommand = new RelayCommand(PreviousQuestion, CanGoToPreviousQuestion);
			BackCommand = new RelayCommand(BackAction);
		}

		private string ModifyTimeInput(string input)
		{
			if (input.Length == 2 && !input.Contains(":") && int.TryParse(input, out int possibleHour))
			{
				if (possibleHour <= 23)
				{
					return input + ":";
				}
			}

			if (!IsTextAllowed(input))
			{
				return _publishTime;
			}
			return input;
		}

		//Check what to do with this.
		private bool IsPublishTimeValid()
		{
			return TimeSpan.TryParse(PublishTime, out TimeSpan result) && result.TotalHours < 24;
		}

		private void ValidatePublishability()
		{
			IsPublishable = Questions.Any() &&
				!string.IsNullOrWhiteSpace(TestName) &&
				!string.IsNullOrWhiteSpace(Subject) &&
				!string.IsNullOrWhiteSpace(TeacherName) &&
				!string.IsNullOrWhiteSpace(PublishTime) &&
				ExamDuration != TimeSpan.Zero &&
				Questions.All(q => q.HasCorrectAnswer());


		}

		private async Task SaveAndPublish()
		{
			// Create a new Exam entity and populate it with data from the UI
			var exam = new Exam
			{
				TestName = this.TestName,
				Subject = this.Subject,
				TeacherName = this.TeacherName,
				Duration = this.ExamDuration,
				PublishDate = this.PublishDate,
				Questions = new List<Question>(this.Questions)
			};

			// Add the exam to the DbContext
			_context.Exams.Add(exam);

			// Save changes to the database asynchronously
			await _context.SaveChangesAsync();
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
