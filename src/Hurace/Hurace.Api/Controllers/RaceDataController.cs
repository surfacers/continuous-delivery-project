using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Hurace.Api.Dtos;
using Hurace.Core.Logic;
using Hurace.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hurace.Api.Controllers
{
    [Route("api/racedata")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class RaceDataController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IRaceDataLogic raceDataLogic;

        public RaceDataController(
            IMapper mapper,
            IRaceDataLogic raceDataLogic)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.raceDataLogic = raceDataLogic ?? throw new ArgumentNullException(nameof(raceDataLogic));
        }

        [HttpGet]
        [Route("{raceId}/run/{runNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<RaceDataDto>>> GetRaceData(int raceId, int runNumber)
        {
            IEnumerable<RaceData> raceData = await this.raceDataLogic.GetByRaceIdAsync(raceId, runNumber);
            return Ok(this.mapper.Map<IEnumerable<RaceDataDto>>(raceData));
        }
    }
}