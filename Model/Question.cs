using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamifyX.Model
{
	//A class setting up the structure of questions(saved to database)

	public enum Option
	{
		None,
		A,
		B,
		C,
		D
	}
	public class Question : INotifyPropertyChanged
	{
		[Key]
		public int Id { get; set; }

		private Option _correctOption;
		private string? _questionText;
		private string? _optionA;
		private string? _optionB;
		private string? _optionC;
		private string? _optionD;

		public Option CorrectOption
		{
			get => _correctOption;
			set
			{
				_correctOption = value;
				OnPropertyChanged(nameof(CorrectOption));
			}
		}
		public string? QuestionText
		{
			get => _questionText;
			set
			{
				_questionText = value;
				OnPropertyChanged(nameof(QuestionText));
			}
		}

		public string? OptionA
		{
			get => _optionA;

			set
			{
				_optionA = value;
				OnPropertyChanged(nameof(OptionA));
			}
		}
		public string? OptionB
		{
			get => _optionB;

			set
			{
				_optionB = value;
				OnPropertyChanged(nameof(OptionB));
			}
		}
		public string? OptionC
		{
			get => _optionC;

			set
			{
				_optionC = value;
				OnPropertyChanged(nameof(OptionC));
			}
		}
		public string? OptionD
		{
			get => _optionD;

			set
			{
				_optionD = value;
				OnPropertyChanged(nameof(OptionD));
			}
		}
		public List<string> Options { get; set; }
		public int ExamId { get; set; }

		[ForeignKey("ExamId")]
		public Exam Exam { get; set; }

		public Question()
		{
			Options = new List<string>();
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public bool HasCorrectAnswer()
		{
			return CorrectOption != Option.None;
		}
	}
}