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
	//A view model made for the structure of exam creation
	public class ExamCreationViewModel : INotifyPropertyChanged
	{
		public Exam Exam { get; set; } = new Exam();
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
				if (TimeSpan.TryParseExact(value, "hh\\:mm", null, out TimeSpan dummyOutput))
				{
					if (_publishTime != value)
					{
						_publishTime = value;
						OnPropertyChanged(nameof(PublishTime));
						ValidatePublishability();
					}
				}
				else
				{
					OnPropertyChanged(nameof(PublishTime));
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
				ValidatePublishability();
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
				ValidatePublishability();
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
				ValidatePublishability();
			}
		}

		private string _examDuration;
		public string ExamDuration
		{
			get => _examDuration;
			set
			{
				if (_examDuration != value)
				{
					_examDuration = value;
					OnPropertyChanged(nameof(ExamDuration));
					ValidatePublishability();
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
				ValidatePublishability();
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
				ValidatePublishability();
			}
		}

		public Question? CurrentQuestion => Questions.Count > 0 && _currentQuestionIndex >= 0 ? Questions[_currentQuestionIndex] : null;

		public ICommand AddQuestionCommand { get; }
		public ICommand RemoveQuestionCommand { get; }
		public ICommand NextQuestionCommand { get; }
		public ICommand PreviousQuestionCommand { get; }

		public ExamCreationViewModel()
		{
			var options = new DbContextOptionsBuilder<MyDbContext>().Options;
			_context = new MyDbContext(options);
			PublishDate = DateTime.Now;

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

		private void ValidatePublishability()
		{
			IsPublishable = Questions.Any() &&
				!string.IsNullOrWhiteSpace(TestName) &&
				!string.IsNullOrWhiteSpace(Subject) &&
				!string.IsNullOrWhiteSpace(TeacherName) &&
				!string.IsNullOrWhiteSpace(PublishTime) &&
				!string.IsNullOrWhiteSpace(ExamDuration) &&
				Questions.All(q => q.HasCorrectAnswer());


		}

		private async Task SaveAndPublish()
		{
			if (!IsPublishable) // Guard clause to prevent saving when not publishable.
			{
				MessageBox.Show("The exam is not ready to be published.", "Cannot Save", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}
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
