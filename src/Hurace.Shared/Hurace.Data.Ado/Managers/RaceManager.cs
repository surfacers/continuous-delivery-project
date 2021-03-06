﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Hurace.Core.Enums;
using Hurace.Core.Models;
using Hurace.Data.Ado.Queries;
using SqlKata.Compilers;

namespace Hurace.Data.Ado.Managers
{
    public class RaceManager : GenericManager<Race>, IRaceManager
    {
        public RaceManager(IMapper mapper, AdoManager ado, Compiler compiler)
            : base(mapper, ado, compiler)
        {
        }

        public Task<IEnumerable<Race>> GetByRaceStateAsync(RaceState raceState)
        {
            var query = this.Query()
                .Where(nameof(Race.RaceState), raceState);

            return this.QueryAsync(query);
        }

        public async Task<bool> UpdateRaceState(int id, RaceState raceState)
        {
            var query = this.Query(id).AsUpdate(nameof(Race.RaceState), raceState);
            return (await this.ExecuteAsync(query)) == 1;
        }
    }
}
