using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Hurace.Core.Enums;
using Hurace.Core.Logic.Models;
using Hurace.Core.Models;
using Hurace.Core.Validators;
using Hurace.Data;
using Hurace.Data.Ado;

namespace Hurace.Core.Logic
{
    public class RaceLogic : IRaceLogic
    {
        private readonly IRaceManager raceManager;
        private readonly IStartListManager startListManager;

        private readonly RaceValidator validator;

        public RaceLogic(
            IRaceManager raceManager,
            IStartListManager startListManager,
            RaceValidator validator)
        {
            this.raceManager = raceManager ?? throw new ArgumentNullException(nameof(raceManager));
            this.startListManager = startListManager ?? throw new ArgumentNullException(nameof(startListManager));

            this.validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task<IEnumerable<Race>> GetAllAsync()
        {
            var races = await raceManager.GetAllAsync();
            return races.OrderByDescending(r => r.RaceDate).ThenBy(r => r.RaceState);
        }

        public async Task<IEnumerable<Race>> GetByRaceStateAsync(RaceState raceState)
        {
            var races = await raceManager.GetByRaceStateAsync(raceState);
            return races.OrderBy(r => r.RaceDate);
        }

        public async Task<Race> GetByIdAsync(int id)
        {
            return await raceManager.GetByIdAsync(id);
        }

        public bool CanRemove(Race race)
        {
            return race.Id == 0
                || race.RaceState == RaceState.NotStarted;
        }

        public async Task<bool> RemoveAsync(Race race)
        {
            if (race.Id == 0)
            {
                return true;
            }

            if (race.RaceState == RaceState.NotStarted)
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    await startListManager.RemoveAsync(race.Id);
                    await raceManager.RemoveAsync(race.Id);

                    transaction.Complete();
                    return true;
                }
            }
            
            return false;
        }

        public async Task<SaveResult> SaveAsync(Race race)
        {
            var result = this.validator.Validate(race);
            if (!result.IsValid)
            {
                return new SaveResult.ValidationError(result.Errors);
            }

            if (race.Id == 0)
            {
                await raceManager.CreateAsync(race);
            }
            else
            {
                bool updateSuccess = await raceManager.UpdateAsync(race);
                if (!updateSuccess)
                {
                    return new SaveResult.Error(ErrorCode.UpdateError);
                }
            }

            return new SaveResult.Success(race.Id);

        }

        public async Task<SaveResult> SaveAsync(Race race, int runNumber, IEnumerable<StartList> startList)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var result = await SaveAsync(race);
                if (result.IsError)
                {
                    return result;
                }

                bool successful = await startListManager.SaveAsync(race.Id, runNumber, startList);
                if (!successful)
                {
                    return new SaveResult.Error(ErrorCode.SaveError);
                }

                transaction.Complete();
            }

            return new SaveResult.Success(race.Id);
        }

        public async Task<bool> StartAsync(int raceId)
        {
            return await raceManager.UpdateRaceState(raceId, RaceState.Running);
        }
    }
}
