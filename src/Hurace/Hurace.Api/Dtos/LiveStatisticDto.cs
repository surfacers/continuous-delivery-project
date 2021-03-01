using System.Collections.Generic;

namespace Hurace.Api.Dtos
{
    public class LiveStatisticDto
    {
        public int SkierId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CountryCode { get; set; }

        public int RaceId { get; set; }

        public int RunNumber { get; set; }

        public IEnumerable<LiveRaceDataDto> RaceData { get; set; }

        public string TotalTime { get; set; }
    }
}
