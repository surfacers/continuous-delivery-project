using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Extensions;
using Hurace.Core.Logic;
using Hurace.Mvvm;
using Hurace.Mvvm.Enums;
using Hurace.Mvvm.ViewModels.Controls;
using Hurace.RaceControl.Services;
using Unity;

namespace Hurace.RaceControl.ViewModels.Skier
{
    public class SkierDetailViewModel : NotifyPropertyChanged
    {
        private readonly INotificationService notificationService;
        private readonly ILocationLogic locationLogic;

        public ISkierViewModel Parent { get; set; }

        public ObservableCollection<string> CountryCodes { get; set; }

        public CommandViewModel SaveCommandViewModel { get; }
        public CommandViewModel RemoveCommandViewModel { get; }

        public SkierDetailViewModel(ISkierViewModel parent)
        {
            this.Parent = parent ?? throw new ArgumentNullException();
            this.CountryCodes = new ObservableCollection<string>();
            this.notificationService = App.Container.Resolve<INotificationService>();
            this.locationLogic = App.Container.Resolve<ILocationLogic>();

            this.SaveCommandViewModel = new CommandViewModel(
                "Save", 
                "Save skier",
                async () => await this.Parent.SaveAsync(),
                () => !this.Parent.Edit?.HasErrors ?? false);

            this.SaveCommandViewModel.OnSuccess += () => this.notificationService.ShowMessage("Saved successfully");
            this.SaveCommandViewModel.OnFailure += (ex) => this.notificationService.ShowMessage("Save failed");

            this.RemoveCommandViewModel = new CommandViewModel(
                "Remove", 
                "Remove skier",
                async () => await this.Parent.RemoveAsync(),
                withStyle: ButtonStyle.Flat);

            this.RemoveCommandViewModel.OnSuccess += () => this.notificationService.ShowMessage("Removed successfully");
            this.RemoveCommandViewModel.OnFailure += (ex) => this.notificationService.ShowMessage("Remove failed");
        }

        public async Task OnInitAsync()
        {
            string countryCode = this.Parent.Edit?.CountryCode;

            var countries = await this.locationLogic.GetCountriesAsync();
            this.CountryCodes.SetItems(countries);

            // Country Code must be reset manually because by calling CountryCodes.SetItems 
            // it gets deleted because its binded as Selected value in view.
            if (this.Parent.Edit != null)
            {
                this.Parent.Edit.CountryCode = this.CountryCodes.Where(c => c == countryCode).FirstOrDefault();
            }
        }
    }
}
