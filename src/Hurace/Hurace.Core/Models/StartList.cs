using Hurace.Core.Models.Interfaces;

namespace Hurace.Core.Models
{
    public class StartList : IEntity
    {
        public int Id { get; set; }
        public int SkierId { get; set; }
        public int RaceId { get; set; }
        public int StartNumber { get; set; }
        public byte RunNumber { get; set; }
        public bool IsDisqualified { get; set; } = false;
    }
}
