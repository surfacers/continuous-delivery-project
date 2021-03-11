using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Hurace.Api.Dtos;
using Hurace.Core.Logic;
using Hurace.Core.Logic.Models;
using Hurace.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hurace.Api.Controllers
{
    [Route("api/statistic")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class StatisticController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IRaceLogic raceLogic;
        private readonly IStatisticLogic statisticLogic;

        public StatisticController(
            IMapper mapper,
            IRaceLogic raceLogic,
            IStatisticLogic statisticLogic)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.raceLogic = raceLogic ?? throw new ArgumentNullException(nameof(raceLogic));
            this.statisticLogic = statisticLogic ?? throw new ArgumentNullException(nameof(statisticLogic));
        }

        [HttpGet]
        [Route("{raceId}/run/{runNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<RaceStatisticEntryDto>>> GetRaceStatistics(int raceId, int runNumber)
        {
            Race race = await this.raceLogic.GetByIdAsync(raceId);
            IEnumerable<RaceStatisticEntry> raceResults = await this.statisticLogic.GetRaceStatistics(raceId, runNumber, race.SensorAmount);
            return Ok(this.mapper.Map<IEnumerable<RaceStatisticEntryDto>>(raceResults));
        }
    }
}