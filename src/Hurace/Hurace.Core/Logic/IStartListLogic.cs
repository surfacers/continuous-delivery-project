using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Logic.Models;
using Hurace.Core.Models;

namespace Hurace.Core.Logic
{
    public interface IStartListLogic
    {
        Task<SaveResult> SaveAsync(int raceId, int runNumber, IEnumerable<StartList> startList);
        Task<IEnumerable<StartList>> GetByRaceIdAsync(int raceId, int runNumber);
        Task<bool> UpdateDisqualified(int id, bool isDisqualified);
        Task<IEnumerable<StartList>> GenerateStartListForRunAsync(int raceId, int runNumber);
    }
}
