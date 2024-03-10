using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ExamifyX.Model
{
	//this class was made to make the answer options in the exam work, almost the same idea as
	//the EnumBooleanConverter class
	public class OptionToBoolConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string selectedOption = value as string;
			string optionParameter = parameter as string;

			return selectedOption == optionParameter;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((bool)value)
			{
				return parameter as string;
			}
			return null;
		}
	}
}
