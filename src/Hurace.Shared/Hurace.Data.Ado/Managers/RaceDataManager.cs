using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Hurace.Core.Models;
using Hurace.Data.Ado.Queries;
using SqlKata.Compilers;

namespace Hurace.Data.Ado.Managers
{
    public class RaceDataManager : GenericManager<RaceData>, IRaceDataManager
    {
        public RaceDataManager(IMapper mapper, AdoManager ado, Compiler compiler)
            : base(mapper, ado, compiler)
        {
        }

        public async Task<IEnumerable<RaceData>> GetByRaceIdAsync(int raceId, int runNumber)
        {
            var query = this.Query()
                .SelectOnly<RaceData>()
                .Join(
                    nameof(StartList),
                    $"{nameof(StartList)}.{nameof(StartList.Id)}",
                    $"{nameof(RaceData)}.{nameof(RaceData.StartListId)}")
                .Where($"{nameof(StartList)}.{nameof(StartList.RaceId)}", raceId)
                .Where($"{nameof(StartList)}.{nameof(StartList.RunNumber)}", runNumber);

            return await this.QueryAsync(query);
        }
    }
}
