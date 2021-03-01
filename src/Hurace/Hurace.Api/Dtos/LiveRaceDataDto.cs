using System;

namespace Hurace.Api.Dtos
{
    public class LiveRaceDataDto
    {
        public byte SensorId { get; set; }

        public DateTime TimeStamp { get; set; }

        public string TotalTime { get; set; }
    }
}
