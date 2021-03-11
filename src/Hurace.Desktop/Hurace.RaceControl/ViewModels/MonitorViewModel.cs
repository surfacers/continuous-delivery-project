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
            View = view ?? throw new ArgumentNullException();
            Parent = parent ?? throw new ArgumentNullException();
            ViewModel = viewModel ?? throw new ArgumentNullException();

            View.DataContext = viewModel;
        }
    }
}
