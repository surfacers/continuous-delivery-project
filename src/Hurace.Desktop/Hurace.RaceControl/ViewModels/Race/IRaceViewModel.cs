using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Hurace.Core.Enums;
using Hurace.Core.Models;
using Hurace.Mvvm.ViewModels;
using Hurace.RaceControl.ViewModels.Controls;
using Hurace.RaceControl.ViewModels.Race.Detail;
using Models = Hurace.Core.Models;

namespace Hurace.RaceControl.ViewModels.Race
{
    public interface IRaceViewModel
    {
        FilterViewModel<RaceListItemViewModel> Races { get; set; }

        RaceEditViewModel Edit { get; set; }

        IEnumerable<Models.Skier> AllSkiers { get; set; }

        StartListItemViewModel SelectedStartListItem { get; set; }

        ObservableCollection<StartListItemViewModel> StartList1 { get; set; }

        ObservableCollection<StartListItemViewModel> StartList2 { get; set; }

        ObservableCollection<ComboBoxItemViewModel<int>> Locations { get; set; }

        ObservableCollection<ComboBoxItemViewModel<RaceType>> RaceTypes { get; set; }

        Task SaveAsync(ObservableCollection<StartListItemViewModel> startListViewModel);

        Task RemoveAsync();

        Task NewAsync();

        Task StartRaceAsync();

        Task StopRaceAsync();

        bool CanStopRaceAsync();

        StartListItemViewModel ToStartListItemViewModel(StartList startList);

        Task GrantRunAsync();

        Task DisqualifyAsync();
    }
}
