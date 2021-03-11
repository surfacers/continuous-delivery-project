using Hurace.Mvvm;
using Enums = Hurace.Core.Enums;
using Models = Hurace.Core.Models;

namespace Hurace.RaceControl.ViewModels.Race
{
    public class RaceListItemViewModel : NotifyPropertyChanged
    {
        public Models.Race Race { get; private set; }

        public string Name => Race.Name;
        public string RaceDate => Race.RaceDate.ToShortDateString();
        public Enums.Gender Gender => Race.Gender;
        public Enums.RaceType RaceType => Race.RaceType;
        public Enums.RaceState RaceState => Race.RaceState;
        
        public RaceListItemViewModel(Models.Race race)
        {
            Race = race;
        }

        public void Update(Models.Race race)
        {
            Race = race;
            Raise(nameof(Name));
            Raise(nameof(RaceDate));
            Raise(nameof(Gender));
            Raise(nameof(RaceType));
            Raise(nameof(RaceState));
        }
    }
}
