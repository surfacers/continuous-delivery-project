using Hurace.Core.Extensions;
using Hurace.Mvvm;
using Models = Hurace.Core.Models;

namespace Hurace.RaceControl.ViewModels.Skier
{
    public class SkierListItemViewModel : NotifyPropertyChanged
    {
        public Models.Skier Skier { get; private set; }

        public string FullName => this.Skier.FullName();
        public string CountryCode => this.Skier.CountryCode;
        public bool IsActive => this.Skier.IsActive;

        public SkierListItemViewModel(Models.Skier skier)
        {
            this.Skier = skier;
        }

        public void Update(Models.Skier skier)
        {
            this.Skier = skier;
            this.Raise(nameof(this.FullName));
            this.Raise(nameof(this.CountryCode));
            this.Raise(nameof(this.IsActive));
        }
    }
}
