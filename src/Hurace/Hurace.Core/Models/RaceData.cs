using System;
using Hurace.Core.Models.Interfaces;

namespace Hurace.Core.Models
{
    public class RaceData : IEntity
    {
        public int Id { get; set; }
        public int StartListId { get; set; }
        public byte SensorId { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
