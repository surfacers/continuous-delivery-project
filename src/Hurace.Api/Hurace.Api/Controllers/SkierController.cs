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
    [Route("api/skier")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class SkierController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ISkierLogic skierLogic;

        public SkierController(
            IMapper mapper,
            ISkierLogic skierLogic)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.skierLogic = skierLogic ?? throw new ArgumentNullException(nameof(skierLogic));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SkierDto>>> GetAll()
        {
            IEnumerable<Skier> skiers = await this.skierLogic.GetAllAsync();
            return this.Ok(this.mapper.Map<IEnumerable<SkierDto>>(skiers));
        }

        [HttpGet]
        [Route("{gender}/active/{isActive}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<SkierDto>>> GetAllBy(Gender gender, bool isActive = true)
        {
            IEnumerable<Skier> skiers = await this.skierLogic.GetAllAsync(gender, isActive);
            return this.Ok(this.mapper.Map<IEnumerable<SkierDto>>(skiers));
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<SkierDto>>> GetById(int id)
        {
            Skier skier = await this.skierLogic.GetByIdAsync(id);
            return this.Ok(this.mapper.Map<SkierDto>(skier));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<(string PropertyName, string ErrorCode)>))]
        public async Task<ActionResult> Save([FromBody] SkierDto skier)
        {
            var dbSkier = this.mapper.Map<Skier>(skier);
            var result = await this.skierLogic.SaveAsync(dbSkier);

            return result.Match(
                success => this.StatusCode(200, success.Id),
                validationError => this.StatusCode(400, validationError.Errors),
                error => this.StatusCode(500, error.ErrorCode));
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Remove(int id)
        {
            return (await this.skierLogic.RemoveAsync(id)) 
                ? this.StatusCode(200) 
                : this.StatusCode(400);
        }
    }
}