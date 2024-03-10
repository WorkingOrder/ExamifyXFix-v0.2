using ExamifyX.Model;
using ExamifyX.Model.Commands;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExamifyX.ViewModel
{
	//A view Model for the ExamStatus view which is basically a data grid that shows uploaded exams
    public class ExamStatusViewModel : INotifyPropertyChanged
    {

		private readonly MyDbContext _context;

		public ObservableCollection<ExamItems> Exams { get; set; } = new ObservableCollection<ExamItems>();

		private void LoadExams()
		{
			var examsFromDb = _context.Exams.ToList();
			foreach(var exam in examsFromDb)
			{
				Exams.Add(new ExamItems
				{
					TestName = exam.TeacherName,
					TestDate = exam.PublishDate,
					Duration = exam.Duration,
					TestSubject = exam.Subject,
					TestTeacher = exam.TeacherName,
					IsPublished = true,
					Status = DetermineStatus(exam)
				});
			}
		}

		private string DetermineStatus(Exam exam)
		{
			TimeSpan duration;
			if(!TimeSpan.TryParse(exam.Duration, out duration))
			{
				duration = TimeSpan.Zero;
			}

			var now = DateTime.Now;

			var endTime = exam.PublishDate.Add(duration);

			if (now >= exam.PublishDate && now <= endTime)
			{
				return "Ongoing";
			}
			else if(now > endTime)
			{
				return "Finished";
			}
			else
			{
				return "Not Started";
			}
		}

		public ICommand BackCommand { get; private set; }
		public event Action RequestNavigateBack;

		public ExamStatusViewModel()
        {
			var options = new DbContextOptionsBuilder<MyDbContext>().Options;
			_context = new MyDbContext(options);
			BackCommand = new RelayCommand(ExecuteBackCommand);

			LoadExams();
		}

		private void ExecuteBackCommand()
		{
			RequestNavigateBack?.Invoke();
		}

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
