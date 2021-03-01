using System;
using System.Windows.Input;

namespace Hurace.Mvvm.Commands
{
    public class DelegateParameterCommand<T> : ICommand
    {
        private readonly Action<T> execute;
        private readonly Predicate<T> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public DelegateParameterCommand(
            Action<T> executeAsync,
            Predicate<T> canExecute = null)
        {
            this.execute = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
         => canExecute == null || canExecute((T)parameter);

        public void Execute(object parameter)
            => execute((T)parameter);
    }
}
