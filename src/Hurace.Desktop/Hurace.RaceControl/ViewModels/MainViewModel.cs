using Hurace.RaceControl.Services;
using Hurace.RaceControl.ViewModels.Controls;
using Hurace.RaceControl.ViewModels.Race;
using Hurace.RaceControl.ViewModels.Simulator;
using Hurace.RaceControl.ViewModels.Skier;
using Hurace.RaceControl.Views.Race;
using Hurace.RaceControl.Views.Simulator;
using Hurace.RaceControl.Views.Skier;
using Unity;

namespace Hurace.RaceControl.ViewModels
{
    public class MainViewModel
    {
        public NavigationControlViewModel<MainViewModel> NavigationViewModel { get; set; }

        [Dependency] public INotificationService NotificationService { get; set; }

        public MainViewModel()
        {
            this.NavigationViewModel = new NavigationControlViewModel<MainViewModel>(new[]
            {
                NavigationItemViewModel<MainViewModel>.Of<SkierView, SkierViewModel>("Skier", this),
                NavigationItemViewModel<MainViewModel>.Of<RaceView, RaceViewModel>("Race", this),
                NavigationItemViewModel<MainViewModel>.Of<SimulatorView, SimulatorViewModel>("Simulator", this)
            });
        }
    }
}
