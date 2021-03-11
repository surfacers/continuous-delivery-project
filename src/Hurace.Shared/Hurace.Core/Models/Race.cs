using System;
using Hurace.Core.Enums;
using Hurace.Core.Models.Interfaces;

namespace Hurace.Core.Models
{
    public class Race : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime RaceDate { get; set; } = DateTime.Now;
        public RaceType RaceType { get; set; } = RaceType.Slalom;
        public int LocationId { get; set; }
        public int SensorAmount { get; set; } = 5;
        public Gender Gender { get; set; }
        public RaceState RaceState { get; set; } = RaceState.NotStarted;
    }
}
