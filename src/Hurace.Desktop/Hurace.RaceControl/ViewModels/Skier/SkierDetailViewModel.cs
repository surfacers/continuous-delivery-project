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
            Parent = parent ?? throw new ArgumentNullException();
            CountryCodes = new ObservableCollection<string>();
            notificationService = App.Container.Resolve<INotificationService>();
            locationLogic = App.Container.Resolve<ILocationLogic>();

            SaveCommandViewModel = new CommandViewModel(
                "Save", "Save skier",
                async () => await Parent.SaveAsync(),
                () => !Parent.Edit?.HasErrors ?? false);

            SaveCommandViewModel.OnSuccess += () => notificationService.ShowMessage("Saved successfully");
            SaveCommandViewModel.OnFailure += (ex) => notificationService.ShowMessage("Save failed");

            RemoveCommandViewModel = new CommandViewModel(
                "Remove", "Remove skier",
                async () => await Parent.RemoveAsync(),
                withStyle: ButtonStyle.Flat);

            RemoveCommandViewModel.OnSuccess += () => notificationService.ShowMessage("Removed successfully");
            RemoveCommandViewModel.OnFailure += (ex) => notificationService.ShowMessage("Remove failed");
        }

        public async Task OnInitAsync()
        {
            string countryCode = Parent.Edit?.CountryCode;

            var countries = await locationLogic.GetCountriesAsync();
            CountryCodes.SetItems(countries);

            // Country Code must be reset manually because by calling CountryCodes.SetItems 
            // it gets deleted because its binded as Selected value in view.
            if (Parent.Edit != null)
            {
                Parent.Edit.CountryCode = CountryCodes.Where(c => c == countryCode).FirstOrDefault();
            }
        }
    }
}
