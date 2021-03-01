using Hurace.Core.Extensions;
using Hurace.Mvvm;
using Models = Hurace.Core.Models;

namespace Hurace.RaceControl.ViewModels.Skier
{
    public class SkierListItemViewModel : NotifyPropertyChanged
    {
        public Models.Skier Skier { get; private set; }

        public string FullName => Skier.FullName();
        public string CountryCode => Skier.CountryCode;
        public bool IsActive => Skier.IsActive;

        public SkierListItemViewModel(Models.Skier skier)
        {
            Skier = skier;
        }

        public void Update(Models.Skier skier)
        {
            Skier = skier;
            Raise(nameof(FullName));
            Raise(nameof(CountryCode));
            Raise(nameof(IsActive));
        }
    }
}
