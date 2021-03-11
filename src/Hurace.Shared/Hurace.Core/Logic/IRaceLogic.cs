using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Enums;
using Hurace.Core.Logic.Models;
using Hurace.Core.Models;

namespace Hurace.Core.Logic
{
    public interface IRaceLogic
    {
        Task<IEnumerable<Race>> GetAllAsync();

        Task<IEnumerable<Race>> GetByRaceStateAsync(RaceState raceState);

        Task<Race> GetByIdAsync(int id);

        bool CanRemove(Race race);

        Task<bool> RemoveAsync(Race race);

        Task<SaveResult> SaveAsync(Race race);

        Task<SaveResult> SaveAsync(Race race, int runNumber, IEnumerable<StartList> startList); // Create or update race

        Task<bool> StartAsync(int raceId);
    }
}
