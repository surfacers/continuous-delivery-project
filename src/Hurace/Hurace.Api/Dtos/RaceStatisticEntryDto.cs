using System;

namespace Hurace.Api.Dtos
{
    public class RaceStatisticEntryDto
    {
        public int CurrentPosition { get; set; }
        public int? DeltaPosition { get; set; }
        public int SkierId { get; set; }
        public string Time { get; set; }
        public string DeltaTimeLeadership { get; set; }
    }
}
