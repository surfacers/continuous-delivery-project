using Hurace.Core.Models.Interfaces;

namespace Hurace.Core.Models
{
    public class Location : IEntity
    {
        public int Id { get; set; }
        public string CountryCode { get; set; }
        public string City { get; set; }
    }
}
