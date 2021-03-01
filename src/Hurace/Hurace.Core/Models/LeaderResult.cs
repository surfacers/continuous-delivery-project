using System;
using System.Collections.Generic;

namespace Hurace.Core.Models
{
    public class LeaderResult
    {
        public int SkierId { get; set; }

        public TimeSpan ResultTime { get; set; }

        public IEnumerable<RaceData> TimeStamps { get; set; }
    }
}
