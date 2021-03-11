using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Hurace.Core.Models;
using Hurace.Data.Ado.Queries;
using SqlKata;
using SqlKata.Compilers;

namespace Hurace.Data.Ado.Managers
{
    public class StartListManager : GenericManager<StartList>, IStartListManager
    {
        public StartListManager(IMapper mapper, AdoManager ado, Compiler compiler)
            : base(mapper, ado, compiler)
        {
        }

        public async Task<IEnumerable<StartList>> GetByRaceIdAsync(int raceId, int runNumber)
        {
            Query query = Query()
                .Where(nameof(StartList.RaceId), raceId)
                .Where(nameof(StartList.RunNumber), runNumber);

            return await QueryAsync(query);
        }

        public async Task<bool> UpdateDisqualified(int id, bool isDisqualified)
        {
            Query query = Query(id).AsUpdate(nameof(StartList.IsDisqualified), isDisqualified);
            return (await ExecuteAsync(query)) == 1;
        }

        public async Task<bool> SaveAsync(int raceId, int runNumber, IEnumerable<StartList> list)
        {      
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                if (runNumber < 1 || runNumber > 2) throw new ArgumentException(nameof(runNumber));

                Query query = Query()
                    .Where(nameof(StartList.RaceId), raceId)
                    .Where(nameof(StartList.RunNumber), runNumber)
                    .AsDelete();

                await ExecuteAsync(query);

                int startNumber = 1;
                foreach (var entry in list)
                {
                    entry.Id = 0;
                    entry.RaceId = raceId;
                    entry.RunNumber = (byte)runNumber;
                    entry.StartNumber = startNumber;

                    await CreateAsync(entry);
                    if (entry.Id <= 0)
                    {
                        return false;
                    }

                    startNumber++;
                }

                scope.Complete();
                return true;
            }
        }

        public async Task<bool> RemoveAsync(int raceId)
        {
            Query query = Query().Where(nameof(StartList.RaceId), raceId).AsDelete();
            await ExecuteAsync(query);
            return true;
        }
    }
}
