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
    [Route("api/location")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class LocationController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ILocationLogic locationLogic;

        public LocationController(
            IMapper mapper,
            ILocationLogic locationLogic)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.locationLogic = locationLogic ?? throw new ArgumentNullException(nameof(locationLogic));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<LocationDto>>> GetLocations()
        {
            IEnumerable<Location> locations = await this.locationLogic.GetAllAsync();
            return this.Ok(this.mapper.Map<IEnumerable<LocationDto>>(locations));
        }

        [HttpGet]
        [Route("country")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<string>>> GetCountries()
        {
            IEnumerable<string> countries = await this.locationLogic.GetCountriesAsync();
            return this.Ok(countries);
        }
    }
}