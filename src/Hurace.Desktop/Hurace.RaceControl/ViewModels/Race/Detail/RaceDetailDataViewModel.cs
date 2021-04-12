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

        public ObservableCollection<StartListItemViewModel> StartList => this.Parent.StartList1;

        public INotificationService NotificationService { get; }

        public CommandViewModel SaveCommandViewModel { get; }
        public CommandViewModel RemoveCommandViewModel { get; }

        public RaceDetailDataViewModel()
        {
            this.NotificationService = App.Container.Resolve<INotificationService>();

            this.SaveCommandViewModel = new CommandViewModel(
                "Save", 
                "Save race",
                async () => await this.Parent.SaveAsync(this.Parent.StartList1),
                () => !this.Parent.Edit?.HasErrors ?? false);

            this.SaveCommandViewModel.OnSuccess += () => this.NotificationService.ShowMessage("Saved successfully");
            this.SaveCommandViewModel.OnFailure += (ex) => this.NotificationService.ShowMessage("Save failed");

            this.RemoveCommandViewModel = new CommandViewModel(
                "Remove", 
                "Remove race",
                async () => await this.Parent.RemoveAsync(),
                show: () => this.Parent.Races.Selected?.Race != null 
                    ? this.RaceLogic.CanRemove(this.Parent.Races.Selected.Race)
                    : true,
                withStyle: ButtonStyle.Flat);

            this.RemoveCommandViewModel.OnSuccess += () => this.NotificationService.ShowMessage("Removed successfully");
            this.RemoveCommandViewModel.OnFailure += (ex) => this.NotificationService.ShowMessage("Remove failed");
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
