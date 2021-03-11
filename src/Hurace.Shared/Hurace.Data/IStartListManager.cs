using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Models;

namespace Hurace.Data
{
    public interface IStartListManager
    {
        Task<StartList> GetByIdAsync(int id);
        Task<IEnumerable<StartList>> GetByRaceIdAsync(int raceId, int runNumber);
        Task<bool> SaveAsync(int raceId, int runNumber, IEnumerable<StartList> list);
        Task<bool> RemoveAsync(int raceId);
        Task<bool> UpdateDisqualified(int id, bool isDisqualified);
    }
}
