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
            this.Parent = parent;
            this.UpdateNavigationTabs();
        }

        public void UpdateNavigationTabs()
        {
            var tabs = new List<NavigationItemViewModel<IRaceViewModel>>()
            {
                NavigationItemViewModel<IRaceViewModel>.Of<RaceDetailDataView, RaceDetailDataViewModel>("Data", this.Parent)
            };

            if (this.Parent?.Races?.Selected?.Race != null)
            {
                var race = this.Parent.Races.Selected.Race;

                if (race.Id != 0 && race.RaceState == RaceState.NotStarted)
                {
                    tabs.Add(NavigationItemViewModel<IRaceViewModel>.Of<RaceDetailStartListView, RaceDetailStartListViewModel>("Start List", this.Parent));
                }

                if (race.Id != 0 && race.RaceState != RaceState.NotStarted)
                {
                    string runTitle = race.HasSecondRun() ? "Run 1" : "Run";
                    tabs.Add(NavigationItemViewModel<IRaceViewModel>.Of<RaceDetailRunView, RaceDetailRunViewModel>(runTitle, this.Parent, 1));
                }

                if (race.Id != 0 && race.RaceState != RaceState.NotStarted && race.HasSecondRun())
                {
                    tabs.Add(NavigationItemViewModel<IRaceViewModel>.Of<RaceDetailRunView, RaceDetailRunViewModel>("Run 2", this.Parent, 2));
                }

                if (race.Id != 0 && race.RaceState != RaceState.NotStarted)
                {
                    tabs.Add(NavigationItemViewModel<IRaceViewModel>.Of<DisplayControlView, DisplayControlViewModel>("Display Control", this.Parent));
                }
            }

            this.NavigationViewModel.SetItems(tabs);
            this.Raise(nameof(this.NavigationViewModel));
        }
    }
}