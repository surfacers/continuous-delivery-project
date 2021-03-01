using System;
using Hurace.Core.Extensions;
using Hurace.Core.Logic.Models;
using Models = Hurace.Core.Models;

namespace Hurace.RaceControl.ViewModels.DisplayControl.ControlPanel
{
    public class StatsItemViewModel
    {
        public StatsItemViewModel(RaceStatisticEntry raceStatisticEntry, Models.Skier skier)
        {
            CurrentPosition = raceStatisticEntry.CurrentPosition;
            DeltaPosition = raceStatisticEntry.DeltaPosition;
            SkierFullname = skier.FullName();
            SkierCountryCode = skier.CountryCode;
            Time = raceStatisticEntry.Time;
            DeltaTimeLeadership = raceStatisticEntry.DeltaTimeLeadership;
        }

        public int CurrentPosition { get; set; }
        public int? DeltaPosition { get; set; }
        public string SkierFullname { get; set; }
        public string SkierCountryCode { get; set; }
        public TimeSpan Time { get; set; }
        public TimeSpan DeltaTimeLeadership { get; set; }
    }
}
