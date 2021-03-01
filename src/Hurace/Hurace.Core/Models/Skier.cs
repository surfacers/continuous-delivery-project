using System;
using Hurace.Core.Enums;
using Hurace.Core.Models.Interfaces;

namespace Hurace.Core.Models
{
    public class Skier : IEntity, IRemovable
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; } = Gender.Male;
        public string CountryCode { get; set; }
        public DateTime? BirthDate { get; set; } = null;
        public string Image { get; set; }
        public bool IsRemoved { get; set; } = false;
        public bool IsActive { get; set; } = true; // Has active career
    }
}
