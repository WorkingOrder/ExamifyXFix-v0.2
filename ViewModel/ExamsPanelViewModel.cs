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
	public class ExamsPanelViewModel : INotifyPropertyChanged
	{
		public event Action OnRequestBack;

		public ICommand BackCommand { get; }

        public ExamsPanelViewModel()
        {
			BackCommand = new RelayCommand(BackFunction);
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
