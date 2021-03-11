using System.Threading.Tasks;
using Hurace.Mvvm;
using Hurace.Mvvm.ViewModels;
using Hurace.RaceControl.Services;
using Hurace.RaceControl.ViewModels.Race.Detail;
using Unity;

namespace Hurace.RaceControl.ViewModels.DisplayControl.ControlPanel
{
    public class LiveViewModel : NotifyPropertyChanged, IComponentViewModel
    {
        private readonly IClockService clockService;

        private StartListItemViewModel currentRun;
        public StartListItemViewModel CurrentRun
        {
            get => currentRun;
            set => Set(ref currentRun, value);
        }

        public LiveViewModel()
        {
            clockService = App.Container.Resolve<IClockService>();
            CurrentRun = clockService.CurrentRun;
        }

        private void OnCurrentRunChange(StartListItemViewModel run)
        {
            if (run == null) return;

            CurrentRun = run;
        }

        public Task OnInitAsync()
        {
            clockService.OnCurrentRunChange += OnCurrentRunChange;
            return Task.CompletedTask;
        }

        public Task OnDestroyAsync()
        {
            clockService.OnCurrentRunChange -= OnCurrentRunChange;
            return Task.CompletedTask;
        }
    }
}
