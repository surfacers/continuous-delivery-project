using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Hurace.Api.Dtos;
using Hurace.Core.Enums;
using Hurace.Core.Logic;
using Hurace.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hurace.Api.Controllers
{
    [Route("api/race")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class RaceController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IRaceLogic raceLogic;

        public RaceController(
            IMapper mapper,
            IRaceLogic raceLogic)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.raceLogic = raceLogic ?? throw new ArgumentNullException(nameof(raceLogic));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RaceDto>>> GetAll()
        {
            IEnumerable<Race> races = await this.raceLogic.GetAllAsync();
            return this.Ok(this.mapper.Map<IEnumerable<RaceDto>>(races));
        }

        [HttpGet]
        [Route("state/{state}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<RaceDto>>> GetAll(RaceState state)
        {
            IEnumerable<Race> races = await this.raceLogic.GetByRaceStateAsync(state);
            return this.Ok(this.mapper.Map<IEnumerable<RaceDto>>(races));
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RaceDto>> GetById(int id)
        {
            Race race = await this.raceLogic.GetByIdAsync(id);
            return this.Ok(this.mapper.Map<RaceDto>(race));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type=typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type=typeof(IEnumerable<(string PropertyName, string ErrorCode)>))]
        public async Task<ActionResult> SaveRace([FromBody] RaceDto race)
        {
            Race dbRace = this.mapper.Map<Race>(race);
            var result = await this.raceLogic.SaveAsync(dbRace);

            return result.Match(
                success => this.StatusCode(200, success.Id),
                validationError => this.StatusCode(400, validationError.Errors),
                error => this.StatusCode(500, error.ErrorCode));
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Remove([FromBody] RaceDto race)
        {
            Race dbRace = this.mapper.Map<Race>(race);
            return (await this.raceLogic.RemoveAsync(dbRace)) 
                ? this.StatusCode(200) 
                : this.StatusCode(400);
        }
    }
}