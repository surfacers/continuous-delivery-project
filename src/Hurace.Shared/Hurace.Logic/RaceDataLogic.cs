using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Hurace.Core.Models;
using Hurace.Core.Validators;
using Hurace.Data;

namespace Hurace.Core.Logic
{
    public class RaceDataLogic : IRaceDataLogic
    {
        private readonly IRaceDataManager raceDataManager;
        private readonly RaceDataValidator validator;

        public RaceDataLogic(IRaceDataManager raceDataManager, RaceDataValidator validator)
        {
            this.validator = validator ?? throw new ArgumentNullException(nameof(validator));
            this.raceDataManager = raceDataManager ?? throw new ArgumentNullException(nameof(raceDataManager));
        }

        public async Task<bool> CreateAsync(IEnumerable<RaceData> raceData)
        {
            foreach (var data in raceData)
            {
                var result = this.validator.Validate(data);
                if (!result.IsValid)
                {
                    return false;
                }
            }

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                foreach (var data in raceData)
                {
                    await this.raceDataManager.CreateAsync(data);
                }

                transaction.Complete();
            }

            return true;
        }

        public async Task<IEnumerable<RaceData>> GetByRaceIdAsync(int raceId, int runNumber)
        {
            var data = await this.raceDataManager.GetByRaceIdAsync(raceId, runNumber);
            return data
                .OrderBy(m => m.StartListId)
                .ThenBy(m => m.SensorId)
                .ToList();
        }
    }
}
