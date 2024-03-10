using ExamifyX.Model;
using ExamifyX.Model.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ExamifyX.ViewModel
{
	//The view model that represents the structure of the exam for the student
	public class StudentExamViewModel : INotifyPropertyChanged
	{
		private readonly MyDbContext _context;
		public ObservableCollection<QuestionViewModel> Questions { get; set; } = new ObservableCollection<QuestionViewModel>();

		//Commands for navigation and submission
		public ICommand SubmitExamCommand { get; set; }

		public StudentExamViewModel(int examId)
		{
			var options = new DbContextOptionsBuilder<MyDbContext>().Options;
			_context = new MyDbContext(options);
			LoadQuestions(examId);
			SetupCommands();
		}

		private void LoadQuestions(int examId)
		{
			try
			{
				Debug.WriteLine($"Loading questions for exam ID: {examId}");
				var questions = _context.Questions.Where(q => q.ExamId == examId).ToList();

				Debug.WriteLine($"Questions found: {questions.Count}");

				foreach (var question in questions)
				{
					var questionVM = new QuestionViewModel();
					questionVM.QuestionText = question.QuestionText;
					questionVM.SetOptions(question.OptionA, question.OptionB, question.OptionC, question.OptionD);
					Questions.Add(questionVM);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Error loading questions: {ex.Message}");
				MessageBox.Show($"Error loading questions: {ex.Message}");
			}
		}

		private void SetupCommands()
		{
			//Initialize SubmitAnswersCommand with appropriate logic
			SubmitExamCommand = new RelayCommand(SubmitExam, CanSubmitExam);
		}

		private bool CanSubmitExam()
		{
			return Questions.All(q => !string.IsNullOrEmpty(q.SelectedOption));
		}

		private void SubmitExam()
		{
			int correctAnswersCount = 0;

			foreach (var questionVm in Questions)
			{
				var correctOption = _context.Questions.FirstOrDefault(q => q.QuestionText == questionVm.QuestionText)?.CorrectOption.ToString();
				if (questionVm.SelectedOption == correctOption)
				{
					correctAnswersCount++;
				}
			}

			// Example: Showing the count of correct answers. Adapt as needed.
			MessageBox.Show($"You got {correctAnswersCount} out of {Questions.Count} questions right.", "Exam Results", MessageBoxButton.OK);

			CloseWindow();
		}

		private void CloseWindow()
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
				window?.Close();
			});
		}

		public event PropertyChangedEventHandler? PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}


	}
}
