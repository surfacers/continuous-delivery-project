using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Hurace.Core.Logic.Models;
using Hurace.Core.Models;
using Hurace.Data;

namespace Hurace.Core.Logic
{
    public class StartListLogic : IStartListLogic
    {
        private readonly IStartListManager startListManager;
        private readonly IRaceDataManager raceDataManager;

        public StartListLogic(
            IStartListManager startListManager,
            IRaceDataManager raceDataManager)
        {
            this.startListManager = startListManager ?? throw new ArgumentNullException(nameof(startListManager));
            this.raceDataManager = raceDataManager ?? throw new ArgumentNullException(nameof(raceDataManager));
        }

        public async Task<SaveResult> SaveAsync(int raceId, int runNumber, IEnumerable<StartList> startList)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                bool successful = await this.startListManager.SaveAsync(raceId, runNumber, startList);
                if (!successful)
                {
                    return new SaveResult.Error(ErrorCode.SaveError);
                }

                transaction.Complete();
            }

            return new SaveResult.Success(raceId);
        }

        public async Task<IEnumerable<StartList>> GenerateStartListForRunAsync(int raceId, int runNumber)
        {
            var previousStartList = await this.GetByRaceIdAsync(raceId, runNumber - 1);
            var raceData = await this.raceDataManager.GetByRaceIdAsync(raceId, runNumber - 1);

            var startList = previousStartList
                .Where(s => !s.IsDisqualified)
                .TakeWhile((s, i) => s != null && i < Settings.DefaultSettings.MinSkierAmount)
                .Select(s => new
                {
                    SkierId = s.SkierId,
                    Start = raceData.Where(r => r.StartListId == s.Id).Min(r => r.TimeStamp),
                    End = raceData.Where(r => r.StartListId == s.Id).Max(r => r.TimeStamp),
                })
                .OrderByDescending(s => (s.End - s.Start).TotalMilliseconds)
                .Select((s, i) => new StartList
                {
                    Id = 0,
                    RaceId = raceId,
                    RunNumber = (byte)runNumber,
                    IsDisqualified = false,
                    SkierId = s.SkierId,
                    StartNumber = i + 1
                })
                .ToList();

            await this.startListManager.SaveAsync(raceId, runNumber, startList);
            return startList;
        }

        public async Task<IEnumerable<StartList>> GetByRaceIdAsync(int raceId, int runNumber)
        {
            return await this.startListManager.GetByRaceIdAsync(raceId, runNumber);
        }

        public async Task<bool> UpdateDisqualified(int id, bool isDisqualified)
        {
            return await this.startListManager.UpdateDisqualified(id, isDisqualified);
        }
    }
}
