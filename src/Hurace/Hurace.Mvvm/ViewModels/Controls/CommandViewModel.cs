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
            get => content;
            set => Set(ref content, value);
        }


        public string ToolTip { get; }

        public ButtonStyle ButtonStyle { get; }

        public ICommand ActionCommand { get; }

        private Func<bool> showCommand;
        public Func<bool> ShowCommand
        {
            get => showCommand;
            set => Set(ref showCommand, value);
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
            Content = content;
            ToolTip = toolTip;
            ButtonStyle = withStyle;

            ShowCommand = show != null 
                ? show 
                : () => true;

            ActionCommand = new AsyncDelegateCommand(
                async () =>
                {
                    try
                    {
                        await execute();
                        OnSuccess?.Invoke();
                        CommandManager.InvalidateRequerySuggested();
                    }
                    catch (Exception ex)
                    {
                        OnFailure?.Invoke(ex);
#if DEBUG
                        throw ex;
#endif
                    }
                },
                canExecute);
        }
    }
}
