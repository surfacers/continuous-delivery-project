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
            get => isLoading;
            set => Set(ref isLoading, value);
        }

        private bool connectionError;
        public bool ConnectionError
        {
            get => connectionError;
            set => Set(ref connectionError, value);
        }

        public async void OnInit()
        {
            ConnectionError = false;
            IsLoading = true;
            try
            {
                await OnInitAsync();
            } 
            catch (Exception ex)
            {
                IsLoading = false;
                ConnectionError = true;
#if DEBUG
                throw ex;
#endif
            }
            IsLoading = false;
        }

        public abstract Task OnInitAsync();

        public abstract Task OnDestroyAsync();
    }
}

