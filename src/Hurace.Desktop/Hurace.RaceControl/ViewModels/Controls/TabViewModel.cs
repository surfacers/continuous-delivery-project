using System;
using System.Threading.Tasks;
using System.Windows.Threading;
using Hurace.Mvvm;
using Hurace.Mvvm.ViewModels;

namespace Hurace.RaceControl.ViewModels.Controls
{
    public abstract class TabViewModel<TParentViewModel> : NotifyPropertyChanged, IComponentViewModel
    {
        public object RouteData { get; set; }

        public TParentViewModel Parent { get; set; }

        public Dispatcher Dispatcher { get; set; }

        private bool isLoading;
        public bool IsLoading
        {
            get => this.isLoading;
            set => this.Set(ref this.isLoading, value);
        }

        private bool connectionError;
        public bool ConnectionError
        {
            get => this.connectionError;
            set => this.Set(ref this.connectionError, value);
        }

        public async void OnInit()
        {
            this.ConnectionError = false;
            this.IsLoading = true;
            try
            {
                await this.OnInitAsync();
            } 
            catch (Exception)
            {
                this.IsLoading = false;
                this.ConnectionError = true;
#if DEBUG
                throw ex;
#endif
            }

            this.IsLoading = false;
        }

        public abstract Task OnInitAsync();

        public abstract Task OnDestroyAsync();
    }
}
