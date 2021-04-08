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
            this.Parent = parent ?? throw new ArgumentNullException();

            this.NewCommandViewModel = new CommandViewModel(
                "New", 
                "Create a new race",
                () => this.Parent.NewAsync());

            this.StopRaceCommandViewModel = new CommandViewModel(
                "Stop race", 
                "Stop race",
                () => this.Parent.StopRaceAsync(),
                () => this.Parent.CanStopRaceAsync(),
                withStyle: ButtonStyle.Flat);
        }
    }
}