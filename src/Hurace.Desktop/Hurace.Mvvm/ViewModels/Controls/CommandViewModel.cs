using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Hurace.Mvvm.Commands;
using Hurace.Mvvm.Enums;

namespace Hurace.Mvvm.ViewModels.Controls
{
    public class CommandViewModel : NotifyPropertyChanged
    {
        private string content;
        public string Content
        {
            get => this.content;
            set => this.Set(ref this.content, value);
        }

        public string ToolTip { get; }

        public ButtonStyle ButtonStyle { get; }

        public ICommand ActionCommand { get; }

        private Func<bool> showCommand;
        public Func<bool> ShowCommand
        {
            get => this.showCommand;
            set => this.Set(ref this.showCommand, value);
        }

        public event Action OnSuccess;

        public event Action<Exception> OnFailure;

        public CommandViewModel(
            string content,
            string toolTip,
            Func<Task> execute, 
            Func<bool> canExecute = null,
            Func<bool> show = null,
            ButtonStyle withStyle = ButtonStyle.Raised)
        {
            this.Content = content;
            this.ToolTip = toolTip;
            this.ButtonStyle = withStyle;

            this.ShowCommand = show != null 
                ? show 
                : () => true;

            this.ActionCommand = new AsyncDelegateCommand(
                async () =>
                {
                    try
                    {
                        await execute();
                        this.OnSuccess?.Invoke();
                        CommandManager.InvalidateRequerySuggested();
                    }
                    catch (Exception ex)
                    {
                        this.OnFailure?.Invoke(ex);
#if DEBUG
                        throw ex;
#endif
                    }
                },
                canExecute);
        }
    }
}
