using System;
using System.Windows.Input;

namespace Hurace.Mvvm.Commands
{
    public class DelegateCommand : ICommand
    {
        private readonly Action execute;
        private readonly Func<bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public DelegateCommand(
            Action executeAsync,
            Func<bool> canExecute = null)
        {
            this.execute = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
         => this.canExecute == null || this.canExecute();

        public void Execute(object parameter) => this.execute();
    }
}
