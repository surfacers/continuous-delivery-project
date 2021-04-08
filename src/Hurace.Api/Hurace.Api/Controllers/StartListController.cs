using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Hurace.Api.Dtos;
using Hurace.Core.Logic;
using Hurace.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hurace.Api.Controllers
{
    [Route("api/startlist")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class StartListController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IStartListLogic startListLogic;

        public StartListController(
            IMapper mapper,
            IStartListLogic startListLogic)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.startListLogic = startListLogic ?? throw new ArgumentNullException(nameof(startListLogic));
        }

        [HttpGet]
        [Route("{raceId}/run/{runNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<StartListDto>>> GetStartList(int raceId, int runNumber)
        {
            IEnumerable<StartList> startLists = await this.startListLogic.GetByRaceIdAsync(raceId, runNumber);
            return this.Ok(this.mapper.Map<IEnumerable<StartListDto>>(startLists));
        }

        [HttpPost]
        [Route("{raceId}/run/{runNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<(string PropertyName, string ErrorCode)>))]
        public async Task<ActionResult> Save(int raceId, int runNumber, [FromBody] IEnumerable<StartListDto> startList)
        {
            List<StartList> dbStartList = new List<StartList>();
            foreach (var startListItem in startList)
            {
                StartList dbStartListItem = this.mapper.Map<StartList>(startListItem);
                dbStartList.Add(this.mapper.Map<StartList>(startListItem));
            }

            var result = await this.startListLogic.SaveAsync(raceId, runNumber, dbStartList);

            return result.Match(
                success => this.StatusCode(200, success.Id),
                validationError => this.StatusCode(400, validationError.Errors),
                error => this.StatusCode(500, error.ErrorCode));
        }

        [HttpPost]
        [Route("generate/{raceId}/run/{runNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<StartListDto>>> GenerateStartList(int raceId, int runNumber)
        {
            IEnumerable<StartList> startLists = await this.startListLogic.GenerateStartListForRunAsync(raceId, runNumber);
            return this.Ok(this.mapper.Map<IEnumerable<StartListDto>>(startLists));
        }

        [HttpPut]
        [Route("{id}/run/{isDisqualified}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateDisqualified(int id, bool isDisqualified)
        {
            return (await this.startListLogic.UpdateDisqualified(id, isDisqualified))
                ? this.StatusCode(200)
                : this.StatusCode(400);
        }
    }
}