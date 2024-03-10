using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ExamifyX.Model
{
	// A class that sets the boolean for the use being a student or a teacher
	public class EnumBooleanConverter : IValueConverter
	{
		//Checks if the current enum value (passed as value) matches the specified parameter(an enum value).
		//If they match, it returns true; otherwise, false
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value.Equals(parameter);
		}

		//Converts a boolean back to the corresponding enum value. If value is true, it returns the parameter (the enum value);
		//if false, it returns Binding.DoNothing to indicate no action should be taken.
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value.Equals(true) ? parameter : Binding.DoNothing;
		}
	}
}
