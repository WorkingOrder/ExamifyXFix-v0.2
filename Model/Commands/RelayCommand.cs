using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExamifyX.Model.Commands
{
	/// <summary>
	/// RelayCommand is an implementation of the ICommand interface
	/// </summary>
	public class RelayCommand : ICommand
    {
		//A delegate that points to a method to be executed when the command is invoked.
		private readonly Action _execute;
		//A delegate that returns a boolean indicating whether the command can execute in its current state.
        //By default, this is set to always return true
		private readonly Func<bool> _canExecute;

		/// <summary>
		/// This event is used to re-evaluate whether the command can execute.
        /// It listens to the CommandManager.RequerySuggested event,
        /// which automatically queries the CanExecute method whenever a UI interaction
        /// that might change the command's ability to execute occurs.
		/// </summary>
		public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

		/// <summary>
		/// Accepts an Action for the execute method and an optional Func<bool> for the can execute method.
        /// If the can execute method is not provided, it defaults to always returning true.
		/// Throws an ArgumentNullException if the execute action is null,
        /// ensuring that a command always has an action to execute.
		/// </summary>
		/// <param name="execute"></param>
		/// <param name="canExecute"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute ?? (() => true);
        }

		/// <summary>
		/// This implementation ignores the parameter and always returns true,
		/// making the command always executable.
		/// To make use of the _canExecute delegate,
		/// you should replace return true; with return _canExecute();
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		public bool CanExecute(object? parameter)
        {
            return true;
        }

		/// <summary>
		/// Executes the _execute delegate. It doesn't use the parameter argument,
		/// meaning this command does not pass parameters to its execution logic
		/// </summary>
		/// <param name="parameter"></param>
		public void Execute(object? parameter)
        {
            _execute();
        }
    }
}
