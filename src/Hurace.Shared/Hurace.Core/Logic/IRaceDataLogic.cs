using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Models;

namespace Hurace.Core.Logic
{
    public interface IRaceDataLogic
    {
        Task<bool> CreateAsync(IEnumerable<RaceData> raceData);
        Task<IEnumerable<RaceData>> GetByRaceIdAsync(int raceId, int runNumber);
    }
}
