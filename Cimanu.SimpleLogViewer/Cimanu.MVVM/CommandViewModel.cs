using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Cimanu.MVVM
{
    public class CommandViewModel : ICommand
    {
        protected Action<Object> action { get; }
        protected Func<object, bool> canExecute { get; }
        public event EventHandler<bool> Executed;

        public CommandViewModel(Action<Object> action, Func<object, bool> canExecute = null)
        {
            if (action == null)
                throw new ArgumentException("Action is null!");

            this.action = action;

            this.canExecute = (canExecute == null) ? ((x) => { return true; }) : canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            var commandTask = Task.Run(() => { action(parameter); });
        }
    }
}
