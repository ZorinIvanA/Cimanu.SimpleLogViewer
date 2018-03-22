using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Cimanu.MVVM
{
    /// <summary>
    /// Command view model
    /// </summary>
    public class CommandViewModel : ICommand
    {
        /// <summary>
        /// Command action
        /// </summary>
        protected Action<Object> _action { get; }

        /// <summary>
        /// Command execution ability calculation
        /// </summary>
        protected Func<object, bool> _canExecute { get; }

        /// <summary>
        /// Raises when command has been executed
        /// </summary>
        public event EventHandler<bool> Executed;

        /// <summary>
        /// Creates Command view model instance
        /// </summary>
        /// <param name="action">Command action</param>
        /// <param name="canExecute">Execute ability calc</param>
        public CommandViewModel(Action<Object> action, Func<object, bool> canExecute = null)
        {
            if (action == null)
                throw new ArgumentException("Action is null!");

            this._action = action;

            this._canExecute = (canExecute == null) ? ((x) => { return true; }) : canExecute;
        }

        /// <summary>
        /// Can execute change event
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Obtains, can be command executed
        /// </summary>
        /// <param name="parameter">Arg</param>
        /// <returns>If command can be executed</returns>
        public bool CanExecute(object parameter)
        {
            var result = _canExecute(parameter);
            var x = CanExecuteChanged;
            x?.Invoke(this, EventArgs.Empty);
            return result;
        }

        /// <summary>
        /// Executes command in separate thread
        /// </summary>
        /// <param name="parameter">Command arg</param>
        public void Execute(object parameter)
        {
            var commandTask = Task.Run(() => { _action(parameter); });
            commandTask.ContinueWith((x) =>
            {
                RaiseExecuted(true);
            }, TaskContinuationOptions.NotOnFaulted);
            commandTask.ContinueWith((x) =>
            {
                RaiseExecuted(false);
            }, TaskContinuationOptions.OnlyOnFaulted);
        }

        private void RaiseExecuted(Boolean hasError)
        {
            var x = Executed;
            x?.Invoke(this, hasError);
        }
    }
}
