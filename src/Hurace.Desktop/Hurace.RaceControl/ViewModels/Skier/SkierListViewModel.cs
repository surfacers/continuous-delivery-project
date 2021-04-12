using System;
using System.Windows.Input;
using Hurace.Mvvm;
using Hurace.Mvvm.Commands;

namespace Hurace.RaceControl.ViewModels.Skier
{
    public class SkierListViewModel : NotifyPropertyChanged
    {
        public ISkierViewModel Parent { get; set; }

        public ICommand NewCommand { get; }

        public SkierListViewModel(ISkierViewModel parent)
        {
            this.Parent = parent ?? throw new ArgumentNullException();
            this.NewCommand = new AsyncDelegateCommand(this.Parent.NewAsync);
        }
    }
}
