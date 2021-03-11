using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Enums;
using Hurace.Core.Models;

namespace Hurace.Data.Ado
{
    public interface IRaceManager
    {
        Task<Race> GetByIdAsync(int id);
        Task<IEnumerable<Race>> GetAllAsync();
        Task<IEnumerable<Race>> GetByRaceStateAsync(RaceState raceState);
        Task CreateAsync(Race race);
        Task<bool> UpdateAsync(Race race);
        Task<bool> RemoveAsync(int id);
        Task<bool> UpdateRaceState(int id, RaceState raceState);
    }
}
