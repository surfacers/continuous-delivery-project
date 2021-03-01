using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Logic.Models;

namespace Hurace.Core.Logic
{
    public interface IStatisticLogic
    {
        Task<IEnumerable<RaceStatisticEntry>> GetRaceStatistics(int raceId, int runNumber, int sensorAmount);
    }
}
