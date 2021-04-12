using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Hurace.Core.Enums;
using Hurace.Core.Extensions;
using Hurace.Core.Models;
using Hurace.Mvvm;
using Models = Hurace.Core.Models;

namespace Hurace.RaceControl.ViewModels.Race.Detail
{
    public class StartListItemViewModel : NotifyPropertyChanged
    {
        public Models.Skier Skier { get; private set; }
        public Models.StartList StartList { get; private set; }
        public ObservableCollection<RaceDataItemViewModel> RaceData { get; private set; }
            = new ObservableCollection<RaceDataItemViewModel>();
        public DateTime? TotalTime => this.RaceData.LastOrDefault()?.TotalTime;

        private int? position;
        public int? Position
        {
            get => this.position;
            set => this.Set(ref this.position, value);
        }

        private DateTime? diffTime;
        public DateTime? DiffTime
        {
            get => this.diffTime;
            set => this.Set(ref this.diffTime, value);
        }

        public string FullName => this.Skier.FullName();
        public string CountryCode => this.Skier.CountryCode;
        public int StartNumber => this.StartList.StartNumber;
        public bool IsDisqualified => this.StartList.IsDisqualified;

        private StartListState startListState;
        public StartListState StartListState
        {
            get => this.startListState;
            set => this.Set(ref this.startListState, value);
        }

        public StartListItemViewModel(Models.Skier skier, Models.StartList startList)
        {
            this.Skier = skier;
            this.StartList = startList;
        }

        public void Update(Models.Skier skier, Models.StartList startList)
        {
            this.Skier = skier;
            this.Raise(nameof(this.FullName));
            this.Raise(nameof(this.CountryCode));

            this.StartList = startList;
            this.Raise(nameof(this.StartNumber));
            this.Raise(nameof(this.IsDisqualified));
        }

        public void SetRaceData(
            IEnumerable<RaceData> raceData, 
            int sensorAmount, 
            int runningStartListId)
        {
            DateTime? startTime = raceData.FirstOrDefault()?.TimeStamp ?? null;
            this.RaceData.SetItems(raceData, r => new RaceDataItemViewModel(r, startTime));
            this.StartListState = this.StartList.GetStartListState(raceData, sensorAmount, runningStartListId);
        }
    }
}
