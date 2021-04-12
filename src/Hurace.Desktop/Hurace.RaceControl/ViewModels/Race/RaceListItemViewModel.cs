using Hurace.Mvvm;
using Enums = Hurace.Core.Enums;
using Models = Hurace.Core.Models;

namespace Hurace.RaceControl.ViewModels.Race
{
    public class RaceListItemViewModel : NotifyPropertyChanged
    {
        public Models.Race Race { get; private set; }

        public string Name => this.Race.Name;
        public string RaceDate => this.Race.RaceDate.ToShortDateString();
        public Enums.Gender Gender => this.Race.Gender;
        public Enums.RaceType RaceType => this.Race.RaceType;
        public Enums.RaceState RaceState => this.Race.RaceState;
        
        public RaceListItemViewModel(Models.Race race)
        {
            this.Race = race;
        }

        public void Update(Models.Race race)
        {
            this.Race = race;
            this.Raise(nameof(this.Name));
            this.Raise(nameof(this.RaceDate));
            this.Raise(nameof(this.Gender));
            this.Raise(nameof(this.RaceType));
            this.Raise(nameof(this.RaceState));
        }
    }
}
