using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Hurace.Mvvm.Commands
{
    public class AsyncDelegateCommand : ICommand
    {
        private readonly Func<Task> executeAsync;
        private readonly Func<bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public AsyncDelegateCommand(
            Func<Task> executeAsync,
            Func<bool> canExecute = null)
        {
            this.executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
            => this.canExecute == null || this.canExecute();

        public async void Execute(object parameter)
            => await this.executeAsync();
    }
}
