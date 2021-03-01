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
    [Route("api/season")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class SeasonController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ISeasonLogic seasonLogic;

        public SeasonController(
            IMapper mapper,
            ISeasonLogic seasonLogic)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.seasonLogic = seasonLogic ?? throw new ArgumentNullException(nameof(seasonLogic));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SeasonDto>>> GetAll()
        {
            IEnumerable<Season> seasons = await this.seasonLogic.GetAllAsync();
            return this.Ok(this.mapper.Map<IEnumerable<SeasonDto>>(seasons));
        }
    }
}
