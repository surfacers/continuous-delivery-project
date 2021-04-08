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
            get => this.currentRun;
            set => this.Set(ref this.currentRun, value);
        }

        public LiveViewModel()
        {
            this.clockService = App.Container.Resolve<IClockService>();
            this.CurrentRun = this.clockService.CurrentRun;
        }

        private void OnCurrentRunChange(StartListItemViewModel run)
        {
            if (run == null)
            {
                return;
            }

            this.CurrentRun = run;
        }

        public Task OnInitAsync()
        {
            this.clockService.OnCurrentRunChange += this.OnCurrentRunChange;
            return Task.CompletedTask;
        }

        public Task OnDestroyAsync()
        {
            this.clockService.OnCurrentRunChange -= this.OnCurrentRunChange;
            return Task.CompletedTask;
        }
    }
}
