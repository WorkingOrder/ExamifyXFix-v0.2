using ExamifyX.Model.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExamifyX.ViewModel
{
    public class ExamStatusViewModel : INotifyPropertyChanged
    {
        public ICommand OpenPublishExamCommand { get; private set; }
        public event Action RequestOpenPublishExamWindow;

		public ICommand BackCommand { get; private set; }
		public event Action RequestNavigateBack;

		public ExamStatusViewModel()
        {
            OpenPublishExamCommand = new RelayCommand(ExecuteOpenPublishExamCommand);
			BackCommand = new RelayCommand(ExecuteBackCommand);
		}

		private void ExecuteBackCommand()
		{
			RequestNavigateBack?.Invoke();
		}

		private void ExecuteOpenPublishExamCommand()
        {
            RequestOpenPublishExamWindow?.Invoke();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
