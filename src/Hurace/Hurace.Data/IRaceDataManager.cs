using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Models;

namespace Hurace.Data
{
    public interface IRaceDataManager
    {
        Task CreateAsync(RaceData raceData);
        Task<IEnumerable<RaceData>> GetByRaceIdAsync(int raceId, int runNumber);
    }
}
