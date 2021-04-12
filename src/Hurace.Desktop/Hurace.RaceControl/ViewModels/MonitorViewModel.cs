using System;
using System.Windows.Controls;
using Hurace.Mvvm.ViewModels;
using Hurace.RaceControl.ViewModels.Race;

namespace Hurace.RaceControl.ViewModels
{
    public class MonitorViewModel
    {
        public Control View { get; set; }

        public IRaceViewModel Parent { get; set; }

        public IComponentViewModel ViewModel { get; set; }

        public MonitorViewModel(
            Control view,
            IComponentViewModel viewModel,
            IRaceViewModel parent)
        {
            this.View = view ?? throw new ArgumentNullException();
            this.Parent = parent ?? throw new ArgumentNullException();
            this.ViewModel = viewModel ?? throw new ArgumentNullException();

            this.View.DataContext = viewModel;
        }
    }
}
