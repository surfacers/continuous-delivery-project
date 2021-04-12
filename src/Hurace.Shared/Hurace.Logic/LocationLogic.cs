using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Logic;
using Hurace.Core.Models;
using Hurace.Data;

namespace Hurace.Logic
{
    public class LocationLogic : ILocationLogic
    {
        private readonly ILocationManager locationManager;

        public LocationLogic(
            ILocationManager locationManager)
        {
            this.locationManager = locationManager ?? throw new ArgumentNullException(nameof(locationManager));
        }

        public async Task<IEnumerable<Location>> GetAllAsync()
            => await this.locationManager.GetAllAsync();

        public Task<IEnumerable<string>> GetCountriesAsync()
        {
            IEnumerable<string> countries = new[] { "AUT", "USA", "FRA", "GER", "SUI" }.OrderBy(s => s);
            return Task.FromResult(countries);
        }
    }
}
