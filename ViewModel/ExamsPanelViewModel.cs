using ExamifyX.Model;
using ExamifyX.Model.Commands;
using ExamifyX.Model.Services;
using ExamifyX.View;
using Microsoft.EntityFrameworkCore;
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
	//A viewmodel for the structure and fuctions for the TakeExamView
	public class ExamsPanelViewModel : INotifyPropertyChanged
	{
		public event Action OnRequestBack;

		public ICommand BackCommand { get; }
		public ICommand StartExamCommand { get; private set; }


		private readonly MyDbContext _context;

		public ObservableCollection<ExamItems> Exams { get; set; } = new ObservableCollection<ExamItems>();

		private ExamItems _selectedExam;
		public ExamItems SelectedExam
		{
			get => _selectedExam;
			set
			{
				_selectedExam = value;
				OnPropertyChanged(nameof(SelectedExam));
			}
		}
		private void LoadExams()
		{
			var examsFromDb = _context.Exams.ToList();
			foreach (var exam in examsFromDb)
			{
				Exams.Add(new ExamItems
				{
					ExamId = exam.ExamId,
					TestName = exam.TeacherName,
					TestDate = exam.PublishDate,
					Duration = exam.Duration,
					TestSubject = exam.Subject,
					TestTeacher = exam.TeacherName,
					Status = DetermineStatus(exam)
				});
			}
		}

		private string DetermineStatus(Exam exam)
		{
			TimeSpan duration;
			if (!TimeSpan.TryParse(exam.Duration, out duration))
			{
				duration = TimeSpan.Zero;
			}

			var now = DateTime.Now;

			var endTime = exam.PublishDate.Add(duration);

			if (now >= exam.PublishDate && now <= endTime)
			{
				return "Ongoing";
			}
			else if (now > endTime)
			{
				return "Finished";
			}
			else
			{
				return "Not Started";
			}
		}

		public ExamsPanelViewModel(ExamItems exam)
        {
			var options = new DbContextOptionsBuilder<MyDbContext>().Options;
			_context = new MyDbContext(options);

			StartExamCommand = new RelayCommand(ExecuteStartExam, CanExecuteStartExam);
			BackCommand = new RelayCommand(BackFunction);

			LoadExams();
        }

		private bool CanExecuteStartExam()
		{
			return SelectedExam != null;
		}

		private void ExecuteStartExam()
		{
			if (SelectedExam != null)
			{
				OpenExamWindow(SelectedExam.ExamId); // Ensure this passes the ExamId correctly
			}
		}

		private void OpenExamWindow(int examId)
		{
			var examWindow = new StudentExamWindow(examId); // Ensure ExamId is correctly passed here
			examWindow.Show();
		}

		private void BackFunction()
		{
			OnRequestBack?.Invoke();
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
