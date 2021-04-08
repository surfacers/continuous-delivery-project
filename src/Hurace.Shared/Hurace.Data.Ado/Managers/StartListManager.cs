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
            Query query = this.Query()
                .Where(nameof(StartList.RaceId), raceId)
                .Where(nameof(StartList.RunNumber), runNumber);

            return await this.QueryAsync(query);
        }

        public async Task<bool> UpdateDisqualified(int id, bool isDisqualified)
        {
            Query query = this.Query(id).AsUpdate(nameof(StartList.IsDisqualified), isDisqualified);
            return (await this.ExecuteAsync(query)) == 1;
        }

        public async Task<bool> SaveAsync(int raceId, int runNumber, IEnumerable<StartList> list)
        {      
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                if (runNumber < 1 || runNumber > 2)
                {
                    throw new ArgumentException(nameof(runNumber));
                }

                Query query = this.Query()
                    .Where(nameof(StartList.RaceId), raceId)
                    .Where(nameof(StartList.RunNumber), runNumber)
                    .AsDelete();

                await this.ExecuteAsync(query);

                int startNumber = 1;
                foreach (var entry in list)
                {
                    entry.Id = 0;
                    entry.RaceId = raceId;
                    entry.RunNumber = (byte)runNumber;
                    entry.StartNumber = startNumber;

                    await this.CreateAsync(entry);
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

        public override async Task<bool> RemoveAsync(int raceId)
        {
            Query query = this.Query().Where(nameof(StartList.RaceId), raceId).AsDelete();
            await this.ExecuteAsync(query);
            return true;
        }
    }
}
