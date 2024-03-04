using ExamifyX.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ExamifyX.Model
{
    //this converter is used for converting between UserRole enum values and boolean values
    public class UserToBoolConv : IValueConverter
    {
        //Convert from UserRole to boolean
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Ensure that 'value' is a UserRole and 'parameter' is a string
            if (!(value is UserRole) || !(parameter is string parameterString))
            {
                return DependencyProperty.UnsetValue;
            }

            //Compare the UserRole value to the parameter and return true if they match
            var userRole = (UserRole)value;
            return userRole.ToString() == parameterString;
        }

        //Convert from boolean back to UserRole
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Ensure the value is true (radio button checked) and 'parameter' is a string
            if (!(value is bool boolValue) || !boolValue || !(parameter is string parameterString))
            {
                return DependencyProperty.UnsetValue;
            }

            //Parse the parameter back to a UserRole enum
            return Enum.Parse(typeof(UserRole), parameterString);
        }
    }
}
