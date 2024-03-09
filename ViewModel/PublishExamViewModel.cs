using ExamifyX.Model.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExamifyX.ViewModel
{
	public class PublishExamViewModel : INotifyPropertyChanged
	{
		private DateTime _publishDate = DateTime.Today;
		public DateTime PublishDate
		{
			get => _publishDate;
			set
			{
				_publishDate = value;
				OnPropertyChanged(nameof(PublishDate));
			}
		}

		private string _publishTime = "00:00";
		public string PublishTime
		{
			get => _publishTime;
			set
			{
				if (_publishTime != value)
				{
					_publishTime = value;
					OnPropertyChanged(nameof(PublishTime));
				}
			}
		}

		public ICommand SetPublishTimeCommand { get; private set; }
		public ICommand BackCommand { get; private set; }
        public PublishExamViewModel()
        {
			SetPublishTimeCommand = new RelayCommand(SetPublishTime, CanExecuteSetPublishTime);
			BackCommand = new RelayCommand(ExecuteBackCommand);
        }

		private void ExecuteBackCommand()
		{
			OnRequestClose?.Invoke();
		}

		public event Action OnRequestClose;

		private void SetPublishTime()
		{
			if(DateTime.TryParseExact(_publishTime, "HH:mm", null, System.Globalization.DateTimeStyles.None, out var timeSpan))
			{
				_publishDate = DateTime.Today.Add(timeSpan.TimeOfDay);
				Console.WriteLine($"Publish Time Set To: {_publishDate}");
			}
		}

		private bool CanExecuteSetPublishTime()
		{
			return DateTime.TryParseExact(_publishTime, "HH:mm", null, System.Globalization.DateTimeStyles.None, out _);
		}

        public event PropertyChangedEventHandler? PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
