using System;

namespace Hurace.Core.Logic.Models
{
    public class RaceStatisticEntry
    {
        public int CurrentPosition { get; set; }
        public int? DeltaPosition { get; set; }
        public int SkierId { get; set; }
        public TimeSpan Time { get; set; }
        public TimeSpan DeltaTimeLeadership { get; set; }
    }
}
