using System;
using Hurace.Mvvm;
using Hurace.Mvvm.Enums;
using Hurace.Mvvm.ViewModels.Controls;

namespace Hurace.RaceControl.ViewModels.Race
{
    public class RaceListViewModel : NotifyPropertyChanged
    {
        public IRaceViewModel Parent { get; set; }
        
        public CommandViewModel NewCommandViewModel { get; }

        public CommandViewModel StopRaceCommandViewModel { get; }

        public RaceListViewModel(IRaceViewModel parent)
        {
            Parent = parent ?? throw new ArgumentNullException();

            NewCommandViewModel = new CommandViewModel(
                "New", "Create a new race",
                () => Parent.NewAsync());

            StopRaceCommandViewModel = new CommandViewModel(
                "Stop race", "Stop race",
                () => Parent.StopRaceAsync(),
                () => Parent.CanStopRaceAsync(),
                withStyle: ButtonStyle.Flat);
        }
    }
}