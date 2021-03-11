using System.Collections.Generic;
using Hurace.Core.Enums;
using Hurace.Core.Extensions;
using Hurace.Mvvm;
using Hurace.RaceControl.ViewModels.Controls;
using Hurace.RaceControl.ViewModels.DisplayControl;
using Hurace.RaceControl.ViewModels.Race.Detail;
using Hurace.RaceControl.Views.DisplayControl;
using Hurace.RaceControl.Views.Race.Detail;

namespace Hurace.RaceControl.ViewModels.Race
{
    public class RaceDetailViewModel : NotifyPropertyChanged
    {
        public IRaceViewModel Parent { get; set; }
        public NavigationControlViewModel<IRaceViewModel> NavigationViewModel { get; set; }
            = new NavigationControlViewModel<IRaceViewModel>();

        public RaceDetailViewModel(IRaceViewModel parent)
        {
            Parent = parent;
            UpdateNavigationTabs();
        }

        public void UpdateNavigationTabs()
        {
            var tabs = new List<NavigationItemViewModel<IRaceViewModel>>()
            {
                NavigationItemViewModel<IRaceViewModel>.Of<RaceDetailDataView, RaceDetailDataViewModel>("Data", Parent)
            };

            if (Parent?.Races?.Selected?.Race != null)
            {
                var race = Parent.Races.Selected.Race;

                if (race.Id != 0 && race.RaceState == RaceState.NotStarted)
                {
                    tabs.Add(NavigationItemViewModel<IRaceViewModel>.Of<RaceDetailStartListView, RaceDetailStartListViewModel>("Start List", Parent));
                }

                if (race.Id != 0 && race.RaceState != RaceState.NotStarted)
                {
                    string runTitle = race.HasSecondRun() ? "Run 1" : "Run";
                    tabs.Add(NavigationItemViewModel<IRaceViewModel>.Of<RaceDetailRunView, RaceDetailRunViewModel>(runTitle, Parent, 1));
                }

                if (race.Id != 0 && race.RaceState != RaceState.NotStarted && race.HasSecondRun())
                {
                    tabs.Add(NavigationItemViewModel<IRaceViewModel>.Of<RaceDetailRunView, RaceDetailRunViewModel>("Run 2", Parent, 2));
                }

                if (race.Id != 0 && race.RaceState != RaceState.NotStarted)
                {
                    tabs.Add(NavigationItemViewModel<IRaceViewModel>.Of<DisplayControlView, DisplayControlViewModel>("Display Control", Parent));
                }
            }

            NavigationViewModel.SetItems(tabs);
            Raise(nameof(NavigationViewModel));
        }
    }
}