using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Hurace.Core.Logic;
using Hurace.Mvvm.Enums;
using Hurace.Mvvm.ViewModels.Controls;
using Hurace.RaceControl.Services;
using Hurace.RaceControl.ViewModels.Controls;
using Unity;

namespace Hurace.RaceControl.ViewModels.Race.Detail
{
    public class RaceDetailDataViewModel : TabViewModel<IRaceViewModel>
    {
        [Dependency] public IRaceLogic RaceLogic { get; set; }

        public ObservableCollection<StartListItemViewModel> StartList => Parent.StartList1;

        public INotificationService NotificationService { get; }

        public CommandViewModel SaveCommandViewModel { get; }
        public CommandViewModel RemoveCommandViewModel { get; }

        public RaceDetailDataViewModel()
        {
            NotificationService = App.Container.Resolve<INotificationService>();

            SaveCommandViewModel = new CommandViewModel(
                "Save", "Save race",
                async () => await Parent.SaveAsync(Parent.StartList1),
                () => !Parent.Edit?.HasErrors ?? false);

            SaveCommandViewModel.OnSuccess += () => NotificationService.ShowMessage("Saved successfully");
            SaveCommandViewModel.OnFailure += (ex) => NotificationService.ShowMessage("Save failed");

            RemoveCommandViewModel = new CommandViewModel(
                "Remove", "Remove race",
                async () => await Parent.RemoveAsync(),
                show: () => Parent.Races.Selected?.Race != null 
                    ? RaceLogic.CanRemove(Parent.Races.Selected.Race)
                    : true,
                withStyle: ButtonStyle.Flat);

            RemoveCommandViewModel.OnSuccess += () => NotificationService.ShowMessage("Removed successfully");
            RemoveCommandViewModel.OnFailure += (ex) => NotificationService.ShowMessage("Remove failed");
        }

        public override Task OnInitAsync()
        {
            return Task.CompletedTask;
        }

        public override Task OnDestroyAsync()
        {
            return Task.CompletedTask;
        }
    }
}
