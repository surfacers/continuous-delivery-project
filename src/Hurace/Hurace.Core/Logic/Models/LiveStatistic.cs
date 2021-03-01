using System;
using System.Collections.Generic;

namespace Hurace.Core.Logic.Models
{
    public class LiveStatistic
    {
        public int SkierId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CountryCode { get; set; }

        public int RaceId { get; set; }

        public int RunNumber { get; set; }

        public IEnumerable<LiveRaceData> RaceData { get; set; }

        public DateTime? TotalTime { get; set; }
    }
}
