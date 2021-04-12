using System;
using Hurace.Core.Models;
using Hurace.Mvvm;

namespace Hurace.RaceControl.ViewModels.Race.Detail
{
    public class RaceDataItemViewModel : NotifyPropertyChanged
    {
        public RaceData RaceData { get; private set; }

        public byte SensorId => this.RaceData.SensorId;
        public DateTime TimeStamp => this.RaceData.TimeStamp;
        public DateTime TotalTime { get; }

        public RaceDataItemViewModel(RaceData raceData, DateTime? startTime)
        {
            this.RaceData = raceData;
            this.TotalTime = startTime == null
                ? new DateTime(0)
                : new DateTime((raceData.TimeStamp - (DateTime)startTime).Ticks);
        }
    }
}
